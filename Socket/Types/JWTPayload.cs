using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace QuantframeLib.Socket.Types
{
    [DataContract]
    public class JWTPayload
    {
        #region Const/Static Values
        #endregion
        #region Private Values
        private string _name;
        private Guid _id;
        #endregion
        #region New
        public JWTPayload()
        {
        }
        #endregion
        #region Methods
        #endregion
        #region Override Methods
        #endregion
        #region Method Get Or Set
        public Guid Id
        {
            get => _id;
            set { _id = value; }
        }

        [DataMember]
        public string name
        {
            get => _name;
            set { _name = value; }
        }
        #endregion
        #region ICopyable<JWTPayload> Members
        public T ShallowCopy<T>() where T : JWTPayload
        {
            return (T)MemberwiseClone();
        }
        public T DeepCopy<T>() where T : JWTPayload
        {
            var clone = (T)MemberwiseClone();
            clone._id= _id;
            clone._name = _name;
            return clone;
        }
        #endregion
    }
}
