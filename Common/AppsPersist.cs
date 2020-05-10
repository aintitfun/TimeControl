namespace Common
{
    public class AppsPersist 
    {        
        public string app;
        public int maxTime;
        public int command;
        public string username;
        public AppsPersist(string app,string username,int maxTime )
        {
            this.app= app;
            this.maxTime= maxTime;
            this.username=username;
        }
    }
}