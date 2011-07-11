#region ©2007 Sage Technology Ltd., All Rights Reserved.
/*=====================================================================
	File:			Price.cs
	Author:			Philipp Schuette
	DateCreated:	04/10/2007 11:12:36
	DateChanged:	04/10/2007 11:12:36
 ---------------------------------------------------------------------
	©2007 Sage Technology Ltd., All Rights Reserved.	
=====================================================================*/

/*=====================================================================
	Date			        Author		Changes			Reasons
	04/10/2007 11:12:36	pschuette		Create			  
	Changes: Create, Refactoring, Upgrade	
=====================================================================*/
#endregion

#region Usings
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using Sage.Integration.Northwind.Application.API;
using Sage.Integration.Northwind.Application.Base;
using Sage.Integration.Northwind.Application.Entities.Product.DataSets.ProductTableAdapters;
using Sage.Integration.Northwind.Application.Entities.Product.Documents;
using Sage.Integration.Northwind.Application.Properties;
#endregion

namespace Sage.Integration.Northwind.Application.Entities.Product
{
    public class Price : EntityBase
    {
        #region Ctor

        public Price() : base (Constants.EntityNames.Price)
        {
        }

        #endregion

        #region Private Helpers

        private PriceDocument GetDocument(DataSets.Product.ProductsRow productRow, Token lastToken, NorthwindConfig config)
        {
            #region Declarations
            PriceDocument priceDoc;
            string identity;
            #endregion

            identity = productRow.ProductID.ToString();

            // create Account Doc
            priceDoc = new PriceDocument();
            priceDoc.Id = identity;

            if (lastToken.InitRequest)
                priceDoc.LogState = LogState.Created;

            else if (productRow.IsCreateIDNull() || productRow.IsModifyIDNull()
                || productRow.IsCreateUserNull() || productRow.IsModifyUserNull())
                priceDoc.LogState = LogState.Created;

            else if ((productRow.CreateID > lastToken.SequenceNumber)
                 && (productRow.CreateUser != config.CrmUser))
                priceDoc.LogState = LogState.Created;

            else if ((productRow.CreateID == lastToken.SequenceNumber)
                && (productRow.CreateUser != config.CrmUser)
                && (identity.CompareTo(lastToken.Id.Id) > 0))
                priceDoc.LogState = LogState.Created;
            else if ((productRow.ModifyID >= lastToken.SequenceNumber) && (productRow.ModifyUser != config.CrmUser))
                priceDoc.LogState = LogState.Updated;

            priceDoc.active.Value = true;

            priceDoc.price_cid.Value = config.CurrencyCode;

            priceDoc.pricinglistid.Value = Constants.DefaultValues.PriceList.ID;

            priceDoc.productid.Value = identity;

            priceDoc.uomid.Value = identity;

            priceDoc.price.Value = productRow.IsUnitPriceNull() ? new Decimal(0) : productRow.UnitPrice;

            return priceDoc;

        }

        #endregion

        public override Document GetDocument(Identity identity, Token lastToken, NorthwindConfig config)
        {
            int recordCount;
            DataSets.Product product = new DataSets.Product();
            int priceID;
            
            priceID = Identity.GetId(identity);


            using (OleDbConnection connection = new OleDbConnection(config.ConnectionString))
            {
                DataSets.ProductTableAdapters.ProductsTableAdapter tableAdapter;
                tableAdapter = new DataSets.ProductTableAdapters.ProductsTableAdapter();
                tableAdapter.Connection = connection;
                recordCount = tableAdapter.FillBy(product.Products, priceID);
            }

            if (recordCount == 0)
                return GetDeletedDocument(identity);

            return GetDocument((DataSets.Product.ProductsRow)product.Products[0], lastToken, config);

        }

        /* ChangeLog */
        public override int FillChangeLog(out System.Data.DataTable table, NorthwindConfig config, Token lastToken)
        {
            #region Declarations
            int recordCount = 0;
            DataSets.Product changelog = new DataSets.Product();
            int lastPriceID = 0;
            #endregion

            lastPriceID = Token.GetId(lastToken);


            // get the first 11 rows of the changelog
            using (OleDbConnection connection = new OleDbConnection(config.ConnectionString))
            {
                ChangeLogsTableAdapter tableAdapter;

                tableAdapter = new ChangeLogsTableAdapter();

                tableAdapter.Connection = connection;

                // fill the Changelog dataset
                if (lastToken.InitRequest)
                    recordCount = tableAdapter.Fill(changelog.ChangeLogs, lastPriceID, lastToken.SequenceNumber, lastToken.SequenceNumber, "");
                else
                    recordCount = tableAdapter.Fill(changelog.ChangeLogs, lastPriceID, lastToken.SequenceNumber, lastToken.SequenceNumber, config.CrmUser);
               
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
            PriceDocument priceDoc = doc as PriceDocument;

            #region check input values

            if (priceDoc == null)
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

                // price_cid???

                // pricinglistid ???

                // productid ???

                // uomid ???

                if (priceDoc.price.IsNull)
                    row.SetUnitPriceNull();
                else
                    row.UnitPrice = (decimal)priceDoc.price.Value;


                /* CreateID and ModifyID */
                row.CreateID = config.SequenceNumber;
                row.CreateUser = config.CrmUser;

                row.ModifyID = config.SequenceNumber;
                row.ModifyUser = config.CrmUser;

            }
            catch (Exception e)
            {
                priceDoc.Id = "";
#warning Check error message
                result.Add(priceDoc.SetTransactionStatus(TransactionStatus.UnRecoverableError, e.ToString()));
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

                priceDoc.Id = ((int)lastid).ToString();
            }

            result.Add(doc.SetTransactionStatus(TransactionStatus.Success));
        }

        /* Update */
        public override void Update(Document doc, NorthwindConfig config, ref List<TransactionResult> result)
        {
            List<TransactionResult> transactionResult = new List<TransactionResult>();
            PriceDocument priceDoc = doc as PriceDocument;

            #region check input values

            if (priceDoc == null)
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
                int recordCount = tableAdapter.FillBy(productDataset.Products, Convert.ToInt32(priceDoc.Id));
                if (recordCount == 0)
                {
                    doc.SetTransactionStatus(TransactionStatus.UnRecoverableError, "Product does not exists");
                    return;
                }
                row = (DataSets.Product.ProductsRow)productDataset.Products.Rows[0];

                try
                {

                    // active???

                    // price_cid???

                    // pricinglistid ???

                    // productid ???

                    // uomid ???

                    if (priceDoc.price.IsNull)
                        row.SetUnitPriceNull();
                    else
                        row.UnitPrice = (decimal)priceDoc.price.Value;

                    // ModifyID
                    row.ModifyID = config.SequenceNumber;
                    row.ModifyUser = config.CrmUser;
                }
                catch (Exception e)
                {
                    priceDoc.Id = "";
#warning Check error message
                    result.Add(priceDoc.SetTransactionStatus(TransactionStatus.UnRecoverableError, e.ToString()));
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
