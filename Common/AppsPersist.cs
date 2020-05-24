namespace Common
{
    public class AppsPersist 
    {        
        public int command;
        public string app;
        public string username;
        public int maxTime;
        

        public AppsPersist( )
        {
            
        }
        public AppsPersist(string app,string username,int maxTime )
        {
            this.app= app;
            this.username=username;
            this.maxTime= maxTime;
        }
    }
}