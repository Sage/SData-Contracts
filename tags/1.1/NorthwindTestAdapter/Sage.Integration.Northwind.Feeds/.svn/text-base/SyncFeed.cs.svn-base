#region Usings

using System;
using System.Collections.Generic;
using System.Net;
using System.Xml.Schema;
using System.Xml.Serialization;
using Sage.Common.Syndication;
using Sage.Integration.Northwind.Common;
using Sage.Integration.Northwind.Feeds.Paging;

#endregion

namespace Sage.Integration.Northwind.Feeds
{
    [Serializable]
    [XmlInclude(typeof(Sage.Common.Syndication.FeedAuthor))]
    [XmlInclude(typeof(Sage.Common.Syndication.FeedGenerator))]
    [XmlInclude(typeof(Sage.Common.Syndication.FeedLinkCollection))]
    [XmlType("feed", Namespace=Namespaces.atomNamespace)]
    public class SyncFeed : Sage.Common.Syndication.IFeed
    {
        #region Ctor.

        public SyncFeed()
        {
            this.Author = new Sage.Common.Syndication.FeedAuthor();
            this.Entries = new List<SyncFeedEntry>();
            this.Generator = new Sage.Common.Syndication.FeedGenerator();
            this.Groups = new Sage.Common.Syndication.FeedGroupCollection();
            this.Links = new Sage.Common.Syndication.FeedLinkCollection();
            this.Sorts = new Sage.Common.Syndication.FeedSortCollection();
            this.HttpStatusCode = HttpStatusCode.OK;
        }

        #endregion

        #region Properties

        [XmlIgnore()]
        public FeedType FeedType { get; set; }

        [XmlIgnore]
        public Sage.Common.Syndication.FeedGenerator Generator { get; set; }
        
        [XmlIgnore]
        public Sage.Common.Syndication.FeedGroupCollection Groups { get; set; }
        
        [XmlIgnore]
        public HttpStatusCode HttpStatusCode { get; set; }

        [XmlIgnore]
        public Sage.Common.Syndication.FeedSortCollection Sorts { get; set; }

        /* OPENSEARCH */ 
        //[XmlIgnore]
        public Paging.IItemsPerPageElement Opensearch_ItemsPerPageElement { get; set; }
        //[XmlIgnore]
        public Paging.IStartIndexElement Opensearch_StartIndexElement { get; set; }
        //[XmlIgnore]
        public Paging.ITotalResultsElement Opensearch_TotalResultsElement { get; set; }

        [XmlElement("author", Namespace = Namespaces.atomNamespace)]
        public Sage.Common.Syndication.FeedAuthor Author  { get; set; }
        
        [XmlElement("digest", Namespace = Namespaces.syncNamespace)]
        public SyncFeedDigest Digest { get; set; }
        
        [XmlArrayItem(ElementName = "entry", Type = typeof(SyncFeedEntry), Namespace = Namespaces.atomNamespace)]
        public System.Collections.IList Entries { get; set; }
        
        [XmlElement("httpETag", Namespace = Namespaces.sdataNamespace)]
        public string HttpETag { get; set; }
        
        [XmlElement("id", Namespace = Namespaces.atomNamespace)]
        public string Id { get; set; }
        
        [XmlElement("link", Namespace = Namespaces.atomNamespace)]
        public Sage.Common.Syndication.FeedLinkCollection Links { get; set; }

        [XmlElement("subtitle", Namespace = Namespaces.atomNamespace)]
        public string SubTitle { get; set; }

        [XmlElement("title", Namespace = Namespaces.atomNamespace)]
        public string Title { get; set; }

        [XmlElement("version", Namespace = Namespaces.sdataNamespace)]
        public string Version { get; set; }
        
        #endregion


        [XmlNamespaceDeclarations()]
        [XmlIgnore()]
        private static XmlSerializerNamespaces SerializerNamespaces
        {
            get { return NameSpaceHelpers.SerializerNamespaces; }
        }

        #region IXmlSerializable Members

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            XmlSchema schema = new XmlSchema();
            schema.TargetNamespace = Namespaces.atomNamespace;
            return schema;
        }

