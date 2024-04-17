using QuantframeLib.Socket.Types;
using QuantframeLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using WatsonWebsocket;
namespace QuantframeLib.Socket
{
    public class SocketServer
    {
        #region Const/Static Values
        #endregion
        #region Private Values
        static WatsonWsServer _wss;
        static List<JWTPayload> _clients = new List<JWTPayload>();

        #endregion
        #region New

        public static void Initializer(string hostname, int port)
        {
            _wss = new WatsonWsServer(hostname, port, false);
            _wss.ClientConnected += OnClientConnected;
            _wss.ClientDisconnected += OnClientDisconnected;
            _wss.MessageReceived += OnMessageReceived;
            _wss.Start();
            new Thread(() =>
            {
                while (true)
                {
                    Console.Title = "Connected Clients: " + _wss.ListClients().Count();
                    Thread.Sleep(3000);
                }
            })
            {
                IsBackground = true
            }.Start();
        }
        #endregion
        #region Override Method

        #endregion
        #region Method
        /// <summary>
        /// Sends a JSON object with an event ID and a payload to all connected clients.
        /// </summary>
        /// <param name="eventId">The event ID to be sent.</param>
        /// <param name="json">The payload to be sent in JSON format.</param>
        public static void Send(string eventId, string json)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"event\":\"" + eventId + "\",");
            sb.Append("\"payload\":\"" + json+ "\"");
            sb.Append("}");
            foreach (ClientMetadata client in _wss.ListClients())
                _wss.SendAsync(client.Guid, sb.ToString());
        }
        #endregion
        #region Websocket Events
        /// <summary>
        /// Event handler for when a message is received from a client.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="e">The event arguments containing the client information and the received data.</param>
        private static void OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            string msg = Encoding.UTF8.GetString(e.Data.ToArray());
            try
            {
                OnSocketEvent eventObj = Misc.Deserialize<Types.OnSocketEvent>(msg);
                if (eventObj.sendto.Contains("*"))
                    foreach (ClientMetadata client in _wss.ListClients())
                        _wss.SendAsync(client.Guid, msg);
                else
                    foreach (Guid client in _clients.Where(x => eventObj.sendto.Contains(x.name)).Select(x => x.Id))
                        _wss.SendAsync(client, msg);
            }
            catch (Exception)
            {
                Console.WriteLine("Message received from " + e.Client.Guid + ": " + msg);
            }
        }

        /// <summary>
        /// Event handler for when a client disconnects from the server.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="e">The event arguments containing the client information.</param>
        private static void OnClientDisconnected(object sender, DisconnectionEventArgs e)
        {
            Console.WriteLine("Client disconnected: " + e.Client.Guid);
            _clients.RemoveAll(x => x.Id == e.Client.Guid);
        }

        /// <summary>
        /// Event handler for when a client connects to the server.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="e">The event arguments containing the client information.</param>
        private static void OnClientConnected(object sender, ConnectionEventArgs e)
        {
            try
            {
                if (e.Client.HttpContext.Request.Cookies["JWT"] == null)
                    throw new Exception("Missing JWT cookie");

                JWTPayload jwt = Misc.Deserialize<JWTPayload>(Misc.Base64Decode(e.Client.HttpContext.Request.Cookies["JWT"].Value));
                jwt.Id = e.Client.Guid;
                _clients.Add(jwt);
                Console.WriteLine("Client connected: " + jwt.name + " " + jwt.Id + "Total clients: " + _clients.Count);
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid client connection attempt. Disconnecting client.");
                e.Client.Ws.Abort();
            }

        }
        #endregion
        #region Method Get Set

        #endregion
    }
}
