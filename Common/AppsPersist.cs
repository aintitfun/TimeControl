namespace Common
{
    public class AppsPersist 
    {        
        public int command;
        public string app;
        public string userName;
        public int time;
        

        public AppsPersist( )
        {
            
        }
        public AppsPersist(string app,string userName,int time )
        {
            this.app= app;
            this.userName=userName;
            this.time= time;
        }
    }
}