#region Usings

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using Sage.Integration.Northwind.Application.API;
using Sage.Integration.Northwind.Application.Base;
using Sage.Integration.Northwind.Application.Entities.Product;
using Sage.Integration.Northwind.Application.Properties;

#endregion

namespace Sage.Integration.Northwind.Application
{

    /// <summary>
    /// Name of         CRUD status
    /// Linked Entity	in CRM 	    Create	        Update	        Delete
    /// Account	        Read Only	Bidirectional	ERP to CRM	    Bidirectional
    /// Person	        Updateable	Bidirectional	Bidirectional	Bidirectional
    /// Address	        Updateable	Bidirectional	bidirectional	Bidirectional
    /// Phone	        Updateable	Bidirectional	bidirectional	Bidirectional
    /// Email	        Updateable	Bidirectional	bidirectional	Bidirectional
    /// Order	        Read Only	Bidirectional	ERP to CRM	    no delete
    /// Product 
    /// (& Family)	    Read Only	ERP to CRM	    ERP to CRM	    ERP to CRM
    /// Pricing List 
    /// (& Family)	    Read Only	ERP to CRM	    ERP to CRM	    ERP to CRM
    /// Units of Measurement 
    /// (& Family)	    Read Only	ERP to CRM	    ERP to CRM	    ERP to CRM

    /// </summary>
    public class NorthwindConnector : INorthwindConnector 
    {

        private NorthwindConfig _config;
        /// <summary>
        /// The Constructor.
        /// </summary>
        public NorthwindConnector()
        {

            _config = new NorthwindConfig("-");

        }

        /// <summary>
        /// OpenSession is called at the start of every call to the ERP system.  
        /// If OpenSession returns a token, 
        /// then it is passed in the header for subsequent calls.  
        /// </summary>
        /// <returns></returns>
        public SessionOpenResult OpenSession(NorthwindConfig config)
        {
            SessionOpenResult result = new SessionOpenResult();
            DateTime now = DateTime.Now;
            string sessionExtention = "." +
                now.Year.ToString() +
                now.Month.ToString() +
               now.Day.ToString() +
                "." + now.Hour.ToString() +
               "." + now.Minute.ToString() +
            "." + now.Second.ToString() +
            "." + now.Millisecond.ToString();

            result.SessionKey = config.LastSequenceNumber.ToString() + sessionExtention;
            return result;
        }

        /// <summary>
        /// CloseSession is called at the end of every session, 
        /// regardless of the authentication type supported.
        /// </summary>
        /// <param name="sessionID"></param>
        /// <returns></returns>
        public void CloseSession(string SessionKey)
        {
            //string id = "1";
            ;
        }

        /// <summary>
        /// There are three authentication types supported by CRM.  
        /// When CRM connects to an ERP system for the first time it calls GetAuthenticationScheme 
        /// to find out what authentication type is supported by the ERP system.   
        /// Subsequent calls will use the supported authentication type.
        /// </summary>
        /// <returns></returns>
        public AuthenticationResult GetAuthenticationScheme(NorthwindConfig config)
        {
            AuthenticationResult result = new AuthenticationResult();
            result.AuthenticationOnEveryCall = true;
            result.PasswordHandling = PasswordType.ClearText;
            return result;
        }


