namespace HttpProxyServer
{
    partial class DetailForm
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
            this.lblUrl = new System.Windows.Forms.TextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.lblIP = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnSendAgain = new System.Windows.Forms.Button();
            this.bnClose = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.txtRequestBody = new System.Windows.Forms.RichTextBox();
            this.splitter3 = new System.Windows.Forms.Splitter();
            this.pgRequest = new System.Windows.Forms.PropertyGrid();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel4 = new System.Windows.Forms.Panel();
            this.txtResponseBody = new System.Windows.Forms.RichTextBox();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.pgResponse = new System.Windows.Forms.PropertyGrid();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblUrl);
            this.panel1.Controls.Add(this.lblStatus);
            this.panel1.Controls.Add(this.lblTime);
            this.panel1.Controls.Add(this.lblIP);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1017, 95);
            this.panel1.TabIndex = 2;
            // 
            // lblUrl
            // 
            this.lblUrl.Location = new System.Drawing.Point(21, 51);
            this.lblUrl.Multiline = true;
            this.lblUrl.Name = "lblUrl";
            this.lblUrl.ReadOnly = true;
            this.lblUrl.Size = new System.Drawing.Size(974, 28);
            this.lblUrl.TabIndex = 5;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(512, 19);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 19);
            this.lblStatus.TabIndex = 2;
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Location = new System.Drawing.Point(274, 19);
            this.lblTime.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(0, 19);
            this.lblTime.TabIndex = 1;
            // 
            // lblIP
            // 
            this.lblIP.AutoSize = true;
            this.lblIP.Location = new System.Drawing.Point(22, 19);
            this.lblIP.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblIP.Name = "lblIP";
            this.lblIP.Size = new System.Drawing.Size(0, 19);
            this.lblIP.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnSendAgain);
            this.panel2.Controls.Add(this.bnClose);
            this.panel2.Controls.Add(this.btnDelete);
            this.panel2.Controls.Add(this.btnSave);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 643);
            this.panel2.Margin = new System.Windows.Forms.Padding(5);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1017, 87);
            this.panel2.TabIndex = 4;
            // 
            // btnSendAgain
            // 
            this.btnSendAgain.Location = new System.Drawing.Point(433, 33);
            this.btnSendAgain.Name = "btnSendAgain";
            this.btnSendAgain.Size = new System.Drawing.Size(110, 42);
            this.btnSendAgain.TabIndex = 3;
            this.btnSendAgain.Text = "再次请求";
            this.btnSendAgain.UseVisualStyleBackColor = true;
            this.btnSendAgain.Click += new System.EventHandler(this.btnSendAgain_Click);
            // 
            // bnClose
            // 
            this.bnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bnClose.Location = new System.Drawing.Point(12, 33);
            this.bnClose.Name = "bnClose";
            this.bnClose.Size = new System.Drawing.Size(110, 42);
            this.bnClose.TabIndex = 2;
            this.bnClose.Text = "关闭";
            this.bnClose.UseVisualStyleBackColor = true;
            this.bnClose.Click += new System.EventHandler(this.bnClose_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.btnDelete.Location = new System.Drawing.Point(757, 33);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(110, 42);
            this.btnDelete.TabIndex = 1;
            this.btnDelete.Text = "删除缓存";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnSave
            // 
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.btnSave.Location = new System.Drawing.Point(895, 33);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(110, 42);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "缓存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.txtRequestBody);
            this.panel3.Controls.Add(this.splitter3);
            this.panel3.Controls.Add(this.pgRequest);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 95);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1017, 165);
            this.panel3.TabIndex = 5;
            // 
            // txtRequestBody
            // 
            this.txtRequestBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRequestBody.Location = new System.Drawing.Point(298, 0);
            this.txtRequestBody.Name = "txtRequestBody";
            this.txtRequestBody.Size = new System.Drawing.Size(719, 165);
            this.txtRequestBody.TabIndex = 1;
            this.txtRequestBody.Text = "";
            // 
            // splitter3
            // 
            this.splitter3.Location = new System.Drawing.Point(295, 0);
            this.splitter3.Name = "splitter3";
            this.splitter3.Size = new System.Drawing.Size(3, 165);
            this.splitter3.TabIndex = 3;
            this.splitter3.TabStop = false;
            // 
            // pgRequest
            // 
            this.pgRequest.Dock = System.Windows.Forms.DockStyle.Left;
            this.pgRequest.HelpVisible = false;
            this.pgRequest.Location = new System.Drawing.Point(0, 0);
            this.pgRequest.Name = "pgRequest";
            this.pgRequest.Size = new System.Drawing.Size(295, 165);
            this.pgRequest.TabIndex = 4;
            this.pgRequest.ToolbarVisible = false;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point(0, 260);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(1017, 3);
            this.splitter1.TabIndex = 6;
            this.splitter1.TabStop = false;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.txtResponseBody);
            this.panel4.Controls.Add(this.splitter2);
            this.panel4.Controls.Add(this.pgResponse);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 263);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1017, 380);
            this.panel4.TabIndex = 7;
            // 
            // txtResponseBody
            // 
            this.txtResponseBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtResponseBody.Location = new System.Drawing.Point(298, 0);
            this.txtResponseBody.Name = "txtResponseBody";
            this.txtResponseBody.Size = new System.Drawing.Size(719, 380);
            this.txtResponseBody.TabIndex = 1;
            this.txtResponseBody.Text = "";
            // 
            // splitter2
            // 
            this.splitter2.Location = new System.Drawing.Point(295, 0);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(3, 380);
            this.splitter2.TabIndex = 2;
            this.splitter2.TabStop = false;
            // 
            // pgResponse
            // 
            this.pgResponse.Dock = System.Windows.Forms.DockStyle.Left;
            this.pgResponse.HelpVisible = false;
            this.pgResponse.Location = new System.Drawing.Point(0, 0);
            this.pgResponse.Name = "pgResponse";
            this.pgResponse.Size = new System.Drawing.Size(295, 380);
            this.pgResponse.TabIndex = 6;
            this.pgResponse.ToolbarVisible = false;
            // 
            // DetailForm
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1017, 730);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "DetailForm";
            this.Text = "DetailForm";
            this.Load += new System.EventHandler(this.DetailForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label lblIP;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button bnClose;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.RichTextBox txtRequestBody;
        private System.Windows.Forms.Splitter splitter3;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.RichTextBox txtResponseBody;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.Button btnSendAgain;
        private System.Windows.Forms.TextBox lblUrl;
        private System.Windows.Forms.PropertyGrid pgRequest;
        private System.Windows.Forms.PropertyGrid pgResponse;
    }
}