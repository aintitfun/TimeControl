using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using Common;
using System.Collections.Generic;
namespace Monitor
{
         public enum Command {
            add=1,
            remove=2,
            list=3,
            stats=4
        }
    public class SocServer
    {

        public SocServer()
        {

        }
        public void CreateServer()
        {
            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddr = ipHost.AddressList[0];
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

                            if (!vSQL.AddApplication(jsonResult.app, jsonResult.maxTime))
                                SendMessage(new List<AppsPersist>{ new AppsPersist("Unable to add app as db is busy, please retry...",-1)},clientSocket);
                            else
                                SendMessage(new List<AppsPersist>{ new AppsPersist($@"{DateTime.Now} [ADDED]: app {jsonResult.app} to list",0)},clientSocket);
                                
                                //logger.Log ($@"{DateTime.Now} [ADDED]: app {jsonResult.app} to list");

                        }

                        if (jsonResult.command==(int)Command.remove)
                        {
                            if (jsonResult.app != "null")
                            {
                                if (!vSQL.RemoveApplication(jsonResult.app))
                                    SendMessage(new List<AppsPersist>{ new AppsPersist("Unable to remove app as db is busy, please retry...",-2)},clientSocket);
                                else
                                    SendMessage(new List<AppsPersist>{ new AppsPersist($@"{DateTime.Now} [REMOVED]: app {jsonResult.app} to list",0)},clientSocket);



                                    //logger.Log ($@"{DateTime.Now} [REMOVED]: app {jsonResult.app} from list");
                            }
                            
                            

                        }

                        if (jsonResult.command==(int)Command.list)
                        {
                            List<AppsPersist> lap=new List<AppsPersist>();
                            lap = vSQL.GetApps();
                            
                            SendMessage(lap,clientSocket);
                        
                            logger.Log ($@"{DateTime.Now} [LIST]: Error listing apps ");
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
