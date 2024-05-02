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
        static void Main(string[] args)
        {
            SocketServer.Initializer("localhost", 7891);
            LogParserClient.Initializer();
            StaticData.Initializer("a");
            HttpServerClient.Initializer("localhost", 7890);
            while (true) ;
        }
    }
}
