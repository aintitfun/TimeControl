namespace MonitorFrontendCli
{
    public class InvocationParameters
    {
        public string command;
        public string host;
        public string app;
        public int maxTime;
        public InvocationParameters(string command, string  host, string userName,string app, int maxTime) : this(command,  host, userName,app)
        {
            this.maxTime=maxTime;
        }
        public InvocationParameters(string command, string  host, string userName,string app) : this(command,host)
        {
            this.app=app;
        }
        public InvocationParameters(string command, string  host){
            this.command=command;
            this.host=host;
        }
    }
}