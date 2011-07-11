#region ©2007 Sage Technology Ltd., All Rights Reserved.
/*=====================================================================
	File:			Constants.cs
	Author:			Philipp Schuette
	DateCreated:	03/16/2007 
	DateChanged:	03/16/2007
 ---------------------------------------------------------------------
	©2007 Sage Technology Ltd., All Rights Reserved.	
=====================================================================*/

/*=====================================================================
	Date			        Author		Changes			Reasons
	03/16/2007          	pschuette		Create			  
	Changes: Create, Refactoring, Upgrade	
=====================================================================*/
#endregion

#region Usings
using System;
using System.Collections.Generic;
using System.Text;
#endregion

namespace Sage.Integration.Northwind.Application.API
{
    public struct Constants
	{
        public const string crlf = "\r\n";
        // Id Handling
        public const string CustomerIdPrefix = "C-";
        public const string SupplierIdPrefix = "S-";
        
        public const string PhoneIdPostfix = "-P";
        public const string FaxIdPostfix = "-F";

        public struct OrderStatus
        {
            public const string Active = "Active";
            public const string Completed = "Completed";
        }

        // DefaultValues

        public const string InstancesName = "Instances";

        public struct DefaultValues
        {
            public struct Email
            {
                public const string Type = "Preferred";
            }

            public struct PriceList
            {
                public const string ID = "Standard";
                public const string Name = "Standard";
            }

            public struct PriceListSpecial
            {
                public const string ID = "Special";
                public const string Name = "Special";
            }

            public const string Active = "Y";
            public const string NotActive = "N";
            public const string DefaultValue = "Y";
            public const string NoDefaultValue = "";
        }

        public struct EntityNames
        {
            public const string Company = "company";
            public const string Account = "account";


            public const string Address = "address";
            public const string Email = "email";
            public const string Person = "person";
            public const string Phone = "phone";


            public const string Product = "products";
            public const string Price = "pricing";
            public const string PricingList = "pricinglist";
            public const string ProductFamily = "productfamily";
            public const string UnitOfMeasure = "uom";
            public const string UnitOfMeasureFamily = "uomfamily";

            public const string Order = "orders";
            public const string LineItem = "orderitems";
            public const string LineItemCollection = "orderitems";
        }

        public struct RTDVNames
        {
            public const string SalesInvoices = "SalesInvoices";
            public const string ProductsPurchased = "ProductsPurchased";
            public const string AdditionalStockDetails = "AdditionalStockDetails";
            public const string SalesOrders = "SalesOrders";
            public const string SalesOrderItems = "SalesOrderItems";
            public const string Employees = "Employees";

            public static string[] GetValues()
            {
                return new string[] 
                {
                    SalesInvoices, 
                    ProductsPurchased, 
                    AdditionalStockDetails,
                    //SalesOrders,
                    //SalesOrderItems,
                    //Employees
                };
            }
        }
    }
}
