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
        static List<UserClient> _clients = new List<UserClient>();

        #endregion
        #region New

        public static void Initializer(string hostname, int port)
        {
            _wss = new WatsonWsServer(hostname, port, false);
            _wss.ClientConnected += OnClientConnected;
            _wss.ClientDisconnected += OnClientDisconnected;
            _wss.MessageReceived += OnMessageReceived;
            _wss.Start();
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
            sb.Append("\"payload\":" + json);
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
            if (string.IsNullOrEmpty(msg))
                return;
            Logger.Info("SocketServer:OnMessageReceived", "Message received: " + e.Client.Guid + ": " + msg);
            if (msg == "disconnect")
            {
                _wss.DisconnectClient(e.Client.Guid);
                return;
            }
            try
            {
                OnSocketEvent eventObj = Misc.Deserialize<OnSocketEvent>(msg);
                if (eventObj.SendTo.Contains("*"))
                    foreach (ClientMetadata client in _wss.ListClients())
                        _wss.SendAsync(client.Guid, msg);
                else
                    foreach (Guid client in _clients.Where(x => eventObj.SendTo.Contains(x.DeviceId)).Select(x => x.Id))
                        _wss.SendAsync(client, msg);
            }
            catch (Exception)
            {
                Logger.Error("SocketServer:OnMessageReceived", "Invalid message received: " + e.Client.Guid + ": " + msg);
            }
        }

        /// <summary>
        /// Event handler for when a client disconnects from the server.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="e">The event arguments containing the client information.</param>
        private static void OnClientDisconnected(object sender, DisconnectionEventArgs e)
        {
            try
            {
                // Find the client in the list and remove it
                UserClient client = _clients.FirstOrDefault(x => x.Id == e.Client.Guid);
                _clients.RemoveAll(x => x.Id == e.Client.Guid);
                if (client == null)
                    Logger.Warning("SocketServer:OnClientDisconnected", "Client was not found in the list: " + e.Client.Guid);
                else
                    Logger.Info("SocketServer:OnClientDisconnected", "Client disconnected: " + client.DeviceId + " Total clients: " + _clients.Count);
            }
            catch (Exception ex)
            {
                Logger.Error("SocketServer:OnClientDisconnected", "Error while disconnecting client: " + ex.Message);
            }
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
                string deviceId = e.Client.HttpContext.Request.Cookies["DEVICEID"]?.Value;
                if (string.IsNullOrEmpty(deviceId))
                {
                    //_wss.SendAsync(e.Client.Guid, "{\"event\":\"error\",\"payload\":\"No device ID provided.\"}");
                    throw new Exception("Missing DEVICEID cookie");
                }

                if (_clients.Any(x => x.DeviceId == deviceId))
                {
                    //_wss.SendAsync(e.Client.Guid, "{\"event\":\"error\",\"payload\":\"Device ID already in use.\"}");
                    throw new Exception("Device ID already in use");
                }

                _clients.Add(new UserClient(e.Client.Guid, deviceId));
                Logger.Info("SocketServer:OnClientConnected", "Client connected: " + deviceId + " Total clients: " + _clients.Count);
            }
            catch (Exception)
            {
                Logger.Error("SocketServer:OnClientConnected", "Invalid client connection attempt. Disconnecting client.");
                e.Client.Ws.Abort();
            }

        }
        #endregion
        #region Method Get Set
        public static List<UserClient> Clients
        {
            get { return _clients; }
        }
        #endregion
    }
}
