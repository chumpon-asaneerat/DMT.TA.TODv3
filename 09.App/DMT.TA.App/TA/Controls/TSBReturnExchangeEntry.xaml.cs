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
        private TSBExchangeGroup _group = null;
        private TSBExchangeTransaction _item = null;
        private TSBCreditBalance _currentBalance, _newBalance = null;

        #endregion

        #region Private Methods

        private void _item_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.StartsWith("Amount")) // filter only amount properties
            {
                // calculate balance.
                _newBalance.AmountST25 = _currentBalance.AmountST25 - _item.AmountST25;
                _newBalance.AmountST50 = _currentBalance.AmountST50 - _item.AmountST50;
                _newBalance.AmountBHT1 = _currentBalance.AmountBHT1 - _item.AmountBHT1;
                _newBalance.AmountBHT2 = _currentBalance.AmountBHT2 - _item.AmountBHT2;
                _newBalance.AmountBHT5 = _currentBalance.AmountBHT5 - _item.AmountBHT5;
                _newBalance.AmountBHT10 = _currentBalance.AmountBHT10 - _item.AmountBHT10;
                _newBalance.AmountBHT20 = _currentBalance.AmountBHT20 - _item.AmountBHT20;
                _newBalance.AmountBHT50 = _currentBalance.AmountBHT50 - _item.AmountBHT50;
                _newBalance.AmountBHT100 = _currentBalance.AmountBHT100 - _item.AmountBHT100;
                _newBalance.AmountBHT500 = _currentBalance.AmountBHT500 - _item.AmountBHT500;
                _newBalance.AmountBHT1000 = _currentBalance.AmountBHT1000 - _item.AmountBHT1000;
            }
        }

        #endregion

        #region Public Methods

        public void Setup(TSBExchangeGroup group, TSBExchangeTransaction item, decimal total)
        {
            if (null != _item)
            {
                // unhook event before assigned new one.
                _item.PropertyChanged -= _item_PropertyChanged;
            }
            _item = null; // reset item.

            _group = group; // set exchange group.

            _total = total;
            txtAmount.Text = (_total.HasValue) ? _total.Value.ToString("n0") : "0";

            _item = item;

            _currentBalance = TSBCreditBalance.GetCurrent().Value();
            _newBalance = TSBCreditBalance.GetCurrent().Value();
            tranEntry.IsEnabled = false;

            if (null != _item)
            {
                _total = item.GrandTotalBHT;
                _item.GroupId = item.GroupId;
                _item.PropertyChanged += _item_PropertyChanged;

                tranEntry.Setup(_item);
                tranEntry.IsEnabled = true;
            }
            if (null != _newBalance)
            {
                _newBalance.Description = "ยอดเงินด่าน";
                tsbBalance.Setup(_newBalance);
            }
        }

        public bool EnoughTSBBalance()
        {
            if (null == _newBalance)
                return false;
            bool bInvalid = (_newBalance.AmountST25 < 0 ||
                _newBalance.AmountST50 < 0 ||
                _newBalance.AmountBHT1 < 0 ||
                _newBalance.AmountBHT2 < 0 ||
                _newBalance.AmountBHT5 < 0 ||
                _newBalance.AmountBHT10 < 0 ||
                _newBalance.AmountBHT20 < 0 ||
                _newBalance.AmountBHT50 < 0 ||
                _newBalance.AmountBHT100 < 0 ||
                _newBalance.AmountBHT500 < 0 ||
                _newBalance.AmountBHT1000 < 0);
            return !bInvalid;
        }

        public bool CheckEqualBorrowAmount()
        {
            if (null == _item)
                return false;
            bool ret = _item.BHTTotal == _item.BorrowBHT;
            return ret;
        }

        #endregion
    }
}
