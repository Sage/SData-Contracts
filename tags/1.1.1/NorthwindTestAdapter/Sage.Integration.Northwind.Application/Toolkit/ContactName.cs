#region ©2007 Sage Technology Ltd., All Rights Reserved.
/*=====================================================================
	File:			ContactName.cs
	Author:			Philipp Schuette
	DateCreated:	04/04/2007 15:08:24
	DateChanged:	04/04/2007 15:08:24
 ---------------------------------------------------------------------
	©2007 Sage Technology Ltd., All Rights Reserved.	
=====================================================================*/

/*=====================================================================
	Date			        Author		Changes			Reasons
	04/04/2007 15:08:24	pschuette		Create			  
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
    public class ContactName
    {
        #region constructor

        public ContactName()
        {
            this.maxLength = 30;
        }

        #endregion

        #region fields
        private int maxLength;
        private string northwindContacName;
        private string crmSalutation;
        private string crmFirstName;
        private string crmMiddleName;
        private string crmLastName;
        private string crmSuffix;
        #endregion

        #region public properties


        public int MaxLength
        {
            get { return maxLength; }
            set { maxLength = value; }
        }

        public string NorthwindContacName
        {
            get
            {
                northwindContacName = CombinePerson(crmSalutation,
                    crmFirstName, crmMiddleName, crmLastName, crmSuffix);
                return northwindContacName;
            }
            set
            {
                northwindContacName = value;
                SplitPerson(northwindContacName,
                    ref crmSalutation,
                    ref crmFirstName, ref crmMiddleName,
                    ref crmLastName, ref crmSuffix);
                northwindContacName = CombinePerson(crmSalutation,
                    crmFirstName, crmMiddleName, crmLastName, crmSuffix);
            }
        }

        public string CrmSalutation
        {
            get { return crmSalutation; }
            set
            {
                crmSalutation = value;
                northwindContacName = CombinePerson(crmSalutation,
                    crmFirstName, crmMiddleName, crmLastName, crmSuffix);
                SplitPerson(northwindContacName,
                        ref crmSalutation,
                        ref crmFirstName, ref crmMiddleName,
                        ref crmLastName, ref crmSuffix);
            }
        }
        public string CrmFirstName
        {
            get { return crmFirstName; }
            set
            {
                crmFirstName = value;
                northwindContacName = CombinePerson(crmSalutation,
                    crmFirstName, crmMiddleName, crmLastName, crmSuffix);
                SplitPerson(northwindContacName,
                        ref crmSalutation,
                        ref crmFirstName, ref crmMiddleName,
                        ref crmLastName, ref crmSuffix);
            }
        }
        public string CrmMiddleName
        {
            get { return crmMiddleName; }
            set
            {
                crmMiddleName = value;
                northwindContacName = CombinePerson(crmSalutation,
                    crmFirstName, crmMiddleName, crmLastName, crmSuffix);
                SplitPerson(northwindContacName,
                        ref crmSalutation,
                        ref crmFirstName, ref crmMiddleName,
                        ref crmLastName, ref crmSuffix);
            }
        }
        public string CrmLastName
        {
            get { return crmLastName; }
            set
            {
                crmLastName = value;
                northwindContacName = CombinePerson(crmSalutation,
                    crmFirstName, crmMiddleName, crmLastName, crmSuffix);
                SplitPerson(northwindContacName,
                        ref crmSalutation,
                        ref crmFirstName, ref crmMiddleName,
                        ref crmLastName, ref crmSuffix);
            }
        }
        public string CrmSuffix
        {
            get { return crmSuffix; }
            set
            {
                crmSuffix = value;
                northwindContacName = CombinePerson(crmSalutation,
                    crmFirstName, crmMiddleName, crmLastName, crmSuffix);
                SplitPerson(northwindContacName,
                        ref crmSalutation,
                        ref crmFirstName, ref crmMiddleName,
                        ref crmLastName, ref crmSuffix);
            }
        }
        #endregion

        #region public members

        public void SetCrmContact(Property salutation, Property fisrtName, Property middleName,
            Property lastName, Property suffix)
        {
            crmSalutation = "";
            crmFirstName = "";
            crmMiddleName = "";
            crmLastName = "";
            crmSuffix = "";

            if (!salutation.NotSet)
                crmSalutation = (string)salutation.Value;

            if (!fisrtName.NotSet)
                crmFirstName = (string)fisrtName.Value;

            if (!middleName.NotSet)
                crmMiddleName = (string)middleName.Value;

            if (!lastName.NotSet)
                crmLastName = (string)lastName.Value;

            if (!suffix.NotSet)
                crmSuffix = (string)suffix.Value;

            northwindContacName = CombinePerson(crmSalutation,
                    crmFirstName, crmMiddleName, crmLastName, crmSuffix);
            SplitPerson(northwindContacName,
                    ref crmSalutation,
                    ref crmFirstName, ref crmMiddleName,
                    ref crmLastName, ref crmSuffix);

        }

        #endregion

        #region private members
        private void SplitPerson(string northwindContacName, ref string crmSalutation, ref string crmFirstName, ref string crmMiddleName, ref string crmLastName, ref string crmSuffix)
        {
            crmSalutation = string.Empty;
            crmFirstName = string.Empty;
            crmMiddleName = string.Empty;
            crmLastName = northwindContacName;
            crmSuffix = string.Empty;
            if ((northwindContacName != null) &&
                (northwindContacName.Contains(" ")))
            {
                string[] parts = northwindContacName.Split(new char[] { ' ' });
                if (parts.Length == 2)
                {
                    crmFirstName = parts[0];
                    crmLastName = parts[1];
                }
                else if (parts.Length == 3)
                {
                    crmFirstName = parts[0];
                    crmMiddleName = parts[1];
                    crmLastName = parts[2];
                }
            }
        }

        private string CombinePerson(string salutation, string firstName, string middleName, string lastName, string suffix)
        {
            string result;
            salutation = (salutation == null) ? "" : salutation;
            firstName = (firstName == null) ? "" : firstName;
            middleName = (middleName == null) ? "" : middleName;
            lastName = (lastName == null) ? "" : lastName;
            suffix = (suffix == null) ? "" : suffix;

            result = "";
            result += (salutation.Length > 0) ? salutation + " " : "";
            result += (firstName.Length > 0) ? firstName + " " : "";
            result += (middleName.Length > 0) ? middleName + " " : "";
            result += (lastName.Length > 0) ? lastName + " " : "";
            result += (suffix.Length > 0) ? suffix + " " : "";
            result = result.Trim();
            if (result.Length <= maxLength)
                return result;


            result = "";
            result += (firstName.Length > 0) ? firstName + " " : "";
            result += (middleName.Length > 0) ? middleName + " " : "";
            result += (lastName.Length > 0) ? lastName + " " : "";
            result += (suffix.Length > 0) ? suffix + " " : "";
            result = result.Trim();
            if (result.Length <= maxLength)
                return result;

            result = "";
            result += (firstName.Length > 0) ? firstName + " " : "";
            result += (middleName.Length > 0) ? middleName.Substring(0, 1) + ". " : "";
            result += (lastName.Length > 0) ? lastName + " " : "";
            result += (suffix.Length > 0) ? suffix + " " : "";
            result = result.Trim();
            if (result.Length <= maxLength)
                return result;


            result = "";
            result += (firstName.Length > 0) ? firstName + " " : "";
            result += (lastName.Length > 0) ? lastName + " " : "";
            result += (suffix.Length > 0) ? suffix + " " : "";
            result = result.Trim();
            if (result.Length <= maxLength)
                return result;


            result = "";
            result += (firstName.Length > 0) ? firstName + " " : "";
            result += (lastName.Length > 0) ? lastName + " " : "";
            result = result.Trim();
            if (result.Length <= maxLength)
                return result;

            result = "";
            result += lastName;
            result = result.Trim();
            return result.Substring(0, maxLength);
        }

        #endregion
    }
}
