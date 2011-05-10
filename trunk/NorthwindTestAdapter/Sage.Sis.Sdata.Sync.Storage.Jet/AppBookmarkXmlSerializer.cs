#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

#endregion

namespace Sage.Sis.Sdata.Sync.Storage.Jet
{
    public class AppBookmarkXmlSerializer : IAppBookmarkSerializer
    {
        #region IAppBookmarkSerializer Members

        public string Serialize(object value)
        {
            System.Xml.Serialization.XmlSerializer xmlSerializer;
            Type valueType;

            try
            {
                valueType = value.GetType();
                xmlSerializer = new System.Xml.Serialization.XmlSerializer(valueType);

                using (StringWriter writer = new StringWriter())
                {
                    xmlSerializer.Serialize(writer, value);

                    return writer.ToString();
                }
            }
            catch (Exception exception)
            {
                throw new Exception("Error serializing application bookmark.", exception);
            }
        }

        public object Deserialize(string value, Type type)
        {
            object obj;
            System.Xml.Serialization.XmlSerializer xmlSerializer;

            try
            {
                xmlSerializer = new System.Xml.Serialization.XmlSerializer(type);

                using (StringReader stringWriter = new StringReader(value))
                {
                    // Get the deserialized instace of the object
                    obj = xmlSerializer.Deserialize(stringWriter);

                    return obj;
                }
            }
            catch (Exception exception)
            {
                throw new Exception("Error deserializing application bookmark.", exception);
            }
        }

        #endregion
    }
}
