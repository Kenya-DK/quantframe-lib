﻿using QuantframeLib.HttpServer.Types;
using QuantframeLib.Socket;
using QuantframeLib.Socket.Types;
using QuantframeLib.Utils;
using SimpleHTTP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WatsonWebsocket;

namespace QuantframeLib.HttpServer
{
    public class HttpServerClient
    {
        #region Const/Static Values
        #endregion
        #region Private Values
        private static SimpleHTTP.HttpServer _server;
        private static CancellationTokenSource _token;
        #endregion
        #region New
        public static string GetItemType(string displayName)
        {
            string type = "";

            return type;
        }

        public static void Initializer(string hostname, int port)
        {
            _token = new CancellationTokenSource();
            _server = new SimpleHTTP.HttpServer(port, _token.Token);
            _server.Routes.OnBefore = (rq, rp) =>
            {
                Logger.Info("HttpServerClient:OnBefore", $"Requested: {rq.Url.PathAndQuery}");
                return false;
            };
            _server.Routes.Add("/clients", (rq, rp, args) =>
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("[");
                foreach (var item in SocketServer.Clients)
                {

                }
                sb.AppendLine("[");
                rp.WithCORS().AsText(sb.ToString());
            });
            _server.Routes.Add("/", (rq, rp, args) =>
            {
                rp.WithCORS().AsText("asd");
            });
            _server.Routes.Add("/test", (rq, rp, args) =>
            {
                string data = File.ReadAllText(@"C:\Users\Kenni Kristiansen\Downloads\Fivem\quantframe-api\backend\target\trades.json");
                TypedProduct[] trades = Misc.Deserialize<TypedProduct[]>(data);
                StringBuilder sb = new StringBuilder();
                foreach (var trade in trades)
                {
                    foreach (var item in trade.Offerings)
                        sb.AppendLine($"{item.DisplayName}: {GetItemType(item.DisplayName)}");
                    foreach (var item in trade.Receiving)
                        sb.AppendLine($"{item.DisplayName}: {GetItemType(item.DisplayName)}");
                }
                rp.WithCORS().AsText(sb.ToString());
            });
            _server.Routes.Add("/stock/add_riven_alecaframe", (rq, rp, args) =>
            {
                try
                {
                    string payload = rq.GetBodyAsString();
                    AddAlecaFrameRiven eventObj = Misc.Deserialize<AddAlecaFrameRiven>(payload);
                    SocketServer.Send("OnAddRivenAlecaFrame", eventObj.ToJson());
                    rp.WithCORS().AsText("{}");
                }
                catch (Exception ex)
                {
                    Logger.Error("SocketServer:OnMessageReceived", "Invalid message received: " + ex.Message);
                    rp.WithCORS().AsText(ex.Message);
                }
            }, "POST");
            _server.Routes.Add("/stock/add_riven", (rq, rp, args) =>
            {
                try
                {
                    string payload = rq.GetBodyAsString();
                    AddStockRiven eventObj = Misc.Deserialize<AddStockRiven>(payload);
                    //SocketServer.Send("OnAddStockRiven", eventObj.ToJson());
                    rp.WithCORS().AsText("{}");
                }
                catch (Exception ex)
                {
                    Logger.Error("SocketServer:OnMessageReceived", "Invalid message received: " + ex.Message);
                    rp.WithCORS().AsText(ex.Message);
                }
            }, "POST");
            _server.Routes.Add("/stock/add_item", (rq, rp, args) =>
            {
                try
                {
                    string payload = rq.GetBodyAsString();
                    AddStockItem eventObj = Misc.Deserialize<AddStockItem>(payload);
                    //SocketServer.Send("OnAddItem", eventObj.ToJson());
                    rp.WithCORS().AsText("{}");
                }
                catch (Exception ex)
                {
                    Logger.Error("SocketServer:OnMessageReceived", "Invalid message received: " + ex.Message);
                    rp.WithCORS().AsText(ex.Message);
                }
            }, "POST");
            _server.Start();
        }
        #endregion
        #region Method

        #endregion
        #region Override Method
        /// <summary>
        /// Override ToString for <see cref="HttpServerClient"/>
        /// </summary>
        /// <returns>
        /// Returns store data in string.
        /// </returns>
        public override string ToString()
        {
            return base.ToString();
        }
        #endregion
        #region Method Get Set

        #endregion
        #region Events

        #endregion
    }
}
