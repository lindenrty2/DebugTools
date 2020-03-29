namespace HttpProxyServer
{
    partial class DebugForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.txtLocal = new System.Windows.Forms.RichTextBox();
            this.pgLocal = new System.Windows.Forms.PropertyGrid();
            this.panel5 = new System.Windows.Forms.Panel();
            this.lblLocal = new System.Windows.Forms.TextBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel3 = new System.Windows.Forms.Panel();
            this.txtServer = new System.Windows.Forms.RichTextBox();
            this.pgServer = new System.Windows.Forms.PropertyGrid();
            this.panel6 = new System.Windows.Forms.Panel();
            this.lblRemote = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblIP = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSaveOK = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.pnlCache = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.splitter1);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1022, 572);
            this.panel1.TabIndex = 3;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.txtLocal);
            this.panel4.Controls.Add(this.pgLocal);
            this.panel4.Controls.Add(this.panel5);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(508, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(514, 572);
            this.panel4.TabIndex = 7;
            // 
            // txtLocal
            // 
            this.txtLocal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLocal.Location = new System.Drawing.Point(0, 160);
            this.txtLocal.Name = "txtLocal";
            this.txtLocal.Size = new System.Drawing.Size(514, 412);
            this.txtLocal.TabIndex = 7;
            this.txtLocal.Text = "";
            // 
            // pgLocal
            // 
            this.pgLocal.Dock = System.Windows.Forms.DockStyle.Top;
            this.pgLocal.HelpVisible = false;
            this.pgLocal.Location = new System.Drawing.Point(0, 30);
            this.pgLocal.Name = "pgLocal";
            this.pgLocal.Size = new System.Drawing.Size(514, 130);
            this.pgLocal.TabIndex = 11;
            this.pgLocal.ToolbarVisible = false;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.lblLocal);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(514, 30);
            this.panel5.TabIndex = 6;
            // 
            // lblLocal
            // 
            this.lblLocal.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLocal.Location = new System.Drawing.Point(7, 4);
            this.lblLocal.Name = "lblLocal";
            this.lblLocal.ReadOnly = true;
            this.lblLocal.Size = new System.Drawing.Size(500, 21);
            this.lblLocal.TabIndex = 0;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(505, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 572);
            this.splitter1.TabIndex = 5;
            this.splitter1.TabStop = false;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.txtServer);
            this.panel3.Controls.Add(this.pgServer);
            this.panel3.Controls.Add(this.panel6);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(505, 572);
            this.panel3.TabIndex = 6;
            // 
            // txtServer
            // 
            this.txtServer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtServer.Location = new System.Drawing.Point(0, 160);
            this.txtServer.Name = "txtServer";
            this.txtServer.ReadOnly = true;
            this.txtServer.Size = new System.Drawing.Size(505, 412);
            this.txtServer.TabIndex = 9;
            this.txtServer.Text = "";
            // 
            // pgServer
            // 
            this.pgServer.Dock = System.Windows.Forms.DockStyle.Top;
            this.pgServer.HelpVisible = false;
            this.pgServer.Location = new System.Drawing.Point(0, 30);
            this.pgServer.Name = "pgServer";
            this.pgServer.Size = new System.Drawing.Size(505, 130);
            this.pgServer.TabIndex = 10;
            this.pgServer.ToolbarVisible = false;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.lblRemote);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(505, 30);
            this.panel6.TabIndex = 7;
            // 
            // lblRemote
            // 
            this.lblRemote.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRemote.Location = new System.Drawing.Point(12, 4);
            this.lblRemote.Name = "lblRemote";
            this.lblRemote.ReadOnly = true;
            this.lblRemote.Size = new System.Drawing.Size(486, 21);
            this.lblRemote.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblIP);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.btnSaveOK);
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Controls.Add(this.pnlCache);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 572);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1022, 147);
            this.panel2.TabIndex = 4;
            // 
            // lblIP
            // 
            this.lblIP.AutoSize = true;
            this.lblIP.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblIP.Location = new System.Drawing.Point(12, 103);
            this.lblIP.Name = "lblIP";
            this.lblIP.Size = new System.Drawing.Size(0, 21);
            this.lblIP.TabIndex = 5;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCancel.Location = new System.Drawing.Point(552, 95);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(143, 36);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "放弃修改";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSaveOK
            // 
            this.btnSaveOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveOK.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.btnSaveOK.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSaveOK.Location = new System.Drawing.Point(712, 95);
            this.btnSaveOK.Name = "btnSaveOK";
            this.btnSaveOK.Size = new System.Drawing.Size(170, 36);
            this.btnSaveOK.TabIndex = 1;
            this.btnSaveOK.Text = "保存并确定";
            this.btnSaveOK.UseVisualStyleBackColor = true;
            this.btnSaveOK.Click += new System.EventHandler(this.btnSaveOK_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOK.Location = new System.Drawing.Point(892, 95);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(117, 36);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // pnlCache
            // 
            this.pnlCache.Location = new System.Drawing.Point(12, 11);
            this.pnlCache.Name = "pnlCache";
            this.pnlCache.Size = new System.Drawing.Size(997, 78);
            this.pnlCache.TabIndex = 4;
            // 
            // DebugForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(1022, 719);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Name = "DebugForm";
            this.Text = "DebugForm";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.DebugForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnSaveOK;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.TextBox lblLocal;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.TextBox lblRemote;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.RichTextBox txtLocal;
        private System.Windows.Forms.RichTextBox txtServer;
        private System.Windows.Forms.PropertyGrid pgServer;
        private System.Windows.Forms.PropertyGrid pgLocal;
        private System.Windows.Forms.FlowLayoutPanel pnlCache;
        private System.Windows.Forms.Label lblIP;

    }
}