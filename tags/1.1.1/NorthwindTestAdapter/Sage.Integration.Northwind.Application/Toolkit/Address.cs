#region ©2007 Sage Technology Ltd., All Rights Reserved.
/*=====================================================================
	File:			Address.cs
	Author:			Philipp Schuette
	DateCreated:	04/04/2007 14:02:14
	DateChanged:	04/04/2007 14:02:14
 ---------------------------------------------------------------------
	©2007 Sage Technology Ltd., All Rights Reserved.	
=====================================================================*/

/*=====================================================================
	Date			        Author		Changes			Reasons
	04/04/2007 14:02:14	pschuette		Create			  
	Changes: Create, Refactoring, Upgrade	
=====================================================================*/
#endregion

#region Usings
using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Northwind.Application.Base;
using Sage.Integration.Northwind.Application.API;
#endregion

namespace Sage.Integration.Northwind.Application.Toolkit
{
    /// <summary>
    /// this class will split northwind address and concat a crm adress
    /// it is also possible to initiate the class with an northwing string, replace an part of crm (like address2)
    /// and get the updated nortwind adress
    /// </summary>
    public class Address
    {
        #region constructor

        public Address()
        {
            //init the default northwind maxLength
            this.maxLength = 60;
        }

        #endregion

        #region fields
        private int maxLength;
        private string northwindAddress;
        private string crmAddressLine1;
        private string crmAddressLine2;
        private string crmAddressLine3;
        private string crmAddressLine4;
        #endregion

        #region public properties


        /// <summary>
        /// the maxlength supported by northwind
        /// depend on this length crm parts will ignored or trancated 
        /// </summary>
        public int MaxLength
        {
            get { return maxLength; }
            set { maxLength = value; }
        }

        /// <summary>
        /// The address line supported by northwind
        /// </summary>
        public string NorthwindAddress
        {
            get
            {
                northwindAddress = CombineAddress(crmAddressLine1, 
                    crmAddressLine2, crmAddressLine3, crmAddressLine4);
                return northwindAddress;
            }
            set
            {
                northwindAddress = value;
                SplitAddress(northwindAddress,
                    ref crmAddressLine1, ref crmAddressLine2, 
                    ref crmAddressLine3, ref crmAddressLine4);
                northwindAddress = CombineAddress(crmAddressLine1, 
                    crmAddressLine2, crmAddressLine3, crmAddressLine4);
            }
        }
        public string CrmAddressLine1
        {
            get { return crmAddressLine1; }
            set { crmAddressLine1 = value; }
        }

        public string CrmAddressLine2
        {
            get { return crmAddressLine2; }
            set { crmAddressLine2 = value; }
        }

        public string CrmAddressLine3
        {
            get { return crmAddressLine3; }
            set { crmAddressLine3 = value; }
        }

        public string CrmAddressLine4
        {
            get { return crmAddressLine4; }
            set { crmAddressLine4 = value; }
        }


        #endregion

        #region public members

        public void SetCrmAdresses(Property address1, Property address2, Property address3, Property address4)
        {
            crmAddressLine1 = "";
            crmAddressLine2 = "";
            crmAddressLine3 = "";
            crmAddressLine4 = "";

            if (!address1.NotSet)
                crmAddressLine1 = (string)address1.Value;

            if (!address2.NotSet)
                crmAddressLine2 = (string)address2.Value;

            if (!address3.NotSet)
                crmAddressLine3 = (string)address3.Value;

            if (!address4.NotSet)
                crmAddressLine4 = (string)address4.Value;

        }
        #endregion

        #region private members



        private string CombineAddress(string address1, string address2,
            string address3, string address4)
        {
            address1 = (address1 == null) ? "" : address1;
            address2 = (address2 == null) ? "" : address2;
            address3 = (address3 == null) ? "" : address3;
            address4 = (address4 == null) ? "" : address4;

            string tmpAdress1 = address1;

            if (address4.Length > 0)
                return address1 + Constants.crlf +
                    address2 + Constants.crlf +
                    address3 + Constants.crlf +
                    address4;
            if (address3.Length > 0)
                return address1 + Constants.crlf +
                    address2 + Constants.crlf +
                    address3;

            if (address2.Length > 0)
                return address1 + Constants.crlf +
                    address2;
            return tmpAdress1.Length > this.maxLength ? tmpAdress1.Substring(this.maxLength) : tmpAdress1;
        }

        private void SplitAddress(string address,
            ref string address1, ref string address2,
            ref string address3, ref string address4)
        {
            string tempAddress = address;
            if (!tempAddress.Contains(Constants.crlf))
            {
                address1 = tempAddress;
                address2 = null;
                address3 = null;
                address4 = null;
                return;
            }
            address1 = tempAddress.Substring(0, tempAddress.IndexOf(Constants.crlf, 0));
            tempAddress = tempAddress.Substring(tempAddress.IndexOf(Constants.crlf, 0) + Constants.crlf.Length);

            if (!tempAddress.Contains(Constants.crlf))
            {
                address2 = tempAddress;
                address3 = null;
                address4 = null;
                return;
            }

            address2 = tempAddress.Substring(0, tempAddress.IndexOf(Constants.crlf, 0));
            tempAddress = tempAddress.Substring(tempAddress.IndexOf(Constants.crlf, 0) + Constants.crlf.Length);

            if (!tempAddress.Contains(Constants.crlf))
            {
                address3 = tempAddress;
                address4 = null;
                return;
            }

            address3 = tempAddress.Substring(0, tempAddress.IndexOf(Constants.crlf, 0));
            address4 = tempAddress.Substring(tempAddress.IndexOf(Constants.crlf, 0) + Constants.crlf.Length);

        }


        #endregion
    }
}
