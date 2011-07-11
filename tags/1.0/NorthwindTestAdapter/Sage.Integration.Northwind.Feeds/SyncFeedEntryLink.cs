#region Usings

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace Sage.Integration.Northwind.Feeds
{
    #region ENUM: RelEnum

    public enum RelEnum
    {
        self,
        //edit,
        related,
        //http://schemas.sage.com/sdata/link-relations/schema,
        schema,
        ////http://schemas.sage.com/sdata/link-relations/template
        //template,
    }

    #endregion

    #region ENUM: LinkTypeEnum

    public enum LinkTypeEnum
    {
        entry, //application/atom+xml; type=entry
        xml, //application/xml
        feed //application/atom+xml; type=feed
    }

    #endregion

    public class SyncFeedEntryLink : ICloneable
    {
        public static SyncFeedEntryLink CreateSelfLink(string href)
        {
            SyncFeedEntryLink result = new SyncFeedEntryLink();
            result.Href = href;
            result.LinkRel = GetRelString(RelEnum.self);
            result.Title = "self";
            result.LinkType = GetTypeString(LinkTypeEnum.entry);
            return result;
        }

        public static SyncFeedEntryLink CreateSchemaLink(string href)
        {
            SyncFeedEntryLink result = new SyncFeedEntryLink();
            result.Href = href;
            result.LinkRel = GetRelString(RelEnum.schema);
            result.Title = "Schema";
            result.LinkType = GetTypeString(LinkTypeEnum.xml);
            return result;
        }

        public static SyncFeedEntryLink CreateRelatedLink(string href, string title ,string payloadPath, string uuid)
        {
            SyncFeedEntryLink result = new SyncFeedEntryLink();
            result.Href = href;
            result.LinkRel = GetRelString(RelEnum.related);
            result.Title = title;
            result.LinkType = GetTypeString(LinkTypeEnum.entry);
            result.PayloadPath = payloadPath;
            result.Uuid = uuid;
            return result;
        }

        public static string GetRelString(RelEnum value)
        {
            switch (value)
            {
                case(RelEnum.schema):
                    return @"http://schemas.sage.com/sdata/link-relations/template";
                default:
                    return value.ToString();
            }
        }

        public static string GetTypeString(LinkTypeEnum value)
        {
            switch (value)
            {
                case (LinkTypeEnum.feed):
                    return @"application/atom+xml;type=feed";
                case (LinkTypeEnum.entry):
                    return @"application/atom+xml;type=entry";
                case (LinkTypeEnum.xml):
                    return @"application/xml";
                default:
                    return "";
            }
        }

        #region Properties

        public string LinkRel { get; set; }
       
        public string LinkType { get; set; }

        public string Title { get; set; }
        
        public string PayloadPath { get; set; }
        
        public string Uuid { get; set; }
       
        public string Href { get; set; }

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            SyncFeedEntryLink clone = new SyncFeedEntryLink();
            clone.Href = this.Href;
            clone.LinkRel = this.LinkRel;
            clone.LinkType = this.LinkType;
            clone.PayloadPath = this.PayloadPath;
            clone.Title = this.Title;
            clone.Uuid = this.Uuid;
            return clone;
        }

        #endregion
    }
}
