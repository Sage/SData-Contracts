using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using SageKa.Samples.VcSync.SyncEngine.Helpers;


namespace SageKa.Samples.VcSync.SyncEngine.Model
{
    [Serializable]
    public class EndPointInfo : IModelChanged
    {
        private Uri _url;
        private CredentialInfo _credentials;
        private bool _changed;

        public EndPointInfo()
        { }

        [XmlIgnore]
        public Uri Url
        {
            get { return _url; }
        //    set
        //    {
        //        _changed = true; 
        //        _url = value; }
        }

        public string UrlString
        {
            get
            {
                if (_url == null)
                    return null;

                return _url.ToString() ;
            }
            set { _changed = true; 
                if (value.EndsWith(@"/"))
                    _url = new Uri(value);
                else
                    _url = new Uri(value + @"/"); }
        }

        public CredentialInfo Credentials
        {
            get { return _credentials; }
            set { _changed = true; _credentials = value; }
        }
        internal Uri GetResourceUri(string resource)
        {
            Uri result;
            if (Uri.TryCreate(_url, resource, out result))
                return result;
            
            return _url;
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
