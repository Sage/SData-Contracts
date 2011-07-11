#region Usings

using Sage.Integration.Northwind.Common;

#endregion

namespace Sage.Integration.Northwind.Feeds.Paging
{
    public class TotalResultsElement : ElementItemBase<int>, ITotalResultsElement
    {
        #region Ctor.

        public TotalResultsElement()
            : this(0)
        {
        }
        public TotalResultsElement(int value)
            : base("totalResults", value, Namespaces.opensearchNamespace, Namespaces.opensearchPrefix)
        {
        }

        #endregion

        protected override bool OnValidateValue(int value, out string errMsg)
        {
            errMsg = null;

            if (!base.OnValidateValue(value, out errMsg))
                return false;

            if (value < 0)
            {
                errMsg = string.Format("The value of {0} cannot be less than 0.", this.LocalElementName);
                return false;
            }

            return true;
        }
    }
}
