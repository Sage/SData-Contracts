#region ©2007 Sage Technology Ltd., All Rights Reserved.
/*=====================================================================
    File:        RTDVField.cs
    Author:      msassanelli
    DateCreated: 05-04-2007
    DateChanged: 05-04-2007
    ---------------------------------------------------------------------
    ©2007 Sage Technology Ltd., All Rights Reserved. 
=====================================================================*/

/*=====================================================================
    Date        Author       Changes     Reasons
    05-04-2007  msassanelli  Create   
    

    Changes: Create, Refactoring, Upgrade 
=====================================================================*/
#endregion

#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;

#endregion

namespace Sage.Integration.Northwind.Application.Base
{
    public class RTDVField
    {
        #region Fields

        private string _name;
        private string _displayName;
        private int _minOccurs;
        private int _maxOccurs;
        private TypeCode _typeCode;
        private int _size;

        #endregion

        #region Ctor.

        public RTDVField(string name, TypeCode typeCode)
        {
            _name = name;
            _typeCode = typeCode;

            // initializations
            _displayName = _name;
            _minOccurs = 0;
            _maxOccurs = 1;
            _size = 0;
        }

        #endregion

        #region Properties

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }

        public int MinOccurs
        {
            get { return _minOccurs; }
            set { _minOccurs = value; }
        }

        public int MaxOccurs
        {
            get { return _maxOccurs; }
            set { _maxOccurs = value; }
        }

        public TypeCode TypeCode
        {
            get { return _typeCode; }
            set { _typeCode = value; }
        }

        public int Size
        {
            get { return _size; }
            set { _size = value; }
        }

        #endregion

        #region Schema Members

        public XmlSchemaElement GetSchemaElement(XmlDocument xmlDoc)
        {
            // declarations
            XmlSchemaElement xmlSchemaElement;

            // initializations            
            xmlSchemaElement = new XmlSchemaElement();
            
            // set defined attributes
            xmlSchemaElement.Name = _name;
            xmlSchemaElement.MinOccurs = _minOccurs;
            xmlSchemaElement.MaxOccurs = _maxOccurs;
            xmlSchemaElement.IsNillable = true;
            xmlSchemaElement.SchemaTypeName = this.GetXmlQualifiedName();

            // set unhandled attributes
            xmlSchemaElement.UnhandledAttributes = this.GetUnhandledAttributes(xmlDoc);

            return xmlSchemaElement;
        }

        

        #region Private Helper Members

        private XmlQualifiedName GetXmlQualifiedName()
        {
            switch (_typeCode)
            {
                case TypeCode.Boolean:
                    return new XmlQualifiedName("boolean", "http://www.w3.org/2001/XMLSchema");
                case TypeCode.DateTime:
                    return new XmlQualifiedName("dateTime", "http://www.w3.org/2001/XMLSchema");
                case TypeCode.Decimal:
                    return new XmlQualifiedName("decimal", "http://www.w3.org/2001/XMLSchema");
                case TypeCode.Double:
                    return new XmlQualifiedName("double", "http://www.w3.org/2001/XMLSchema");
                case TypeCode.Int32:
                    return new XmlQualifiedName("int", "http://www.w3.org/2001/XMLSchema");
                case TypeCode.Int16:
                    return new XmlQualifiedName("short", "http://www.w3.org/2001/XMLSchema");
                case TypeCode.String:
                    return  new XmlQualifiedName("varchar", "http://www.w3.org/2001/XMLSchema");
                    //return  new XmlQualifiedName("multitext", "http://www.w3.org/2001/XMLSchema");
                default:
                    //return new XmlQualifiedName("text", "http://www.w3.org/2001/XMLSchema");
                    return new XmlQualifiedName("varchar", "http://www.w3.org/2001/XMLSchema");
            }
        }

        private int GetSize()
        {
            switch (_typeCode)
            {
                case TypeCode.Boolean:
                    return 1;
                case TypeCode.DateTime:
                    return 8;
                case TypeCode.Decimal:
                    return 9;
                case TypeCode.Double:
                    return 8;
                case TypeCode.Int32:
                    return 4;
                case TypeCode.Int16:
                    return 2;
                case TypeCode.String:
                    return this._size;
                default:
                    return this._size;
            }
        }

        private XmlAttribute[] GetUnhandledAttributes(XmlDocument xmlDoc)
        {
            // declarations
            List<XmlAttribute> attributeList;
            XmlAttribute attribute;

            // Initializations
            attributeList  = new List<XmlAttribute>();

            // caption
            attribute = xmlDoc.CreateAttribute("caption");
            attribute.Value = _displayName;
            attributeList.Add(attribute);

            if (_typeCode == TypeCode.String)
            {
                // size
                attribute = xmlDoc.CreateAttribute("size");
                attribute.Value = Convert.ToString(_size);
                attributeList.Add(attribute);
            }
#warning remove if crm does not need the size anymore
            else
            {
                attribute = xmlDoc.CreateAttribute("size");
                attribute.Value = Convert.ToString(GetSize());
                attributeList.Add(attribute);
            }

            return attributeList.ToArray();
        }

        #endregion

        #endregion
    }
}
