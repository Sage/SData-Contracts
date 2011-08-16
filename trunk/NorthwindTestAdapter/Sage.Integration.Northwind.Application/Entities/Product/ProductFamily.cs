#region ©2007 Sage Technology Ltd., All Rights Reserved.
/*=====================================================================
	File:			ProductFamily.cs
	Author:			Philipp Schuette
	DateCreated:	04/12/2007 13:35:10
	DateChanged:	04/12/2007 13:35:10
 ---------------------------------------------------------------------
	©2007 Sage Technology Ltd., All Rights Reserved.	
=====================================================================*/

/*=====================================================================
	Date			        Author		Changes			Reasons
	04/12/2007 13:35:10	pschuette		Create			  
	Changes: Create, Refactoring, Upgrade	
=====================================================================*/
#endregion

#region Usings
using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Northwind.Application.Entities.Product.DataSets;
using System.Xml;
using Sage.Integration.Northwind.Application.Toolkit;
using System.Data;
using Sage.Integration.Northwind.Application.Entities.Product.Documents;
using Sage.Integration.Northwind.Application.Entities.Product.DataSets.CategoryTableAdapters;
using System.Data.OleDb;
using Sage.Integration.Northwind.Application.Base;
using Sage.Integration.Northwind.Application.API;
using Sage.Integration.Northwind.Application.Properties;
#endregion

namespace Sage.Integration.Northwind.Application.Entities.Product
{
    public class ProductFamily: EntityBase
    {
        #region constructor

        public ProductFamily() : base(Constants.EntityNames.ProductFamily)
        {
        }

        #endregion

        public override Document GetDocument(Identity identity, Token lastToken, NorthwindConfig config)
        {
            int recordCount;
            Category category = new Category();

            int id = Identity.GetId(identity);

            using (OleDbConnection connection = new OleDbConnection(config.ConnectionString))
            {
                CategoriesTableAdapter tableAdapter;
                tableAdapter = new CategoriesTableAdapter();
                tableAdapter.Connection = connection;
                recordCount = tableAdapter.FillBy(category.Categories, id);
            }

            if (recordCount == 0)
                return GetDeletedDocument(identity);

            return GetDocument((Category.CategoriesRow)category.Categories[0], lastToken, config);

        }

        private Document GetDocument(Category.CategoriesRow row, Token lastToken, NorthwindConfig config)
        {
            #region Declarations
            ProductFamilyDocument doc;
            string id;
            #endregion



            id = row.CategoryID.ToString();

            doc = new ProductFamilyDocument();
            doc.Id = id;

            if (lastToken.InitRequest)
                doc.LogState = LogState.Created;

            else if (row.IsCreateIDNull() || row.IsModifyIDNull()
                || row.IsCreateUserNull() || row.IsModifyUserNull())
                doc.LogState = LogState.Created;

            else if ((row.CreateID > lastToken.SequenceNumber)
                   && (row.CreateUser != config.CrmUser))
                doc.LogState = LogState.Created;

            else if ((row.CreateID == lastToken.SequenceNumber)
                && (row.CreateUser != config.CrmUser)
                && (id.CompareTo(lastToken.Id.Id) > 0))
                doc.LogState = LogState.Created;
            else if ((row.ModifyID >= lastToken.SequenceNumber) && (row.ModifyUser != config.CrmUser))
                doc.LogState = LogState.Updated;

            doc.active.Value = Constants.DefaultValues.Active;
            doc.name.Value = row.IsCategoryNameNull() ? null : row.CategoryName;

            doc.description.Value = row.IsDescriptionNull() ? null : row.Description;

            return doc;

        }

        public override int FillChangeLog(out DataTable table, NorthwindConfig config, Token lastToken)
        {
            #region declarations
            Category category;
            int lastId;
            int recordCount;
            #endregion

            category = new Category();

            lastId = Token.GetId(lastToken);

            using (OleDbConnection connection = new OleDbConnection(config.ConnectionString))
            {

                ChangeLogTableAdapter tableAdapter;
                tableAdapter = new ChangeLogTableAdapter();
                tableAdapter.Connection = connection;
                // fill the Changelog dataset
                if (lastToken.InitRequest)
                    recordCount = tableAdapter.Fill(category.ChangeLog, lastId, lastToken.SequenceNumber, lastToken.SequenceNumber, "");
                else
                    recordCount = tableAdapter.Fill(category.ChangeLog, lastId, lastToken.SequenceNumber, lastToken.SequenceNumber, config.CrmUser);
               
            }

            table = category.ChangeLog;
            return recordCount;
        }

