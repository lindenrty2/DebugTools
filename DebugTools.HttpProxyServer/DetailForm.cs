using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HttpProxyServer
{
    public partial class DetailForm : Form
    {
        private ProxyRecord _record;
        public DetailForm(ProxyRecord record)
        {
            InitializeComponent();
            this._record = record;
        }

        private void DetailForm_Load(object sender, EventArgs e)
        {
            this.lblUrl.Text = this._record.Uri.ToString();
            this.lblIP.Text = this._record.ClientIP;
            this.lblTime.Text = this._record.AccessTime.ToString();
            this.lblStatus.Text = this._record.StatusText;
            this.pgRequest.SelectedObject = this._record.Request;
            this.txtRequestBody.Text = this._record.Request.StringBody;
            this.pgResponse.SelectedObject = this._record.ProxyResponse;
            this.txtResponseBody.Text = this._record.ProxyResponse == null ? "" : this._record.ProxyResponse.StringBody;
        }

        private void bnClose_Click(object sender, EventArgs e)
        {
           
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _record.SaveCache();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            _record.DeleteCache();
        }

        private void btnSendAgain_Click(object sender, EventArgs e)
        {
            
        } 
    }
}
