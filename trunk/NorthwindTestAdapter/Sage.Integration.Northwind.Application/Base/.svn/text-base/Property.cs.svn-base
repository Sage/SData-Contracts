using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using Sage.Integration.Northwind.Application.EntityResources;

namespace Sage.Integration.Northwind.Application.Base
{
    /// <summary>
    /// This class represents a property of an document
    /// </summary>
    public class Property
    {
        #region constructor
        
        /// <summary>
        /// Constructor. The method initializes the class.
        /// </summary>
        /// <param name="propName">the Name Of the Property</param>
        /// <param name="propTypecode">the typecode of the value</param>
        /// <param name="propMaxLength">the maxlength of an string value</param>
        /// /// <param name="propParent">the parent document</param>
        public Property(string propName, TypeCode propTypecode, int propMaxLength, Document propParent)
        {
            InitProperty(propName, propTypecode, propParent);
            this.maxLength = propMaxLength;
        }

        /// <summary>
        /// Constructor. The method initializes the class.
        /// </summary>
        /// <param name="propName">the Name Of the Property</param>
        /// <param name="propTypecode">the typecode of the value</param>
        /// /// <param name="propParent">the parent document</param>
        public Property(string propName, TypeCode propTypecode, Document propParent)
        {
            InitProperty(propName, propTypecode, propParent);
            this.maxLength = 0;
        }

        private void InitProperty(string propName, TypeCode propTypecode, Document propParent)
        {

            this.parent = propParent;
            this.name = propName;
            this.typeCode = propTypecode;
            this.NotSet = true;

            try
            {
                this.crmCaption = DocumentPropertyCaption.ResourceManager.GetString(parent.DocumentName + "." + this.Name);
                if ((this.crmCaption == null) || (this.crmCaption == ""))
                    this.crmCaption = this.Name;

            }
            catch (Exception)
            {
                this.crmCaption = this.Name;
            }

        }

        #endregion

        #region fields
        private Document parent;
        private object value;
        private string name;
        private string crmCaption;
        private TypeCode typeCode;
        private int maxLength;
        private bool notSet;

        #endregion

        #region public properties

        /// <summary>
        /// is true if the value is null.
        /// </summary>
        public bool IsNull
        {
            get { return (value == null); }
        }

        /// <summary>
        /// is true if the value was never set. 
        /// Neither thru the value set, nor thru the setxmlValue method
        /// </summary>
        public bool NotSet
        {
            get { return notSet; }
            set { this.notSet = value; }
        }

        /// <summary>
        /// the value of the property
        /// </summary>
        public object Value
        {
            get
            {
                // truncate the value if a maxlength was set
                if ((this.value != null) && (this.typeCode == TypeCode.String || this.typeCode == TypeCode.Object) && (maxLength > 0))
                {
                    string result = this.value.ToString();
                    return result.Length > this.maxLength ? result.Substring(0, this.maxLength) : result;
                }

                return this.value;
            }
            set
            {
                this.value = value;

                // The value is set now
                this.notSet = false;
            }
        }

        /// <summary>
        /// The typecode of the value.
        /// </summary>
        public TypeCode TypeCode
        {
            get { return this.typeCode; }
        }

        /// <summary>
        /// the name of the property. 
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        #endregion

        #region public members

