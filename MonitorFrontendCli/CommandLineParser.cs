using System;
using System.Linq;

namespace MonitorFrontendCli
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
                        if (args[0]=="-removelogout")
                            return ParseRemoveLogout(args[0],args[1],args[2]);
                        if (args[0]=="-removelogin")
                            return ParseRemoveLogin(args[0],args[1],args[2]);
                        if (args[0]=="-addlogoutnow")
                            return ParseAddLogoutNow(args[0],args[1],args[2]);
                        if (args[0]=="-removeactivetime")
                            return ParseRemoveActiveTime(args[0],args[1],args[2]);

                        break;
                    case 4:
                        if (args[3].Contains(":"))
                        {
                            if (args[0]=="-addlogout")
                                return ParseAddLogout(args[0],args[1],args[2],args[3]);
                            if (args[0]=="-addlogin")
                                return ParseAddLogin(args[0],args[1],args[2],args[3]);
                        }
                        else
                        {
                            if (args[0]=="-remove")
                                return ParseRemove(args[0],args[1],args[2],args[3]);
                            if (args[0]=="-addactivetime")
                                return ParseAddActiveTime(args[0],args[1],args[2],System.Convert.ToInt32(args[3]));
                        }

                        break;
                    case 5:
                        return ParseAdd(args[0],args[1],args[2],args[3],System.Convert.ToInt32(args[4]));
                }

            } 

            return new InvocationParameters(null,null);

        }

        // the next Parse methods are to customice the check for each operation (nothing now at this moment)
        public InvocationParameters ParseList(string command,string host)
        {
            return new InvocationParameters(command,host);
        }

        public InvocationParameters ParseRemove(string command,string host, string userName,string app)
        {
            return new InvocationParameters(command,host,userName,app);
        }

        public InvocationParameters ParseAddLogout(string command,string host, string userName,string hourMin)
        {
            return new InvocationParameters(command,host,userName,System.Convert.ToInt32(hourMin.Replace(":","")));
        }

        public InvocationParameters ParseAddLogin(string command,string host, string userName,string hourMin)
        {
            return new InvocationParameters(command,host,userName,System.Convert.ToInt32(hourMin.Replace(":","")));
        }

        public InvocationParameters ParseAddLogoutNow(string command,string host, string userName)
        {
            return new InvocationParameters(command,host,userName);
        }
        public InvocationParameters ParseAdd(string command,string host, string userName,string app,int maxTime)
        {
            return new InvocationParameters(command,host,userName,app,maxTime);
        }

        public InvocationParameters ParseRemoveLogout(string command,string host, string userName)
        {
            return new InvocationParameters(command,host,userName);
        }

        public InvocationParameters ParseRemoveLogin(string command,string host, string userName)
        {
            return new InvocationParameters(command,host,userName);
        }
        public InvocationParameters ParseAddActiveTime(string command,string host, string userName, int maxTime)
        {
            return new InvocationParameters(command,host,userName,maxTime);
        }
        public InvocationParameters ParseRemoveActiveTime(string command,string host, string userName)
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
                    case "add":
                    return RemoveArguments(5,ref args);
                    case "list" :
                    return RemoveArguments(2,ref args);
                    case "stats":
                    return RemoveArguments(2,ref args);
                    case "remove":
                    return RemoveArguments(4,ref args);
                    case "listlogouts" :
                    return RemoveArguments(2,ref args);
                    case "listlogins" :
                    return RemoveArguments(2,ref args);
                    case "addlogout" :
                    return RemoveArguments(4,ref args);
                    case "removelogout" :
                    return RemoveArguments(3,ref args);
                    case "addlogin" :
                    return RemoveArguments(4,ref args);
                    case "removelogin" :
                    return RemoveArguments(3,ref args);
                    case "addlogoutnow" :
                    return RemoveArguments(3,ref args);
                    case "addactivetime" :
                    return RemoveArguments(4,ref args);
                    case "removeactivetime" :
                    return RemoveArguments(3,ref args);
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
                 argsList.RemoveAt(i);
            }
            args=argsList.ToArray();
            return true;
        }
    }
}