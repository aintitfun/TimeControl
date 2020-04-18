namespace MonitorFrontendCli
{
    public class InvocationParameters
    {
        public string command;
        public string host;
        public string app;
        public int maxTime;
        public InvocationParameters(string inCommand, string  inHost, string inApp, int inMaxTime) : this(inCommand,  inHost, inApp)
        {
            maxTime=inMaxTime;
        }
        public InvocationParameters(string inCommand, string  inHost, string inApp) : this(inCommand,inHost)
        {
            app=inApp;
        }
        public InvocationParameters(string inCommand, string  inHost){
            command=inCommand;
            host=inHost;
        }
    }
}