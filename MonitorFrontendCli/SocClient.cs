using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Generic;
using Common;


namespace MonitorFrontendCli
{

     public enum Command {
            add=1,
            remove=2,
            list=3,
            stats=4
        }
    public class SocClient
    {

        System.Net.Sockets.Socket sender;

        public void Connect(string strHostname)
        {


            
            //IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            //IPAddress ipAddr = IPAddress.Parse(strIP);
            
            sender = new System.Net.Sockets.Socket(SocketType.Stream, ProtocolType.Tcp);

            try
            {
                sender.Connect(strHostname, 20100);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Disconnect()
        {
            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
        }

        public List<AppsPersist> SendMessage(string app, string username,int maxTime, int command)
        {

            try
            {
                AppsPersist message = new AppsPersist();
                message.app = app;
                message.maxTime = maxTime;
                message.command = command;
                message.username = username;
                string mJSon = JsonConvert.SerializeObject(message);


             
                byte[] messageSent = Encoding.ASCII.GetBytes(mJSon + "<EOF>");
                int byteSent = sender.Send(messageSent);

                if (message.command ==  (int) Command.add )
                {

                 
                    byte[] messageReceived = new byte[10000];
                    int byteRecv = sender.Receive(messageReceived);
                    String data = Encoding.ASCII.GetString(messageReceived,
                                                    0, byteRecv);

                    
                    List<AppsPersist> lap = JsonConvert.DeserializeObject<List<AppsPersist>>(data.Replace("<EOF>", ""));

                    return lap;

                }

                if (message.command ==  (int) Command.remove )
                {

                 
                    byte[] messageReceived = new byte[10000];
                    int byteRecv = sender.Receive(messageReceived);
                    String data = Encoding.ASCII.GetString(messageReceived,
                                                    0, byteRecv);

                    
                    List<AppsPersist> lap = JsonConvert.DeserializeObject<List<AppsPersist>>(data.Replace("<EOF>", ""));

                    return lap;

                }

                if (message.command ==  (int) Command.list )
                {

                 
                    byte[] messageReceived = new byte[10000];
                    int byteRecv = sender.Receive(messageReceived);
                    String data = Encoding.ASCII.GetString(messageReceived,
                                                    0, byteRecv);

                    
                    List<AppsPersist> lap = JsonConvert.DeserializeObject<List<AppsPersist>>(data.Replace("<EOF>", ""));

                    return lap;

                }
                if (message.command == (int) Command.stats)
                {
                    byte[] messageReceived = new byte[1024];
                    int byteRecv = sender.Receive(messageReceived);
                    String data = Encoding.ASCII.GetString(messageReceived, 0, byteRecv);


                    List<AppsPersist> lap = JsonConvert.DeserializeObject<List<AppsPersist>>(data.Replace("<EOF>", ""));

                    return lap;


                }
                return new List<AppsPersist>();
            }
            catch (ArgumentNullException ane)
            {

                Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                return new List<AppsPersist>();
            }

            catch (SocketException se)
            {

                Console.WriteLine("SocketException : {0}", se.ToString());
                return new List<AppsPersist>();
            }

            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
                return new List<AppsPersist>();
            }


        }


    }

}






