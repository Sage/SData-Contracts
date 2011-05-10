#region Usings

using System;
using System.Xml.Serialization;

using Sage.Integration.Northwind.Feeds.SalesOrders;
using Sage.Integration.Northwind.Feeds.TradingAccounts;
using Sage.Integration.Northwind.Common;
using System.Collections.Generic;

#endregion

namespace Sage.Integration.Northwind.Feeds
{
    [XmlInclude(typeof(TradingAccountPayload))]
    [XmlInclude(typeof(ContactPayload))]
    [XmlInclude(typeof(EmailPayload))]
    [XmlInclude(typeof(PhoneNumberPayload))]
    [XmlInclude(typeof(PostalAddressPayload))]
    [XmlInclude(typeof(CommodityGroupPayload))]
    [XmlInclude(typeof(CommodityPayload))]
    [XmlInclude(typeof(PriceListPayload))]
    [XmlInclude(typeof(PricePayload))]
    [XmlInclude(typeof(SalesOrderLinePayload))]
    [XmlInclude(typeof(SalesOrderPayload))]
    [XmlInclude(typeof(UnitOfMeasureGroupPayload))]
    [XmlInclude(typeof(UnitOfMeasurePayload))]
    [XmlInclude(typeof(SyncDigestPayload))]
    [XmlInclude(typeof(SalesInvoiceLinePayload))]
    [XmlInclude(typeof(SalesInvoicePayload))]
    [XmlRootAttribute("payload", DataType="payload" , Namespace = Namespaces.sdataNamespace, IsNullable = false)]
    public class PayloadBase
    {

        
        
        private Guid _syncUuid;

        [XmlIgnore]
        public Guid SyncUuid
        {
            get { return _syncUuid; }
            set { _syncUuid = value; }
        }

        private string _localID;

        [XmlIgnore]
        public string LocalID
        {
            get { return _localID; }
            set { _localID = value; }
        }

        private Dictionary<string, string> _foreignIds;
        [XmlIgnore]
        public Dictionary<string, string> ForeignIds
        {
            get { return _foreignIds; }
            set { _foreignIds = value; }
        }

        #region Ctor.

        public PayloadBase()
        {
            _foreignIds = new Dictionary<string, string>();
        }

        #endregion
    }
}
