#region ©2007 Sage Technology Ltd., All Rights Reserved.
/*=====================================================================
	File:			EntityBase.cs
	Author:			Philipp Schuette
	DateCreated:	04/12/2007 14:11:01
	DateChanged:	04/12/2007 14:11:01
 ---------------------------------------------------------------------
	©2007 Sage Technology Ltd., All Rights Reserved.	
=====================================================================*/

/*=====================================================================
	Date			        Author		Changes			Reasons
	04/12/2007 14:11:01	pschuette		Create			  
	Changes: Create, Refactoring, Upgrade	
=====================================================================*/
#endregion

#region Usings
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Sage.Integration.Northwind.Application.Toolkit;
using System.Xml;
using System.Data.Common;
using Sage.Integration.Northwind.Application.API;
using Sage.Integration.Northwind.Application.Base;
using Sage.Integration.Northwind.Application.EntityResources;
using System.Reflection;
using System.Data.OleDb;

#endregion

namespace Sage.Integration.Northwind.Application.Base
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class EntityBase
    {
        private string entityName;
        private string delimiterClause;

        public string DelimiterClause
        {
            get { return delimiterClause; }
            set { delimiterClause = value; }
        }
        #region constructor

/// <summary>
/// Initiate an Entity with its name
/// </summary>
/// <param name="entityName"></param>
        public EntityBase(string entityName)
        {
            this.entityName = entityName;
            this.delimiterClause = string.Empty;
        }

        public EntityBase(string entityName, string delimiterClause)
        {
            this.entityName = entityName;
            this.delimiterClause = delimiterClause;
        }

        #endregion



        #region public members

        /// <summary>
        /// ExecuteTransaction is for the CRM system to send all create, update, and delete information from CRM to the ERP system. 
        /// </summary>
        /// <param name="TransactionData"></param>
        /// <param name="config">the configuration object</param>
        /// <returns>ExecuteTransactionsReponse is the response 
        /// from the ERP system providing results of the attempted changes.   
        /// The method returns an ArrayofTransactionResults, 
        /// which contains a list of TransactionResults.</returns>
        public TransactionResult[] ExecuteTransactions(Transaction[] TransactionData, NorthwindConfig config)
        {
            Document doc = GetDocumentTemplate();
            List<TransactionResult> result = new List<TransactionResult>();
            try
            {
                for (int index = 0; index < TransactionData.Length; index++)
                {
                    doc = GetDocumentTemplate();
                    doc.ReadFromXmlNode(TransactionData[index].Instance);
                    ExecuteTransaction(doc, config, ref result);
                }
            }
            catch(Exception e)
            {
                result.Add(doc.SetTransactionStatus(TransactionStatus.FatalError, e.Message));
            }
            return result.ToArray();
        }


        public Identity[] GetLastChanges(Token lastToken, NorthwindConfig config, out Token nextToken)
        {
            #region Declarations
            int recordCount = 0;
            DataTable dataTable;
            Identity identity;
            DataRow row;
            List<Identity> identites = new List<Identity>();
            #endregion

            recordCount = 11;
            nextToken = lastToken;

            while (recordCount == 11)
            {
                lastToken = nextToken;
                recordCount = FillChangeLog(out dataTable, config, lastToken);
                
                for (int i = 0; i < ((recordCount == 11) ? recordCount - 1 : recordCount); i++)
                {
                    //get the next changelog entry
                    row = dataTable.Rows[i];
                    identity = new Identity(this.EntityName, (string)row[0]);
                    identites.Add(identity);
                    nextToken = new Token(identity, (int)row[1], lastToken.InitRequest);
                }
            }
            return identites.ToArray();
        }


        /// <summary>
        /// GetChangeLog is a request to the ERP system for changes to 
        /// a specific named entity instance for specified time durations.  
        /// A change can be either create, update, or delete.
        /// </summary>
        /// <param name="lastToken">the last passed token</param>
        /// <param name="config">the configuration object</param>
        /// <returns>
        /// The response to GetChangeLog is a GetChangeLogResponse, 
        /// which returns a list of changes (creates, deletes and updates), 
        /// in a complex type called ArrayofChangeLogEntries
        /// ArrayofChangeLogEntries contains a list of ChangeLogEntry’s 
        /// which contain details of the changes.
        /// </returns>
        public ChangeLog GetChangelog(Token lastToken, NorthwindConfig config)
        {
            #region Declarations
            int recordCount = 0;
            Token token;
            Identity identity;
            Document doc;
            ChangeLog result = new ChangeLog();
            DataTable dataTable;
            DataRow row;
            XmlDocument xmlDoc = new XmlDocument();
            #endregion


            // get the first 11 rows of the changelog
            recordCount = FillChangeLog(out dataTable, config, lastToken);

            // set the attend flag if the changlog contains less then 11 recorde
            result.AtEnd = (recordCount < 11);

            // if there are 11 instances, only 100 will retuned in this request
            result.ChangeLogInstances = new ChangeLogEntry[recordCount == 11 ? 10 : recordCount];

            token = lastToken;

            // go thru at least 10 records of the changelog
            for (int i = 0; i < result.ChangeLogInstances.Length; i++)
            {
                //get the next changelog entry
                row = dataTable.Rows[i];

                identity = new Identity(this.EntityName, (string)row[0]);

                // create a token from the changelog data
                token = new Token(identity, (int)row[1], lastToken.InitRequest);

                // get the Document by id
                doc = GetDocument(identity, lastToken, config);

                // create a changelog entry
                result.ChangeLogInstances[i] = new ChangeLogEntry();

                // set the token of this entry
                result.ChangeLogInstances[i].Token = Token.SerializeTokenToString(token);

                // add the xmldocument of the account document to the result
                result.ChangeLogInstances[i].Instance = doc.GetXmlNode(xmlDoc);

            }

            if (result.AtEnd)
                token.InitRequest = false;
                

            // update the nextpass token to the current one
            result.NextPassToken = Token.SerializeTokenToString(token);
            if (result.ChangeLogInstances.Length > 0)
                result.ChangeLogInstances[result.ChangeLogInstances.Length - 1].Token = result.NextPassToken;

            return result;
        }

        /// <summary>
        /// The Name of the Entity
        /// </summary>
        public string EntityName { get { return this.entityName; } }

        /// <summary>
        /// Returns an empty document
        /// </summary>
        /// <returns>an empty document of the correct type</returns>
        public Document GetDocumentTemplate()
        {
            return EntityFactory.GetDocumentTemplate(this.entityName);
        }

        /// <summary>
        /// returns a deleted document
        /// </summary>
        /// <param name="identity">the identity</param>
        /// <returns>a document of the correct type, mared as deleted within the identity</returns>
        protected Document GetDeletedDocument(Identity identity)
        {
            Document doc;
            doc = GetDocumentTemplate();
            doc.LogState = LogState.Deleted;
            doc.Id = identity.Id;
            try
            {
                PropertyInfo propInfo = doc.GetType().GetProperty("active");
                Property prop = (Property)propInfo.GetValue(doc, null);
                if (prop.TypeCode == TypeCode.Boolean)
                {
                    prop.Value = false;
                    doc.LogState = LogState.Updated;
                }
                if (prop.TypeCode == TypeCode.String)
                {
                    prop.Value = Constants.DefaultValues.NotActive;
                    doc.LogState = LogState.Updated;
                }
                
            }
            catch (Exception){}
            
            return doc;
        }

        #endregion

        #region abstract members



        
        /// <summary>
        /// returns a filled document by the identity
        /// </summary>
        /// <param name="identity">the identity</param>
        /// <param name="lastToken">the last token to set the logstate</param>
        /// <param name="config">the configuration object</param>
        /// <returns>returns a filled document by the identity</returns>
        public abstract Document GetDocument(Identity identity, Token lastToken, NorthwindConfig config);
        
        
        /// <summary>
        /// get the next changes from an entity
        /// </summary>
        /// <param name="table">the datatable to fill</param>
        /// <param name="config">the configuration object</param>
        /// <param name="lastToken">the last token to get the next 10 changelog entries</param>
        /// <returns>retunrs a recordcount and a filled datatable</returns>
        public abstract int FillChangeLog(out DataTable table, NorthwindConfig config, Token lastToken);

        public abstract List<Identity> GetAll(NorthwindConfig config, string whereExpression, OleDbParameter[] oleDbParameters);
        public abstract List<Identity> GetAll(NorthwindConfig config, string whereExpression, int startIndex, int count);
       

        #endregion

        #region virtual members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public virtual void Add(Document doc, NorthwindConfig config, ref List<TransactionResult> result)
        {
            result.Add(doc.GetOperationNotImplementedTransactionResult());
        }


        /// <summary>
        /// does an update for one entity entry
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public virtual void Update(Document doc, NorthwindConfig config, ref List<TransactionResult> result)
        {
            //result.Add(doc.GetSuccessTransactionResult());
            result.Add( doc.GetOperationNotImplementedTransactionResult());
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public virtual void Delete(Document doc, NorthwindConfig config, ref List<TransactionResult> result)
        {
            //result.Add(doc.GetSuccessTransactionResult());
            result.Add( doc.GetOperationNotImplementedTransactionResult());
        }
        #endregion

        #region private members

        /// <summary>
        /// dispatch the transaction between Add, Update,Delete
        /// </summary>
        /// <param name="doc">the data document</param>
        /// <param name="config">the configuratio Object</param>
        /// <returns>returns a transaction result for the transaction and all nested ones</returns>
        private void ExecuteTransaction(Document doc, NorthwindConfig config, ref List<TransactionResult> result)
        {
            if (doc.LogState == LogState.Created)
                Add(doc, config, ref result);

            else if (doc.LogState == LogState.Deleted)
                Delete(doc, config, ref result);

            else
                Update(doc, config, ref result);

        }

        #endregion

        protected void HandleDelimiterClause(ref string whereExpression)
        {
            if (!string.IsNullOrEmpty(this.DelimiterClause))
            {
                if (string.IsNullOrEmpty(whereExpression))
                {
                    whereExpression = string.Format("where ({0})", this.DelimiterClause);
                }
                else
                {
                    whereExpression = whereExpression + " AND (" + this.DelimiterClause + ")";
                }
            }
        }

    }
}
