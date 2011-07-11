#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Northwind.Application.API;
using System.Net;
using System.ComponentModel;
using Sage.Integration.Northwind.Feeds;
using Sage.Sis.Sdata.Sync.Storage.Syndication;
using Sage.Integration.Northwind.Feeds.TradingAccounts;
using Sage.Integration.Northwind.Feeds.SalesOrders;
using Sage.Integration.Northwind.Sync.Syndication;

#endregion

namespace Sage.Integration.Northwind.Adapter.Common
{
    public static class Helper
    {
        public static SyncFeedEntryLink FindLinkByPayloadPath(SyncFeedEntryLink[] links, string payloadPath)
        {
            foreach(SyncFeedEntryLink link in links)
            {
                if (link.PayloadPath.Equals(payloadPath))
                    return link;
            }

            return null;
        }

        public static List<SyncFeedEntryLink> ReducePayloadPath(List<SyncFeedEntryLink> links)
        {
            List<SyncFeedEntryLink> result = new List<SyncFeedEntryLink>();
            foreach (SyncFeedEntryLink originalLink in links)
                result.Add(originalLink.Clone() as SyncFeedEntryLink);

            foreach(SyncFeedEntryLink link in result)
            {
                if (!String.IsNullOrEmpty(link.PayloadPath))
                {
                    if (link.PayloadPath.Contains("/") && link.PayloadPath.Length> 1)
                    {
                        link.PayloadPath = link.PayloadPath.Substring(link.PayloadPath.IndexOf("/") + 1);
                    }
                        else
                        link.PayloadPath = "";
                }
            }
            return result;
        }

        public static List<SyncFeedEntryLink> ExtendPayloadPath(List<SyncFeedEntryLink> links, string payload)
        {
            List<SyncFeedEntryLink> result = new List<SyncFeedEntryLink>();
            foreach (SyncFeedEntryLink originalLink in links)
                result.Add(originalLink.Clone() as SyncFeedEntryLink);

            foreach (SyncFeedEntryLink link in result)
            {
                if (!String.IsNullOrEmpty(link.PayloadPath))
                {
                        link.PayloadPath = payload + "/" + link.PayloadPath;
                }
            }
            return result;
        }

        internal static string GetHttpMethod(TransactionAction transactionAction)
        {
            switch (transactionAction)
            {
                case TransactionAction.Created:
                    return "POST";
                case TransactionAction.Updated:
                    return "PUT";
                case TransactionAction.Deleted:
                    return "DELETE";
            }
            return "";
        }

        internal static HttpStatusCode GetHttpStatus(TransactionStatus transactionStatus, TransactionAction transactionAction)
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

        internal static SdataTransactionResult GetSdataTransactionResult(List<TransactionResult> transactions, string endpoint, SupportedResourceKinds resource)
        {
            if (transactions == null)
                return null;
            if (transactions.Count == 0)
                return null;

            GuidConverter guidConverter = new GuidConverter();
            SdataTransactionResult result;
            foreach (TransactionResult transaction in transactions)
            {
                if (transaction.Status != TransactionStatus.Success)
                {
                    result = new SdataTransactionResult();
                    result.ResourceKind = resource;
                    result.LocalId = transaction.ID;
                    try
                    {
                        result.Uuid = (Guid)guidConverter.ConvertFromString(transaction.CRMID);
                    }
                    catch (Exception)
                    { }
                    result.HttpMessage = transaction.Message;
                    result.Location = endpoint + "('" + transaction.ID + "')";
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
            try
            {
                result.Uuid = (Guid)guidConverter.ConvertFromString(transactions[0].CRMID);
            }
            catch (Exception)
            { }
            result.HttpMessage = transactions[0].Message;
            result.Location = endpoint + "('" + transactions[0].ID + "')";
            result.HttpStatus = Helper.GetHttpStatus(transactions[0].Status, transactions[0].Action);
            result.HttpMethod = Helper.GetHttpMethod(transactions[0].Action);
            return result;

        }

        public static SyncState GetSyncState(CorrelatedResSyncInfo corelation)
        {
            SyncState result = new SyncState();
            result.Endpoint = corelation.ResSyncInfo.Endpoint;
            result.Stamp = corelation.ResSyncInfo.ModifiedStamp;
            result.Tick = corelation.ResSyncInfo.Tick;
            return result;
        }

        public static SyncState GetSyncState(SyncFeedDigest digest, string endpoint)
        {
            foreach (SyncFeedDigestEntry entry in digest.Entries)
            {
                if ((entry.Endpoint != null) && (entry.Endpoint.Equals(endpoint, StringComparison.InvariantCultureIgnoreCase)))
                {
                    SyncState result = new SyncState();
                    result.Endpoint = entry.Endpoint;
                    result.Stamp = entry.Stamp;
                    result.Tick = entry.Tick;
                    return result;
                }
            }
            return null;
        }

        public static int GetConflictPriority(SyncFeedDigest digest, string endpoint)
        {
            foreach (SyncFeedDigestEntry entry in digest.Entries)
                if ((entry.Endpoint != null) && (entry.Endpoint.Equals(endpoint, StringComparison.InvariantCultureIgnoreCase)))
                    return entry.ConflictPriority;

            return 10000;
        }

         public static SyncState GetSyncState(SyncDigestInfo digest, string endpoint)
        {
            foreach (SyncDigestEntryInfo entry in digest)
            {
                if ((entry.Endpoint != null) && (entry.Endpoint.Equals(endpoint, StringComparison.InvariantCultureIgnoreCase)))
                {
                    SyncState result = new SyncState();
                    result.Endpoint = entry.Endpoint;
                    //result.Stamp = entry.Stamp;
                    result.Tick = entry.Tick;
                    return result;
                }
            }
            return null;
        }

         public static int GetConflictPriority(SyncDigestInfo digest, string endpoint)
         {
             foreach (SyncDigestEntryInfo entry in digest)
                 if ((entry.Endpoint != null) && (entry.Endpoint.Equals(endpoint, StringComparison.InvariantCultureIgnoreCase)))
                     return entry.ConflictPriority;

             return 10000;
         }
    }
}
