using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Sage.Integration.Northwind.Application.Base
{
    public class CRMDocumentProperties
    {
        public XmlAttribute[] GetUnhandledAttributes()
        {
            XmlDocument doc = new XmlDocument();
            List<XmlAttribute> attributes = new List<XmlAttribute>();
            XmlAttribute attr;


            if (this.mappingname != null && this.mappingname.Length > 0)
            {
                attr = doc.CreateAttribute("mappingname");
                attr.Value = this.mappingname;
                attributes.Add(attr);
            }

            if (this.winner != null && this.winner.Length > 0)
            {
                attr = doc.CreateAttribute("winner");
                attr.Value = this.winner;
                attributes.Add(attr);
            }


            if (this.prefix != null && this.prefix.Length > 0)
            {
                attr = doc.CreateAttribute("prefix");
                attr.Value = this.prefix;
                attributes.Add(attr);
            }


            if (this.crmentity != null && this.crmentity.Length > 0)
            {
                attr = doc.CreateAttribute("crmentity");
                attr.Value = this.crmentity;
                attributes.Add(attr);
            }


            if (this.syncdata != null && this.syncdata.Length > 0)
            {
                attr = doc.CreateAttribute("syncdata");
                attr.Value = this.syncdata;
                attributes.Add(attr);
            }


            if (this.syncorder != null && this.syncorder.Length > 0)
            {
                attr = doc.CreateAttribute("syncorder");
                attr.Value = this.syncorder;
                attributes.Add(attr);
            }


            if (this.isprimarytable != null && this.isprimarytable.Length > 0)
            {
                attr = doc.CreateAttribute("isprimarytable");
                attr.Value = this.isprimarytable;
                attributes.Add(attr);
            }

            if (this.idfield != null && this.idfield.Length > 0)
            {
                attr = doc.CreateAttribute("idfield");
                attr.Value = this.idfield;
                attributes.Add(attr);
            }


            return attributes.ToArray();
        }
        private string mappingname;

        public string Mappingname
        {
            get { return mappingname; }
            set { mappingname = value; }
        }
        private string winner;

        public string Winner
        {
            get { return winner; }
            set { winner = value; }
        }
        private string prefix;

        public string Prefix
        {
            get { return prefix; }
            set { prefix = value; }
        }
        private string crmentity;

        public string Crmentity
        {
            get { return crmentity; }
            set { crmentity = value; }
        }
        private string syncdata;

        public string Syncdata
        {
            get { return syncdata; }
            set { syncdata = value; }
        }
        private string syncorder;

        public string Syncorder
        {
            get { return syncorder; }
            set { syncorder = value; }
        }
        private string isprimarytable;

        public string Isprimarytable
        {
            get { return isprimarytable; }
            set { isprimarytable = value; }
        }
        private string idfield;

        public string Idfield
        {
            get { return idfield; }
            set { idfield = value; }
        }
    }
}
