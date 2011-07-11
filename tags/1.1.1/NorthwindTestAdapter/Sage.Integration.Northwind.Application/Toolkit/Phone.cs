#region ©2007 Sage Technology Ltd., All Rights Reserved.
/*=====================================================================
	File:			Phone.cs
	Author:			Philipp Schuette
	DateCreated:	04/04/2007 14:51:24
	DateChanged:	04/04/2007 14:51:24
 ---------------------------------------------------------------------
	©2007 Sage Technology Ltd., All Rights Reserved.	
=====================================================================*/

/*=====================================================================
	Date			        Author		Changes			Reasons
	04/04/2007 14:51:24	pschuette		Create			  
	Changes: Create, Refactoring, Upgrade	
=====================================================================*/
#endregion

#region Usings
using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Northwind.Application.Base;
#endregion

namespace Sage.Integration.Northwind.Application.Toolkit
{
    public class Phone
    {
        #region constructor

        public Phone()
        {
            this.maxLength = 24; 
        }

        #endregion

        #region fields
        private int maxLength;

        public int MaxLength
        {
            get { return maxLength; }
            set { maxLength = value; }
        }

        private string northwindPhone;
        private string crmCountryCode;
        private string crmAreaCode;
        private string crmPhone;
        #endregion

        #region public properties
        public string NorthwindPhone
        {
            get
            {
                northwindPhone = CombinePhone(crmCountryCode,
                    crmAreaCode, crmPhone);
                return northwindPhone;
            }
            set
            {
                northwindPhone = value;
                SplitPhone(northwindPhone,
                    ref crmCountryCode, ref crmAreaCode,
                    ref crmPhone);
                northwindPhone = CombinePhone(crmCountryCode,
                    crmAreaCode, crmPhone);
            }
        }
        public string CrmCountryCode
        {
            get { return crmCountryCode; }
            set { crmCountryCode = value; }
        }
        public string CrmAreaCode
        {
            get { return crmAreaCode; }
            set { crmAreaCode = value; }
        }
        public string CrmPhone
        {
            get { return crmPhone; }
            set { crmPhone = value; }
        }
        #endregion

        #region public members
        public void SetCrmPhone(Property countryCode, Property areaCode, Property phone)
        {
            crmCountryCode = "";
            crmAreaCode = "";
            crmPhone = "";
            if (!countryCode.NotSet)
                crmCountryCode = (string)countryCode.Value;

            if (!areaCode.NotSet)
                crmAreaCode = (string)areaCode.Value;

            if (!phone.NotSet)
                crmPhone = (string)phone.Value;


        }
        #endregion

        #region private members
        #region Phone
        private const string digits = "01234567890";

        private void SplitPhone(string phone,
            ref string countryCode,
            ref string areaCode,
            ref string phoneNumber)
        {
            string tempPhone;

            if (phone == null)
            {
                countryCode = null;
                areaCode = null;
                phoneNumber = null;
                return;
            }
            phone = FormatPhone(phone);

            tempPhone = phone;

            if (tempPhone.StartsWith("+") && !tempPhone.Contains("-"))
            {
                countryCode = tempPhone;
                areaCode = null;
                phoneNumber = null;
                return;

            }

            if (tempPhone.StartsWith("+"))
            {
                countryCode = tempPhone.Substring(0, tempPhone.IndexOf("-", 0));
                tempPhone = tempPhone.Substring(tempPhone.IndexOf("-", 0) + 1);
            }
            else
                countryCode = "";

            if (!tempPhone.Contains("-"))
            {
                areaCode = null;
                phoneNumber = tempPhone;
                return;
            }

            areaCode = tempPhone.Substring(0, tempPhone.IndexOf("-", 0));
            phoneNumber = tempPhone.Substring(tempPhone.IndexOf("-", 0) + 1);


        }

        private string FormatPhone(string number)
        {
            string result = "";
            if (number == null)
                return "";
            if ((number.StartsWith("+")) || (number.StartsWith("++")))
                result += "+";

            for (int i = 0; i < number.Length; i++)
            {
                if (digits.Contains(number[i].ToString()))
                    result += number[i];
                else if (!((result.EndsWith("-")) || (result.EndsWith("+"))))
                    result += "-";
            }

            if (result.StartsWith("-"))
                result = result.Substring(1);

            return result;
        }

        private string RemoveNonDigits(string number)
        {
            string result = "";
            if (number == null)
                return "";
            for (int i = 0; i < number.Length; i++)
            {
                if (digits.Contains(number[i].ToString()))
                    result += number[i];
            }
            return result;
        }

        private string CombinePhone(
            string countryCode,
            string areaCode,
            string phoneNumber)
        {
            string phone = "";
            if ((countryCode == null) &&
                (areaCode == null) &&
                (phoneNumber == null))
                return null;

            countryCode = RemoveNonDigits(countryCode);
            areaCode = RemoveNonDigits(areaCode);
            phoneNumber = RemoveNonDigits(phoneNumber);

            if ((countryCode.Length + areaCode.Length + phoneNumber.Length) <= this.maxLength)
            {

                if (countryCode.StartsWith("00"))
                    phone = "+" + countryCode.Substring(1);
                else if (countryCode.StartsWith("0"))
                    phone = "+" + countryCode.Substring(0);
                else if (countryCode.Length > 0)
                    phone = "+" + countryCode;

            }
            if ((areaCode.Length + phoneNumber.Length) <= this.maxLength)
            {
                if (areaCode.Length > 0)
                    if (phone.Length > 0)
                        phone += "-" + areaCode;
                    else
                  phone = areaCode;
            }
                if (phoneNumber.Length > 0)
                    if (phone.Length > 0)
                        phone += "-" + phoneNumber;
                    else
                        phone = phoneNumber;

            
                return phone.Length>this.maxLength ? phone.Substring(this.maxLength): phone;
        }
        #endregion
        #endregion
    }
}
