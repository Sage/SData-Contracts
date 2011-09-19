#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

#endregion

namespace Sage.Sis.Sdata.Sync.Storage.Syndication
{
    public class SyncDigestInfo : IEnumerable<SyncDigestEntryInfo>
    {
        #region Class Variables

        private List<SyncDigestEntryInfo> _entries = new List<SyncDigestEntryInfo>();

        #endregion

        #region Ctor.

        public SyncDigestInfo()
        {

        }

        #endregion

        public void Add(SyncDigestEntryInfo entry)
        {
            _entries.Add(entry);
        }
        public void AddRange(SyncDigestEntryInfo[] entries)
        {
            _entries.AddRange(entries);
        }
        public void Remove(SyncDigestEntryInfo entry)
        {
            _entries.Remove(entry);
        }
        public void Clear()
        {
            _entries.Clear();
        }

        public SyncDigestEntryInfo[] ToArray()
        {
            return _entries.ToArray();
        }
        public SyncDigestEntryInfo this[int index]  
        {
            get { return _entries[index]; }
            set { _entries[index] = value; }
        }
        public SyncDigestEntryInfo this[string EndPoint]
        {
            get
            {
                SyncDigestEntryInfo entry;
                if (!this.FindByEndPoint(EndPoint, out entry))
                    throw new ArgumentException("No entry with given EndPoint exists.");

                return entry;
            }
        }

        public int Count
        {
            get { return _entries.Count; }
        }

        #region IEnumerable<SyncDigestEntryInfo> Members

        IEnumerator<SyncDigestEntryInfo> IEnumerable<SyncDigestEntryInfo>.GetEnumerator()
        {
            return _entries.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _entries.GetEnumerator();
        }

        #endregion

        #region Private Methods

        private bool FindByEndPoint(string EndPoint, out SyncDigestEntryInfo entry)
        {
            entry = null;

            foreach (SyncDigestEntryInfo info in _entries)
            {
                if (info.EndPoint == EndPoint)
                {
                    entry = info;
                    return true;
                }
            }
            return false;
        }

        #endregion
    }
}
