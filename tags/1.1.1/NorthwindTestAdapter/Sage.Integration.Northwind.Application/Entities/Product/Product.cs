#region ©2007 Sage Technology Ltd., All Rights Reserved.
/*=====================================================================
	File:			Product.cs
	Author:			Philipp Schuette
	DateCreated:	04/05/2007 15:33:28
	DateChanged:	04/05/2007 15:33:28
 ---------------------------------------------------------------------
	©2007 Sage Technology Ltd., All Rights Reserved.	
=====================================================================*/

/*=====================================================================
	Date			        Author		Changes			Reasons
	04/05/2007 15:33:28	pschuette		Create			  
	Changes: Create, Refactoring, Upgrade	
=====================================================================*/
#endregion

#region Usings
using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Northwind.Application.Toolkit;
using Sage.Integration.Northwind.Application.Entities.Product.Documents;
using Sage.Integration.Northwind.Application.Entities.Product.DataSets;
using System.Data.OleDb;
using System.Xml;
using Sage.Integration.Northwind.Application.Entities.Product.DataSets.ProductTableAdapters;
using Sage.Integration.Northwind.Application.EntityResources;
using Sage.Integration.Northwind.Application.API;

using Sage.Integration.Northwind.Application.Base;
using System.Data;
using Sage.Integration.Northwind.Application.Properties;
#endregion

namespace Sage.Integration.Northwind.Application.Entities.Product
{
    public class Product : EntityBase
    {
        #region Ctor.

        public Product() : base (Constants.EntityNames.Product)
        {
        }

        #endregion

        #region public members

        public ProductPriceDocument[] GetProductPrices(string[] productIds, NorthwindConfig config)
        {
            List<ProductPriceDocument> items = new List<ProductPriceDocument>();
            DataSet dataSet = new DataSet();
            OleDbDataAdapter dataAdapter;

            string whereClause =  "where";

            List<OleDbParameter> oleDbParameters = new List<OleDbParameter>();

            foreach (string productId in productIds)
            {
                whereClause += " ProductID=? OR";
                string name = "p" + oleDbParameters.Count.ToString();
                OleDbParameter oleDbParam = new OleDbParameter(name, Convert.ToInt32(productId));
                oleDbParameters.Add(oleDbParam);
            }

            whereClause = whereClause.Remove(whereClause.Length - 3, 3);

            string sqlQuery = "Select * from Products " + whereClause;

            using (OleDbConnection connection = new OleDbConnection(config.ConnectionString))
            {                
                dataAdapter = new OleDbDataAdapter(sqlQuery, connection);
                dataAdapter.SelectCommand.Parameters.AddRange(oleDbParameters.ToArray());
                if (dataAdapter.Fill(dataSet, "Products") == 0)
                {
                    // no products found
                }
                else
                {
                    foreach (DataRow row in dataSet.Tables["Products"].Rows)
                    {
                        ProductPriceDocument productPrice = new ProductPriceDocument();
                        productPrice.ProductId = Convert.ToInt32(row["ProductID"]);
                        productPrice.UnitPrice = Convert.ToDecimal(row["UnitPrice"]);

                        items.Add(productPrice);
                    }
                }

            }

            return items.ToArray();
        }

