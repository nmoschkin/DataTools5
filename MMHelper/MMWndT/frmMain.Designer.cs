namespace MMWndT
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.status = new System.Windows.Forms.StatusStrip();
            this.tss1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tss2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnWatch = new System.Windows.Forms.Button();
            this.notify = new System.Windows.Forms.NotifyIcon(this.components);
            this.cmenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuRestore = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuEnable = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDisable = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuQuit = new System.Windows.Forms.ToolStripMenuItem();
            this.lstEvents = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.colStatus = new System.Windows.Forms.ColumnHeader();
            this.colMonitor = new System.Windows.Forms.ColumnHeader();
            this.colTime = new System.Windows.Forms.ColumnHeader();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.btnQuit = new System.Windows.Forms.Button();
            this.status.SuspendLayout();
            this.cmenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // status
            // 
            this.status.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tss1,
            this.tss2});
            this.status.Location = new System.Drawing.Point(0, 497);
            this.status.Name = "status";
            this.status.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.status.Size = new System.Drawing.Size(643, 22);
            this.status.TabIndex = 2;
            this.status.Text = "statusStrip1";
            // 
            // tss1
            // 
            this.tss1.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.tss1.Name = "tss1";
            this.tss1.Size = new System.Drawing.Size(4, 17);
            // 
            // tss2
            // 
            this.tss2.Name = "tss2";
            this.tss2.Size = new System.Drawing.Size(0, 17);
            // 
            // btnWatch
            // 
            this.btnWatch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnWatch.Location = new System.Drawing.Point(13, 460);
            this.btnWatch.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnWatch.Name = "btnWatch";
            this.btnWatch.Size = new System.Drawing.Size(136, 27);
            this.btnWatch.TabIndex = 3;
            this.btnWatch.Text = "Start Watching Shell";
            this.btnWatch.UseVisualStyleBackColor = true;
            this.btnWatch.Click += new System.EventHandler(this.btnToggle_Click);
            // 
            // notify
            // 
            this.notify.ContextMenuStrip = this.cmenu;
            this.notify.Icon = ((System.Drawing.Icon)(resources.GetObject("notify.Icon")));
            this.notify.Text = "Multi-Monitor Dynamic Window Positioner";
            this.notify.Visible = true;
            this.notify.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notify_DoubleClick);
            // 
            // cmenu
            // 
            this.cmenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuRestore,
            this.toolStripSeparator1,
            this.mnuEnable,
            this.mnuDisable,
            this.toolStripSeparator2,
            this.mnuQuit});
            this.cmenu.Name = "contextMenuStrip1";
            this.cmenu.Size = new System.Drawing.Size(223, 104);
            // 
            // mnuRestore
            // 
            this.mnuRestore.Name = "mnuRestore";
            this.mnuRestore.Size = new System.Drawing.Size(222, 22);
            this.mnuRestore.Text = "&Restore Window";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(219, 6);
            // 
            // mnuEnable
            // 
            this.mnuEnable.Name = "mnuEnable";
            this.mnuEnable.Size = new System.Drawing.Size(222, 22);
            this.mnuEnable.Text = "Enable Window Positioning";
            // 
            // mnuDisable
            // 
            this.mnuDisable.Name = "mnuDisable";
            this.mnuDisable.Size = new System.Drawing.Size(222, 22);
            this.mnuDisable.Text = "Disable Window Positioning";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(219, 6);
            // 
            // mnuQuit
            // 
            this.mnuQuit.Name = "mnuQuit";
            this.mnuQuit.Size = new System.Drawing.Size(222, 22);
            this.mnuQuit.Text = "Quit";
            // 
            // lstEvents
            // 
            this.lstEvents.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstEvents.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.colStatus,
            this.colMonitor,
            this.colTime});
            this.lstEvents.FullRowSelect = true;
            this.lstEvents.HideSelection = false;
            this.lstEvents.Location = new System.Drawing.Point(14, 27);
            this.lstEvents.Name = "lstEvents";
            this.lstEvents.Size = new System.Drawing.Size(617, 425);
            this.lstEvents.TabIndex = 4;
            this.lstEvents.UseCompatibleStateImageBehavior = false;
            this.lstEvents.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Name = "columnHeader1";
            this.columnHeader1.Text = "Window Name";
            this.columnHeader1.Width = 250;
            // 
            // colStatus
            // 
            this.colStatus.Name = "colStatus";
            this.colStatus.Text = "Event";
            this.colStatus.Width = 150;
            // 
            // colMonitor
            // 
            this.colMonitor.Name = "colMonitor";
            this.colMonitor.Text = "Monitor";
            this.colMonitor.Width = 75;
            // 
            // colTime
            // 
            this.colTime.Name = "colTime";
            this.colTime.Text = "Timestamp";
            this.colTime.Width = 120;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(643, 24);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // btnQuit
            // 
            this.btnQuit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnQuit.Location = new System.Drawing.Point(542, 460);
            this.btnQuit.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(89, 27);
            this.btnQuit.TabIndex = 6;
            this.btnQuit.Text = "&Quit";
            this.btnQuit.UseVisualStyleBackColor = true;
            this.btnQuit.Click += new System.EventHandler(this.btnQuit_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(643, 519);
            this.Controls.Add(this.btnQuit);
            this.Controls.Add(this.lstEvents);
            this.Controls.Add(this.btnWatch);
            this.Controls.Add(this.status);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Dynamic Desktop Windows Tool";
            this.status.ResumeLayout(false);
            this.status.PerformLayout();
            this.cmenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.StatusStrip status;
        private System.Windows.Forms.ToolStripStatusLabel tss1;
        private System.Windows.Forms.Button btnWatch;
        private System.Windows.Forms.ToolStripStatusLabel tss2;
        private System.Windows.Forms.NotifyIcon notify;
        private System.Windows.Forms.ContextMenuStrip cmenu;
        private System.Windows.Forms.ToolStripMenuItem mnuRestore;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mnuEnable;
        private System.Windows.Forms.ToolStripMenuItem mnuDisable;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem mnuQuit;
        private System.Windows.Forms.ListView lstEvents;
        private System.Windows.Forms.ColumnHeader colWndName;
        private System.Windows.Forms.ColumnHeader colStatus;
        private System.Windows.Forms.ColumnHeader colTime;
        private System.Windows.Forms.ColumnHeader colMonitor;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.Button btnQuit;
    }
}

