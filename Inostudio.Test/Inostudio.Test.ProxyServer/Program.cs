﻿using Inostudio.Test.Core;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Inostudio.Test.ProxyServer
{
    class Program
    {
        static void Main(string[] args)
        {
            //init params
            var hostName = args.Length > 0 ? args[0] : PROXY_HOST_NAME;
            var port = args.Length > 1 && int.TryParse(args[1], out int possiblePort) ? possiblePort : PROXY_PORT;

            //create server

            var server = new ProxyServer(hostName, port);
            Task serverTask;
            var cts = new CancellationTokenSource();

            Console.WriteLine(TextConstants.ResourceManager.GetString("SeparatorLine"));
            Console.WriteLine(TextConstants.ResourceManager.GetString("WelcomeMessage"));
            Console.WriteLine(TextConstants.ResourceManager.GetString("SeparatorLine"));
            Console.WriteLine(TextConstants.ResourceManager.GetString("AcceptedCommandsList"));
            Console.WriteLine(TextConstants.ResourceManager.GetString("SeparatorLine"));

            //take commands until EXIT command
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine(TextConstants.ResourceManager.GetString("CommandWait"));
                var command = Console.ReadLine().ToLower();

                switch (command)
                {
                    case "start":
                        serverTask = server.StartAsync(cts.Token);
                        Console.WriteLine(TextConstants.ResourceManager.GetString("StartServer"));
                        break;
                    case "stop":
                        server.StopAsync(cts.Token).Wait();
                        Console.WriteLine(TextConstants.ResourceManager.GetString("StopServer"));
                        break;
                    case "exit":
                        server.StopAsync(cts.Token).Wait();
                        exit = true;
                        break;
                    default:
                        Console.WriteLine(TextConstants.ResourceManager.GetString("WrongCommand"));
                        Console.WriteLine(TextConstants.ResourceManager.GetString("AcceptedCommandsList"));
                        break;
                }

                Console.WriteLine(TextConstants.ResourceManager.GetString("SeparatorLine"));
            }
        }

        #region private constants
        private static string PROXY_HOST_NAME = "127.0.0.1";
        private static int PROXY_PORT = 8888;
        #endregion
    }
}
