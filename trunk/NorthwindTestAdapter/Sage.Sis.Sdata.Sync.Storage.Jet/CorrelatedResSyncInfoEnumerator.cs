#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Sis.Sdata.Sync.Storage.Syndication;
using System.Collections;

#endregion

namespace Sage.Sis.Sdata.Sync.Storage.Jet
{
    public class CorrelatedResSyncInfoEnumerator : ICorrelatedResSyncInfoEnumerator
    {
        #region Class Variables

        private readonly CorrelatedResSyncInfo[] _infos;
        
        // Enumerators are positioned before the first element
        // until the first MoveNext() call.
        int _position = -1;


        #endregion

        #region Ctor.

        internal CorrelatedResSyncInfoEnumerator(CorrelatedResSyncInfo[] infos)
        {
            _infos = infos;
        }

        #endregion

        #region IEnumerator<CorrelatedResSyncInfo> Members

        public CorrelatedResSyncInfo Current
        {
            get
            {
                try
                {
                    return _infos[_position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion

        #region IEnumerator Members

        object IEnumerator.Current
        {
            get
            {
                return this.Current;
            }
        }

        public bool MoveNext()
        {
            _position++;
            return (_position < _infos.Length);
        }

        public void Reset()
        {
            _position = -1;

        }

        #endregion
    }
}
