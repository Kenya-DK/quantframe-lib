﻿using SimpleHTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
        public static void Initializer(string hostname, int port)
        {
            _token = new CancellationTokenSource();
            _server = new SimpleHTTP.HttpServer(port, _token.Token);
            _server.Routes.OnBefore = (rq, rp) => { Console.WriteLine($"Requested: {rq.Url.PathAndQuery}"); return false; };
            _server.Routes.Add("/", (rq, rp, args) =>
            {
                rp.WithCORS().AsText("asd");
            });
            _server.Routes.Add("/stock/addriven", (rq, rp, args) =>
            {
                var asd = rq.GetBodyAsString();
                Console.WriteLine(asd);
                rp.WithCORS().AsText("{}");
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
