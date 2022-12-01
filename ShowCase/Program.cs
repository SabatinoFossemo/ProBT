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
            mydata.AdjValueBelow0();
            mydata.PrintInfo();
            Console.ReadLine();
            // mydata.Statistics();

            MyStrategy mystrategy = new MyStrategy();

            // ProBT.Backtest myBT = new ProBT.Backtest();
            // myBT.MaxBarsBack = 200;

            // myBT.Run(mydata, mystrategy);
            
            ProBT.Permutation myRand = new ProBT.Permutation();
            myRand.Run(mydata, mystrategy, 500);
            
        }
    }

    public class MyStrategy : ProBT.Strategy
    {
        public override void Iniialize()
        {
        }

        public override void OnBarUpdate()
        {

            bool condition1 = C[0] < C[1];
            bool condition2 = DayOfWeek.Thursday == D[0].DayOfWeek;
            bool condition3 = C[0] >  C[7];
            bool condition4 = H[0]-L[0] >  H[7]-L[7];

// if not condition1 and condition2 and condition3 and not condition4 then buy this bar close;
// if condition1 and condition2  and not condition3 and not condition4 then sellshort this bar close;

            if(!condition1 && condition2 && condition3 && !condition4)
                Buy("close", "CL_DC");

            if(condition1 && condition2 && !condition3 && !condition4)
                SellShort("close", "CL_DC");

        }

        public override void Deinitialize()
        {
        }

    }
}