#region Usings

using System;
using System.Windows.Forms;
using SageKa.Samples.VcSync.SyncEngine;
using System.Xml;
using SageKa.Samples.VcSync.SyncEngine.Model;
using System.Xml.Serialization;
using System.IO;

#endregion

namespace SageKa.Samples.VCSync.Synchronizer
{
    public partial class MainForm : Form
    {
        private const string RUNNAME_TEMPLATE = "VCSyncSampleSynchronizer_{0}";
        private const string APPCONFIGFILENAME = "SageKa.Samples.VCSync.Synchronizer.exe.config";
        private XmlDocument _doc;
        private XmlNode _runCounterNode;
        private SynchronisationInfo _syncInfo;
        private String _fileName;
        private bool _changed;

        private bool DisplayModel()
        {
            txtApp1.Text = _syncInfo.Source.UrlString;
            txtApp2.Text = _syncInfo.Target.UrlString;
            txtLoggingApp.Text = _syncInfo.Logging.UrlString;

            grdResources.Rows.Clear();
            foreach (string resource in _syncInfo.Resources)
            {
                grdResources.Rows.Add(resource);
     
            }
            _changed = false;
            grdResources.Refresh();
            return true;
        }

        private bool PersistFormToModel()
        {

            string endpoint1 = txtApp1.Text;
            string endpoint2 = txtApp2.Text;
            string endpointLog = txtLoggingApp.Text;


            // Validate user input
            if (string.IsNullOrEmpty(endpoint1))
            {
                MessageBox.Show("Please enter the url of the source application");
                return false ;
            }
            if (string.IsNullOrEmpty(endpoint2))
            {
                MessageBox.Show("Please enter the url of the target application");
                return false;
            }


            Uri endpoint1Uri;
            Uri endpoint2Uri;
            Uri endpointLogUri;
            if (!Uri.TryCreate(endpoint1, UriKind.Absolute, out endpoint1Uri))
            {
                MessageBox.Show("Url of source application is not valid");
                return false ;
            }
            else
                _syncInfo.Source.UrlString = endpoint1;


            if (!Uri.TryCreate(endpoint2, UriKind.Absolute, out endpoint2Uri))
            {
                MessageBox.Show("Url of target application is not valid");
                return false;
            }
            else
                _syncInfo.Target.UrlString = endpoint2;


            if (!string.IsNullOrEmpty(endpointLog))
            {
                if (!Uri.TryCreate(endpointLog, UriKind.Absolute, out endpointLogUri))
                {
                    MessageBox.Show("Url of logging application is not valid");
                    return false;
                }
                else
                    _syncInfo.Logging.UrlString = endpointLog;

            }
            else
            { endpointLogUri = null; }

            

            if (grdResources.Rows.Count == 0)
            {
                MessageBox.Show("Please specify one resource");
                return false;
            }

            if (grdResources.Rows.Count == 1 && grdResources.Rows[0].IsNewRow)
            {
                MessageBox.Show("Please specify one resource");
                return false;
            }
            grdResources.CommitEdit(DataGridViewDataErrorContexts.Commit);
            _syncInfo.Resources.Clear();
            foreach (DataGridViewRow row in grdResources.Rows)
            {
                if (!row.IsNewRow)
                {
                    
                    _syncInfo.Resources.Add(row.Cells[0].Value.ToString());
                }
            }
            _changed = false;
            return true;

        }

        #region Ctor.

        public MainForm()
        {
            InitializeComponent();

            _doc = new XmlDocument();
            _doc.Load(APPCONFIGFILENAME);
            _syncInfo = new SynchronisationInfo();
        }

        #endregion

        private void btnSynchronize_Click(object sender, EventArgs e)
        {
            Synchronize(SynchronisationDirection.forward);
        }

