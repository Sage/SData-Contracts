#region Usings

using System;
using System.Reflection;

#endregion

namespace Sage.Integration.Northwind.Common
{
    public static class ReflectionHelpers
    {
        public static bool TryGetSingleCustomAttribute<T>(Type type, out T attr)
            where T : Attribute
        {
            attr = null;

            T[] attributes = (T[])type.GetCustomAttributes(typeof(T), true);

            if (attributes.Length > 0)
            {
                attr = attributes[0];
                return true;
            }

            return false;
        }
        /// <summary>
        /// Tries to get the first specific attribute from a member.
        /// </summary>
        /// <typeparam name="T">Type of the attribute to look for</typeparam>
        /// <param name="memberInfo">The member.</param>
        /// <param name="attr">Contains the attribute if found; otherwise null</param>
        /// <returns>True if attribute of type T exists; otherwise false</returns>
        public static bool TryGetSingleCustomAttribute<T>(MemberInfo memberInfo, out T attr)
            where T : Attribute
        {
            attr = null;

            T[] attributes = (T[])memberInfo.GetCustomAttributes(typeof(T), true);

            if (attributes.Length > 0)
            {
                attr = attributes[0];
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks whether the member contains at least one property of type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        public static bool HasCustomAttribute<T>(MemberInfo memberInfo)
            where T : Attribute
        {
            T[] attributes = (T[])memberInfo.GetCustomAttributes(typeof(T), true);

            return attributes.Length > 0;
        }
    }
}
