﻿#region Using

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using DMT.Models;
using DMT.Services;
using NLib.Services;
using NLib.Reflection;
using NLib.Reports.Rdlc;
using System.Reflection;

#endregion

namespace DMT.TOD.Pages.Reports
{
    /// <summary>
    /// Interaction logic for DailyRevenueSummaryPreviewPage.xaml
    /// </summary>
    public partial class DailyRevenueSummaryPreviewPage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public DailyRevenueSummaryPreviewPage()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private User _user = null;
        private List<Models.RevenueEntry> _revenues = null;

        #endregion

        #region Button Handlers

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            Print();
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            GotoReportMenu();
        }

        #endregion

        #region Private Methods

        #region Navigate methods

        private void GotoReportMenu()
        {
            // Report Menu Page
            var page = TODApp.Pages.ReportMenu;
            PageContentManager.Instance.Current = page;
        }

        private void Print()
        {
            this.rptViewer.Print();
            GotoReportMenu();
        }

        #endregion

        #region Report methods

        private RdlcReportModel GetReportModel()
        {
            Assembly assembly = this.GetType().Assembly;
            RdlcReportModel inst = new RdlcReportModel();
            inst.Definition.EmbededReportName = "DMT.TOD.Reports.RevenueSlipSum.rdlc";
            inst.Definition.RdlcInstance = RdlcReportUtils.GetEmbededReport(assembly,
                inst.Definition.EmbededReportName);
            // clear reprot datasource.
            inst.DataSources.Clear();

            List<RevenueEntry> items = new List<RevenueEntry>();
            if (null != _revenues) items.AddRange(_revenues);

            // assign new data source
            RdlcReportDataSource mainDS = new RdlcReportDataSource();
            mainDS.Name = "main"; // the datasource name in the rdlc report.
            mainDS.Items = items; // setup data source
            // Add to datasources
            inst.DataSources.Add(mainDS);

            // Add parameters (if required).
            //DateTime today = DateTime.Now;
            //string printDate = today.ToThaiDateTimeString("dd/MM/yyyy HH:mm:ss");
            //inst.Parameters.Add(RdlcReportParameter.Create("PrintDate", printDate));

            return inst;
        }

        #endregion

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        /// <param name="user">The user instance.</param>
        /// <param name="revenues">The List of RevenueEntry.</param>
        public void Setup(User user, List<Models.RevenueEntry> revenues)
        {
            _user = user;
            _revenues = revenues;
            if (null != _user && null != _revenues)
            {
            }

            var model = GetReportModel();
            if (null == model ||
                null == model.DataSources || model.DataSources.Count <= 0 ||
                null == model.DataSources[0] || null == model.DataSources[0].Items)
            {
                var win = TODApp.Windows.MessageBox;
                win.Setup("ไม่พบข้อมูลในการจัดพิมพ์รายงาน.", "DMT - Tour of Duty");
                win.ShowDialog();
                this.rptViewer.ClearReport();
            }
            else
            {
                this.rptViewer.LoadReport(model);
            }
        }

        #endregion
    }
}