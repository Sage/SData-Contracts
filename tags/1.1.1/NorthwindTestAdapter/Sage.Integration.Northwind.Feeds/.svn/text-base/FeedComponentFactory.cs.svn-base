#region Usings

using System;
using System.Collections.Generic;

#endregion

namespace Sage.Integration.Northwind.Feeds
{
    public static class FeedComponentFactory
    {
        private readonly static Dictionary<Type, Type> stat_types = new Dictionary<Type, Type>();

        static FeedComponentFactory()
        {
            // This is a simple way to initialize the FeedComponentFactory
            Register<Paging.IStartIndexElement>(typeof(Paging.StartIndexElement));
            Register<Paging.IItemsPerPageElement>(typeof(Paging.ItemsPerPageElement));
            Register<Paging.ITotalResultsElement>(typeof(Paging.TotalResultsElement));
        }

        public static T Create<T>()
        {
            return (T)Activator.CreateInstance(stat_types[typeof(T)]);
        }

        public static void Register<T>(Type implType)
        {
            if (implType.IsInterface)
                    throw new InvalidOperationException(string.Format("The supplied type {0} cannot be an interface.", typeof(T).FullName));
            if (implType.IsAbstract)
                    throw new InvalidOperationException(string.Format("The supplied type {0} cannot be an abstract class.", typeof(T).FullName));
            if (null == implType.GetConstructor(Type.EmptyTypes))
                throw new InvalidOperationException(string.Format("The supplied type {0} must provide an empty constructor.", typeof(T).FullName));            
            
            if (!typeof(T).IsAssignableFrom(implType))
                throw new InvalidOperationException(string.Format("The supplied type does not implement {0}.", typeof(T).FullName));

            stat_types.Add(typeof(T), implType);
        }
    }
}
