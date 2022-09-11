using System.Threading;
using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Security.Principal ;
using System.Runtime.InteropServices;
using System.Diagnostics.Eventing.Reader;
using System.Management;
using System.Text.RegularExpressions;

namespace Backend
{
    public class Monitor
    {
        ProcessSQL vSQL = new ProcessSQL();
        //test comment
        public List<ProcessesPersist> processes_persist_old = new List<ProcessesPersist>();
        public List<ProcessesPersist> processes_persist = new List<ProcessesPersist>();
        public DateTime START_TIME;
        //this var is not optmized: it reads the file on each app iteration. maybe better read once on start or add a method to reload by commmand line
        public static List<string> IgnoredApps {get=> File.ReadAllLines("IgnoredApps.cfg").ToList<string>(); }
        public Process[] processes;
        public int nCycleCount;
        public static List<string> users;
        public Monitor()
        {
            users = new List<string>();
            PopulateListOfWindowsLocalUsers();

            START_TIME =DateTime.Now;
            nCycleCount=0;

        }

        public void Start()
        {
            StartDatabase();

            ProcessSQL vSQLite = new ProcessSQL();

            vSQLite.CheckAndRecreateTables();
            vSQLite.HistApps();
            vSQLite.ReStartConsumedTimeFromUsers();




            while (true)
            {
                CheckApps();
                CheckShutdowns();
                Thread.Sleep(3000);
            }
        }

        public void StartDatabase()
        {
            try
            {
                string path = System.Reflection.Assembly.GetEntryAssembly().Location.Replace("BlazorServerApp.dll", "");
                Logger.Log($@"{DateTime.Now} [INFO]: Trying to start postgres database on path: {path}");
                Process.Start(path + "\\pgsql\\bin\\pg_ctl.exe", "-D " + path + "\\pgsql\\data start");
            }
            catch (Exception ex)
            {
                Logger.Log($@"Error starting PG: {ex.Message}. Anyway trying to start Monitor");
            }
                //Thread.Sleep(30000);
            
        }

