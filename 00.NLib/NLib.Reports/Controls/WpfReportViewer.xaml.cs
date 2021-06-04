#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Microsoft.Reporting.WinForms;
using System.Reflection;
using System.IO;

#endregion

using NLib;
using NLib.Controls;
using NLib.Reports.Rdlc;

using System.Drawing.Printing;
using System.Printing;

namespace NLib.Wpf.Controls
{
    /// <summary>
    /// Interaction logic for WpfReportViewer.xaml
    /// </summary>
    public partial class WpfReportViewer : UserControl
    {
        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public WpfReportViewer()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~WpfReportViewer()
        {

        }

        #endregion

        #region Internal Varaibles

        private string _printerName = string.Empty;
        private LocalReportPageSettings _pageSetting = null;

        #endregion

        #region Private Methods

        private void InitPrinterParameter(string printerName = "")
        {
            PrintQueue pq = null;
            if (string.IsNullOrEmpty(printerName))
            {
                try
                {
                    pq = LocalPrintServer.GetDefaultPrintQueue();
                }
                catch { pq = null; }
            }
            else
            {
                try
                {
                    LocalPrintServer pser = new LocalPrintServer();
                    pq = pser.GetPrintQueue(printerName);
                }
                catch { pq = null; }
            }
            if (null != pq)
            {
                _printerName = pq.FullName;
                // load default page setting.
                _pageSetting = new LocalReportPageSettings();
            }
            else
            {
                _printerName = string.Empty;
                // default
                _pageSetting = new LocalReportPageSettings();
            }
        }

        #endregion

        #region Load/Unload

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            rptViewer.ShowPrintButton = false;
            rptViewer.ShowFindControls = false;
            rptViewer.ShowProgress = false;
            rptViewer.ReportError += new ReportErrorEventHandler(rptViewer_ReportError);
            rptViewer.RenderingComplete += RptViewer_RenderingComplete;
            // Init default printer.
            InitPrinterParameter();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            rptViewer.RenderingComplete -= RptViewer_RenderingComplete;
            rptViewer.ReportError -= new ReportErrorEventHandler(rptViewer_ReportError);
        }

        #endregion

        #region Report Viewer Handlers

        void rptViewer_ReportError(object sender, ReportErrorEventArgs e)
        {
            if (null != e.Exception)
            {
                RdlcMessageService.Instance.SendMessage(e.Exception.ToString());
            }
            e.Handled = true;
        }

        private void RptViewer_RenderingComplete(object sender, RenderingCompleteEventArgs e)
        {
            //Console.WriteLine("Render completed call refresh.");
            if (null != rptViewer.LocalReport)
            {
                //rptViewer.LocalReport.Refresh();
            }
            //rptViewer.Refresh();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Load Report.
        /// </summary>
        /// <param name="reportSource">The instance of RdlcReportModel.</param>
        public void LoadReport(RdlcReportModel reportSource)
        {
            rptViewer.LoadReport(reportSource);
            if (rptViewer.ZoomMode != ZoomMode.PageWidth)
            {
                rptViewer.ZoomMode = ZoomMode.PageWidth;
            }
        }
        /// <summary>
        /// Clear Report.
        /// </summary>
        public void ClearReport()
        {
            rptViewer.Clear();
        }
        /// <summary>
        /// Refresh local report.
        /// </summary>
        public void Refresh()
        {
            if (null != rptViewer.LocalReport)
            {
                rptViewer.LocalReport.Refresh();
            }
        }
        /// <summary>
        /// Refresh Report.
        /// </summary>
        public void RefreshReport()
        {
            rptViewer.RefreshReport();
        }
        /// <summary>
        /// Print.
        /// </summary>
        /// <param name="documentName">The document name.</param>
        /// <param name="fromPage">-1 for print all</param>
        /// <param name="toPage">-1 for print all</param>
        /// <param name="noOfCopies">The Number of Copies.</param>
        /// <param name="raiseEvent">True for raise event after print finished.</param>
        /// <returns>Returns true if success.</returns>
        public bool Print(string documentName, int fromPage = -1, int toPage = -1,
            short noOfCopies = 1, bool raiseEvent = true)
        {
            RdlcPrintResult result = rptViewer.PrintTo(
                documentName,
                _printerName, fromPage, toPage, noOfCopies);

            if (result.Success)
            {
                // Raise event.
                if (raiseEvent && null != Printed)
                {
                    Printed.Call(this, EventArgs.Empty);
                }
            }
            else
            {
                if (raiseEvent)
                {
                    MessageBox.Show(result.Message, "CKD Robbing Program");
                }
            }

            return result.Success;
        }
        /// <summary>
        /// Print To.
        /// </summary>
        /// <param name="documentName">The document name.</param>
        public void PrintTo(string documentName)
        {
            int errCnt = 0;
            int fromPage = -1;
            int toPage = -1;
            System.Windows.Controls.PrintDialog pd = new System.Windows.Controls.PrintDialog();
            pd.UserPageRangeEnabled = true;
            if (true == pd.ShowDialog())
            {
                if (null != pd.PrintQueue)
                {
                    _printerName = pd.PrintQueue.FullName;
                    short noOfCopies = (!pd.PrintTicket.CopyCount.HasValue) ?
                        (short)1 : Convert.ToInt16(pd.PrintTicket.CopyCount.Value);

                    System.Windows.Controls.PageRangeSelection sel = pd.PageRangeSelection;
                    if (sel == PageRangeSelection.UserPages)
                    {
                        // User page.
                        if (null != pd.PageRange)
                        {
                            fromPage = pd.PageRange.PageFrom;
                            toPage = pd.PageRange.PageTo;
                        }
                    }

                    // Print but not raise event.
                    if (!this.Print(documentName, fromPage, toPage, noOfCopies, false))
                    {
                        ++errCnt;
                    }
                }
            }
            pd = null;

            // Raise event.
            if (errCnt <= 0)
            {
                if (null != Printed)
                {
                    Printed.Call(this, EventArgs.Empty);
                }
            }
            else
            {
                MessageBox.Show("Print failed.", "CKD Robbing Program");
            }
        }

        #endregion

        #region Public Propeties

        /// <summary>
        /// Gets all report data sources.
        /// </summary>
        public ReportDataSourceCollection DataSources
        {
            get
            {
                LocalReport lr = rptViewer.LocalReport;
                ReportDataSourceCollection datasources = lr.DataSources;
                return datasources;
            }
        }

        #endregion

        #region Public Events

        /// <summary>
        /// The Printed Event Handler.
        /// </summary>
        public event System.EventHandler Printed;

        #endregion
    }
}

