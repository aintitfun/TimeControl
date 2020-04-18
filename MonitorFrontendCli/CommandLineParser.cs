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
            if (ParseCommand(args[0], ref args))
            {
                switch (args.GetLength(0)){
                    case 1:
                        return new InvocationParameters(null,null);
                    case 2:
                        return ParseListStats(args[0],args[1]);
                    case 3:
                        return ParseRemove(args[0],args[1],args[2]);
                    case 4:
                        return ParseAdd(args[0],args[1],args[2],System.Convert.ToInt32(args[3]));

               
                }

            } 

            return new InvocationParameters(null,null);

        }


        // the next Parse methods are to customice the check for each operation (nothing now at this moment)
        public InvocationParameters ParseListStats(string inCommand,string inHost)
        {
            return new InvocationParameters(inCommand,inHost);
        }

        public InvocationParameters ParseRemove(string inCommand,string inHost, string inApp)
        {
            return new InvocationParameters(inCommand,inHost,inApp);
        }

        public InvocationParameters ParseAdd(string inCommand,string inHost, string inApp,int inMaxTime)
        {
            return new InvocationParameters(inCommand,inHost,inApp,inMaxTime);
        }

        /// <summary>
        /// Check that command exists, also remove non used parameters
        /// </summary>
        /// <param name="inCommand"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public bool ParseCommand(string inCommand, ref string[] args)
        {
            if (inCommand.StartsWith("-"))
            {
                switch(inCommand.Substring(1))
                {
                    case "add":
                    return RemoveArguments(4,ref args);
                    case "list" :
                    return RemoveArguments(2,ref args);
                    case "stats":
                    return RemoveArguments(2,ref args);
                    case "remove":
                    return RemoveArguments(3,ref args);
                 
                }
            }

            return false;
        }
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