#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Northwind.Adapter.Common;
using Sage.Integration.Messaging.Model;
using Sage.Integration.Northwind.Application;
using Sage.Sis.Sdata.Sync.Storage.Syndication;
using Sage.Sis.Sdata.Sync.Context;
using Sage.Sis.Sdata.Sync.Storage;
using Sage.Integration.Northwind.Feeds;
using System.Reflection;
using System.Xml.Schema;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

#endregion

namespace Sage.Integration.Northwind.Adapter.Common.Performers
{
    /// <summary>
    /// Performer that returns the general contract schema.
    /// </summary>
    internal class GetSchemaRequestPerformer : IRequestPerformer
    {
        #region Class Variables

        private RequestContext _requestContext;

        #endregion
        #region IRequestPerformer Members

        public void DoWork(IRequest request)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            XmlSchema xmlschema = null;
            using (StreamReader sr = new StreamReader(assembly.GetManifestResourceStream("Sage.Integration.Northwind.Adapter.crmErp.xsd")))
            {
                xmlschema = XmlSchema.Read(sr, null);
            }

            // replace common import url
            foreach (XmlSchemaImport schemaInportObj in xmlschema.Includes)
            {
                if (schemaInportObj.Namespace == "http://schemas.sage.com/sc/2009")
                {
                    schemaInportObj.SchemaLocation = string.Format("{0}{1}/import/common", _requestContext.DatasetLink, Constants.schema);
                    break;
                }
            }
        
            request.Response.ContentType = Sage.Common.Syndication.MediaType.Xml;
            request.Response.Xml = GetXml(xmlschema);
        

        }

        public void Initialize(RequestContext requestContext)
        {
            _requestContext = requestContext;
        }

        #endregion

        private string GetXml(XmlSchema schema)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(SerializeObjectToXml(schema));
            return @"<?xml version=""1.0"" encoding=""utf-8""?>"
                + xmlDoc.DocumentElement.OuterXml;
        }

        private string SerializeObjectToXml(object param)
        {
            XmlSerializer xmlSerializer;
            StringWriter stringWriter;
            string xml;

            // Handle null
            if (null == param)
                return null;

            xmlSerializer = new XmlSerializer(param.GetType());

            using (stringWriter = new StringWriter())
            {
                // Write the xml content into the memory stream
                xmlSerializer.Serialize((TextWriter)stringWriter, param);
                xml = stringWriter.ToString();
            }

            // Return the xml representation of the object
            return xml;
        }
    }
}
