using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Messaging.Model;
using Sage.Common.Syndication;
using System.Reflection;
using System.Xml.Schema;
using System.IO;
using Sage.Integration.Northwind.Adapter.Common;
using System.Xml;

namespace Sage.Integration.Northwind.Adapter.Receiver
{
    //[RequestPath("$schema")]
    public class TopLevelRequestReceiver
    {
        //private string VER = "1.1";
        private string VER = string.Empty;

       /* [GetRequestTarget("*"
        public void getContracts(IRequest request)
        {
        }*/

        [GetRequestTarget("$schema")]
        [SupportedAccept(MediaType.Atom, true)]
        [SupportedContentType(MediaType.Atom)]
        public void GetSchema(IRequest request)
        {
            Assembly assembly = GetSchemaAssembly();
            RequestContext context = new RequestContext(request.Uri);
            XmlSchema xmlschema = null;
            using (StreamReader sr = new StreamReader(assembly.GetManifestResourceStream("Sage.Integration.Northwind.Adapter.crmErp"+VER+".xsd")))
            {
                xmlschema = XmlSchema.Read(sr, null);
            }

            // replace common import url
            foreach (XmlSchemaImport schemaInportObj in xmlschema.Includes)
            {
                if (schemaInportObj.Namespace == "http://schemas.sage.com/sc/2009")
                {
                    schemaInportObj.SchemaLocation = string.Format("{0}{1}/import/common"+VER, context.DatasetLink, Constants.schema);
                    break;
                }
            }
        
            request.Response.ContentType = Sage.Common.Syndication.MediaType.Xml;
            request.Response.Xml = GetXml(xmlschema);
        }

        [GetRequestTarget("$schema/import/*")]
        [SupportedAccept(MediaType.Atom, true)]
        [SupportedContentType(MediaType.Atom)]
        public void GetCommonSchema(IRequest request)
        {
            string importSchemaName = request.Uri.LastPathSegment.ToString();
            string manifestResourceName = manifestResourceName = string.Format("Sage.Integration.Northwind.Adapter.{0}.xsd", importSchemaName); 
            Assembly assembly = GetSchemaAssembly();
            XmlSchema xmlschema = null;
            try
            {
                using (StreamReader sr = new StreamReader(assembly.GetManifestResourceStream(manifestResourceName)))
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

        [GetRequestTarget("*/$schema")]
        public void GetResourceSchema(IRequest request)
        {
            RequestContext context = new RequestContext(request.Uri);
            string redirect = String.Format("{0}{1}#{2}", context.DatasetLink, Constants.schema, context.ResourceKind.ToString());
            request.Response.StatusCode = System.Net.HttpStatusCode.Found;
            request.Response.Protocol.SendUnknownResponseHeader("Location", redirect);
        }

        private string GetXml(XmlSchema schema)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(SerializeObjectToXml(schema));
            return @"<?xml version=""1.0"" encoding=""utf-8""?>"
                + xmlDoc.DocumentElement.OuterXml;
        }

        private string SerializeObjectToXml(object param)
        {
            System.Xml.Serialization.XmlSerializer xmlSerializer;
            StringWriter stringWriter;
            string xml;

            // Handle null
            if (null == param)
                return null;

            xmlSerializer = new System.Xml.Serialization.XmlSerializer(param.GetType());

            using (stringWriter = new StringWriter())
            {
                // Write the xml content into the memory stream
                xmlSerializer.Serialize((TextWriter)stringWriter, param);
                xml = stringWriter.ToString();
            }

            // Return the xml representation of the object
            return xml;
        }

        private Assembly GetSchemaAssembly()
        {
            Assembly assembly = Assembly.GetAssembly(typeof(Sage.Integration.Northwind.Adapter.NorthwindAdapter));
            return assembly;
        }
    }
}