namespace ProBT
{
    public class Position
    {
        protected int id;
        protected string name;
        protected ORDER_TYPE type;
        protected DateTime entry_date;
        protected double entry_price;
        double sl = 0;
        double tp = 0;
        protected double mae = 0;
        protected double mfe = 0;
        protected double profit = 0;

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



        public int ID {get => this.id;}
        public string Name {get => this.name;}
        public ORDER_TYPE Type {get => this.type;}
        public DateTime EntryDate {get => this.entry_date;}
        public double EntryPrice {get => this.entry_price;}
        public double StopLoss {get => this.sl;}
        public double TakeProfit {get => this.tp;}
        public double MAE {get => this.mae;}
        public double MFE {get => this.mfe;}
        public double Profit {get => this.profit;}

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
                return "POS: " + 
                id + " - " + 
                name + " - " + 
                type + " - " +
                entry_date + " - " + 
                entry_price.ToString("F2") + " - " + 
                sl.ToString("F2") + " - " + 
                tp.ToString("F2") + " - " + 
                mae.ToString("F2") + " - " + 
                mfe.ToString("F2") + " - " + 
                profit.ToString("F2");
        }

        public Position Update(double h, double l, double c, double bpv)
        {            
            double profit = (c - this.entry_price) * bpv;
            double h_ep = (h - this.entry_price) * bpv;
            double l_ep = (l - this.entry_price) * bpv;

            double mfe = h_ep;
            double mae = l_ep;

            if(this.type == ORDER_TYPE.SELLSHORT)
            {
                profit = -profit;
                mfe = -l_ep;
                mae = -h_ep;
            }
            this.profit = profit;
            this.mfe = Math.Max(mfe, Convert.ToDouble(this.mfe));
            this.mae = Math.Min(mae, Convert.ToDouble(this.mae));

            return this;
        }

        public Trade Close(DateTime exit_date, double exit_price, string exit_reason)
        {
            Trade trade = new Trade(this, exit_date, exit_price, exit_reason);
            return trade;
        }

        public Position Reset()
        {
            return new Position();
        }

    }
}