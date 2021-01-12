
namespace DMT
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.scanTimer = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.lbStatus = new System.Windows.Forms.Label();
            this.notify = new System.Windows.Forms.NotifyIcon(this.components);
            this.images = new System.Windows.Forms.ImageList(this.components);
            this.cmdStart = new System.Windows.Forms.Button();
            this.cmdStop = new System.Windows.Forms.Button();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // scanTimer
            // 
            this.scanTimer.Interval = 2000;
            this.scanTimer.Tick += new System.EventHandler(this.scanTimer_Tick);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 132);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Status:";
            // 
            // lbStatus
            // 
            this.lbStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbStatus.AutoSize = true;
            this.lbStatus.Location = new System.Drawing.Point(70, 132);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(13, 17);
            this.lbStatus.TabIndex = 1;
            this.lbStatus.Text = "-";
            // 
            // notify
            // 
            this.notify.ContextMenuStrip = this.contextMenu;
            this.notify.Text = "notify";
            this.notify.Visible = true;
            this.notify.DoubleClick += new System.EventHandler(this.notify_DoubleClick);
            // 
            // images
            // 
            this.images.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("images.ImageStream")));
            this.images.TransparentColor = System.Drawing.Color.Transparent;
            this.images.Images.SetKeyName(0, "running.ico");
            this.images.Images.SetKeyName(1, "stop.ico");
            // 
            // cmdStart
            // 
            this.cmdStart.Location = new System.Drawing.Point(12, 24);
            this.cmdStart.Name = "cmdStart";
            this.cmdStart.Size = new System.Drawing.Size(122, 30);
            this.cmdStart.TabIndex = 2;
            this.cmdStart.Text = "Start";
            this.cmdStart.UseVisualStyleBackColor = true;
            this.cmdStart.Click += new System.EventHandler(this.cmdStart_Click);
            // 
            // cmdStop
            // 
            this.cmdStop.Location = new System.Drawing.Point(141, 24);
            this.cmdStop.Name = "cmdStop";
            this.cmdStop.Size = new System.Drawing.Size(122, 30);
            this.cmdStop.TabIndex = 3;
            this.cmdStop.Text = "Shutdown";
            this.cmdStop.UseVisualStyleBackColor = true;
            this.cmdStop.Click += new System.EventHandler(this.cmdStop_Click);
            // 
            // contextMenu
            // 
            this.contextMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(103, 28);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(102, 24);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(390, 158);
            this.Controls.Add(this.cmdStop);
            this.Controls.Add(this.cmdStart);
            this.Controls.Add(this.lbStatus);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Local Plaza Service";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer scanTimer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbStatus;
        private System.Windows.Forms.NotifyIcon notify;
        private System.Windows.Forms.ImageList images;
        private System.Windows.Forms.Button cmdStart;
        private System.Windows.Forms.Button cmdStop;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
    }
}