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

namespace QuantframeLib
{

    internal class Program
    {
        // Args 1:StoragePath, 2:SocketHost, 3:HttpServer
        static void Main(string[] args)
        {
            string storagePath;

            Tuple<string, int> SocketHost, HttpServer;
            if (args[0] != null && Directory.Exists(args[0]))
                storagePath = args[0];
            else
                storagePath = "Storage";

            if (args[1] != null)
                SocketHost = SpiltHost(args[1]);
            else
                SocketHost = new Tuple<string, int>("localhost", 9999);

            if (args[2] != null)
                HttpServer = SpiltHost(args[2]);
            else
                HttpServer = new Tuple<string, int>("localhost", 9998);

            // Validate the input
            if (storagePath == null || SocketHost == null || HttpServer == null)
            {
                Console.WriteLine("Invalid input. Please provide the correct input.");
                return;
            }
            // Initialize the server
            SocketServer.Initializer(SocketHost.Item1, SocketHost.Item2);
            // Initialize the log parser
            LogParserClient.Initializer();
            // Initialize the static data
            StaticData.Initializer(storagePath);
            // Initialize the http server
            HttpServerClient.Initializer(HttpServer.Item1, HttpServer.Item2);
            while (true) ;
        }
        public static Tuple<string, int> SpiltHost(string host)
        {
            if (!host.Contains(":"))
                return null;
            string[] hostParts = host.Split(':');
            return new Tuple<string, int>(hostParts[0], int.Parse(hostParts[1]));
        }
    }

}
