namespace Common
{
    public class AppsPersist 
    {        
        public int command;
        public string app;
        public string userName;
        public int maxTime;
        

        public AppsPersist( )
        {
            
        }
        public AppsPersist(string app,string userName,int maxTime )
        {
            this.app= app;
            this.userName=userName;
            this.maxTime= maxTime;
        }
    }
}