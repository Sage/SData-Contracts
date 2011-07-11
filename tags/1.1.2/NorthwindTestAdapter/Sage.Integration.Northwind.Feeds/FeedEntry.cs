#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Xml;
using System.Xml.Serialization;
using Sage.Common.Syndication;
using Sage.Integration.Northwind.Common;
using Sage.Integration.Northwind.Sync.Syndication;
using Sage.Integration.Northwind.Feeds.TradingAccounts;

#endregion

namespace Sage.Integration.Northwind.Feeds
{
    public class SyncFeedEntry: FeedEntry
    {
        #region Ctor.

        public SyncFeedEntry()
        {
            this.SyncState = new SyncState();
            this.SyncLinks = new List<SyncFeedEntryLink>();
            this.IsUuidSet = false;
        }

        #endregion

        #region Linked Extensions

        [XmlElement("linked", Namespace = Namespaces.syncNamespace)]
        public LinkedElement Linked { get; set; }

        private Guid _uuid;
        [XmlIgnore]
        public bool IsUuidSet { get; set; }
        [XmlElement(ElementName = "uuid", Namespace = Namespaces.syncNamespace)]
        public Guid Uuid
        {
            get { return _uuid; }
            set { _uuid = value; this.IsUuidSet = true; }
        }

        #endregion

        [XmlNamespaceDeclarations()]
        [XmlIgnore()]
        private static XmlSerializerNamespaces SerializerNamespaces
        {
            get { return NameSpaceHelpers.SerializerNamespaces; }
        }

        public List<SyncFeedEntryLink> SyncLinks { get; set; }
                
        [XmlElement(ElementName = "syncState", Namespace = Namespaces.syncNamespace)]
        public SyncState SyncState { get; set; }

        [XmlElement(ElementName = "payload", Namespace = Namespaces.sdataNamespace)]
        public PayloadBase Payload { get; set; }

        [XmlElement(ElementName = "httpMessage", Namespace = Namespaces.sdataHttpNamespace)]
        public string HttpMessage { get; set; }

        #region Serialization Helpers

