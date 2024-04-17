using QuantframeLib.LogParser;
using QuantframeLib.Model;
using QuantframeLib.Socket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WatsonWebsocket;
namespace QuantframeLib
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SocketServer.Initializer("localhost", 7891);
            LogParserClient.Initializer();
            StaticData.Initializer("a");

            //using (WatsonWsClient wsc = new WatsonWsClient("localhost", 7891, false))
            //{
            //    wsc.ServerConnected += (s, e) => Console.WriteLine("Client connected to server");
            //    wsc.ServerDisconnected += (s, e) => Console.WriteLine("Client disconnected from server");
            //    wsc.MessageReceived += (s, e) => Console.WriteLine("Client received message from server: " + Encoding.UTF8.GetString(e.Data.ToArray()));
            //    wsc.AddCookie(new System.Net.Cookie("JWT", "eyJuYW1lIjoiQyMifQ==", "/", "localhost"));
            //    wsc.Start();

            //    Thread.Sleep(2500);

            //    Console.WriteLine("Sending message from client to server...");
            //    while (true)
            //    {
            //        //wsc.SendAsync("Hello from client").Wait();
            //        Random rnd = new Random();
            //        //SocketServer.Send("trade", rnd.Next(5, 2000).ToString());
            //        Thread.Sleep(2500);
            //    }
            //}
            while (true) ;
        }
    }
}