        public PricingDetail GetPricingDetails(OrderDetailInformation OrderDetails, NorthwindConfig config)
        {
            PricingDetail result = new PricingDetail();
            string customerID;
            int productId;
            List<int> productIds = new List<int>();

            int recordCount;

            if (!OrderDetails.Currency.Equals(config.CurrencyCode, StringComparison.InvariantCultureIgnoreCase))
            {
                result.Result = false;
                result.ErrorMessage = Resources.ErrorMessages_CurrencyNotSupported;
                return result;
            }

            if (!OrderDetails.PricingListId.Equals(Constants.DefaultValues.PriceList.ID, StringComparison.InvariantCultureIgnoreCase))
            {
                result.Result = false;
                result.ErrorMessage = Resources.ErrorMessages_PriceListNotSupported;
                return result;
            }


            if (OrderDetails.AccountId == null)
            {
                result.Result = false;
                result.ErrorMessage = Resources.ErrorMessages_AccountNotFound;
                return result;
            }

            customerID = OrderDetails.AccountId;
            if (!(customerID.StartsWith(Constants.CustomerIdPrefix, StringComparison.InvariantCultureIgnoreCase)))
            {
                result.Result = false;
                result.ErrorMessage = Resources.ErrorMessages_OnlyCustomersSupported;
                return result;
            }

            customerID = customerID.Substring(Constants.CustomerIdPrefix.Length);
            DataSet dataSet = new DataSet();
            OleDbDataAdapter dataAdapter;
            string sqlQuery = "Select * from Customers where CustomerID = '" + customerID + "'";


            using (OleDbConnection connection = new OleDbConnection(config.ConnectionString))
            {
                dataAdapter = new OleDbDataAdapter(sqlQuery, connection);
                if (dataAdapter.Fill(dataSet, "Customers") == 0)
                {
                    result.Result = false;
                    result.ErrorMessage = Resources.ErrorMessages_AccountNotFound;
                    return result;
                }
            }


            result.Result = true;
            result.ErrorMessage = "";

            result.PricingListId = Constants.DefaultValues.PriceList.ID;


            DataSets.Product product = new DataSets.Product();
            using (OleDbConnection connection = new OleDbConnection(config.ConnectionString))
            {
                DataSets.ProductTableAdapters.ProductsTableAdapter tableAdapter;
                tableAdapter = new DataSets.ProductTableAdapters.ProductsTableAdapter();
                tableAdapter.Connection = connection;
                recordCount = tableAdapter.Fill(product.Products);
            }

            result.PricingDetailLineItems = new PricingDetailLineItem[OrderDetails.LineItemDetails.Length];
            DataSets.Product.ProductsRow row;
            decimal totalPrice = 0;
            decimal totalLineItemDiscount = 0;
            result.ErrorMessage = "";
            for (int index = 0; index < OrderDetails.LineItemDetails.Length; index++)
            {
                result.PricingDetailLineItems[index] = new PricingDetailLineItem();
                result.PricingDetailLineItems[index].Description = OrderDetails.LineItemDetails[index].Description;
                result.PricingDetailLineItems[index].LineItemId = OrderDetails.LineItemDetails[index].LineItemId;
                result.PricingDetailLineItems[index].LineType = OrderDetails.LineItemDetails[index].LineType;
                result.PricingDetailLineItems[index].SynchMessage = "";

                try
                {
                    productId = Convert.ToInt32(OrderDetails.LineItemDetails[index].ProductId);
                }
                catch (Exception)
                {
                    result.PricingDetailLineItems[index].SynchMessage = String.Format(Resources.ErrorMessages_ProductIdWrongFormat, OrderDetails.LineItemDetails[index].ProductId.ToString());
                    result.ErrorMessage += result.PricingDetailLineItems[index].SynchMessage + "\r\n";
                    result.Result = false;
                    continue;
                }
                if (productIds.Contains(productId))
                {
                    result.PricingDetailLineItems[index].SynchMessage = String.Format(Resources.ErrorMessages_OrderWithProductTwice, productId);
                    //result.ErrorMessage += result.PricingDetailLineItems[index].SynchMessage + "\r\n";
                    //result.Result = false;
                    //continue;
                }

                productIds.Add(productId);

                row = product.Products.FindByProductID(productId);
                if (row == null)
                {
                    result.PricingDetailLineItems[index].SynchMessage = String.Format(Resources.ErrorMessages_ProductIdNotFound, productId);
                    result.ErrorMessage += result.PricingDetailLineItems[index].SynchMessage + "\r\n";
                    result.Result = false;
                    continue;
                }


                //result.PricingDetailLineItems[index].RepricingStatus = ??;
                
                //result.PricingDetailLineItems[index].StockQuantity = row.IsUnitsInStockNull() ? 0 : (int)row.UnitsInStock;
                
                result.PricingDetailLineItems[index].ListPrice = row.IsUnitPriceNull() ? (decimal)0 : row.UnitPrice;

                result.PricingDetailLineItems[index].Tax = 0;
                //result.PricingDetailLineItems[index].TaxRate = "no Tax";
                result.PricingDetailLineItems[index].TaxRate = "0";
                
                result.PricingDetailLineItems[index].DiscountRate = 0;
                result.PricingDetailLineItems[index].Discount = 0;
                if (OrderDetails.LineItemDetails[index].Quantity >= 10)
                {
                    result.PricingDetailLineItems[index].DiscountRate = 10;
                    result.PricingDetailLineItems[index].Discount = 10;
                }

                result.PricingDetailLineItems[index].DiscountSum = ((result.PricingDetailLineItems[index].DiscountRate / 100)) * result.PricingDetailLineItems[index].ListPrice;
                
                result.PricingDetailLineItems[index].QuotedPrice = result.PricingDetailLineItems[index].ListPrice * (1 - (result.PricingDetailLineItems[index].DiscountRate / 100));
                result.PricingDetailLineItems[index].QuotedPriceTotal = OrderDetails.LineItemDetails[index].Quantity * result.PricingDetailLineItems[index].QuotedPrice;
                totalPrice += result.PricingDetailLineItems[index].QuotedPriceTotal;
                totalLineItemDiscount += OrderDetails.LineItemDetails[index].Quantity * result.PricingDetailLineItems[index].DiscountSum;
            }

            result.DiscountAmt = 0;
            result.DiscountPC = 0;
            result.DiscountType = "";
            result.GrossAmt = totalPrice;
            result.LineItemDisc = totalLineItemDiscount;
            result.NetAmt = totalPrice;
            //result.NoDiscAmt = 0; //????
            result.PricingListId = OrderDetails.PricingListId;
            result.OrderQuoteId = OrderDetails.OrderQuoteId;
            //result.Reference = "";
            result.Tax = 0;
           
            return result;
        }