        /// <summary>
        /// GetConfiguration is called after GetAuthentication when a new integration is created.  
        /// It is also called when an integration is enabled.  
        /// GetConfiguration is a request to the ERP system for configuration information.  
        /// This will be used to populate the configuration area of the integration in CRM.
        /// </summary>
        /// <returns>The ERP system responds with a GetConfigurationResponse.  
        /// GetConfigurationResponse contains a Configuration structure. 
        /// The contents of a Configuration structure are:
        /// •	Version – The version of the contract supported.  
        ///     The version supported in the 6.0.1 release is version 1.0.
        /// •	Product Name – The name of the ERP system, eg MAS200, Line 50.
        /// •	ERP Schema – This is a complex type used to feed back 
        ///     the xsd for the webservices interface.  
        ///     The xsd must conform to the specification defined by sage CRM.
        /// </returns>
        public Configuration GetConfiguration(NorthwindConfig nwConfig)
        {
            string schemastring;
            List<Document> documents = EntityFactory.GetSupportedDocumentTemplates();
            Configuration config = new Configuration();

            config.ProductName = "Northwind";
            config.SchemaVersion = nwConfig.Version;

            XmlDocument xmlDoc = new XmlDocument();
            XmlSchema schema = new XmlSchema();
            schema.ElementFormDefault = XmlSchemaForm.Qualified;
            schema.TargetNamespace = "http://schemas.sage.com/sis/Northwind/2007/05/Synch";
            //schema.Namespaces.Add("xs", XmlSchema.Namespace);
            schema.Namespaces.Add("tns", schema.TargetNamespace);

            foreach (Document doc in documents)
                schema.Items.Add(doc.GetSchemaElement());

            schemastring = XmlSerializationHelpers.SerializeObjectToXml(schema);
            xmlDoc.LoadXml(schemastring);

            config.SyncSchema= xmlDoc.DocumentElement;


            #region RTDV

            // create and initialize rtdv schema
            schema = new XmlSchema();
            schema.Id = "RTDV";
            schema.TargetNamespace = "http://schemas.sage.com/sis/Northwind/2007/05/RTDV";
            schema.ElementFormDefault = XmlSchemaForm.Qualified;

            schema.Namespaces.Add("mstns", schema.TargetNamespace);
            
            // append elements to schema
            RTDVBase[] rtdvArray = RTDVFactory.GetRTDVAll();        // get instances of all RTDVs
            
            foreach (RTDVBase rtdv in rtdvArray)                    // iterate through all RTDVs and get their schema element
                schema.Items.Add(rtdv.GetXmlSchemaElement());              // append schema element to rtdv schema

            xmlDoc = new XmlDocument();
            schemastring = XmlSerializationHelpers.SerializeObjectToXml(schema);
            
            xmlDoc.LoadXml(schemastring);

            config.RTDSchema = xmlDoc.DocumentElement;                              // set rtdv schema to config

            #endregion

            return config;
        }

        /// <summary>
        /// GetCustomisations is called after GetConfiguration, 
        /// either when a new integration is being created or when an integration is enabled.  
        /// It is a request to the ERP system for customisation information.
        /// </summary>
        /// <returns>The ERP system responds to a GetCustomisations request, 
        /// with a GetCustomisationsReponse, 
        /// which contains a list of ERPCustomisations changes</returns>
        public ERPCustomisations GetCustomisations(NorthwindConfig nwConfig)
        {
            ERPCustomisations result;
            result =  new ERPCustomisations();

            result.Version = nwConfig.CustomisationVersion;

            XmlDocument resultDoc = new XmlDocument();

            resultDoc.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8"" ?><getcustomisationsresponse/>");

            AddCustomisations(ref resultDoc, "AdditionalStockDetails");
            AddCustomisations(ref resultDoc, "ProductsPurchased");
            AddCustomisations(ref resultDoc, "SalesInvoices");
            AddCustomisations(ref resultDoc, "AccountTab");
            AddCustomisations(ref resultDoc, "ErpSelections");

            AddSelesReps(ref resultDoc, nwConfig);

            result.CustomisationData = resultDoc.DocumentElement;
            return result;
        }

