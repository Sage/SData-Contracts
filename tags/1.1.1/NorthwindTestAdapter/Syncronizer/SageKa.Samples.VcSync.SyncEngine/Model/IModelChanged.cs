using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SageKa.Samples.VcSync.SyncEngine.Model
{
    public interface IModelChanged
    {
        bool Changed { get; set; }
    }
}
