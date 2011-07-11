#region Usings

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace Sage.Sis.Sdata.Sync.Context
{
    public class SdataContext
    {
        #region Ctor.

        public SdataContext(string application, string dataSet, string contract, string baseUrl)
        {
            this.Application = application;
            this.DataSet = dataSet;
            this.Contract = contract;
            this.BaseUrl = baseUrl;
        }

        #endregion

        #region Properties

        public string Application { get; private set; }
        public string DataSet { get; private set; }
        public string Contract { get; private set; }
        public string BaseUrl { get; private set; }

        #endregion

        public override string ToString()
        {
            return string.Format("{0}/{1}/{2}", this.Application, this.DataSet, this.Contract);
        }

        public override int GetHashCode()
        {
            return this.Application.GetHashCode() ^ this.DataSet.GetHashCode() ^ this.Contract.GetHashCode();
        }

        public override bool Equals(object obj)
        { 
            if (!(obj is SdataContext))
                return false;
            return Equals((SdataContext)obj);
        }         
        
        public bool Equals(SdataContext other)
        {
            return (this.Contract == other.Contract && this.DataSet == other.DataSet && this.Application == other.Application);
        }

        public bool Equals(string application, string dataSet, string contract)
        {
            return (this.Contract == contract && this.DataSet == dataSet && this.Application == application);
        }
        
        public static bool operator ==(SdataContext context1, SdataContext context2)
        { 
            return context1.Equals(context2);
        }

        public static bool operator !=(SdataContext context1, SdataContext context2)
        {
            return !context1.Equals(context2);
        }
    }
}