        public void ReadXml(System.Xml.XmlReader reader, Type payloadType)
        {
            reader.MoveToContent();
            if ((reader.NodeType == System.Xml.XmlNodeType.Element)
                && (reader.LocalName == "feed")
                && (reader.NamespaceURI == Namespaces.atomNamespace))
            {
                bool reading = true;
                while (reading)
                {
                    if (reader.NodeType == System.Xml.XmlNodeType.Element)
                    {
                        switch(reader.LocalName)
                        {
                            case "title":
                                reading = reader.Read();
                                if (reader.NodeType == System.Xml.XmlNodeType.Text)
                                    this.Title = reader.Value;
                                break;
                            case "id":
                                reading = reader.Read();
                                if (reader.NodeType == System.Xml.XmlNodeType.Text)
                                    this.Id = reader.Value;
                                break;
                            case "entry":
                                SyncFeedEntry entry = new SyncFeedEntry();
                                entry.ReadXml(reader, payloadType);
                                this.Entries.Add(entry);
                                break;
                            case "digest":
                                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(SyncFeedDigest));
                                object obj = serializer.Deserialize(reader);
                                if (obj is SyncFeedDigest)
                                    this.Digest = obj as SyncFeedDigest;
                                break;
                            case "itemsPerPage":
                                reading = reader.Read();
                                if (reader.NodeType == System.Xml.XmlNodeType.Text)
                                {
                                    IItemsPerPageElement itemsPerPageElement = FeedComponentFactory.Create<IItemsPerPageElement>();
                                    itemsPerPageElement.LoadXmlValue(reader.Value);
                                }
                                break;
                            case "startIndex":
                                reading = reader.Read();
                                if (reader.NodeType == System.Xml.XmlNodeType.Text)
                                {
                                    IStartIndexElement startIndexElement = FeedComponentFactory.Create<IStartIndexElement>();
                                    startIndexElement.LoadXmlValue(reader.Value);
                                }
                                break;
                            case "totalResults":
                                reading = reader.Read();
                                if (reader.NodeType == System.Xml.XmlNodeType.Text)
                                {
                                    ITotalResultsElement totalResultsElement = FeedComponentFactory.Create<ITotalResultsElement>();
                                    totalResultsElement.LoadXmlValue(reader.Value);
                                }
                                break;
                            
                            default:
                                reading = reader.Read();
                                break;
                        }

                    }
                    else{
                        reading = reader.Read();
                    }
                }
            }

        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            if (FeedType != FeedType.ResourceEntry && FeedType != FeedType.LinkedSingle)
            {
                writer.WriteStartElement("feed", Namespaces.atomNamespace);
                writer.WriteElementString("title", Namespaces.atomNamespace, this.Title);
                writer.WriteElementString("id", Namespaces.atomNamespace, this.Id);
                foreach (FeedLink link in this.Links)
                {
                    writer.WriteStartElement("link", Namespaces.atomNamespace);
                    writer.WriteAttributeString("rel", link.LinkType.ToString().ToLower());
#warning get xmlenum descrition
                    writer.WriteAttributeString("type", link.Type.ToString());
                    writer.WriteAttributeString("title", link.Title);
                    writer.WriteAttributeString("href", link.Uri);
                    writer.WriteEndElement();
                }

                /* OPENSEARCH */
                if (null != this.Opensearch_ItemsPerPageElement)
                    writer.WriteRaw(this.Opensearch_ItemsPerPageElement.ToXml());
                if (null != this.Opensearch_StartIndexElement)
                    writer.WriteRaw(this.Opensearch_StartIndexElement.ToXml());
                if (null != this.Opensearch_TotalResultsElement)
                    writer.WriteRaw(this.Opensearch_TotalResultsElement.ToXml());

                #region SyncDigest

                if (this.FeedType == FeedType.SyncSource)
                {
                    System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(SyncFeedDigest));
                    serializer.Serialize(writer, this.Digest);
                }

                #endregion
            }
            foreach (object entryObj in this.Entries)
            {
                if (entryObj is SyncFeedEntry)
                    (entryObj as SyncFeedEntry).WriteXml(writer, this.FeedType);
            }
            if (FeedType != FeedType.ResourceEntry && FeedType != FeedType.LinkedSingle)
            {
                writer.WriteEndElement();
            }
        }

        #endregion
    }
}
