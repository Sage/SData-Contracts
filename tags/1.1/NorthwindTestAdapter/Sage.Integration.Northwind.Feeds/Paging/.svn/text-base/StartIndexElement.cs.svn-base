#region Usings

using Sage.Integration.Northwind.Common;

#endregion

namespace Sage.Integration.Northwind.Feeds.Paging
{
    public class StartIndexElement: ElementItemBase<int>, IStartIndexElement
    {
        #region Ctor.

        public StartIndexElement()
            : this(1)
        {
        }
        public StartIndexElement(int value)
            : base("startIndex", value, Namespaces.opensearchNamespace, Namespaces.opensearchPrefix)
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