        private static string GetProcessUser(Process process) 
        {
            
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return GetProcessUserWindows(process);
            }
            else 
            {
                return GetProcessUserLinux(process);
            }
        }
        /// <summary>
        /// Get the owner of a process
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>    
        private static string GetProcessUserWindows(Process process)
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
       private static string GetProcessUserLinux(Process process)
        {
            var proc = new Process 
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "ps",
                    Arguments = $@"-p {process.Id} -o euser",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            proc.Start();
            string line="";
            while (!proc.StandardOutput.EndOfStream)
            {
                line = proc.StandardOutput.ReadLine();
            }

            return line;
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
                    Logger.Log ($@"{DateTime.Now} [ERROR]: Populating {process.ProcessName}");
                }

            }
        }
        /// <summary>
        /// Old snapshot is saved on db, this method populates the old processpersist object
        /// </summary>
        public void PopulateOldProcessSnapshot(){
            processes_persist_old.Clear();
            vSQL.GetDailyApps(ref processes_persist_old);
        }

            const int WTS_CURRENT_SESSION = -1;
            static readonly IntPtr WTS_CURRENT_SERVER_HANDLE = IntPtr.Zero;
        /// <summary>
        /// This method checks every cycle if any process should be killed. Also checks if some data must saved to sqlite
        /// </summary>
        public void CheckShutdowns()
        {
            /*    if (vSQL.GetShutdownTime()>0)
                {
                    var psi = new ProcessStartInfo("shutdown","/s /t 0");
                    psi.CreateNoWindow = true;
                    psi.UseShellExecute = false;
                    Logger.Log ($@"{DateTime.Now} [Shutdown]:");
                    Process.Start(psi);
                 }*/
            foreach (string userName in vSQL.GetUsers())
            {
                if (HasOpenSessionUserWindows(userName)>-1)
                    vSQL.UpdateSessionTime(userName);
            }

            foreach (string username in vSQL.GetUsersToLogOut())
            {
                int sessionid=0;
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    sessionid=HasOpenSessionUserWindows(username);
                }
                /*else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    sessionid=HasOpenSessionUserLinux(username);
                }*/
                if (sessionid>-1){
                    Logger.Log($@"{DateTime.Now} [KILL]: Forcing Logout session {username}");
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        if (!WTSDisconnectSession(WTS_CURRENT_SERVER_HANDLE,sessionid, false))
                            Logger.Log ($@"{DateTime.Now} [ERROR]: Forcing Logout windows session {username}");
                    } 
                    /*else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    {
                        Process.Start ("/usr/bin/kill",$@"-p {sessionid}");
                        Logger.Log ($@"{DateTime.Now} [ERROR]: Forcing Logout linux session {username}");
                    }*/
 
                }
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
                        vSQL.UpdateApp(process.ProcessName, process.User, process.Id);
                        Logger.Log ($@"{DateTime.Now} [CLOSED]: {process.ProcessName} {process.Id}");
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
                        vSQL.UpdateApp(process.ProcessName, process.User,process.Id);
                        Logger.Log ($@"{DateTime.Now} [STARTED]: {process.ProcessName} {process.Id}");
                    }

                }
                catch (Exception ex)
                {
                }
            }

            nCycleCount++;
            if (nCycleCount>5){
                nCycleCount=0;
                List<AppsPersist> lap = vSQL.GetApps();
                
                foreach (AppsPersist a in lap){
                    if (vSQL.GetActiveTimeByAppAndUser(a._app,a._userName)>a._time && a._dayOfTheWeek.ToLower()==DateTime.Today.DayOfWeek.ToString().ToLower())
                    {
                        foreach (Process process in Process.GetProcesses().Where(x => x.ProcessName==a._app && GetProcessUserWindows(x)==a._userName)){
                            Logger.Log($@"{DateTime.Now} [KILLING]: {process.ProcessName} {process.Id} {a._userName}");
                            try 
                            {
                                process.Kill();
                            }
                            catch (Exception e)
                            {
                                Logger.Log($@"{DateTime.Now} [ERROR]: Unable to kill {process.ProcessName} {process.Id} {a._userName}");

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

            public int HasOpenSessionUserLinux(string username)
            {
                var proc = new Process 
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "who",
                        Arguments = $@"-u",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    }
                };

                proc.Start();
                string line="";
                while (!proc.StandardOutput.EndOfStream)
                {
                    line = proc.StandardOutput.ReadLine();
                    if (line.Substring(0,line.IndexOf(" "))==username)
                    {
                        string lineWitoutLastWord=line.Substring(0,line.LastIndexOf(" "));
                        return int.Parse(lineWitoutLastWord.Substring(lineWitoutLastWord.LastIndexOf(" ")));

                    }
                }
                return -1;
            }
            public int HasOpenSessionUserWindows(string username)
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

                        //Console.WriteLine("Domain and User: " + Marshal.PtrToStringAnsi(domainPtr) + "\\" + Marshal.PtrToStringAnsi(userPtr));

                        if (Marshal.PtrToStringAnsi(userPtr) == username)
                        {
                            if (si.State == WTS_CONNECTSTATE_CLASS.WTSActive)
                                sessionid = si.SessionID;
                        } 
                            

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
        string GetSidByUserName(string username)
        {
            // var user = WindowsIdentity.
            //var user = WindowsIdentity.GetCurrent().User;
            // string sid = UserPrincipal.Current.Sid.ToString();


            NTAccount f = new NTAccount(username);
            SecurityIdentifier s = (SecurityIdentifier)f.Translate(typeof(SecurityIdentifier));
            String sidString = s.ToString();

            

            return sidString;
        }
        public void PopulateListOfWindowsLocalUsers()
        {
            SelectQuery query = new SelectQuery("Win32_UserAccount");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            foreach (ManagementObject envVar in searcher.Get())
            {
                if (envVar["AccountType"].ToString()=="512" && envVar["Disabled"].ToString()=="False")
                    users.Add(envVar["Name"].ToString());
                //Console.WriteLine("Username : {0} {1}", envVar["Name"], GetSidByUserName(envVar["Name"].ToString()));
                //vSQL.InsertLocalUser(envVar["Name"].ToString(), GetSidByUserName(envVar["Name"].ToString()));
            }
        }

        public string getMacByIp(string ip)
        {
            var macIpPairs = GetAllMacAddressesAndIppairs();
            int index = macIpPairs.FindIndex(x => x.IpAddress == ip);
            if (index >= 0)
            {
                return macIpPairs[index].MacAddress.ToUpper();
            }
            else
            {
                return null;
            }
        }

        public List<MacIpPair> GetAllMacAddressesAndIppairs()
        {
            List<MacIpPair> mip = new List<MacIpPair>();
            System.Diagnostics.Process pProcess = new System.Diagnostics.Process();
            pProcess.StartInfo.FileName = "arp";
            pProcess.StartInfo.Arguments = "-a ";
            pProcess.StartInfo.UseShellExecute = false;
            pProcess.StartInfo.RedirectStandardOutput = true;
            pProcess.StartInfo.CreateNoWindow = true;
            pProcess.Start();
            string cmdOutput = pProcess.StandardOutput.ReadToEnd();
            string pattern = @"(?<ip>([0-9]{1,3}\.?){4})\s*(?<mac>([a-f0-9]{2}-?){6})";

            foreach (Match m in Regex.Matches(cmdOutput, pattern, RegexOptions.IgnoreCase))
            {
                mip.Add(new MacIpPair()
                {
                    MacAddress = m.Groups["mac"].Value,
                    IpAddress = m.Groups["ip"].Value
                });
            }

            return mip;
        }
        public struct MacIpPair
        {
            public string MacAddress;
            public string IpAddress;
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
    //public voidGetIgnoredAppsFromFile()
    //{
    //    IgnoredApps=File.ReadAllLines(textFile);
    //}


}

