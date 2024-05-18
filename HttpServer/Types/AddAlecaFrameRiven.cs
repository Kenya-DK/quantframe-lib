using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace QuantframeLib.HttpServer.Types
{
    [DataContract]
    public class AddAlecaFrameRiven
    {
        #region Const/Static Values
        #endregion
        #region Private Values		
        private string _wfm_url;
        private string _weapon_type;
        private string _mod_name;
        private int _mastery_rank;
        private int _re_rolls;
        private int _bought;
        private int? _minimum_price;
        private string _polarity;
        private int _rank;
        private List<AlecaFrameRivenAttribute> _attributes;
        #endregion
        #region New


        /// <summary>
        /// Initializes a new instance of the <see cref="AddAlecaFrameRiven<T>"/> class.
        /// </summary>
        public AddAlecaFrameRiven()
        {
        }

        #endregion

        #region Method 
        public string ToJson()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"wfm_url\":\"" + WeaponName + "\",");
            sb.Append("\"weapon_type\":\"" + WeaponType + "\",");
            sb.Append("\"mod_name\":\"" + ModName + "\",");
            sb.Append("\"mastery_rank\":" + MasteryRank + ",");
            sb.Append("\"re_rolls\":" + ReRolls + ",");
            sb.Append("\"polarity\":\"" + Polarity + "\",");
            sb.Append("\"rank\":" + Rank + ",");
            sb.Append("\"bought\":" + Bought + ",");
            if (MinimumPrice.HasValue)
                sb.Append("\"minimum_price\":" + MinimumPrice.Value + ",");
            sb.Append("\"attributes\":[");
            for (int i = 0; i < Attributes.Count; i++)
            {
                sb.Append(Attributes[i].ToJson());
                if (i < Attributes.Count - 1)
                    sb.Append(",");
            }
            sb.Append("]");
            sb.Append("}");
            return sb.ToString();
        }
        #endregion

        #region Override Method      
        /// <summary>
        /// Override ToString for <see cref="AddAlecaFrameRiven<t>"/>
        /// </summary>
        /// <returns>
        /// Returns store data in string.
        /// </returns>
        public override string ToString()
        {
            return $"WeaponName: {_wfm_url}, WeaponType: {_weapon_type}, ModName: {_mod_name}, MasteryRank: {_mastery_rank}, ReRolls: {_re_rolls}, Polarity: {_polarity}, Rank: {_rank}, Attributes: {_attributes.Count}";
        }
        #endregion

        #region Method Get Set
        [DataMember(Name = "wfm_url")]
        public string WeaponName
        {
            get { return _wfm_url; }
            set { _wfm_url = value; }
        }

        [DataMember(Name = "weapon_type")]
        public string WeaponType
        {
            get { return _weapon_type; }
            set { _weapon_type = value; }
        }

        [DataMember(Name = "mod_name")]
        public string ModName
        {
            get { return _mod_name; }
            set { _mod_name = value; }
        }

        [DataMember(Name = "mastery_rank")]
        public int MasteryRank
        {
            get { return _mastery_rank; }
            set { _mastery_rank = value; }
        }

        [DataMember(Name = "re_rolls")]
        public int ReRolls
        {
            get { return _re_rolls; }
            set { _re_rolls = value; }
        }

        [DataMember(Name = "polarity")]
        public string Polarity
        {
            get { return _polarity; }
            set { _polarity = value; }
        }
        [DataMember(Name = "rank")]
        public int Rank
        {
            get { return _rank; }
            set { _rank = value; }
        }
        [DataMember(Name = "bought")]
        public int Bought
        {
            get { return _bought; }
            set { _bought = value; }
        }
        [DataMember(Name = "minimum_price")]
        public int? MinimumPrice
        {
            get { return _minimum_price; }
            set { _minimum_price = value; }
        }
        [DataMember(Name = "attributes")]
        public List<AlecaFrameRivenAttribute> Attributes
        {
            get { return _attributes; }
            set { _attributes = value; }
        }
        #endregion
    }
    [DataContract]
    public class AlecaFrameRivenAttribute
    {
        #region Const/Static Values
        #endregion
        #region Private Values		
        private string _url_name;
        private double _value;
        private bool _positive;
        #endregion
        #region New


        /// <summary>
        /// Initializes a new instance of the <see cref="AddAlecaFrameRiven<T>"/> class.
        /// </summary>
        public AlecaFrameRivenAttribute()
        {
        }

        #endregion

        #region Method 
        public string ToJson()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"url_name\":\"" + _url_name + "\",");
            sb.Append("\"value\":" + _value.ToString().Replace(",", ".") + ",");
            sb.Append("\"positive\":" + _positive.ToString().ToLower());
            sb.Append("}");
            return sb.ToString();
        }
        #endregion

        #region Override Method      
        /// <summary>
        /// Override ToString for <see cref="AlecaFrameRivenAttribute<t>"/>
        /// </summary>
        /// <returns>
        /// Returns store data in string.
        /// </returns>
        public override string ToString()
        {
            return $"Name: {_url_name}, Value: {_value}, Positive: {_positive}";
        }
        #endregion

        #region Method Get Set
        [DataMember(Name = "url_name")]
        public string Name
        {
            get { return _url_name; }
            set { _url_name = value; }
        }

        [DataMember(Name = "value")]
        public double Value
        {
            get { return _value; }
            set { _value = value; }
        }

        [DataMember(Name = "positive")]
        public bool Positive
        {
            get { return _positive; }
            set { _positive = value; }
        }
        #endregion
    }
}
