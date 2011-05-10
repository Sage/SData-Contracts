#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml.Schema;
using Sage.Integration.Northwind.Common;
using System.Xml;
using System.IO;

#endregion

namespace Sage.Integration.Northwind.Feeds.Paging
{
    public abstract class ElementItemBase<T>
    {
        private T _value;

        public string LocalElementName { get; private set; }

        public T Value
        {
            get { return _value; }
            set
            {
                string errMsg;
                if (!this.OnValidateValue(value, out errMsg))
                    throw new ArgumentOutOfRangeException("value", value, errMsg);

                _value = value;
            }
        }

        public string Namespace { get; private set; }
        public string Prefix { get; private set; }

        #region Ctor.

        protected ElementItemBase(string localElementName, T value, string ns, string prefix)
        {
            this.LocalElementName = localElementName;
            this.Value = value;
            this.Namespace = ns;
            this.Prefix = prefix;
        }

        #endregion

        protected virtual bool OnValidateValue(T value, out string errMsg)
        {
            errMsg = null;
            return true;
        }

        public void LoadXmlValue(string xml)
        {
            this.Value = XmlGenericConvert<T>.ConvertToGeneric(xml);
        }
        public string ToXml()
        {
            string xmlValue = XmlGenericConvert<T>.ConvertToString(this.Value);
            return string.Format("<{0}:{1} xmlns:{0}=\"{2}\">{3}</{0}:{1}>", this.Prefix, this.LocalElementName, this.Namespace, xmlValue);
        }

        public override string ToString()
        {
            return (this.LocalElementName + ": " + _value.ToString());
        }
    }
}
