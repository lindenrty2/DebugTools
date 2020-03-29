namespace DebugModule.Function
{
    partial class WarpperMakerView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.codeViewerFrame1 = new DebugTools.Common.Viewer.CodeViewer.CodeViewerFrame();
            this.SuspendLayout();
            // 
            // codeViewerFrame1
            // 
            this.codeViewerFrame1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.codeViewerFrame1.Location = new System.Drawing.Point(0, 0);
            this.codeViewerFrame1.Name = "codeViewerFrame1";
            this.codeViewerFrame1.Size = new System.Drawing.Size(688, 501);
            this.codeViewerFrame1.TabIndex = 0;
            // 
            // WarpperMakerView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(688, 501);
            this.Controls.Add(this.codeViewerFrame1);
            this.Name = "WarpperMakerView";
            this.Text = "WarpperMaker";
            this.ResumeLayout(false);

        }

        #endregion

        private DebugTools.Common.Viewer.CodeViewer.CodeViewerFrame codeViewerFrame1;

    }
}