  
namespace ProBT
{
    public class Backtest
    {   
        internal int MaxBarsBack {get; set;} = 8;
        internal decimal InitialBalance {get; set;} = 100000;

        // Start BackTest
        public PerformanceReport Run(Quote _quote, Strategy _strategy)
        {            
            IQuote quote = _quote;
            // quote.PrintInfo();

            Strategy strategy =  _strategy.Clean();

            Equity equity = new Equity(this.InitialBalance);

            strategy.send_attribute(quote.BigPointValue, quote.TickSize);
            strategy.Iniialize();

            for (int bar = this.MaxBarsBack; bar < quote.Date.Count(); bar++)
            {
                List<DateTime> _dt = quote.Date.GetRange(bar-this.MaxBarsBack, this.MaxBarsBack);
                List<double> _op = quote.Open.GetRange(bar-this.MaxBarsBack, this.MaxBarsBack);
                List<double> _hi = quote.High.GetRange(bar-this.MaxBarsBack, this.MaxBarsBack);
                List<double> _lo = quote.Low.GetRange(bar-this.MaxBarsBack, this.MaxBarsBack);
                List<double> _cl = quote.Close.GetRange(bar-this.MaxBarsBack, this.MaxBarsBack);
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
            PerformanceReport performance_report = new PerformanceReport(strategy.Trades, equity);
            // Console.WriteLine(strategy.Trades);
            // Console.WriteLine($"{performance_report.Summary.StratSum["NumTrades"]}");
            // Console.ReadLine();

            return performance_report;
        }
    }
}