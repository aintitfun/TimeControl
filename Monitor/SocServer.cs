using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using Common;
using System.Collections.Generic;
namespace Monitor
{
    public class SocServer
    {

        public SocServer()
        {

        }

        /// <summary>
        /// Returns the first valid InterNetwork interface to bind to a local network ip
        /// </summary>
        /// <returns></returns>
        public static IPAddress GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip;
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
        public void CreateServer()
        {
            IPAddress ipAddr = GetLocalIPAddress();
            IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 20100);

            Socket listener = new Socket(ipAddr.AddressFamily,
                         SocketType.Stream, ProtocolType.Tcp);
            ProcessSQL vSQL = new ProcessSQL();

            Logger logger =new Logger();

            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);
                while (true)
                {
                    Console.WriteLine("Waiting connection ... ");
                    Socket clientSocket = listener.Accept();
                    Console.WriteLine("se han conectado");
                    byte[] bytes = new Byte[10000];
                    string data = null;
                    while (true)
                    {
                        int numByte = clientSocket.Receive(bytes);
                        data += Encoding.ASCII.GetString(bytes,
                                                   0, numByte);
                        Console.WriteLine(data);
                        if (data.IndexOf("<EOF>") > -1)
                            break;

                    }//----------------------------------------------------------------------------

                        var jsonResult = JsonConvert.DeserializeObject<AppsPersist>(data.Replace("<EOF>",""));

                        if (jsonResult.command==(int)Command.add)
                        {

                            if (!vSQL.AddApplication(jsonResult.app, jsonResult.userName,jsonResult.time))
                                SendMessage(new List<AppsPersist>{ new AppsPersist($@"{DateTime.Now} [ERROR] Adding app {jsonResult.app} for {jsonResult.userName}","",-1)},clientSocket);
                            else
                                SendMessage(new List<AppsPersist>{ new AppsPersist($@"{DateTime.Now} [ADDED]: app {jsonResult.app} to list","",0)},clientSocket);
                                

                        }

                        if (jsonResult.command==(int)Command.remove)
                        {
                            if (jsonResult.app != "null")
                            {
                                if (!vSQL.RemoveApplicationFromUser(jsonResult.app,jsonResult.userName))
                                    SendMessage(new List<AppsPersist>{ new AppsPersist("Unable to remove app as db is busy, please retry...","",-2)},clientSocket);
                                else
                                    SendMessage(new List<AppsPersist>{ new AppsPersist($@"{DateTime.Now} [REMOVED]: app {jsonResult.app} from user {jsonResult.userName}","",0)},clientSocket);
                            }
                            
                            

                        }

                        if (jsonResult.command==(int)Command.list)
                        {
                            List<AppsPersist> lap=new List<AppsPersist>();
                            lap = vSQL.GetApps();
                            
                            SendMessage(lap,clientSocket);
                        
                        }

                        if (jsonResult.command==(int)Command.listlogouts)
                        {
                            List<AppsPersist> lap=new List<AppsPersist>();
                            lap = vSQL.GetConfiguredLogouts();
                            
                            SendMessage(lap,clientSocket);
                        
                        }
                        if (jsonResult.command==(int)Command.listlogins)
                        {
                            List<AppsPersist> lap=new List<AppsPersist>();
                            lap = vSQL.GetConfiguredLogins();
                            
                            SendMessage(lap,clientSocket);
                        
                        }
                        if (jsonResult.command==(int)Command.addlogout)
                        {
                            if (vSQL.AddLogout(jsonResult.userName,jsonResult.time))
                                SendMessage(new List<AppsPersist>{ new AppsPersist($@"{DateTime.Now} [INFO]: Added logout {jsonResult.userName} with {jsonResult.time}","",0)},clientSocket);
                            else 
                            {
                                SendMessage(new List<AppsPersist>{ new AppsPersist($@"{DateTime.Now} [ERROR]: adding logout {jsonResult.userName} {jsonResult.time}","",0)},clientSocket);
                            }
                            
                        }
                        if (jsonResult.command==(int)Command.addlogoutnow)
                        {
                            if (vSQL.AddLogoutNow(jsonResult.userName))
                                SendMessage(new List<AppsPersist>{ new AppsPersist($@"{DateTime.Now} [INFO]: Added logoutnow {jsonResult.userName}","",0)},clientSocket);
                            else 
                            {
                                SendMessage(new List<AppsPersist>{ new AppsPersist($@"{DateTime.Now} [ERROR]: adding logoutnow {jsonResult.userName}","",0)},clientSocket);
                            }
                            
                        }
                        if (jsonResult.command==(int)Command.addlogin)
                        {
                            if (vSQL.AddLogin(jsonResult.userName,jsonResult.time))
                                SendMessage(new List<AppsPersist>{ new AppsPersist($@"{DateTime.Now} [INFO]: Added login {jsonResult.userName} with {jsonResult.time}","",0)},clientSocket);
                            else 
                            {
                                SendMessage(new List<AppsPersist>{ new AppsPersist($@"{DateTime.Now} [ERROR]: adding login {jsonResult.userName} {jsonResult.time}","",0)},clientSocket);
                            }
                            
                        }
                        if (jsonResult.command==(int)Command.removelogout)
                        {
                            if (vSQL.RemoveLogout(jsonResult.userName))
                                SendMessage(new List<AppsPersist>{ new AppsPersist($@"{DateTime.Now} [INFO]: Removed logout {jsonResult.userName}","",0)},clientSocket);
                            else 
                            {
                                SendMessage(new List<AppsPersist>{ new AppsPersist($@"{DateTime.Now} [ERROR]: Removing logout {jsonResult.userName}","",0)},clientSocket);
                            }
                        }
                        if (jsonResult.command==(int)Command.removelogin)
                        {
                            if (vSQL.RemoveLogin(jsonResult.userName))
                                SendMessage(new List<AppsPersist>{ new AppsPersist($@"{DateTime.Now} [INFO]: Removed login {jsonResult.userName}","",0)},clientSocket);
                            else 
                            {
                                SendMessage(new List<AppsPersist>{ new AppsPersist($@"{DateTime.Now} [ERROR]: Removing login {jsonResult.userName}","",0)},clientSocket);
                            }
                        }
                        if (jsonResult.command==(int)Command.addactivetime)
                        {
                            if (vSQL.AddActiveTime(jsonResult.userName, jsonResult.time))
                                SendMessage(new List<AppsPersist>{ new AppsPersist($@"{DateTime.Now} [INFO]: Added Active Time {jsonResult.userName}","",0)},clientSocket);
                            else 
                            {
                                SendMessage(new List<AppsPersist>{ new AppsPersist($@"{DateTime.Now} [ERROR]: Adding Active Time {jsonResult.userName}","",0)},clientSocket);
                            }
                        }
                        if (jsonResult.command==(int)Command.listactivetime)
                        {
                            
                            List<AppsPersist> lap=new List<AppsPersist>();
                            lap = vSQL.ListActiveTime();
                            
                            SendMessage(lap,clientSocket);

                           /* 
                            if (vSQL.ListActiveTime())
                                SendMessage(new List<AppsPersist>{ new AppsPersist($@"{DateTime.Now} [INFO]: Added Active Time {jsonResult.userName}","",0)},clientSocket);
                            else 
                            {
                                SendMessage(new List<AppsPersist>{ new AppsPersist($@"{DateTime.Now} [ERROR]: Adding Active Time {jsonResult.userName}","",0)},clientSocket);
                            }*/
                        }
                        if (jsonResult.command==(int)Command.removeactivetime)
                        {
                            if (vSQL.RemoveActiveTime(jsonResult.userName))
                                SendMessage(new List<AppsPersist>{ new AppsPersist($@"{DateTime.Now} [INFO]: Removed active time for {jsonResult.userName}","",0)},clientSocket);
                            else 
                            {
                                SendMessage(new List<AppsPersist>{ new AppsPersist($@"{DateTime.Now} [ERROR]: Removing active time for {jsonResult.userName}","",0)},clientSocket);
                            }
                        }
                        if (jsonResult.command==(int)Command.stats)
                        {
                            SendMessage(vSQL.GetCurrentDayAppUsage(),clientSocket);
                        }
                    }
           
            }

            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void SendMessage(List<AppsPersist> lap, Socket clientSocket)
        {
            string mJSon = JsonConvert.SerializeObject(lap);
            byte[] messageSent = Encoding.ASCII.GetBytes(mJSon+"<EOF>");
            int byteSent = clientSocket.Send(messageSent);
        }
    }



}
