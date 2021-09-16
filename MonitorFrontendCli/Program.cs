using System;
using TimeControl.Common;
using System.Collections.Generic;

namespace TimeControl.MonitorFrontendCli
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandLineParser cl=new CommandLineParser();

            if (args.Length!=0){

                SocClient socClient=new SocClient();
                List<AppsPersist> lap;
                InvocationParameters invocationParameters=cl.Parse(args);

                if (invocationParameters.command!=null)
                {
                    switch (invocationParameters.command)
                    {
                        case "-addapp":
                            socClient.Connect(args[1]);
                            lap=socClient.SendMessage(args[2],args[3],System.Convert.ToInt32(args[4]),(int)Command.addapp,args[5]);
                            socClient.Disconnect();
                            foreach(AppsPersist a in lap){
                                Console.WriteLine(a._app);
                            }
                        break;
                        case "-removeapp":
                            socClient.Connect(args[1]);
                            lap=socClient.SendMessage(args[2],args[3],0,(int)Command.removeapp,args[4]);
                            socClient.Disconnect();
                            foreach(AppsPersist a in lap){
                                Console.WriteLine(a._app);
                            }
                        break;
                        case "-listapps":
                            socClient.Connect(args[1]);
                            lap= socClient.SendMessage("null","null",0,(int)Command.listapps,"null");
                            foreach(AppsPersist a in lap){
                                Console.WriteLine(a._app+" "+" "+a._userName+" "+a._time+ " "+ a._dayOfTheWeek);
                            }
                            //Console.ReadLine();
                            socClient.Disconnect();
                        break;
                        case "-stats":
                            socClient.Connect(args[1]);
                            lap=socClient.SendMessage("null","null",0,(int) Command.stats,"null");
                            foreach(AppsPersist a in lap){
                                Console.WriteLine(a._app+" "+" "+a._userName+" "+a._time);
                            }
                            socClient.Disconnect();
                        break;
                        case "-addlogout":
                            socClient.Connect(args[1]);
                            lap= socClient.SendMessage(null,args[2],System.Convert.ToInt32(args[3].Replace(":","")),(int) Command.addlogout,args[4]);
                            foreach(AppsPersist a in lap){
                                Console.WriteLine(a._app);
                            }
                            socClient.Disconnect();
                        break;
                        case "-addlogin":
                            socClient.Connect(args[1]);
                            lap= socClient.SendMessage(null,args[2],System.Convert.ToInt32(args[3].Replace(":","")),(int) Command.addlogin, args[4]);
                            foreach(AppsPersist a in lap){
                                Console.WriteLine(a._app);
                            }
                            socClient.Disconnect();
                        break;
                        case "-addlogoutnow":
                            socClient.Connect(args[1]);
                            lap= socClient.SendMessage(null,args[2],0,(int) Command.addlogoutnow,"null");
                            foreach(AppsPersist a in lap){
                                Console.WriteLine(a._app);
                            }
                            socClient.Disconnect();
                        break;
                        case "-listlogouts":
                            socClient.Connect(args[1]);
                            lap=socClient.SendMessage("null","null",0,(int) Command.listlogouts, "null");
                            string maxTimeTmp;
                            foreach(AppsPersist a in lap){
                                maxTimeTmp=a._time.ToString().PadLeft(4,'0');
                                Console.WriteLine(a._userName+" "+maxTimeTmp.Substring(0,2)+":"+maxTimeTmp.Substring(2,2)+ " "+a._dayOfTheWeek);
                            }
                            socClient.Disconnect();
                        break;
                        case "-listlogins":
                            socClient.Connect(args[1]);
                            lap=socClient.SendMessage("null","null",0,(int) Command.listlogins, "null");
                            string minTimeTmp;
                            foreach(AppsPersist a in lap){
                                minTimeTmp=a._time.ToString().PadLeft(4,'0');
                                Console.WriteLine(a._userName+" "+minTimeTmp.Substring(0,2)+":"+minTimeTmp.Substring(2,2) + " " + a._dayOfTheWeek);
                            }
                            socClient.Disconnect();
                        break;
                        case "-removelogout":
                            socClient.Connect(args[1]);
                            lap=socClient.SendMessage("null",args[2],0,(int) Command.removelogout, args[3]);
                            foreach(AppsPersist a in lap){
                                Console.WriteLine(a._app);
                            }
                            socClient.Disconnect();
                        break;
                        case "-removelogin":
                            socClient.Connect(args[1]);
                            lap=socClient.SendMessage("null",args[2],0,(int) Command.removelogin, args[3]);
                            foreach(AppsPersist a in lap){
                                Console.WriteLine(a._app);
                            }
                            socClient.Disconnect();
                        break;
                        case "-addactivetime":
                            socClient.Connect(args[1]);
                            lap=socClient.SendMessage("null",args[2],System.Convert.ToInt32(args[3]),(int)Command.addactivetime, args[4]);
                            socClient.Disconnect();
                            foreach(AppsPersist a in lap){
                                Console.WriteLine(a._app);
                            }
                        break;
                        case "-removeactivetime":
                            socClient.Connect(args[1]);
                            lap=socClient.SendMessage("null",args[2],0,(int)Command.removeactivetime, args[3]);
                            socClient.Disconnect();
                            foreach(AppsPersist a in lap){
                                Console.WriteLine(a._app);
                            }
                        break;
                        case "-listactivetime":
                            socClient.Connect(args[1]);
                            lap=socClient.SendMessage("null","null",0,(int)Command.listactivetime, "null");
                            socClient.Disconnect();
                            foreach(AppsPersist a in lap){
                                Console.WriteLine(a._userName+" "+a._time+ " "+a._dayOfTheWeek);
                            }
                        break;


                    }
                }
                else
                    Console.WriteLine("Error on parameters...");
                
            }
            
        }
    }
}
