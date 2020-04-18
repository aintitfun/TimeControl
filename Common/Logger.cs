using System.IO;
using System;

namespace Common
{
    public class Logger
    {
        public StreamWriter swLog;
        public Logger()
        {
                        
        }
        public void Log(string strMessage){
            swLog = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory+"log.txt", true);
            swLog.AutoFlush = true;
            swLog.WriteLine(strMessage);
            swLog.Close();
            Console.WriteLine(strMessage);
        }
    }
}