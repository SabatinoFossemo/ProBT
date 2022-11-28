namespace ProBT
{
    public class Position
    {
        protected internal int id;
        protected internal string name;
        protected internal ORDER_TYPE type;
        protected internal DateTime entry_date;
        protected internal double entry_price;
        protected internal double sl = 0;
        protected internal double tp = 0;
        protected internal decimal mae = 0;
        protected internal decimal mfe = 0;
        protected internal decimal profit = 0;

        public Position(int id, Order order, DateTime entry_date, double entry_price)
        {
            this.id = id;
            this.name = order.Name;
            this.type = order.Type;
            this.entry_date = entry_date;
            this.entry_price = entry_price;
            this.sl = StopLossCalcPrice(order.StopLoss);
            this.tp = TakeProfitCalcPrice(order.TakeProfit);
        }

        public Position(Position position)
        {
            this.id = position.ID;
            this.name = position.Name;
            this.type = position.Type;
            this.entry_date = position.EntryDate;
            this.entry_price = position.EntryPrice;
            this.sl = position.StopLoss;
            this.tp = position.TakeProfit;
            this.mae = position.MAE;
            this.mfe = position.MFE;
            this.profit = position.Profit;
        }

        public Position()
        {
            this.id = -1;
            this.name = "";
            this.type = ORDER_TYPE.NULL;
            this.entry_date = Convert.ToDateTime("01-01-0001");
            this.entry_price = 0;
        }



        internal int ID {get => this.id;}
        internal string Name {get => this.name;}
        internal ORDER_TYPE Type {get => this.type;}
        internal DateTime EntryDate {get => this.entry_date;}
        internal double EntryPrice {get => this.entry_price;}
        internal double StopLoss {get => this.sl;}
        internal double TakeProfit {get => this.tp;}
        internal decimal MAE {get => this.mae;}
        internal decimal MFE {get => this.mfe;}
        internal decimal Profit {get => this.profit;}

        private double StopLossCalcPrice(double value)
        {
            if(value <= 0) return 0;

            double result = 0;

            if(this.Type == ORDER_TYPE.BUY)
                result = this.EntryPrice - value;
            if(this.Type == ORDER_TYPE.SELLSHORT)
                result= this.EntryPrice + value;

            return result;
        }

        private double TakeProfitCalcPrice(double value)
        {
            if(value <= 0) return 0;

            double result = 0;

            if(this.Type == ORDER_TYPE.BUY)
                result = this.EntryPrice + value;
            if(this.Type == ORDER_TYPE.SELLSHORT)
                result= this.EntryPrice - value;

            return result;
        }




        public override string ToString()
        {
            if(this.Type == ORDER_TYPE.NULL)
                return "POS: FLAT";
            else
                return $"POS: {id}  - {name} - {type} - {entry_date} - {entry_price.ToString("F2")} - {sl.ToString("F2")} - {tp.ToString("F2")} - {mae.ToString("F2")} + - {mfe.ToString("F2")} - {profit.ToString("F2")}";
        }

        internal Position Update(double h, double l, double c, decimal bpv)
        {            
            decimal profit = (decimal)(c - this.entry_price) * bpv;
            decimal h_ep = (decimal)(h - this.entry_price) * bpv;
            decimal l_ep = (decimal)(l - this.entry_price) * bpv;

            decimal mfe = h_ep;
            decimal mae = l_ep;

            if(this.type == ORDER_TYPE.SELLSHORT)
            {
                profit = -profit;
                mfe = -l_ep;
                mae = -h_ep;
            }
            this.profit = profit;
            this.mfe = (decimal)Math.Max(mfe, Convert.ToDecimal(this.mfe));
            this.mae = (decimal)Math.Min(mae, Convert.ToDecimal(this.mae));

            return this;
        }

        internal Trade Close(DateTime exit_date, double exit_price, string exit_reason)
        {
            Trade trade = new Trade(this, exit_date, exit_price, exit_reason);
            return trade;
        }

        internal Position Reset()
        {
            return new Position();
        }

    }
}