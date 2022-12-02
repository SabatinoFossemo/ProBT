
namespace ProBT
{
    public class Quote: IQuote
    {
        public List<DateTime> Date {get; }
        public List<double> Open {get; }
        public List<double> High {get; }
        public List<double> Low {get; }
        public List<double> Close {get; }

        public string Symbol {get; }
        public string Category {get; }
        public string Sector {get; }
        public double TickSize {get; }
        public decimal BigPointValue {get; }


        public Quote(string file, string symbol="NONE", string category="NONE", string sector="NONE", decimal bigpointvalue=1, double ticksize=1)
        {
            Date = new List<DateTime>();
            Open = new List<double>();
            High = new List<double>();
            Low = new List<double>();
            Close = new List<double>();

            var rows = ProcessCSV(file);

            foreach (var row in rows)
            {
                this.Date.Add(row.date);
                this.Open.Add(row.open);
                this.High.Add(row.high);
                this.Low.Add(row.low);
                this.Close.Add(row.close);
            }
            
            this.Symbol = symbol;
            this.Category = category;
            this.Sector = sector;
            this.BigPointValue = bigpointvalue;
            this.TickSize = ticksize;
        }


        public Quote(Quote in_quote, List<DateTime> D, List<double> O, List<double> H, List<double> L, List<double> C)
        {
            this.Date = D;
            this.Open = O;
            this.High = H;
            this.Low = L;
            this.Close = C;
            this.Symbol = in_quote.Symbol;
            this.Category = in_quote.Category;
            this.Sector = in_quote.Sector;
            this.BigPointValue = in_quote.BigPointValue;
            this.TickSize = in_quote.TickSize;
        }

        public void print_bar(int i)
        {
            Console.WriteLine($"{Date[i]} - {Open[i]} - {High[i]} - {Low[i]} - {Close[i]}");
        }

        private List<Bar> ProcessCSV(string path)
        {
            return File.ReadAllLines(path)
                .Skip(1)
                .Where(row => row.Length > 0)
                .Select(Bar.ParseRow).ToList();
        }

        public void PrintInfo()
        {
            Console.WriteLine("*  QUOTE INFO  *");
            Console.WriteLine("Symbol         : {0}", Symbol);
            Console.WriteLine("Sector         : {0}", Sector);
            Console.WriteLine("Category       : {0}", Category);
            Console.WriteLine("BigPointValue  : {0:0.00}", BigPointValue);
            Console.WriteLine("TickSize       : {0:0.00}", TickSize);
            Console.WriteLine("TotalBars      : {0:0.00}", Date.Count);
            Console.WriteLine("DateFrom       : {0}", Stat["DateFrom"]);
            Console.WriteLine("DateTo         : {0}", Stat["DateTo"]);
            Console.WriteLine("Omin           : {0}", Stat["OpenMin"]);
            Console.WriteLine("Omax           : {0}", Stat["OpenMax"]);
            Console.WriteLine("Hmin           : {0}", Stat["HighMin"]);
            Console.WriteLine("Hmax           : {0}", Stat["HighMax"]);
            Console.WriteLine("Lmin           : {0}", Stat["LowMin"]);
            Console.WriteLine("Lmax           : {0}", Stat["LowMax"]);
            Console.WriteLine("Cmin           : {0}", Stat["CloseMin"]);
            Console.WriteLine("Cmax           : {0}", Stat["CloseMax"]);
            Console.WriteLine("*--  samples  -------------*\n");
            Console.WriteLine("               date   open   high    low  close");
            for (int i = 0; i < 5; i++)
                Console.WriteLine("{0}   {1}   {2}   {3}   {4}",Date[i], Open[i], High[i], Low[i], Close[i]);
            Console.WriteLine("*--------------------------*");
        }

        private Dictionary<string, object> Stat
        {
            get{
                Dictionary<string, object> result = new Dictionary<string, object>();
                result.Add("DateFrom", Date.Min());
                result.Add("DateTo", Date.Max());
                result.Add("OpenMin", Open.Min());
                result.Add("OpenMax", Open.Max());
                result.Add("HighMin", High.Min());
                result.Add("HighMax", High.Max());
                result.Add("LowMin", Low.Min());
                result.Add("LowMax", Low.Max());
                result.Add("CloseMin", Close.Min());
                result.Add("CloseMax", Close.Max());
                
                return result;
            }
        }

        public void AdjValueBelow0()
        {
            double min_val = Low.Min() - 1.0;

            if(min_val<0)
                min_val *= -1;
                for (int i = 0; i < Date.Count; i++)
                {
                    Open[i] = (Open[i] + min_val).RoundTicks(this.TickSize);
                    High[i] = (High[i] + min_val).RoundTicks(this.TickSize);
                    Low[i] = (Low[i] + min_val).RoundTicks(this.TickSize);
                    Close[i] = (Close[i] + min_val).RoundTicks(this.TickSize);
                }
        }
    }

    internal class Bar
    {
        internal DateTime date { get; set; }
        internal double open { get; set; }
        internal double high { get; set; }
        internal double low { get; set; }
        internal double close { get; set; }

        internal static Bar ParseRow(string row)
        {
            var columns = row.Split(',');

            return new Bar()
            {
                date  = DateTime.Parse(columns[0]),
                open  = double.Parse(columns[1], System.Globalization.CultureInfo.InvariantCulture),
                high  = double.Parse(columns[2], System.Globalization.CultureInfo.InvariantCulture),
                low   = double.Parse(columns[3], System.Globalization.CultureInfo.InvariantCulture),
                close = double.Parse(columns[4], System.Globalization.CultureInfo.InvariantCulture)
            };
        }
    }


}

