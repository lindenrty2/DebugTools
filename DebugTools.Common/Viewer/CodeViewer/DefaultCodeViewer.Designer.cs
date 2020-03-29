namespace DebugTools.Common.Viewer.CodeViewer
{
    partial class DefaultCodeViewer
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
            this.rtxtCode = new System.Windows.Forms.RichTextBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.tvMember = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // rtxtCode
            // 
            this.rtxtCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtxtCode.Location = new System.Drawing.Point(157, 0);
            this.rtxtCode.Name = "rtxtCode";
            this.rtxtCode.Size = new System.Drawing.Size(497, 513);
            this.rtxtCode.TabIndex = 0;
            this.rtxtCode.Text = "";
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(157, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 513);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            // 
            // tvMember
            // 
            this.tvMember.Dock = System.Windows.Forms.DockStyle.Left;
            this.tvMember.Location = new System.Drawing.Point(0, 0);
            this.tvMember.Name = "tvMember";
            this.tvMember.Size = new System.Drawing.Size(157, 513);
            this.tvMember.TabIndex = 2;
            // 
            // DefaultCodeViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.rtxtCode);
            this.Controls.Add(this.tvMember);
            this.Name = "DefaultCodeViewer";
            this.Size = new System.Drawing.Size(654, 513);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtxtCode;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.TreeView tvMember;
    }
}
