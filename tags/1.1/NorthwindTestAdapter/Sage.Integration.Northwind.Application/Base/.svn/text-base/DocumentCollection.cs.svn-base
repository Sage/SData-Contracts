#region ©2007 Sage Technology Ltd., All Rights Reserved.
/*=====================================================================
	File:			DocumentCollection.cs
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
using System.Collections;
using System.Xml;
using System.Xml.Schema;
using Sage.Integration.Northwind.Application.API;
using Sage.Integration.Northwind.Application.Toolkit;

#endregion

namespace Sage.Integration.Northwind.Application.Base
{
    /// <summary>
    /// DocumentCollection class
    /// </summary>
    public abstract class DocumentCollection : IEnumerable
    {
        #region constructor

        /// <summary>
        /// constructor of an document Collection
        /// </summary>
        /// <param name="name">the name of the document collection</param>
        protected DocumentCollection(string name)
        {
            this.name = name;
            this.documents = new List<Document>();
        }
        #endregion

        #region fields
        /// <summary>
        /// the name of the document collection
        /// </summary>
        protected string name;
        public List<Document> documents;
        #endregion

        #region public properties
        
        public string CollectionName
        {
            get { return this.name; }
        }

        #endregion

        #region public members

        public void GetTransactionResult(ref List<TransactionResult> result)
        {
            foreach (Document doc in documents)
                doc.GetTransactionResult(ref result);

        }


        public void Merge(DocumentCollection sourceCollection)
        {
            #region Declarations
            List<string> matchedIDs;
            List<Document> newDocs;

            matchedIDs = new List<string>();
            newDocs = new List<Document>();
            Document sourceDocument;

            #endregion

            // remove deleted documents from the target collection
            for (int index = this.documents.Count - 1; index >= 0; index--)
                if (this.documents[index].LogState == LogState.Deleted)
                    this.documents.RemoveAt(index);


            // go thru each target
            foreach (Document target in this)
            {

                // set the temopry soure docuemtn to null
                sourceDocument = null;
                
                // go thru each source document
                foreach (Document source in sourceCollection)
                {
                    //try to match by type
                    if ((source.Id == null) || (source.Id == ""))
                    {
                        //if the is no already matched sourec document or the matched source is deleted
                        if (((sourceDocument == null) || (sourceDocument.LogState == LogState.Deleted)) &&
 
                            // if it is a new one
                            (source.LogState == LogState.Created) &&


                            (source.GetTypePropertyValue() != "") &&
                            (source.GetTypePropertyValue() == target.GetTypePropertyValue()))
                            
                            sourceDocument = source;
                            
                        continue;
                    }

                    if (matchedIDs.Contains(source.Id))
                        continue;

                    if (source.Id != target.Id)
                        continue;

                    if (source.LogState != LogState.Deleted)
                        matchedIDs.Add(source.Id);


                    sourceDocument = source;
                    
                }

                if (sourceDocument != null)
                    target.Merge(sourceDocument);
                else
                    target.ClearTransactionStatus();
            }
            foreach (Document source in sourceCollection)
            {
                if (matchedIDs.Contains(source.Id))
                    continue;

                if ((source.LogState == LogState.Created) || (source.LogState == LogState.Deleted))
                    this.Add(source);
            }
        }

        public void ReadElement(XmlNode parentNode)
        {
            this.documents.Clear();

            if (parentNode[this.name] == null)
                return;

            Document doc;
            foreach (XmlNode node in parentNode[this.name].ChildNodes)
            {
                doc = GetDocumentTemplate();
                if (node.Name != doc.DocumentName)
                    continue;
                doc.ReadFromXmlNode(node);
                this.Add(doc);
            }
        }

        public abstract Document GetDocumentTemplate();

        public XmlSchemaElement GetSchemaElement()
        {
            XmlSchemaElement element = new XmlSchemaElement();
            
            element.Name = this.name;

            XmlSchemaComplexType complexType = new XmlSchemaComplexType();
            element.SchemaType = complexType;

            XmlSchemaSequence sequence = new XmlSchemaSequence();
            complexType.Particle = sequence;
            sequence.MinOccurs = 0;
            sequence.MaxOccursString = "unbounded";

            sequence.Items.Add(GetDocumentTemplate().GetSchemaElement());

            return element;
        }

        public XmlNode GetXmlNode(XmlDocument document)
        {
            XmlNode node = document.CreateElement(this.name);
            
            foreach (Document doc in this.documents)
            {
                if (doc.HasNoLogStatus)
                    continue;
                node.AppendChild(doc.GetXmlNode(document));
            }
            return node;
        }

        public bool NotSet
        {
            get
            {
                bool notSet = true;
                
                if (this.documents.Count < 1)
                    return notSet;

                foreach (Document doc in this.documents)
                {
                    notSet = notSet && doc.NotSet;
                    if (!notSet)
                        return notSet;

                }
                return notSet;

            }
        }

        public void Add(Document document)
        {
            documents.Add(document);
        }


        #endregion

        #region IEnumerable Members

        /// <summary>
        /// GetEnumerator
        /// </summary>
        /// <returns>IEnumerator</returns>
        public IEnumerator GetEnumerator()
        {
            return this.documents.GetEnumerator();
        }

        #endregion

        #region private members

        #endregion
    }
}
