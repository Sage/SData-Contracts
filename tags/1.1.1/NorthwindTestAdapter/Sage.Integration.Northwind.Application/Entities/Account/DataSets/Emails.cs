namespace Sage.Integration.Northwind.Application.Entities.Account.DataSets
{
}
namespace Sage.Integration.Northwind.Application.Entities.Account.DataSets
{
}
namespace Sage.Integration.Northwind.Application.Entities.Account.DataSets
{
}
namespace Sage.Integration.Northwind.Application.Entities.Account.DataSets
{
}
namespace Sage.Integration.Northwind.Application.Entities.Account.DataSets
{
}
namespace Sage.Integration.Northwind.Application.Entities.Account.DataSets
{
}
namespace Sage.Integration.Northwind.Application.Entities.Account.DataSets
{
}
namespace Sage.Integration.Northwind.Application.Entities.Account.DataSets
{
}
namespace Sage.Integration.Northwind.Application.Entities.Account.DataSets
{
}
namespace Sage.Integration.Northwind.Application.Entities.Account.DataSets
{
}
namespace Sage.Integration.Northwind.Application.Entities.Account.DataSets
{
}
namespace Sage.Integration.Northwind.Application.Entities.Account.DataSets
{
}
namespace Sage.Integration.Northwind.Application.Entities.Account.DataSets
{
}
namespace Sage.Integration.Northwind.Application.Entities.Account.DataSets
{
}
namespace Sage.Integration.Northwind.Application.Entities.Account.DataSets
{
}
namespace Sage.Integration.Northwind.Application.Entities.Account.DataSets
{
}
namespace Sage.Integration.Northwind.Application.Entities.Account.DataSets
{
}
namespace Sage.Integration.Northwind.Application.Entities.Account.DataSets
{
}
namespace Sage.Integration.Northwind.Application.Entities.Account.DataSets
{
}
namespace Sage.Integration.Northwind.Application.Entities.Account.DataSets
{
}
namespace Sage.Integration.Northwind.Application.Entities.Account.DataSets
{
}
namespace Sage.Integration.Northwind.Application.Entities.Account.DataSets
{
}
namespace Sage.Integration.Northwind.Application.Entities.Account.DataSets
{
}
namespace Sage.Integration.Northwind.Application.Entities.Account.DataSets
{
}
namespace Sage.Integration.Northwind.Application.Entities.Account.DataSets
{
}
namespace Sage.Integration.Northwind.Application.Entities.Account.DataSets
{
}
namespace Sage.Integration.Northwind.Application.Entities.Account.DataSets
{
}
namespace Sage.Integration.Northwind.Application.Entities.Account.DataSets
{
}
namespace Sage.Integration.Northwind.Application.Entities.Account.DataSets.EmailsTableAdapters
{
    public partial class CustomerEmailsTableAdapter
    {
        public virtual int FillByWhereClause(Emails.CustomerEmailsDataTable dataTable, string whereClause, System.Data.OleDb.OleDbParameter[] parameters)
        {
            System.Data.OleDb.OleDbCommand command = new System.Data.OleDb.OleDbCommand();
            command.Connection = this.Connection;
            this._commandCollection[0].CommandText = @"SELECT ID, Email, CustomerID, CreateID, CreateUser, ModifyID, ModifyUser FROM CustomerEmails "
                + whereClause + ";";

            command.CommandType = System.Data.CommandType.Text;
            
            if (null != parameters)
                command.Parameters.AddRange(parameters);

            this.Adapter.SelectCommand = command;
            if ((this.ClearBeforeFill == true))
            {
                dataTable.Clear();
            }
            int returnValue = this.Adapter.Fill(dataTable);
            return returnValue;
        }
    }
}
