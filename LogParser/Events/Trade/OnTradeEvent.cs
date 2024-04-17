using QuantframeLib.LogParser.Events.Conversation;
using QuantframeLib.LogParser.Events.Trade.Types;
using QuantframeLib.Types;
using QuantframeLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantframeLib.LogParser.Events.Trade
{
    public class OnTradeEvent
    {
        #region Const/Static Values
        #endregion
        #region Private Values	
        private static bool gettingTradeMessageMultiline = false;
        private static bool waitingForTradeMessageConfirm = false;
        private static List<string> tradeLines = new List<string>();
        private static Dictionary<WarframeLanguage, TradeDetector> tradeDetectors = new Dictionary<WarframeLanguage, TradeDetector>()
        {
            {WarframeLanguage.English, new TradeDetector("description=Are you sure you want to accept this trade? You are offering", "description=The trade was successful!, leftItem=/Menu/Confirm_Item_Ok", "description=The trade failed., leftItem=/Menu/Confirm_Item_Ok", "and will receive from ", " the following:", "Platinum") }
        };
        private static PlayerStatsTrade currentTrade = new PlayerStatsTrade();
        private static WarframeLanguage currentLanguage = WarframeLanguage.English;
        #endregion
        #region New
        public OnTradeEvent()
        {
        }
        #endregion
        #region Method
        public static string ToJson()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{");
            stringBuilder.Append("\"playerName\":\"" + currentTrade.PlayerName + "\",");
            stringBuilder.Append("\"tradeTime\":\"" + currentTrade.TradeTime + "\",");
            stringBuilder.Append("\"type\":\"" + currentTrade.Type + "\",");
            stringBuilder.Append("\"platinum\":" + currentTrade.Platinum + ",");
            stringBuilder.Append("\"offeredItems\":[" + string.Join(",", currentTrade.OfferedItems.Select(p => p.ToJson())) + "],");
            stringBuilder.Append("\"receivedItems\":[" + string.Join(",", currentTrade.ReceivedItems.Select(p => p.ToJson())) + "]");
            stringBuilder.Append("}");
            return stringBuilder.ToString();
        }
        public static void TradeStarted(string msg)
        {
            AddTradeMessage(msg);
        }
        public static void TradeFinished()
        {
            try
            {
                WarframeLanguage language = currentLanguage;
                TradeDetector detector = tradeDetectors[language];
                if (detector == null)
                    return;

                List<string> lines = tradeLines;

                string[] strArray = lines[0].Split('\n');
                lines.RemoveAt(0);
                for (int index = 0; index < strArray.Length; ++index)
                    lines.Insert(index, strArray[index]);
                bool flag = true;
                foreach (string line in lines)
                {
                    if (!string.IsNullOrEmpty(line) && !(line == "\n") && !line.Contains(detector.BeginLine))
                    {
                        if (line.Contains(detector.ReceiveLineFirstPart) && line.Contains(detector.ReceiveLineSecondPart))
                        {
                            currentTrade.PlayerName = Misc.RemoveSpecialCharacters(line.Replace(detector.ReceiveLineFirstPart, "").Replace(detector.ReceiveLineSecondPart, "").Trim());
                            flag = false;
                        }
                        else
                        {
                            string str2 = line;
                            if (line.Contains(", leftItem=/"))
                                str2 = line.Substring(0, line.IndexOf(", leftItem=/"));
                            string str3 = str2.Replace("\r", "").Replace("\n", "");
                            string itemName = "";
                            int quantity = 1;
                            if (str3.Contains(" x "))
                            {
                                itemName = str3.Substring(0, str3.IndexOf(" x "));
                                quantity = int.Parse(str3.Substring(str3.IndexOf(" x ") + 3));
                            }
                            else
                                itemName = str3;
                            itemName = itemName.Trim();
                            if (itemName == detector.PlatinumName)
                                itemName = "plat";
                            if (!string.IsNullOrEmpty(itemName))
                            {
                                if (flag)
                                {
                                    if (currentTrade.OfferedItems.Any(p => p.Name == itemName))
                                        ++currentTrade.OfferedItems.First(p => p.Name == itemName).Quantity;
                                    else
                                        currentTrade.OfferedItems.Add(new TradeItem(itemName, quantity));
                                }
                                else if (currentTrade.ReceivedItems.Any(p => p.Name == itemName))
                                    ++currentTrade.ReceivedItems.First(p => p.Name == itemName).Quantity;
                                else
                                    currentTrade.ReceivedItems.Add(new TradeItem(itemName, quantity));
                            }
                        }
                    }
                }
                currentTrade.TradeTime = DateTime.UtcNow;

                int offeredPlatinum = currentTrade.OfferedItems.Where(p => p.Name == "plat").Sum(p => p.Quantity);
                int receivedPlatinum = currentTrade.ReceivedItems.Where(p => p.Name == "plat").Sum(p => p.Quantity);
                if (offeredPlatinum > 0)
                    currentTrade.Platinum = offeredPlatinum;
                else if (receivedPlatinum > 0)
                    currentTrade.Platinum = receivedPlatinum;
                else
                    currentTrade.Platinum = null;

                if (offeredPlatinum > 1 && currentTrade.OfferedItems.Count() == 1)
                    currentTrade.Type = TradeClassification.Purchase;
                else if (receivedPlatinum > 1 && currentTrade.ReceivedItems.Count() == 1)
                    currentTrade.Type = TradeClassification.Sale;
                else
                    currentTrade.Type = TradeClassification.Trade;
            }
            catch (Exception)
            {

            }
        }
        public static void TradeFailed()
        {
            Reset();
        }
        public static void TradeAccepted()
        {
            Socket.SocketServer.Send(typeof(OnTradeEvent).Name, currentTrade.ToJson());
            Reset();
        }
        public static void AddTradeMessage(string msg)
        {
            tradeLines.Add(msg);
        }
        public static void Reset()
        {
            tradeLines.Clear();
            currentTrade = new PlayerStatsTrade();
        }
        public static bool IsBeginningOfTradeLog(string msg)
        {
            WarframeLanguage language = Misc.GetWarframeLanguage();
            return language != WarframeLanguage.UNKNOWN && tradeDetectors.ContainsKey(language) && msg.Contains(tradeDetectors[language].BeginLine);
        }
        public static bool WasTradeSuccessful(string msg)
        {
            WarframeLanguage language = Misc.GetWarframeLanguage();
            return language != WarframeLanguage.UNKNOWN && tradeDetectors.ContainsKey(language) && msg.Contains(tradeDetectors[language].TradeConfirmationLine);
        }
        public static bool WasTradeFailed(string msg)
        {
            WarframeLanguage language = Misc.GetWarframeLanguage();
            return language != WarframeLanguage.UNKNOWN && tradeDetectors.ContainsKey(language) && msg.Contains(tradeDetectors[language].TradeFailedLine);
        }
        #endregion
        #region Event
        public static bool ProcessLine(string line)
        {
            while (gettingTradeMessageMultiline)
            {
                if (line.Contains("[Info]") || line.Contains("[Error]") || line.Contains("[Warning]"))
                {
                    gettingTradeMessageMultiline = false;
                    TradeFinished();
                    waitingForTradeMessageConfirm = true;
                }
                else
                {
                    AddTradeMessage(line);
                    return true;
                }
            }

            if (line.Contains("[Info]: Dialog.lua: Dialog::CreateOkCancel(description=") && IsBeginningOfTradeLog(line))
            {
                TradeStarted(line);
                if (line.Contains(", leftItem=/Menu/Confirm_Item_Ok, rightItem=/Menu/Confirm_Item_Cancel)"))
                {
                    TradeFinished();
                    waitingForTradeMessageConfirm = true;
                }
                else
                    gettingTradeMessageMultiline = true;
                return true;
            }
            else if (waitingForTradeMessageConfirm && line.Contains("[Info]: Dialog.lua: Dialog::CreateOk(description="))
            {
                if (WasTradeSuccessful(line))
                    TradeAccepted();
                else if (WasTradeFailed(line))
                    TradeFailed();
                waitingForTradeMessageConfirm = false;
                return true;
            }
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
