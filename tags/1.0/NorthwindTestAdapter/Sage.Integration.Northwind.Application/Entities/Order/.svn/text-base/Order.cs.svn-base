#region Usings

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using Sage.Integration.Northwind.Application.API;
using Sage.Integration.Northwind.Application.Base;
using Sage.Integration.Northwind.Application.Entities.Order.DataSets.OrderTableAdapters;
using Sage.Integration.Northwind.Application.Entities.Order.Documents;
using Sage.Integration.Northwind.Application.Properties;
using Sage.Integration.Northwind.Application.Toolkit;

#endregion

namespace Sage.Integration.Northwind.Application.Entities.Order
{
    public class Order : EntityBase
    {
        #region Ctor.

        public Order()
            : base(Constants.EntityNames.Order)
        {
        }

        #endregion

        #region fields

        #endregion

        #region public properties

        #endregion

        #region public members

        #endregion

        #region private members

        #endregion




        public override void Add(Document doc, NorthwindConfig config, ref List<TransactionResult> result)
        {
            TransactionResult tmpTransactionResult;
            List<TransactionResult> transactionResult = new List<TransactionResult>();
            OrderDocument orderDoc = doc as OrderDocument;

            #region check input values
            if (orderDoc == null)
            {
                result.Add(doc.SetTransactionStatus(TransactionStatus.UnRecoverableError, Resources.ErrorMessages_DocumentTypeNotSupported));
                return;
            }

            string customerID;
            if (orderDoc.accountid.IsNull)
            {

                result.Add(orderDoc.SetTransactionStatus(TransactionStatus.UnRecoverableError, Resources.ErrorMessages_AccountIDMadatory));
                return;
            }

            customerID = (string)orderDoc.accountid.Value;
            if (!customerID.StartsWith(Constants.CustomerIdPrefix))
            {
                result.Add(orderDoc.SetTransactionStatus(TransactionStatus.UnRecoverableError, Resources.ErrorMessages_AddOrdersForCustomersOnly));
                return;
            }
            #endregion

            DataSets.OrderTableAdapters.OrdersTableAdapter tableAdapter;
            DataSets.OrderTableAdapters.Order_DetailsTableAdapter detailsTableAdapter;

            DataSets.Order order = new DataSets.Order();

            DataSets.Order.OrdersRow newOrder = order.Orders.NewOrdersRow();


            customerID = customerID.Substring(Constants.CustomerIdPrefix.Length);
            newOrder.CustomerID = customerID;

            #region get Company Name
            DataSet dataSet = new DataSet();
            OleDbDataAdapter dataAdapter;
            string sqlQuery = "Select CompanyName from Customers where CustomerID = '" + customerID + "'";

            try
            {
                using (OleDbConnection connection = new OleDbConnection(config.ConnectionString))
                {
                    dataAdapter = new OleDbDataAdapter(sqlQuery, connection);
                    if (dataAdapter.Fill(dataSet, "Customers") == 0)
                    {
                        result.Add(orderDoc.SetTransactionStatus(TransactionStatus.UnRecoverableError, Resources.ErrorMessages_AccountNotFound));
                        return;
                    }
                    newOrder.ShipName = dataSet.Tables[0].Rows[0][0].ToString();
                }
            }
            catch (Exception e)
            {
                orderDoc.Id = "";
                throw;
            }

            #endregion


            #region get Sels rep
            if (orderDoc.salesrepr.IsNull)
                newOrder.SetEmployeeIDNull();
            else
            {
                try
                {
                    newOrder.EmployeeID = int.Parse((string)orderDoc.salesrepr.Value);
                }
                catch (Exception)
                {
                    newOrder.SetEmployeeIDNull();
                }
                if (newOrder.IsEmployeeIDNull())
                {
                    try
                    {
                        dataSet = new DataSet();
                        sqlQuery = "SELECT Employees.EmployeeID FROM Employees where Employees.FirstName + ' ' + Employees.LastName = ? ";
                        using (OleDbConnection connection = new OleDbConnection(config.ConnectionString))
                        {
                            dataAdapter = new OleDbDataAdapter(sqlQuery, connection);
                            OleDbParameter parameter = new OleDbParameter("Name", (string)orderDoc.salesrepr.Value);
                            dataAdapter.SelectCommand.Parameters.Add(parameter);
                            if (dataAdapter.Fill(dataSet, "Employees") > 0)
                                newOrder.EmployeeID = Convert.ToInt32(dataSet.Tables[0].Rows[0][0]);
                            else
                                newOrder.EmployeeID = 1;
                        }
                    }
                    catch (Exception e)
                    {
                        orderDoc.Id = "";
                        throw;
                    }

                }
            }
            #endregion

            #region fill dataset from document
            try
            {

                if (orderDoc.opened.IsNull)
                    newOrder.SetOrderDateNull();
                else
                    newOrder.OrderDate = (DateTime)orderDoc.opened.Value;

                if (orderDoc.deliverydate.IsNull)
                    newOrder.SetRequiredDateNull();
                else
                    newOrder.RequiredDate = (DateTime)orderDoc.deliverydate.Value;

                if (orderDoc.shippedvia.IsNull)
                    newOrder.SetShipViaNull();
                else
                    newOrder.ShipVia = (int)orderDoc.shippedvia.Value;


                if (orderDoc.shipaddress.IsNull)
                {
                    newOrder.SetShipAddressNull();
                    newOrder.SetShipCityNull();
                    newOrder.SetShipCountryNull();
                    newOrder.SetShipPostalCodeNull();
                }
                else
                {
                    OrderAddress orderAddress = new OrderAddress();
                    orderAddress.CrmOrderAddress = (string)orderDoc.shipaddress.Value;
                    newOrder.ShipAddress = (string)orderAddress.NorthwindAddress;
                    newOrder.ShipCity = (string)orderAddress.NorthwindCity;
                    newOrder.ShipPostalCode = (string)orderAddress.NorthwindZipCode;
                    newOrder.ShipCountry = (string)orderAddress.NorthwindCountry;

                }


                if (orderDoc.freight.IsNull)
                    newOrder.Freight = (decimal)0;
                else
                    newOrder.Freight = (decimal)orderDoc.freight.Value;

                newOrder.CreateUser = config.CrmUser;
                newOrder.ModifyUser = config.CrmUser;
                newOrder.CreateID = config.SequenceNumber;
                newOrder.ModifyID = config.SequenceNumber;
            }
            catch (Exception e)
            {
                orderDoc.Id = "";
#warning Check error message
                result.Add(orderDoc.SetTransactionStatus(TransactionStatus.UnRecoverableError, e.ToString()));
                return;
            }

            #endregion

            using (OleDbConnection connection = new OleDbConnection(config.ConnectionString))
            {
                OleDbTransaction transaction = null;
                try
                {
                    connection.Open();
                    transaction = connection.BeginTransaction();

                    tableAdapter = new DataSets.OrderTableAdapters.OrdersTableAdapter();
                    tableAdapter.Connection = connection;
                    detailsTableAdapter = new DataSets.OrderTableAdapters.Order_DetailsTableAdapter();
                    detailsTableAdapter.Connection = connection;

                    tableAdapter.SetTransaction(transaction);
                    detailsTableAdapter.SetTransaction(transaction);
                    order.Orders.AddOrdersRow(newOrder);
                    tableAdapter.Update(order.Orders);
                    OleDbCommand Cmd = new OleDbCommand("SELECT @@IDENTITY", connection);
                    Cmd.Transaction = transaction;
                    object lastid = Cmd.ExecuteScalar();
                    orderDoc.Id = ((int)lastid).ToString();
                    // add line Items

                    DataSets.Order.Order_DetailsRow detailRow;

                    Hashtable addedProductsProducts;
                    addedProductsProducts = new Hashtable();
                    int productID;


                    foreach (LineItemDocument lineItemDoc in orderDoc.orderitems)
                    {
                        try
                        {
                            try
                            {
                                productID = (int)lineItemDoc.productid.Value;
                            }
                            catch (Exception)
                            {
#warning only to test unsupported products  
                                productID = 0;
                            }
                            if (addedProductsProducts.Contains(productID))
                            {
                                transaction.Rollback();
                                orderDoc.Id = "";
                                result.Add(orderDoc.SetTransactionStatus(TransactionStatus.UnRecoverableError, Resources.ErrorMessages_OrderContainsProductTwice));
                                return;
                            }
                            addedProductsProducts.Add(productID, productID);
                            detailRow = order.Order_Details.NewOrder_DetailsRow();
                            lineItemDoc.Id = orderDoc.Id + "-" + productID.ToString();
                            detailRow.OrderID = Convert.ToInt32(orderDoc.Id);
                            detailRow.ProductID = productID;
                            detailRow.Quantity = Convert.ToInt16(lineItemDoc.quantity.Value);
                            detailRow.UnitPrice = (Decimal)lineItemDoc.listprice.Value;




                            if ((lineItemDoc.discountsum.IsNull) || (detailRow.Quantity == 0) || (detailRow.UnitPrice == 0))
                            {
                                detailRow.Discount = (float)0;
                            }
                            else
                            {
                                // discountPC = discountsum / qunatity * listprice
                                //detailRow.Discount = Convert.ToSingle((decimal)lineItemDoc.discountsum.Value / ((decimal)detailRow.Quantity * detailRow.UnitPrice));
                                float discount = Convert.ToSingle((decimal)lineItemDoc.discountsum.Value / (detailRow.UnitPrice));
                                if (discount > 1)
                                    discount = 0;
                                detailRow.Discount = discount;
                            }

                            detailRow.CreateUser = config.CrmUser;
                            detailRow.ModifyUser = config.CrmUser;
                            detailRow.CreateID = config.SequenceNumber;
                            detailRow.ModifyID = config.SequenceNumber;
                        }
                            // this error occours in case of invalid data types
                        catch (Exception e)
                        {
                            transaction.Rollback();
                            orderDoc.Id = "";
                            result.Add(orderDoc.SetTransactionStatus(TransactionStatus.UnRecoverableError, e.Message));
                            return;
                        }
                        order.Order_Details.AddOrder_DetailsRow(detailRow);
                        lineItemDoc.SetTransactionStatus(TransactionStatus.Success);
                    }

                    // here could an error ouucour in case on broken database connection 
                    // or of same invalid constraints which are unhandled before
                    try
                    {
                        detailsTableAdapter.Update(order.Order_Details);
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        orderDoc.Id = "";
                        result.Add(orderDoc.SetTransactionStatus(TransactionStatus.UnRecoverableError, e.Message));
                        return;
                    }
                    transaction.Commit();

                    orderDoc.GetTransactionResult(ref result);

                }
                catch (Exception transactionException)
                {
                    if (transaction != null)
                        transaction.Rollback();

                    orderDoc.Id = "";
                    throw;

                }

            }
        }

