#region Usings

using System.IO;
using System.Reflection;
using System.Xml.Schema;
using Sage.Integration.Messaging.Model;
using System;
using System.Xml;
using System.Xml.Serialization;

#endregion

namespace Sage.Integration.Northwind.Adapter.Common.Performers
{
    /// <summary>
    /// Performer that returns an additional imported schema. (i.e. common.xsd)
    /// </summary>
    internal class GetSchemaImportRequestPerformer : IRequestPerformer
    {
        #region Class Variables

        private RequestContext _requestContext;

        #endregion

        #region IRequestPerformer Members

        public void DoWork(IRequest request)
        {
            string importSchemaName;
            string manifestResourceName;
            Assembly executingAssembly;
            XmlSchema xmlschema = null;
            
            // get the schema name to be imported from url
            importSchemaName = _requestContext.ImportSchemaName;
            // build the manifestResourceName
            manifestResourceName = string.Format("Sage.Integration.Northwind.Adapter.{0}.xsd", importSchemaName);
            

            executingAssembly = Assembly.GetExecutingAssembly();

            // receive the schema from manifest resource stream
            // and read it into an XmlSchema
            try
            {
                using (StreamReader sr = new StreamReader(executingAssembly.GetManifestResourceStream(manifestResourceName)))
                {
                    xmlschema = XmlSchema.Read(sr, null);
                }
            }
            catch (Exception exception)
            {
                throw new RequestException(string.Format("Invalid import schema requested. No schema named '{0}' could be resolved.", importSchemaName), exception);
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
