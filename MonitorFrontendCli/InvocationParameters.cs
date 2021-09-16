namespace TimeControl.MonitorFrontendCli
{
    public class InvocationParameters
    {
        public string command;
        public string host;
        public string app;
        public int maxTime;
        public string userName;
        public string dayOfTheWeek;
        public InvocationParameters(string command, string  host, string userName,string app, int maxTime, string dayOfTheWeek) : this(command,  host, userName,app)
        {
            this.maxTime=maxTime;
            this.dayOfTheWeek = dayOfTheWeek;
        }
        public InvocationParameters(string command, string  host, string userName,string app) : this(command,host)
        {
            this.app=app;
            this.userName=userName;
        }
        public InvocationParameters(string command, string  host, string userName) : this(command,host)
        {
            this.userName=userName;
        }
        public InvocationParameters(string command, string  host){
            this.command=command;
            this.host=host;
        }
        public InvocationParameters(string command, string  host, string userName, int maxTime) : this(command,  host)
        {
            this.userName=userName;
            this.maxTime=maxTime;
        }

    }
}