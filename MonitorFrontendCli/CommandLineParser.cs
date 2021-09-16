using System;
using System.Linq;

namespace TimeControl.MonitorFrontendCli
{
    /// <summary>
    /// This class should return a InvocationParameter object null in case of some error
    /// on parameters, otherwhise it will be filled with the parameters ready to be invoked
    /// </summary>
    public class CommandLineParser
    {
        public InvocationParameters Parse (string[] args)
        {
            if (RemoveUnusedArguments(args[0], ref args))
            {
                switch (args.GetLength(0)){
                    case 1:
                        return new InvocationParameters(null,null);
                    case 2:
                        return ParseList(args[0],args[1]);
                    case 3:
                        if (args[0] == "-addlogoutnow")
                            return ParseAddLogoutNow(args[0], args[1], args[2], args[3]);
                        break;
                    case 4:
                        if (args[0]=="-removelogout")
                            return ParseRemoveLogout(args[0],args[1],args[2], args[3]);
                        if (args[0]=="-removelogin")
                            return ParseRemoveLogin(args[0],args[1],args[2], args[3]);
                        if (args[0]=="-removeactivetime")
                            return ParseRemoveActiveTime(args[0],args[1],args[2], args[3]);

                        break;
                    case 5:
                        if (args[3].Contains(":"))
                        {
                            if (args[0]=="-addlogout")
                                return ParseAddLogout(args[0],args[1],args[2],args[3], args[4]);
                            if (args[0]=="-addlogin")
                                return ParseAddLogin(args[0],args[1],args[2],args[3], args[4]);
                        }
                        else
                        {
                            if (args[0]=="-removeapp")
                                return ParseRemoveApp(args[0],args[1],args[2],args[3], args[3]);
                            if (args[0]=="-addactivetime")
                                return ParseAddActiveTime(args[0],args[1],args[2],System.Convert.ToInt32(args[3]), args[4]);
                        }

                        break;
                    case 6:
                        return ParseAddApp(args[0],args[1],args[2],args[3],System.Convert.ToInt32(args[4]), args[5]);
                }

            } 

            return new InvocationParameters(null,null);

        }

        // the next Parse methods are to customice the check for each operation (nothing now at this moment)
        public InvocationParameters ParseList(string command,string host)
        {
            return new InvocationParameters(command,host);
        }

        public InvocationParameters ParseRemoveApp(string command,string host, string userName,string app, string dayOfTheWeek)
        {
            return new InvocationParameters(command,host,userName,app);
        }

        public InvocationParameters ParseAddLogout(string command,string host, string userName,string hourMin, string dayOfTheWeek)
        {
            return new InvocationParameters(command,host,userName,System.Convert.ToInt32(hourMin.Replace(":","")));
        }

        public InvocationParameters ParseAddLogin(string command,string host, string userName,string hourMin, string dayOfTheWeek)
        {
            return new InvocationParameters(command,host,userName,System.Convert.ToInt32(hourMin.Replace(":","")));
        }

        public InvocationParameters ParseAddLogoutNow(string command,string host, string userName, string dayOfTheWeek)
        {
            return new InvocationParameters(command,host,userName);
        }
        public InvocationParameters ParseAddApp(string command,string host, string userName,string app,int maxTime, string dayOfTheWeek)
        {
            return new InvocationParameters(command,host,userName,app,maxTime,dayOfTheWeek);
        }

        public InvocationParameters ParseRemoveLogout(string command,string host, string userName, string dayOfTheWeek)
        {
            return new InvocationParameters(command,host,userName);
        }

        public InvocationParameters ParseRemoveLogin(string command,string host, string userName, string dayOfTheWeek)
        {
            return new InvocationParameters(command,host,userName);
        }
        public InvocationParameters ParseAddActiveTime(string command,string host, string userName, int maxTime, string dayOfTheWeek)
        {
            return new InvocationParameters(command,host,userName,maxTime);
        }
        public InvocationParameters ParseRemoveActiveTime(string command,string host, string userName, string dayOfTheWeek)
        {
            return new InvocationParameters(command,host,userName);
        }
        /// <summary>
        /// Check that command exists, also remove non used parameters
        /// </summary>
        /// <param name="inCommand"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public bool RemoveUnusedArguments(string inCommand, ref string[] args)
        {
            if (inCommand.StartsWith("-"))
            {
                switch(inCommand.Substring(1))
                {
                    case "addapp":
                    return RemoveArguments(6,ref args);
                    case "listapps" :
                    return RemoveArguments(3,ref args);
                    case "stats":
                    return RemoveArguments(3,ref args);
                    case "removeapp":
                    return RemoveArguments(5,ref args);
                    case "listlogouts" :
                    return RemoveArguments(3,ref args);
                    case "listlogins" :
                    return RemoveArguments(3,ref args);
                    case "addlogout" :
                    return RemoveArguments(5,ref args);
                    case "removelogout" :
                    return RemoveArguments(4,ref args);
                    case "addlogin" :
                    return RemoveArguments(5,ref args);
                    case "removelogin" :
                    return RemoveArguments(4,ref args);
                    case "addlogoutnow" :
                    return RemoveArguments(4,ref args);
                    case "addactivetime" :
                    return RemoveArguments(5,ref args);
                    case "removeactivetime" :
                    return RemoveArguments(4,ref args);
                    case "listactivetime" :
                    return RemoveArguments(2,ref args);

                }
            }

            return false;
        }
        /// <summary>
        /// remove arguments after a position
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private bool RemoveArguments(int pos, ref string[] args)
        {
            var argsList = args.ToList();
            
            for (int i=pos;i<args.GetLength(0);i++){
                Console.WriteLine(@$"Ignoring extra parameter {argsList[i]}");
                argsList.RemoveAt(i);
            }
            args=argsList.ToArray();
            return true;
        }
    }
}