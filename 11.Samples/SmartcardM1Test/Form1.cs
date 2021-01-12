using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using DMT.Smartcard;

namespace SmartcardM1Test
{
    public partial class Form1 : Form
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }

        #endregion

        #region Private Methods

        private void InitSmartcardHandlers()
        {
            // Init Event Handlers.
            SmartcardService.Instance.OnIdle += SmartcardService_OnIdle;
            SmartcardService.Instance.OnCardReadSerial += SmartcardService_OnCardReadSerial;
            SmartcardService.Instance.OnCardReadBlock += SmartcardService_OnCardReadBlock;
        }

        private void FreeSmartcardHandlers()
        {
            // Release Event Handlers.
            SmartcardService.Instance.OnCardReadBlock -= SmartcardService_OnCardReadBlock;
            SmartcardService.Instance.OnCardReadSerial -= SmartcardService_OnCardReadSerial;
            SmartcardService.Instance.OnIdle -= SmartcardService_OnIdle;
        }

        private void InitSmartcardService()
        {
            // Read both serial and block.
            SmartcardService.Instance.ReadSerialNoOnly = false;
            // Set Secure Key.
            SmartcardService.Instance.SecureKey = SL600SDK.DefaultKey;
            // Init all handlers
            InitSmartcardHandlers();
        }

        private void FreeSmartcardService()
        {
            // Required when close program to prevent process halt.
            SmartcardService.Instance.Shutdown();
            // Free all handlers
            FreeSmartcardHandlers();
        }

        private void StartService()
        {
            SmartcardService.Instance.Start();
        }

        private void ShutdownService()
        {
            SmartcardService.Instance.Shutdown();
        }

        private void UpdateStatus()
        {
            bool isConnected = SmartcardService.Instance.Connected;
            // Set Buttons enable state.
            cmdStart.Enabled = !isConnected;
            cmdStop.Enabled = isConnected;

            // Update connect status.
            lbConnectStatus.Text = (isConnected) ? "Connected" : "Disconnected";
            lbConnectStatus.ForeColor = (isConnected) ? Color.Green : Color.Red;
            // Update connect status's message.
            lbConnectStatusMsg.Text = SmartcardService.Instance.ConnectStatus;
            lbConnectStatusMsg.ForeColor = (isConnected) ? Color.Green : Color.Red;
        }

        #endregion

        #region UI Event Handlers

        #region Load/Closing

        private void Form1_Load(object sender, EventArgs e)
        {
            InitSmartcardService();
            // Update Status
            UpdateStatus();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            FreeSmartcardService();
        }

        #endregion

        #region SmartcardService Event Handlers

        private void SmartcardService_OnIdle(object sender, EventArgs e)
        {
            // Update Status
            UpdateStatus();

            lbCardExist.ForeColor = Color.Red;
            lbCardExist.Text = "Card not avaliable.";
            lbSN.Text = "-";
            txtSN.Text = "";
            lbBlock0.Text = "Block 0: ";
            lbBlock1.Text = "Block 1: ";
            lbBlock2.Text = "Block 2: ";
            lbBlock3.Text = "Block 3: ";
        }

        private void SmartcardService_OnCardReadSerial(object sender, M1CardReadSerialEventArgs e)
        {
            // Update Status
            UpdateStatus();

            lbCardExist.ForeColor = Color.Green;
            lbCardExist.Text = "Card avaliable.";
            lbSN.Text = e.SerialNo;
            txtSN.Text = e.SerialNo.Replace(" ", string.Empty);
        }

        private void SmartcardService_OnCardReadBlock(object sender, M1CardReadBlockEventArgs e)
        {
            lbBlock0.Text = "Block 0: " + e.Block0;
            lbBlock1.Text = "Block 1: " + e.Block1;
            lbBlock2.Text = "Block 2: " + e.Block2;
            lbBlock3.Text = "Block 3: " + e.Block3;
        }

        #endregion

        #region CheckBox Handlers

        private void chkReadBoth_CheckedChanged(object sender, EventArgs e)
        {
            SmartcardService.Instance.ReadSerialNoOnly = !chkReadBoth.Checked;
        }

        #endregion

        #region Button Handlers

        private void cmdStart_Click(object sender, EventArgs e)
        {
            // start listen port.
            StartService();
            // Update Status
            UpdateStatus();
        }

        private void cmdStop_Click(object sender, EventArgs e)
        {
            // stop listen port.
            ShutdownService();
            // Update Status
            UpdateStatus();
        }

        private void cmdShutdownForce_Click(object sender, EventArgs e)
        {
            // stop listen port and force cleanup all resource.
            ShutdownService();
        }

        private void cmdRelease_Click(object sender, EventArgs e)
        {
            SmartcardService.Instance.Shutdown(true);
            SmartcardService.Release();
            // Update Status
            UpdateStatus();

            MessageBox.Show("Note: This function is still not work properly.");
        }

        #endregion

        #endregion
    }
}
