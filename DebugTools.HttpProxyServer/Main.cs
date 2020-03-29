using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HttpProxyServer
{
    public partial class Main : Form
    {
        private Dictionary<String,TabPage> _tabPageMap = new Dictionary<String,TabPage>();
        private ProxyServer _server = new ProxyServer();
        public Main()
        {
            InitializeComponent();
            _server.OnProxyStarted += new ProxyEventHandler(Server_OnProxyStarted);
            _server.OnProxyFinished += new ProxyEventHandler(Server_OnProxyFinished);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                _server.Start((int)nudPort.Value);
                this.btnStart.Enabled = false;
                this.btnStop.Enabled = true;
                this.nudPort.Enabled = false;
                this.lblStatus.Text = "运行中";
                this.lblStatus.ForeColor = Color.Green;
            }
            catch
            {
                this.lblStatus.Text = "启动失败";
                this.lblStatus.ForeColor = Color.Red;
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            _server.Stop();
            this.btnStart.Enabled = true;
            this.btnStop.Enabled = false;
            this.nudPort.Enabled = true;

            this.lblStatus.Text = "已停止";
            this.lblStatus.ForeColor = Color.Gray;
        }
         

        private void mode_CheckedChanged(object sender, EventArgs e)
        {
            if (!((RadioButton)sender).Checked) return;
            this._server.ProxyMode = (ProxyModes)int.Parse(((RadioButton)sender).Tag.ToString());

        }

        private void chkRecord_CheckedChanged(object sender, EventArgs e)
        {
            this._server.IsRecordData = ((CheckBox)sender).Checked;
        }

        private void chkDebug_CheckedChanged(object sender, EventArgs e)
        {
            this._server.IsDebug = ((CheckBox)sender).Checked;
        }

        private void chkOnlyNoData_CheckedChanged(object sender, EventArgs e)
        {
            this._server.IsOnlyNoData = ((CheckBox)sender).Checked;
        }
        
        void Server_OnProxyStarted(ProxyRecord record)
        {
            this.BeginInvoke(new ProxyEventHandler(OnProxyStarted), record); 
        }

        void OnProxyStarted(ProxyRecord record)
        {
            ListView listView = null;
            if (_tabPageMap.ContainsKey(record.ClientIP))
            {
                TabPage page = _tabPageMap[record.ClientIP];
                listView = (ListView)page.Controls[0];
            }
            else
            {
                TabPage page = new TabPage(record.ClientIP);
                listView = new ListView();
                listView.Dock = DockStyle.Fill;
                listView.FullRowSelect = true;
                listView.View = View.Details;
                listView.Columns.Add("访问时间", 220);
                listView.Columns.Add("文档类型", 200);
                listView.Columns.Add("URL", 500);
                listView.Columns.Add("状态", 100);
                listView.MouseDoubleClick += new MouseEventHandler(listView_MouseDoubleClick);
                listView.ContextMenuStrip = cmsRecord;
                page.Controls.Add(listView);
                this.tabControl1.TabPages.Add(page);
                this._tabPageMap.Add(record.ClientIP, page);
            }

            ListViewItem item = new ListViewItem();
            record.Control = item;
            item.Text = record.AccessTime.ToString();
            item.SubItems.Add(record.Request.Accept);
            item.SubItems.Add(record.Uri.ToString());
            item.SubItems.Add(record.StatusText);
            item.Tag = record;
            listView.Items.Add(item);
            while (listView.Items.Count > nudRecordCount.Value)
            {
                listView.Items.RemoveAt(0);
            }
        }

        void listView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListView listView = (ListView)sender;
            if (listView.SelectedItems.Count == 0)
            {
                return;
            }
            ProxyRecord record = (ProxyRecord)listView.SelectedItems[0].Tag;
            DetailForm form = new DetailForm(record);
            form.ShowDialog();
        }

        void Server_OnProxyFinished(ProxyRecord record)
        {
            this.BeginInvoke(new ProxyEventHandler(OnProxyFinished), record); 
   
        }

        void OnProxyFinished(ProxyRecord record)
        {
            if (record.Control == null) return;
            ListViewItem item = (ListViewItem)record.Control;
            item.SubItems[1].Text = record.ProxyResponse.ContentType;
            item.SubItems[3].Text = record.StatusText;
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_server != null)
            {
                _server.Stop();
                _server = null;
            }
        }

        private void tsmClear_Click(object sender, EventArgs e)
        {
            ((ListView)tabControl1.SelectedTab.Controls[0]).Items.Clear();
        }

        private void tsmRemoveIp_Click(object sender, EventArgs e)
        {
            tabControl1.TabPages.RemoveAt(tabControl1.SelectedIndex);
        }
         

    }
}
