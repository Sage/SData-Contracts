#region Usings

using System;
using System.Collections.Generic;
using System.Data.OleDb;
using Sage.Integration.Northwind.Application.API;
using Sage.Integration.Northwind.Application.Base;
using Sage.Integration.Northwind.Application.Entities.Account.DataSets;
using Sage.Integration.Northwind.Application.Entities.Account.DataSets.EmailsTableAdapters;
using Sage.Integration.Northwind.Application.Entities.Account.Documents;
using Sage.Integration.Northwind.Application.Properties;

#endregion

namespace Sage.Integration.Northwind.Application.Entities.Email
{
    public class Email : EntityBase
    {
        #region Ctor.

        public Email() : base(Constants.EntityNames.Email)
        {
        }

        #endregion

        /* Get */
        public override Document GetDocument(Identity identity, Token lastToken, NorthwindConfig config)
        {
            int recordCount;
            Emails emails = new Emails();

            int id = Identity.GetId(identity);
            //string strId = Convert.ToString(id);

            using (OleDbConnection connection = new OleDbConnection(config.ConnectionString))
            {
                CustomerEmailsTableAdapter tableAdapter;
                tableAdapter = new CustomerEmailsTableAdapter();
                tableAdapter.Connection = connection;
                recordCount = tableAdapter.FillBy(emails.CustomerEmails, id);
            }

            if (recordCount == 0)
                return GetDeletedDocument(identity);

            return GetDocument((Emails.CustomerEmailsRow)emails.CustomerEmails[0], lastToken, config);
        }

