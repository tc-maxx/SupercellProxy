using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Timers;

namespace SupercellProxy
{
    static class Proxy
    {
        public static List<Client> ClientPool = new List<Client>();

        // @FICTURE7 ;)
        static Proxy()
        {
            if (!Directory.Exists(Environment.CurrentDirectory + @"\\JsonPackets\\"))
                Directory.CreateDirectory(Environment.CurrentDirectory + @"\\JsonPackets\\");

            if (!Directory.Exists("Packets"))
                Directory.CreateDirectory("Packets");
        }
        
        /// <summary>
        /// Starts the proxy
        /// </summary>
        public static void Start()
        {
            try
            {
                // ASCII art
                Console.ForegroundColor = Helper.ChooseRandomColor();
                Logger.CenterStr(@"  _____                                 ____ ");
                Logger.CenterStr(@" / ___/__  ______  ___  _____________  / / / ");
                Logger.CenterStr(@" \__ \/ / / / __ \/ _ \/ ___/ ___/ _ \/ / /  ");
                Logger.CenterStr(@"  _/ / /_/ / /_/ /  __/ /  / /__/  __/ / /   ");
                Logger.CenterStr(@"/____/\__,_/ .___/\___/_/   \___/\___/_/_/   ");
                Logger.CenterStr(@"          /_/____                            ");
                Logger.CenterStr(@"            / __ \_________  _  ____  __      ");
                Logger.CenterStr(@"           / /_/ / ___/ __ \| |/_/ / / /      ");
                Logger.CenterStr(@"          / ____/ /  / /_/ />  </ /_/ /      ");
                Logger.CenterStr(@"         /_/   /_/   \____/_/|_|\__, /       ");
                Logger.CenterStr(@"                               /____/        ");
                Logger.CenterStr(@"                                             ");
                Logger.CenterStr(Helper.AssemblyVersion);
                Logger.CenterStr("Coded by expl0itr");
                Logger.CenterStr("Apache Version 2.0 License - © 2016");
                Logger.CenterStr("https://github.com/expl0itr/SupercellProxy/");
                Console.Write(Environment.NewLine);
                Console.ResetColor();

                // Show configuration values
                Console.Write(Environment.NewLine);
                Logger.CenterStr("Game: " + Config.Game.ReadableName());
                Logger.CenterStr("Host: " + Config.Host);
                Logger.CenterStr("JSON Logging: " + ((Config.JSON_Logging) ? "Yes" : "No"));
                Console.Write(Environment.NewLine);

                // Start the proxy
                Logger.Log("Starting the proxy..");

                // Get latest public key
                Keys.GetPublicKey();

                // Bind a new socket to the local EP     
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 9339);
                Socket ClientListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                ClientListener.Bind(endPoint);
                ClientListener.Listen(100);

                // Initialize the JSON Packets
                JSONPacketManager.Initialize();

                // Listen for connections
                Logger.Log("Connect to " + Helper.LocalNetworkIP + " now.");
                new Thread(() =>
                {
                    while (true)
                    {
                        Socket ClientSocket = ClientListener.Accept();
                        Client client = new Client(ClientSocket);
                        ClientPool.Add(client);

                        Logger.Log("Remote connection #" + (ClientPool.ToArray().Length) + " (" + ClientSocket.GetIP() + "), enqueuing..");
                        client.Enqueue();
                    }
                }).Start();
            }
            catch(Exception ex)
            {
                Logger.Log("Failed to start the proxy (" + ex.GetType() + ")!");        
            }
        }

        /// <summary>
        /// Stops the proxy
        /// </summary>
        public static void Stop()
        {
            for (int i = 0; i < ClientPool.Count; i++)
            {
                ClientPool[i].Dequeue();
            }
            ClientPool.Clear();
        }
    }
}
