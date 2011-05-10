#region Usings

using System;
using System.Xml;

#endregion

namespace Sage.Integration.Northwind.Common
{
    public static class XmlConverter
    {
        /// <summary>
        /// Tries get a xml string representation for a given object.
        /// </summary>
        /// <param name="value">The value to be converted.</param>
        /// <param name="convertedValue">Contains the converted value.</param>
        /// <returns>True if value could be converted; otherwise false.</returns>
        public static bool TryGetString(object value, out string convertedValue)
        {
            return TryGetString(value, value.GetType().FullName, out convertedValue);
        }
        
        /// <summary>
        /// Converts the value to a xml string representation.
        /// </summary>
        /// <param name="value">The value to be converted.</param>
        /// <returns>The converted value.</returns>
        /// <exception cref="NotImplementedException">If value type is not supported.</exception>
        public static string GetString(object value)
        {
            return GetString(value.GetType().FullName, value);
        }

        #region Private Helpers

        private static string GetString(string typeName, object value)
        {
            string resultValue;
            if (!TryGetString(value, typeName, out resultValue))
                throw new InvalidOperationException(string.Format("An object of type {0} is not suppported for xml conversion.", typeName));

            return resultValue;
        }
        private static bool TryGetString(object value, string typeName, out string convertedValue)
        {
            convertedValue = null;
            if (value == null)
            {
                return true;
            }

            switch (typeName)
            {
                case "System.String":
                    {
                        convertedValue = (string)value;
                        return true;
                    }
                case "System.Guid":
                    {
                        convertedValue = XmlConvert.ToString((Guid)value);
                        return true;
                    }
                case "System.DateTime":
                    {
                        convertedValue = XmlConvert.ToString((DateTime)value, XmlDateTimeSerializationMode.Utc);
                        return true;
                    }
                case "System.DateTimeOffset":
                    {
                        convertedValue = XmlConvert.ToString((DateTimeOffset)value);
                        return true;
                    }
                case "System.TimeSpan":
                    {
                        convertedValue = XmlConvert.ToString((TimeSpan)value);
                        return true;
                    }
                case "System.Boolean":
                case "System.Bool":
                    {
                        convertedValue = XmlConvert.ToString((bool)value);
                        return true;
                    }
                case "System.Byte":
                    {
                        convertedValue = XmlConvert.ToString((byte)value);
                        return true;
                    }
                case "System.Char":
                    {
                        convertedValue = XmlConvert.ToString((char)value);
                        return true;
                    }
                case "System.Decimal":
                    {
                        convertedValue = XmlConvert.ToString((decimal)value);
                        return true;
                    }
                case "System.Double":
                    {
                        convertedValue = XmlConvert.ToString((double)value);
                        return true;
                    }
                case "System.Single":
                    {
                        convertedValue = XmlConvert.ToString((float)value);
                        return true;
                    }
                case "System.Int16":
                    {
                        convertedValue = XmlConvert.ToString((short)value);
                        return true;
                    }
                case "System.Int32":
                    {
                        convertedValue = XmlConvert.ToString((int)value);
                        return true;
                    }
                case "System.Int64":
                    {
                        convertedValue = XmlConvert.ToString((long)value);
                        return true;
                    }
                case "System.SByte":
                    {
                        convertedValue = XmlConvert.ToString((sbyte)value);
                        return true;
                    }
                case "System.UInt16":
                    {
                        convertedValue = XmlConvert.ToString((ushort)value);
                        return true;
                    }
                case "System.UInt32":
                    {
                        convertedValue = XmlConvert.ToString((uint)value);
                        return true;
                    }
                case "System.UInt64":
                    {
                        convertedValue = XmlConvert.ToString((ulong)value);
                        return true;
                    }
            }

            return false;
        }

        #endregion
    }
}
