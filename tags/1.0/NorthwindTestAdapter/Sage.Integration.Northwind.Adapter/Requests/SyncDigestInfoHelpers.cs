#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Sis.Sdata.Sync.Storage.Syndication;
using System.Xml;
using System.IO;

using Sage.Integration.Northwind.Common;

#endregion

namespace Sage.Integration.Northwind.Adapter.Common
{
    internal static class SyncDigestInfoHelpers
    {
        public static string ToXml(SyncDigestInfo info, string endpoint)
        {
            StringBuilder sbDigest = new StringBuilder();
            sbDigest.Append(@"<?xml version=""1.0"" encoding=""utf-8"" ?>");
            sbDigest.Append(string.Format(@"<digest xmlns=""{0}"" mark=""current"">", Namespaces.syncNamespace));
            sbDigest.Append(@"<origin>");
            sbDigest.Append(endpoint);
            sbDigest.Append(@"</origin>");


            foreach (SyncDigestEntryInfo digestEntry in info)
            {
                sbDigest.Append(@"<digestEntry>");
                sbDigest.Append(@"<endpoint>");
                sbDigest.Append(digestEntry.Endpoint);
                sbDigest.Append(@"</endpoint>");
                sbDigest.Append(@"<tick>");
                sbDigest.Append(digestEntry.Tick.ToString());
                sbDigest.Append(@"</tick>");
                sbDigest.Append(@"<conflictPriority>");
                sbDigest.Append(digestEntry.ConflictPriority);
                sbDigest.Append(@"</conflictPriority>");
                sbDigest.Append(@"</digestEntry>");
            }
            sbDigest.Append(@"</digest>");
            return sbDigest.ToString();
        }

        //public static SyncDigestInfo Load(XmlDocument doc)
        //{
        //    SyncDigestInfo result = new SyncDigestInfo();
        //    XmlNamespaceManager nsMgr = new XmlNamespaceManager(doc.NameTable);

        //    nsMgr.AddNamespace(Namespaces.syncPrefix, Namespaces.syncNamespace);
        //    foreach (XmlNode node in doc.DocumentElement.SelectNodes(string.Format("{0}:digestEntry", Namespaces.syncPrefix), nsMgr))
        //    {
        //        string endpoint = node.SelectSingleNode(string.Format("{0}:endpoint", Namespaces.syncPrefix), nsMgr).InnerText;
        //        int tick = Convert.ToInt32(node.SelectSingleNode(string.Format("{0}:tick", Namespaces.syncPrefix), nsMgr).InnerText);
        //        int conflictPriority = Convert.ToInt32(node.SelectSingleNode(string.Format("{0}:conflictPriority", Namespaces.syncPrefix), nsMgr).InnerText);
        //        DateTime stamp = XmlConvert.ToDateTime(node.SelectSingleNode(string.Format("{0}:stamp", Namespaces.syncPrefix), nsMgr).InnerText);

        //        SyncDigestEntryInfo entry = new SyncDigestEntryInfo(endpoint, tick, conflictPriority, stamp);
        //        result.Add(entry);
        //    }
        //    return result;

        //}
    }
}
