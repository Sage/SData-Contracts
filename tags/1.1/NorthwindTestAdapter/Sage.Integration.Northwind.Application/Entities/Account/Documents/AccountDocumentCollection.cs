#region ©2007 Sage Technology Ltd., All Rights Reserved.
/*=====================================================================
	File:			AccountDocumentCollection.cs
	Author:			Philipp Schuette
	DateCreated:	04/24/2007 11:32:24
	DateChanged:	04/24/2007 11:32:24
 ---------------------------------------------------------------------
	©2007 Sage Technology Ltd., All Rights Reserved.	
=====================================================================*/

/*=====================================================================
	Date			        Author		Changes			Reasons
	04/24/2007 11:32:24	pschuette		Create			  
	Changes: Create, Refactoring, Upgrade	
=====================================================================*/
#endregion

#region Usings
using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Northwind.Application.Base;
#endregion

namespace Sage.Integration.Northwind.Application.Entities.Account.Documents
{
    public class AccountDocumentCollection : DocumentCollection
    {

            public AccountDocumentCollection()
                : base("accounts")
            { }

            public override Document GetDocumentTemplate()
            {
                return new AccountDocument();
            }
    }
}
