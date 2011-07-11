#region Usings

using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.IO;
using System.Reflection; 
using Microsoft.Win32;
using System.ComponentModel;
using Sage.Integration.Server;
using Sage.Integration.Diagnostics;
using Sage.Integration.Messaging.Model;
using Sage.Common.Syndication;
using Sage.Integration.Northwind.Adapter.Common;
using Sage.Integration.Northwind.Sync;

#endregion

namespace Sage.Integration.Northwind.Adapter
{
	[RequestPath(Common.Constants.RootPath)]
	[Description(Common.Constants.AdapterDescription)]
	public class NorthwindAdapter : Sage.Integration.Adapter.Adapter
	{
        #region Ctor.

        public NorthwindAdapter()
		{
            this.StoreLocator = new StoreLocator();
            this.RequestPerformerLocator = new RequestPerformerLocator();
        }

        #endregion

        #region Start / Stop

        /// <summary>
		/// Called when service is starting.
		/// </summary>
		/// <param name="args">Data passed by the caller.</param>
		protected override void OnStart(string [] args)
		{ 
			base.OnStart(args);
		}
		
		/// <summary>
		/// Called when service is stopping
		/// </summary>
		protected override void OnStop()
		{
			
			base.OnStop();
		}
		
		#endregion

        #region Properties

        internal StoreLocator StoreLocator { get; private set; }
        internal RequestPerformerLocator RequestPerformerLocator { get; private set; }

        #endregion
    }
}
