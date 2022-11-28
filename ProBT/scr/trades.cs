namespace ProBT
{
    public class ListTrades
    {
        List<Trade> trades;

        public ListTrades()
        {
            this.trades = new List<Trade>();
        }

        internal List<Trade> Trades{get => this.trades;}
        internal int NumTrades {get => this.trades.Count();}
        internal int NumTradesLong {get => this.trades.Count(a => a.Type == ORDER_TYPE.BUY);}
        internal int NumTradesShort {get => this.trades.Count(a => a.Type == ORDER_TYPE.SELLSHORT);}


        internal ListTrades TradesLong 
        {
            get{
                ListTrades result = new ListTrades();
                foreach (var item in this.Trades)
                    if(item.Type == ORDER_TYPE.BUY)
                        result.Append(item);
                return result;
            }
        }
        internal ListTrades TradesShort
        {
            get{
                ListTrades result = new ListTrades();
                foreach (var item in this.Trades)
                    if(item.Type == ORDER_TYPE.SELLSHORT)
                        result.Append(item);
                return result;
            }
        }

        internal List<decimal> Profit 
        {
            get{
                List<decimal> result = new List<decimal>();
                foreach (var item in this.Trades)
                    result.Add(item.Profit);
                return result;
            }
        }

        internal List<decimal> Equity
        {
            get{
                List<decimal> result = new List<decimal>();
                result.Add(0);
                for (int i = 1; i < this.NumTrades; i++)
                {
                    result.Add(result[i-1] + this.Profit[i]);
                }

                return result;
            }
        }

        internal List<decimal> DrawDown
        {
            get{

                List<decimal> result = new List<decimal>();
                decimal cum_max = 0;

                foreach (var item in this.Equity)
                {
                    cum_max = Math.Max(cum_max, item);
                    result.Add(item - cum_max);
                }
                return result;
            }
        }

        internal decimal Balance
        {
            get{return this.Profit.Sum();}
        }


        internal void Append(Trade trade)
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
}