        private void Synchronize(SynchronisationDirection direction)
        {
            try
            {

                if (!PersistFormToModel())
                    return;

                // Create a new instance of the sync engine
                // and add a Logger. The Logger of type FifoStackLogger appended here
                // will provide a stack of messages that can be polled.
                Engine syncEngine = new Engine(this.toolStripRunName.Text, DateTime.Now, direction, _syncInfo);
                FifoStackLogger logger = new FifoStackLogger();
                logger.SeverityFilter = Severity.Info;
                syncEngine.Logger = logger;

                // Create and diaplay progress form that will start the syncEngine
                ProgressForm progressForm = new ProgressForm(syncEngine);
                progressForm.ShowDialog(this);
            }

            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occured: " + Environment.NewLine + ex.Message);
            }
            finally
            {
                try
                {
                    int runCounter = this.GetRunCounter();
                    runCounter++;
                    // Change the RunCounter and display new next run Name
                    this.SetRunCounter(runCounter);
                    this.toolStripRunName.Text = string.Format(RUNNAME_TEMPLATE, runCounter);
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Failed to save config file: " + Environment.NewLine + exception.Message);
                }
            }
        }

        

        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            XmlNodeList nodes;

            nodes = _doc.SelectNodes("/configuration/applicationSettings/SageKa.Samples.VCSync.Synchronizer.MainSettings/setting[@name='RunCounter']/value");

            if (nodes.Count == 0)
                throw new ApplicationException("Invalid application config file. Cannot read setting 'RunCounter'");
            else
                _runCounterNode = nodes[0];


