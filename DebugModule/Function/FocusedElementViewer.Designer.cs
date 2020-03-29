namespace DebugModule
{
    partial class FocusedElementViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FocusedElementViewer));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.tscMode = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.tscTarget = new System.Windows.Forms.ToolStripComboBox();
            this.tsbCtrl = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.objectViewer = new DebugTools.Common.ObjectViewer.ObjectViewerFrame();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.tvObjectView = new System.Windows.Forms.TreeView();
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.tsslTargetType = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.statusBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.tscMode,
            this.toolStripLabel2,
            this.tscTarget,
            this.tsbCtrl});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(755, 26);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(44, 23);
            this.toolStripLabel1.Text = "モード";
            // 
            // tscMode
            // 
            this.tscMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tscMode.Name = "tscMode";
            this.tscMode.Size = new System.Drawing.Size(121, 26);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(68, 23);
            this.toolStripLabel2.Text = "目標タイプ";
            // 
            // tscTarget
            // 
            this.tscTarget.AutoCompleteCustomSource.AddRange(new string[] {
            "System.Windows.Visual",
            "System.Windows.UIElement",
            "System.Windows.FrameWorkElement",
            "System.Windows.Control",
            "System.Windows.Window"});
            this.tscTarget.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tscTarget.Items.AddRange(new object[] {
            "Visual",
            "UIElement",
            "FrameworkElement",
            "Control",
            "Window"});
            this.tscTarget.Name = "tscTarget";
            this.tscTarget.Size = new System.Drawing.Size(121, 26);
            this.tscTarget.SelectedIndexChanged += new System.EventHandler(this.tscTarget_SelectedIndexChanged);
            // 
            // tsbCtrl
            // 
            this.tsbCtrl.CheckOnClick = true;
            this.tsbCtrl.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbCtrl.Image = ((System.Drawing.Image)(resources.GetObject("tsbCtrl.Image")));
            this.tsbCtrl.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCtrl.Name = "tsbCtrl";
            this.tsbCtrl.Size = new System.Drawing.Size(57, 23);
            this.tsbCtrl.Text = "Ctrl選択";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.objectViewer);
            this.panel1.Controls.Add(this.splitter1);
            this.panel1.Controls.Add(this.tvObjectView);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 26);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(755, 409);
            this.panel1.TabIndex = 5;
            // 
            // objectViewer
            // 
            this.objectViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectViewer.Location = new System.Drawing.Point(182, 0);
            this.objectViewer.Name = "objectViewer";
            this.objectViewer.Size = new System.Drawing.Size(573, 409);
            this.objectViewer.TabIndex = 7;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(174, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(8, 409);
            this.splitter1.TabIndex = 6;
            this.splitter1.TabStop = false;
            // 
            // tvObjectView
            // 
            this.tvObjectView.Dock = System.Windows.Forms.DockStyle.Left;
            this.tvObjectView.Location = new System.Drawing.Point(0, 0);
            this.tvObjectView.Name = "tvObjectView";
            this.tvObjectView.Size = new System.Drawing.Size(174, 409);
            this.tvObjectView.TabIndex = 5;
            this.tvObjectView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvObjectView_AfterSelect);
            // 
            // statusBar
            // 
            this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslTargetType});
            this.statusBar.Location = new System.Drawing.Point(0, 435);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(755, 22);
            this.statusBar.TabIndex = 6;
            this.statusBar.Text = "statusStrip1";
            // 
            // tsslTargetType
            // 
            this.tsslTargetType.Name = "tsslTargetType";
            this.tsslTargetType.Size = new System.Drawing.Size(0, 17);
            // 
            // FocusedElementViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(755, 457);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusBar);
            this.Name = "FocusedElementViewer";
            this.Text = "監視画面";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FocusedElementViewer_FormClosed);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.statusBar.ResumeLayout(false);
            this.statusBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.TreeView tvObjectView;
        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.ToolStripComboBox tscMode;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripStatusLabel tsslTargetType;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripComboBox tscTarget;
        private System.Windows.Forms.ToolStripButton tsbCtrl;
        private DebugTools.Common.ObjectViewer.ObjectViewerFrame objectViewer;


    }
}