        public Pricing CheckPrice(Pricing PricingInformation, NorthwindConfig config)
        {
            int productId = 0;
            int recordCount;




            //TODO: did we need to check if Account exists?
            try
            {
                productId = int.Parse(PricingInformation.ProductId);
                if (productId == 0)
                    throw new ArgumentException();
            }
            catch (Exception)
            {
                PricingInformation.SynchMessage = Resources.ErrorMessages_ProductNotFound;
                return PricingInformation;
            }


            DataSets.Product product = new DataSets.Product();
            using (OleDbConnection connection = new OleDbConnection(config.ConnectionString))
            {
                DataSets.ProductTableAdapters.ProductsTableAdapter tableAdapter;
                tableAdapter = new DataSets.ProductTableAdapters.ProductsTableAdapter();
                tableAdapter.Connection = connection;
                recordCount = tableAdapter.FillBy(product.Products, productId);
            }
            if (recordCount == 0)
            {
                PricingInformation.SynchMessage = Resources.ErrorMessages_ProductNotFound;
                return PricingInformation;
            }

            DataSets.Product.ProductsRow row = product.Products[0];

            decimal listprice = row.IsUnitPriceNull() ? (decimal)0 : row.UnitPrice;

            PricingInformation.ListPrice = XmlConvert.ToString(listprice);

            PricingInformation.DiscountRate = 0;
            PricingInformation.Discount = 0;
            PricingInformation.DiscountSum = 0;

            if (PricingInformation.Quantity >= 10)
            {
                PricingInformation.DiscountRate = new decimal(10);
                PricingInformation.Discount = new decimal(10);
                PricingInformation.DiscountSum = listprice * new decimal(0.1);
            }

            decimal quotedPrice = listprice - PricingInformation.DiscountSum;

            PricingInformation.QuotedPrice = XmlConvert.ToString(quotedPrice);
            PricingInformation.QuotedPriceTotal = XmlConvert.ToString(PricingInformation.Quantity * quotedPrice);
            PricingInformation.Tax = 0;
            PricingInformation.TaxRate = 0;

            return PricingInformation;
        }

        #endregion

        #region private Helpers
        
        private ProductDocument GetProductDocument(Sage.Integration.Northwind.Application.Entities.Product.DataSets.Product.ProductsRow productRow, Token lastToken, NorthwindConfig config)
        {
            ProductDocument productDoc;
            string identity;
            bool active;

            identity = productRow.ProductID.ToString();

            // create Product Document
            productDoc = new ProductDocument();
            productDoc.Id = identity;

            if (lastToken.InitRequest)
                productDoc.LogState  = LogState.Created;

            else if (productRow.IsCreateIDNull() || productRow.IsModifyIDNull()
                || productRow.IsCreateUserNull() || productRow.IsModifyUserNull())
                productDoc.LogState = LogState.Created;

            else if ((productRow.CreateID >= lastToken.SequenceNumber) && (productRow.CreateUser != config.CrmUser))
                productDoc.LogState  = LogState.Created;
            else if ((productRow.ModifyID >= lastToken.SequenceNumber) && (productRow.ModifyUser != config.CrmUser))
                productDoc.LogState  = LogState.Updated;

            active = productRow.IsDiscontinuedNull() ? false : !productRow.Discontinued;
            productDoc.active.Value = active ? Constants.DefaultValues.Active : Constants.DefaultValues.NotActive;

            productDoc.code.Value = identity;

            productDoc.name.Value = productRow.IsProductNameNull() ? null : productRow.ProductName;

            productDoc.productfamilyid.Value = productRow.IsCategoryIDNull() ? 0 : productRow.CategoryID;

            productDoc.uomcategory.Value = identity;

            productDoc.instock.Value = productRow.IsUnitsInStockNull() ? 0 : Convert.ToInt32(productRow.UnitsInStock);

            return productDoc;

        }

