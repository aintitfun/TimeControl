using System.Threading;
using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using Common;
using System.IO;
using System.Security.Principal ;
using System.Runtime.InteropServices;

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
            //Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName)
            string path=System.Reflection.Assembly.GetEntryAssembly().Location.Replace("\\Monitor\\bin\\Debug\\netcoreapp3.0\\Monitor.dll","");
            Process.Start(path+"\\pgsql\\bin\\pg_ctl.exe","-D "+path+"\\pgsql\\data start");


            //Process.Start(@"..\\..\\..\\..\\pgsql\\bin\\pg_ctl.exe", " -D ..\\..\\..\\..\\pgsql\\data start ");
        }

        /// <summary>
        /// Get the owner of a process
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>    
        private static string GetProcessUser(Process process)
        {
            IntPtr processHandle = IntPtr.Zero;
            try
            {
                OpenProcessToken(process.Handle, 8, out processHandle);
                WindowsIdentity wi = new WindowsIdentity(processHandle);
                string user = wi.Name;
                return user.Contains(@"\") ? user.Substring(user.IndexOf(@"\") + 1) : user;
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                if (processHandle != IntPtr.Zero)
                {
                    CloseHandle(processHandle);
                }
            }
        }

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool OpenProcessToken(IntPtr ProcessHandle, uint DesiredAccess, out IntPtr TokenHandle);
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(IntPtr hObject);
        
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
                    processList.Add(new ProcessesPersist(process.Id,process.ProcessName,GetProcessUser(process)));
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
                        vSQLite.UpdateApp(process.ProcessName, process.User, process.Id);
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
                        vSQLite.UpdateApp(process.ProcessName, process.User,process.Id);
                        //logger.Log ($@"{DateTime.Now} [STARTED]: {process.ProcessName} {process.Id}");
                    }

                }
                catch (Exception ex)
                {
                }
            }

            nCycleCount++;
            if (nCycleCount>5){
                nCycleCount=0;
                List<AppsPersist> lap = vSQLite.GetApps();
                
                foreach (AppsPersist a in lap){
                    if (vSQLite.GetActiveMinutes(a.app,a.username)>a.maxTime){
                        foreach (Process process in Process.GetProcesses().Where(x => x.ProcessName==a.app && GetProcessUser(x)==a.username)){
                            logger.Log($@"{DateTime.Now} [KILLING]: {process.ProcessName} {process.Id} {a.username}");
                            try 
                            {
                                process.Kill();
                            }
                            catch (Exception e)
                            {
                                logger.Log($@"{DateTime.Now} [ERROR]: Unable to kill {process.ProcessName} {process.Id} {a.username}");

                            }
                            
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
        public string User;
        public ProcessesPersist(int nPid,string name, string username )

        {
            Id=nPid;
            ProcessName=name;
            User=username;
            
        }

    }
}