        public override Document GetDocument(Identity identity, Token lastToken, NorthwindConfig config)
        {
            int recordCount;
            DataSets.Order order = new DataSets.Order();
            CalculatedOrdersTableAdapter tableAdapter;
            tableAdapter = new CalculatedOrdersTableAdapter();
            CalculatedOrderDetailsTableAdapter detailTableAdapter;
            detailTableAdapter = new CalculatedOrderDetailsTableAdapter();
            DeletedOrderDetailsTableAdapter deletedDetailTableAdapter;
            deletedDetailTableAdapter = new DeletedOrderDetailsTableAdapter();


            int id;

            id = Identity.GetId(identity);



            using (OleDbConnection connection = new OleDbConnection(config.ConnectionString))
            {
                tableAdapter.Connection = connection;
                recordCount = tableAdapter.FillBy(order.CalculatedOrders, id);
                if (recordCount == 0)
                    return GetDeletedDocument(identity);

                detailTableAdapter.Connection = connection;
                detailTableAdapter.FillBy(order.CalculatedOrderDetails, id);

                deletedDetailTableAdapter.Connection = connection;
                deletedDetailTableAdapter.Fill(order.DeletedOrderDetails, id.ToString(), lastToken.SequenceNumber, config.CrmUser);
            }


            return GetDocument((DataSets.Order.CalculatedOrdersRow)order.CalculatedOrders[0],
                order.CalculatedOrderDetails,
                order.DeletedOrderDetails,
                lastToken, config);

        }

