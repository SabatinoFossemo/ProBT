

namespace ProBT
{
    public class Equity
    {
        decimal initial_balance;
        List<Point> point;


        public Equity(decimal initial_balance)
        {
            this.initial_balance = initial_balance;
            this.point = new List<Point>();

        }

        public void AddPoint(Point point)
        {
            this.point.Add(point);
        }

        public List<decimal> Peak
        {
            get{
                List<decimal> result = new List<decimal>();

                foreach (var item in point)
                    result.Add(item.Peak);
                return result;
            }
        }

        public List<decimal> Valley
        {
            get{
                List<decimal> result = new List<decimal>();

                foreach (var item in point)
                    result.Add(item.Valley);
                return result;
            }
        }

        public List<decimal> Close
        {
            get{
                List<decimal> result = new List<decimal>();

                foreach (var item in point)
                    result.Add(item.Close);
                return result;
            }
        }

        public Equity EquityLong 
        {
            get{
                Equity result = new Equity(this.initial_balance);

                foreach (var item in point)
                    if( item.Type == ORDER_TYPE.BUY)
                            result.AddPoint(item);
                return result;
            }
        }
        public Equity EquityShort 
        {
            get{
                Equity result = new Equity(this.initial_balance);

                foreach (var item in point)
                    if( item.Type == ORDER_TYPE.SELLSHORT)
                            result.AddPoint(item);
                return result;
            }
        }
        




        public List<decimal> DrawDown
        {
            get{
                List<decimal> result = new List<decimal>();

                decimal cum_max = 0;

                foreach (var item in this.point)
                {
                    cum_max = Math.Max(cum_max, item.Peak);
                    result.Add(item.Valley - cum_max);
                }

                return result;
            }
        }

        
        public MyEnumerator GetEnumerator()
        {  
            return new MyEnumerator(this);  
        }  


        // Declare the enumerator class:  
        public class MyEnumerator
        {  
            int nIndex;  
            Equity collection;  
            public MyEnumerator(Equity coll)
            {  
                collection = coll;  
                nIndex = -1;  
            }  
    
            public bool MoveNext()
            {  
                nIndex++;  
                return (nIndex < collection.point.Count());  
            }  
  
            public Point Current => collection.point[nIndex];
        }  



    }

    public class Point
    {
        int id;
        ORDER_TYPE type;
        DateTime date;
        decimal peak;
        decimal valley;
        decimal close;

        public Point(int id, ORDER_TYPE type, DateTime date, decimal peak, decimal valley, decimal close)
        {
            this.id = id;
            this.type = type;
            this.date = date;
            this.peak = peak;
            this.valley = valley;
            this.close = close;
        }

        public int ID  {get => this.id;}
        public ORDER_TYPE Type  {get => this.type;}
        public DateTime Date  {get => this.date;}
        public decimal Peak  {get => this.peak;}
        public decimal Valley  {get => this.valley;}
        public decimal Close  {get => this.close;}

        public override string ToString()
        {
            return $"EQ: - {id} - {date} - {peak.ToString("F2")} - {valley.ToString("F2")} - {close.ToString("F2")}";
        }
    } 
}