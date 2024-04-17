using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantframeLib.LogParser.Events.Conversation
{
    public class OnConversationEvent
    {
        #region Const/Static Values
        #endregion
        #region Private Values		
        #endregion
        #region New
        public OnConversationEvent()
        {
        }
        #endregion
        #region Method

        public static bool ProcessLine(string line)
        {
            if (line.Contains("ChatRedux::AddTab: Adding tab with channel name"))
                Task.Run(() =>
                {
                    try
                    {
                        string channelName = line.Substring(line.IndexOf("channel name: ") + 14);
                        string userName = channelName.Substring(0, channelName.IndexOf(" to index"));
                        if (!userName.StartsWith("F"))
                            return;
                        if (Encoding.UTF8.GetByteCount(userName.Substring(userName.Length - 1)) != 1)
                            userName = userName.Substring(0, userName.Length - 1);
                        userName = userName.Substring(1);
                        Socket.SocketServer.Send(typeof(OnConversationEvent).Name, userName);
                    }
                    catch (Exception ex)
                    {

                    }
                });
            return false;
        }
        #endregion
        #region Override Method
        /// <summary>
        /// Override ToString for <see cref="OnTradeEvent"/>
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
    }
}
