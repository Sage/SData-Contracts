#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Sis.Sdata.Sync.Tick;
using Sage.Sis.Common.Data.OleDb;
using Sage.Sis.Sdata.Sync.Storage.Jet.TableAdapters;
using Sage.Sis.Sdata.Sync.Context;
using Sage.Sis.Sdata.Sync.Storage.Jet.Syndication;

#endregion

namespace Sage.Sis.Sdata.Sync.Storage.Jet
{
    /// <summary>
    /// Tick Provider that increments an Int32 value beginning at 0 and stores the resource kind dependent value into 
    /// to the store.
    /// </summary>
    public class Int32TickProvider : ITickProvider
    {
        private const int STARTTICK = 0;

        #region Class Variables

        private readonly IJetConnectionProvider _jetConnectionProvider;
        private readonly SdataContext _context;

        #endregion

        #region Ctor.

        public Int32TickProvider(IJetConnectionProvider jetConnectionProvider, SdataContext context)
        {
            _jetConnectionProvider = jetConnectionProvider;
            _context = context;

            StoreEnvironment.Initialize(jetConnectionProvider, context);
        }

        #endregion

        internal int CreateNextTick(string resourceKind)
        {
            int nextTick;
            int currentTick;

            ITickTableAdapter tickTableAdapter = StoreEnvironment.Resolve<ITickTableAdapter>(_context);
            IResourceKindTableAdapter resourceKindTableAdapter = StoreEnvironment.Resolve<IResourceKindTableAdapter>(_context);

            using (IJetTransaction jetTransaction = _jetConnectionProvider.GetTransaction(false))
            {
                ResourceKindInfo resourceKindInfo = resourceKindTableAdapter.GetOrCreate(resourceKind, jetTransaction);

                if (!tickTableAdapter.TryGet(resourceKindInfo.Id, out currentTick, jetTransaction))
                {
                    currentTick = STARTTICK;

                    tickTableAdapter.Insert(resourceKindInfo.Id, currentTick, jetTransaction);
                    nextTick = currentTick;
                }
                else
                {
                    currentTick++;
                    tickTableAdapter.Update(resourceKindInfo.Id, currentTick, jetTransaction);

                    nextTick = currentTick;
                }

                jetTransaction.Commit();
            }

            return nextTick;
        }

        #region ITickProvider Members

        object ITickProvider.CreateNextTick(string resourceKind)
        {
            try
            {
                return this.CreateNextTick(resourceKind);
            }
            catch (Exception exception)
            {
                throw new TickCreateException(string.Format("Failed to create next tick."), exception);
            }
        }

        #endregion
    }
}
