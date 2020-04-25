using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using Common;

namespace Monitor
{
    public class Monitor
    {
        ProcessSQL vSQLite = new ProcessSQL();
        //test comment
        Logger logger=new Logger();
        public List<ProcessesPersist> processes_persist_old = new List<ProcessesPersist>();
        public List<ProcessesPersist> processes_persist = new List<ProcessesPersist>();
        public DateTime START_TIME;
                                      
        public Process[] processes;
        public int nCycleCount;
        public Monitor()
        {
            
            START_TIME=DateTime.Now;
            nCycleCount=0;

        }

        public void StartDatabase()
        {
            


            //var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../pgsql/bin/pg_ctl.exe", " -D ..\\..\\..\\..\\pgsql\\data start");
            //var dbpath= Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../pgsql/data/");
            Process.Start(Environment.CurrentDirectory+"\\..\\pgsql\\bin\\pg_ctl.exe","-D "+Environment.CurrentDirectory+"\\..\\pgsql\\data -l logfile.txt start");


            //Process.Start(@"..\\..\\..\\..\\pgsql\\bin\\pg_ctl.exe", " -D ..\\..\\..\\..\\pgsql\\data start ");
        }
        /// <summary>
        /// Fills the given list with current processes
        /// </summary>
        /// <param name="l"></param>
        public void PopulateProcesses(List<ProcessesPersist> processList)
        {
            processList.Clear();
            
            processes=(Process[])Process.GetProcesses();

            foreach (Process process in processes )
            {
                try 
                {
                    processList.Add(new ProcessesPersist(process.Id,process.ProcessName));
                }
                catch (Exception e)
                {
                    logger.Log ($@"{DateTime.Now} [ERROR]: Populating {process.ProcessName}");
                }

            }
        }
        /// <summary>
        /// Old snapshot is saved on db, this method populates the old processpersist object
        /// </summary>
        public void PopulateOldProcessSnapshot(){
            processes_persist_old.Clear();
            vSQLite.GetDailyApps(ref processes_persist_old);
        }

        /// <summary>
        /// This method checks every cycle if any process should be killed. Also checks if some data must saved to sqlite
        /// </summary>
        public void CheckProcesses() 
        {

            //Insertamos los procesos nuevos de memoroia
            PopulateProcesses(processes_persist); 
            
            //insertamos los procesos viejos de db
            PopulateOldProcessSnapshot();

            // Buscamos aplicaciones que se cierren
            foreach (ProcessesPersist process in processes_persist_old)
            {
                try
                {
                    var results = processes_persist.Where(x => x.ProcessName == process.ProcessName && x.Id==process.Id);
                    if (results.Count()==0){
                        vSQLite.UpdateApp(process.ProcessName, process.Id);
                        logger.Log ($@"{DateTime.Now} [CLOSED]: {process.ProcessName} {process.Id}");
                    }

                }
                catch (Exception ex)
                {

                }
            }
            //buscamos apps que se hayan abierto
            
            foreach (ProcessesPersist process in processes_persist)
            {
                try
                {
                    //var results =  Array.FindAll(processes.ProcessName, s => s.Equals(process.ProcessName));
                    var results = processes_persist_old.Where(x => x.ProcessName == process.ProcessName && x.Id==process.Id);
                    if (results.Count()==0){
                        vSQLite.UpdateApp(process.ProcessName, process.Id);
                        //logger.Log ($@"{DateTime.Now} [STARTED]: {process.ProcessName} {process.Id}");
                    }

                }
                catch (Exception ex)
                {
                }
            }

            nCycleCount++;
            if (nCycleCount>10){
                nCycleCount=0;
                List<AppsPersist> lap = vSQLite.GetApps();
                
                foreach (AppsPersist a in lap){
                    if (vSQLite.GetActiveMinutes(a.app)>a.maxTime){
                        foreach (Process process in Process.GetProcesses().Where(x => x.ProcessName==a.app)){
                            logger.Log($@"{DateTime.Now} [KILLING]: {process.ProcessName} {process.Id}");
                            process.Kill();
                        }
                    }
                }
            }
        } 
            
        
    }

    /// <summary>
    /// This class is the struct to save the processes from Process object to Memory
    /// </summary>
    public class ProcessesPersist 
    {
        public string ProcessName;
        public int Id;
        public ProcessesPersist(int nPid,string name )

        {
            Id=nPid;
            ProcessName=name;
            
        }

    }
}

