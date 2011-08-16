#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Sis.Sdata.Etag.Crc;
using System.Collections;
using System.Reflection;
using Sage.Integration.Northwind.Common;
using System.Xml.Serialization;
using Sage.Sis.Sdata.Etag;
using Sage.Common.Syndication;

#endregion

namespace Sage.Integration.Northwind.Etag
{
    internal class PayloadEtagSerializer : IEtagSerializer
    {
        #region CONSTANTS

#warning TODO: Which value should we use for NULL values???
        private const string NULL = "NULL";

        #endregion

        #region IEtagSerializer Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj">must be of type <see cref="PayloaContainer"/>.</param>
        /// <returns></returns>
        
        public string Serialize(object obj)
        {
            // if obj == null -> append null value and return
            if (null == obj)
                return NULL;

            FeedEntry payloadObj = obj as FeedEntry;

            if (null == payloadObj)
                throw new ArgumentException(string.Format("The object given must implement {0}.", typeof(FeedEntry).FullName), "obj");


#warning WORKAROUND: Problems with enum types
            try
            {
                //object compositeObj = this.GetPayloadObject(payloadObj);

                // serialize the composite payload object
                return this.SerializeCompositeObject(payloadObj);
            }
            catch
            {
                return "";
            }
        }

        #endregion

        #region Protected Methods

        protected string SerializeCompositeObject(object compositeObj)
        {
            // if compositeObj == null -> append null value and return
            if (null == compositeObj)
                return NULL;

            string resultString = string.Empty;

            // get the composite object's type for reflection
            Type compositeObjType = compositeObj.GetType();

            IEnumerator arrayEnumerator;

            if (compositeObjType.IsArray)
            {
                arrayEnumerator = ((IEnumerable)compositeObj).GetEnumerator();

                while (arrayEnumerator.MoveNext())
                    resultString += this.SerializeProperties(arrayEnumerator.Current.GetType().GetProperties(), arrayEnumerator.Current);
            }
            else if (compositeObj is FeedEntryCollection<FeedEntry>)
                {
                    arrayEnumerator = ((IEnumerable)compositeObj).GetEnumerator();

                    while (arrayEnumerator.MoveNext())
                        resultString += this.Serialize(arrayEnumerator.Current);
                }
              else if (compositeObj is IFeed)
                {
                    arrayEnumerator = ((IFeed)compositeObj).Entries.GetEnumerator();

                    while (arrayEnumerator.MoveNext())
                        resultString += this.Serialize(arrayEnumerator.Current);
                }
              //  typeof(IFeed).IsAssignableFrom(propInfo.PropertyType)
           /* else if (compositeObj is FeedEntry)
            {
                resultString = this.Serialize(compositeObj);
            }*/
            else
            {
                resultString = this.SerializeProperties(compositeObjType.GetProperties(), compositeObj);
            }
            
            return resultString;
        }

