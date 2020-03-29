namespace DebugModule
{
    partial class AssembliesView
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
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel3 = new System.Windows.Forms.Panel();
            this.itemView = new System.Windows.Forms.TreeView();
            this.cmsFunctionMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsMakeWarpper = new System.Windows.Forms.ToolStripMenuItem();
            this.panel2 = new System.Windows.Forms.Panel();
            this.txtFilter = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.cmsFunctionMenu.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.propertyGrid);
            this.panel1.Controls.Add(this.splitter1);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(10);
            this.panel1.Size = new System.Drawing.Size(854, 465);
            this.panel1.TabIndex = 0;
            // 
            // propertyGrid
            // 
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.Location = new System.Drawing.Point(218, 10);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(626, 445);
            this.propertyGrid.TabIndex = 10;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(210, 10);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(8, 445);
            this.splitter1.TabIndex = 12;
            this.splitter1.TabStop = false;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.itemView);
            this.panel3.Controls.Add(this.panel2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(10, 10);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(200, 445);
            this.panel3.TabIndex = 13;
            // 
            // itemView
            // 
            this.itemView.ContextMenuStrip = this.cmsFunctionMenu;
            this.itemView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.itemView.Location = new System.Drawing.Point(0, 27);
            this.itemView.Name = "itemView";
            this.itemView.Size = new System.Drawing.Size(200, 418);
            this.itemView.TabIndex = 7;
            this.itemView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.itemView_AfterSelect);
            // 
            // cmsFunctionMenu
            // 
            this.cmsFunctionMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsMakeWarpper});
            this.cmsFunctionMenu.Name = "cmsFunctionMenu";
            this.cmsFunctionMenu.Size = new System.Drawing.Size(186, 26);
            // 
            // tsMakeWarpper
            // 
            this.tsMakeWarpper.Name = "tsMakeWarpper";
            this.tsMakeWarpper.Size = new System.Drawing.Size(185, 22);
            this.tsMakeWarpper.Text = "Warpperを作成する";
            this.tsMakeWarpper.Click += new System.EventHandler(this.tsMakeWarpper_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.txtFilter);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(200, 27);
            this.panel2.TabIndex = 10;
            // 
            // txtFilter
            // 
            this.txtFilter.Location = new System.Drawing.Point(1, 3);
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.Size = new System.Drawing.Size(199, 19);
            this.txtFilter.TabIndex = 14;
            this.txtFilter.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFilter_KeyPress);
            // 
            // AssembliesView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(854, 465);
            this.Controls.Add(this.panel1);
            this.Name = "AssembliesView";
            this.Text = "アセンブリ ブラウザ";
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.cmsFunctionMenu.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TreeView itemView;
        private System.Windows.Forms.ContextMenuStrip cmsFunctionMenu;
        private System.Windows.Forms.ToolStripMenuItem tsMakeWarpper;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox txtFilter;



    }
}