        private Document GetDocument(DataSets.Order.CalculatedOrdersRow row,
            DataSets.Order.CalculatedOrderDetailsDataTable detailDataTable,
            DataSets.Order.DeletedOrderDetailsDataTable deletedOrderDetailsDataTable,
            Token lastToken, NorthwindConfig config)
        {
            #region Declarations
            OrderDocument doc;
            string id;
            LogState logState = LogState.Updated;
            Document lineItemDoc;
            CountryCodes countryCodes = new CountryCodes();
            #endregion

            id = row.OrderID.ToString();


            if (lastToken.InitRequest)
                logState = LogState.Created;

            else if (row.IsCreateIDNull() || row.IsModifyIDNull()
                || row.IsCreateUserNull() || row.IsModifyUserNull())
                logState = LogState.Created;

            else if ((row.CreateID > lastToken.SequenceNumber)
                   && (row.CreateUser != config.CrmUser))
                logState = LogState.Created;

            else if ((row.CreateID == lastToken.SequenceNumber)
                && (row.CreateUser != config.CrmUser)
                && (id.CompareTo(lastToken.Id.Id) > 0))
                logState = LogState.Created;

            else if ((row.ModifyID >= lastToken.SequenceNumber) && (row.ModifyUser != config.CrmUser))
                logState = LogState.Updated;



            doc = new OrderDocument();
            doc.Id = id;
            doc.LogState = logState;

            doc.currency.Value = config.CurrencyCode;
            doc.pricinglistid.Value = Constants.DefaultValues.PriceList.ID;
            doc.reference.Value = id;

            if (row.IsCustomerIDNull())
                doc.accountid.Value = null;
            else
                doc.accountid.Value = Constants.CustomerIdPrefix + row.CustomerID;


            if (row.IsOrderDateNull())
                doc.opened.Value = null;
            else
                doc.opened.Value = row.OrderDate;

            if (row.IsShippedDateNull())
                doc.status.Value = Constants.OrderStatus.Active;
            else
                doc.status.Value = Constants.OrderStatus.Completed;


            //doc.DiscountPercentage.Value = new decimal(0);

            //doc.grossamt.Value = row.IsTotalQuotedPriceNull() ? new decimal(0) : Convert.ToDecimal(row.TotalQuotedPrice);
            doc.discountamt.Value = new decimal(0);
            doc.lineitemdisc.Value = row.IsDiscountAmountNull() ? new decimal(0) : Convert.ToDecimal(row.DiscountAmount);
            doc.nettamt.Value = row.IsTotalNetPriceNull() ? new decimal(0) : Convert.ToDecimal(row.TotalNetPrice);
            doc.freight.Value = row.IsFreightNull() ? new decimal(0) : row.Freight;

            doc.tax.Value = new decimal(0);
            doc.grossamt.Value = doc.nettamt.Value;



            if (row.IsRequiredDateNull())
                doc.deliverydate.Value = null;
            else
                doc.deliverydate.Value = row.RequiredDate;
            if (row.IsEmployeeIDNull())
                doc.salesrepr.Value = null;
            else
                doc.salesrepr.Value = Convert.ToString(row.EmployeeID);


            if (row.IsShipViaNull())
                doc.shippedvia.Value = null;
            else
                doc.shippedvia.Value = row.ShipVia;

            OrderAddress orderAddress = new OrderAddress();
            orderAddress.SetNorthwindAddress(row.IsShipAddressNull() ? "" : row.ShipAddress,
                row.IsShipCityNull() ? "" : row.ShipCity,
                row.IsShipPostalCodeNull() ? "" : row.ShipPostalCode,
                row.IsShipCountryNull() ? "" : row.ShipCountry);

            doc.shipaddress.Value = orderAddress.CrmOrderAddress;

            foreach (DataSets.Order.CalculatedOrderDetailsRow detailRow in detailDataTable.Rows)
            {
                lineItemDoc = GetDocumentLineItem(detailRow, lastToken, config);
                if ((doc.LogState != LogState.Created) && (lineItemDoc.HasNoLogStatus))
                    continue;

                doc.orderitems.Add(lineItemDoc);
            }

            foreach (DataSets.Order.DeletedOrderDetailsRow deletedRow in deletedOrderDetailsDataTable.Rows)
            {
                lineItemDoc = new LineItemDocument();
                lineItemDoc.Id = deletedRow[0].ToString();
                lineItemDoc.LogState = LogState.Deleted;
                doc.orderitems.Add(lineItemDoc);
            }



            return doc;

        }

