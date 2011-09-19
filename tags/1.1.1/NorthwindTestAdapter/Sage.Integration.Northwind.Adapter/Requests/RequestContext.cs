#region Usings

using System;
using System.ComponentModel;
using System.IO;
using Sage.Common.Expressions;
using Sage.Common.Syndication;
using Sage.Integration.Northwind.Application;
using Sage.Sis.Sdata.Sync.Context;

#endregion

namespace Sage.Integration.Northwind.Adapter.Common
{
    public class RequestContext
    {
        #region Properties

        public string ErrorMessage { get; set; }

        public SDataUri SdataUri { get; set; }

        public RequestType RequestType { get; set; }

        public string Application { get; set; }

        public string Contract { get; set; }

        public string Dataset { get; set; }

        public SdataContext SdataContext { get; set; }

        public SupportedResourceKinds ResourceKind { get; set; }

        public string ResourceKey { get; set; }

        public Guid TrackingId { get; set; }

        public bool HasTrackingId { get; set; }

        public string BaseLink { get; set; }

        public string ApplicationLink { get; set; }

        public string ContractLink { get; set; }

        public string DatasetLink { get; set; }

        /// <summary>
        /// Contains the name of an import schema Only set when this.RequestType==ImportSchema
        /// </summary>
        public string ImportSchemaName { get; private set; }

        private string _originEndPoint;
        public string OriginEndPoint
        {
            get
            {
                if (String.IsNullOrEmpty(_originEndPoint))
                {
                    _originEndPoint = this.DatasetLink + this.ResourceKind.ToString();
                }

                return _originEndPoint;
            }
            set { _originEndPoint = value; }
        }

        public NorthwindConfig Config { get; set; }

        #endregion

        #region Ctor.

