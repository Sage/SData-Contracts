#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Sis.Sdata.Sync.Storage.Jet.Tables;
using Sage.Sis.Sdata.Sync.Context;
using Sage.Sis.Common.Data.OleDb;
using System.Data.OleDb;
using System.Data;

#endregion

namespace Sage.Sis.Sdata.Sync.Storage.Jet.TableAdapters
{
    internal class tickTableAdapter : ItickTableAdapter
    {
        #region Class Variables

        private readonly ItickTable _tickTable;

        #endregion

        #region Ctor.

        public tickTableAdapter(ItickTable tickTable, SdataContext context)
        {
            _tickTable = tickTable;
            this.Context = context;
        }

        #endregion

        #region ItickTableAdapter Members

        public bool TryGet(int resourceKindId, out int tick, IJetTransaction jetTransaction)
        {
            tick = int.MinValue;

            OleDbCommand oleDbCommand = jetTransaction.CreateOleCommand();

            string sqlQuery = string.Empty;

            sqlQuery = "SELECT [FKResourceKindId], [tick] FROM {0} WHERE ([FKResourceKindId]=@ResourceKindId);";
            oleDbCommand.CommandText = string.Format(sqlQuery, _tickTable.TableName);
            oleDbCommand.Parameters.AddWithValue("@ResourceKindId", resourceKindId);

            using (OleDbDataReader reader = oleDbCommand.ExecuteReader(CommandBehavior.SingleRow))
            {
                if (reader.Read())
                {
                    tick = Convert.ToInt32(reader["tick"]);

                    return true;
                }
            }
            return false;
        }
        
        public void Update(int resourceKindId, int tick, IJetTransaction jetTransaction)
        {
            OleDbCommand oleDbCommand = jetTransaction.CreateOleCommand();

            string sqlQuery = string.Empty;
            sqlQuery = "UPDATE [{0}] SET [tick]=@tick WHERE (FKResourceKindId=@ResourceKindId);";

            oleDbCommand.CommandText = string.Format(sqlQuery, _tickTable.TableName);

            oleDbCommand.Parameters.AddWithValue("@tick", tick);
            oleDbCommand.Parameters.AddWithValue("@ResourceKindId", resourceKindId);

            oleDbCommand.ExecuteNonQuery();
        }

        public void Insert(int resourceKindId, int tick, IJetTransaction jetTransaction)
        {
            OleDbCommand oleDbCommand = jetTransaction.CreateOleCommand();

            string sqlQuery = string.Empty;
            sqlQuery = "INSERT INTO [{0}] ([tick], [FKResourceKindId]) VALUES (@tick, @ResourceKindId);";

            oleDbCommand.CommandText = string.Format(sqlQuery, _tickTable.TableName);

            oleDbCommand.Parameters.AddWithValue("@tick", tick);
            oleDbCommand.Parameters.AddWithValue("@ResourceKindId", resourceKindId);

            oleDbCommand.ExecuteNonQuery();
        }

        public ItickTable Table { get { return _tickTable; } }
        
        #endregion

        #region ITableAdapter Members

        public SdataContext Context { get; private set; }
        ITable ITableAdapter.Table { get { return this.Table; } }

        #endregion
    }
}
