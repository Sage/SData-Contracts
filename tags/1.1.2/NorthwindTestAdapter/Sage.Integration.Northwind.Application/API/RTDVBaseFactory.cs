#region ©2007 Sage Technology Ltd., All Rights Reserved.
/*=====================================================================
    File:        RealtimeDataViewingBaseFactory.cs
    Author:      msassanelli
    DateCreated: 04-27-2007
    DateChanged: 04-27-2007
    ---------------------------------------------------------------------
    ©2007 Sage Technology Ltd., All Rights Reserved. 
=====================================================================*/

/*=====================================================================
    Date        Author       Changes     Reasons
    04-27-2007  msassanelli  Create   
    

    Changes: Create, Refactoring, Upgrade 
=====================================================================*/
#endregion

#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Northwind.Application.API;
using Sage.Integration.Northwind.Application.Base;
using Sage.Integration.Northwind.Application.RTDV;

#endregion

namespace Sage.Integration.Northwind.Application.API
{
    public class RTDVFactory
    {
        public static RTDVBase GetRTDV(string reportName)
        {
            switch (reportName)
            {
                case Constants.RTDVNames.SalesInvoices:
                    return new RTDVSalesInvoices();

                case Constants.RTDVNames.AdditionalStockDetails:
                    return new RTDVAdditionalStockDetails();

                case Constants.RTDVNames.ProductsPurchased:
                    return new RTDVProductsPurchased();

                default:
                    return null;
            }
        }

        public static RTDVBase[] GetRTDVAll()
        {
            RTDVBase[] results;

            string[] rtdvNames = Constants.RTDVNames.GetValues();

            results = new RTDVBase[rtdvNames.Length];
            int i = 0;
            foreach (string name in rtdvNames)
                results[i++] = GetRTDV(name);

            return results;
        }
    }
}
