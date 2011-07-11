#region Usings

using Sage.Integration.Northwind.Common;

#endregion

namespace Sage.Integration.Northwind.Feeds.Paging
{
    public class ItemsPerPageElement : ElementItemBase<int>, IItemsPerPageElement
    {
        #region Ctor.

        public ItemsPerPageElement()
            : this(1)
        {
        }
        public ItemsPerPageElement(int value)
            : base("itemsPerPage", value, Namespaces.opensearchNamespace, Namespaces.opensearchPrefix)
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
