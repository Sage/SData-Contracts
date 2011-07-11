namespace Sage.Integration.Northwind.Feeds.Paging
{
    public interface ITotalResultsElement
    {
        void LoadXmlValue(string xml);
        string ToXml();

        int Value { get; set; }
    }
}
