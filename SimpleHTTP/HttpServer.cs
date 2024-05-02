using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleHTTP
{
    public class HttpServer
    {
        #region Const/Static Values
        #endregion
        #region Private Values
        private CancellationToken _token;
        private int _maxHttpConnectionCount;
        private HttpListener _listener;
        private readonly string _httpListenerPrefix;
        private readonly Routes _routes;
        #endregion
        #region New
        /// <summary>
        /// Creates and starts a new instance of the http(s) server.
        /// </summary>
        /// <param name="port">The http/https URI listening port.</param>
        /// <param name="token">Cancellation token.</param>
        /// <param name="onHttpRequestAsync">Action executed on HTTP request.</param>
        /// <param name="useHttps">True to add 'https://' prefix instead of 'http://'.</param>
        /// <param name="maxHttpConnectionCount">Maximum HTTP connection count, after which the incoming requests will wait (sockets are not included).</param>
        public HttpServer(int port, CancellationToken token, bool useHttps = false, int maxHttpConnectionCount = 64)
        {
            if (port < 0 || port > UInt16.MaxValue)
                throw new NotSupportedException($"The provided port value must in the range: [0..{UInt16.MaxValue}");

            if (token == null)
                throw new ArgumentNullException(nameof(token), "The provided token must not be null.");

            if (maxHttpConnectionCount < 1)
                throw new ArgumentException(nameof(maxHttpConnectionCount), "The value must be greater or equal than 1.");

            _token = token;
            var s = useHttps ? "s" : String.Empty;
            _maxHttpConnectionCount = maxHttpConnectionCount;
            _httpListenerPrefix = $"http{s}://localhost:{port}/";
            _routes = new Routes();
        }
        #endregion
        #region Method

        /// <summary>
        /// Start the server.
        /// </summary>
        public void Start()
        {
            _listener = new HttpListener();
            try { _listener.Prefixes.Add(_httpListenerPrefix); }
            catch (Exception ex) { throw new ArgumentException($"The provided prefix is not supported. Prefixes have the format: '{_httpListenerPrefix}", ex); }

            try { _listener.Start(); }
            catch (Exception ex) when ((ex as HttpListenerException)?.ErrorCode == 5)
            {
                var msg = GetNamespaceReservationExceptionMessage();
                throw new UnauthorizedAccessException(msg, ex);
            }
            new Thread(async () =>
            {
                using (var s = new SemaphoreSlim(_maxHttpConnectionCount))
                using (var r = _token.Register(() => _listener.Close()))
                {
                    bool shouldStop = false;
                    while (!shouldStop)
                    {
                        try
                        {
                            var ctx = await _listener.GetContextAsync();

                            if (ctx.Request.IsWebSocketRequest)
                            {
                                ctx.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                                ctx.Response.Close();
                            }
                            else
                            {
                                await s.WaitAsync();
                                //Task.Factory.StartNew(() => onHttpRequestAsync(ctx.Request, ctx.Response), TaskCreationOptions.None)
                                //            .ContinueWith(t => s.Release())
                                //            .Wait(0);
                            }
                        }
                        catch (Exception)
                        {
                            if (!_token.IsCancellationRequested)
                                throw;
                        }
                        finally
                        {
                            if (_token.IsCancellationRequested)
                                shouldStop = true;
                        }
                    }
                }
            })
            { IsBackground = true }.Start();
        }

        private string GetNamespaceReservationExceptionMessage()
        {
            var m = Regex.Match(_httpListenerPrefix, @"(?<protocol>\w+)://localhost:?(?<port>\d*)");

            string msg;
            if (m.Success)
            {
                var protocol = m.Groups["protocol"].Value;
                var port = m.Groups["port"].Value; if (String.IsNullOrEmpty(port)) port = 80.ToString();

                msg = $"The HTTP server can not be started, as the namespace reservation already exists.\n" +
                      $"Please run (elevated): 'netsh http delete urlacl url={protocol}://+:{port}/'.";
            }
            else
            {
                msg = $"The HTTP server can not be started, as the namespace reservation does not exist.\n" +
                      $"Please run (elevated): 'netsh http add urlacl url={_httpListenerPrefix} user=\"Everyone\"'.";
            }

            return msg;
        }
        #endregion
        #region Override Method
        /// <summary>
        /// Override ToString for <see cref="HttpServer"/>
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
        /// <summary>
        /// Adds a new route to the server.
        /// </summary>
        public
        #endregion
        #region Events

        #endregion
    }
}
