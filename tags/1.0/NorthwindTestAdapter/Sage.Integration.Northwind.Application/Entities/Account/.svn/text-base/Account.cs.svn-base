#region ©2007 Sage Technology Ltd., All Rights Reserved.
/*=====================================================================
	File:			Account.cs
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
using System.Xml;
using System.Xml.Schema;
using System.Data.OleDb;
using System.Data;


using Sage.Integration.Northwind.Application.Toolkit;


using Sage.Integration.Northwind.Application.API;
using Sage.Integration.Northwind.Application.Base;
using Sage.Integration.Northwind.Application.Entities;
using Sage.Integration.Northwind.Application.EntityResources;

using Sage.Integration.Northwind.Application.Entities.Account.Documents;

using Sage.Integration.Northwind.Application.Entities.Account.DataSets;
using Sage.Integration.Northwind.Application.Entities.Account.DataSets.AccountDatasetTableAdapters;
using Sage.Integration.Northwind.Application.Entities.Account.DataSets.EmailsTableAdapters;
using Sage.Integration.Northwind.Application.Entities.Account.DataSets.DeleteHistoryDatasetTableAdapters;
using Sage.Integration.Northwind.Application.Properties;


#endregion

namespace Sage.Integration.Northwind.Application.Entities.Account
{

    /// <summary>
    /// The AccountEntity presents an account object of the application in the integration
    /// space. The entity supports: 
    ///  - GetDocument, GetDocumentTemplate 
    ///  - Insert, Update and Delete verb (to support execute transcation)
    ///  - FillChangeLog and FillAll (to Support getChangelog and getAll)
    /// </summary>
    public class Account : EntityBase
    {
        #region Constructor
        
        /// <summary>
        /// Constructor. The method initializes the base class.
        /// </summary>
        public Account()
            : base(Constants.EntityNames.Account)
        {
        }

        #endregion


        #region Entitybase Members

        /// <summary>
        /// The Update verb ammends an existing account and all subentities (Person, Email, Address, Phone) in the underlying data-store.
        /// The data as it should be stored is found in the parameter document.
        ///
        /// Should no account instance be found in the data-store that matches the id of the document, 
        /// the transaction result will set accordingly.
        /// </summary>
        /// <param name="doc">Document (incl. ID) containing the data to be stored</param>
        /// <param name="config">The configuration object</param>
        /// <returns>The transactionResult contais status information of the single transaction an also the nesed transactions of the subentities</returns>
        public override void Update(Document doc, NorthwindConfig config, ref List<TransactionResult> result)
        {
            #region declarations
            string accountID;
            AccountDocument sourceAccount;
            AccountDocument targetAccount;
            AccountDataset.AccountsRow row;
            #endregion

            // cast the given document to an account document
            sourceAccount = doc as AccountDocument;
            if (sourceAccount == null)
            {
                result.Add(doc.SetTransactionStatus(TransactionStatus.UnRecoverableError, Resources.ErrorMessages_DocumentTypeNotSupported));
                return;
            }

            
            // get the linked account id
            accountID = sourceAccount.Id;

            //try to get the original Document
            targetAccount = GetDocument(accountID, config) as AccountDocument;

            // return error if the account not found
            if ((targetAccount == null) || (targetAccount.LogState == LogState.Deleted))
            {
                result.Add(sourceAccount.SetTransactionStatus(TransactionStatus.UnRecoverableError, Resources.ErrorMessages_AccountNotFound));
                return;
            }

            // merge the source data into the original account
            targetAccount.Merge(sourceAccount);


            // convert the Acoount Doc into a Dataset row
            row = GetRow(targetAccount, config, false);

            // check if it is linked and the given id is a customer id
            if ((accountID != null) &&
                (accountID.Length > 2) &&
                accountID.StartsWith(Constants.CustomerIdPrefix))
            {
                // Store the taget account document into the customer and customeremail table
                StoreCustomer(targetAccount, row, config, ref result);
            }
            // check if it is linked and the given id is a customer id
            else if ((accountID != null) &&
                (accountID.Length > 2) &&
                accountID.StartsWith(Constants.SupplierIdPrefix))
            {
                // Store the taget account document into the suppliers table
                StoreSupplier(targetAccount, row, config, ref result);
            }

            // return the set transaction result
            //targetAccount.GetTransactionResult(true, ref result);
        }

        /// <summary> 
        /// The Delete verb deletes existing account and all subentities in the underlying data-store.
        /// 
        /// Should no account instance be found in the data-store that matches the id of the document, 
        /// the transaction result will set accordingly.
        /// </summary>
        /// <param name="doc">Document (incl. ID) containing the data to be stored</param>
        /// <param name="config">The configuration object</param>
        /// <returns>The transactionResult contais status information of the single transaction an also the nesed transactions of the subentities</returns>
        public override void Delete(Document doc, NorthwindConfig config, ref List<TransactionResult> result)
        {
            #region Declarations
            CustomersTableAdapter customers;
            SuppliersTableAdapter suppliers;
            DeleteHistoryTableAdapter deleteHistory;
            int sequenceId;
            string accountId ;
            string customerId = "";
            int supplierID = 0;
            #endregion


            // get the account Id from the given document
            accountId = doc.Id == null ? "" : doc.Id;

            // get the northwind Supplier or customer id if possible
            if (doc.Id.StartsWith(Constants.CustomerIdPrefix))
                customerId = accountId.Substring(Constants.CustomerIdPrefix.Length);

            else if (doc.Id.StartsWith(Constants.SupplierIdPrefix))
                supplierID = Identity.GetId(accountId.Substring(Constants.SupplierIdPrefix.Length));

            else
            {
                result.Add(doc.SetTransactionStatus(TransactionStatus.UnRecoverableError, Resources.ErrorMessages_AccountTypeNotSupported));
                return;
            }

            // get the logging sequence number
            sequenceId = config.SequenceNumber;

            using (OleDbConnection connection = new OleDbConnection(config.ConnectionString))
            {
                try
                {
                    // get Table  Adpater and set the connecion
                    deleteHistory = new DeleteHistoryTableAdapter();
                    deleteHistory.Connection = connection;
                    
                    if (supplierID > 0)
                    {
                        // get Table  Adpater and set the connecion
                        suppliers = new SuppliersTableAdapter();
                        suppliers.Connection = connection;
                        
                        // delete the supplier
                        suppliers.DeleteQuery(supplierID);

                        // logg the deleted supplier in the delete history
                        deleteHistory.LogSuppliers(supplierID.ToString(), sequenceId, config.CrmUser);
                        
                        // set transaction status to success
                        result.Add(doc.SetTransactionStatus(TransactionStatus.Success));
                    }
                    
                    else if (customerId.Length > 0)
                    {
                        // get Table  Adpater and set the connecion
                        customers = new CustomersTableAdapter();
                        customers.Connection = connection;

                        // delete The customer
                        customers.DeleteQuery(customerId);

                        // logg the deleted Customer in the delete history
                        deleteHistory.LogCustomers(customerId, sequenceId, config.CrmUser);

                        // set transaction status to success
                        result.Add(doc.SetTransactionStatus(TransactionStatus.Success));
                    }

                    else
                        // set transaction status
                        result.Add(doc.SetTransactionStatus(TransactionStatus.UnRecoverableError, Resources.ErrorMessages_AccountTypeNotSupported));
                }
                catch (Exception deleteException)
                { 
#warning Check the error status. this occours if the account is not deletable, because of having orders
                    // set transaction status and logg the error message
                    result.Add(doc.SetTransactionStatus(TransactionStatus.UnRecoverableError, deleteException.ToString()));
                }
            }

        }

        /// <summary>
        /// The Add verb inserts an account and all subentities  in the underlying data-store. On success, the
        /// identity of the newly inserted instances will added to the document and all subdocuments.
        /// </summary>
        /// <param name="doc">Document (without ID) containing the data to be stored</param>
        /// <param name="config">The configuration object</param>
        /// <returns>The transactionResult contais status information of the single transaction an also the nesed transactions of the subentities</returns>
        public override void Add(Document doc, NorthwindConfig config, ref List<TransactionResult> result)
        {
            #region Declarations
            AccountDocument accDoc;
            AccountDataset.AccountsRow row;
            #endregion

            // cast the given document to an account document
            accDoc = doc as AccountDocument;
            if (accDoc == null)
            {
                result.Add(doc.SetTransactionStatus(TransactionStatus.UnRecoverableError, Resources.ErrorMessages_DocumentTypeNotSupported));
                return;
            }

            // get the account row from the given document
            row = GetRow(accDoc, config, true);


            // store as customer if the account type is set to customer
            if (CRMSelections.acc_type_Customer == (string)accDoc.customerSupplierFlag.Value)
                StoreCustomer(accDoc, row, config, ref result);

            // store as supplier if the account type is set to supplier
            else if (CRMSelections.acc_type_Supplier == (string)accDoc.customerSupplierFlag.Value)
                StoreSupplier(accDoc, row, config, ref result);

           // for any other account type, set the transactionstatus to not supported
            else 
                result.Add(accDoc.SetTransactionStatus(
                TransactionStatus.UnRecoverableError,
                Resources.ErrorMessages_AccountTypeNotSupported));

        }

        /// <summary>
        /// The GetDocument verb retuns an complete AccountDocument containing the data associated with
        /// an existing account instance.
        ///
        /// Should no account instance be found in the data-store that matches the identity
        /// passed as a parameter it will return an empty account document marked as deleted within the given identity. 
        /// </summary>
        /// <param name="identity">Identity of the account</param>
        /// <param name="lastToken">the last given Token to mark the account status as updated or created</param>
        /// <param name="config">The configuration object</param>
        /// <returns>an account document</returns>
        public override Document GetDocument(Identity identity, Token lastToken, NorthwindConfig config)
        {
            #region declarations
            int recordCount;
            AccountsTableAdapter tableAdapter;
            AccountDataset account = new AccountDataset();
            #endregion


            // get the Account by the given identity
            using (OleDbConnection connection = new OleDbConnection(config.ConnectionString))
            {
                tableAdapter = new AccountsTableAdapter();
                tableAdapter.Connection = connection;
                recordCount = tableAdapter.FillBy(account.Accounts, identity.Id);
            }

            // when the record does not exists return an deleted document of the given identity
            if (recordCount == 0)
                return GetDeletedDocument(identity);

            // convert the dataset row to an account document and return it
            return GetDocument((AccountDataset.AccountsRow)account.Accounts[0], lastToken, config);
        }


        /// <summary>
        /// fill up a datatable with the next 11 id's of inserted, modified and deleted accounts since the given token. 
        /// </summary>
        /// <param name="table">tha DataTable to filled up. The first column contains the identity</param>
        /// <param name="config">The configuration object</param>
        /// <param name="lastToken">the last token</param>
        /// <returns>The count of all the identities available</returns>
        public override int FillChangeLog(out DataTable table, NorthwindConfig config, Token lastToken)
        {
            #region Declarations
            int recordCount = 0;
            AccountDataset changelog = new AccountDataset();
            #endregion

            // get the first 11 rows of the changelog
            using (OleDbConnection connection = new OleDbConnection(config.ConnectionString))
            {
                ChangeLogsTableAdapter tableAdapter;

                tableAdapter = new ChangeLogsTableAdapter();

                tableAdapter.Connection = connection;

                // fill the Changelog dataset
                // if the last token was an init request, the changelog will also return changes done by the crm User
                if (lastToken.InitRequest)
                    recordCount = tableAdapter.Fill(changelog.ChangeLogs, lastToken.Id.Id, lastToken.SequenceNumber, lastToken.SequenceNumber, "");
                else
                    recordCount = tableAdapter.Fill(changelog.ChangeLogs, lastToken.Id.Id, lastToken.SequenceNumber, lastToken.SequenceNumber, config.CrmUser);
            }

            // set the out table to the filled account data table
            table = changelog.ChangeLogs;

            // return the count of the existing records
            return recordCount;
        }
        

        #endregion

        #region private members

        #region GetDocument


        /// <summary>
        /// returns a complete account document without using a token
        /// </summary>
        /// <param name="accountId">the account id</param>
        /// <param name="config"></param>
        /// <returns></returns>
        private Document GetDocument(string accountId, NorthwindConfig config)
        {
            Identity identity = new Identity(this.EntityName, accountId);
            Token lastToken = new Token(new Identity(EntityName, ""), 0, true);

            return GetDocument(identity, lastToken, config);
        }

        private AccountDocument GetDocument(AccountDataset.AccountsRow row, Token lastToken, NorthwindConfig config)
        {

            #region Declarations
            CountryCodes countryCodes = new CountryCodes();
            AccountDocument accDoc;
            PersonDocument persDoc;
            PhoneDocument phoneDoc;
            AddressDocument addrDoc;
            Address address;
            Phone phone;
            string identity;
            ContactName contactName;
            #endregion


            identity = row.ID;

            // create Account Doc
            accDoc = new AccountDocument();
            
            // set the account id
            accDoc.Id = identity;

            // change the the log state regarding the timestamps stored in the northwind database.

            // for an init request the logstate is always created
            if (lastToken.InitRequest)
                accDoc.LogState = LogState.Created;

            // if something wrong, than it is created
            else if (row.IsCreateIDNull() || row.IsModifyIDNull() 
                || row.IsCreateUserNull() || row.IsModifyUserNull())
                accDoc.LogState = LogState.Created;


            // the log state is created if the create id is greater 
            // than the sequence number of the last token and it was not created by the crm user
            else if ((row.CreateID > lastToken.SequenceNumber) 
                && (row.CreateUser != config.CrmUser))
                accDoc.LogState = LogState.Created;

            else if ((row.CreateID == lastToken.SequenceNumber)
                && (row.CreateUser != config.CrmUser)
                && (identity.CompareTo(lastToken.Id.Id) > 0))
                accDoc.LogState = LogState.Created;

            // the log state is modified if the modify id is greater 
            // than the sequence number of the last token and it was not created by the crm user
            else if ((row.ModifyID >= lastToken.SequenceNumber) && (row.ModifyUser != config.CrmUser))
                accDoc.LogState = LogState.Updated;

            
            // set the account type
            //accDoc.type.Value = GetAccountType(identity);


            // set the account name
            accDoc.name.Value = row.IsCompanyNameNull() ? null : row.CompanyName;

            // set the customerSupplierFlag
            accDoc.customerSupplierFlag.Value = row.IsCustomerSupplierFlagNull() ? null : row.CustomerSupplierFlag;

            // set default values if it is no update
            if (accDoc.LogState != LogState.Updated)
            {
                accDoc.onhold.Value = !accDoc.Id.StartsWith(Constants.CustomerIdPrefix);
                accDoc.currencyid.Value = config.CurrencyCode;
            }

            // create person Doc
            persDoc = new PersonDocument();
            
            // since there is only one person in Northwind, the Identity of the person is the same as 
            // the account id
            persDoc.Id = identity;
            
            // set the log state if the account also has a logstate
            if (!accDoc.HasNoLogStatus)
                persDoc.LogState = accDoc.LogState;

            // set the transaction status for the person doc.
            persDoc.SetTransactionStatus(TransactionStatus.Success);

            // set the first and lst name to null if the contact in northwind is null
            if (row.IsContactNameNull())
            {
                persDoc.firstname.Value = null;
                persDoc.lastname.Value = null;
                persDoc.fullname.Value = null;
            }
            else
            {
                persDoc.fullname.Value = row.ContactName;

                // create an object to splitt the contact name
                contactName = new ContactName();

                // initiate the object with the northwind contact name
                contactName.NorthwindContacName = row.ContactName;

                // get the splitted values
                persDoc.salutation.Value = contactName.CrmSalutation;
                persDoc.firstname.Value = contactName.CrmFirstName;
                persDoc.middlename.Value = contactName.CrmMiddleName;
                persDoc.lastname.Value = contactName.CrmLastName;
                persDoc.suffix.Value = contactName.CrmSuffix;

            }

            if (row.IsContactTitleNull())
                persDoc.title.Value = null;
            else
                persDoc.title.Value = row.ContactTitle;

            // set the person type to billing
            persDoc.primaryperson.Value = "True";

            // add the person to the people collection of the account document
            accDoc.people.Add(persDoc);


            // create Phone Doc
            phoneDoc = new PhoneDocument();

            // since there are exact 2 phone numbers stored in northwind
            // the id for the phone number ist the account id plus a postfix
            phoneDoc.Id = identity + Constants.PhoneIdPostfix;

            // set the log state if the account also has a logstate
            if (!accDoc.HasNoLogStatus)
                phoneDoc.LogState = accDoc.LogState;

            // set the person type to business 
            phoneDoc.type.Value = CRMSelections.Link_PersPhon_Business;
            
            
            phoneDoc.SetTransactionStatus(TransactionStatus.Success);

            if (row.IsPhoneNull())
            {
                phoneDoc.countrycode.Value = null;
                phoneDoc.areacode.Value = null;
                phoneDoc.number.Value = null;
                phoneDoc.fullnumber.Value = null;
            }
            else
            {
                phoneDoc.fullnumber.Value = row.Phone;
                
                phone = new Phone();
                phone.NorthwindPhone = row.Phone;
                phoneDoc.countrycode.Value = phone.CrmCountryCode;
                phoneDoc.areacode.Value = phone.CrmAreaCode;
                phoneDoc.number.Value = phone.CrmPhone;
                
            }


            accDoc.phones.Add(phoneDoc);

            // create Fax Doc
            phoneDoc = new PhoneDocument();
            phoneDoc.Id = identity + Constants.FaxIdPostfix;
            if (!accDoc.HasNoLogStatus)
                phoneDoc.LogState = accDoc.LogState;
            phoneDoc.type.Value = CRMSelections.Link_PersPhon_Fax;
            phoneDoc.SetTransactionStatus(TransactionStatus.Success);
            if (row.IsFaxNull())
            {
                phoneDoc.countrycode.Value = null;
                phoneDoc.areacode.Value = null;
                phoneDoc.number.Value = null;
                phoneDoc.fullnumber.Value = null;
            }
            else
            {
                phoneDoc.fullnumber.Value = row.Fax;
                phone = new Phone();
                phone.NorthwindPhone = row.Fax;
                phoneDoc.countrycode.Value = phone.CrmCountryCode;
                phoneDoc.areacode.Value = phone.CrmAreaCode;
                phoneDoc.number.Value = phone.CrmPhone;

            }
            accDoc.phones.Add(phoneDoc);

            // create Address Doc
            addrDoc = new AddressDocument();
            addrDoc.Id = identity;
            if (!accDoc.HasNoLogStatus)
                addrDoc.LogState = accDoc.LogState;
            //addrDoc.AddressType.Value = CRMSelections.Link_CompAddr_Billing;
            addrDoc.primaryaddress.Value = "True";
            addrDoc.SetTransactionStatus(TransactionStatus.Success);


            if (row.IsAddressNull())
            {
                addrDoc.address1.Value = null;
                addrDoc.address2.Value = null;
                addrDoc.address3.Value = null;
                addrDoc.address4.Value = null;
            }
            else
            {
                address = new Address();
                address.NorthwindAddress = row.Address;
                addrDoc.address1.Value = address.CrmAddressLine1;
                addrDoc.address2.Value = address.CrmAddressLine2;
                addrDoc.address3.Value = address.CrmAddressLine3;
                addrDoc.address4.Value = address.CrmAddressLine4;
            }

            addrDoc.City.Value = row.IsCityNull() ? null : row.City;

            addrDoc.state.Value = row.IsRegionNull() ? null : row.Region;
            if (row.IsCountryNull())
                addrDoc.country.Value = null;
            else
                addrDoc.country.Value = countryCodes.GetCountryCode(row.Country);

            addrDoc.postcode.Value = row.IsPostalCodeNull() ? null : row.PostalCode;

            if (accDoc.Id.StartsWith(Constants.CustomerIdPrefix))
                accDoc.emails = GetEmailsCollectionFromCustomer(accDoc.Id.Substring(Constants.CustomerIdPrefix.Length), lastToken, config);



            accDoc.addresses.Add(addrDoc);

            return accDoc;


        }
        
        
        //private string GetAccountType(string id)
        //{
        //    if (id.StartsWith(Constants.CustomerIdPrefix))
        //        return CRMSelections.acc_type_Customer;

        //    if (id.StartsWith(Constants.SupplierIdPrefix))
        //        return CRMSelections.acc_type_Supplier;

        //    return "";

        //}
        #endregion

        #region execute transaction

        /// <summary>
        /// get a northwind account row, which is a union of customers and suppliers,
        /// from the given account document and set the transaction status on the documents
        /// </summary>
        /// <param name="accDoc">the crm Account document</param>
        /// <param name="config"></param>
        /// <returns>a filled northwind account row</returns>
        private AccountDataset.AccountsRow GetRow(AccountDocument accDoc, NorthwindConfig config, bool newAccount)
        {
            #region Declarations
            AccountDataset accDataset;
            AccountDataset.AccountsRow result;
            Address address;
            Phone phone;
            ContactName personName;
            int subentities = 4;
            #endregion

            accDataset = new AccountDataset();
            result = accDataset.Accounts.NewAccountsRow();

            // get the account name from the document
            if (!accDoc.name.IsNull)
                result.CompanyName =  (string)accDoc.name.Value;

            if (newAccount)
            {
                // set create user and id
                result.CreateID = config.SequenceNumber;
                result.CreateUser = config.CrmUser;
            }

            // set modify user and id
            result.ModifyID = config.SequenceNumber;
            result.ModifyUser = config.CrmUser;

            #region Address
            
            // go throuh all addresses to find the business address, 
            // the rest of the adresses will ignored
            foreach (AddressDocument adrDoc in accDoc.addresses)
            {
                // check if the Address is from the supported type
                if ((adrDoc.primaryaddress.IsNull) ||
                    (!adrDoc.primaryaddress.Value.ToString().Equals("True", StringComparison.InvariantCultureIgnoreCase)))
                {
                    // set the transactionsstatus to none to remove this status from the result
                    adrDoc.ClearTransactionStatus();
                    continue;
                }

                // the first correct address found

                // get a new Address Object to convert beween the systems
                address = new Address();

                // fill the address object with the crm data
                address.SetCrmAdresses(adrDoc.address1, adrDoc.address2, adrDoc.address3, adrDoc.address4);

                // set the Northwind address
                result.Address = address.NorthwindAddress;


                // get the city from the Address document
                if (!adrDoc.City.IsNull)
                    result.City = (string)adrDoc.City.Value;

                // get the state from the Address document
                if (!adrDoc.state.IsNull)
                    result.Region = (string)adrDoc.state.Value;

                // get the state from the Address document
                if (!adrDoc.postcode.IsNull)
                    result.PostalCode = (string)adrDoc.postcode.Value;


                // get the country from the Address document
                if (!adrDoc.country.IsNull)
                    result.Country = (string)adrDoc.country.Value;
                
                // stop searching
                subentities--;
                adrDoc.SetTransactionStatus(TransactionStatus.Success);
                break;

            }
            #endregion

            #region Contact
            // go throuh all people to find the billing person, 
            // the rest of the people will ignored
            foreach (PersonDocument persDoc in accDoc.people)
            {
                // check if the person is from the supported type
                if ((persDoc.primaryperson.IsNull) || (!persDoc.primaryperson.Value.ToString().Equals("True",StringComparison.InvariantCultureIgnoreCase)))
                {
                    // set the transactionsstatus to none to remove this status from the result
                    persDoc.ClearTransactionStatus();
                    continue;
                }
                // the first correct people found

                // get the Title from the Person document
                if (!persDoc.title.IsNull)
                    result.ContactTitle = (string)persDoc.title.Value;

                // get a new ContactName Object to convert beween the systems
                personName = new ContactName();

                // fill the ContactName object with the crm data
                personName.SetCrmContact(persDoc.salutation,
                    persDoc.firstname,
                    persDoc.middlename,
                    persDoc.lastname,
                    persDoc.suffix);

                // set the Northwind ContactName
                result.ContactName = personName.NorthwindContacName;

                // stop searching
                subentities--;
                persDoc.SetTransactionStatus(TransactionStatus.Success);
                break; 

            }
            #endregion


            #region Phones
            // go throuh all phones to find phone and fax, 
            // the rest of the phones  will ignored

            foreach (PhoneDocument phoneDoc in accDoc.phones)
            {
                // check if the phone is from the supported type
                if ((phoneDoc.type.IsNull) ||
                    !((phoneDoc.type.Value.ToString() == CRMSelections.Link_PersPhon_Business)))
                {
                    // set the transactionsstatus to none to remove this status from the result
                    if(!((phoneDoc.type.Value.ToString() == CRMSelections.Link_PersPhon_Fax)))
                    phoneDoc.ClearTransactionStatus();
                    continue;
                }

                // get a new phone Object to convert beween the systems
                phone = new Phone();

                // fill the ContactName object with the crm data
                phone.SetCrmPhone(phoneDoc.countrycode, phoneDoc.areacode, phoneDoc.number);


                // set the northwind phone
                result.Phone = phone.NorthwindPhone;

                // on new pone entries store the phonetype postfix in the id 
                // to fill it up with the Account id later
                if ((phoneDoc.Id == null) || (phoneDoc.Id == ""))
                    phoneDoc.Id = Constants.PhoneIdPostfix;
                subentities--;
                phoneDoc.SetTransactionStatus(TransactionStatus.Success);
                break;
            }


            foreach (PhoneDocument phoneDoc in accDoc.phones)
            {
                // check if the phone is from the supported type
                if ((phoneDoc.type.IsNull) ||
                    !((phoneDoc.type.Value.ToString() == CRMSelections.Link_PersPhon_Fax)))
                {
                    // set the transactionsstatus to none to remove this status from the result
                    if (!((phoneDoc.type.Value.ToString() == CRMSelections.Link_PersPhon_Business)))
                    phoneDoc.ClearTransactionStatus();
                    continue;
                }

                // get a new phone Object to convert beween the systems
                phone = new Phone();

                // fill the ContactName object with the crm data
                phone.SetCrmPhone(phoneDoc.countrycode, phoneDoc.areacode, phoneDoc.number);


                // set the northwind fax
                result.Fax = phone.NorthwindPhone;

                // on new pone entries store the phonetype postfix in the id 
                // to fill it up with the Account id later
                if ((phoneDoc.Id == null) || (phoneDoc.Id == ""))
                    phoneDoc.Id = Constants.FaxIdPostfix;

                subentities--;
                phoneDoc.SetTransactionStatus(TransactionStatus.Success);
                break;
            }
            #endregion

            if (newAccount && (subentities > 0))
            {
                result.CreateUser = "Admin";
                result.ModifyUser = "Admin";
            }
            
            //return the row 
            return result;

        }

        /// <summary>
        /// ste the Identies of the account sub entities address, phone and person
        /// </summary>
        /// <param name="accountID">the new account ID</param>
        /// <param name="doc"></param>
        private void SetIdentitiesForAccounts(string accountID, AccountDocument account)
        {
            if ((account.Id == null) || (account.Id == string.Empty))
                account.Id = accountID;

            foreach (AddressDocument addr in account.addresses)
                if ((!addr.HasNoLogStatus ) &&
                ((addr.Id == null) || (addr.Id == string.Empty)))
                    addr.Id = accountID;

            foreach (PersonDocument pers in account.people)
                if ((!pers.HasNoLogStatus) &&
                 ((pers.Id == null) || (pers.Id == string.Empty)))
                    pers.Id = accountID;

            foreach (PhoneDocument phone in account.phones)
                if ((!phone.HasNoLogStatus) &&
                ((phone.Id == Constants.PhoneIdPostfix) || (phone.Id == Constants.FaxIdPostfix)))
                    phone.Id = accountID + phone.Id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accDoc"></param>
        /// <param name="accountRow"></param>
        /// <param name="config"></param>
        private void StoreCustomer(AccountDocument accDoc, AccountDataset.AccountsRow accountRow, NorthwindConfig config, ref List<TransactionResult> result)
        {
            #region declaration
            AccountDataset.CustomersRow customerRow;
            AccountDataset account;
            CustomersTableAdapter tableAdapter;
            string columnName;
            int recordCount;
            bool newCustomer;
            string customerId;
            #endregion

            newCustomer = ((accDoc.Id == null) || (accDoc.Id == ""));

            try
            {
                if (newCustomer)
                    customerId = GetNewCustomerID((string)accDoc.name.Value, config);
                else if (accDoc.Id.StartsWith(Constants.CustomerIdPrefix, StringComparison.InvariantCultureIgnoreCase))
                    customerId = accDoc.Id.Substring(Constants.CustomerIdPrefix.Length);
                else
                {
                    result.Add(accDoc.SetTransactionStatus(TransactionStatus.UnRecoverableError, Resources.ErrorMessages_AccountNotFound));
                    return;
                }
            }
            catch (Exception)
            {
                result.Add(accDoc.SetTransactionStatus(TransactionStatus.UnRecoverableError, Resources.ErrorMessages_AccountNotFound));
                return;
            }
            try
            {

                using (OleDbConnection connection = new OleDbConnection(config.ConnectionString))
                {
                    account = new AccountDataset();

                    tableAdapter = new CustomersTableAdapter();
                    tableAdapter.Connection = connection;
                    if (newCustomer)
                    {
                        customerRow = account.Customers.NewCustomersRow();
                        customerRow.CustomerID = customerId;
                    }
                    else
                    {
                        recordCount = tableAdapter.FillBy(account.Customers, customerId);
                        if (recordCount == 0)
                        {
                            accDoc.SetTransactionStatus(TransactionStatus.UnRecoverableError, Resources.ErrorMessages_AccountNotFound);
                            return;
                        }
                        customerRow = (AccountDataset.CustomersRow)account.Customers.Rows[0];

                    }


                    for (int index = 0; index < accountRow.Table.Columns.Count; index++)
                    {

                        columnName = accountRow.Table.Columns[index].ColumnName;

                        if (!customerRow.Table.Columns.Contains(columnName))
                            continue;

                        if (columnName.StartsWith("Create", StringComparison.InvariantCultureIgnoreCase))
                            if ((accountRow[columnName].GetType().Equals(typeof(DBNull))))
                                continue;

                        customerRow[columnName] = accountRow[columnName];
                    }

                    if (newCustomer)
                        account.Customers.AddCustomersRow(customerRow);


                    tableAdapter.Update(account.Customers);
                    accDoc.SetTransactionStatus(TransactionStatus.Success);

                    SetIdentitiesForAccounts(Constants.CustomerIdPrefix + customerId, accDoc);
                    accDoc.GetTransactionResult(ref result);
                }
            }
            catch (Exception addCustomerException)
            {
                result.Add(accDoc.SetTransactionStatus(TransactionStatus.FatalError, addCustomerException.ToString()));
                throw;
            }

            UpdateEmailsCollectionFromCustomer(customerId, accDoc.emails, config, ref result);
        }

        private void StoreSupplier(AccountDocument accDoc, AccountDataset.AccountsRow accountRow, NorthwindConfig config, ref List<TransactionResult> result)
        {
            AccountDataset.SuppliersRow suppliersRow;
            AccountDataset account;
            SuppliersTableAdapter tableAdapter;
            string columnName;
            int recordCount;
            bool newSupplier = ((accDoc.Id == null) || (accDoc.Id == ""));

            int supplierId;

            if (newSupplier)
                supplierId = 0;
            else if (accDoc.Id.StartsWith(Constants.SupplierIdPrefix, StringComparison.InvariantCultureIgnoreCase))
                try
                {
                    supplierId = Convert.ToInt32(accDoc.Id.Substring(Constants.SupplierIdPrefix.Length));
                }
                catch (Exception)
                { supplierId = 0; }
            else
            {
                accDoc.SetTransactionStatus(TransactionStatus.UnRecoverableError, Resources.ErrorMessages_AccountNotFound);
                return;
            }

            try
            {
                using (OleDbConnection connection = new OleDbConnection(config.ConnectionString))
                {
 
                        connection.Open();
                        account = new AccountDataset();

                        tableAdapter = new SuppliersTableAdapter();
                        tableAdapter.Connection = connection;
                        if (newSupplier)
                            suppliersRow = account.Suppliers.NewSuppliersRow();
                        else
                        {
                            recordCount = tableAdapter.FillBy(account.Suppliers, supplierId);
                            if (recordCount == 0)
                            {
                                accDoc.SetTransactionStatus(TransactionStatus.UnRecoverableError, Resources.ErrorMessages_AccountNotFound);
                                return;
                            }
                            suppliersRow = (AccountDataset.SuppliersRow)account.Suppliers.Rows[0];

                        }


                        for (int index = 0; index < accountRow.Table.Columns.Count; index++)
                        {

                            columnName = accountRow.Table.Columns[index].ColumnName;

                            if (!suppliersRow.Table.Columns.Contains(columnName))
                                continue;

                            suppliersRow[columnName] = accountRow[columnName];
                        }

                        if (newSupplier)
                            account.Suppliers.AddSuppliersRow(suppliersRow);


                        tableAdapter.Update(account.Suppliers);

                        if (newSupplier)
                        {
                            OleDbCommand Cmd = new OleDbCommand("SELECT @@IDENTITY", connection);

                            object lastid = Cmd.ExecuteScalar();
                            supplierId = (int)lastid;
                        }

                        accDoc.SetTransactionStatus(TransactionStatus.Success);

                        SetIdentitiesForAccounts(Constants.SupplierIdPrefix + supplierId.ToString(), accDoc);
                        accDoc.GetTransactionResult(ref result);

                }
            }
            catch (Exception addCustomerException)
            {
                accDoc.SetTransactionStatus(TransactionStatus.FatalError, addCustomerException.ToString());
                throw;
            }


        }

        private string GetNewCustomerID(string customerName, NorthwindConfig config)
        {
            string customerId = "";
            if ((customerName != null) && (customerName.Length > 0))
                customerId = customerName.Substring(0, 1);

            customerId += config.Random.Next(1000, 9999).ToString();

            return customerId.Substring(0, Math.Min(5, customerId.Length));
        }

        #endregion

        #region emails
        private void UpdateEmailsCollectionFromCustomer(string customerId, EmailsDocumentCollection emailCollection, NorthwindConfig config, ref List<TransactionResult> result)
        {
            #region declarations
            int recordCount;
            Emails emails = new Emails();
            CustomerEmailsTableAdapter tableAdapter;
            DeleteHistoryTableAdapter history = new DeleteHistoryTableAdapter();
            Emails.CustomerEmailsRow row;
            OleDbCommand Cmd;
            object lastid;
            int newEmailID;
            #endregion

            using (OleDbConnection connection = new OleDbConnection(config.ConnectionString))
            {
                connection.Open();
                tableAdapter = new CustomerEmailsTableAdapter();
                tableAdapter.Connection = connection;
                recordCount = tableAdapter.FillByCustomerId(emails.CustomerEmails, customerId);
                foreach (EmailDocument emailDocument in emailCollection)
                {
                    if (emailDocument.HasNoLogStatus)
                        continue;

                    #region process deleted
                    if (emailDocument.LogState == LogState.Deleted)
                    {
                        try
                        {
                            if (tableAdapter.Delete(Identity.GetId(emailDocument.Id)) > 0)
                            {
                                history.Connection = connection;
                                history.LogCustomerEmails(emailDocument.Id, config.SequenceNumber, config.CrmUser, customerId);
                            }
                            result.Add(emailDocument.SetTransactionStatus(TransactionStatus.Success));
                        }
                        catch (Exception deleteException)
                        {
#warning todo: check this
                            result.Add(emailDocument.SetTransactionStatus(TransactionStatus.UnRecoverableError, deleteException.Message));

                        }
                        continue;
                    }
                    #endregion

                    if (emailDocument.emailaddress.IsNull)
                    {
                        emailDocument.ClearTransactionStatus();
                        continue;
                    }

                    #region process created
                    if (emailDocument.LogState == LogState.Created)
                    {
                        if ((emailDocument.Id != null) && (emailDocument.Id.Length > 0))
                            continue;


                        row = emails.CustomerEmails.NewCustomerEmailsRow();
                        row.CustomerID = customerId;
                        row.Email = (string)emailDocument.emailaddress.Value;

                        row.CreateID = config.SequenceNumber;
                        row.CreateUser = config.CrmUser;

                        row.ModifyID = config.SequenceNumber;
                        row.ModifyUser = config.CrmUser;

                        emails.CustomerEmails.AddCustomerEmailsRow(row);
                        try
                        {
                            tableAdapter.Update(emails.CustomerEmails);

                            Cmd = new OleDbCommand("SELECT @@IDENTITY", connection);

                            lastid = Cmd.ExecuteScalar();
                            newEmailID = (int)lastid;
                            emailDocument.Id = newEmailID.ToString();
                            result.Add(emailDocument.SetTransactionStatus(TransactionStatus.Success));
                        }
                        catch (Exception addException)
                        {
#warning todo: check this
                            result.Add(emailDocument.SetTransactionStatus(TransactionStatus.UnRecoverableError, addException.Message));

                        }

                        continue;
                    }
                    #endregion 

                    #region process changed
                    if (emailDocument.LogState == LogState.Updated)
                    {

                        row = emails.CustomerEmails.FindByID(Identity.GetId(emailDocument.Id));
                        if (row == null)
                        {
                            result.Add(emailDocument.SetTransactionStatus(TransactionStatus.UnRecoverableError, Resources.ErrorMessages_EmailNotFound));
                            continue;
                        }
                        row.Email = (string)emailDocument.emailaddress.Value;
                        row.ModifyID = config.SequenceNumber;
                        row.ModifyUser = config.CrmUser;

                        try
                        {
                            tableAdapter.Update(emails.CustomerEmails);
                            result.Add(emailDocument.SetTransactionStatus(TransactionStatus.Success));
                        }
                        catch (Exception updateException)
                        {
#warning todo: check this
                            result.Add(emailDocument.SetTransactionStatus(TransactionStatus.UnRecoverableError, updateException.Message));
                        }

                        continue;
                    }
                    #endregion

                    emailDocument.ClearTransactionStatus();
                }

            }

        }


        private EmailsDocumentCollection GetEmailsCollectionFromCustomer(string customerId, Token lastToken, NorthwindConfig config)
        {
            int recordCount;
            Emails emails = new Emails();
            DeleteHistoryDataset history = new DeleteHistoryDataset();

            using (OleDbConnection connection = new OleDbConnection(config.ConnectionString))
            {
                Sage.Integration.Northwind.Application.Entities.Account.DataSets.EmailsTableAdapters.CustomerEmailsTableAdapter tableAdapter;
                tableAdapter = new Sage.Integration.Northwind.Application.Entities.Account.DataSets.EmailsTableAdapters.CustomerEmailsTableAdapter();
                tableAdapter.Connection = connection;
                recordCount = tableAdapter.FillByCustomerId(emails.CustomerEmails, customerId);

                DeleteHistoryTableAdapter tableAdapterDeleted;
                tableAdapterDeleted = new DeleteHistoryTableAdapter();
                tableAdapterDeleted.Connection = connection;
                recordCount += tableAdapterDeleted.FillCustomerEmailsBy(history.DeleteHistory, customerId.ToString());
            }

            if (recordCount == 0)
                return new EmailsDocumentCollection();

            return GetEmailsCollectionFromCustomer(emails, history, lastToken, config.CrmUser);

        }

        private EmailsDocumentCollection GetEmailsCollectionFromCustomer(Emails emails, DeleteHistoryDataset history, Token lastToken, string crmUser)
        {
            EmailsDocumentCollection emailCollection = new EmailsDocumentCollection();
            EmailDocument email;

            foreach (DeleteHistoryDataset.DeleteHistoryRow dr in history.DeleteHistory)
            {
                if (dr.DeleteUser == crmUser)
                    continue;
                if (dr.DeleteID <= lastToken.SequenceNumber)
                    continue;
                email = new EmailDocument();
                email.Id = dr.Identity;
                email.LogState = LogState.Deleted;
                emailCollection.Add(email);

            }

            int index = 0;
            foreach (Emails.CustomerEmailsRow er in emails.CustomerEmails)
            {
                email = new EmailDocument();
                if ((er.CreateUser != crmUser) && (er.CreateID > lastToken.SequenceNumber))
                    email.LogState = LogState.Created;
                else if ((er.ModifyUser != crmUser) && (er.ModifyID > lastToken.SequenceNumber))
                    email.LogState = LogState.Updated;

                email.Id = er.ID.ToString();
                try
                {

                    email.emailaddress.Value = er.Email;
                    
                    if (index == 0)
                        email.type.Value = "Business";
                    if (index == 1)
                        email.type.Value = "Private";
                    index++;
                }
                catch (StrongTypingException)
                {
                    email.emailaddress.Value = null;
                }


                emailCollection.Add(email);

            }
            return emailCollection;
        }
        #endregion

        #endregion

        public override List<Identity> GetAll(NorthwindConfig config, string whereExpression, OleDbParameter[] oleDbParameters)
        {
            #region Declarations
            List<Identity> result = new List<Identity>();
            int recordCount = 0;
            AccountDataset dataset = new AccountDataset();
            #endregion

            // get the first 11 rows of the changelog
            using (OleDbConnection connection = new OleDbConnection(config.ConnectionString))
            {
                AccountsTableAdapter tableAdapter;

                tableAdapter = new AccountsTableAdapter();

                tableAdapter.Connection = connection;
              
                if (string.IsNullOrEmpty(whereExpression))
                    recordCount = tableAdapter.Fill(dataset.Accounts);
                else
                    recordCount = tableAdapter.FillByWhereClause(dataset.Accounts, whereExpression, oleDbParameters);
            }

            foreach (AccountDataset.AccountsRow row in dataset.Accounts.Rows)
            {
                // use where expression !!
                result.Add(new Identity(this.EntityName, row.ID));
            }

            return result;

        }

        public override List<Identity> GetAll(NorthwindConfig config, string whereExpression, int startIndex, int count)
        {
            throw new NotImplementedException();
        }
    }
}
