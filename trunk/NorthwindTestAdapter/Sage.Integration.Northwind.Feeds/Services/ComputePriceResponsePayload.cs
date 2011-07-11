#region Usings

using System.Xml;
using System.Xml.Serialization;
using Sage.Integration.Northwind.Common;

#endregion

namespace Sage.Integration.Northwind.Feeds.Services
{
    [XmlRootAttribute("payload", Namespace = Namespaces.sdataNamespace, IsNullable = false)]
    public class ComputePriceResponsePayload : PayloadBase
    {
        #region Serialization class: _InnerPayload

        // class used for serialization only
        public class _InnerPayload : PayloadBase
        {
            #region Ctor.

            public _InnerPayload()
            {
                this.ComputePriceResponse = new computePriceResponsetype();
            }

            #endregion

            [XmlElement(ElementName = "computePrice", Namespace = Namespaces.northwindNamespace)]
            public computePriceResponsetype ComputePriceResponse { get; set; }
        }

        #endregion

        #region Ctor.

        public ComputePriceResponsePayload()
        {
            this.InnerPayload = new _InnerPayload();
        }

        #endregion

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [XmlElement(ElementName = "response", Namespace = Namespaces.northwindNamespace)]
        public _InnerPayload InnerPayload { get; set; }


        [XmlIgnore]
        public computePriceResponsetype ComputePriceResponse
        {
            get { return this.InnerPayload.ComputePriceResponse; }
            set { this.InnerPayload.ComputePriceResponse = value; }
        }
    }
}