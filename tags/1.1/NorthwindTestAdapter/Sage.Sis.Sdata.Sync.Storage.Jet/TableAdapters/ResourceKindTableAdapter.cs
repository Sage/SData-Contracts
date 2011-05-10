#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Sis.Sdata.Sync.Storage.Jet.Syndication;
using System.Data.OleDb;
using Sage.Sis.Common.Data.OleDb;
using System.Data;
using Sage.Sis.Sdata.Sync.Storage.Jet.Tables;
using Sage.Sis.Sdata.Sync.Context;

#endregion

namespace Sage.Sis.Sdata.Sync.Storage.Jet.TableAdapters
{
    internal class ResourceKindTableAdapter : IResourceKindTableAdapter
    {
        #region Class Variables

        private readonly IResourceKindTable _resourceKindTable;

        #endregion

        #region Ctor.

        public ResourceKindTableAdapter(IResourceKindTable resourceKindTable, SdataContext context)
        {
            _resourceKindTable = resourceKindTable;
            this.Context = context;
        }

        #endregion

        #region IResourceKindTableAdapter Members

        public ResourceKindInfo GetOrCreate(string resourceKind, IJetTransaction jetTransaction)
        {
            ResourceKindInfo resourceKindInfo = null;

            OleDbCommand oleDbCommand = jetTransaction.CreateOleCommand();

            oleDbCommand.CommandText = string.Format("SELECT [ID] FROM {0} WHERE [Name]=@Name;", _resourceKindTable.TableName);
            oleDbCommand.Parameters.AddWithValue("@Name", resourceKind);

            OleDbDataReader reader = oleDbCommand.ExecuteReader(CommandBehavior.SingleRow);

            if (!reader.HasRows)
            {
                resourceKindInfo = this.Add(resourceKind, jetTransaction);
            }
            else
            {
                reader.Read();
            
                int id = Convert.ToInt32(reader["ID"]);
                resourceKindInfo = new ResourceKindInfo(id, resourceKind);
            }
            

            return resourceKindInfo;            
        }

        public ResourceKindInfo[] GetAll(IJetTransaction jetTransaction)
        {
            List<ResourceKindInfo> resultInfos = new List<ResourceKindInfo>();
            OleDbCommand oleDbCommand = jetTransaction.CreateOleCommand();

            oleDbCommand.CommandText = string.Format("SELECT [ID], [Name] FROM {0};", _resourceKindTable.TableName);

            OleDbDataReader reader = oleDbCommand.ExecuteReader();

            int tmpId;
            string tmpName;
            while (reader.Read())
            {
                tmpId = Convert.ToInt32(reader["ID"]);
                tmpName = Convert.ToString(reader["Name"]);
                resultInfos.Add(new ResourceKindInfo(tmpId, tmpName));
            }

            return resultInfos.ToArray();
        }

        public IResourceKindTable Table { get { return _resourceKindTable; } }

        #endregion

        #region ITableAdapter Members

        public SdataContext Context { get; private set; }

        ITable ITableAdapter.Table { get { return this.Table; } }

        #endregion

        #region Private Helpers

        private ResourceKindInfo Add(string resourceKind, IJetTransaction jetTransaction)
        {
            OleDbCommand oleDbCommand = jetTransaction.CreateOleCommand();
            oleDbCommand.CommandText = string.Format("INSERT INTO [{0}] ([Name]) VALUES (@Name);", _resourceKindTable.TableName);

            oleDbCommand.Parameters.AddWithValue("@Name", resourceKind);

            try
            {
                oleDbCommand.ExecuteNonQuery();
            }
            catch (OleDbException exception)
            {
                if (exception.Errors.Count == 1 && exception.Errors[0].SQLState == "3022")
                    throw new StoreException("An error occured while adding a new resource kind. The resourcekind already exists for the resource kind '{0}'.", exception);

                throw;
            }

            oleDbCommand.Parameters.Clear();

            oleDbCommand.CommandText = string.Format("SELECT @@IDENTITY FROM [{0}];", _resourceKindTable.TableName);
            oleDbCommand.CommandType = CommandType.Text;

            int newId = (int)oleDbCommand.ExecuteScalar();

            return new ResourceKindInfo(newId, resourceKind);
        }

        #endregion
    }
}
