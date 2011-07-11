#region ©2007 Sage Technology Ltd., All Rights Reserved.
/*=====================================================================
	File:			Document.cs
	Author:			Philipp Schuette
	DateCreated:	03/16/2007 
	DateChanged:	03/16/2007
 ---------------------------------------------------------------------
	©2007 Sage Technology Ltd., All Rights Reserved.	
=====================================================================*/

/*=====================================================================
	Date			        Author		Changes			Reasons
	03/16/2007          	pschuette		Create			  
	Changes: Create, Refactoring, Upgrade	
=====================================================================*/
#endregion

#region Usings
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Collections;
using Sage.Integration.Northwind.Application.Toolkit;
using Sage.Integration.Northwind.Application.API;
using System.Xml.Schema;
using Sage.Integration.Northwind.Application.EntityResources;
using Sage.Integration.Northwind.Application;
using Sage.Integration.Northwind.Application.Properties;

#endregion

namespace Sage.Integration.Northwind.Application.Base
{
    public class Document
    {
        #region constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="documentName">the name of the Document</param>
        protected Document(string documentName)
        {
            this.documentName = documentName;
            //this.hasNoTransactionStatus = true;
            this.hasNoLogStatus = true;
            this.status = TransactionStatus.Success;
            this.message = "";
            this.properties = new Dictionary<string, Property>();
            this.collections = new Dictionary<string, DocumentCollection>();
        }
        
        #endregion

        #region fields
        private Dictionary<string, Property> properties;
        private Dictionary<string, DocumentCollection> collections;
        //private bool hasNoTransactionStatus;

        private bool hasNoLogStatus;


        private TransactionStatus status;
        private string message;
        private string typeProperty;
        private string id;
        private string crmId;
        private LogState logState;
        private string documentName;
        #endregion

        #region public properties
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        public string CrmId
        {
            get { return crmId; }
            set { crmId = value; }
        }

        public LogState LogState
        {
            get { return logState; }
            set
            {
                hasNoLogStatus = false; 
                logState = value; }
        }

        public bool HasNoLogStatus
        {
            get { return hasNoLogStatus; }
            set { hasNoLogStatus = value; }
        }

        //public bool HasNoTransactionStatus
        //{
        //    get { return hasNoTransactionStatus; }
        //    set { hasNoTransactionStatus = value; }
        //}

        public string DocumentName
        {
            get { return this.documentName; }
            set { this.documentName = value; }
        }

        #endregion

        #region protected members

        /// <summary>
        /// a dictonary of all document properties with an access thru the property name.
        /// </summary>
        protected Dictionary<string, Property> Properties
        {
            get { return this.properties; }
            set { this.properties = value; }
        }

        /// <summary>
        /// add a new property to the document. If the property already exists, it returns false
        /// </summary>
        /// <param name="propertyName">the Name of the Property</param>
        /// <param name="type">the typecode of the property value</param>
        /// <returns>true if suceeded</returns>
        protected bool AddProperty(string propertyName, TypeCode type)
        {
            if (properties.ContainsKey(propertyName))
                return false;

            this.Properties.Add(propertyName, new Property(propertyName, type, this));

            return true;
        }


        /// <summary>
        /// add a new property to the document. If the property already exists, it returns false
        /// </summary>
        /// <param name="propertyName">the Name of the Property</param>
        /// <param name="type">the typecode of the property value</param>
        /// <param name="maxLength">the max lentgh of the property value</param>
        /// <returns>true if suceeded</returns>
        protected bool AddProperty(string propertyName, TypeCode type, int maxLength)
        {
            if (properties.ContainsKey(propertyName))
                return false;

            this.Properties.Add(propertyName, new Property(propertyName, type, maxLength, this));

            return true;
        }


        /// <summary>
        /// a dictonary of all document collections with an acces thru the collection name.
        /// </summary>
        protected Dictionary<string, DocumentCollection> Collections
        {
            get { return this.collections; }
            set { this.collections = value; }
        }


        /// <summary>
        /// add a new collection to the document. If the collection already exists, it returns false
        /// </summary>
        /// <param name="collection">the collection to add</param>
        /// <returns>true if suceeded</returns>
        protected bool AddCollection(DocumentCollection collection)
        {
            if (collections.ContainsKey(collection.CollectionName))
                return false;

            this.collections.Add(collection.CollectionName, collection);

            return true;
        }

        /// <summary>
        /// If the document could identity by a type, 
        /// the name of the document type property will stored here
        /// </summary>
        protected string TypeProperty
        {
            get { return typeProperty; }
            set { typeProperty = value; }
        }

        #endregion

        #region public members


        //public abstract XmlAttribute[] GetUnhandledAttributes();

