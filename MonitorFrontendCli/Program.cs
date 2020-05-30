﻿using System;
using Common;
using System.Collections.Generic;

namespace MonitorFrontendCli
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
                        case "-add":
                            socClient.Connect(args[1]);
                            lap=socClient.SendMessage(args[2],args[3],System.Convert.ToInt32(args[4]),(int)Command.add);
                            socClient.Disconnect();
                            foreach(AppsPersist a in lap){
                                Console.WriteLine(a.app+" "+a.maxTime);
                            }
                        break;
                        case "-remove":
                            socClient.Connect(args[1]);
                            lap=socClient.SendMessage(args[2],args[3],0,(int)Command.remove);
                            socClient.Disconnect();
                            foreach(AppsPersist a in lap){
                                Console.WriteLine(a.app+" "+a.maxTime);
                            }
                        break;
                        case "-list":
                            socClient.Connect(args[1]);
                            lap= socClient.SendMessage("null","null",0,(int)Command.list);
                            foreach(AppsPersist a in lap){
                                Console.WriteLine(a.app+" "+" "+a.username+" "+a.maxTime);
                            }
                            //Console.ReadLine();
                            socClient.Disconnect();
                        break;
                        case "-stats":
                            socClient.Connect(args[1]);
                            lap=socClient.SendMessage("null","null",0,(int) Command.stats);
                            foreach(AppsPersist a in lap){
                                Console.WriteLine(a.app+" "+" "+a.username+" "+a.maxTime);
                            }
                            socClient.Disconnect();
                        break;
                        case "-addlogout":
                            socClient.Connect(args[1]);
                            lap= socClient.SendMessage(null,args[2],System.Convert.ToInt32(args[3].Replace(":","")),(int) Command.addlogout);
                            foreach(AppsPersist a in lap){
                                Console.WriteLine(a.username+" "+a.maxTime);
                            }
                            socClient.Disconnect();
                        break;
                        case "-listlogouts":
                            socClient.Connect(args[1]);
                            lap=socClient.SendMessage("null","null",0,(int) Command.listlogouts);
                            string maxTimeTmp;
                            foreach(AppsPersist a in lap){
                                maxTimeTmp=a.maxTime.ToString().PadLeft(4,'0');
                                Console.WriteLine(a.username+" "+maxTimeTmp.Substring(0,2)+":"+maxTimeTmp.Substring(2,2));
                            }
                            socClient.Disconnect();
                        break;
                        case "-removelogout":
                            socClient.Connect(args[1]);
                            lap=socClient.SendMessage("null",args[2],0,(int) Command.removelogout);
                            foreach(AppsPersist a in lap){
                                Console.WriteLine(a.app);
                            }
                            socClient.Disconnect();
                        break;
                    }
                }
                else
                {
                    Console.WriteLine("Error on parameters...");

                }
                
            }
            
        }
    }
}
