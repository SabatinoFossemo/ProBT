namespace ProBT
{
    public class Position
    {
        // PROPERTIES ................................................................................................
        public int ID { get; }
        public string Name { get; }
        public ORDER_TYPE Type { get; }
        public DateTime EntryDate { get; }
        public double EntryPrice { get; }
        public double StopLoss { get; } = 0;
        public double TakeProfit { get; } =0;
        public decimal MAE { get; set; }
        public decimal MFE { get; set; }
        public decimal Profit { get; set; }


        // CONSTRUCTORS ................................................................................................
        public Position(int id, IOrder order, DateTime entry_date, double entry_price){
            this.ID = id;
            this.Name = order.Name;
            this.Type = order.Type;
            this.EntryDate = entry_date;
            this.EntryPrice = entry_price;
            this.StopLoss = StopLossCalcPrice(order.StopLoss);
            this.TakeProfit = TakeProfitCalcPrice(order.TakeProfit);
        }

        public Position(Position position){
            this.ID = position.ID;
            this.Name = position.Name;
            this.Type = position.Type;
            this.EntryDate = position.EntryDate;
            this.EntryPrice = position.EntryPrice;
            this.StopLoss = position.StopLoss;
            this.TakeProfit = position.TakeProfit;
            this.MAE = position.MAE;
            this.MFE = position.MFE;
            this.Profit = position.Profit;
        }

        public Position(){
            this.ID = -1;
            this.Name = "";
            this.Type = ORDER_TYPE.NULL;
            this.EntryDate = Convert.ToDateTime("01-01-0001");
            this.EntryPrice = 0;
        }

        // PUBLIC METHODS ................................................................................................
        public Position Update(double h, double l, double c, decimal bpv){            
            decimal profit = (decimal)(c - this.EntryPrice) * bpv;
            decimal h_ep = (decimal)(h - this.EntryPrice) * bpv;
            decimal l_ep = (decimal)(l - this.EntryPrice) * bpv;

            decimal mfe = h_ep;
            decimal mae = l_ep;

            if(this.Type == ORDER_TYPE.SELLSHORT){
                profit = -profit;
                mfe = -l_ep;
                mae = -h_ep;
            }

            this.Profit = profit;
            this.MFE = (decimal)Math.Max(mfe, Convert.ToDecimal(this.MFE));
            this.MAE = (decimal)Math.Min(mae, Convert.ToDecimal(this.MAE));

            return this;
        }

        public Trade Close(DateTime exit_date, double exit_price, string exit_reason){
            Trade trade = new Trade(this, exit_date, exit_price, exit_reason);
            return trade;
        }

        public Position Reset(){
            return new Position();
        }

        // PRIVATE METHODS ................................................................................................
        private double StopLossCalcPrice(double value){
            if(value <= 0) return 0;

            double result = 0;

            if(this.Type == ORDER_TYPE.BUY)
                result = this.EntryPrice - value;
            if(this.Type == ORDER_TYPE.SELLSHORT)
                result= this.EntryPrice + value;

            return result;
        }

        private double TakeProfitCalcPrice(double value){
            if(value <= 0) return 0;

            double result = 0;

            if(this.Type == ORDER_TYPE.BUY)
                result = this.EntryPrice + value;
            if(this.Type == ORDER_TYPE.SELLSHORT)
                result= this.EntryPrice - value;

            return result;
        }

        // OVERRIDE METHODS ................................................................................................
        public override string ToString(){
            if(this.Type == ORDER_TYPE.NULL)
                return "POS: FLAT";
            else
                return $"POS: {this.ID}  - {this.Name} - {this.Type} - {this.EntryDate} - {this.EntryPrice.ToString()} - {this.StopLoss.ToString("F2")} - {this.TakeProfit.ToString("F2")} - {this.MAE.ToString("F2")} + - {this.MFE.ToString("F2")} - {this.Profit.ToString("F2")}";
        }



    }
}