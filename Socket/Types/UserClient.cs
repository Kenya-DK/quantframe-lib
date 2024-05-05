using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantframeLib.Socket.Types
{
    public class UserClient
    {
        #region Const/Static Values
        #endregion
        #region Private Values		
        private string _deviceId;
        private Guid _id;
        #endregion
        #region New


        /// <summary>
        /// Initializes a new instance of the <see cref="UserClient<T>"/> class.
        /// </summary>
        public UserClient(Guid id, string deviceId)
        {
            _id = id;
            _deviceId = deviceId;
        }

        #endregion
        #region Method 

        #endregion
        #region Override Method      
        /// <summary>
        /// Override ToString for <see cref="UserClient<t>"/>
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
        /// Gets or sets the id.
        /// </summary>
        public Guid Id
        {
            get => _id;
            set { _id = value; }
        }
        /// <summary>
        /// Gets or sets the device id.
        /// </summary>
        public string DeviceId
        {
            get => _deviceId;
            set { _deviceId = value; }
        }
        #endregion
    }
}
