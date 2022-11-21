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
        double bigpointvalue { get; set; }
        double ticksize { get; set; }

        public Quote(string file, string symbol="NONE", string category="NONE", string sector="NONE", double bigpointvalue=1, double ticksize=1)
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

        // public Quote(Quote in_quote)
        // {
        //     date = new List<DateTime>();
        //     open = new List<double>();
        //     high = new List<double>();
        //     low = new List<double>();
        //     close = new List<double>();

        //     this.symbol = in_quote.Symbol;
        //     this.category = in_quote.Category;
        //     this.sector = in_quote.Sector;
        //     this.bigpointvalue = in_quote.BigPointValue;
        //     this.ticksize = in_quote.TickSize;
        // }

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



        private List<Bar> ProcessCSV(string path)
        {
            return File.ReadAllLines(path)
                .Skip(1)
                .Where(row => row.Length > 0)
                .Select(Bar.ParseRow).ToList();
        }

        public List<DateTime> Date{get => date;}
        public List<double> Open{get => open;}
        public List<double> High{get => high;}
        public List<double> Low{get => low;}
        public List<double> Close{get => close;}

        public string Symbol {get => symbol;}
        public string Category {get => category;}
        public string Sector {get => sector;}
        public double TS {get => ticksize;}
        public double TickSize {get => ticksize;}
        public double BPV {get => bigpointvalue;}
        public double BigPointValue {get => bigpointvalue;}

        public void PrintInfo()
        {
            Console.WriteLine("*  QUOTE INFO  *");
            Console.WriteLine("Symbol         : {0}", symbol);
            Console.WriteLine("Sector         : {0}", sector);
            Console.WriteLine("Category       : {0}", category);
            Console.WriteLine("BigPointValue  : {0:0.00}", BPV);
            Console.WriteLine("TickSize       : {0:0.00}", TS);
            Console.WriteLine("*--  samples  -------------*\n");
            Console.WriteLine("               date   open   high    low  close");
            for (int i = 0; i < 5; i++)
                Console.WriteLine("{0}   {1}   {2}   {3}   {4}",date[i], open[i], high[i], low[i], close[i]);
            Console.WriteLine("*--------------------------*");
        }
    }

    public class Bar
    {
        public DateTime date { get; set; }
        public double open { get; set; }
        public double high { get; set; }
        public double low { get; set; }
        public double close { get; set; }

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

        public override string ToString()
        {
            string result = string.Concat(date.ToString() + ' ' +  
            string.Format("{0:0.00}",open) + ' ' + 
            string.Format("{0:0.00}",high) + ' ' + 
            string.Format("{0:0.00}",low) + ' ' + 
            string.Format("{0:0.00}",close)
            );
            return result;
        }
    }


}

