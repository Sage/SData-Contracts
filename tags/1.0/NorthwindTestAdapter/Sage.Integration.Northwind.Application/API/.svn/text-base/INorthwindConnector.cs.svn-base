using System;
using System.Collections.Generic;
using System.Text;

namespace Sage.Integration.Northwind.Application.API
{
    // generated interface
//public interface test
//{


        

//    public Pricing CheckPrice(Pricing PricingInformation);
//    public void CloseSession(string SessionKey);
//    public TransactionResult[] ExecuteTransactions(string EntityName, Transaction[] TransactionData);
//    public AuthenticationResult GetAuthenticationScheme();
//    public ChangeLog GetChangeLog(string EntityName, string Token, string LastIdentity);
        

//        public Configuration GetConfiguration();
//        public ERPCustomisations GetCustomisations();
//        public PricingDetail GetPricingDetails(OrderDetailInformation OrderDetails);
//    public SessionOpenResult OpenSession();
//        public ViewRealTimeDataResult ViewRealTimeData(string EntityName, string[] SelectFields, SearchField[] SearchFields, OrderFields[] OrderByFields, string RowsPerPage, string PageNumber);
        
        
//    }


    interface INorthwindConnector
    {
        Pricing CheckPrice(Pricing PricingInformation, NorthwindConfig northwindConfig);
        void CloseSession(string SessionKey);
        TransactionResult[] ExecuteTransactions(string EntityName, Transaction[] TransactionData, NorthwindConfig config);
        AuthenticationResult GetAuthenticationScheme(NorthwindConfig northwindConfig);
        ChangeLog GetChangeLog(string EntityName, string Token, NorthwindConfig config);
        Configuration GetConfiguration(NorthwindConfig northwindConfig);
        ERPCustomisations GetCustomisations(NorthwindConfig northwindConfig);
        PricingDetail GetPricingDetails(OrderDetailInformation OrderDetails, NorthwindConfig config);
        SessionOpenResult OpenSession(NorthwindConfig northwindConfig);
        //new, please change
        //public ViewRealTimeDataResult ViewRealTimeData(string EntityName, string[] SelectFields, SearchField[] SearchFields, OrderFields[] OrderByFields, string RowsPerPage, string PageNumber);
        ViewRealTimeDataResult ViewRealTimeData(string entityName, string[] selectFields, SearchField[] searchFields, string[] orderFields, int rowsPerPage, int pageNumber, NorthwindConfig northwindConfig);
    }
}
