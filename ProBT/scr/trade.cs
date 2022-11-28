namespace ProBT
{
    public class Trade : Position
    {
        DateTime exit_date;
        double exit_price;
        string exit_reason;

        public Trade(Position pos, DateTime exit_date, double exit_price, string exit_reason) : 
        base(pos)
        {
            this.exit_date = exit_date;
            this.exit_price = exit_price;
            this.exit_reason = exit_reason;
        }

        internal DateTime ExitDate {get => this.exit_date;}
        internal double ExitPrice {get => this.exit_price;}
        internal string ExitReason {get => this.exit_reason;}

        public override string ToString()
        {
            int cw = 10;
            int cw1 = 20;
            
            string result =  "TRD: " +
            ID.ToString().PadLeft(5, ' ') + " |" + 
            Name.PadLeft(cw, ' ') + " |" + 
            Type.ToString().PadLeft(cw, ' ') + " |" +
            EntryDate.ToString().PadLeft(cw1, ' ') + " |" + 
            EntryPrice.ToString("F2").PadLeft(cw, ' ') + " |" + 
            ExitDate.ToString().PadLeft(cw1, ' ') + " |" + 
            ExitPrice.ToString("F2").PadLeft(cw, ' ') + " |" + 
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