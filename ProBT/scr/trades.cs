namespace ProBT
{
    public class ListTrades
    {
        List<Trade> trades;

        public ListTrades()
        {
            this.trades = new List<Trade>();
        }

        public List<Trade> Trades{get => this.trades;}
        public int NumTrades {get => this.trades.Count();}
        public int NumTradesLong {get => this.trades.Count(a => a.Type == ORDER_TYPE.BUY);}
        public int NumTradesShort {get => this.trades.Count(a => a.Type == ORDER_TYPE.SELLSHORT);}


        public ListTrades TradesLong 
        {
            get{
                ListTrades result = new ListTrades();
                foreach (var item in this.Trades)
                    if(item.Type == ORDER_TYPE.BUY)
                        result.Append(item);
                return result;
            }
        }
        public ListTrades TradesShort
        {
            get{
                ListTrades result = new ListTrades();
                foreach (var item in this.Trades)
                    if(item.Type == ORDER_TYPE.SELLSHORT)
                        result.Append(item);
                return result;
            }
        }

        public List<double> Profit 
        {
            get{
                List<double> result = new List<double>();
                foreach (var item in this.Trades)
                    result.Add(item.Profit);
                return result;
            }
        }

        public List<double> Equity
        {
            get{
                List<double> result = new List<double>();
                result.Add(0);
                for (int i = 1; i < this.NumTrades; i++)
                {
                    result.Add(result[i-1] + this.Profit[i]);
                }

                return result;
            }
        }

        public List<double> DrawDown
        {
            get{

                List<double> result = new List<double>();
                double cum_max = 0;

                foreach (var item in this.Equity)
                {
                    cum_max = Math.Max(cum_max, item);
                    result.Add(item - cum_max);
                }
                return result;
            }
        }

        public double Balance
        {
            get{return this.Profit.Sum();}
        }


        public void Append(Trade trade)
        {
            this.trades.Add(trade);
        }


        public override string ToString()
        {
            string result =  
            "TRD: " +
            "   ID |"  + 
            "      Name |" + 
            "      Type |" +
            "               EDate |" + 
            "    EPrice |" + 
            "               XDate |" + 
            "    EPrice |" + 
            "        SL |" +
            "        TP |" + 
            "   XReason |" + 
            "       MAE |" + 
            "       MFE |" + 
            "    Profit |" + "\n";

            
            foreach (var item in trades)
            {
                result+=item.ToString() + "\n";
            }
            return result;
        }

        public MyEnumerator GetEnumerator()
        {  
            return new MyEnumerator(this);  
        }  

        // Declare the enumerator class:  
        public class MyEnumerator
        {  
            int nIndex;  
            ListTrades collection;  
            public MyEnumerator(ListTrades coll)
            {  
                collection = coll;  
                nIndex = -1;  
            }  
    
            public bool MoveNext()
            {  
                nIndex++;  
                return (nIndex < collection.trades.Count());  
            }  
  
            public Trade Current => collection.trades[nIndex];
        }  
    }

//............................................................................................................
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

        public DateTime ExitDate {get => this.exit_date;}
        public double ExitPrice {get => this.exit_price;}
        public string ExitReason {get => this.exit_reason;}

        public override string ToString()
        {
            int cw = 10;
            int cw1 = 20;
            return "TRD: " +
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
        }

        // public string[] ToStringArray
        // {
        //     get{
        //         string[] row = new string[14];
        //         int cw = 10;
        //         row[0] = "TRD: ";
        //         row[1] = ID ;
        //         row[2] = Name.PadLeft(cw, ' '); 
        //         row[3] = Type.ToString().PadLeft(cw, ' ');
        //         row[4] = EntryDate.ToString().PadLeft(cw, ' ');
        //         row[5] = EntryPrice.ToString("F2").PadLeft(cw, ' ');
        //         row[6] = ExitDate.ToString().PadLeft(cw, ' ');
        //         row[7] = ExitPrice.ToString("F2").PadLeft(cw, ' ');
        //         row[8] = StopLoss.ToString("F2").PadLeft(cw, ' ');
        //         row[9] = TakeProfit.ToString("F2").PadLeft(cw, ' ');
        //         row[10] = ExitReason.PadLeft(cw, ' ');
        //         row[11] = MAE.ToString("F2").PadLeft(cw, ' ');
        //         row[12] = MFE.ToString("F2").PadLeft(cw, ' '); 
        //         row[13] = Profit.ToString("F2").PadLeft(cw, ' ');
        //         return row;
        //     }
        // }
    }
}