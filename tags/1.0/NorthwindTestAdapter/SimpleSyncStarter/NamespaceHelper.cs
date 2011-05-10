using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace SimpleSyncStarter
{
    public static class NameSpaceHelper
    {
        //public static string PayloadXpath
        //{
        //    get
        //    {
        //        if (Properties.Settings.Default.UseNoPayloadTag)
        //            return "";
        //        return String.Format(@"{0}:payload", NameSpaceHelper.SDataNamespace.prefix);

        //    }
        //}


        public struct SmeNamespace
        {
            public const string prefix = "sme";
            public const string uri = @"http://schemas.sage.com/sdata/sme/2007";
        }

        public struct XsNamespace
        {
            public const string prefix = "xs";
            public const string uri = @"http://www.w3.org/2001/XMLSchema";
        }

        public struct AtomNamespace
        {
            public const string prefix = "atom";
            public const string uri = @"http://www.w3.org/2005/Atom";
        }


        public struct SDataNamespace
        {
            public const string prefix = "sdata";
            public const string uri = @"http://schemas.sage.com/sdata/2008/1";
            //public const string uri = @"http://schemas.sage.com/sdata/2007";
        }

        public struct OpenSearchNamespace
        {
            public const string prefix = "opensearch";
            public const string uri = @"http://a9.com/-/spec/opensearch/1.1/";
        }

        public struct SyncNamespace
        {
            public const string prefix = "sync";
            public const string uri = @"http://schemas.sage.com/sdata/sync/2008/1";
        }

        public static XmlNamespaceManager CreateNamespaceManager(XmlNameTable nt)
        {
            XmlNamespaceManager result = new XmlNamespaceManager(nt);

            result.AddNamespace(AtomNamespace.prefix, AtomNamespace.uri);
            result.AddNamespace(SmeNamespace.prefix, SmeNamespace.uri);
            //if (Properties.Settings.Default.OldNamespace)
            //    result.AddNamespace(SDataNamespace.prefix, SDataNamespace.oldUri);
            //else
                result.AddNamespace(SDataNamespace.prefix, SDataNamespace.uri);
            result.AddNamespace(XsNamespace.prefix, XsNamespace.uri);
            result.AddNamespace(OpenSearchNamespace.prefix, OpenSearchNamespace.uri);
            result.AddNamespace(SyncNamespace.prefix, SyncNamespace.uri);

            return result;
        }

    }
}
