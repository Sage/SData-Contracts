namespace Sage.Integration.Northwind.Application.Entities.Product.DataSets.CategoryTableAdapters
{
    public partial class CategoriesTableAdapter
    {
        public virtual int FillByWhereClause(Category.CategoriesDataTable dataTable, string whereClause, System.Data.OleDb.OleDbParameter[] parameters)
        {
            System.Data.OleDb.OleDbCommand command = new System.Data.OleDb.OleDbCommand();
            command.Connection = this.Connection;
            command.CommandText = @"SELECT CategoryID, CategoryName, Description, 
Picture, CreateID, CreateUser, ModifyID, ModifyUser FROM Categories " +
                whereClause + ";";
            
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