        /// <summary>
        /// merge a document into the current.
        /// </summary>
        /// <param name="sourceDocument">the source Document</param>
        public void Merge(Document sourceDocument)
        {
            #region Declarations
            Property sourceProperty;
            DocumentCollection sourceCollection;
            #endregion

            // set the CRM id from the source document
            this.crmId = sourceDocument.crmId;

            // set the log state from the source document
            this.logState = sourceDocument.logState;

            // go thru each property of the current document
            foreach (Property prop in Properties.Values)
            {
                // if the log state is deleted clear the property
                if (sourceDocument.LogState == LogState.Deleted)
                {
                    prop.Value = null;
                    continue;
                }

                // if the property not part of the source documet 
                // continue with the next property
                if (!sourceDocument.Properties.ContainsKey(prop.Name))
                    continue;

                // get the source property
                sourceProperty = sourceDocument.Properties[prop.Name];

                // continue if the property was not set. Not set is different from null
                if (sourceProperty.NotSet)
                    continue;
                
                // set the property
                prop.Value = sourceProperty.Value;
            }


            // go thru each document collection
            foreach (DocumentCollection coll in Collections.Values)
            {
                // continue if the document collection is not part of the source document 
                if (!sourceDocument.Collections.ContainsKey(coll.CollectionName))
                    continue;

                // get the source collection
                sourceCollection = sourceDocument.Collections[coll.CollectionName];

                // continue if the collection was not set. Not set is different from null
                if (sourceCollection.NotSet)
                    continue;

                //merge the collection
                coll.Merge(sourceCollection);
            }
        }



        /// <summary>
        /// get the type on an documemt. 
        /// If the document has no type propery or the property is null, it returns an emty string
        /// </summary>
        /// <returns>the document type value, if there is one</returns>
        public string GetTypePropertyValue()
        {
            if (TypeProperty == null)
                return "";
            if ((this.Properties.ContainsKey(TypeProperty)) && (!this.Properties[TypeProperty].IsNull))
                return this.Properties[TypeProperty].Value.ToString();
            return "";
        }

        
        /// <summary>
        /// returns true in no property or documentcoleection of the document was set by reading 
        /// external xml. If the xml does not contains a specific node, 
        /// this property is set for the corresponding document property
        /// </summary>
        public bool NotSet
        {
            get
            {
                foreach (Property prop in this.properties.Values)
                    if (!prop.NotSet)
                        return false;


                foreach (DocumentCollection coll in this.collections.Values)
                    if (!coll.NotSet)
                        return false;

                return true;
            }
        }

        public TransactionResult GetOperationNotImplementedTransactionResult()
        {
            TransactionResult transactionResult;
            transactionResult = GetTransactionResult();

            transactionResult.Status = TransactionStatus.UnRecoverableError;
            transactionResult.Message = Resources.ErrorMessages_NotImplemented;
            return transactionResult;
        }


        public TransactionResult GetSuccessTransactionResult()
        {
            TransactionResult transactionResult;
            transactionResult = GetTransactionResult();

            transactionResult.Status = TransactionStatus.Success;
            transactionResult.Message = "";
            return transactionResult;
        }

        public TransactionResult GetTransactionResult()
        {
            TransactionResult transactionResult;

            transactionResult = new TransactionResult();

            transactionResult.EntityName = this.DocumentName;
            transactionResult.Action = LogstateToTransactionAction(this.LogState);
            transactionResult.CRMID = this.CrmId;
            transactionResult.ID = this.Id;
            transactionResult.Status = this.status;
            transactionResult.Message = this.message;

            return transactionResult;
        }



        public void GetTransactionResult(ref List<TransactionResult> result)
        {
            if ((this.hasNoLogStatus))
                return;
            TransactionResult transactionResult;

            transactionResult = new TransactionResult();

            transactionResult.EntityName = this.DocumentName;
            transactionResult.Action = LogstateToTransactionAction(this.LogState);
            transactionResult.CRMID = this.CrmId;
            transactionResult.ID = this.Id;
            transactionResult.Status = this.status;
            transactionResult.Message = this.message;

            if(transactionResult.CRMID != null)
                result.Add(transactionResult);

            foreach (DocumentCollection coll in this.collections.Values)
                coll.GetTransactionResult(ref result);

        }


        /// <summary>
        /// Returns the document as xml node
        /// </summary>
        /// <param name="document">The paren xmlDocument to work on</param>
        /// <returns>document as xml node</returns>
        public XmlNode GetXmlNode(XmlDocument document)
        {
            XmlElement docElem;
            docElem = document.CreateElement(this.DocumentName);
            CreateAttribute(document, docElem, "id", this.Id);
            CreateAttribute(document, docElem, "state", this.LogState.ToString());

            if (this.LogState != LogState.Deleted)
            {
                foreach (Property prop in this.properties.Values)
                    CreateElement(document, docElem, prop);

                foreach (DocumentCollection coll in this.Collections.Values)
                    CreateElement(document, docElem, coll);
            }

            return docElem;
        }


