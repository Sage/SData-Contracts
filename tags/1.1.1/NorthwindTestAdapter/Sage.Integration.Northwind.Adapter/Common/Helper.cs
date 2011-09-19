#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Northwind.Application.API;
using System.Net;
using System.ComponentModel;
using Sage.Sis.Sdata.Sync.Storage.Syndication;

using Sage.Integration.Northwind.Sync.Syndication;
using Sage.Common.Syndication;

#endregion

namespace Sage.Integration.Northwind.Adapter.Common
{
    public static class Helper
    {
        //public static ResourcePayloadContainerLink FindLinkByPayloadPath(ResourcePayloadContainerLink[] links, string payloadPath)
        //{
        //    foreach(ResourcePayloadContainerLink link in links)
        //    {
        //        if (link.PayloadPath.Equals(payloadPath))
        //            return link;
        //    }

        //    return null;
        //}

        //public static List<ResourcePayloadContainerLink> ReducePayloadPath(List<ResourcePayloadContainerLink> links)
        //{
        //    List<ResourcePayloadContainerLink> result = new List<ResourcePayloadContainerLink>();
        //    foreach (ResourcePayloadContainerLink originalLink in links)
        //        result.Add(originalLink.Clone() as ResourcePayloadContainerLink);

        //    foreach(ResourcePayloadContainerLink link in result)
        //    {
        //        if (!String.IsNullOrEmpty(link.PayloadPath))
        //        {
        //            if (link.PayloadPath.Contains("/") && link.PayloadPath.Length> 1)
        //            {
        //                link.PayloadPath = link.PayloadPath.Substring(link.PayloadPath.IndexOf("/") + 1);
        //            }
        //                else
        //                link.PayloadPath = "";
        //        }
        //    }
        //    return result;
        //}

        //public static List<ResourcePayloadContainerLink> ExtendPayloadPath(List<ResourcePayloadContainerLink> links, string payload)
        //{
        //    List<ResourcePayloadContainerLink> result = new List<ResourcePayloadContainerLink>();
        //    foreach (ResourcePayloadContainerLink originalLink in links)
        //        result.Add(originalLink.Clone() as ResourcePayloadContainerLink);

        //    foreach (ResourcePayloadContainerLink link in result)
        //    {
        //        if (!String.IsNullOrEmpty(link.PayloadPath))
        //        {
        //                link.PayloadPath = payload + "/" + link.PayloadPath;
        //        }
        //    }
        //    return result;
        //}

        public static HttpMethod GetHttpMethod(TransactionAction transactionAction)
        {
            switch (transactionAction)
            {
                case TransactionAction.Created:
                    return HttpMethod.POST;
                case TransactionAction.Updated:
                    return HttpMethod.PUT;
                case TransactionAction.Deleted:
                default:
                    return HttpMethod.DELETE;
            }
        }

        public static HttpStatusCode GetHttpStatus(TransactionStatus transactionStatus, TransactionAction transactionAction)
        {
            switch (transactionStatus)
            {
                case TransactionStatus.Success:
                    //switch (transactionAction)
                    //{
                    //    case TransactionAction.Created:
                    //        return HttpStatusCode.Created;
                    //    case TransactionAction.Updated:
                    //        return HttpStatusCode.Accepted;
                    //    case TransactionAction.Deleted:
                    //        return HttpStatusCode.Accepted;
                    //}
                    return HttpStatusCode.OK;
                    //break;
            }
            return HttpStatusCode.Conflict;
        }

        /*internal static SdataTransactionResult GetSdataTransactionResult(List<TransactionResult> transactions, string EndPoint, SupportedResourceKinds resource)
        {
            if (transactions == null)
                return null;
            if (transactions.Count == 0)
                return null;

            SdataTransactionResult result;
            foreach (TransactionResult transaction in transactions)
            {
                if (transaction.Status != TransactionStatus.Success)
                {
                    result = new SdataTransactionResult();
                    result.ResourceKind = resource;
                    result.LocalId = transaction.ID;
                    result.Uuid = transaction.CRMID;
                    result.HttpMessage = transaction.Message;
                    result.Location = EndPoint + "('" + transaction.ID + "')";
                    result.HttpStatus = Helper.GetHttpStatus(transaction.Status, transaction.Action);
                    result.HttpMethod = Helper.GetHttpMethod(transaction.Action);
                    return result;
                }
            }
            if (transactions.Count == 0)
                return null;

            result = new SdataTransactionResult();
            result.ResourceKind = resource;
            result.LocalId = transactions[0].ID;

            result.Uuid = transactions[0].CRMID;

            result.HttpMessage = transactions[0].Message;
            result.Location = EndPoint + "('" + transactions[0].ID + "')";
            result.HttpStatus = Helper.GetHttpStatus(transactions[0].Status, transactions[0].Action);
            result.HttpMethod = Helper.GetHttpMethod(transactions[0].Action);
            return result;

        }*/

        public static SyncState GetSyncState(CorrelatedResSyncInfo corelation)
        {
            SyncState result = new SyncState();
            result.EndPoint = corelation.ResSyncInfo.EndPoint;
            result.Stamp = corelation.ResSyncInfo.ModifiedStamp;
            result.Tick = (corelation.ResSyncInfo.Tick>0)?corelation.ResSyncInfo.Tick:1;
            return result;
        }

        public static SyncState GetSyncState(Digest digest, string EndPoint)
        {
            if (digest.Entries != null)
            {
                foreach (DigestEntry entry in digest.Entries)
                {
                    if ((entry.EndPoint != null) && (entry.EndPoint.Equals(EndPoint, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        SyncState result = new SyncState();
                        result.EndPoint = entry.EndPoint;
                        result.Stamp = entry.Stamp;
                        result.Tick = (entry.Tick > 0) ?(int)entry.Tick : 1;
                        return result;
                    }
                }
            }
            return null;
        }

        public static int GetConflictPriority(Digest digest, string EndPoint)
        {
            foreach (DigestEntry entry in digest.Entries)
                if ((entry.EndPoint != null) && (entry.EndPoint.Equals(EndPoint, StringComparison.InvariantCultureIgnoreCase)))
                    return entry.ConflictPriority;

            return 10000;
        }

         public static SyncState GetSyncState(SyncDigestInfo digest, string EndPoint)
        {
            foreach (SyncDigestEntryInfo entry in digest)
            {
                if ((entry.EndPoint != null) && (entry.EndPoint.Equals(EndPoint, StringComparison.InvariantCultureIgnoreCase)))
                {
                    SyncState result = new SyncState();
                    result.EndPoint = entry.EndPoint;
                    //result.Stamp = entry.Stamp;
                    result.Tick = (entry.Tick>0)?entry.Tick:1;
                    return result;
                }
            }
            return null;
        }

         public static int GetConflictPriority(SyncDigestInfo digest, string EndPoint)
         {
             foreach (SyncDigestEntryInfo entry in digest)
                 if ((entry.EndPoint != null) && (entry.EndPoint.Equals(EndPoint, StringComparison.InvariantCultureIgnoreCase)))
                     return entry.ConflictPriority;

             return 10000;
         }
    }
}