            this.toolStripRunName.Text = string.Format(RUNNAME_TEMPLATE, this.GetRunCounter());
        }

        private int GetRunCounter()
        {
            return Convert.ToInt32(_runCounterNode.InnerText);
        }
        private void SetRunCounter(int newRunCounter)
        {
            _runCounterNode.InnerText = XmlConvert.ToString(newRunCounter);
            _doc.Save(APPCONFIGFILENAME);
        }

        private void btnSynchronizeBack_Click(object sender, EventArgs e)
        {
            Synchronize(SynchronisationDirection.backward);

        }


        private void toolStripSynchronize_Click(object sender, EventArgs e)
        {
            Synchronize(SynchronisationDirection.forward);
        }

        private void toolStripSynchronizeBack_Click(object sender, EventArgs e)
        {
            Synchronize(SynchronisationDirection.backward);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void sourceAuthenticationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Uri endPointUri;
            if (!Uri.TryCreate(txtApp1.Text, UriKind.Absolute, out endPointUri))
            {
                MessageBox.Show("Url of source application is not valid");
            }
            else
            {
                _syncInfo.Source.UrlString = txtApp1.Text;
                AuthenticationForm authform = new AuthenticationForm(_syncInfo.Source);
                authform.ShowDialog();
            }
        }

        private void targetAuthenticationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Uri endPointUri;
            if (!Uri.TryCreate(txtApp2.Text, UriKind.Absolute, out endPointUri))
            {
                MessageBox.Show("Url of target application is not valid");
            }
            else
            {
                _syncInfo.Target.UrlString = txtApp2.Text;
                AuthenticationForm authform = new AuthenticationForm(_syncInfo.Target);
                authform.ShowDialog();
            }
        }

        private void loggingAuthenticationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Uri endPointUri;
            if (!Uri.TryCreate(txtLoggingApp.Text, UriKind.Absolute, out endPointUri))
            {
                MessageBox.Show("Url of logging application is not valid");
            }
            else
            {
                _syncInfo.Logging.UrlString = txtLoggingApp.Text;
                AuthenticationForm authform = new AuthenticationForm(_syncInfo.Logging);
                authform.ShowDialog();
            }
        }

        private void proxySettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_syncInfo.Proxy == null)
                _syncInfo.Proxy = new ProxyInfo();

            ProxyForm proxyForm = new ProxyForm(_syncInfo.Proxy);
            proxyForm.ShowDialog();
            
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!PersistFormToModel())
                    return;

                if (string.IsNullOrEmpty(_fileName))
                {
                    SaveAs();
                }
                else
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(SynchronisationInfo));
                    TextWriter tw = new StreamWriter(_fileName); //"test.sync");
                    xmlSerializer.Serialize(tw, _syncInfo);
                    _syncInfo.Changed = false;
                    _changed = false;
                    tw.Close();
                }
            }
            catch (Exception exp)
            {
                ShowException(exp);
            }
        }

        private void SaveAs()
        {
            saveFileDialog1.Filter = "Sdata Synchronizer Settings (*.sync)|*.sync";
            saveFileDialog1.FileName = "";  
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Stream st = saveFileDialog1.OpenFile();
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(SynchronisationInfo));
                TextWriter tw = new StreamWriter(st); //"test.sync");
                xmlSerializer.Serialize(tw, _syncInfo);
                tw.Close();
                _syncInfo.Changed = false;
                _changed = false;
                _fileName = saveFileDialog1.FileName;
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (_syncInfo.Changed || _changed)
                {
                    if (MessageBox.Show("Discard changes?", "", MessageBoxButtons.YesNo) == DialogResult.No)
                        return;
                }

                openFileDialog1.FileName = "";
                openFileDialog1.Multiselect = false;
                openFileDialog1.Filter = "Sdata Synchronizer Settings (*.sync)|*.sync";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {

                    Stream st = openFileDialog1.OpenFile();
                    XmlSerializer serializer = new XmlSerializer(typeof(SynchronisationInfo));
                    TextReader tr = new StreamReader(st);// ("test.sync");
                    _syncInfo = (SynchronisationInfo)serializer.Deserialize(tr);
                    _syncInfo.Changed = false;
                    _changed = false;
                    _fileName = openFileDialog1.FileName;
                    tr.Close();
                    DisplayModel();
                }
            }
            catch (Exception exp)
            {
                ShowException(exp);
            }
        }

        private void ShowException(Exception exp)
        {
            MessageBox.Show(string.Format("Following exception occured: {0}", exp.Message));
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!PersistFormToModel())
                    return;
                SaveAs();
            }
            catch (Exception exp)
            {
                ShowException(exp);
            }
        }

        private void grdResources_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            _changed = true;
        }

        private void txtApp1_TextChanged(object sender, EventArgs e)
        {
            _changed = true;
        }

        private void txtApp2_TextChanged(object sender, EventArgs e)
        {
            _changed = true;
        }

        private void txtLoggingApp_TextChanged(object sender, EventArgs e)
        {
            _changed = true;
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            int index = GetSelectedResourceIndex();
            
            if (index <= 0)
                    return;

            try
            {
                DataGridViewRow row = grdResources.Rows[index];
                grdResources.Rows.Remove(row);
                grdResources.Rows.Insert(index - 1, row);

                grdResources.ClearSelection();
                grdResources.Refresh();
                _changed = true;
            }
            catch (Exception)
            { }
        
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
           
            int index = GetSelectedResourceIndex();

            if (index < 0)
                return;
            int count = grdResources.Rows.Count;
            if (index > count - 2)
                return;
            try
            {
            DataGridViewRow row = grdResources.Rows[index];
            grdResources.Rows.Remove(row);
            grdResources.Rows.Insert(index +1, row);

            grdResources.ClearSelection();
            grdResources.Refresh();
            _changed = true;
            }
            catch (Exception)
            { }
        }

        private void grdResources_SelectionChanged(object sender, EventArgs e)
        {
            int index = GetSelectedResourceIndex();
            int count = grdResources.Rows.Count;
            if (index == -1 || count == 0)
            {
                btnUp.Enabled = false;
                btnDown.Enabled = false;
            }
            else if (index == 0)
            {
                btnUp.Enabled = false;
                btnDown.Enabled = true;
            }
            else if (index < count - 2)
            {
                btnUp.Enabled = true;
                btnDown.Enabled = true;
            }
            else if (index == count-1)
            {
                btnUp.Enabled = false;
                btnDown.Enabled = false;
            }
            else
            {
                btnUp.Enabled = true;
                btnDown.Enabled = false;
            }

        }

        private int GetSelectedResourceIndex()
        {
            int index = 0;
            if (grdResources.CurrentRow != null)
                index = grdResources.CurrentRow.Index;
            if (index > 0)
                return index;
            if (grdResources.SelectedRows.Count == 1)
            {
                index = grdResources.SelectedRows[0].Index;
            }
            else if (grdResources.SelectedCells.Count > 0)
            {
                index = grdResources.SelectedCells[0].RowIndex;
            }
            else
            {
                return -1;
            }

            return index;
        }

    }
}
