#region Usings

using System.Xml.Serialization;

#endregion

namespace Sage.Integration.Northwind.Common
{
    public static class NameSpaceHelpers
    {
        private static XmlSerializerNamespaces stat_serializerNamespaces = null;

        public static XmlSerializerNamespaces SerializerNamespaces
        {
            get
            {
                if (stat_serializerNamespaces == null)
                {
                    stat_serializerNamespaces = new XmlSerializerNamespaces();

                    stat_serializerNamespaces.Add(Namespaces.syncPrefix, Namespaces.syncNamespace);
                    stat_serializerNamespaces.Add(Namespaces.northwindPrefix, Namespaces.northwindNamespace);
                    stat_serializerNamespaces.Add(Namespaces.smePrefix, Namespaces.smeNamespace);
                    stat_serializerNamespaces.Add(Namespaces.sdataPrefix, Namespaces.sdataNamespace);
                    stat_serializerNamespaces.Add(Namespaces.sdataHttpPrefix, Namespaces.sdataHttpNamespace);
                    stat_serializerNamespaces.Add(Namespaces.xsiPrefix, Namespaces.xsiNamespace);
                    stat_serializerNamespaces.Add(Namespaces.atomPrefix, Namespaces.atomNamespace);
                    stat_serializerNamespaces.Add(Namespaces.opensearchPrefix, Namespaces.opensearchNamespace);
                }
                return stat_serializerNamespaces;
            }
        }
    }
}
