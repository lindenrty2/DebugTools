namespace DebugTools.Common.Viewer.CodeViewer
{
    partial class CodeViewerFrame
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
            this.tabCodeViewer = new System.Windows.Forms.TabControl();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabCodeViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabCodeViewer.Location = new System.Drawing.Point(0, 0);
            this.tabCodeViewer.Name = "tabControl1";
            this.tabCodeViewer.SelectedIndex = 0;
            this.tabCodeViewer.Size = new System.Drawing.Size(601, 385);
            this.tabCodeViewer.TabIndex = 3;
            // 
            // CodeViewerFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabCodeViewer);
            this.Name = "CodeViewerFrame";
            this.Size = new System.Drawing.Size(601, 385);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabCodeViewer;
    }
}
