using QuantframeLib.LogParser;
using QuantframeLib.Model;
using QuantframeLib.Socket;
using QuantframeLib.HttpServer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WatsonWebsocket;
using SimpleHTTP;
using QuantframeLib.Utils;
using System.Diagnostics;

namespace QuantframeLib
{

    internal class Program
    {

        private static string GetArgsByName(string name)
        {
            string[] args = Environment.GetCommandLineArgs();
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == name && args.Length > i + 1)
                    return args[i + 1];
            }
            return null;
        }

        // Args 1:StoragePath, 2:SocketHost, 3:HttpServer
        static void Main(string[] args)
        {

            string storagePath = GetArgsByName("--storage") ?? "storage";
            Tuple<string, int> SocketHost = SpiltHost(GetArgsByName("--socketHost")) ?? new Tuple<string, int>("localhost", 9999);
            Tuple<string, int> HttpServer = SpiltHost(GetArgsByName("--httpServer")) ?? new Tuple<string, int>("localhost", 9998);
            string keepAliveProcessName = GetArgsByName("--keepAlive");



            Logger.Info("Main", $"StoragePath: {storagePath} | SocketHost: {SocketHost.Item1}:{SocketHost.Item2} | HttpServer: {HttpServer.Item1}:{HttpServer.Item2} | KeepAlive: {keepAliveProcessName?? "N/A"}");

            // Initialize the static data
            StaticData.Initializer(storagePath);
            // Initialize the server
            SocketServer.Initializer(SocketHost.Item1, SocketHost.Item2);
            // Initialize the log parser
            LogParserClient.Initializer();
            // Initialize the http server
            HttpServerClient.Initializer(HttpServer.Item1, HttpServer.Item2);
            new Thread(() =>
            {
                while (true)
                {
                    Console.Title = "Connected Clients: " + SocketServer.Clients.Count();
                    if (!string.IsNullOrEmpty(keepAliveProcessName) && Process.GetProcessesByName(keepAliveProcessName).Length == 0)
                    {
                        Logger.Critical("Main", "Keep alive process is not running. Exiting...");
                        Environment.Exit(0);
                    }
                    Thread.Sleep(3000);
                }
            })
            {
                IsBackground = true
            }.Start();
            while (true) ;
        }
        public static Tuple<string, int> SpiltHost(string host)
        {
            if (host == null)
                return null;
            if (!host.Contains(":"))
                return null;
            string[] hostParts = host.Split(':');
            return new Tuple<string, int>(hostParts[0], int.Parse(hostParts[1]));
        }
    }

}
