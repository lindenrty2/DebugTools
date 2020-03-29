namespace DebugTools.ControlCenter 
{
    partial class ControlCenterFrm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ControlCenterFrm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.InOutBrowser = new System.Windows.Forms.ToolStripMenuItem();
            this.終了EToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmBrowser = new System.Windows.Forms.ToolStripMenuItem();
            this.ツールToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmOption = new System.Windows.Forms.ToolStripMenuItem();
            this.ウィンドウWToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ProessImageList = new System.Windows.Forms.ImageList(this.components);
            this.ctlTab = new System.Windows.Forms.TabControl();
            this.tabDetail = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.menuStrip1.SuspendLayout();
            this.ctlTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.InOutBrowser,
            this.tsmBrowser,
            this.ツールToolStripMenuItem,
            this.ウィンドウWToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.MdiWindowListItem = this.ウィンドウWToolStripMenuItem;
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1484, 26);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // InOutBrowser
            // 
            this.InOutBrowser.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.終了EToolStripMenuItem});
            this.InOutBrowser.Name = "InOutBrowser";
            this.InOutBrowser.Size = new System.Drawing.Size(76, 22);
            this.InOutBrowser.Text = "メイン(&M)";
            // 
            // 終了EToolStripMenuItem
            // 
            this.終了EToolStripMenuItem.Name = "終了EToolStripMenuItem";
            this.終了EToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.終了EToolStripMenuItem.Text = "終了(&X)";
            // 
            // tsmBrowser
            // 
            this.tsmBrowser.Name = "tsmBrowser";
            this.tsmBrowser.Size = new System.Drawing.Size(86, 22);
            this.tsmBrowser.Text = "ブラウザ(&B)";
            this.tsmBrowser.Visible = false;
            // 
            // ツールToolStripMenuItem
            // 
            this.ツールToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmOption});
            this.ツールToolStripMenuItem.Name = "ツールToolStripMenuItem";
            this.ツールToolStripMenuItem.Size = new System.Drawing.Size(74, 22);
            this.ツールToolStripMenuItem.Text = "ツール(&T)";
            this.ツールToolStripMenuItem.Visible = false;
            // 
            // tsmOption
            // 
            this.tsmOption.Name = "tsmOption";
            this.tsmOption.Size = new System.Drawing.Size(155, 22);
            this.tsmOption.Text = "オプション(&O)";
            this.tsmOption.Click += new System.EventHandler(this.tsmOption_Click);
            // 
            // ウィンドウWToolStripMenuItem
            // 
            this.ウィンドウWToolStripMenuItem.Name = "ウィンドウWToolStripMenuItem";
            this.ウィンドウWToolStripMenuItem.Size = new System.Drawing.Size(102, 22);
            this.ウィンドウWToolStripMenuItem.Text = "ウィンドウ(&W)";
            this.ウィンドウWToolStripMenuItem.Visible = false;
            // 
            // ProessImageList
            // 
            this.ProessImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ProessImageList.ImageStream")));
            this.ProessImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.ProessImageList.Images.SetKeyName(0, "10204_keijikirokuviewer.png");
            this.ProessImageList.Images.SetKeyName(1, "10214_kanjaprofile.png");
            this.ProessImageList.Images.SetKeyName(2, "10604_keijikirokuviewer_BL.png");
            this.ProessImageList.Images.SetKeyName(3, "10614_kanjaprofile_BL.png");
            // 
            // ctlTab
            // 
            this.ctlTab.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ctlTab.Controls.Add(this.tabDetail);
            this.ctlTab.Controls.Add(this.tabPage2);
            this.ctlTab.Location = new System.Drawing.Point(12, 29);
            this.ctlTab.Name = "ctlTab";
            this.ctlTab.SelectedIndex = 0;
            this.ctlTab.Size = new System.Drawing.Size(1460, 480);
            this.ctlTab.TabIndex = 5;
            // 
            // tabDetail
            // 
            this.tabDetail.Location = new System.Drawing.Point(4, 22);
            this.tabDetail.Name = "tabDetail";
            this.tabDetail.Padding = new System.Windows.Forms.Padding(3);
            this.tabDetail.Size = new System.Drawing.Size(1452, 454);
            this.tabDetail.TabIndex = 0;
            this.tabDetail.Text = "詳細情報";
            this.tabDetail.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1452, 454);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // ControlCenterFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(1484, 521);
            this.Controls.Add(this.ctlTab);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(1400, 38);
            this.Name = "ControlCenterFrm";
            this.Text = "MegaOak/iS ";
            this.Load += new System.EventHandler(this.ControlCenterFrm_Load);
            this.MdiChildActivate += new System.EventHandler(this.ControlCenterFrm_MdiChildActivate);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ctlTab.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem InOutBrowser;
        private System.Windows.Forms.ToolStripMenuItem 終了EToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ツールToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmOption;
        private System.Windows.Forms.ToolStripMenuItem ウィンドウWToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmBrowser;
        private System.Windows.Forms.ImageList ProessImageList;
        private System.Windows.Forms.TabControl ctlTab;
        private System.Windows.Forms.TabPage tabDetail;
        private System.Windows.Forms.TabPage tabPage2;
    }
}

