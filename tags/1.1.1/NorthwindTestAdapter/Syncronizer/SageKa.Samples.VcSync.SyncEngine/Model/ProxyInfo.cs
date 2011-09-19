using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using SageKa.Samples.VcSync.SyncEngine.Helpers;

namespace SageKa.Samples.VcSync.SyncEngine.Model
{
    [Serializable]
    public class ProxyInfo : IModelChanged
    {
        private string _host;
        private Int32 _port;
        private CredentialInfo _credentials;
        private bool _changed;

        public ProxyInfo()
        {
            _changed = false;
        }

        public string Host
        {
            get { return _host; }
            set { _changed = true; _host = value; }
        }
        

        public Int32 Port
        {
            get { return _port; }
            set { _changed = true; _port = value; }
        }
        
        public CredentialInfo Credentials
        {
            get { return _credentials; }
            set { _changed = true; 
                _credentials = value; }
        }

        #region IModelChanged Members

        [XmlIgnore]
        public bool Changed
        {
            get
            {
                if (_changed)
                    return true;
                if (Credentials != null && Credentials.Changed)
                    return true;
                return false;
            }
            set
            {
                if (Credentials != null)
                    Credentials.Changed = value;
                _changed = value;
            }
        }

        #endregion
    }
}