        #endregion
        
        public override Document GetDocument(Identity identity, Token lastToken, NorthwindConfig config)
        {
            int recordCount;
            DataSets.Product product = new DataSets.Product();
            int productId;

            productId = Identity.GetId(identity);



            using (OleDbConnection connection = new OleDbConnection(config.ConnectionString))
            {
                Sage.Integration.Northwind.Application.Entities.Product.DataSets.ProductTableAdapters.ProductsTableAdapter tableAdapter;
                tableAdapter = new Sage.Integration.Northwind.Application.Entities.Product.DataSets.ProductTableAdapters.ProductsTableAdapter();
                tableAdapter.Connection = connection;
                recordCount = tableAdapter.FillBy(product.Products, productId);
            }

            if (recordCount == 0)
                return GetDeletedDocument(identity);

            return GetProductDocument((DataSets.Product.ProductsRow)product.Products[0], lastToken, config);

        }

        /* Change Log */
        public override int FillChangeLog(out System.Data.DataTable table, NorthwindConfig config, Token lastToken)
        {
            #region Declarations
            int recordCount = 0;
            DataSets.Product changelog = new DataSets.Product();
            int lastProductID = 0;
            #endregion


             lastProductID = Token.GetId(lastToken);

            // get the first 11 rows of the changelog
            using (OleDbConnection connection = new OleDbConnection(config.ConnectionString))
            {
                ChangeLogsTableAdapter tableAdapter;

                tableAdapter = new ChangeLogsTableAdapter();

                tableAdapter.Connection = connection;

                // fill the Changelog dataset
                if (lastToken.InitRequest)
                    recordCount = tableAdapter.Fill(changelog.ChangeLogs, lastProductID, lastToken.SequenceNumber, lastToken.SequenceNumber, "");
                else
                    recordCount = tableAdapter.Fill(changelog.ChangeLogs, lastProductID, lastToken.SequenceNumber, lastToken.SequenceNumber, config.CrmUser);
               
            }

            table = changelog.ChangeLogs;

            return recordCount;
        }
        
        /* Get */
        public override List<Identity> GetAll(NorthwindConfig config, string whereExpression, OleDbParameter[] oleDbParameters)
        {
            List<Identity> result = new List<Identity>();
            int recordCount = 0;
            DataSets.Product productsDataset = new DataSets.Product();

            // get the first 11 rows of the changelog
            using (OleDbConnection connection = new OleDbConnection(config.ConnectionString))
            {
                DataSets.ProductTableAdapters.ProductsTableAdapter tableAdapter;

                tableAdapter = new DataSets.ProductTableAdapters.ProductsTableAdapter();

                tableAdapter.Connection = connection;

                if (string.IsNullOrEmpty(whereExpression))
                    recordCount = tableAdapter.Fill(productsDataset.Products);
                else
                    recordCount = tableAdapter.FillByWhereClause(productsDataset.Products, whereExpression, oleDbParameters);
            }

            foreach (DataSets.Product.ProductsRow row in productsDataset.Products.Rows)
            {
                // use where expression !!
                result.Add(new Identity(this.EntityName, row.ProductID.ToString()));
            }

            return result;
        }
        public override List<Identity> GetAll(NorthwindConfig config, string whereExpression, int startIndex, int count)
        {
            throw new NotImplementedException();
        }

