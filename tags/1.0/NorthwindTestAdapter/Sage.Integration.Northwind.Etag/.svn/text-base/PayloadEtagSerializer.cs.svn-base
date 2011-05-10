#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Sis.Sdata.Etag.Crc;
using Sage.Integration.Northwind.Feeds;
using System.Collections;
using System.Reflection;
using Sage.Integration.Northwind.Common;
using System.Xml.Serialization;
using Sage.Sis.Sdata.Etag;

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
        /// <param name="obj">must be of type <see cref="PayloadBase"/>.</param>
        /// <returns></returns>
        
        public string Serialize(object obj)
        {
            // if obj == null -> append null value and return
            if (null == obj)
                return NULL;

            PayloadBase payloadObj = obj as PayloadBase;

            if (null == payloadObj)
                throw new ArgumentException(string.Format("The object given must implement {0}.", typeof(PayloadBase).FullName), "obj");


#warning WORKAROUND: Problems with enum types
            try
            {
                object compositeObj = this.GetPayloadObject(payloadObj);

                // serialize the composite payload object
                return this.SerializeCompositeObject(compositeObj);
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
                if ((prop.Name != "SyncUuid") && 
                    (prop.Name != "LocalID") &&
                    (prop.Name != "ForeignIds"))
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
