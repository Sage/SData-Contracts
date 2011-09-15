#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Sis.Sdata.Sync;
using Sage.Sis.Common.Data.OleDb;
using Sage.Sis.Sdata.Sync.Storage.Jet.TableAdapters;
using Sage.Sis.Sdata.Sync.Context;
using Sage.Sis.Sdata.Sync.Storage.Jet.Syndication;

#endregion

namespace Sage.Sis.Sdata.Sync.Storage.Jet
{
    /// <summary>
    /// tick Provider that increments an Int32 value beginning at 0 and stores the resource kind dependent value into 
    /// to the store.
    /// </summary>
    public class Int32tickProvider : Sage.Sis.Sdata.Sync.tick.ItickProvider
    {
        private const int STARTtick = 0;

        #region Class Variables

        private readonly IJetConnectionProvider _jetConnectionProvider;
        private readonly SdataContext _context;

        #endregion

        #region Ctor.

        public Int32tickProvider(IJetConnectionProvider jetConnectionProvider, SdataContext context)
        {
            _jetConnectionProvider = jetConnectionProvider;
            _context = context;

            StoreEnvironment.Initialize(jetConnectionProvider, context);
        }

        #endregion

        internal int CreateNexttick(string resourceKind)
        {
            int nexttick;
            int currenttick;

            ItickTableAdapter tickTableAdapter = StoreEnvironment.Resolve<ItickTableAdapter>(_context);
            IResourceKindTableAdapter resourceKindTableAdapter = StoreEnvironment.Resolve<IResourceKindTableAdapter>(_context);

            using (IJetTransaction jetTransaction = _jetConnectionProvider.GetTransaction(false))
            {
                ResourceKindInfo resourceKindInfo = resourceKindTableAdapter.GetOrCreate(resourceKind, jetTransaction);

                if (!tickTableAdapter.TryGet(resourceKindInfo.Id, out currenttick, jetTransaction))
                {
                    currenttick = STARTtick;

                    tickTableAdapter.Insert(resourceKindInfo.Id, currenttick, jetTransaction);
                    nexttick = currenttick;
                }
                else
                {
                    currenttick++;
                    tickTableAdapter.Update(resourceKindInfo.Id, currenttick, jetTransaction);

                    nexttick = currenttick;
                }

                jetTransaction.Commit();
            }

            return nexttick;
        }

        #region ItickProvider Members

        object Sage.Sis.Sdata.Sync.tick.ItickProvider.CreateNexttick(string resourceKind)
        {
            try
            {
                return this.CreateNexttick(resourceKind);
            }
            catch (Exception exception)
            {
                throw new Sage.Sis.Sdata.Sync.tick.tickCreateException(string.Format("Failed to create next tick."), exception);
            }
        }

        #endregion
    }
}
