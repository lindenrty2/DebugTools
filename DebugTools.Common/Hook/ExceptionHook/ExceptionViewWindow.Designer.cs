namespace DebugTools.Common.Hook.ExceptionHook
{
    partial class ExceptionViewWindow
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
            this.tabInfo = new System.Windows.Forms.TabControl();
            this.tabProfile = new System.Windows.Forms.TabPage();
            this.exceptionViewer1 = new DebugTools.Common.Hook.ExceptionHook.ExceptionViewer();
            this.tabObjectView = new System.Windows.Forms.TabPage();
            this.objectViewerFrame1 = new DebugTools.Common.ObjectViewer.ObjectViewerFrame();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabInfo.SuspendLayout();
            this.tabProfile.SuspendLayout();
            this.tabObjectView.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabInfo
            // 
            this.tabInfo.Controls.Add(this.tabProfile);
            this.tabInfo.Controls.Add(this.tabObjectView);
            this.tabInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabInfo.Location = new System.Drawing.Point(0, 24);
            this.tabInfo.Name = "tabInfo";
            this.tabInfo.SelectedIndex = 0;
            this.tabInfo.Size = new System.Drawing.Size(660, 329);
            this.tabInfo.TabIndex = 0;
            // 
            // tabProfile
            // 
            this.tabProfile.Controls.Add(this.exceptionViewer1);
            this.tabProfile.Location = new System.Drawing.Point(4, 22);
            this.tabProfile.Name = "tabProfile";
            this.tabProfile.Padding = new System.Windows.Forms.Padding(3);
            this.tabProfile.Size = new System.Drawing.Size(652, 303);
            this.tabProfile.TabIndex = 0;
            this.tabProfile.Text = "概要";
            this.tabProfile.UseVisualStyleBackColor = true;
            // 
            // exceptionViewer1
            // 
            this.exceptionViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.exceptionViewer1.Location = new System.Drawing.Point(3, 3);
            this.exceptionViewer1.Name = "exceptionViewer1";
            this.exceptionViewer1.Size = new System.Drawing.Size(646, 297);
            this.exceptionViewer1.TabIndex = 0;
            // 
            // tabObjectView
            // 
            this.tabObjectView.Controls.Add(this.objectViewerFrame1);
            this.tabObjectView.Location = new System.Drawing.Point(4, 22);
            this.tabObjectView.Name = "tabObjectView";
            this.tabObjectView.Padding = new System.Windows.Forms.Padding(3);
            this.tabObjectView.Size = new System.Drawing.Size(652, 303);
            this.tabObjectView.TabIndex = 1;
            this.tabObjectView.Text = "詳細情報";
            this.tabObjectView.UseVisualStyleBackColor = true;
            // 
            // objectViewerFrame1
            // 
            this.objectViewerFrame1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectViewerFrame1.Location = new System.Drawing.Point(3, 3);
            this.objectViewerFrame1.Name = "objectViewerFrame1";
            this.objectViewerFrame1.Size = new System.Drawing.Size(646, 297);
            this.objectViewerFrame1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(660, 24);
            this.panel1.TabIndex = 1;
            // 
            // ExceptionViewWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(660, 353);
            this.Controls.Add(this.tabInfo);
            this.Controls.Add(this.panel1);
            this.Name = "ExceptionViewWindow";
            this.Text = "Exception";
            this.tabInfo.ResumeLayout(false);
            this.tabProfile.ResumeLayout(false);
            this.tabObjectView.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabInfo;
        private System.Windows.Forms.TabPage tabProfile;
        private DebugTools.Common.Hook.ExceptionHook.ExceptionViewer exceptionViewer1;
        private System.Windows.Forms.TabPage tabObjectView;
        private ObjectViewer.ObjectViewerFrame objectViewerFrame1;
        private System.Windows.Forms.Panel panel1;
    }
}