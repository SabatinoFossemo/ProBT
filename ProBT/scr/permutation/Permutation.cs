namespace ProBT
{
    public class Permutation
    {
        List<PerformanceReport> backtestList = new List<PerformanceReport>();

        public void Run(Quote _quote, Strategy _strategy, int iter_number)
        {
            // Original BackTest
            ProBT.Backtest orig_bt = new ProBT.Backtest();

            PerformanceReport orig_perf_rep = orig_bt.Run(_quote, _strategy);

            backtestList.Add(orig_perf_rep);
            
            // Randomization
            for (int i = 0; i < iter_number; i++)
            {
                ProBT.Backtest bt = new ProBT.Backtest();
                Quote fake_q = perform_randomization(_quote);
                // Quote fake_q = perform_linear_randomization(_quote);
                PerformanceReport perf_rep = bt.Run(fake_q, _strategy);

                backtestList.Add(perf_rep);
            }

            this.Result("ProfitFactor");
        }

        public void Result(string metric)
        {
            var original = this.backtestList[0].Summary.StratSum[metric];
            
            int count  = 0;
            for (int i = 1; i < this.backtestList.Count; i++)
            {
                Console.WriteLine($"{this.backtestList[i].Summary.StratSum[metric]}");

                if( Convert.ToDouble(original) >  Convert.ToDouble(this.backtestList[i].Summary.StratSum[metric]))
                    count++;
            }

            double result = Convert.ToDouble(count) /  Convert.ToDouble(this.backtestList.Count)  * 100;
            Console.WriteLine($"Original is better than {result.ToString("F2")} % of the results");
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

            // calculate first bar
            double value = 0;
            O.Add(value);

            value = (_q.High[0] - _q.Open[0]) / _q.Open[0];
            H.Add(value);

            value = (_q.Low[0] - _q.High[0]) / _q.High[0];
            L.Add(value);

            value = (_q.Close[0] - _q.Low[0]) / _q.Low[0];
            C.Add(value);

            // calculate all other bars
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


            // shuffle index
            var rng = new Random();
            RandomExtensions.Shuffle(rng, idx);

            // randomized list
            List<double> O_r = new List<double>();
            List<double> H_r = new List<double>();
            List<double> L_r = new List<double>();
            List<double> C_r = new List<double>();

            // first bar
            bool first_bar = true;

            // all other bars
            for(int i = 0; i < _q.Date.Count(); i++)
            {
                // Randomized index
                int r_i = idx[i];

                if(first_bar){
                    O_r.Add(_q.Open[r_i]);
                    first_bar = false;
                }
                else{
                    O_r.Add(C_r[i-1] + (C_r[i-1] * O[r_i]));
                }
                H_r.Add(O_r[i] + (O_r[i] * H[r_i]));
                L_r.Add(H_r[i] + (H_r[i] * L[r_i]));
                C_r.Add(L_r[i] + (L_r[i] * C[r_i]));

                // Console.WriteLine($"{O[r_i]} - {H[r_i]} - {L[r_i]} - {C[r_i]}");
                // Console.WriteLine($"{O_r[i]} - {H_r[i]} - {L_r[i]} - {C_r[i]}");
                // Console.WriteLine();
                // Console.ReadLine();
            }


            Quote result = new Quote(_q, D, O_r, H_r, L_r, C_r);
            result.AdjValueBelow0();

            return result;
        }

       private Quote perform_linear_randomization(Quote _q)
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

            // calculate first bar
            double value = 0;
            O.Add(value);

            value = _q.High[0] - _q.Open[0];
            H.Add(value);

            value = _q.Low[0] - _q.High[0];
            L.Add(value);

            value = _q.Close[0] - _q.Low[0];
            C.Add(value);

            // calculate all other bars
            for (int i = 1; i < _q.Date.Count(); i++)
            {
                // close to open
                value = _q.Open[i] - _q.Close[i-1];
                O.Add(value);

                // open to high
                value = _q.High[i] - _q.Open[i];
                H.Add(value);

                // high to low
                value = _q.Low[i] - _q.High[i];
                L.Add(value);

                // low to close
                value = _q.Close[i] - _q.Low[i];
                C.Add(value);
            }


            // shuffle index
            var rng = new Random();
            RandomExtensions.Shuffle(rng, idx);

            // randomized list
            List<double> O_r = new List<double>();
            List<double> H_r = new List<double>();
            List<double> L_r = new List<double>();
            List<double> C_r = new List<double>();

            // first bar
            bool first_bar = true;

            // all other bars
            for(int i = 0; i < _q.Date.Count(); i++)
            {
                // Randomized index
                int r_i = idx[i];

                if(first_bar){
                    O_r.Add(_q.Open[r_i]);
                    first_bar = false;
                }
                else{
                    O_r.Add(C_r[i-1] + O[r_i]);
                }
                H_r.Add(O_r[i] + H[r_i]);
                L_r.Add(H_r[i] + L[r_i]);
                C_r.Add(L_r[i] + C[r_i]);

                // Console.WriteLine($"{O[r_i]} - {H[r_i]} - {L[r_i]} - {C[r_i]}");
                // Console.WriteLine($"{O_r[i]} - {H_r[i]} - {L_r[i]} - {C_r[i]}");
                // Console.WriteLine();
                // Console.ReadLine();
            }


            Quote result = new Quote(_q, D, O_r, H_r, L_r, C_r);
            result.AdjValueBelow0();

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