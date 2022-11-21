using ProBT;
using System;

namespace TestCL
{
    class Program 
    {
        static void Main(string[] args) 
        {
            string file_path = "/Users/sabatinofossemo/VSC_Project/CSHARP/ClassLibraryProjects/ShowCase/@CL.csv";

            Quote mydata = new Quote(file_path, "CL", "Futures", "Energy", 1000, 0.01);
            mydata.PrintInfo();

            MyStrategy mystrategy = new MyStrategy();

            ProBT.Backtest myBT = new ProBT.Backtest();
            myBT.MaxBarsBack = 200;

            myBT.Run(mydata, mystrategy);
            
            // ProBT.Permutation myRand = new ProBT.Permutation();
            // myRand.Run(mydata, mystrategy, 10);
            
        }
    }

    public class MyStrategy : ProBT.Strategy
    {
        public override void Iniialize()
        {
            Console.WriteLine("Inizialize...");
        }
        public override void OnBarUpdate()
        {
            // Console.Write("Strategy: ");
            // Console.WriteLine(D[0]);

            if(C[0] > C[1])
                SellShort("close", "CL_DC");

            if(C[0] < C[1])
                Buy("close", "CL_DC");
            SetTakeProfitDollar(2000);
        }
        public override void Deinitialize()
        {
            Console.WriteLine("Deinitialize...");

            // Console.WriteLine("DeInitialize...");
            // Console.WriteLine(this.Trades.ToString());
        }

    }
}