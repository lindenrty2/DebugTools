namespace DebugTools.ControlCenter.Config
{
    partial class BaseSettingControl
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
            this.spMainAppDirectory = new DebugTools.Common.Window.SelectPathControl();
            this.lblMainPath = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // spMainAppDirectory
            // 
            this.spMainAppDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.spMainAppDirectory.Filter = "";
            this.spMainAppDirectory.Location = new System.Drawing.Point(20, 42);
            this.spMainAppDirectory.Name = "spMainAppDirectory";
            this.spMainAppDirectory.SelectedPath = "";
            this.spMainAppDirectory.SelectPathType = DebugTools.Common.Window.SelectPathType.Dictionary;
            this.spMainAppDirectory.Size = new System.Drawing.Size(606, 22);
            this.spMainAppDirectory.TabIndex = 0;
            // 
            // lblMainPath
            // 
            this.lblMainPath.AutoSize = true;
            this.lblMainPath.Location = new System.Drawing.Point(15, 25);
            this.lblMainPath.Name = "lblMainPath";
            this.lblMainPath.Size = new System.Drawing.Size(103, 12);
            this.lblMainPath.TabIndex = 1;
            this.lblMainPath.Text = "Marooonディレクトリ：";
            // 
            // BaseSettingControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblMainPath);
            this.Controls.Add(this.spMainAppDirectory);
            this.Name = "BaseSettingControl";
            this.Size = new System.Drawing.Size(647, 409);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DebugTools.Common.Window.SelectPathControl spMainAppDirectory;
        private System.Windows.Forms.Label lblMainPath;
    }
}
