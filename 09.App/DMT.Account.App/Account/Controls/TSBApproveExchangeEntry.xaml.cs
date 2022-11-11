#region Using

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using DMT.Configurations;
using DMT.Controls;
using DMT.Models;
using DMT.Services;

#endregion

namespace DMT.Account.Controls
{
    /// <summary>
    /// Interaction logic for TSBApproveExchangeEntry.xaml
    /// </summary>
    public partial class TSBApproveExchangeEntry : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public TSBApproveExchangeEntry()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private TSBExchangeTransaction source = null;
        private TSBExchangeTransaction _approve = null;

        #endregion

        #region Private Methods

        private void CloneApprove()
        {
            if (null != source)
            {
                _approve = new TSBExchangeTransaction();

                _approve.TransactionDate = DateTime.Now;
                _approve.TransactionType = TSBExchangeTransaction.TransactionTypes.Approve;

                _approve.AmountST25 = source.AmountST25;
                _approve.AmountST50 = source.AmountST50;
                _approve.AmountBHT1 = source.AmountBHT1;
                _approve.AmountBHT2 = source.AmountBHT2;
                _approve.AmountBHT5 = source.AmountBHT5;
                _approve.AmountBHT10 = source.AmountBHT10;
                _approve.AmountBHT20 = source.AmountBHT20;
                _approve.AmountBHT50 = source.AmountBHT50;
                _approve.AmountBHT100 = source.AmountBHT100;
                _approve.AmountBHT500 = source.AmountBHT500;
                _approve.AmountBHT1000 = source.AmountBHT1000;

                _approve.AdditionalBHT = source.AdditionalBHT;
                _approve.BorrowBHT = source.BorrowBHT;
                _approve.ExchangeBHT = source.ExchangeBHT;
                _approve.TSBId = source.TSBId;
                _approve.UserId = AccountApp.User.Current.UserId; // current approve user.

                _approve.PeriodBegin = source.PeriodBegin;
                _approve.PeriodEnd = source.PeriodEnd;

                _approve.HasRemark = true;
                _approve.Remark = source.Remark;
            }
        }

        #endregion

        #region Public Methods

        public void Setup(TSBExchangeTransaction approve)
        {
            // keep source instance.
            source = approve;
            if (null != source)
            {
                source.HasRemark = true;
            }
            CloneApprove();
            approveEntry.Setup(_approve);
        }

        public void BeginEdit()
        {
            approveEntry.IsEnabled = true;
        }

        public void CancelEdit()
        {
            approveEntry.IsEnabled = false;
            CloneApprove(); // restore current approve from source item.
            approveEntry.Setup(_approve);
        }

        public void EndEdit()
        {
            approveEntry.IsEnabled = false;
        }

        /// <summary>
        /// Gets current approve item.
        /// </summary>
        public TSBExchangeTransaction Current { get { return _approve; } }

        #endregion
    }
}
