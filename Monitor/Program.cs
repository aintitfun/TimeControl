using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Monitor
{
    class Program
    {
static void Main(string[] args)
        {
            Console.WriteLine("hola" );

            StringBuilder sb = new StringBuilder();

            Monitor vMon = new Monitor();
            vMon.StartDatabase();

            ProcessSQL vSQLite = new ProcessSQL();

            vSQLite.CheckAndRecreateTables();
            vSQLite.HistApps();

            //vSQLite.AddApplication("xterm",true,1);



            SocServer vCom = new SocServer();
            Thread thr= new Thread(new ThreadStart(vCom.CreateServer));
            thr.Start(); 

            while (true) {
                vMon.CheckProcesses(); 
                Thread.Sleep(3000);
            }
            

        

            Console.ReadKey();
        }
    }
}
