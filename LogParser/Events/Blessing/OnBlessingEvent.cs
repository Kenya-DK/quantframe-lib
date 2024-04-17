using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantframeLib.LogParser.Events.Blessing
{
    public class OnBlessingEvent
    {
        #region Const/Static Values
        #endregion
        #region Private Values		
        #endregion
        #region New
        public OnBlessingEvent()
        {
        }
        #endregion
        #region Method

        public static bool ProcessLine(string line)
        {
            if (line.Contains("LotusProfileData::OnSendHubBlessing"))
                Task.Run(() =>
                {
                    try
                    {
                        // Find the index of the "body=" substring
                        int bodyIndex = line.IndexOf("body=");
                        if (bodyIndex == -1)
                            throw new ArgumentException("Invalid input: 'body=' substring not found");

                        // Extract the substring containing the JSON data
                        string jsonSubstring = line.Substring(bodyIndex + 5);

                        // Send Event
                        Socket.SocketServer.Send(typeof(OnBlessingEvent).Name, jsonSubstring);
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