        /// <summary>
        /// Read the Document from a given xml node
        /// </summary>
        /// <param name="node">The xml node named as the document</param>
        public void ReadFromXmlNode(XmlNode node)
        {

            if (node.Attributes["state"] != null)
                this.LogState = (LogState)Enum.Parse(typeof(LogState), node.Attributes["state"].Value, true);

            if (node.Attributes["id"] != null)
                this.Id = node.Attributes["id"].Value;

            if (node.Attributes["crmid"] != null)
                this.CrmId = node.Attributes["crmid"].Value;

            foreach (Property prop in this.properties.Values)
                ReadElement(node, prop);

            foreach (DocumentCollection coll in this.Collections.Values)
                ReadElement(node, coll);

        }


        public void ClearTransactionStatus()
        {

            this.hasNoLogStatus = true;
            this.message = "";

        }

        public TransactionResult SetTransactionStatus(TransactionStatus status)
        {

            return SetTransactionStatus(status, "");

        }

        public TransactionResult SetTransactionStatus(TransactionStatus status, string message)
        {
            this.hasNoLogStatus = false;
            this.status = status;
            this.message = message;
            return GetTransactionResult();

        }
        #endregion

        #region private members
        
        
        //private string ReadAttribute(XmlNode node, string name)
        //{
        //    if (node.Attributes[name] != null)
        //        return node.Attributes[name].Value;
        //    return "";
        //}

        private void CreateAttribute(XmlDocument document, XmlElement element, string name, string value)
        {
            XmlAttribute attr;
            attr = document.CreateAttribute(name);
            attr.Value = value;
            element.Attributes.Append(attr);
        }

        public XmlSchemaElement GetSchemaElement()
        {
            XmlDocument doc = new XmlDocument();
            XmlSchemaElement element = new XmlSchemaElement();
            element.Name = this.documentName;

            XmlSchemaComplexType complexType = new XmlSchemaComplexType();
            element.SchemaType = complexType;

            XmlSchemaSequence sequence = new XmlSchemaSequence();
            complexType.Particle = sequence;
            sequence.MinOccurs = 0;
            sequence.MaxOccurs = 1;

            foreach (Property prop in this.properties.Values)
                sequence.Items.Add(prop.GetSchemaElement(doc));

             foreach (DocumentCollection coll in this.Collections.Values)
                sequence.Items.Add(coll.GetSchemaElement());

            return element;
        }


        private void ReadElement(XmlNode parentNode, Property property)
        {
            if (parentNode[property.Name] == null)
            {
                property.Value = null;
                property.NotSet = true;
            }
            else if ((parentNode[property.Name].Attributes["nil"] != null) && (parentNode[property.Name].Attributes["nil"].Value.Equals(XmlConvert.ToString(true), StringComparison.InvariantCultureIgnoreCase)))
                property.Value = null;
            
            /* added by: msassanelli
             * 07-09-10
             * We have to check for the type of the property. Properties that do not accept an empty string as value
             * have to be handled as a not set property.
             * i.e. A tag is given that represents a boolean type and no innerText exists: It is handled as if the tag does not exist.
            */
            else if ((parentNode[property.Name].InnerText == "") &&
                   (property.TypeCode != TypeCode.String && property.TypeCode != TypeCode.Object))
            {
                property.Value = null;          
                property.NotSet = true;
            }
            /* end msassanelli */
            else
                property.setXMLValue(parentNode[property.Name].InnerText);
        }

        private void ReadElement(XmlNode parentNode, DocumentCollection collection)
        {
            if (collection == null)
                return;
            collection.ReadElement(parentNode);

        }

        private void CreateElement(XmlDocument document, XmlNode parentElement, Property property)
        {
            XmlElement elem;
            if (property == null) 
                return;

            if ((this.LogState != LogState.Created) && (property.NotSet))
                return;

            elem = document.CreateElement(property.Name);
            if (property.IsNull)
            {
                elem.IsEmpty = true;
                elem.Attributes.Append(document.CreateAttribute("nil"));
                elem.Attributes["nil"].Value = XmlConvert.ToString(true);
            }
            else
                elem.InnerText = property.GetXMLValue();

            parentElement.AppendChild(elem);
        }

        private TransactionAction LogstateToTransactionAction(LogState logState)
        {
            switch (logState)
            {
                case LogState.Created:
                    return TransactionAction.Created;
                case LogState.Updated:
                    return TransactionAction.Updated;
                case LogState.Deleted:
                    return TransactionAction.Deleted;
                default:
                    return TransactionAction.Created;

            }
        }

        private void CreateElement(XmlDocument document, XmlNode parentElement, DocumentCollection collection)
        {
            if (collection == null)
                return;
            parentElement.AppendChild(collection.GetXmlNode(document));
        }
        #endregion

        public void AddDefaultResourceElements(XmlElement root)
        {
            foreach (Property prop in this.properties.Values)
                prop.AddDefaultResourceElements(root);

            foreach (DocumentCollection coll in this.collections.Values)
                coll.GetDocumentTemplate().AddDefaultResourceElements(root);
                
        }
    }
}
