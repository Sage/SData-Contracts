using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using SageKa.Samples.VcSync.SyncEngine.Helpers;

namespace SageKa.Samples.VcSync.SyncEngine.Model
{
    [Serializable]
    public class SynchronisationInfo : IModelChanged
    {
        private EndPointInfo _source;
        private EndPointInfo _target;
        private EndPointInfo _logging;
        private ProxyInfo _proxy;
        private List<string> _resources;
        private bool _changed;

        public SynchronisationInfo()
        {
            _resources = new List<string>();
            _source = new EndPointInfo();
            _target = new EndPointInfo();
            _logging = new EndPointInfo();
        }

        public EndPointInfo Source
        {
            get
            {
                return _source;
            }
            set { _changed = true; _source = value; }
        }


        public EndPointInfo Target
        {
            get
            {
                return _target;
            }
            set { _changed = true; _target = value; }
        }


        public EndPointInfo Logging
        {
            get
            {
                return _logging;
            }
            set { _changed = true; _logging = value; }
        }
        public ProxyInfo Proxy
        {
            get
            {
                return _proxy;
            }
            set { _changed = true; _proxy = value; }
        }

        public List<string> Resources
        {
            get { return _resources; }
            set
            {
                _changed = true;
                _resources = value;
            }
        }

        #region IModelChanged Members
        [XmlIgnore]
        public bool Changed
        {
            get
            {
                if (_changed)
                    return true;
                if (Source != null && Source.Changed)
                    return true;
                if (Target != null && Target.Changed)
                    return true;
                if (Logging != null && Logging.Changed)
                    return true;
                if (Proxy != null && Proxy.Changed)
                    return true;
                return false;
            }
            set
            {
                if (Source != null)
                    Source.Changed = value ;
                if (Target != null)
                    Target.Changed = value;
                if (Logging != null)
                    Logging.Changed = value;
                if (Proxy != null)
                    Proxy.Changed = value;

                _changed = value;
            }
        }

        #endregion
    }
}