        /// <summary>
        /// Returns a xml schema element wich defines the property
        /// </summary>
        /// <returns>xml schema element</returns>
        public XmlSchemaElement GetSchemaElement(XmlDocument doc)
        {
            XmlAttribute[] attr;

            //create a new xml schema element to return
            XmlSchemaElement elem = new XmlSchemaElement();

            // set the name of the element
            elem.Name = this.Name;

            attr = new XmlAttribute[1];
            attr[0] = doc.CreateAttribute("caption");
            attr[0].Value = this.crmCaption;
            //attr[1] = doc.CreateAttribute("Size");
            elem.UnhandledAttributes = attr;

            // set defaulvalues for min and max occurs
            elem.MinOccurs = 0;
            elem.MaxOccurs = 1;

            // by default is every property nillable
            elem.IsNillable = true;

            // set the schema type mane by typecode
            switch (this.typeCode)
            {
                case TypeCode.Boolean:
                    elem.SchemaTypeName = new XmlQualifiedName("boolean", "http://www.w3.org/2001/XMLSchema");
                    break;
                case TypeCode.DateTime:
                    elem.SchemaTypeName = new XmlQualifiedName("dateTime", "http://www.w3.org/2001/XMLSchema");
                    break;
                case TypeCode.Decimal:
                    elem.SchemaTypeName = new XmlQualifiedName("decimal", "http://www.w3.org/2001/XMLSchema");
                    break;
                case TypeCode.Double:
                    elem.SchemaTypeName = new XmlQualifiedName("double", "http://www.w3.org/2001/XMLSchema");
                    break;
                case TypeCode.Int32:
                    elem.SchemaTypeName = new XmlQualifiedName("int", "http://www.w3.org/2001/XMLSchema");
                    break;
                case TypeCode.Int16:
                    elem.SchemaTypeName = new XmlQualifiedName("short", "http://www.w3.org/2001/XMLSchema");
                    break;
                case TypeCode.String:

                    // for string values 
                    elem.SchemaTypeName = new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema");

                    if (this.maxLength > 0)
                    {
                        attr = new XmlAttribute[2];
                        attr[0] = doc.CreateAttribute("caption");
                        attr[0].Value = this.crmCaption;
                        attr[1] = doc.CreateAttribute("size");
                        attr[1].Value = this.maxLength.ToString();
                        elem.UnhandledAttributes = attr;

                        //XmlSchemaSimpleType simpleType = new XmlSchemaSimpleType();
                        //simpleType.Name = this.name + "Type";
                        //XmlSchemaSimpleTypeRestriction restriction = new XmlSchemaSimpleTypeRestriction();
                        //restriction.BaseTypeName = new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema");
                        //XmlSchemaMaxLengthFacet maxLengthAttibute = new XmlSchemaMaxLengthFacet();
                        //maxLengthAttibute.Value = maxLength.ToString();
                        //restriction.Facets.Add(maxLengthAttibute);
                        //simpleType.Content = restriction;
                        //elem.SchemaType = simpleType;
                        //elem.SchemaTypeName = simpleType.QualifiedName;
                    }
                    break;

                case TypeCode.Object:

                    // for string values 
                    elem.SchemaTypeName = new XmlQualifiedName("text", "http://www.w3.org/2001/XMLSchema");
                    break;
                default:
                    elem.SchemaTypeName = new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema");
                    break;
            }

            // return the schema element
            return elem;
        }

        public string GetXMLValue()
        {
            switch (this.typeCode)
            {
                case TypeCode.Boolean:
//#warning Temporary fix since "OnHold" passes boolean "True" as "Y"
//                    if ((bool)this.value)
//                        return "True";
//                    return "False";
                    return XmlConvert.ToString((bool)this.value);
                case TypeCode.DateTime:
                    return XmlConvert.ToString((DateTime)this.value, XmlDateTimeSerializationMode.Unspecified);
                case TypeCode.Decimal:
                    return XmlConvert.ToString(Convert.ToDecimal( this.value));
                case TypeCode.Double:
                    return XmlConvert.ToString(Convert.ToDouble(this.value));
                case TypeCode.Int32:
                    return XmlConvert.ToString(Convert.ToInt32(this.value));
                case TypeCode.Int16:
                    return XmlConvert.ToString(Convert.ToInt16( this.value));
                case TypeCode.String:
                    return (string)this.value;
                case TypeCode.Object:
                    return (string)this.value;
                default:
                    return null;
            }
        }

        public void setXMLValue(string value)
        {
            this.notSet = false;
            switch (this.typeCode)
            {
                case TypeCode.Boolean:
                    switch (value)
                    {
#warning Temporary fix since "OnHold" passes boolean "True" as "Y"
                        case "Y":
                            this.value = true;
                            break;
                        case "N":
                            this.value = false;
                            break;
                        default:
                            this.value = XmlConvert.ToBoolean(value);
                            break;
                    }
                    break;
                case TypeCode.DateTime:
                    this.value = XmlConvert.ToDateTime(value, XmlDateTimeSerializationMode.Unspecified);
                    break;
                case TypeCode.Decimal:
                    this.value = XmlConvert.ToDecimal(value);
                    break;

                case TypeCode.Double:
                    this.value = XmlConvert.ToDouble(value);
                    break;
                case TypeCode.Int32:
                    this.value = XmlConvert.ToInt32(value);
                    break;

                case TypeCode.Int16:
                    this.value = XmlConvert.ToInt16(value);
                    break;
                case TypeCode.String:
                    this.value = value;
                    break;
                case TypeCode.Object:
                    this.value = value;
                    break;
                default:
                    this.value = null;
                    break;
            }
        }

        #endregion

        public void AddDefaultResourceElements(XmlElement root)
        {
            XmlElement data = root.OwnerDocument.CreateElement("data");
            XmlAttribute name = root.OwnerDocument.CreateAttribute("name");
            name.Value = this.parent.DocumentName + "." + this.Name;
            data.Attributes.Append(name);
            XmlAttribute xmlspace = root.OwnerDocument.CreateAttribute("xml:space");
            xmlspace.Value = "preserve";
            XmlElement value = root.OwnerDocument.CreateElement("value");
            value.InnerText = this.crmCaption;
            data.AppendChild(value);
            root.AppendChild(data);
        }
    }
}
