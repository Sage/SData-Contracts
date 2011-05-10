using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OleDb;

namespace Sage.Integration.Northwind.Adapter.Data.SalesOrders.DataSets.OrderTableAdapters
{
    partial class Order_DetailsTableAdapter
    {
        public void SetTransaction(OleDbTransaction trans)
        {


            foreach (OleDbCommand cmd in this.CommandCollection)
            {

                if (cmd != null)

                    cmd.Transaction = trans;

            }

            if ((Adapter.InsertCommand != null))
            {

                Adapter.InsertCommand.Transaction = trans;

            }

            if ((Adapter.DeleteCommand != null))
            {

                Adapter.DeleteCommand.Transaction = trans;

            }

            if ((Adapter.UpdateCommand != null))
            {

                Adapter.UpdateCommand.Transaction = trans;

            }

            if ((Adapter.SelectCommand != null))
            {

                Adapter.SelectCommand.Transaction = trans;

            }






        }
    }
    partial class OrdersTableAdapter
    {
        public void SetTransaction(OleDbTransaction trans)
        {


            foreach (OleDbCommand cmd in this.CommandCollection)
            {

                if (cmd != null)

                    cmd.Transaction = trans;

            }

            if ((Adapter.InsertCommand != null))
            {

                Adapter.InsertCommand.Transaction = trans;

            }

            if ((Adapter.DeleteCommand != null))
            {

                Adapter.DeleteCommand.Transaction = trans;

            }

            if ((Adapter.UpdateCommand != null))
            {

                Adapter.UpdateCommand.Transaction = trans;

            }

            if ((Adapter.SelectCommand != null))
            {

                Adapter.SelectCommand.Transaction = trans;

            }




            

        }

    }
}