        public override void Add(Document doc, NorthwindConfig config, ref List<TransactionResult> result)
        {

            List<TransactionResult> transactionResult = new List<TransactionResult>();
            ProductFamilyDocument productFamilyDoc = doc as ProductFamilyDocument;

            #region check input values
            if (productFamilyDoc == null)
            {
                result.Add(doc.SetTransactionStatus(TransactionStatus.UnRecoverableError, Resources.ErrorMessages_DocumentTypeNotSupported));
                return;
            }
            #endregion

            DataSets.CategoryTableAdapters.CategoriesTableAdapter tableAdapter;


            DataSets.Category category = new DataSets.Category();
            DataSets.Category.CategoriesRow row = category.Categories.NewCategoriesRow();


            #region fill dataset from document
            try
            {

                if (productFamilyDoc.name.IsNull)
                    row.SetCategoryNameNull();
                else
                    row.CategoryName = (string)productFamilyDoc.name.Value;

                if (productFamilyDoc.description.IsNull)
                    row.SetDescriptionNull();
                else
                    row.Description = (string)productFamilyDoc.description.Value;


                row.CreateID = config.SequenceNumber;
                row.CreateUser = config.CrmUser;

                row.ModifyID = config.SequenceNumber;
                row.ModifyUser = config.CrmUser;

            }
            catch (Exception e)
            {
                productFamilyDoc.Id = "";
#warning Check error message
                result.Add(productFamilyDoc.SetTransactionStatus(TransactionStatus.UnRecoverableError, e.ToString()));
                return;
            }

            #endregion

            using (OleDbConnection connection = new OleDbConnection(config.ConnectionString))
            {
              
                    connection.Open();


                    tableAdapter = new DataSets.CategoryTableAdapters.CategoriesTableAdapter();
                    tableAdapter.Connection = connection;


                    category.Categories.AddCategoriesRow(row);
                    tableAdapter.Update(category.Categories);
                    OleDbCommand Cmd = new OleDbCommand("SELECT @@IDENTITY", connection);
                    object lastid = Cmd.ExecuteScalar();
                    productFamilyDoc.Id = ((int)lastid).ToString();



            }
            result.Add(doc.SetTransactionStatus(TransactionStatus.Success));
        }

        public override void Update(Document doc, NorthwindConfig config, ref List<TransactionResult> result)
        {
            List<TransactionResult> transactionResult = new List<TransactionResult>();
            ProductFamilyDocument productFamilyDoc = doc as ProductFamilyDocument;

            #region check input values
            if (productFamilyDoc == null)
            {
                result.Add(doc.SetTransactionStatus(TransactionStatus.UnRecoverableError, Resources.ErrorMessages_DocumentTypeNotSupported));
                return;
            }

            // check id
            #endregion

            DataSets.CategoryTableAdapters.CategoriesTableAdapter tableAdapter;


            DataSets.Category category = new DataSets.Category();
            DataSets.Category.CategoriesRow row;
            tableAdapter = new DataSets.CategoryTableAdapters.CategoriesTableAdapter();
            using (OleDbConnection connection = new OleDbConnection(config.ConnectionString))
            {

                connection.Open();
                tableAdapter.Connection = connection;
                int recordCount = tableAdapter.FillBy(category.Categories, Convert.ToInt32(productFamilyDoc.Id));
                if (recordCount == 0)
                {
                    doc.SetTransactionStatus(TransactionStatus.UnRecoverableError, "Category does not exists");
                    return;
                }
                row = (Category.CategoriesRow)category.Categories.Rows[0];

                try
                {
                    if(!productFamilyDoc.name.NotSet)
                        if (productFamilyDoc.name.IsNull)
                            row.SetCategoryNameNull();
                        else
                            row.CategoryName = (string)productFamilyDoc.name.Value;

                    if(!productFamilyDoc.description.NotSet)
                        if (productFamilyDoc.description.IsNull)
                            row.SetDescriptionNull();
                        else
                            row.Description = (string)productFamilyDoc.description.Value;

                    row.ModifyID = config.SequenceNumber;
                    row.ModifyUser = config.CrmUser;
                }
                catch (Exception e)
                {
                    productFamilyDoc.Id = "";
#warning Check error message
                    result.Add(productFamilyDoc.SetTransactionStatus(TransactionStatus.UnRecoverableError, e.ToString()));
                    return;
                }

                tableAdapter = new DataSets.CategoryTableAdapters.CategoriesTableAdapter();
                tableAdapter.Connection = connection;

                tableAdapter.Update(category.Categories);

                result.Add(doc.SetTransactionStatus(TransactionStatus.Success));
                
            }


        }
        public override List<Identity> GetAll(NorthwindConfig config, string whereExpression, OleDbParameter[] oleDbParameters)
        {
            #region Declarations
            List<Identity> result = new List<Identity>();
            int recordCount = 0;
            Category dataset = new Category();
            #endregion

            // get the first 11 rows of the changelog
            using (OleDbConnection connection = new OleDbConnection(config.ConnectionString))
            {
                CategoriesTableAdapter tableAdapter;

                tableAdapter = new CategoriesTableAdapter();

                tableAdapter.Connection = connection;

                if (string.IsNullOrEmpty(whereExpression))
                    recordCount = tableAdapter.Fill(dataset.Categories);
                else
                    recordCount = tableAdapter.FillByWhereClause(dataset.Categories, whereExpression, oleDbParameters);
            }

            foreach (Category.CategoriesRow row in dataset.Categories.Rows)
            {
                // use where expression !!
                result.Add(new Identity(this.EntityName, row.CategoryID.ToString()));
            }

            return result;
        }

        public override List<Identity> GetAll(NorthwindConfig config, string whereExpression, int startIndex, int count)
        {
            throw new NotImplementedException();
        }

    }
}
