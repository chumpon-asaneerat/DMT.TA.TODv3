#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using DMT.Services;

#endregion

namespace DMT
{
    /// <summary>
    /// The Main Form.
    /// </summary>
    public partial class MainForm : Form
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Form Loaded/Closing

        private Icon icoRunning;
        private Icon icoStoped;
        private Icon icoDefault;

        private void MainForm_Load(object sender, EventArgs e)
        {
            icoDefault = this.Icon;

            icoRunning = Icon.FromHandle(((Bitmap)images.Images[0]).GetHicon());
            icoStoped = Icon.FromHandle(((Bitmap)images.Images[1]).GetHicon());

            notify.Icon = icoStoped;
            this.Icon = notify.Icon; // Setup icon.

            cmdStart.Enabled = false;
            cmdStop.Enabled = false;

            LocalServiceManager.Instance.ServiceMonitor.Start();
            scanTimer.Start();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            cmdStart.Enabled = false;
            cmdStop.Enabled = false;

            scanTimer.Stop();
            LocalServiceManager.Instance.ServiceMonitor.Shutdown();

            this.Icon = icoDefault; // Restore icon.

            if (null != icoRunning)
            {
                icoRunning.Dispose();
            }
            icoRunning = null;
            if (null != icoRunning)
            {
                icoRunning.Dispose();
            }
            icoRunning = null;
        }

        #endregion

        #region Form Resize

        private void MainForm_Resize(object sender, EventArgs e)
        {
            // if the form is minimized  hide it from the task bar  
            // and show the system tray icon (represented by the NotifyIcon control)
            if (this.WindowState == FormWindowState.Minimized)
            {
                Hide();
            }
        }

        #endregion

        #region Timer Handler

        private void scanTimer_Tick(object sender, EventArgs e)
        {
            var status = LocalServiceManager.Instance.CheckInstalled();
            if (null != status)
            {
                string winSerStatus;
                string healthStatus;
                Color cr = Color.Black;
                // check service installed/uninstalled.
                if (status.PlazaLocalServiceInstalled) 
                {
                    cr = Color.Green;
                    winSerStatus = "Installed";

                    notify.Icon = icoRunning;

                    cmdStart.Enabled = false;
                    cmdStop.Enabled = true;
                }
                else
                {
                    winSerStatus = "Uninstalled";
                    cr = Color.Red;

                    notify.Icon = icoStoped;
                    
                    cmdStart.Enabled = true;
                    cmdStop.Enabled = false;
                }
                // check serice access.
                healthStatus = "OK.";

                string msg = string.Format("Service: {0}, Status: {1}", winSerStatus, healthStatus);
                lbStatus.Text = msg;
                lbStatus.ForeColor = cr;

                this.Icon = notify.Icon; // Setup icon.
            }
        }

        #endregion

        #region Button Handler

        private void cmdStart_Click(object sender, EventArgs e)
        {
            cmdStart.Enabled = false;
            cmdStop.Enabled = false;

            LocalServiceManager.Instance.Install();
        }

        private void cmdStop_Click(object sender, EventArgs e)
        {
            cmdStart.Enabled = false;
            cmdStop.Enabled = false;

            LocalServiceManager.Instance.Uninstall();
        }

        #endregion

        #region NotifyIcon Handlers

        private void notify_DoubleClick(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                Show();
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                Hide();
                this.WindowState = FormWindowState.Minimized;
            }
        }

        #endregion

        #region Context Menu Handler

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close(); // Close windows.
        }

        #endregion
    }
}
