#region Using

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Reflection;

using DMT.Models;
using DMT.Services;
using NLib;
using NLib.Services;
using NLib.Reflection;
using System.Threading.Tasks;

#endregion

namespace DMT.TA.Windows.Exchange
{
    /// <summary>
    /// Interaction logic for ReturnExchangeWindow.xaml
    /// </summary>
    public partial class ReturnExchangeWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public ReturnExchangeWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private TSB _tsb = null;
        private decimal? _total = decimal.Zero;
        private TSBExchangeGroup _group = null;
        private TSBExchangeTransaction _item = null;

        #endregion

        #region Button Handlers

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (!entry.CheckEqualBorrowAmount())
            {
                // not equal borrow amount.
                var win = TAApp.Windows.MessageBox;
                win.Setup("จำนวนเงินคืน ไม่เท่ากันกับ ยอดเงินที่ยืม กรุณาตรวจสอบข้อมูล", "Toll Admin");
                win.ShowDialog();
                return;
            }
                
            if (!entry.EnoughTSBBalance())
            {
                // TSB has not enough coin/bill to returns.
                var win = TAApp.Windows.MessageBox;
                win.Setup("จำนวนเหรียญ/ธนบัตรในด่าน ไม่เพียงพอ กรุณาตรวจสอบข้อมูล", "Toll Admin");
                win.ShowDialog();
                return;
            }


            var confirm = TAApp.Windows.ConfirmAccountReceiveMoney;
            confirm.Setup(_total.Value);
            if (confirm.ShowDialog() == false)
            {
                // failed to verify user
                return;
            }
            else
            {
                // OK
                this.Save();
                // Send Balance data to TA Server
                DialogResult = true;
            }
        }

        private void cmdBack_Click(object sender, RoutedEventArgs e)
        {
            // Close Window
            DialogResult = false;
        }

        #endregion

        #region Private Methods

        private void Save()
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            string msg = string.Empty;
            // Write log.
            med.Info("=== RETURNS EXCHANGE BEGIN ==");

            // Update TSB Exchange Group
            if (null != _group)
            {
                _group.State = TSBExchangeGroup.StateTypes.Return;
                _group.FinishFlag = TSBExchangeGroup.FinishedFlags.Completed;
                // Save group
                var ret = TSBExchangeGroup.SaveTSBExchangeGroup(_group).Value();
                if (null != ret)
                {
                    msg = "TSB Exchange Group successfully saved.";
                }
                else msg = "TSB Exchange Group failed to saved.";
                // Write log.
                med.Info(msg);
            }

            // Update TSB Exchange Transaction
            if (null != _item)
            {
                var ret2 = TSBExchangeTransaction.SaveTransaction(_item).Value();
                if (null != ret2)
                {
                    msg = "TSB Exchange Transaction (Return) successfully saved.";
                }
                else msg = "TSB Exchange Transaction (Return) failed to saved.";
                // Write log.
                med.Info(msg);
            }

            // Update TSB Credit Transaction
            var creditTran = new TSBCreditTransaction();
            creditTran.TransactionDate = DateTime.Now;
            creditTran.GroupId = _group.GroupId;
            creditTran.TransactionType = TSBCreditTransaction.TransactionTypes.Returns;

            creditTran.TSBId = _group.TSBId;
            creditTran.TSBNameEN = _group.TSBNameEN;
            creditTran.TSBNameTH = _group.TSBNameTH;

            creditTran.SupervisorId = TAApp.User.Current.UserId; // set current sup id.
            creditTran.SupervisorNameEN = TAApp.User.Current.FullNameEN;
            creditTran.SupervisorNameTH = TAApp.User.Current.FullNameTH;

            creditTran.AmountST25 = _item.AmountST25;
            creditTran.AmountST50 = _item.AmountST50;
            creditTran.AmountBHT1 = _item.AmountBHT1;
            creditTran.AmountBHT2 = _item.AmountBHT2;
            creditTran.AmountBHT5 = _item.AmountBHT5;
            creditTran.AmountBHT10 = _item.AmountBHT10;
            creditTran.AmountBHT20 = _item.AmountBHT20;
            creditTran.AmountBHT50 = _item.AmountBHT50;
            creditTran.AmountBHT100 = _item.AmountBHT100;
            creditTran.AmountBHT500 = _item.AmountBHT500;
            creditTran.AmountBHT1000 = _item.AmountBHT1000;

            creditTran.ExchangeBHT = _item.ExchangeBHT; // เงินขอแลก
            creditTran.BorrowBHT = _item.BorrowBHT; // เงินยืมเพิ่ม
            creditTran.AdditionalBHT = _item.AdditionalBHT; // เพิ่มวงเงิน

            // Save transaction
            var ret3 = TSBCreditTransaction.SaveTransaction(creditTran).Value();
            if (null != ret3)
            {
                msg = "TSB Credit Transaction (Return) successfully saved.";
            }
            else msg = "TSB Credit Transaction (Return) failed to saved.";
            // Write log.
            med.Info(msg);


            // Update TSB Balance to TA Server
            var tsbBal = TSBCreditBalance.GetCurrent(TAAPI.TSB).Value(); // Find current TSB balance.
            TAATSBCredit tsbCdt = null;
            if (null != tsbBal)
            {
                // For Update TSB balance
                tsbCdt = new TAATSBCredit();
                tsbCdt.TSBId = tsbBal.TSBId;
                tsbCdt.Amnt1 = tsbBal.AmountBHT1;
                tsbCdt.Amnt2 = tsbBal.AmountBHT2;
                tsbCdt.Amnt5 = tsbBal.AmountBHT5;
                tsbCdt.Amnt10 = tsbBal.AmountBHT10;
                tsbCdt.Amnt20 = tsbBal.AmountBHT20;
                tsbCdt.Amnt50 = tsbBal.AmountBHT50;
                tsbCdt.Amnt100 = tsbBal.AmountBHT100;
                tsbCdt.Amnt500 = tsbBal.AmountBHT500;
                tsbCdt.Amnt1000 = tsbBal.AmountBHT1000;
                tsbCdt.Remark = null;
                tsbCdt.Updatedate = DateTime.Now;

                msg = "Send Update TSB Credit Balance to TA Server.";
                med.Info(msg);
            }
            else
            {
                msg = "Cannot find TSB Credit Balance. No data send to TA Server.";
                med.Err(msg);
            }

            Task.Run(() =>
            {
                TAxTODMQService.Instance.WriteQueue(tsbCdt);
            });


            med.Info("=== RETURNS EXCHANGE END ==");
        }

        #endregion

        #region Public Method

        /// <summary>
        /// Setup.
        /// </summary>
        /// <param name="item">The Exchange Item to return.</param>
        public void Setup(TSBExchangeTransaction item)
        {
            this._item = item;

            _tsb = TAAPI.TSB; // get current tsb
            if (null != _item)
            {
                // get total
                _total = item.BorrowBHT;
                // Gets TSBExchangeGroup
                _group = TSBExchangeGroup.GetTSBExchangeGroup(_tsb, _item.GroupId).Value();
                entry.Setup(_group, _item, _total.Value);
            }
            else
            {
                MethodBase med = MethodBase.GetCurrentMethod();
                med.Err("คืนเงินยืมทอน - TSBExchangeTransaction is null.");
            }
        }

        #endregion
    }
}
