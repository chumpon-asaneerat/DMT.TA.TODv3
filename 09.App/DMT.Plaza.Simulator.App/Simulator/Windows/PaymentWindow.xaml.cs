#region Using

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

using DMT.Models;
using DMT.Configurations;
using DMT.Services;
using NLib.Services;
using NLib.Reflection;

#endregion

namespace DMT.Simulator.Windows
{
    using emuOps = Services.Operations.SCW.Emulator; // reference to static class.

    /// <summary>
    /// Interaction logic for PaymentWindow.xaml
    /// </summary>
    public partial class PaymentWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public PaymentWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private Random rand = new Random();
        private LaneInfo _lane = null;

        #endregion

        #region Button Handlers

        private void cmdOk_Click(object sender, RoutedEventArgs e)
        {
            if (null == _lane || null == _lane.User) return;


            if (string.IsNullOrEmpty(txtApproveCode.Text))
            {
                ShowError("Please Enter Approve Code.");
                txtApproveCode.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txtRefCode.Text))
            {
                ShowError("Please Enter Ref Code.");
                txtRefCode.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txtAmount.Text))
            {
                ShowError("Please Enter Amount.");
                txtAmount.Focus();
                return;
            }

            decimal val = decimal.Zero;
            if (!decimal.TryParse(txtAmount.Text, out val))
            {
                ShowError("The Amount must be numeric.");
                txtAmount.Focus();
                return;
            }
            if (val <= 0)
            {
                ShowError("The Amount not zero or negative value.");
                txtAmount.Focus();
                return;
            }

            int networkId = PlazaAppConfigManager.Instance.DMT.networkId;
            if (rbEMV.IsChecked == true)
            {
                SCWAddEMV inst = new SCWAddEMV();
                inst.networkId = networkId;
                inst.plazaId = _lane.SCWPlazaId;
                inst.laneId = _lane.LaneNo;
                inst.staffId = _lane.UserId;
                inst.staffNameEn = _lane.FullNameEN;
                inst.staffNameTh = _lane.FullNameTH;
                inst.trxDateTime = DateTime.Now;
                inst.approvalCode = txtApproveCode.Text;
                inst.refNo = txtRefCode.Text;
                inst.amount = val;
                emuOps.addEMV(inst);
            }
            else if (rbQRCode.IsChecked == true)
            {
                SCWAddQRCode inst = new SCWAddQRCode();
                inst.networkId = networkId;
                inst.plazaId = _lane.SCWPlazaId;
                inst.laneId = _lane.LaneNo;
                inst.staffId = _lane.UserId;
                inst.staffNameEn = _lane.FullNameEN;
                inst.staffNameTh = _lane.FullNameTH;
                inst.trxDateTime = DateTime.Now;
                inst.approvalCode = txtApproveCode.Text;
                inst.refNo = txtRefCode.Text;
                inst.amount = val;
                emuOps.addQRCode(inst);
            }
            else
            {
                ShowError("Please select payment type.");
                return;
            }
            DialogResult = true;
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        #endregion

        #region Private Methods

        private void ShowError(string msg)
        {
            txtErrMsg.Text = msg;
        }

        private string GenerateRandomChar(int length)
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[rand.Next(s.Length)]).ToArray());
        }

        private void GenerateRandomCode()
        {
            string apvChr = GenerateRandomChar(2);
            string refChr = GenerateRandomChar(2);

            int apvVal = rand.Next(100000);
            int refVal = rand.Next(100000);

            txtApproveCode.Text = "APV-" + apvChr + "-" + apvVal.ToString("D5");
            txtRefCode.Text = "REF-" + refChr + "-" + refVal.ToString("D5");
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        /// <param name="value">The LaneInfo instance.</param>
        public void Setup(LaneInfo value)
        {
            _lane = value;
            if (null == _lane || null == _lane.User) cmdOk.IsEnabled = false;
            GenerateRandomCode();
            txtAmount.Focus();
        }

        #endregion
    }
}
