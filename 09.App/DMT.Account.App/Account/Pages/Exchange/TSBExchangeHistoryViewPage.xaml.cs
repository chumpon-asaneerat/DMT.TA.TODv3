#region Using

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Input;
using System.Windows.Threading;
using System.Reflection;

using DMT.Models;
using DMT.Services;

using NLib;
using NLib.Services;
using NLib.Reflection;

#endregion

namespace DMT.Account.Pages.Exchange
{
    using ops = DMT.Services.Operations.TAxTOD;

    /// <summary>
    /// Interaction logic for TSBExchangeHistoryViewPage.xaml
    /// </summary>
    public partial class TSBExchangeHistoryViewPage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public TSBExchangeHistoryViewPage()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private CultureInfo culture = new CultureInfo("th-TH");
        private XmlLanguage language = XmlLanguage.GetLanguage("th-TH");

        #endregion

        #region Loaded/Unload

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            dtRequestDate.CultureInfo = culture;
            dtRequestDate.Language = language;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region Button Handlers

        private void cmdApproveDetail_Click(object sender, RoutedEventArgs e)
        {
            // Show Detail windows
            if (null == sender || !(sender is Button)) return;
            var item = (sender as Button).DataContext as Models.TAAExchangeSummary;
            if (null != item)
            {
                int reqId = (item.RequestId.HasValue) ? item.RequestId.Value : -1;
                var doc = CreateExchangeTransaction(item);
                var win = AccountApp.Windows.ApproveExchangeWindow;
                win.Setup(reqId, doc, false);
                win.ShowDialog();
            }
        }

        private void cmdExport_Click(object sender, RoutedEventArgs e)
        {
            List<TAAExchangeSummary> results = grid.DataContext as List<TAAExchangeSummary>;
            if (null == results)
            {
                var win = AccountApp.Windows.MessageBox;
                win.Setup("ไม่มีรายการในการส่งออกเป็น Excel กรุณาตรวจสอบข้อมูล.", "DMT - TA (Account)");
                win.ShowDialog();
                return;
            }
            Exports(results);
        }

        private void cmdBack_Click(object sender, RoutedEventArgs e)
        {
            GotoMainMenu();
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }

        #endregion

        #region Private Methods

        private TSBExchangeTransaction CreateExchangeTransaction(Models.TAAExchangeSummary item)
        {
            TSBExchangeTransaction doc = null;
            if (null != item)
            {
                int reqId = (item.RequestId.HasValue) ? item.RequestId.Value : -1;
                string tsbId = item.TSBId;
                TSB tsb = TSB.GetTSB(tsbId).Value();

                // create approve document
                doc = new TSBExchangeTransaction();
                doc.TransactionDate = (item.RequestDate.HasValue) ? item.RequestDate.Value : DateTime.MinValue;
                doc.TransactionType = TSBExchangeTransaction.TransactionTypes.Approve;
                doc.Remark = item.RequestRemark;
                doc.AdditionalBHT = (item.AppAdditionalBHT.HasValue) ? item.AppAdditionalBHT.Value : decimal.Zero;
                doc.BorrowBHT = (item.AppBorrowBHT.HasValue) ? item.AppBorrowBHT.Value : decimal.Zero;
                doc.ExchangeBHT = (item.AppExchangeBHT.HasValue) ? item.AppExchangeBHT.Value : decimal.Zero;
                doc.PeriodBegin = item.PeriodBegin;
                doc.PeriodEnd = item.PeriodEnd;
                doc.TSBId = tsb.TSBId;
                doc.TSBNameEN = tsb.TSBNameEN;
                doc.TSBNameTH = tsb.TSBNameTH;

                doc.Remark = item.ApproveRemark; // assign approve's remark.

                // get request details
                var details = ops.Exchange.GetApproveItems(tsbId, reqId).Value();
                if (null != details && details.Count > 0)
                {
                    details.ForEach(detail =>
                    {
                        if (detail.CurrencyDenomId == 1)
                        {
                            doc.AmountST25 = (detail.ApproveValue.HasValue) ? detail.ApproveValue.Value : decimal.Zero;
                        }
                        else if (detail.CurrencyDenomId == 2)
                        {
                            doc.AmountST50 = (detail.ApproveValue.HasValue) ? detail.ApproveValue.Value : decimal.Zero;
                        }
                        else if (detail.CurrencyDenomId == 3)
                        {
                            doc.AmountBHT1 = (detail.ApproveValue.HasValue) ? detail.ApproveValue.Value : decimal.Zero;
                        }
                        else if (detail.CurrencyDenomId == 4)
                        {
                            doc.AmountBHT2 = (detail.ApproveValue.HasValue) ? detail.ApproveValue.Value : decimal.Zero;
                        }
                        else if (detail.CurrencyDenomId == 5)
                        {
                            doc.AmountBHT5 = (detail.ApproveValue.HasValue) ? detail.ApproveValue.Value : decimal.Zero;
                        }
                        else if (detail.CurrencyDenomId == 6)
                        {
                            doc.AmountBHT10 = (detail.ApproveValue.HasValue) ? detail.ApproveValue.Value : decimal.Zero;
                        }
                        else if (detail.CurrencyDenomId == 7)
                        {
                            // Skip 10 BHT Notes.
                        }
                        else if (detail.CurrencyDenomId == 8)
                        {
                            doc.AmountBHT20 = (detail.ApproveValue.HasValue) ? detail.ApproveValue.Value : decimal.Zero;
                        }
                        else if (detail.CurrencyDenomId == 9)
                        {
                            doc.AmountBHT50 = (detail.ApproveValue.HasValue) ? detail.ApproveValue.Value : decimal.Zero;
                        }
                        else if (detail.CurrencyDenomId == 10)
                        {
                            doc.AmountBHT100 = (detail.ApproveValue.HasValue) ? detail.ApproveValue.Value : decimal.Zero;
                        }
                        else if (detail.CurrencyDenomId == 11)
                        {
                            doc.AmountBHT500 = (detail.ApproveValue.HasValue) ? detail.ApproveValue.Value : decimal.Zero;
                        }
                        else if (detail.CurrencyDenomId == 12)
                        {
                            doc.AmountBHT1000 = (detail.ApproveValue.HasValue) ? detail.ApproveValue.Value : decimal.Zero;
                        }
                    });
                }
            }
            return doc;
        }

