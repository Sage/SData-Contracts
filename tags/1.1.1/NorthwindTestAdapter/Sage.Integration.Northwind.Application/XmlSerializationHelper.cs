#region Usings

using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

#endregion

namespace Sage.Integration.Northwind.Application
{
    public static class XmlSerializationHelpers
    {
        public static string SerializeElementToXml(object feedElement)
        {
            System.Xml.XmlDocument doc = new XmlDocument();
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(feedElement.GetType());
            
            using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
            {
                serializer.Serialize(stream, feedElement);
                stream.Position = 0;
                doc.Load(stream);
                return doc.DocumentElement.InnerXml;
            }

        }
        public static string SerializeObjectToXml(object obj)
        {
            if (null == obj)
                return null;

            try
            {
                String xml = null;
                MemoryStream memoryStream = new MemoryStream();

                XmlSerializer xs = new XmlSerializer(obj.GetType());

                XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);

                xs.Serialize(xmlTextWriter, obj);

                memoryStream = (MemoryStream)xmlTextWriter.BaseStream;

                xml = UTF8ByteArrayToString(memoryStream.ToArray());
                return xml;

            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return null;
            }
        }

        public static T DeserializeXmlToObject<T>(string xml)
        {
            return (T)DeserializeXmlToObject(typeof(T), xml);
        }
        public static object DeserializeXmlToObject(Type type, string xml)
        {
            XmlSerializer xmlSerializer;
            StringReader stringReader;
            object param;

            // Handle null or empty xml
            if ((xml == null) || (xml == String.Empty))
                return null;

            xmlSerializer = new XmlSerializer(type);

            // Get a new instance of the xml reader			
            using (stringReader = new StringReader(xml))
            {
                // Get the deserialized instace of the object
                param = xmlSerializer.Deserialize((TextReader)stringReader);
            }

            // Return the instance
            return param;
        }
        public static object DeserializeXmlToObject(object param, string xml)
        {
            return DeserializeXmlToObject(param.GetType(), xml);
        }

        #region Private Methods

        private static string UTF8ByteArrayToString(Byte[] characters)
        {

            UTF8Encoding encoding = new UTF8Encoding();
            String constructedString = encoding.GetString(characters);
            return (constructedString);
        }

        #endregion
    }
}