        /* Add */
        public override void Add(Document doc, NorthwindConfig config, ref List<TransactionResult> result)
        {
            List<TransactionResult> transactionResult = new List<TransactionResult>();
            ProductDocument productDoc = doc as ProductDocument;

            #region check input values
            
            if (productDoc == null)
            {
                result.Add(doc.SetTransactionStatus(TransactionStatus.UnRecoverableError, Resources.ErrorMessages_DocumentTypeNotSupported));
                return;
            }
            #endregion

            DataSets.ProductTableAdapters.ProductsTableAdapter tableAdapter;


            DataSets.Product productDataset = new DataSets.Product();
            DataSets.Product.ProductsRow row = productDataset.Products.NewProductsRow();


            #region fill dataset from document

            try
            {

                // active???
                
                // code???

                if (productDoc.name.IsNull)
                    row.SetProductNameNull();
                else
                    row.ProductName = (string)productDoc.name.Value;

                if (productDoc.productfamilyid.IsNull)
                    row.SetCategoryIDNull();
                else
                    row.CategoryID = (int)productDoc.productfamilyid.Value;

                // uom category ???

                if (productDoc.instock.IsNull)
                    row.SetUnitsInStockNull();
                else
                    row.UnitsInStock = Convert.ToInt16(productDoc.instock.Value);


                /* CreateID and ModifyID */
                row.CreateID = config.SequenceNumber;
                row.CreateUser = config.CrmUser;

                row.ModifyID = config.SequenceNumber;
                row.ModifyUser = config.CrmUser;

            }
            catch (Exception e)
            {
                productDoc.Id = "";
#warning Check error message
                result.Add(productDoc.SetTransactionStatus(TransactionStatus.UnRecoverableError, e.ToString()));
                return;
            }

            #endregion

            using (OleDbConnection connection = new OleDbConnection(config.ConnectionString))
            {
                connection.Open();

                tableAdapter = new DataSets.ProductTableAdapters.ProductsTableAdapter();
                tableAdapter.Connection = connection;

                productDataset.Products.AddProductsRow(row);
                
                tableAdapter.Update(productDataset.Products);
                
                OleDbCommand Cmd = new OleDbCommand("SELECT @@IDENTITY", connection);
                
                object lastid = Cmd.ExecuteScalar();

                productDoc.Id = ((int)lastid).ToString();
            }

            result.Add(doc.SetTransactionStatus(TransactionStatus.Success));
        }

        /* Update */
        public override void Update(Document doc, NorthwindConfig config, ref List<TransactionResult> result)
        {
            List<TransactionResult> transactionResult = new List<TransactionResult>();
            ProductDocument productDoc = doc as ProductDocument;

            #region check input values

            if (productDoc == null)
            {
                result.Add(doc.SetTransactionStatus(TransactionStatus.UnRecoverableError, Resources.ErrorMessages_DocumentTypeNotSupported));
                return;
            }

            // check id
            
            #endregion

            DataSets.ProductTableAdapters.ProductsTableAdapter tableAdapter;


            DataSets.Product productDataset = new DataSets.Product();
            DataSets.Product.ProductsRow row;
            tableAdapter = new DataSets.ProductTableAdapters.ProductsTableAdapter();
            using (OleDbConnection connection = new OleDbConnection(config.ConnectionString))
            {
                connection.Open();
                
                tableAdapter.Connection = connection;
                int recordCount = tableAdapter.FillBy(productDataset.Products, Convert.ToInt32(productDoc.Id));
                if (recordCount == 0)
                {
                    doc.SetTransactionStatus(TransactionStatus.UnRecoverableError, "Product does not exists");
                    return;
                }
                row = (DataSets.Product.ProductsRow)productDataset.Products.Rows[0];

                try
                {

                    // active???

                    // code???

                    if (productDoc.name.IsNull)
                        row.SetProductNameNull();
                    else
                        row.ProductName = (string)productDoc.name.Value;

                    if (productDoc.productfamilyid.IsNull)
                        row.SetCategoryIDNull();
                    else
                        row.CategoryID = (int)productDoc.productfamilyid.Value;

                    // uom category ???

                    if (productDoc.instock.IsNull)
                        row.SetUnitsInStockNull();
                    else
                        row.UnitsInStock = Convert.ToInt16(productDoc.instock.Value);

                    // ModifyID
                    row.ModifyID = config.SequenceNumber;
                    row.ModifyUser = config.CrmUser;
                }
                catch (Exception e)
                {
                    productDoc.Id = "";
#warning Check error message
                    result.Add(productDoc.SetTransactionStatus(TransactionStatus.UnRecoverableError, e.ToString()));
                    return;
                }

                tableAdapter = new DataSets.ProductTableAdapters.ProductsTableAdapter();
                tableAdapter.Connection = connection;

                tableAdapter.Update(productDataset.Products);

                result.Add(doc.SetTransactionStatus(TransactionStatus.Success));

            }
        }
    }
}
