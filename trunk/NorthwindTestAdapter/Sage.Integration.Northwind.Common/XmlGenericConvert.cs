#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

#endregion

namespace Sage.Integration.Northwind.Common
{
    // USAGE:
    //string data = XmlGenericConvert<int>.ConvertToString(11);
    //Guid g = XmlGenericConvert<Guid>.ConvertToGeneric("13371337-1337-1337-1337-133713371337");

    public class XmlGenericConvert<T>
    {
        public static readonly Converter<T, string> ConvertToString;
        public static readonly Converter<string, T> ConvertToGeneric;

        static XmlGenericConvert()
        {
            Type convertor = typeof(System.Xml.XmlConvert);

            // Get "public static string ToString(T data)" method 
            MethodInfo methodToString = convertor.GetMethod("ToString", BindingFlags.Public | BindingFlags.Static, null, new Type[] { typeof(T) }, null);
            if (methodToString == null)
            {
                throw new ArgumentException("Unable to find a XML converter string from type " + typeof(T).FullName, "T");
            }
            // And create delegate based on it
            ConvertToString = (Converter<T, string>)Delegate.CreateDelegate(typeof(Converter<T, string>), methodToString, true);

            // Ugly search for "public static T ToMyName(string data)" method
            foreach (MethodInfo methodFromString in convertor.GetMembers(BindingFlags.Public | BindingFlags.Static))
            {
                if (!methodFromString.Name.StartsWith("To")) continue;
                if (!methodFromString.ReturnType.Equals(typeof(T))) continue;
                ParameterInfo[] parameters = methodFromString.GetParameters();
                if (parameters.Length != 1) continue;
                if (!parameters[0].ParameterType.Equals(typeof(string))) continue;
                // Create delegate based on member found 
                ConvertToGeneric = (Converter<string, T>)Delegate.CreateDelegate(typeof(Converter<string, T>), methodFromString, true);
                return;
            }
            // Looks like was unable to find ToMyName method. Not good. 
            throw new ArgumentException("Unable to find a XML converter string to type " + typeof(T).FullName, "T");

            // Or alternative reflection-less hack
            // Will work much better in restricted enviroments but requere you to list all types you support
            /*
                if (typeof(T).Equals(typeof(bool)))
                {
                    toString = new Converter<bool, string>(System.Xml.XmlConvert.ToString) as Converter<T, string>;
                    fromString = new Converter<string, bool>(System.Xml.XmlConvert.ToBoolean) as Converter<string, T>;
                };
                */
        }
    }
}
