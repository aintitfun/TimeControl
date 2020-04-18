namespace Common
{
    public class AppsPersist 
    {        
        public string app;
        public int maxTime;
        public int command;
        public AppsPersist( )
        {

        }
        public AppsPersist(string app,int maxTime )
        {
            this.app= app;
            this.maxTime= maxTime;
            
        }
    }
}