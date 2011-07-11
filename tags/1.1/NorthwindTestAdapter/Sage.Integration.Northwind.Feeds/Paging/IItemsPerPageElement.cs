namespace Sage.Integration.Northwind.Feeds.Paging
{
    public interface IItemsPerPageElement
    {
        void LoadXmlValue(string xml);
        string ToXml();

        int Value { get; set; }
    }
}