        public class ExchangeStatusItem
        {
            public string Code { get; set; }
            public string DisplayText { get; set; }

            public static List<ExchangeStatusItem> Gets()
            {
                List<ExchangeStatusItem> results = new List<ExchangeStatusItem>();
                // R = รออนุมัติ
                // A = อนุมัติ
                // C = ไม่อนุมัติ (finish = 1) 
                // F = ด่านรับเงิน (finish = 1)
                results.Add(new ExchangeStatusItem() { Code = null, DisplayText = "[ ทุกสถานะ ]" });
                results.Add(new ExchangeStatusItem() { Code = "R", DisplayText = "รออนุมัติ" });
                results.Add(new ExchangeStatusItem() { Code = "A", DisplayText = "อนุมัติแล้ว" });
                results.Add(new ExchangeStatusItem() { Code = "C", DisplayText = "ไม่อนุมัติ" });
                results.Add(new ExchangeStatusItem() { Code = "F", DisplayText = "ด่านรับเงิน" });
                return results;
            }
        }

        private void LoadAllTSB()
        {
            List<TSB> tsbs = TSB.GetTSBs().Value();
            tsbs.Insert(0, new TSB() { TSBId = null, TSBNameEN = "[ None ]", TSBNameTH = "[ ทุกด่าน ]" });
            cbTSB.ItemsSource = tsbs;
            if (tsbs.Count > 0) cbTSB.SelectedIndex = 0;
        }

        private void LoadAllStatus()
        {
            var items = ExchangeStatusItem.Gets();
            cbStatus.ItemsSource = items;
            if (items.Count > 0) cbStatus.SelectedIndex = 0;
        }

        private void Search()
        {
            string tsbId = (null != cbTSB.SelectedItem && cbTSB.SelectedItem is TSB) ?
                (cbTSB.SelectedItem as TSB).TSBId: null;
            string status = (null != cbStatus.SelectedItem && cbStatus.SelectedItem is ExchangeStatusItem) ?
                (cbStatus.SelectedItem as ExchangeStatusItem).Code: null;
            DateTime? requestDate = dtRequestDate.Value;
            
            grid.DataContext = null;

            List<TAAExchangeSummary> results = new List<TAAExchangeSummary>(); ;
            var rets = ops.Exchange.Gets(status, tsbId, requestDate).Value();
            if (null != rets)
            {
                rets.ForEach(ret => 
                {
                    if (null == tsbId)
                        results.Add(ret); // all tsb case.
                    else if (null != tsbId && ret.TSBId == tsbId) 
                        results.Add(ret); // filter by selected tsbid
                });
            }

            grid.DataContext = results;
        }

        private void Exports(List<TAAExchangeSummary> items)
        {
            if (null == items) return;

            var trans = new List<TSBExchangeTransaction>();
            items.ForEach(header =>
            {
                if (null == header) return;
                var tran = CreateExchangeTransaction(header);
                if (null != tran) trans.Add(tran);
            });

            trans.ForEach(tran => 
            {
                // export.
            });
        }

        private void GotoMainMenu()
        {
            // Main Menu Page
            var page = AccountApp.Pages.MainMenu;
            PageContentManager.Instance.Current = page;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        public void Setup()
        {
            //dtRequestDate.Value = DateTime.Now.Date;
            dtRequestDate.Value = new DateTime?();
            grid.DataContext = null; // reset

            LoadAllTSB();
            LoadAllStatus();
        }

        #endregion
    }
}
