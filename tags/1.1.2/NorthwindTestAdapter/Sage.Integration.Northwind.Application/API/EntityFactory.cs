#region ©2007 Sage Technology Ltd., All Rights Reserved.
/*=====================================================================
	File:			EntityFactory.cs
	Author:			Philipp Schuette
	DateCreated:	04/23/2007 11:59:52
	DateChanged:	04/23/2007 11:59:52
 ---------------------------------------------------------------------
	©2007 Sage Technology Ltd., All Rights Reserved.	
=====================================================================*/

/*=====================================================================
	Date			        Author		Changes			Reasons
	04/23/2007 11:59:52	pschuette		Create			  
	Changes: Create, Refactoring, Upgrade	
=====================================================================*/
#endregion

#region Usings
using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Northwind.Application.Toolkit;
using System.IO;
using System.Xml.Schema;
using Sage.Integration.Northwind.Application.EntityResources;
using Sage.Integration.Northwind.Application.API;
using Sage.Integration.Northwind.Application.Entities;
using Sage.Integration.Northwind.Application.Base;
#endregion

namespace Sage.Integration.Northwind.Application.API
{
    public static class EntityFactory
    {

        #region public members

        public static List<Document> GetSupportedDocumentTemplates()
        {
           
            List<Document> documents;

            // create a documents of all supported top entities
            documents = new List<Document>();

            // add the account Document, this will include all subdocuments
            documents.Add(GetDocumentTemplate(Constants.EntityNames.Account));

            // add the company Document, this will include all subdocuments
            //documents.Add(GetDocumentTemplate(Constants.EntityNames.Company));

            // add the Order Document, this will include
            documents.Add(GetDocumentTemplate(Constants.EntityNames.Order));

            // add the Product Document
            documents.Add(GetDocumentTemplate(Constants.EntityNames.Product));

            // add the Price document
            documents.Add(GetDocumentTemplate(Constants.EntityNames.Price));

            documents.Add(GetDocumentTemplate(Constants.EntityNames.PricingList));

            // add the product family Document
            documents.Add(GetDocumentTemplate(Constants.EntityNames.ProductFamily));

            // add the UOM doucment
            documents.Add(GetDocumentTemplate(Constants.EntityNames.UnitOfMeasure));

            // add the uom Family Document
            documents.Add(GetDocumentTemplate(Constants.EntityNames.UnitOfMeasureFamily));

            return documents;
        
        }

        public static Document GetDocumentTemplate(string entityName)
        {
            switch (entityName)
            {
                case Constants.EntityNames.Company:
                    return new Sage.Integration.Northwind.Application.Entities.Account.Documents.CompanyDocument();

                case Constants.EntityNames.Account:
                    return new Sage.Integration.Northwind.Application.Entities.Account.Documents.AccountDocument();

                case Constants.EntityNames.Product:
                    return new Sage.Integration.Northwind.Application.Entities.Product.Documents.ProductDocument();

                case Constants.EntityNames.Price:
                    return new Sage.Integration.Northwind.Application.Entities.Product.Documents.PriceDocument();

                case Constants.EntityNames.PricingList:
                    return new Sage.Integration.Northwind.Application.Entities.Product.Documents.PricingListsDocument();

                case Constants.EntityNames.UnitOfMeasure:
                    return new Sage.Integration.Northwind.Application.Entities.Product.Documents.UnitOfMeasureDocument();

                case Constants.EntityNames.UnitOfMeasureFamily:
                    return new Sage.Integration.Northwind.Application.Entities.Product.Documents.UnitOfMeasureFamilyDocument();

                case Constants.EntityNames.ProductFamily:
                    return new Sage.Integration.Northwind.Application.Entities.Product.Documents.ProductFamilyDocument();

                case Constants.EntityNames.Order:
                    return new Sage.Integration.Northwind.Application.Entities.Order.Documents.OrderDocument();

                case Constants.EntityNames.Email:
                    return new Sage.Integration.Northwind.Application.Entities.Account.Documents.EmailDocument();

                default:
                    return null;
            }
        }

        public static EntityBase GetEntity(string entityName)
        {

            if (entityName.Equals(Constants.EntityNames.Account, StringComparison.InvariantCultureIgnoreCase))
                return new Sage.Integration.Northwind.Application.Entities.Account.Account();

            if (entityName.Equals(Constants.EntityNames.Product, StringComparison.InvariantCultureIgnoreCase))
                return new Sage.Integration.Northwind.Application.Entities.Product.Product();

            if (entityName.Equals(Constants.EntityNames.Price, StringComparison.InvariantCultureIgnoreCase))
                return new Sage.Integration.Northwind.Application.Entities.Product.Price();

            if (entityName.Equals(Constants.EntityNames.PricingList, StringComparison.InvariantCultureIgnoreCase))
                return new Sage.Integration.Northwind.Application.Entities.Product.PricingList();

            if (entityName.Equals(Constants.EntityNames.UnitOfMeasure, StringComparison.InvariantCultureIgnoreCase))
                return new Sage.Integration.Northwind.Application.Entities.Product.UnitOfMeasure();

            if (entityName.Equals(Constants.EntityNames.UnitOfMeasureFamily, StringComparison.InvariantCultureIgnoreCase))
                return new Sage.Integration.Northwind.Application.Entities.Product.UnitOfMeasureFamily();

            if (entityName.Equals(Constants.EntityNames.ProductFamily, StringComparison.InvariantCultureIgnoreCase))
                return new Sage.Integration.Northwind.Application.Entities.Product.ProductFamily();

            if (entityName.Equals(Constants.EntityNames.Order, StringComparison.InvariantCultureIgnoreCase))
                return new Sage.Integration.Northwind.Application.Entities.Order.Order();

            if (entityName.Equals(Constants.EntityNames.Email, StringComparison.InvariantCultureIgnoreCase))
                return new Sage.Integration.Northwind.Application.Entities.Email.Email();

            return null;
        }

        #endregion
    }
}