        public RequestContext(SDataUri sdataUri)
        {
            this.SdataUri = sdataUri;

            // config
            NorthwindConfig newConfig = new NorthwindConfig(sdataUri.CompanyDataset);

            newConfig.CurrencyCode = "EUR";
            newConfig.CrmUser = "Sdata";
            newConfig.Path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Northwind");

            this.Config = newConfig;

            // initialize requestType
            this.RequestType = RequestType.None;

            // hasTrackingId
            this.HasTrackingId = false;

            // baseLink
            string newBaseLink = string.Empty;

            if (String.IsNullOrEmpty(sdataUri.Scheme))
                newBaseLink += "http://";
            else
                newBaseLink += sdataUri.Scheme + "://";

            if (String.IsNullOrEmpty(sdataUri.Host))
                newBaseLink += "localhost";
            else
                newBaseLink += sdataUri.Host;

            if (sdataUri.Port > 0)
                newBaseLink += ":" + sdataUri.Port.ToString();

            if (String.IsNullOrEmpty(sdataUri.Server))
                newBaseLink += "/sdata/";
            else
                newBaseLink += "/" + sdataUri.Server + "/";

            this.BaseLink = newBaseLink;

            // application
            this.Application = (sdataUri.PathSegments.Length > 0) ? sdataUri.PathSegments[0].Text : "";

            // applicationLink
            this.ApplicationLink = this.BaseLink + this.Application + "/";

            if (sdataUri.PathSegments.Length == 1)
            {
                this.RequestType = RequestType.Contract;            // -> Type: contract
                return;
            }

            // contract
            string newContract = sdataUri.PathSegments[1].Text;

            if (newContract == "*")
            {
                this.RequestType = RequestType.Contract;            // -> Type: contract
                this.Contract = string.Empty;
                return;
            }
            this.Contract = newContract;

            // contractLink
            this.ContractLink = this.ApplicationLink + this.Contract + "/";

            if (sdataUri.PathSegments.Length == 2)
            {
                this.RequestType = RequestType.Dataset;             // -> Type: dataset
                return;
            }

            // dataset
            string newDataset = sdataUri.PathSegments[2].Text;

            if (newDataset == "*")
            {
                this.RequestType = RequestType.Dataset;             // -> Type: dataset
                this.Dataset = string.Empty;
                return;
            }
            this.Dataset = newDataset;

            // datasetLink
            this.DatasetLink = this.ContractLink + this.Dataset + "/";

            // sdataContext
            this.SdataContext = new SdataContext(this.Application, this.Dataset, this.Contract, this.DatasetLink);


            if (this.SdataUri.PathSegments.Length == 3)
            {
                if (!string.IsNullOrEmpty(this.SdataUri.ServiceMethod))
                {
                    this.RequestType = RequestType.Service;                    // -> Type: Service
                    return;
                }

                this.RequestType = RequestType.ResourceCollection;  // -> Type: ResourceCollection
                return;
            }            

            string temp;
            temp = this.SdataUri.PathSegments[3].Text;

            if (temp == "*")
            {
                this.RequestType = RequestType.ResourceCollection;      // -> Type: ResourceCollection
                return;
            }

            if (temp.Equals(Constants.schema, StringComparison.InvariantCultureIgnoreCase))
            {
                // check whether it is an import schema
                if (this.SdataUri.PathSegments.Length == 4)
                {
                    this.RequestType = RequestType.ResourceCollectionSchema;    // -> Type: ResourceCollectionSchema
                    return;
                }
                else if (this.SdataUri.PathSegments.Length == 6 && this.SdataUri.PathSegments[4].Text.Equals("import", StringComparison.InvariantCultureIgnoreCase) && (!string.IsNullOrEmpty(this.SdataUri.PathSegments[5].Text)))
                {
                    this.ImportSchemaName = this.SdataUri.PathSegments[5].Text;
                    this.RequestType = RequestType.ImportSchema;    // -> Type: ImportSchema
                    return;
                }
                
            }
            if (temp.Equals(Constants.service, StringComparison.InvariantCultureIgnoreCase))
            {
                if (this.SdataUri.PathSegments.Length >= 6)
                {
                    this.SdataUri.ServiceMethod = this.SdataUri.PathSegments[4].Text;
                    if (this.SdataUri.PathSegments[5].Text.Equals(Constants.schema, StringComparison.InvariantCultureIgnoreCase))
                    {
                        this.RequestType = RequestType.ServiceSchema;

                        return;
                    }
                }
                else
                {
                    throw new RequestException(string.Format("Url {0} cannot be parsed.", this.SdataUri.ToString()));
                }
            }

            try
            {
                // resourceKind
                this.ResourceKind = (SupportedResourceKinds)Enum.Parse(typeof(SupportedResourceKinds), temp, true);

                if (sdataUri.PathSegments.Length > 4 && sdataUri.PathSegments[4].Text.Equals(Constants.linked, StringComparison.InvariantCultureIgnoreCase))
                {
                    this.RequestType = RequestType.Linked;                    // -> Type: Linked

                    // resourceKey
                    if (sdataUri.PathSegments[4].HasPredicate)
                    {
                        if (sdataUri.PathSegments[4].PredicateExpression is StringLiteralExpression)
                        {
                            this.ResourceKey = ((StringLiteralExpression)sdataUri.PathSegments[4].PredicateExpression).Value.ToString();
                            return;
                        }
                        else
                        {
                            this.RequestType = RequestType.None;            // -> Type: None
                            this.ResourceKind = SupportedResourceKinds.None;
                            this.ErrorMessage = "please specify the primary Key inside the predicate as string";
                            return;
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(this.SdataUri.ServiceMethod))
                {

                    if (sdataUri.PathSegments.Length >= 7 && sdataUri.PathSegments[6].Text.Equals(Constants.schema, StringComparison.InvariantCultureIgnoreCase))
                        this.RequestType = RequestType.ServiceSchema;   // currently we do not enter here, but we don't know if SIF will be changed to set the service name
                    else
                        this.RequestType = RequestType.Service;                    // -> Type: Service
                    return;
                }
                else if (sdataUri.PathSegments.Length >= 7 &&
                    sdataUri.PathSegments[4].Text.Equals(Constants.service, StringComparison.InvariantCultureIgnoreCase) &&
                    sdataUri.PathSegments[6].Text.Equals(Constants.schema, StringComparison.InvariantCultureIgnoreCase))
                {
                    this.SdataUri.ServiceMethod = sdataUri.PathSegments[5].Text;
                    this.RequestType = RequestType.ServiceSchema;
                    return;
                }
                else
                {
                    this.RequestType = RequestType.Resource;                    // -> Type. Resource

                    // resourceKey
                    if (sdataUri.PathSegments[3].HasPredicate)
                    {
                        if (sdataUri.PathSegments[3].PredicateExpression is StringLiteralExpression)
                        {
                            this.ResourceKey = ((StringLiteralExpression)sdataUri.PathSegments[3].PredicateExpression).Value.ToString();
                            return;
                        }
                        else
                        {
                            this.RequestType = RequestType.None;            // -> Type: None
                            this.ResourceKind = SupportedResourceKinds.None;
                            this.ErrorMessage = "please specify the primary Key inside the predicate as string";
                            return;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                this.RequestType = RequestType.None;
                this.ResourceKind = SupportedResourceKinds.None;
                this.ErrorMessage = e.Message;
                return;
            }



            if (this.SdataUri.PathSegments.Length == 4)
                return;


            temp = sdataUri.PathSegments[4].Text;


            if (temp.Equals(Constants.schema, StringComparison.InvariantCultureIgnoreCase))
            {
                this.RequestType = RequestType.ResourceSchema;      // -> Type: ResourceSchema
                return;
            }

            if (temp.Equals(Constants.template, StringComparison.InvariantCultureIgnoreCase))
            {
                this.RequestType = RequestType.Template;        // -> Type: template
                return;
            }

            if (temp.Equals(Constants.syncDigest, StringComparison.InvariantCultureIgnoreCase))
            {
                this.RequestType = RequestType.SyncDigest;          // -> Type: syncDigest
                return;
            }

            if (temp.Equals(Constants.syncResults, StringComparison.InvariantCultureIgnoreCase))
            {
                this.RequestType = RequestType.SyncResults;      // -> Type: SyncResults
                return;
            }

            if ((temp.Equals(Constants.syncSource, StringComparison.InvariantCultureIgnoreCase))
                || (temp.Equals(Constants.syncTarget, StringComparison.InvariantCultureIgnoreCase)))
            {
                if (temp.Equals(Constants.syncSource, StringComparison.InvariantCultureIgnoreCase))
                    this.RequestType = RequestType.SyncSource;              // -> Type: syncSource
                if (temp.Equals(Constants.syncTarget, StringComparison.InvariantCultureIgnoreCase))
                    this.RequestType = RequestType.SyncTarget;              // -> Type: syncTarget

                if (!string.IsNullOrEmpty(sdataUri.TrackingID))
                {
                    try
                    {
                        GuidConverter converter = new GuidConverter();

                        this.TrackingId = (Guid)converter.ConvertFrom(sdataUri.TrackingID);
                        this.HasTrackingId = true;
                    }
                    catch (Exception)
                    {
                    }
                }
                else if (this.SdataUri.PathSegments[4].HasPredicate)
                {
                    try
                    {
                        GuidConverter converter = new GuidConverter();
                        this.TrackingId = (Guid)converter.ConvertFrom(((StringLiteralExpression)this.SdataUri.PathSegments[4].PredicateExpression).Value);
                        this.HasTrackingId = true;
                    }
                    catch (Exception)
                    {
                    }
                }
                return;
            }
        }

        #endregion
    }
}
