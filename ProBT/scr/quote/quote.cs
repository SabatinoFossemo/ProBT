//using Microsoft.Data.Analysis;
using System.Text;
using System.Linq;
using System;

//using Microsoft.AspNetCore.Html;

namespace ProBT
{
    public class Quote
    {
        List<DateTime> date;
        List<double> open;
        List<double> high;
        List<double> low;
        List<double> close;

        string symbol { get; set; }
        string category { get; set; }
        string sector { get; set; }
        decimal bigpointvalue { get; set; }
        double ticksize { get; set; }

        internal List<DateTime> Date{get => date;}
        internal List<double> Open{get => open;}
        internal List<double> High{get => high;}
        internal List<double> Low{get => low;}
        internal List<double> Close{get => close;}

        internal string Symbol {get => symbol;}
        internal string Category {get => category;}
        internal string Sector {get => sector;}
        internal double TS {get => ticksize;}
        internal double TickSize {get => ticksize;}
        internal decimal BPV {get => bigpointvalue;}
        internal decimal BigPointValue {get => bigpointvalue;}

        public Quote(string file, string symbol="NONE", string category="NONE", string sector="NONE", decimal bigpointvalue=1, double ticksize=1)
        {
            date = new List<DateTime>();
            open = new List<double>();
            high = new List<double>();
            low = new List<double>();
            close = new List<double>();

            var rows = ProcessCSV(file);

            foreach (var row in rows)
            {
                this.date.Add(row.date);
                this.open.Add(row.open);
                this.high.Add(row.high);
                this.low.Add(row.low);
                this.close.Add(row.close);
            }
            
            this.symbol = symbol;
            this.category = category;
            this.sector = sector;
            this.bigpointvalue = bigpointvalue;
            this.ticksize = ticksize;
        }


        public Quote(Quote in_quote, List<DateTime> D, List<double> O, List<double> H, List<double> L, List<double> C)
        {
            this.date = D;
            this.open = O;
            this.high = H;
            this.low = L;
            this.close = C;
            this.symbol = in_quote.Symbol;
            this.category = in_quote.Category;
            this.sector = in_quote.Sector;
            this.bigpointvalue = in_quote.BigPointValue;
            this.ticksize = in_quote.TickSize;
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
            Console.WriteLine("Symbol         : {0}", symbol);
            Console.WriteLine("Sector         : {0}", sector);
            Console.WriteLine("Category       : {0}", category);
            Console.WriteLine("BigPointValue  : {0:0.00}", BPV);
            Console.WriteLine("TickSize       : {0:0.00}", TS);
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
                Console.WriteLine("{0}   {1}   {2}   {3}   {4}",date[i], open[i], high[i], low[i], close[i]);
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

