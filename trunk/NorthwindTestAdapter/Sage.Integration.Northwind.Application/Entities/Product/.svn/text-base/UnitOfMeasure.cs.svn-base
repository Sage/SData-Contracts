#region ©2007 Sage Technology Ltd., All Rights Reserved.
/*=====================================================================
	File:			UnitOfMeasure.cs
	Author:			Philipp Schuette
	DateCreated:	04/10/2007 16:55:09
	DateChanged:	04/10/2007 16:55:09
 ---------------------------------------------------------------------
	©2007 Sage Technology Ltd., All Rights Reserved.	
=====================================================================*/

/*=====================================================================
	Date			        Author		Changes			Reasons
	04/10/2007 16:55:09	pschuette		Create			  
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
    public class UnitOfMeasure : EntityBase
    {
        #region Ctor.

        public UnitOfMeasure() : base(Constants.EntityNames.UnitOfMeasure)
        {
        }

        #endregion


        #region Private Helpers

        private UnitOfMeasureDocument GetUOMDocument(Sage.Integration.Northwind.Application.Entities.Product.DataSets.Product.ProductsRow productRow, Token lastToken, NorthwindConfig config)
        {
            #region Declarations
            UnitOfMeasureDocument uomDoc;
            string identity;
            #endregion


            identity = productRow.ProductID.ToString();

            // create Account Doc
            uomDoc = new UnitOfMeasureDocument();
            uomDoc.Id = identity;

            if (lastToken.InitRequest)
                uomDoc.LogState = LogState.Created;

            else if (productRow.IsCreateIDNull() || productRow.IsModifyIDNull()
                || productRow.IsCreateUserNull() || productRow.IsModifyUserNull())
                uomDoc.LogState = LogState.Created;
            
            else if ((productRow.CreateID > lastToken.SequenceNumber)
                   && (productRow.CreateUser != config.CrmUser))
                uomDoc.LogState = LogState.Created;

            else if ((productRow.CreateID == lastToken.SequenceNumber)
           && (productRow.CreateUser != config.CrmUser)
                && (identity.CompareTo(lastToken.Id.Id) > 0))
                uomDoc.LogState = LogState.Created;
            else if ((productRow.ModifyID >= lastToken.SequenceNumber) && (productRow.ModifyUser != config.CrmUser))
                uomDoc.LogState = LogState.Updated;

            uomDoc.active.Value = Constants.DefaultValues.Active;

            uomDoc.name.Value = productRow.IsQuantityPerUnitNull() ? null : productRow.QuantityPerUnit.ToString(); ;

            uomDoc.units.Value = 1;

            uomDoc.defaultvalue.Value = true;

            uomDoc.familyid.Value = identity;

            return uomDoc;

        }

        #endregion

        public override Document GetDocument(Identity identity, Token lastToken, NorthwindConfig config)
        {
            int recordCount;
            DataSets.Product product = new DataSets.Product();
            int uomId;

            uomId = Identity.GetId(identity);



            using (OleDbConnection connection = new OleDbConnection(config.ConnectionString))
            {
                Sage.Integration.Northwind.Application.Entities.Product.DataSets.ProductTableAdapters.ProductsTableAdapter tableAdapter;
                tableAdapter = new Sage.Integration.Northwind.Application.Entities.Product.DataSets.ProductTableAdapters.ProductsTableAdapter();
                tableAdapter.Connection = connection;
                recordCount = tableAdapter.FillBy(product.Products, uomId);
            }

            if (recordCount == 0)
                return GetDeletedDocument(identity);

            return GetUOMDocument((DataSets.Product.ProductsRow)product.Products[0], lastToken, config);

        }

        /* ChangeLog */
        public override int FillChangeLog(out System.Data.DataTable table, NorthwindConfig config, Token lastToken)
        {
            #region Declarations
            int recordCount = 0;
            DataSets.Product changelog = new DataSets.Product();
            int lastUomID = 0;
            ChangeLogsTableAdapter tableAdapter;
            #endregion


            lastUomID = Token.GetId(lastToken);

            // get the first 11 rows of the changelog
            using (OleDbConnection connection = new OleDbConnection(config.ConnectionString))
            {
                tableAdapter = new ChangeLogsTableAdapter();
                tableAdapter.Connection = connection;
                // fill the Changelog dataset
                if (lastToken.InitRequest)
                    recordCount = tableAdapter.Fill(changelog.ChangeLogs, lastUomID, lastToken.SequenceNumber, lastToken.SequenceNumber, "");
                else
                    recordCount = tableAdapter.Fill(changelog.ChangeLogs, lastUomID, lastToken.SequenceNumber, lastToken.SequenceNumber, config.CrmUser);
               
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
            UnitOfMeasureDocument uomDoc = doc as UnitOfMeasureDocument;

            #region check input values

            if (uomDoc == null)
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

                if (uomDoc.name.IsNull)
                    row.SetQuantityPerUnitNull();
                else
                    row.QuantityPerUnit = (string)uomDoc.name.Value;

                // units ???
                
                // defaultvalue ???

                // familyId ???

                /* CreateID and ModifyID */
                row.CreateID = config.SequenceNumber;
                row.CreateUser = config.CrmUser;

                row.ModifyID = config.SequenceNumber;
                row.ModifyUser = config.CrmUser;

            }
            catch (Exception e)
            {
                uomDoc.Id = "";
#warning Check error message
                result.Add(uomDoc.SetTransactionStatus(TransactionStatus.UnRecoverableError, e.ToString()));
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

                uomDoc.Id = ((int)lastid).ToString();
            }

            result.Add(doc.SetTransactionStatus(TransactionStatus.Success));
        }

        /* Update */
        public override void Update(Document doc, NorthwindConfig config, ref List<TransactionResult> result)
        {
            List<TransactionResult> transactionResult = new List<TransactionResult>();
            UnitOfMeasureDocument uomDoc = doc as UnitOfMeasureDocument;

            #region check input values

            if (uomDoc == null)
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
                int recordCount = tableAdapter.FillBy(productDataset.Products, Convert.ToInt32(uomDoc.Id));
                if (recordCount == 0)
                {
                    doc.SetTransactionStatus(TransactionStatus.UnRecoverableError, "Product does not exists");
                    return;
                }
                row = (DataSets.Product.ProductsRow)productDataset.Products.Rows[0];

                try
                {

                    // active???

                    if (uomDoc.name.IsNull)
                        row.SetQuantityPerUnitNull();
                    else
                        row.QuantityPerUnit = (string)uomDoc.name.Value;

                    // units ???

                    // defaultvalue ???

                    // familyId ???

                    // ModifyID
                    row.ModifyID = config.SequenceNumber;
                    row.ModifyUser = config.CrmUser;
                }
                catch (Exception e)
                {
                    uomDoc.Id = "";
#warning Check error message
                    result.Add(uomDoc.SetTransactionStatus(TransactionStatus.UnRecoverableError, e.ToString()));
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
