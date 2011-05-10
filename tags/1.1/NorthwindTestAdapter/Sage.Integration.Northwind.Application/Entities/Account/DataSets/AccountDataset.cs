namespace Sage.Integration.Northwind.Application.Entities.Account.DataSets {


    partial class AccountDataset
    {
        partial class AccountsDataTable
        {
            //public System.Data.DataColumn CustomerSupplierFlag
            //{
            //    get
            //    {
            //        return this.;
            //    }
            //}
        }

        
    }
}

namespace Sage.Integration.Northwind.Application.Entities.Account.DataSets.AccountDatasetTableAdapters
{
    public partial class AccountsTableAdapter
    {
        public virtual int FillByWhereClause(AccountDataset.AccountsDataTable dataTable, string whereClause, System.Data.OleDb.OleDbParameter[] parameters)
        {
            System.Data.OleDb.OleDbCommand command = new System.Data.OleDb.OleDbCommand();
            command.Connection = this.Connection;
            command.CommandText = @"select * from 
(Select ""C-"" & CustomerID as ID, ""Customer"" AS CustomerSupplierFlag,
CompanyName, ContactName, ContactTitle, Address, City, Region, PostalCode, Country, Phone, Fax, '' as HomePage, CreateID, CreateUser, ModifyID,  ModifyUser

 from Customers
Union
Select ""S-"" &  SupplierID as ID, ""Supplier"" AS CustomerSupplierFlag,

CompanyName, ContactName, ContactTitle, Address, City, Region, PostalCode, Country, Phone, Fax, HomePage, CreateID, CreateUser, ModifyID,  ModifyUser
FROM         Suppliers) as a "
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
