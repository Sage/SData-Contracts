#region ©2007 Sage Technology Ltd., All Rights Reserved.
/*=====================================================================
	File:			OrderAddress.cs
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
    public class OrderAddress
    {
        #region constructor

        public OrderAddress()
        {
            //init the default northwind maxLength
            this.countryCodes = new CountryCodes();
        }

        #endregion

        #region fields
        private string crmOrderAddress;
        private string northwindAddress;
        private string northwindCity;
        private string northwindZipCode;
        private string northwindCountry;
        private CountryCodes countryCodes;
        #endregion

        #region public properties




        /// <summary>
        /// The address line supported by northwind
        /// </summary>
        public string CrmOrderAddress
        {
            get
            {
                crmOrderAddress = CombineAddress(northwindAddress,
                    northwindCity, northwindZipCode, northwindCountry);
                return crmOrderAddress;
            }
            set
            {
                crmOrderAddress = value;
                SplitAddress(crmOrderAddress,
                    ref northwindAddress, ref northwindCity,
                    ref northwindZipCode, ref northwindCountry);
                crmOrderAddress = CombineAddress(northwindAddress,
                    northwindCity, northwindZipCode, northwindCountry);
            }
        }


        public string NorthwindAddress
        {
            get {
                if (northwindAddress!= null && northwindAddress.Length > 60)
                    northwindAddress = northwindAddress.Substring(0, 60); 
                return northwindAddress; }
            set { northwindAddress = value; }
        }

        public string NorthwindCity
        {
            get {
                if (northwindCity != null && northwindCity.Length > 15)
                    northwindCity = northwindCity.Substring(0, 15); 
                
                return northwindCity; }
            set { northwindCity = value; }
        }

        public string NorthwindZipCode
        {
            get
            {
                if (northwindZipCode != null && northwindZipCode.Length > 10)
                    northwindZipCode = northwindZipCode.Substring(0, 10);
                return northwindZipCode; 
            }
            set { northwindZipCode = value; }
        }

        public string NorthwindCountry
        {
            get
            {
                if (northwindCountry != null && northwindCountry.Length > 15)
                    northwindCountry = northwindCountry.Substring(0, 15); 
                
                return northwindCountry; }
            set { northwindCountry = value; }
        }


        #endregion

        #region public members

        public void SetNorthwindAddress(string street, string city, string zipCode, string country)
        {
                northwindAddress = (string)street;

                northwindCity = (string)city;

                northwindZipCode = (string)zipCode;

                northwindCountry = (string)country;

        }
        #endregion

        #region private members
        private const string lf = "\n";





        private string CombineAddress(string northwindAddress, string northwindCity, string northwindZipCode, string northwindCountry)
        {
            string tmpAdress1 = northwindAddress;
            try
            {
                northwindAddress = (northwindAddress == null) ? "" : northwindAddress;
                northwindCity = (northwindCity == null) ? "" : northwindCity;
                northwindZipCode = (northwindZipCode == null) ? "" : northwindZipCode;
                northwindCountry = (northwindCountry == null) ? "" : northwindCountry;

                tmpAdress1 = northwindAddress + Constants.crlf + northwindCity + Constants.crlf + northwindZipCode;

                string country = this.countryCodes.GetCountry(northwindCountry);
                if (country.Length > 0)
                    tmpAdress1 = tmpAdress1 + " " + country;
            }

            catch (Exception) { }
            return tmpAdress1;
         }


        private void SplitAddress(string crmOrderAddress, ref string northwindAddress, ref string northwindCity, ref string northwindZipCode, ref string northwindCountry)
        {
            List<string> stringList = new List<string>();

            string tempAddress = crmOrderAddress;
            if (!tempAddress.Contains(lf))
            {
                northwindAddress = tempAddress;
                northwindCity = null;
                northwindZipCode = null;
                northwindCountry = null;
                return;
            }


            while (tempAddress.Contains(lf))
            {
                if (tempAddress.IndexOf(Constants.crlf) >= 0 && (tempAddress.IndexOf(Constants.crlf) < tempAddress.IndexOf(lf)))
                {
                    stringList.Add(tempAddress.Substring(0, tempAddress.IndexOf(Constants.crlf, 0)));
                    tempAddress = tempAddress.Substring(tempAddress.IndexOf(Constants.crlf, 0) + Constants.crlf.Length);
                }
                else
                {
                    stringList.Add(tempAddress.Substring(0, tempAddress.IndexOf(lf, 0)));
                    tempAddress = tempAddress.Substring(tempAddress.IndexOf(lf, 0) + lf.Length);
                }
            }


            stringList.Add(tempAddress);

            if (stringList.Count == 2)
            {
                northwindAddress = stringList[0];
                northwindCity = stringList[1];
                northwindZipCode = null;
                northwindCountry = null;
                return;
            }

            northwindAddress = stringList[0];
            northwindCity = stringList[stringList.Count - 2];
            northwindZipCode = stringList[stringList.Count - 1];
            northwindCountry = this.countryCodes.extractCountry(ref northwindZipCode);

            for (int index = 1; index < stringList.Count-2; index++)
                northwindAddress = northwindAddress + Constants.crlf + stringList[index];
        }


        #endregion
    }
}