        private string SerializeProperties(PropertyInfo[] propertyInfos, object obj)
        {
            string resultString = String.Empty;
            Type objType = obj.GetType();

            object propertyValue;
            Type propertyValueType;
            IEnumerator arrayEnumerator;

            // iterate through all properties that are declared 'public' and support 'get'
            foreach (PropertyInfo propInfo in objType.GetProperties())
            {
                Type proptype = propInfo.GetType();

                if ((propInfo.Name != "IsEmpty") &&
                       (propInfo.Name != "IsDeleted") &&
                       (propInfo.Name != "Url") &&
                       (propInfo.Name != "Key") &&
                       (propInfo.Name != "Uuid") &&
                       (propInfo.Name != "Author") &&
                       (propInfo.Name != "Categories") &&
                       (propInfo.Name != "ContainsPayload") &&
                       (propInfo.Name != "Content") &&
                       (propInfo.Name != "DeleteMissing") &&
                       (propInfo.Name != "Descriptor") &&
                       (propInfo.Name != "Diagnoses") &&
                       (propInfo.Name != "HttpETag") &&
                       (propInfo.Name != "HttpIfMatch") &&
                       (propInfo.Name != "HttpLocation") &&
                       (propInfo.Name != "HttpMessage") &&
                       (propInfo.Name != "HttpMethod") &&
                       (propInfo.Name != "HttpStatusCode") &&
                       (propInfo.Name != "Id") &&
                       (propInfo.Name != "IsDeleted") &&
                       (propInfo.Name != "Key") &&
                       (propInfo.Name != "Links") &&
                       (propInfo.Name != "Published") &&
                       (propInfo.Name != "Source") &&
                       (propInfo.Name != "Summary") &&
                       (propInfo.Name != "SyncState") &&
                       (propInfo.Name != "Title") &&
                       (propInfo.Name != "Updated") &&
                       (propInfo.Name != "UpdateOperation") &&
                       (propInfo.Name != "Uri") &&
                       (propInfo.Name != "UUID"))
                {
                    // ignore properties with XmlIgnore attribute
                    if (!ReflectionHelpers.HasCustomAttribute<XmlIgnoreAttribute>(propInfo))
                    {
                        // get the value of the property
                        // (values of type Nullable<T> will be boxed automatically, and return either null or an object of type T. see Nullable<T> in MSDN).
                        propertyValue = propInfo.GetValue(obj, null);

                        if (null == propertyValue)
                        {
                            resultString += NULL;
                        }
                        else
                        {
                            // get the type of the property value
                            propertyValueType = propInfo.PropertyType;

                            // the property is in general a simple xml type that can be converted to a xml string.
                            if (propertyValueType.IsArray)
                            {
                                arrayEnumerator = ((IEnumerable)propertyValue).GetEnumerator();

                                while (arrayEnumerator.MoveNext())
                                    resultString += GetSerializedValue(arrayEnumerator.Current);
                            }
                            else
                            {
                                resultString += GetSerializedValue(propertyValue);
                            }
                        }
                    }
                }
            }

            return resultString;
        }

        protected object GetPayloadObject(object obj)
        {
            if (null == obj)
                return null;

            Type objType = obj.GetType();

            // get the only property defining the composite payload
            PropertyInfo[] propInfos = objType.GetProperties();


            // get value of composite payload property
            foreach (PropertyInfo prop in propInfos)
            {
                if ((prop.Name != "IsEmpty") && 
                    (prop.Name != "IsDeleted") &&
                    (prop.Name != "Url") &&
                    (prop.Name != "Key") &&
                    (prop.Name != "Uuid") &&
                    (prop.Name != "Author") && 
                    (prop.Name != "Categories") && 
                    (prop.Name != "ContainsPayload") && 
                    (prop.Name != "Content") && 
                    (prop.Name != "DeleteMissing") &&  
                    (prop.Name != "Descriptor") && 
                    (prop.Name != "Diagnoses") && 
                    (prop.Name != "HttpETag") && 
                    (prop.Name != "HttpIfMatch") &&  
                    (prop.Name != "HttpLocation") &&  
                    (prop.Name != "HttpMessage") &&  
                    (prop.Name != "HttpMethod") &&  
                    (prop.Name != "HttpStatusCode") &&  
                    (prop.Name != "Id") && 
                    (prop.Name != "IsDeleted") &&  
                    (prop.Name != "Key") && 
                    (prop.Name != "Links") && 
                    (prop.Name != "Published") && 
                    (prop.Name != "Source") && 
                    (prop.Name != "Summary") &&  
                    (prop.Name != "SyncState") && 
                    (prop.Name != "Title") &&  
                    (prop.Name != "Updated") && 
                    (prop.Name != "UpdateOperation") &&  
                    (prop.Name != "Uri") &&  
                    (prop.Name != "UUID"))
                    return prop.GetValue(obj, null);

            }

            throw new EtagComputeException(string.Format("Cannot serialze object of type '{0}'. More than one property not allowed.", objType.FullName));
            return null;
        }

        /// <summary>
        /// Serializes a simple type value. If the object cannot be serialized an empty string is returned.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected virtual string GetSerializedValue(object value)
        {
            string serializedValue = null;
            XmlConverter.TryGetString(value, out serializedValue);  // (ignore other types)

            return serializedValue;
        }

        #endregion
    }
}
