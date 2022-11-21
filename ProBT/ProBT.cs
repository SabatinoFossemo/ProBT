
namespace ProBT
{
    public class Backtest
    {   
        int maxbarsback = 200;
        double init_balance = 100000;

        //property
        public int MaxBarsBack{get => this.maxbarsback; set=>this.maxbarsback = value;}
        public double InitialBalance{get => this.init_balance; set=>this.init_balance = value;}


        // Start BackTest
        public PerformanceReport Run(Quote _quote, Strategy _strategy)
        {
            Console.WriteLine("Run Backtest...");
            
            Quote quote = _quote;
            Strategy strategy = _strategy;
            Equity equity = new Equity(this.init_balance);

            strategy.send_attribute(quote.BigPointValue, quote.TickSize);
            strategy.Iniialize();

            for (int bar = maxbarsback; bar < quote.Date.Count(); bar++)
            {
                List<DateTime> _dt = quote.Date.GetRange(bar-maxbarsback, maxbarsback);
                List<double> _op = quote.Open.GetRange(bar-maxbarsback, maxbarsback);
                List<double> _hi = quote.High.GetRange(bar-maxbarsback, maxbarsback);
                List<double> _lo = quote.Low.GetRange(bar-maxbarsback, maxbarsback);
                List<double> _cl = quote.Close.GetRange(bar-maxbarsback, maxbarsback);

                // # ReIndex as bars ago.
                // # example:
                // # bar[0] is the current bar.
                // # bar[5] is 5 bars ago.
                _dt.Reverse();
                _op.Reverse();
                _hi.Reverse();
                _lo.Reverse();
                _cl.Reverse();

                //# send updated data to strategy
                strategy.update_quote(_dt, _op, _hi, _lo, _cl);

                // # strategy execute order at open
                strategy.execute_orders(CHECK_AT.OPEN);

                // # print('execute IS SESSION')
                strategy.execute_orders(CHECK_AT.SESSION);

                // # strategy delete pending
                strategy.DeleteOrders();

                // # update equity
                equity.AddPoint(strategy.GetPoint());

                // # Strategy On BAR Function
                strategy.OnBarUpdate();

                // # strategy execute order at close
                strategy.execute_orders(CHECK_AT.CLOSE);
            }

            strategy.Deinitialize();

            // End BackTest
            Console.WriteLine(strategy.Trades);
            PerformanceReport performance_report = new PerformanceReport(strategy.Trades, equity);
            Console.WriteLine(performance_report.Summary);

            return performance_report;
        }


    }

    public class Permutation
    {
        List<PerformanceReport> backtestList = new List<PerformanceReport>();

        public void Run(Quote _quote, Strategy _strategy, int iter_number)
        {
            // Original BackTest
            ProBT.Backtest orig_bt = new ProBT.Backtest();
            backtestList.Add(orig_bt.Run(_quote, _strategy));
            
            // Randomization
            for (int i = 0; i < iter_number; i++)
            {
                ProBT.Backtest bt = new ProBT.Backtest();
                Quote fake_q = new Quote(_quote);

                backtestList.Add(bt.Run(fake_q, _strategy));
            }
        }

        private Quote perform_randomization(Quote _q)
        {
            List<DateTime> D = new List<DateTime>();
            List<double> O = new List<double>();
            List<double> H = new List<double>();
            List<double> L = new List<double>();
            List<double> C = new List<double>();

            int[] idx = new int[_q.Date.Count()];
            for (int i = 0; i < _q.Date.Count(); i++)
                idx[i] = i;

            D = _q.Date;

            double value = 0;
            O.Add(value);

            value = (_q.High[0] - _q.Open[0]) / _q.Open[0];
            H.Add(value);

            value = (_q.Low[0] - _q.High[0]) / _q.High[0];
            L.Add(value);

            value = (_q.Close[0] - _q.Low[0]) / _q.Low[0];
            C.Add(value);

            for (int i = 1; i < _q.Date.Count(); i++)
            {
                // close to open
                value = (_q.Open[i] - _q.Close[i-1]) / _q.Close[i-1];
                O.Add(value);

                // open to high
                value = (_q.High[i] - _q.Open[i]) / _q.Open[i];
                H.Add(value);

                // high to low
                value = (_q.Low[i] - _q.High[i]) / _q.High[i];
                L.Add(value);

                // low to close
                value = (_q.Close[i] - _q.Low[i]) / _q.Low[i];
                C.Add(value);
            }

            var rng = new Random();
            RandomExtensions.Shuffle(rng, idx);
            foreach(var x in idx)
            Console.WriteLine(x);
            Console.ReadLine();



            Quote result = new Quote(_q, D, O, H, L, C);

            return result;
        }


    }

    static class RandomExtensions
    {
        public static void Shuffle<T> (this Random rng, T[] array)
        {
            int n = array.Length;
            while (n > 1) 
            {
                int k = rng.Next(n--);
                T temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
        }
    }

}