        private Document GetDocumentLineItem(DataSets.Order.CalculatedOrderDetailsRow row, Token lastToken, NorthwindConfig config)
        {
            #region Declarations
            LineItemDocument doc;
            string id;
            decimal discountPercentage;
            #endregion



            id = row.OrderID.ToString() + "-" + row.ProductID.ToString();

            doc = new LineItemDocument();
            doc.Id = id;

            if (lastToken.InitRequest)
                doc.LogState = LogState.Created;
            else if ((row.CreateID >= lastToken.SequenceNumber) && (row.CreateUser != config.CrmUser))
                doc.LogState = LogState.Created;
            else if ((row.ModifyID >= lastToken.SequenceNumber) && (row.ModifyUser != config.CrmUser))
                doc.LogState = LogState.Updated;

            doc.productid.Value = row.ProductID;
            //doc.orderquouteid.Value = row.OrderID;
            doc.uomid.Value = row.ProductID;
            doc.quantity.Value = row.IsQuantityNull() ? Convert.ToInt16(0) : row.Quantity;
            doc.listprice.Value = row.IsUnitPriceNull() ? new decimal(0) : row.UnitPrice;
            discountPercentage = row.IsDiscountNull() ? (decimal)0 : Convert.ToDecimal(row.Discount);

            //doc.discountsum.Value = (decimal)(short)doc.Quantity.Value * (decimal)doc.ListPrice.Value * discountPercentage;
            doc.discountsum.Value = (decimal)doc.listprice.Value * discountPercentage;
            doc.quotedprice.Value = (decimal)doc.listprice.Value * (1 - discountPercentage);

            //doc.quotedprice.Value = row.IsQuotePriceNull() ? new decimal(0) : Convert.ToDecimal(row.QuotePrice);

            doc.taxrate.Value = "0";
            doc.tax.Value = new decimal(0);
            doc.quotedpricetotal.Value = Convert.ToDecimal(doc.quantity.Value) * Convert.ToDecimal(doc.quotedprice.Value);
            return doc;

        }

