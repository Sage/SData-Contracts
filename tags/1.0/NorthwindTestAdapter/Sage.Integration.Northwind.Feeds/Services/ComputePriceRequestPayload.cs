#region Usings

using System.Xml;
using System.Xml.Serialization;
using Sage.Integration.Northwind.Common;

#endregion

namespace Sage.Integration.Northwind.Feeds.Services
{
    [XmlRootAttribute("payload", Namespace = Namespaces.sdataNamespace, IsNullable = false)]
    public class ComputePriceRequestPayload : PayloadBase
    {
        #region Serialization class: _InnerPayload

        // class used for serialization only
        public class _InnerPayload : PayloadBase
        {
            #region Ctor.

            public _InnerPayload()
            {
                this.ComputePriceRequest = new computePriceRequesttype();
            }

            #endregion

            [XmlElement(ElementName = "computePrice", Namespace = Namespaces.northwindNamespace)]
            public computePriceRequesttype ComputePriceRequest { get; set; }
        }

        #endregion

        #region Ctor.

        public ComputePriceRequestPayload()
        {
            this.InnerPayload = new _InnerPayload();
        }

        #endregion

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [XmlElement(ElementName = "request", Namespace = Namespaces.northwindNamespace)]
        public _InnerPayload InnerPayload { get; set; }


        [XmlIgnore]
        public computePriceRequesttype ComputePriceRequest
        {
            get { return this.InnerPayload.ComputePriceRequest; }
            set { this.InnerPayload.ComputePriceRequest = value; }
        }
        
    }
}
