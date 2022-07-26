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

namespace DMT.TA.Controls
{
    /// <summary>
    /// Interaction logic for TSBReturnExchangeEntry.xaml
    /// </summary>
    public partial class TSBReturnExchangeEntry : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public TSBReturnExchangeEntry()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private decimal? _total = decimal.Zero;
        private TSBExchangeTransaction _item = null;
        private TSBCreditBalance _balance = null;

        #endregion

        #region Private Methods

        /// <summary>
        /// Clone Transaction.
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        private void CloneTransaction(TSBExchangeTransaction src, TSBExchangeTransaction dst)
        {
            if (null == src || null == dst)
                return;

            dst.GroupId = src.GroupId;
            dst.TransactionDate = src.TransactionDate;

            dst.TSBId = src.TSBId;
            dst.TSBNameEN = src.TSBNameEN;
            dst.TSBNameTH = src.TSBNameTH;
            dst.MaxCredit = src.MaxCredit;

            dst.UserId = src.UserId;
            dst.FullNameEN = src.FullNameEN;
            dst.FullNameTH = src.FullNameTH;

            dst.PeriodBegin = src.PeriodBegin;
            dst.PeriodEnd = src.PeriodEnd;

            dst.AmountST25 = src.AmountST25;
            dst.AmountST50 = src.AmountST50;
            dst.AmountBHT1 = src.AmountBHT1;
            dst.AmountBHT2 = src.AmountBHT2;
            dst.AmountBHT5 = src.AmountBHT5;
            dst.AmountBHT10 = src.AmountBHT10;
            dst.AmountBHT20 = src.AmountBHT20;
            dst.AmountBHT50 = src.AmountBHT50;
            dst.AmountBHT100 = src.AmountBHT100;
            dst.AmountBHT500 = src.AmountBHT500;
            dst.AmountBHT1000 = src.AmountBHT1000;

            dst.ExchangeBHT = src.ExchangeBHT;
            dst.BorrowBHT = src.BorrowBHT;
            dst.AdditionalBHT = src.AdditionalBHT;

            dst.HasRemark = true;
            dst.Remark = src.Remark;
            //dst.Description = src.Description;

            dst.FinishFlag = src.FinishFlag;
        }

        #endregion



        #region Public Methods

        public void Setup(TSBExchangeTransaction item)
        {
            _total = decimal.Zero;
            _item = new TSBExchangeTransaction();
            _balance = TSBCreditBalance.GetCurrent().Value();
            tranEntry.IsEnabled = false;
            if (null != _item)
            {
                _total = item.GrandTotalBHT;
                _item.GroupId = item.GroupId;
                

                tranEntry.Setup(_item);
                tranEntry.IsEnabled = true;
            }
            if (null != _balance) tsbBalance.Setup(_balance);
        }

        #endregion
    }
}