        public override List<Identity> GetAll(NorthwindConfig config, string whereExpression, OleDbParameter[] oleDbParameters)
        {
            List<Identity> result = new List<Identity>();
            int recordCount = 0;
            Emails emailsDataset = new Emails();

            // get the first 11 rows of the changelog
            using (OleDbConnection connection = new OleDbConnection(config.ConnectionString))
            {
                CustomerEmailsTableAdapter tableAdapter;

                tableAdapter = new CustomerEmailsTableAdapter();

                tableAdapter.Connection = connection;

                if (string.IsNullOrEmpty(whereExpression))
                    recordCount = tableAdapter.Fill(emailsDataset.CustomerEmails);
                else
                    recordCount = tableAdapter.FillByWhereClause(emailsDataset.CustomerEmails, whereExpression, oleDbParameters);
            }

            foreach (Emails.CustomerEmailsRow row in emailsDataset.CustomerEmails.Rows)
            {
                // use where expression !!
                result.Add(new Identity(this.EntityName, row.ID.ToString()));
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

            // cast the given document to an email document
            // return if fails
            EmailDocument emailDocument = doc as EmailDocument;
            if (emailDocument == null)
            {
                result.Add(doc.SetTransactionStatus(TransactionStatus.UnRecoverableError, Resources.ErrorMessages_DocumentTypeNotSupported));
                return;
            }

            CustomerEmailsTableAdapter tableAdapter;
            Emails emailsDataset = new Emails();
            Emails.CustomerEmailsRow emailRow = emailsDataset.CustomerEmails.NewCustomerEmailsRow();

            #region fill dataset from document

            try
            {
                if (emailDocument.emailaddress.IsNull)
                    emailRow.SetEmailNull();
                else
                    emailRow.Email = (string)emailDocument.emailaddress.Value;

                emailRow.CreateID = config.SequenceNumber;
                emailRow.CreateUser = config.CrmUser;

                emailRow.ModifyID = config.SequenceNumber;
                emailRow.ModifyUser = config.CrmUser;
            }
            catch (Exception e)
            {
                emailDocument.Id = "";
#warning Check error message
                result.Add(emailDocument.SetTransactionStatus(TransactionStatus.UnRecoverableError, e.ToString()));
                return;
            }

            #endregion

            #region Get the ID of the new row and set it to the document

            using (OleDbConnection connection = new OleDbConnection(config.ConnectionString))
            {
                connection.Open();


                tableAdapter = new CustomerEmailsTableAdapter();
                tableAdapter.Connection = connection;


                emailsDataset.CustomerEmails.AddCustomerEmailsRow(emailRow);
                tableAdapter.Update(emailsDataset.CustomerEmails);
                OleDbCommand Cmd = new OleDbCommand("SELECT @@IDENTITY", connection);
                object lastid = Cmd.ExecuteScalar();
                emailDocument.Id = ((int)lastid).ToString();
            }

            #endregion
        }

        /* Update */
        public override void Update(Document doc, NorthwindConfig config, ref List<TransactionResult> result)
        {
            List<TransactionResult> transactionResult = new List<TransactionResult>();
            EmailDocument emailDocument = doc as EmailDocument;

            #region check input values

            if (emailDocument == null)
            {
                result.Add(doc.SetTransactionStatus(TransactionStatus.UnRecoverableError, Resources.ErrorMessages_DocumentTypeNotSupported));
                return;
            }

            // check id
            #endregion

            CustomerEmailsTableAdapter tableAdapter;


            Emails emailsDataset = new Emails();
            Emails.CustomerEmailsRow emailsRow;
            tableAdapter = new CustomerEmailsTableAdapter();
            using (OleDbConnection connection = new OleDbConnection(config.ConnectionString))
            {
                connection.Open();
                tableAdapter.Connection = connection;
                int recordCount = tableAdapter.FillBy(emailsDataset.CustomerEmails, Convert.ToInt32(emailDocument.Id));
                if (recordCount == 0)
                {
                    doc.SetTransactionStatus(TransactionStatus.UnRecoverableError, "Category does not exists");
                    return;
                }
                emailsRow = (Emails.CustomerEmailsRow)emailsDataset.CustomerEmails.Rows[0];

                try
                {
                    if (emailDocument.emailaddress.IsNull)
                        emailsRow.SetEmailNull();
                    else
                        emailsRow.Email = (string)emailDocument.emailaddress.Value;

                    emailsRow.ModifyID = config.SequenceNumber;
                    emailsRow.ModifyUser = config.CrmUser;
                }
                catch (Exception e)
                {
                    emailDocument.Id = "";
#warning Check error message
                    result.Add(emailDocument.SetTransactionStatus(TransactionStatus.UnRecoverableError, e.ToString()));
                    return;
                }

                tableAdapter = new CustomerEmailsTableAdapter();
                tableAdapter.Connection = connection;

                tableAdapter.Update(emailsDataset.CustomerEmails);
            }
        }

        /* ChangeLog */
        public override int FillChangeLog(out System.Data.DataTable table, NorthwindConfig config, Token lastToken)
        {
            Emails emails;
            int lastId;
            int recordCount;

            emails = new Emails();

            lastId = Token.GetId(lastToken);

            using (OleDbConnection connection = new OleDbConnection(config.ConnectionString))
            {
                CustomerEmailsChangeLogsTableAdapter tableAdapter;
                tableAdapter = new CustomerEmailsChangeLogsTableAdapter();
                tableAdapter.Connection = connection;
                // fill the Changelog dataset
                if (lastToken.InitRequest)
                    recordCount = tableAdapter.Fill(emails.CustomerEmailsChangeLogs, lastId, lastToken.SequenceNumber, lastToken.SequenceNumber, "");
                else
                    recordCount = tableAdapter.Fill(emails.CustomerEmailsChangeLogs, lastId, lastToken.SequenceNumber, lastToken.SequenceNumber, config.CrmUser);

            }

            table = emails.CustomerEmailsChangeLogs;
            return recordCount;
        }

        #region Private Helpers

        private Document GetDocument(Emails.CustomerEmailsRow row, Token lastToken, NorthwindConfig config)
        {
            EmailDocument doc;
            string id;

            id = row.ID.ToString();

            doc = new EmailDocument();
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

            doc.emailaddress.Value = row.IsEmailNull() ? null : row.Email;

            doc.type.Value = Constants.DefaultValues.Email.Type;

            return doc;

        }

        #endregion
    }
}
