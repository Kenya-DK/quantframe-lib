using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantframeLib.LogParser.Events.Trade.Types
{
    public class TradeDetector
    {
        #region Const/Static Values
        #endregion
        #region Private Values		
        private string _trade_start;
        private string _trade_confirmation_line;
        private string _trade_failed_line;
        private string _receive_line_first_part;
        private string _receive_line_second_part;
        private string _platinum_name;
        #endregion
        #region New


        /// <summary>
        /// Initializes a new instance of the <see cref="TradeDetector"/> class.
        /// </summary>
        /// <param name="line">The line to be detected.</param>
        /// <param name="confirmation">The confirmation line for the trade.</param>
        /// <param name="failed">The line indicating a failed trade.</param>
        /// <param name="first_part">The first part of the receive line.</param>
        /// <param name="second_part">The second part of the receive line.</param>
        /// <param name="platinum_name">The name of the platinum to be traded.</param>
        public TradeDetector(string start, string confirmation, string failed, string first_part, string second_part, string platinum_name)
        {
            _trade_start = start;
            _trade_confirmation_line = confirmation;
            _trade_failed_line = failed;
            _receive_line_first_part = first_part;
            _receive_line_second_part = second_part;
            _platinum_name = platinum_name;
        }

        #endregion
        #region Method 
        #endregion
        #region Override Method      
        /// <summary>
        /// Override ToString for <see cref="Trading"/>
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
        /// Gets or sets the begin line.
        /// </summary>
        public string BeginLine
        {
            get { return _trade_start; }
            set { _trade_start = value; }
        }
        /// <summary>
        /// Gets or sets the trade confirmation line.
        /// </summary>
        public string TradeConfirmationLine
        {
            get { return _trade_confirmation_line; }
            set { _trade_confirmation_line = value; }
        }
        /// <summary>
        /// Gets or sets the trade failed line.
        /// </summary>
        public string TradeFailedLine
        {
            get { return _trade_failed_line; }
            set { _trade_failed_line = value; }
        }
        /// <summary>
        /// Gets or sets the receive line first part.
        /// </summary>
        public string ReceiveLineFirstPart
        {
            get { return _receive_line_first_part; }
            set { _receive_line_first_part = value; }
        }
        /// <summary>
        /// Gets or sets the receive line second part.
        /// </summary>
        public string ReceiveLineSecondPart
        {
            get { return _receive_line_second_part; }
            set { _receive_line_second_part = value; }
        }
        /// <summary>
        /// Gets or sets the platinum name.
        /// </summary>
        public string PlatinumName
        {
            get { return _platinum_name; }
            set { _platinum_name = value; }
        }
        #endregion
    }
}
