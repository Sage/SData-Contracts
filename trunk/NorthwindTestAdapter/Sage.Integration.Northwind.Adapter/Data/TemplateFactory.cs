using System;
using System.Collections.Generic;
using System.Text;
using Sage.Common.Syndication;
using Sage.Integration.Messaging.Model;
using Sage.Integration.Northwind.Adapter.Feeds;
using System.Reflection;

namespace Sage.Integration.Northwind.Adapter.Data
{
    public class TemplateFactory
    {
        public static FeedEntry GetTemplate<T>(IRequest request) where T : FeedEntry, new()
        {
            T template = new T();
            template.Id = template.Uri = request.Uri.ToString();
            template.Updated = DateTime.Now;

            if (template is TradingAccountFeedEntry)
            {
                /*(template as TradingAccountFeedEntry).customerSupplierFlag = "Customer";
                return template;*/
                return new TradingAccountFeedEntryTemplate();
            }

            else if (template is ContactFeedEntry)
            {
                return new ContactFeedEntryTemplate();
            }

            else if (template is SalesOrderFeedEntry)
            {
                return new SalesOrderFeedEntryTemplate();
            }

            else if (template is CommodityGroupFeedEntry)
            {
                return new CommodityGroupFeedEntryTemplate();
            }

            return template;
        }
    }

    public class IgnoreOnTemplateAttribute : Attribute { }
}
