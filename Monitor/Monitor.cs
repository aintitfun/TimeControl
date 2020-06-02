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
            
            logger.Log ($@"{DateTime.Now} [INFO]: Trying to start postgres database");
            string path=System.Reflection.Assembly.GetEntryAssembly().Location.Replace("\\Monitor\\bin\\Debug\\netcoreapp3.0\\Monitor.dll","");
            Process.Start(path+"\\pgsql\\bin\\pg_ctl.exe","-D "+path+"\\pgsql\\data start");
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

            const int WTS_CURRENT_SESSION = -1;
            static readonly IntPtr WTS_CURRENT_SERVER_HANDLE = IntPtr.Zero;
        /// <summary>
        /// This method checks every cycle if any process should be killed. Also checks if some data must saved to sqlite
        /// </summary>
        public void CheckShutdowns()
        {
        /*    if (vSQLite.GetShutdownTime()>0)
            {
                var psi = new ProcessStartInfo("shutdown","/s /t 0");
                psi.CreateNoWindow = true;
                psi.UseShellExecute = false;
                logger.Log ($@"{DateTime.Now} [Shutdown]:");
                Process.Start(psi);
             }*/

            
            //string username=System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            //username=username.Replace(System.Net.Dns.GetHostName()+"\\","");
            
            foreach(string username in vSQLite.GetUsersToLogOut())
            {
                int sessionid=HasOpenSessionUser(username);
                if (sessionid>-1)
                    if (!WTSDisconnectSession(WTS_CURRENT_SERVER_HANDLE,sessionid, false))
                        logger.Log ($@"{DateTime.Now} [ERROR]: Forcing Logout {username}");
            }
        }
        [DllImport("wtsapi32.dll", SetLastError = true)]
        static extern bool WTSDisconnectSession(IntPtr hServer, int sessionId, bool bWait);


        /// <summary>
        /// This method checks every cycle if any process should be killed. Also checks if some data must saved to sqlite
        /// </summary>
        public void CheckApps() 
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
                        logger.Log ($@"{DateTime.Now} [STARTED]: {process.ProcessName} {process.Id}");
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
                    if (vSQLite.GetActiveMinutes(a.app,a.userName)>a.maxTime){
                        foreach (Process process in Process.GetProcesses().Where(x => x.ProcessName==a.app && GetProcessUser(x)==a.userName)){
                            logger.Log($@"{DateTime.Now} [KILLING]: {process.ProcessName} {process.Id} {a.userName}");
                            try 
                            {
                                process.Kill();
                            }
                            catch (Exception e)
                            {
                                logger.Log($@"{DateTime.Now} [ERROR]: Unable to kill {process.ProcessName} {process.Id} {a.userName}");

                            }
                            
                        }
                    }
                }
            }
        } 

/// <summary>
/// all this part is a function to get a list of loged users on win
/// extracted from https://stackoverflow.com/questions/132620/how-do-you-retrieve-a-list-of-logged-in-connected-users-in-net
/// </summary>
/// <returns></returns>
            [DllImport("wtsapi32.dll")]
            static extern IntPtr WTSOpenServer([MarshalAs(UnmanagedType.LPStr)] string pServerName);

            [DllImport("wtsapi32.dll")]
            static extern void WTSCloseServer(IntPtr hServer);

            [DllImport("wtsapi32.dll")]
            static extern Int32 WTSEnumerateSessions(
            IntPtr hServer,
            [MarshalAs(UnmanagedType.U4)] Int32 Reserved,
            [MarshalAs(UnmanagedType.U4)] Int32 Version,
            ref IntPtr ppSessionInfo,
            [MarshalAs(UnmanagedType.U4)] ref Int32 pCount);

            [DllImport("wtsapi32.dll")]
            static extern void WTSFreeMemory(IntPtr pMemory);

            [DllImport("wtsapi32.dll")]
            static extern bool WTSQuerySessionInformation(
                IntPtr hServer, int sessionId, WTS_INFO_CLASS wtsInfoClass, out IntPtr ppBuffer, out uint pBytesReturned);

            [StructLayout(LayoutKind.Sequential)]
            private struct WTS_SESSION_INFO
            {
            public Int32 SessionID;

            [MarshalAs(UnmanagedType.LPStr)]
            public string pWinStationName;

            public WTS_CONNECTSTATE_CLASS State;
            }

            public enum WTS_INFO_CLASS
            {
            WTSInitialProgram,
            WTSApplicationName,
            WTSWorkingDirectory,
            WTSOEMId,
            WTSSessionId,
            WTSUserName,
            WTSWinStationName,
            WTSDomainName,
            WTSConnectState,
            WTSClientBuildNumber,
            WTSClientName,
            WTSClientDirectory,
            WTSClientProductId,
            WTSClientHardwareId,
            WTSClientAddress,
            WTSClientDisplay,
            WTSClientProtocolType
            }

            public enum WTS_CONNECTSTATE_CLASS
            {
            WTSActive,
            WTSConnected,
            WTSConnectQuery,
            WTSShadow,
            WTSDisconnected,
            WTSIdle,
            WTSListen,
            WTSReset,
            WTSDown,
            WTSInit
            }

            public int HasOpenSessionUser(string username)
            {
            string serverName=Environment.MachineName;
            IntPtr serverHandle = IntPtr.Zero;
            List<string> resultList = new List<string>();
            serverHandle = WTSOpenServer(serverName);
            int sessionid=-1;

            try
            {
                IntPtr sessionInfoPtr = IntPtr.Zero;
                IntPtr userPtr = IntPtr.Zero;
                IntPtr domainPtr = IntPtr.Zero;
                Int32 sessionCount = 0;
                Int32 retVal = WTSEnumerateSessions(serverHandle, 0, 1, ref sessionInfoPtr, ref sessionCount);
                Int32 dataSize = Marshal.SizeOf(typeof(WTS_SESSION_INFO));
                IntPtr currentSession = sessionInfoPtr;
                uint bytes = 0;

                if (retVal != 0)
                {
                for (int i = 0; i < sessionCount; i++)
                {
                    WTS_SESSION_INFO si = (WTS_SESSION_INFO)Marshal.PtrToStructure((System.IntPtr)currentSession, typeof(WTS_SESSION_INFO));
                    currentSession += dataSize;

                    WTSQuerySessionInformation(serverHandle, si.SessionID, WTS_INFO_CLASS.WTSUserName, out userPtr, out bytes);
                    WTSQuerySessionInformation(serverHandle, si.SessionID, WTS_INFO_CLASS.WTSDomainName, out domainPtr, out bytes);

                    Console.WriteLine("Domain and User: " + Marshal.PtrToStringAnsi(domainPtr) + "\\" + Marshal.PtrToStringAnsi(userPtr));

                    if (Marshal.PtrToStringAnsi(userPtr) == username)
                        sessionid=si.SessionID;

                    WTSFreeMemory(userPtr); 
                    WTSFreeMemory(domainPtr);
                }

                WTSFreeMemory(sessionInfoPtr);
                }
            }
            finally
            {
                WTSCloseServer(serverHandle);
            }
            return sessionid;

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

