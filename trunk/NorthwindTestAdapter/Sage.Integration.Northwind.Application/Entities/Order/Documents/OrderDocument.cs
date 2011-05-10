#region ©2007 Sage Technology Ltd., All Rights Reserved.
/*=====================================================================
	File:			OrderDocument.cs
	Author:			Philipp Schuette
	DateCreated:	04/11/2007 09:53:22
	DateChanged:	04/11/2007 09:53:22
 ---------------------------------------------------------------------
	©2007 Sage Technology Ltd., All Rights Reserved.	
=====================================================================*/

/*=====================================================================
	Date			        Author		Changes			Reasons
	04/11/2007 09:53:22  	pschuette		Create			  
	Changes: Create, Refactoring, Upgrade	
=====================================================================*/
#endregion

#region Usings
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Sage.Integration.Northwind.Application.Toolkit;
using Sage.Integration.Northwind.Application.Base;
using Sage.Integration.Northwind.Application.API;
#endregion

namespace Sage.Integration.Northwind.Application.Entities.Order.Documents
{
    public class OrderDocument : Document
    {
        #region constructor
        /// <summary>
        /// Creates a new OrderDocument Document
        /// </summary>
        public OrderDocument()
            : base(Constants.EntityNames.Order)
        {
            AddProperty("accountid", TypeCode.String);
            AddProperty("currency", TypeCode.String, 4);
            AddProperty("pricinglistid", TypeCode.String,10);

            AddProperty("reference", TypeCode.String);
            AddProperty("opened", TypeCode.DateTime);
            AddProperty("status", TypeCode.String);
            
            //AddProperty("nodiscamt", TypeCode.Decimal);
            //AddProperty("discountpc", TypeCode.Decimal);
            AddProperty("discountamt", TypeCode.Decimal);
            AddProperty("lineitemdisc", TypeCode.Decimal);
            AddProperty("nettamt", TypeCode.Decimal);
            AddProperty("tax", TypeCode.Decimal);
            AddProperty("grossamt", TypeCode.Decimal);
            AddProperty("deliverydate", TypeCode.DateTime);
            AddProperty("salesrepr", TypeCode.String, 40);// should be TypeCode.Int32);
            AddProperty("shipaddress", TypeCode.Object, 60);
            AddProperty("shippedvia", TypeCode.Int32);
            AddProperty("freight", TypeCode.Decimal);
           
            
            AddCollection(new LineItemsDocumentCollection());

        }



        
        #endregion


        #region public properties


        public Property accountid
        {
            get { return Properties["accountid"]; }
            set { Properties["accountid"] = value; }
        }

        public Property currency
        {
            get { return Properties["currency"]; }
            set { Properties["currency"] = value; }
        }

        public Property pricinglistid
        {
            get { return Properties["pricinglistid"]; }
            set { Properties["pricinglistid"] = value; }
        }
        public Property reference
        {
            get { return Properties["reference"]; }
            set { Properties["reference"] = value; }
        }
        public Property opened
        {
            get { return Properties["opened"]; }
            set { Properties["opened"] = value; }
        }
        public Property status
        {
            get { return Properties["status"]; }
            set { Properties["status"] = value; }
        }
        //public Property TotalQuotedPrice
        //{
        //    get { return Properties["nodiscamt"]; }
        //    set { Properties["nodiscamt"] = value; }
        //}



        //public Property DiscountPercentage
        //{
        //    get { return Properties["discountpc"]; }
        //    set { Properties["discountpc"] = value; }
        //}
        public Property discountamt
        {
            get { return Properties["discountamt"]; }
            set { Properties["discountamt"] = value; }
        }
        public Property nettamt
        {
            get { return Properties["nettamt"]; }
            set { Properties["nettamt"] = value; }
        }
        public Property tax
        {
            get { return Properties["tax"]; }
            set { Properties["tax"] = value; }
        }

        public Property lineitemdisc
        {
            get { return Properties["lineitemdisc"]; }
            set { Properties["lineitemdisc"] = value; }
        }

        public Property grossamt
        {
            get { return Properties["grossamt"]; }
            set { Properties["grossamt"] = value; }
        }

        public Property deliverydate
        {
            get { return Properties["deliverydate"]; }
            set { Properties["deliverydate"] = value; }
        }
        public Property salesrepr
        {
            get { return Properties["salesrepr"]; }
            set { Properties["salesrepr"] = value; }
        }


        public Property shipaddress
        {

            get { return Properties["shipaddress"]; }
            set { Properties["shipaddress"] = value; }
        }


        public Property shippedvia
        {
            get { return Properties["shippedvia"]; }
            set { Properties["shippedvia"] = value; }
        }
        public Property freight
        {
            get { return Properties["freight"]; }
            set { Properties["freight"] = value; }
        }
        //public Property ShipName
        //{
        //    get { return Properties["ship_name"]; }
        //    set { Properties["ship_name"] = value; }
        //}
        //public Property ShipAddress
        //{
        //    get { return Properties["ship_address"]; }
        //    set { Properties["ship_address"] = value; }
        //}
        //public Property ShipRegion
        //{
        //    get { return Properties["ship_region"]; }
        //    set { Properties["ship_region"] = value; }
        //}
        //public Property ShipPostalCode
        //{
        //    get { return Properties["ship_postalcode"]; }
        //    set { Properties["ship_postalcode"] = value; }
        //}

        //public Property ShipCity
        //{
        //    get { return Properties["ship_city"]; }
        //    set { Properties["ship_city"] = value; }
        //}

        //public Property ShipCountry
        //{
        //    get { return Properties["ship_country"]; }
        //    set { Properties["ship_country"] = value; }
        //}

        public LineItemsDocumentCollection orderitems
        {
            get { return (LineItemsDocumentCollection)Collections[Constants.EntityNames.LineItemCollection]; }
            set { Collections[Constants.EntityNames.LineItemCollection] = value; }
        }



        #endregion

    }
}
