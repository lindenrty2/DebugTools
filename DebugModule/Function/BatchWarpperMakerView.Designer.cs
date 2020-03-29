namespace DebugModule.Function
{
    partial class BatchWarpperMakerView
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
            this.txtTarget = new DebugTools.Common.Window.SelectPathControl();
            this.rdoSkip = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.rdoOverride = new System.Windows.Forms.RadioButton();
            this.rdoAlert = new System.Windows.Forms.RadioButton();
            this.spCreatePath = new DebugTools.Common.Window.SelectPathControl();
            this.txtNameSpace = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCreate = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.codeViewerFrame1 = new DebugTools.Common.Viewer.CodeViewer.CodeViewerFrame();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel3 = new System.Windows.Forms.Panel();
            this.chklTargetDetailList = new System.Windows.Forms.CheckedListBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnUnSelectAll = new System.Windows.Forms.Button();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtTarget);
            this.panel1.Controls.Add(this.rdoSkip);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.rdoOverride);
            this.panel1.Controls.Add(this.rdoAlert);
            this.panel1.Controls.Add(this.spCreatePath);
            this.panel1.Controls.Add(this.txtNameSpace);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btnCreate);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(655, 126);
            this.panel1.TabIndex = 9;
            // 
            // txtTarget
            // 
            this.txtTarget.Filter = "";
            this.txtTarget.Location = new System.Drawing.Point(84, 13);
            this.txtTarget.Name = "txtTarget";
            this.txtTarget.SelectedPath = "";
            this.txtTarget.SelectPathType = DebugTools.Common.Window.SelectPathType.OpenFile;
            this.txtTarget.Size = new System.Drawing.Size(473, 19);
            this.txtTarget.TabIndex = 19;
            this.txtTarget.PathChanged += new DebugTools.Common.Window.PathChangedEventHander(this.txtTarget_PathChanged);
            // 
            // rdoSkip
            // 
            this.rdoSkip.AutoSize = true;
            this.rdoSkip.Location = new System.Drawing.Point(186, 64);
            this.rdoSkip.Name = "rdoSkip";
            this.rdoSkip.Size = new System.Drawing.Size(107, 16);
            this.rdoSkip.TabIndex = 18;
            this.rdoSkip.Text = "すべでスキップする";
            this.rdoSkip.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 66);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 12);
            this.label4.TabIndex = 17;
            this.label4.Text = "既存の場合";
            // 
            // rdoOverride
            // 
            this.rdoOverride.AutoSize = true;
            this.rdoOverride.Location = new System.Drawing.Point(84, 64);
            this.rdoOverride.Name = "rdoOverride";
            this.rdoOverride.Size = new System.Drawing.Size(96, 16);
            this.rdoOverride.TabIndex = 16;
            this.rdoOverride.Text = "すべで上書する";
            this.rdoOverride.UseVisualStyleBackColor = true;
            // 
            // rdoAlert
            // 
            this.rdoAlert.AutoSize = true;
            this.rdoAlert.Checked = true;
            this.rdoAlert.Location = new System.Drawing.Point(299, 64);
            this.rdoAlert.Name = "rdoAlert";
            this.rdoAlert.Size = new System.Drawing.Size(47, 16);
            this.rdoAlert.TabIndex = 15;
            this.rdoAlert.TabStop = true;
            this.rdoAlert.Text = "提示";
            this.rdoAlert.UseVisualStyleBackColor = true;
            // 
            // spCreatePath
            // 
            this.spCreatePath.Filter = "";
            this.spCreatePath.Location = new System.Drawing.Point(84, 39);
            this.spCreatePath.Name = "spCreatePath";
            this.spCreatePath.SelectedPath = "";
            this.spCreatePath.SelectPathType = DebugTools.Common.Window.SelectPathType.Dictionary;
            this.spCreatePath.Size = new System.Drawing.Size(473, 19);
            this.spCreatePath.TabIndex = 14;
            // 
            // txtNameSpace
            // 
            this.txtNameSpace.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNameSpace.Location = new System.Drawing.Point(84, 85);
            this.txtNameSpace.Name = "txtNameSpace";
            this.txtNameSpace.Size = new System.Drawing.Size(473, 19);
            this.txtNameSpace.TabIndex = 13;
            this.txtNameSpace.Text = "DebugTools.MaroonWarpper";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 89);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 12;
            this.label3.Text = "NameSpace";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "作成パス";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "作成対象";
            // 
            // btnCreate
            // 
            this.btnCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreate.Location = new System.Drawing.Point(564, 11);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(79, 69);
            this.btnCreate.TabIndex = 7;
            this.btnCreate.Text = "作成";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.codeViewerFrame1);
            this.panel2.Controls.Add(this.splitter1);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 126);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(10);
            this.panel2.Size = new System.Drawing.Size(655, 313);
            this.panel2.TabIndex = 10;
            // 
            // codeViewerFrame1
            // 
            this.codeViewerFrame1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.codeViewerFrame1.Location = new System.Drawing.Point(162, 10);
            this.codeViewerFrame1.Name = "codeViewerFrame1";
            this.codeViewerFrame1.Size = new System.Drawing.Size(483, 293);
            this.codeViewerFrame1.TabIndex = 10;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(157, 10);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(5, 293);
            this.splitter1.TabIndex = 11;
            this.splitter1.TabStop = false;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.chklTargetDetailList);
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(10, 10);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(147, 293);
            this.panel3.TabIndex = 13;
            // 
            // chklTargetDetailList
            // 
            this.chklTargetDetailList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chklTargetDetailList.FormattingEnabled = true;
            this.chklTargetDetailList.Location = new System.Drawing.Point(0, 24);
            this.chklTargetDetailList.Name = "chklTargetDetailList";
            this.chklTargetDetailList.Size = new System.Drawing.Size(147, 269);
            this.chklTargetDetailList.TabIndex = 13;
            this.chklTargetDetailList.SelectedIndexChanged += new System.EventHandler(this.chklTargetDetailList_SelectedIndexChanged);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnUnSelectAll);
            this.panel4.Controls.Add(this.btnSelectAll);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(147, 24);
            this.panel4.TabIndex = 14;
            // 
            // btnUnSelectAll
            // 
            this.btnUnSelectAll.Location = new System.Drawing.Point(81, 0);
            this.btnUnSelectAll.Name = "btnUnSelectAll";
            this.btnUnSelectAll.Size = new System.Drawing.Size(66, 24);
            this.btnUnSelectAll.TabIndex = 15;
            this.btnUnSelectAll.Text = "全解除";
            this.btnUnSelectAll.UseVisualStyleBackColor = true;
            this.btnUnSelectAll.Click += new System.EventHandler(this.btnUnSelectAll_Click);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Location = new System.Drawing.Point(0, 0);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(66, 24);
            this.btnSelectAll.TabIndex = 14;
            this.btnSelectAll.Text = "全選択";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // BatchWarpperMakerView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(655, 439);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "BatchWarpperMakerView";
            this.Text = "Warpperクラス作成";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtNameSpace;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Panel panel2;
        private DebugTools.Common.Viewer.CodeViewer.CodeViewerFrame codeViewerFrame1;
        private System.Windows.Forms.Splitter splitter1;
        private DebugTools.Common.Window.SelectPathControl spCreatePath;
        private System.Windows.Forms.RadioButton rdoSkip;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton rdoOverride;
        private System.Windows.Forms.RadioButton rdoAlert;
        private DebugTools.Common.Window.SelectPathControl txtTarget;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.CheckedListBox chklTargetDetailList;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnUnSelectAll;
        private System.Windows.Forms.Button btnSelectAll;

    }
}