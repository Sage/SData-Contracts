#region Usings

using System.ComponentModel;

#endregion

namespace Sage.Integration.Northwind.Adapter.Installer
{
    /// <summary>
    /// Custom installer implementation that provides overridable transaction methods called at installation time.
    /// Here: Currently no installation code neccessary. We only define the type of the adapter (NorthwindAdapter).
    /// </summary>
    [RunInstaller(true)]
    public partial class NorthwindInstaller : Sage.Integration.Adapter.AdapterInstaller
    {
        #region Ctor.

        public NorthwindInstaller() : base(typeof(NorthwindAdapter))
        {
            InitializeComponent();
        }

        #endregion
    }
}