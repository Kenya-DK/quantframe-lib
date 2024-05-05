using QuantframeLib.LogParser.Events.Trade.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace QuantframeLib.Socket.Types
{
    [DataContract]
    public class OnSocketEvent
    {
        #region Const/Static Values
        #endregion
        #region Private Values		
        private string _id;
        private List<string> _sendTo = new List<string>();
        private string _payload;
        #endregion
        #region New


        /// <summary>
        /// Initializes a new instance of the <see cref="OnSocketEvent<T>"/> class.
        /// </summary>
        public OnSocketEvent(string eventId, string payload)
        {
            _id = eventId;
            _payload = payload;
        }

        #endregion

        #region Method 

        #endregion

        #region Override Method      
        /// <summary>
        /// Override ToString for <see cref="OnSocketEvent<t>"/>
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
        /// Gets or sets the event.
        /// </summary>
        [DataMember(Name = "id")]
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Gets or sets the payload.
        /// </summary>
        [DataMember(Name = "payload")]
        public string Payload
        {
            get { return _payload; }
            set { _payload = value; }
        }

        /// <summary>
        /// Gets or sets the send to.
        /// </summary>
        [DataMember(Name = "send_to")]
        public List<string> SendTo
        {
            get { return _sendTo; }
            set { _sendTo = value; }
        }
        #endregion
    }
}