        private void AddSelesReps(ref XmlDocument resultDoc, NorthwindConfig nwConfig)
        {
            try
            {
                // declarations
                string sqlQuery;
                OleDbDataAdapter dataAdapter;
                int nOfRows;
                DataSet dataSet;
                XmlElement addcustomcaptions;
                XmlElement subElem;
                string name;
                string id;
                int maxSalesCode = 0;

                // initializations
                dataSet = null;
                nOfRows = 0;
                dataSet = new DataSet();
                sqlQuery = @"SELECT Employees.EmployeeID, Employees.FirstName + ' ' +  Employees.LastName as Name FROM Employees";

                // get the data base records using a table adapter.
                using (OleDbConnection connection = new OleDbConnection(nwConfig.ConnectionString))
                {
                    connection.Open();
                    dataAdapter = new OleDbDataAdapter(sqlQuery, connection);
                    dataAdapter.Fill(dataSet, "Employees");

                    OleDbCommand Cmd = new OleDbCommand("SELECT Max(Employees.EmployeeID) AS MaxOfID FROM Employees", connection);

                    object lastid = Cmd.ExecuteScalar();
                    maxSalesCode = (int)lastid;
                }
                for (int index = 1; index <= maxSalesCode; index++)
                {
                    addcustomcaptions = resultDoc.CreateElement("deletecustomcaption");
                    subElem = resultDoc.CreateElement("familytype");
                    subElem.InnerText = "Choices";
                    addcustomcaptions.AppendChild(subElem);
                    subElem = resultDoc.CreateElement("family");
                    subElem.InnerText = "salesrepresentative";
                    addcustomcaptions.AppendChild(subElem);
                    subElem = resultDoc.CreateElement("code");
                    subElem.InnerText = index.ToString();
                    addcustomcaptions.AppendChild(subElem);
                    resultDoc.DocumentElement.AppendChild(addcustomcaptions);

                }

                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    if (row.IsNull(0) || row.IsNull(1))
                        continue;

                    id = row[0].ToString();
                    name = row[1].ToString();

                    addcustomcaptions = resultDoc.CreateElement("addcustomcaptions");
                    subElem = resultDoc.CreateElement("familytype");
                    subElem.InnerText = "Choices";
                    addcustomcaptions.AppendChild(subElem);

                    subElem = resultDoc.CreateElement("family");
                    subElem.InnerText = "salesrepresentative";
                    addcustomcaptions.AppendChild(subElem);

                    subElem = resultDoc.CreateElement("code");
                    subElem.InnerText = id;
                    addcustomcaptions.AppendChild(subElem);

                    subElem = resultDoc.CreateElement("order");
                    subElem.InnerText = id;
                    addcustomcaptions.AppendChild(subElem);

                    subElem = resultDoc.CreateElement("de");
                    subElem.InnerText = name;
                    addcustomcaptions.AppendChild(subElem);

                    subElem = resultDoc.CreateElement("es");
                    subElem.InnerText = name;
                    addcustomcaptions.AppendChild(subElem);

                    subElem = resultDoc.CreateElement("fr");
                    subElem.InnerText = name;
                    addcustomcaptions.AppendChild(subElem);

                    subElem = resultDoc.CreateElement("uk");
                    subElem.InnerText = name;
                    addcustomcaptions.AppendChild(subElem);

                    subElem = resultDoc.CreateElement("us");
                    subElem.InnerText = name;
                    addcustomcaptions.AppendChild(subElem);

                    resultDoc.DocumentElement.AppendChild(addcustomcaptions);
                }
                
            }
            catch (Exception) { }
        }

        private void AddCustomisations(ref XmlDocument resultDoc, string name)
        {
            XmlDocument partDoc = new XmlDocument();
            Stream s;
            name = "Sage.Integration.Northwind.Application.CRMCustomisation." + name + ".xml";


            using (s = this.GetType().Assembly.GetManifestResourceStream(name))
            {
                partDoc.Load(s);
            }
            foreach (XmlNode child in partDoc.DocumentElement.ChildNodes)
                resultDoc.DocumentElement.AppendChild(resultDoc.ImportNode(child, true));

        }

