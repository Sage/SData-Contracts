using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SageKa.Samples.VcSync.SyncEngine.Model;

namespace SageKa.Samples.VCSync.Synchronizer
{
    public partial class ProxyForm : Form
    {
        private ProxyInfo _proxyInfo;
        public ProxyForm(ProxyInfo proxyInfo)
        {
            _proxyInfo = proxyInfo;
            InitializeComponent();

            if(!string.IsNullOrEmpty(proxyInfo.Host))
            {
                txtProxy.Text = _proxyInfo.Host;
                txtPort.Text = _proxyInfo.Port.ToString();
            }
            if (_proxyInfo.Credentials != null && (!string.IsNullOrEmpty(_proxyInfo.Credentials.User)))
            {
                txtUser.Text = _proxyInfo.Credentials.User;
                txtPassword.Text = _proxyInfo.Credentials.Password;
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtProxy.Text))
            {
                MessageBox.Show("Please specify a Proxy");
                return;
            }
            int port;
            if (!int.TryParse(txtPort.Text, out port))
            {
                MessageBox.Show("Please specify a numeric Port");
                return;
            }
            else
            {
                _proxyInfo.Port = port;
            }
            _proxyInfo.Host = txtProxy.Text;

            if(string.IsNullOrEmpty(txtUser.Text))
            {
                if (_proxyInfo.Credentials == null)
                    _proxyInfo.Credentials = new CredentialInfo();
                _proxyInfo.Credentials.User = txtUser.Text;
                _proxyInfo.Credentials.Password = txtPassword.Text;

            }

            this.Close();
        }

    }
}