        public override int FillChangeLog(out DataTable table, NorthwindConfig config, Token lastToken)
        {
            #region declarations
            DataSets.Order order;
            int lastId;
            int recordCount;
            #endregion

            order = new DataSets.Order();

            lastId = Token.GetId(lastToken);

            using (OleDbConnection connection = new OleDbConnection(config.ConnectionString))
            {

                ChangeLogsTableAdapter tableAdapter;
                tableAdapter = new ChangeLogsTableAdapter();
                tableAdapter.Connection = connection;// fill the Changelog dataset

                if (lastToken.InitRequest)
                    recordCount = tableAdapter.Fill(order.ChangeLogs, lastId, lastToken.SequenceNumber, lastToken.SequenceNumber, "");
                else
                    recordCount = tableAdapter.Fill(order.ChangeLogs, lastId, lastToken.SequenceNumber, lastToken.SequenceNumber, config.CrmUser);
            }

            table = order.ChangeLogs;
            return recordCount;
        }

        public override List<Identity> GetAll(NorthwindConfig config, string whereExpression, OleDbParameter[] oleDbParameters)
        {
            List<Identity> result = new List<Identity>();
            int recordCount = 0;
            DataSets.Order orderDataset = new DataSets.Order();

            // get the first 11 rows of the changelog
            using (OleDbConnection connection = new OleDbConnection(config.ConnectionString))
            {
                DataSets.OrderTableAdapters.OrdersTableAdapter tableAdapter;

                tableAdapter = new DataSets.OrderTableAdapters.OrdersTableAdapter();

                tableAdapter.Connection = connection;
#warning TODO: Filter support
                recordCount = tableAdapter.Fill(orderDataset.Orders);

            }

            foreach (DataSets.Order.OrdersRow row in orderDataset.Orders.Rows)
            {
                // use where expression !!
                result.Add(new Identity(this.EntityName, row.OrderID.ToString()));
            }

            return result;
        }

        public override List<Identity> GetAll(NorthwindConfig config, string whereExpression, int startIndex, int count)
        {
            throw new NotImplementedException();
        }
    }
}
