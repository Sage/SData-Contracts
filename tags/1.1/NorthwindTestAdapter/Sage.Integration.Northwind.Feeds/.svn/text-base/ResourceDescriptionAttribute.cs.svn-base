#region Usings

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace Sage.Integration.Northwind.Feeds
{
    [global::System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class ResourceDescriptionAttribute : Attribute
    {
        public ResourceDescriptionAttribute(string name, string description, bool canGet)
        {
            this.Name = name;
            this.Description = description;
            this.CanGet = canGet;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public bool CanGet { get; set; }

    }
}
