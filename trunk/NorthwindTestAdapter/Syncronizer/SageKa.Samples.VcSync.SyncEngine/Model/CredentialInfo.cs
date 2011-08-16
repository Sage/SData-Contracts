using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using SageKa.Samples.VcSync.SyncEngine.Helpers;

namespace SageKa.Samples.VcSync.SyncEngine.Model
{
    [Serializable]
    public class CredentialInfo : IModelChanged
    {
        private string _user;
        private string _password;
        private bool _changed;

        public CredentialInfo()
        { _changed = false; }

        public string User
        {
            get { return _user; }
            set { _changed = true; _user = value; }
        }
        
        [XmlIgnore]
        public string Password
        {
            get { return _password; }
            set { _changed = true; _password = value; }
        }

        public string PwdEncrypt
        {
            get { return Encryption.EncryptString(_password, "k4rls6uh3"); }
            set { _changed = true; _password = Encryption.DecryptString(value, "k4rls6uh3"); }

        }
        #region IModelChanged Members

        [XmlIgnore]
        public bool Changed
        {
            get
            {
                return _changed;
            }
            set
            {
                _changed = value;
            }
        }

        #endregion
    }
}
