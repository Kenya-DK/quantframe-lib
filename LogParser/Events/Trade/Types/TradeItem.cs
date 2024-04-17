using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantframeLib.LogParser.Events.Trade.Types
{
    public class TradeItem
    {
        #region Const/Static Values
        #endregion
        #region Private Values		
        private string _name;
        private int _quantity;
        #endregion
        #region New


        /// <summary>
        /// Initializes a new instance of the <see cref="TradeDetector"/> class.
        /// </summary>
        /// <param name="name">The name of the item.</param>
        /// <param name="quantity">The quantity of the item.</param>
        public TradeItem(string name, int quantity)
        {
            _name = name;
            _quantity = quantity;
        }

        #endregion
        #region Method 
        public string ToJson()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"name\":\"" + _name + "\",");
            sb.Append("\"quantity\":" + _quantity);
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
        /// Gets or sets the name.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        public int Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }
        #endregion
    }
}
