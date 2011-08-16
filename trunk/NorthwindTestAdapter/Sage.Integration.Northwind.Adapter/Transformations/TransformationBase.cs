#region Usings

using System;
using System.ComponentModel;
using Sage.Integration.Northwind.Adapter.Common;
using Sage.Integration.Northwind.Application;
using Sage.Sis.Sdata.Sync.Storage;
using Sage.Sis.Sdata.Sync.Storage.Syndication;
using Sage.Integration.Northwind.Sync;

#endregion

namespace Sage.Integration.Northwind.Adapter.Transformations
{
    public abstract class TransformationBase
    {
        #region Class Variables

        private readonly SupportedResourceKinds _resourceKind;
        private readonly string _resourceKindString;
        private readonly string _originApplication;

        protected readonly RequestContext _context;
        protected readonly NorthwindConfig _config;
        protected readonly string _datasetLink;
        protected readonly ICorrelatedResSyncInfoStore _correlatedResSyncInfoStore;

        #endregion

        #region Ctor.

        public TransformationBase(RequestContext context, SupportedResourceKinds resourceKind)
        {
            _context = context;
            _datasetLink = _context.DatasetLink;
            _config = _context.Config;
            _resourceKind = resourceKind;

            _resourceKindString = _resourceKind.ToString();
            _originApplication = _datasetLink + _resourceKindString;

            _correlatedResSyncInfoStore = NorthwindAdapter.StoreLocator.GetCorrelatedResSyncStore(_context.SdataContext);
        }

        #endregion

        protected Guid StringToGuid(string guid)
        {
            try
            {
                GuidConverter converter = new GuidConverter();

                Guid result = (Guid)converter.ConvertFromString(guid);
                return result;
            }
            catch
            {
                return Guid.Empty;
            }

        }
        
        protected CorrelatedResSyncInfo GetUuidAsGuid(string localId, string uuidString)
        {
            if (String.IsNullOrEmpty(localId))
            {
                return null;
            }

           CorrelatedResSyncInfo[] results =  _correlatedResSyncInfoStore.GetByLocalId(_resourceKindString, new string[]{localId});
           if (results.Length > 0)
               return results[0];
            Guid result;
            if (string.IsNullOrEmpty(uuidString))
                   result = Guid.NewGuid();
            else
                try
                {
                    GuidConverter converter = new GuidConverter();
                    result = (Guid)converter.ConvertFromString(uuidString);
                    if (Guid.Empty.Equals(result))
                        result = Guid.NewGuid();
                }
                catch (Exception)
                {
                    result = Guid.NewGuid();
                }
                ResSyncInfo newResSyncInfo = new ResSyncInfo(result, _originApplication, 0, string.Empty, DateTime.Now);
                CorrelatedResSyncInfo syncInfo = new CorrelatedResSyncInfo(localId, newResSyncInfo);
                _correlatedResSyncInfoStore.Put(_resourceKindString, syncInfo);
                return syncInfo;
        }

        protected string GetLocalId(string uuidString)
        {
            GuidConverter converter = new GuidConverter();
            try
            {
                Guid uuid = (Guid)converter.ConvertFromString(uuidString);
                return GetLocalId(uuid);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        protected string GetLocalId(Guid uuid)
        {
            CorrelatedResSyncInfo[] results = _correlatedResSyncInfoStore.GetByUuid(_resourceKindString, new Guid[] { uuid });
            if (results.Length > 0)
                return results[0].LocalId;
            return null;
        }

        protected bool GuidIsNullOrEmpty(Guid guid)
        {
            return guid == null || guid == Guid.Empty;
        }

        protected string GetSDataId(string id)
        {
            return String.Format("{0}{1}('{2}')", _context.DatasetLink, _resourceKind.ToString(), id);
        }

        protected void SetCommonProperties(Sage.Integration.Northwind.Application.Base.Document document, string descriptor, Sage.Common.Syndication.FeedEntry entry)
        {
            entry.Id = GetSDataId(document.Id);
            entry.Key = document.Id;
            CorrelatedResSyncInfo syncInfo = GetUuidAsGuid(document.Id, document.CrmId);
            entry.UUID = syncInfo.ResSyncInfo.Uuid;
            entry.Updated = syncInfo.ResSyncInfo.ModifiedStamp.ToLocalTime();
            entry.Descriptor = descriptor;
        }
    }
}
