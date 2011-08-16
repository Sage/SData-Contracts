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
    public partial class AuthenticationForm : Form
    {
        private EndPointInfo _endpointInfo;
        public AuthenticationForm(EndPointInfo endpointInfo)
        {
            _endpointInfo = endpointInfo;
            InitializeComponent();
            if (_endpointInfo.Url != null)
                lblUrl.Text = _endpointInfo.Url.ToString();
            if (_endpointInfo.Credentials != null)
            {
                this.txtUser.Text = _endpointInfo.Credentials.User;
                this.txtPassword.Text = _endpointInfo.Credentials.Password;
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (_endpointInfo.Credentials == null)
                _endpointInfo.Credentials = new CredentialInfo();

            _endpointInfo.Credentials.User = this.txtUser.Text;
            _endpointInfo.Credentials.Password = this.txtPassword.Text;
            this.Close();

        }
    }
}
