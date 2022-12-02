namespace ProBT
{
    public class Trade : Position
    {
        // PROPERTIES ................................................................................................

        public DateTime ExitDate { get; }
        public double ExitPrice { get; }
        public string ExitReason { get; }

        // CONSTRUCTORS ................................................................................................
        public Trade(Position pos, DateTime exit_date, double exit_price, string exit_reason) : base(pos)
        {
            this.ExitDate = exit_date;
            this.ExitPrice = exit_price;
            this.ExitReason = exit_reason;
        }

        // OVERRIDE METHODS ................................................................................................
        public override string ToString()
        {
            int cw = 10;
            int cw1 = 20;
            
            string result =  "TRD: " +
            ID.ToString().PadLeft(5, ' ') + " |" + 
            Name.PadLeft(cw, ' ') + " |" + 
            Type.ToString().PadLeft(cw, ' ') + " |" +
            EntryDate.ToString().PadLeft(cw1, ' ') + " |" + 
            EntryPrice.ToString().PadLeft(cw, ' ') + " |" + 
            ExitDate.ToString().PadLeft(cw1, ' ') + " |" + 
            ExitPrice.ToString().PadLeft(cw, ' ') + " |" + 
            StopLoss.ToString("F2").PadLeft(cw, ' ') + " |" + 
            TakeProfit.ToString("F2").PadLeft(cw, ' ') + " |" + 
            ExitReason.PadLeft(cw, ' ') + " |" + 
            MAE.ToString("F2").PadLeft(cw, ' ') + " |" + 
            MFE.ToString("F2").PadLeft(cw, ' ') + " |" + 
            Profit.ToString("F2").PadLeft(cw, ' ') + " |";

            return result;
        }
    }
}