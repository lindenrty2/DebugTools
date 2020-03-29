namespace DebugTools.Common.Hook.ExceptionHook
{
    partial class ExceptionViewer
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

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.wbInformation = new System.Windows.Forms.WebBrowser();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnExportReport = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // wbInformation
            // 
            this.wbInformation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wbInformation.Location = new System.Drawing.Point(0, 0);
            this.wbInformation.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbInformation.Name = "wbInformation";
            this.wbInformation.Size = new System.Drawing.Size(643, 293);
            this.wbInformation.TabIndex = 0;
            this.wbInformation.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.wbInformation_Navigated);
            this.wbInformation.NewWindow += new System.ComponentModel.CancelEventHandler(this.wbInformation_NewWindow);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnExportReport);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 293);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(643, 39);
            this.panel1.TabIndex = 2;
            // 
            // btnExportReport
            // 
            this.btnExportReport.Location = new System.Drawing.Point(500, 7);
            this.btnExportReport.Name = "btnExportReport";
            this.btnExportReport.Size = new System.Drawing.Size(137, 26);
            this.btnExportReport.TabIndex = 2;
            this.btnExportReport.Text = "レポートを出力する";
            this.btnExportReport.UseVisualStyleBackColor = true;
            this.btnExportReport.Click += new System.EventHandler(this.btnExportReport_Click);
            // 
            // ExceptionViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.wbInformation);
            this.Controls.Add(this.panel1);
            this.Name = "ExceptionViewer";
            this.Size = new System.Drawing.Size(643, 332);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser wbInformation;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnExportReport;
    }
}
