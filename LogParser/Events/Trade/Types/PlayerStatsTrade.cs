using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantframeLib.LogParser.Events.Trade.Types
{
    public class PlayerStatsTrade
    {
        #region Const/Static Values
        #endregion
        #region Private Values		
        private DateTime _tradeTime;
        private List<TradeItem> _offeredItems;
        private List<TradeItem> _receivedItems;
        private string _playerName;
        private TradeClassification _type;
        private int? _platinum;
        #endregion
        #region New


        /// <summary>
        /// Initializes a new instance of the <see cref="TradeDetector"/> class.
        /// </summary>
        public PlayerStatsTrade()
        {
            _offeredItems = new List<TradeItem>();
            _receivedItems = new List<TradeItem>();
        }

        #endregion
        #region Method 
        public string ToJson()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"tradeTime\":\"" + _tradeTime + "\",");
            sb.Append("\"offeredItems\":[");
            for (int i = 0; i < _offeredItems.Count; i++)
            {
                sb.Append(_offeredItems[i].ToJson());
                if (i < _offeredItems.Count - 1)
                    sb.Append(",");
            }
            sb.Append("],");
            sb.Append("\"receivedItems\":[");
            for (int i = 0; i < _receivedItems.Count; i++)
            {
                sb.Append(_receivedItems[i].ToJson());
                if (i < _receivedItems.Count - 1)
                    sb.Append(",");
            }
            sb.Append("],");
            sb.Append("\"playerName\":\"" + _playerName + "\",");
            sb.Append("\"type\":\"" + _type + "\",");
            sb.Append("\"platinum\":" + _platinum);
            sb.Append("}");
            return sb.ToString();
        }
        #endregion
        #region Override Method      
        /// <summary>
        /// Override ToString for <see cref="PlayerStatsTrade"/>
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
        /// Gets or sets the trade time.
        /// </summary>
        public DateTime TradeTime
        {
            get { return _tradeTime; }
            set { _tradeTime = value; }
        }
        /// <summary>
        /// Gets or sets the offered items.
        /// </summary>
        public List<TradeItem> OfferedItems
        {
            get { return _offeredItems; }
            set { _offeredItems = value; }
        }
        /// <summary>
        /// Gets or sets the received items.
        /// </summary>
        public List<TradeItem> ReceivedItems
        {
            get { return _receivedItems; }
            set { _receivedItems = value; }
        }
        /// <summary>
        /// Gets or sets the player name.
        /// </summary>
        public string PlayerName
        {
            get { return _playerName; }
            set { _playerName = value; }
        }
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        public TradeClassification Type
        {
            get { return _type; }
            set { _type = value; }
        }
        /// <summary>
        /// Gets or sets the platinum. Nullable.
        /// </summary>
        public int? Platinum
        {
            get { return _platinum; }
            set { _platinum = value; }
        }
        #endregion
    }
}
