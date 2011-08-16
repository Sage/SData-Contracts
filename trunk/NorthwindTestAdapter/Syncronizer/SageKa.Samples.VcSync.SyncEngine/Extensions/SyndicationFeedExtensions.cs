#region Usings

using System.Xml.Serialization;
using System.Collections.ObjectModel;
using System.ServiceModel.Syndication;
using SageKa.Samples.VcSync.SyncEngine.Syndications;

#endregion

namespace SageKa.Samples.VcSync.SyncEngine.Extensions
{
    public static class SyndicationFeedExtensions
    {
        public static Diagnosis ReadSdataDiagnosis(this SyndicationFeed feed)
        {
            Collection<Diagnosis> elements = feed.ElementExtensions.ReadElementExtensions<Diagnosis>("diagnosis", "http://schemas.sage.com/sdata/2008/1", new XmlSerializer(typeof(Diagnosis)));

            if (elements.Count == 0)
            {
                return null;
            }

            return elements[0];
        }
    }
}
