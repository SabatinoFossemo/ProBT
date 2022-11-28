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
            // mydata.Statistics();

            MyStrategy mystrategy = new MyStrategy();

            // ProBT.Backtest myBT = new ProBT.Backtest();
            // myBT.MaxBarsBack = 200;

            // myBT.Run(mydata, mystrategy);
            
            ProBT.Permutation myRand = new ProBT.Permutation();
            myRand.Run(mydata, mystrategy, 10);
            
        }
    }

    public class MyStrategy : ProBT.Strategy
    {
        public override void Iniialize()
        {
        }

        public override void OnBarUpdate()
        {

            if(C[0] > C[1])
                Buy(High[0], "CL_DC");

            if(C[0] < C[1])
                SellShort(Low[0], "CL_DC");
        }

        public override void Deinitialize()
        {
        }

    }
}