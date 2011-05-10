namespace Sage.Integration.Northwind.Adapter.Services
{
    interface ICommodityPrice
    {
        string LocalId { get; }
        decimal UnitPrice { get; }

        bool IsEmpty();
    }
}
