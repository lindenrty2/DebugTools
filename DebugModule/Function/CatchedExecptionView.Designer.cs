namespace DebugModule.Function
{
    partial class CatchedExecptionView
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
            this.lvExecptionList = new System.Windows.Forms.ListView();
            this.colIndex = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colUntreated = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colMessage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colComment = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.tsmControl = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmClear = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmClose = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvExecptionList
            // 
            this.lvExecptionList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colIndex,
            this.colUntreated,
            this.colTime,
            this.colName,
            this.colMessage,
            this.colComment});
            this.lvExecptionList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvExecptionList.FullRowSelect = true;
            this.lvExecptionList.Location = new System.Drawing.Point(0, 26);
            this.lvExecptionList.MultiSelect = false;
            this.lvExecptionList.Name = "lvExecptionList";
            this.lvExecptionList.Size = new System.Drawing.Size(644, 333);
            this.lvExecptionList.TabIndex = 0;
            this.lvExecptionList.UseCompatibleStateImageBehavior = false;
            this.lvExecptionList.View = System.Windows.Forms.View.Details;
            this.lvExecptionList.DoubleClick += new System.EventHandler(this.lvExecptionList_DoubleClick);
            // 
            // colIndex
            // 
            this.colIndex.Text = "順番";
            this.colIndex.Width = 40;
            // 
            // colUntreated
            // 
            this.colUntreated.Text = "未処理";
            this.colUntreated.Width = 54;
            // 
            // colTime
            // 
            this.colTime.Text = "発生時間";
            // 
            // colName
            // 
            this.colName.Text = "名称";
            this.colName.Width = 174;
            // 
            // colMessage
            // 
            this.colMessage.Text = "メッセージ";
            this.colMessage.Width = 226;
            // 
            // colComment
            // 
            this.colComment.Text = "コメント";
            this.colComment.Width = 86;
            // 
            // menuMain
            // 
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmControl});
            this.menuMain.Location = new System.Drawing.Point(0, 0);
            this.menuMain.Name = "menuMain";
            this.menuMain.Size = new System.Drawing.Size(644, 26);
            this.menuMain.TabIndex = 1;
            this.menuMain.Text = "menuStrip1";
            // 
            // tsmControl
            // 
            this.tsmControl.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmClear,
            this.tsmClose});
            this.tsmControl.Name = "tsmControl";
            this.tsmControl.Size = new System.Drawing.Size(56, 22);
            this.tsmControl.Text = "メイン";
            // 
            // tsmClear
            // 
            this.tsmClear.Name = "tsmClear";
            this.tsmClear.Size = new System.Drawing.Size(152, 22);
            this.tsmClear.Text = "クリア";
            this.tsmClear.Click += new System.EventHandler(this.tsmClear_Click);
            // 
            // tsmClose
            // 
            this.tsmClose.Name = "tsmClose";
            this.tsmClose.Size = new System.Drawing.Size(152, 22);
            this.tsmClose.Text = "閉じる";
            this.tsmClose.Click += new System.EventHandler(this.tsmClose_Click);
            // 
            // CatchedExecptionView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(644, 359);
            this.Controls.Add(this.lvExecptionList);
            this.Controls.Add(this.menuMain);
            this.Name = "CatchedExecptionView";
            this.Text = "捕捉されたExecptionの一覧";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CatchedExecptionView_FormClosed);
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvExecptionList;
        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem tsmControl;
        private System.Windows.Forms.ToolStripMenuItem tsmClear;
        private System.Windows.Forms.ToolStripMenuItem tsmClose;
        private System.Windows.Forms.ColumnHeader colIndex;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colMessage;
        private System.Windows.Forms.ColumnHeader colUntreated;
        private System.Windows.Forms.ColumnHeader colComment;
        private System.Windows.Forms.ColumnHeader colTime;
    }
}