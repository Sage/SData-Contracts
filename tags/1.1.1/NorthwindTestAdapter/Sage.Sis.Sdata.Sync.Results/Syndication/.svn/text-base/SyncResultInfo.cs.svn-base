#region Usings

using System;
using System.Collections;
using System.Collections.Generic;

#endregion

namespace Sage.Sis.Sdata.Sync.Results.Syndication
{
    public class SyncResultInfo : IEnumerable<SyncResultEntryInfo>
    {
        #region Class Variables

        private List<SyncResultEntryInfo> _entries = new List<SyncResultEntryInfo>();

        #endregion

        #region Properties

        public DateTime Timestamp { get; private set; }
        public string TrackingId { get; private set; }

        #endregion

        #region Ctor.

        public SyncResultInfo(string trackingId)
        {
            this.Timestamp = DateTime.Now;
            this.TrackingId = trackingId;
        }

        #endregion

        public void Add(SyncResultEntryInfo entry)
        {
            _entries.Add(entry);
        }
        public void AddRange(SyncResultEntryInfo[] entries)
        {
            _entries.AddRange(entries);
        }

        public SyncResultEntryInfo[] ToArray()
        {
            return _entries.ToArray();
        }
        public SyncResultEntryInfo this[int index]
        {
            get { return _entries[index]; }
            set { _entries[index] = value; }
        }

        public int Count
        {
            get { return _entries.Count; }
        }

        #region IEnumerable<SyncDigestEntryInfo> Members

        IEnumerator<SyncResultEntryInfo> IEnumerable<SyncResultEntryInfo>.GetEnumerator()
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
    }
}
