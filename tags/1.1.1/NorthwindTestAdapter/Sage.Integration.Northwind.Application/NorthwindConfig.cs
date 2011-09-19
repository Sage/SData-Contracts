#region ©2007 Sage Technology Ltd., All Rights Reserved.
/*=====================================================================
	File:			NorthwindConfig.cs
	Author:			Philipp Schuette
	DateCreated:	04/03/2007 11:19:43
	DateChanged:	04/03/2007 11:19:43
 ---------------------------------------------------------------------
	©2007 Sage Technology Ltd., All Rights Reserved.	
=====================================================================*/

/*=====================================================================
	Date			        Author		Changes			Reasons
	04/03/2007 11:19:43	pschuette		Create			  
	Changes: Create, Refactoring, Upgrade	
=====================================================================*/
#endregion

#region Usings
using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Northwind.Application.Toolkit;
using System.Data.OleDb;

using Sage.Integration.Northwind.Application.API;
#endregion

namespace Sage.Integration.Northwind.Application
{

    public class NorthwindConfig
    {
        private const string DEFAULT_DATASET = "Northwind.mdb";

        #region constructor

        public NorthwindConfig(string dataset)
        {
            this.path = "";
            this.currencyCode = "EUR";
            sequenceNumber = 0;
            lastSequenceNumber = 0;
            random = new Random();
            version = "1.0.2";
            customisationVersion = version;
            logChangeLog = false;
            logDir = "c:\\";

            if (dataset == "-")
            {
                this._dataset = DEFAULT_DATASET;
            }
            else if (!dataset.Contains("."))
            {
                this._dataset = dataset + ".mdb";
            }
            else
            {
                this._dataset = dataset;
            }
        }

        #endregion


        #region fields
        private string crmUser;
        private string password;
        private Random random;
        private string path;
        private string currencyCode;
        private string version;
        private string customisationVersion;
        private bool logChangeLog;
        private string logDir;

        public string LogDir
        {
            get { return logDir; }
            set { logDir = value; }
        }

        public bool LogChangeLog
        {
            get { return logChangeLog; }
            set { logChangeLog = value; }
        }



        public string CurrencyCode
        {
            get
            {
                return currencyCode;
            }
            set
            {
                currencyCode = value;
            }
        }



        private int sequenceNumber;
        private int lastSequenceNumber;



        #endregion

        #region public properties


        public Random Random
        {
            get {
                if (random == null)
                    random = new Random();
                
                return random; }
        }

        public int SequenceNumber
        {
            get
            {
                if (sequenceNumber == 0)
                    try
                    {
                        sequenceNumber = GetNewSequenceNumber(this);
                    }
                    catch (Exception) { }


                return sequenceNumber;
            }
            set { sequenceNumber = value; }
        }

        public int LastSequenceNumber
        {
            get
            {
                if (lastSequenceNumber == 0)
                    try
                    {
                        lastSequenceNumber = GetLastSequenceNumber(this);
                    }
                    catch (Exception) { }


                return lastSequenceNumber;
            }
            set { lastSequenceNumber = value; }
        }

        public string Path
        {
            get
            {
                return path;
            }
            set
            {
                path = value;
                if (!path.EndsWith("\\"))
                    path += "\\";
            }
        }

        public string ConnectionString
        {
            get 
            {
                //return "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Path + Dataset + ";Persist Security Info=True;"; //Connection pooling disabled
                return "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Path + Dataset + ";Persist Security Info=True;OLE DB Services=-4;"; //Connection pooling enabled
            }
        }

        private string _dataset;
        public string Dataset
        {
            get { return _dataset; }
        }

        public string CrmUser
        {
            get { return crmUser; }
            set { crmUser = value; }
        }


        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public string Version
        {
            get { return version; }
            set { version = value; }
        }

        public string CustomisationVersion
        {
            get
            {
                if (lastSequenceNumber == 0)
                    lastSequenceNumber = GetLastSequenceNumber(this);
                customisationVersion = version + "." + lastSequenceNumber;
                return customisationVersion; }
            set { customisationVersion = value; }
        }

        #endregion

        #region public members

        #endregion

        #region private members
        private int GetNewSequenceNumber(NorthwindConfig config)
        {
            int sequenceNumber = -1;

            using (OleDbConnection connection = new OleDbConnection(config.ConnectionString))
            {
                connection.Open();
                sequenceNumber = GetNewSequenceNumber(config, connection);
            }

            return sequenceNumber;

        }

        private int GetLastSequenceNumber(NorthwindConfig config)
        {
            int sequenceNumber = -1;

            using (OleDbConnection connection = new OleDbConnection(config.ConnectionString))
            {
                connection.Open();
                sequenceNumber = GetLastSequenceNumber(config, connection);
            }

            return sequenceNumber;

        }

        private int GetNewSequenceNumber(NorthwindConfig config, OleDbConnection connection)
        {
            //Sequence sequence;
            //sequence = new Data.Sequence();
            int sequenceNumber = -1;

            //SequenceTableAdapter sequenceTableAdapter;
            //sequenceTableAdapter = new SequenceTableAdapter();
            //Sequence.SequenceRow row = sequence._Sequence.NewSequenceRow();
            //row.Date = DateTime.Now;
            //sequence._Sequence.AddSequenceRow(row);
            //sequenceTableAdapter.Connection = connection;
            //sequenceTableAdapter.Update(sequence._Sequence);

            OleDbCommand CmdSequence = new OleDbCommand("INSERT INTO Sequence ( [Date] ) SELECT Now()", connection);
            CmdSequence.ExecuteNonQuery();
            OleDbCommand Cmd = new OleDbCommand("SELECT @@IDENTITY", connection);

            object lastid = Cmd.ExecuteScalar();
            sequenceNumber = (int)lastid;


            return sequenceNumber;

        }


        private int GetLastSequenceNumber(NorthwindConfig config, OleDbConnection connection)
        {

            int sequenceNumber = -1;


            OleDbCommand Cmd = new OleDbCommand("SELECT Max(Sequence.ID) AS MaxOfID FROM Sequence", connection);

            object lastid = Cmd.ExecuteScalar();
            if (lastid == null || lastid == DBNull.Value)
                sequenceNumber = 0;
            else
                sequenceNumber = (int)lastid;


            return sequenceNumber;

        }
        


#endregion



       
    }
}