        public void ReadXml(System.Xml.XmlReader reader, Type payloadType)
        {
            GuidConverter guidConverter = new GuidConverter();
            if ((reader.NodeType == System.Xml.XmlNodeType.Element)
                && (reader.LocalName == "entry")
                && (reader.NamespaceURI == Namespaces.atomNamespace))
            {
                bool reading = true;
                while (reading)
                {
                    if (reader.NodeType == System.Xml.XmlNodeType.Element)
                    {
                        switch (reader.LocalName)
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

                            case "uuid":
                                reading = reader.Read();
                                if (reader.NodeType == System.Xml.XmlNodeType.Text)
                                {
                                    string uuidString = reader.Value;

                                    this.Uuid = (Guid)guidConverter.ConvertFromString(uuidString);
                                }
                                break;

                            case "httpStatus":
                                reading = reader.Read();
                                if (reader.NodeType == System.Xml.XmlNodeType.Text)
                                    this.HttpStatusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), reader.Value);
                                break;

                            case "httpMessage":
                                reading = reader.Read();
                                if (reader.NodeType == System.Xml.XmlNodeType.Text)
                                    this.HttpMessage = reader.Value;
                                break;
                                
                            case "location":
                                reading = reader.Read();
                                if (reader.NodeType == System.Xml.XmlNodeType.Text)
                                    this.HttpLocation = reader.Value;
                                break;

                            case "httpMethod":
                                reading = reader.Read();
                                if (reader.NodeType == System.Xml.XmlNodeType.Text)
                                    this.HttpMethod = reader.Value;
                                break;

                            case "payload":
                                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(payloadType);
                                object obj = serializer.Deserialize(reader);
                                if (obj is PayloadBase)
                                    this.Payload = obj as PayloadBase;
                                break;

                            case "syncState":
                                System.Xml.Serialization.XmlSerializer syncStateSerializer = new System.Xml.Serialization.XmlSerializer(typeof(SyncState));
                                object obj1 = syncStateSerializer.Deserialize(reader);
                                if (obj1 is SyncState)
                                    this.SyncState = obj1 as SyncState;
                                break;
                            case "link":

                                if (reader.HasAttributes)
                                {
                                    SyncFeedEntryLink link = new SyncFeedEntryLink();
                                    while (reader.MoveToNextAttribute())
                                    {
                                        if (reader.LocalName.Equals("payloadpath", StringComparison.InvariantCultureIgnoreCase))
                                            link.PayloadPath = reader.Value;
                                        if (reader.LocalName.Equals("rel", StringComparison.InvariantCultureIgnoreCase))
                                            link.LinkRel = reader.Value;
                                        if (reader.LocalName.Equals("type", StringComparison.InvariantCultureIgnoreCase))
                                            link.LinkType = reader.Value;
                                        if (reader.LocalName.Equals("title", StringComparison.InvariantCultureIgnoreCase))
                                            link.Title = reader.Value;
                                        if (reader.LocalName.Equals("uuid", StringComparison.InvariantCultureIgnoreCase))
                                            link.Uuid = reader.Value;
                                        if (reader.LocalName.Equals("href", StringComparison.InvariantCultureIgnoreCase))
                                            link.Href = reader.Value;
                                    }

                                    this.SyncLinks.Add(link);

                                }
                                break;
                            case "linked":
                                System.Xml.Serialization.XmlSerializer linkedSerializer = new System.Xml.Serialization.XmlSerializer(typeof(LinkedElement));
                                object linkedObj = linkedSerializer.Deserialize(reader);
                                if (linkedObj is LinkedElement)
                                    this.Linked = linkedObj as LinkedElement;
                                break;
                                

                            default:
                                reading = reader.Read();
                                break;
                        }

                    }
                    else
                    {
                        if ((reader.NodeType == System.Xml.XmlNodeType.EndElement)
                            && (reader.LocalName == "entry"))
                            reading = false;
                        else
                            reading = reader.Read();
                    }
                }
            }
        }

        public void WriteXml(System.Xml.XmlWriter writer, FeedType feedType)
        {

            writer.WriteStartElement("entry", Namespaces.atomNamespace);
            // id
            writer.WriteElementString("id", Namespaces.atomNamespace, this.Id);
            // title
            writer.WriteElementString("title", Namespaces.atomNamespace, this.Title);

            // uuid
            if (this.IsUuidSet)
                writer.WriteElementString("uuid", Namespaces.syncNamespace, this.Uuid.ToString());

            foreach (SyncFeedEntryLink link in this.SyncLinks)
            {
                writer.WriteStartElement("link", Namespaces.atomNamespace);
                if (!String.IsNullOrEmpty(link.LinkRel))
                    writer.WriteAttributeString("rel", link.LinkRel);
                if (!String.IsNullOrEmpty(link.LinkType))
                    writer.WriteAttributeString( "type",  link.LinkType);
                if (!String.IsNullOrEmpty(link.Href))
                    writer.WriteAttributeString( "href",  link.Href);
                if (!String.IsNullOrEmpty(link.Title))
                    writer.WriteAttributeString( "title",  link.Title);
                if (!String.IsNullOrEmpty(link.PayloadPath ))
                    writer.WriteAttributeString(Namespaces.sdataPrefix, "payloadPath", Namespaces.sdataNamespace, link.PayloadPath);
                if (!String.IsNullOrEmpty(link.Uuid))
                    writer.WriteAttributeString(Namespaces.syncPrefix, "uuid", Namespaces.syncNamespace, link.Uuid);
                writer.WriteEndElement();
            }

            switch (feedType)
            {
                case FeedType.Resource:
                    break;
                case FeedType.SyncSource:
                    //<!-- Per-Resource Synchronization State -->
                    System.Xml.Serialization.XmlSerializer syncStateSerializer = new System.Xml.Serialization.XmlSerializer(typeof(SyncState));
                    syncStateSerializer.Serialize(writer, this.SyncState);
                    if (!String.IsNullOrEmpty(this.HttpMethod))
                        writer.WriteElementString("httpMethod", Namespaces.sdataHttpNamespace, this.HttpMethod);
                    break;
                case FeedType.SyncTarget:
                    writer.WriteElementString("httpStatus", Namespaces.sdataHttpNamespace, Convert.ToInt32(this.HttpStatusCode).ToString());
                    writer.WriteElementString("httpMessage", Namespaces.sdataHttpNamespace, this.HttpMessage);
                    writer.WriteElementString("location", Namespaces.sdataHttpNamespace, this.HttpLocation);
                    writer.WriteElementString("httpMethod", Namespaces.sdataHttpNamespace, this.HttpMethod);

                    break;
                case FeedType.Linked:
                case FeedType.LinkedSingle:
                    System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(LinkedElement));
                    serializer.Serialize(writer, this.Linked);

                    break;
            }
            // Content ??
            // links  ??

            if (null != this.HttpETag)
                writer.WriteElementString("etag", Namespaces.sdataHttpNamespace, this.HttpETag);

            //<!-- XML payload -->
            if (this.Payload != null)
            {
                System.Xml.Serialization.XmlSerializer serializer;
                //if (this.Payload is TradingAccountPayload)
                serializer = new System.Xml.Serialization.XmlSerializer(this.Payload.GetType());
                //else
                    //serializer = new System.Xml.Serialization.XmlSerializer(typeof(PayloadBase));
                serializer.Serialize(writer, this.Payload);
            }
            writer.WriteEndElement();
        }

        #endregion
    }
}