        /// <summary>
        ///•	Account
        /// •	Order
        /// •	Product
        /// •	ProductFamily
        /// •	Price
        /// •	UnitOfMesure
        /// •	UnitOfMeasureFamily
        /// </summary>
        /// <param name="EntityName">The name of the entity</param>
        /// <param name="Token"></param>
        /// <param name="config">the configuration object</param>
        /// <returns></returns>
        public ChangeLog GetChangeLog(string entityName, string token, NorthwindConfig config)
        {
            Token intToken;

            // create an new token for the requested entity
            intToken = new Token(new Identity(entityName, ""), 0, true);

            // if an serialzed tokenstring passed in, deserialize and use this one
            if (!((token == null) || (token.Length == 0)))
                //token = (Token)XmlHelper.DeserializeXmlToObject(token, Token);
                intToken = (Token)Token.DeserializeToken(token);

            // get a new entity object of the requestet entity
            EntityBase entity = EntityFactory.GetEntity(entityName);

            // if the entityname is not supported by the connector an error will thrown
            if (entity == null)
                throw new Exception(string.Format(Resources.ErrorMessages_OperationNotImplementedForEntity, entityName));

            // get the changelog an an entity
            return entity.GetChangelog(intToken, config);

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="OrderDetails"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public PricingDetail GetPricingDetails(OrderDetailInformation OrderDetails, NorthwindConfig config)
        {
            PricingDetail result; 
            Product product = new Product();
            result = product.GetPricingDetails(OrderDetails, config);
            return result;
        }


        /// <summary>
        /// of following entities:
        /// •	Account
        /// •	Order
        /// </summary>
        /// <param name="EntityName"></param>
        /// <param name="TransactionData"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public TransactionResult[] ExecuteTransactions(string EntityName, Transaction[] TransactionData, NorthwindConfig config)
        {
            int seqenceID;
            try
            {

                EntityBase entity = EntityFactory.GetEntity(EntityName);

                if (entity == null)
                    throw new Exception(string.Format(Resources.ErrorMessages_OperationNotImplementedForEntity, EntityName));

                seqenceID = config.SequenceNumber;
                return entity.ExecuteTransactions(TransactionData, config);
            }

            catch (Exception e)
            {

                TransactionResult[] results = new TransactionResult[1];

                results[0] = new TransactionResult();
                results[0].Status = TransactionStatus.FatalError;
                results[0].EntityName = EntityName;
                results[0].Message = e.ToString();

                return results;

            }

        }

        /// <summary>
        /// ViewRealTimeData is a request to the ERP system for real time viewing data from the ERP system.
        /// </summary>
        /// <param name="queryFields">An array of query fields that represent the parameters needed for the request.</param>
        /// <param name="config">the configuration object</param>
        /// <returns>The response to ViewRealTimeData is a XmlDocument, which contains a list of real time data.</returns>
        public ViewRealTimeDataResult ViewRealTimeData(string entityName, string[] selectFields, SearchField[] searchFields, string[] orderFields, int rowsPerPage, int pageNumber, NorthwindConfig northwindConfig)
        {
            // declarations
            RTDVBase rdtv;

            rdtv = RTDVFactory.GetRTDV(entityName);

            if (rdtv == null)
                throw new Exception(string.Format(Resources.ErrorMessages_RTDVNotImplemented, entityName));

            if (orderFields == null)
                orderFields = new string[0];

            if (searchFields == null)
                searchFields = new SearchField[0];

            if (selectFields == null)
                selectFields = new string[0];
            ViewRealTimeDataResult result = new ViewRealTimeDataResult();
            result.RealTimeData  = rdtv.ViewRealTimeData(entityName, selectFields, searchFields, orderFields, rowsPerPage, pageNumber, northwindConfig);
            return result;
        }

        public Pricing CheckPrice(Pricing PricingInformation, NorthwindConfig northwindConfig)
        {
            Pricing result;
            Product product = new Product();
            result = product.CheckPrice(PricingInformation, northwindConfig);
            return result;
        }










    }
}
