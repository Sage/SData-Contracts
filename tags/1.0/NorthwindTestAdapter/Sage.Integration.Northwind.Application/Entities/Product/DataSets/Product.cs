namespace Sage.Integration.Northwind.Application.Entities.Product.DataSets.ProductTableAdapters
{
    public partial class ProductsTableAdapter
    {
        public virtual int FillByWhereClause(Product.ProductsDataTable dataTable, string whereClause, System.Data.OleDb.OleDbParameter[] parameters)
        {
            System.Data.OleDb.OleDbCommand command = new System.Data.OleDb.OleDbCommand();
            command.Connection = this.Connection;
            command.CommandText = "SELECT ProductID, ProductName, SupplierID, CategoryID, QuantityPerUnit, UnitPrice" +
                ", UnitsInStock, UnitsOnOrder, ReorderLevel, Discontinued, CreateID, CreateUser, " +
                "ModifyID, ModifyUser FROM Products " + whereClause + ";";
            
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
namespace Sage.Integration.Northwind.Application.Entities.Product.DataSets {
    
    
    public partial class Product {
        partial class ChangeLogsDataTable
        {
        }
    }
}
