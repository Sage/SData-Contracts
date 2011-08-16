#region Usings

using System.Xml.Serialization;
using Microsoft.Http;
using SageKa.Samples.VcSync.SyncEngine.Syndications;

#endregion

namespace SageKa.Samples.VcSync.SyncEngine.Extensions
{
    public static class HttpContentExtensions
    {
        public static Tracking SdataReadAsTracking(this HttpContent httpContent)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Tracking));
            return (Tracking)serializer.Deserialize(httpContent.ReadAsStream());
        }
    }
}
