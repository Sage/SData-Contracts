#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Northwind.Common;

#endregion

namespace Sage.Integration.Northwind.Etag
{
    internal class DeepPayloadEtagSerializer : PayloadEtagSerializer
    {
        /// <summary>
        /// Serializes an object. If the object is no simple xml type this method
        /// calls reflects for the composite object and calls SerializeCompositeObject to serialize it.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override string GetSerializedValue(object value)
        {
            string serializedValue = null;
            if (!XmlConverter.TryGetString(value, out serializedValue))
            {

#warning WORKAROUND: Problems with enum types
                try
                {
                    serializedValue = this.SerializeCompositeObject(value);  // recursive call
                }
                catch
                {
                    serializedValue = "";
                }
            }

            return serializedValue;
        }
    }
}
