
namespace Talk
{
    partial class Talk
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Talk));
            this.notifications = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.resetSetupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.quitterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.login = new CefSharp.WinForms.ChromiumWebBrowser();
            this.checkMessages = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifications
            // 
            this.notifications.BalloonTipText = "Nouveau message !";
            this.notifications.BalloonTipTitle = "Nextcloud Talk";
            this.notifications.ContextMenuStrip = this.contextMenuStrip1;
            this.notifications.Icon = ((System.Drawing.Icon)(resources.GetObject("notifications.Icon")));
            this.notifications.Text = "Nextcloud Talk";
            this.notifications.Visible = true;
            this.notifications.BalloonTipClicked += new System.EventHandler(this.notifications_BalloonTipClicked);
            this.notifications.MouseUp += new System.Windows.Forms.MouseEventHandler(this.notifications_MouseUp);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resetSetupToolStripMenuItem,
            this.toolStripSeparator1,
            this.quitterToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(135, 54);
            // 
            // resetSetupToolStripMenuItem
            // 
            this.resetSetupToolStripMenuItem.Name = "resetSetupToolStripMenuItem";
            this.resetSetupToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.resetSetupToolStripMenuItem.Text = "Reset setup";
            this.resetSetupToolStripMenuItem.Click += new System.EventHandler(this.resetSetupToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(131, 6);
            // 
            // quitterToolStripMenuItem
            // 
            this.quitterToolStripMenuItem.Name = "quitterToolStripMenuItem";
            this.quitterToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.quitterToolStripMenuItem.Text = "Exit";
            this.quitterToolStripMenuItem.Click += new System.EventHandler(this.quitterToolStripMenuItem_Click);
            // 
            // login
            // 
            this.login.ActivateBrowserOnCreation = false;
// TODO: Code generation for '' failed because of Exception 'Invalid Primitive Type: System.IntPtr. Consider using CodeObjectCreateExpression.'.
            this.login.Dock = System.Windows.Forms.DockStyle.Fill;
            this.login.Location = new System.Drawing.Point(0, 0);
            this.login.Name = "login";
            this.login.Size = new System.Drawing.Size(1113, 636);
            this.login.TabIndex = 2;
            this.login.Visible = false;
            // 
            // checkMessages
            // 
            this.checkMessages.Interval = 15000;
            this.checkMessages.Tick += new System.EventHandler(this.checkMessages_TickAsync);
            // 
            // Talk
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1113, 636);
            this.Controls.Add(this.login);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Talk";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Nextcloud Talk";
            this.Activated += new System.EventHandler(this.Talk_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Talk_FormClosing);
            this.Load += new System.EventHandler(this.Talk_Load);
            this.Resize += new System.EventHandler(this.Talk_Resize);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.NotifyIcon notifications;
        private CefSharp.WinForms.ChromiumWebBrowser login;
        private System.Windows.Forms.Timer checkMessages;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem quitterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetSetupToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    }
}

