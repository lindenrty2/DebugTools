using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;

namespace HttpProxyServer
{
    public partial class DebugForm : Form
    {
        public DebugForm()
        {
            InitializeComponent();
        }

        private void DebugForm_Load(object sender, EventArgs e)
        {

        } 

        private ProxyRecord _record = null;
        private HttpResponseContent _oResponse = null;
        private List<Button> _cacheButtonList = null;
        public void setData(ProxyRecord record)
        {

            _record = record;
            _oResponse = record.ProxyResponse.Copy();
            lblIP.Text = record.ClientIP;
            if (record.loadServerResponse())
            { 
                pgServer.SelectedObject = record.ServerResponse;
                txtServer.Text = record.ServerResponse.StringBody;
                lblRemote.Text = record.Uri.ToString();
            }
            if (record.ProxyResponse != null )
            {
                pgLocal.SelectedObject = record.ProxyResponse.Copy();
                txtLocal.Text = record.ProxyResponse.StringBody;
                
            }
            if(record.loadCache()){
                lblLocal.Text = record.CacheResponse.LocalCachePath;
                lblLocal.ForeColor = Color.Black;
            }
            else
            {
                txtLocal.Text = txtServer.Text;
                lblLocal.Text = "本地无缓存";
                lblLocal.ForeColor = Color.Red;
            }

            if (record.CacheResponse.LocalCachePath != null)
            {
                initCachePathSelector(record.Uri, record.CacheResponse.LocalCachePath);
            }
            else
            {
                initCachePathSelector(record.Uri, CacheHelper.getLocalCachePath(record.Uri));
            }
        }

        private void initCachePathSelector(Uri uri,String path)
        {
            List<Button> buttonList = new List<Button>();
            String defaultPath = CacheHelper.getLocalCachePath(uri);
            FileInfo fi = new FileInfo(defaultPath);
            DirectoryInfo currentDir = fi.Directory;
            while (currentDir != null)
            {
                if (currentDir.FullName + @"\" == CacheHelper.PATH_CACHEBASE)
                {
                    break;
                }
                Button button = new Button();
                button.AutoSize = true;
                button.Text = currentDir.Name;
                button.FlatStyle = FlatStyle.Flat;
                button.Click += new EventHandler(button_Click);
                buttonList.Insert(0, button);
                currentDir = currentDir.Parent;
            }
            FileInfo efi = new FileInfo(path);
            List<String> dis = new List<String>();
            currentDir = efi.Directory;
            while (currentDir != null)
            {
                if (currentDir.FullName + @"\" == CacheHelper.PATH_CACHEBASE)
                {
                    break;
                }
                dis.Insert(0,currentDir.Name);
                currentDir = currentDir.Parent;
            }
            Button baseLabel = new Button();
            baseLabel.BackColor = Color.Black;
            baseLabel.ForeColor = Color.White;
            baseLabel.AutoSize = true;
            baseLabel.Text = CacheHelper.PATH_CACHEBASE;
            baseLabel.Click += button_Click;
            pnlCache.Controls.Add(baseLabel);
            bool samePath = true;
            for (int i = 0; i < buttonList.Count; i++)
            {
                Button currentButton = buttonList[i];
                if (samePath && dis.Count > i && dis[i] == currentButton.Text)
                {
                    currentButton.BackColor = Color.Black;
                    currentButton.ForeColor = Color.White;
                }
                else
                {
                    currentButton.BackColor = Color.White;
                    currentButton.ForeColor = Color.Black;
                    samePath = false;
                }
                pnlCache.Controls.Add(currentButton);
            }
            _cacheButtonList = buttonList;
        }

        private String getCacheFilePath()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(CacheHelper.PATH_CACHEBASE);
            for (int i = 0; i < this._cacheButtonList.Count; i++)
            {
                Button currentButton = this._cacheButtonList[i];
                if (currentButton.BackColor != Color.Black)
                {
                    break;
                }
                sb.Append(@"\" + currentButton.Text);
            }
            return sb.ToString();
        }

        private void button_Click(object sender, EventArgs e)
        {
            bool findEnd = false;
            for (int i = 0; i < this._cacheButtonList.Count; i++)
            {
                Button currentButton = this._cacheButtonList[i];
                if (!findEnd)
                {
                    currentButton.BackColor = Color.Black;
                    currentButton.ForeColor = Color.White;
                }
                else
                {
                    currentButton.BackColor = Color.White;
                    currentButton.ForeColor = Color.Black;
                }
                if (currentButton == sender)
                {
                    findEnd = true;
                }
            }
        }


        private void btnOK_Click(object sender, EventArgs e)
        {
            _record.ProxyResponse = (HttpResponseContent)pgLocal.SelectedObject;
            _record.ProxyResponse.StringBody = txtLocal.Text;
        }

        private void btnSaveOK_Click(object sender, EventArgs e)
        {
            String cachePath = getCacheFilePath();
            _record.ProxyResponse = (HttpResponseContent)pgLocal.SelectedObject; 
            _record.ProxyResponse.LocalCachePath = cachePath + @"\" + CacheHelper.TYPE_CACHE_CONTENT;
            _record.ProxyResponse.LocalHeadersPath = cachePath + @"\" + CacheHelper.TYPE_HEADERS_CONTENT;
            _record.ProxyResponse.LocalCookiesPath = cachePath + @"\" + CacheHelper.TYPE_COOKIES_CONTENT;
            _record.ProxyResponse.StringBody = txtLocal.Text;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _record.ProxyResponse = _oResponse.Copy();
        }
            
    }
}
