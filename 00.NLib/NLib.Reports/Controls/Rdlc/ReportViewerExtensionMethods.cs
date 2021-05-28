//#define WYSIWYG

#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using Microsoft.Reporting.WinForms;
using NLib;
using NLib.Reports.Rdlc;

#endregion

namespace NLib.Controls
{
    #region Extension Methods for Microsoft Report Viewer Control

    /// <summary>
    /// Windows Forms Report Viewer Control Extension Methods.
    /// </summary>
    public static class ReportViewerExtensionMethods
    {
        #region Public Methods

        /// <summary>
        /// Load Report.
        /// </summary>
        /// <param name="rptViewer">The instance of Report Viewer Control.</param>
        /// <param name="reportSource">The instance of RdlcReportModel.</param>
        public static void LoadReport(this ReportViewer rptViewer,
            RdlcReportModel reportSource)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            med.Info("======== BEGIN LOAD REPORT ========");

            RdlcMessageService.Instance.SendMessage("Begin Load Report.");

            rptViewer.ProcessingMode = ProcessingMode.Local;
            LocalReport lr = rptViewer.LocalReport;
            lr.DisplayName = reportSource.DisplayName;

            med.Info("  - REPORT VIEWER - Check Data Source.");
            if (null != reportSource && null != reportSource.Definition &&
                null != reportSource.Definition.RdlcInstance &&
                null != reportSource.DataSources)
            {
                RdlcMessageService.Instance.SendMessage("Begin Load Report Definition.");

                med.Info("    - REPORT VIEWER - Load Report Definition.");
                // Load Rdlc file
                lr.LoadReportDefinition(reportSource.Definition.RdlcInstance);

                RdlcMessageService.Instance.SendMessage("Clear Report Data source.");

                med.Info("    - REPORT VIEWER - Clear Report datasource(s).");
                // Clear all datasource before assign new one.
                lr.DataSources.Clear();

                if (reportSource.DataSources.Count > 0)
                {
                    // Set all data source.
                    foreach (var dataSource in reportSource.DataSources)
                    {
                        if (null == dataSource.Items)
                            continue;
                        RdlcMessageService.Instance
                            .SendMessage("Add New Report Data source - " + dataSource.Name);

                        med.Info("    - REPORT VIEWER - Add datasource [ " + dataSource.Name + " ].");
                        lr.DataSources.Add(new ReportDataSource(
                            dataSource.Name, dataSource.Items));
                    }
                }
                else
                {
                    RdlcMessageService.Instance
                        .SendMessage("No New Report Data source.");

                    med.Info("    - REPORT VIEWER - No Report datasource.");
                }


                med.Info("    - REPORT VIEWER - Set Report Parameter(s).");
                if (lr.DataSources.Count > 0 &&
                    null != reportSource.Parameters && reportSource.Parameters.Count > 0)
                {
                    foreach (RdlcReportParameter para in reportSource.Parameters)
                    {
                        if (null == para || string.IsNullOrWhiteSpace(para.Name))
                            continue;
                        try
                        {
                            RdlcMessageService.Instance
                                .SendMessage("Set Report parameter - " + para.Name);

                            med.Info("    - REPORT VIEWER - Set Parameter [ " + para.Name  + " ].");
                            lr.SetValue(para.Name, para.Value);
                        }
                        catch (Exception ex) 
                        {
                            med.Err(ex);
                        }
                    }
                }
                else
                {
                    RdlcMessageService.Instance
                        .SendMessage("No Report parameter assigned.");

                    med.Info("    - REPORT VIEWER - No Report parameter.");
                }

                if (lr.DataSources.Count > 0)
                {
                    med.Info("    - REPORT VIEWER - Read pringer page setting(s).");

                    RdlcMessageService.Instance
                        .SendMessage("Set Report Page setting from Printer page setting.");

                    // Read Setting and set to report viewer
                    ReportPageSettings rdlcPageSettings = lr.GetDefaultPageSettings();
                    System.Drawing.Printing.PageSettings pageSettings = new System.Drawing.Printing.PageSettings();

                    pageSettings.PaperSize = rdlcPageSettings.PaperSize;
                    pageSettings.Landscape = rdlcPageSettings.IsLandscape;
                    pageSettings.Margins = rdlcPageSettings.Margins;

                    med.Info("    - REPORT VIEWER - Update Report page setting(s).");
                    rptViewer.SetPageSettings(pageSettings);

                    med.Info("    - REPORT VIEWER - Refresh report.");
                    // refresh
                    rptViewer.LocalReport.Refresh();
                }
                else
                {
                    RdlcMessageService.Instance
                        .SendMessage("No Report data source so cannot set report page setting.");
                }
            }

            med.Info("  - REPORT VIEWER - Refresh Report.");
            rptViewer.RefreshReport();

            med.Info("  - REPORT VIEWER - Set Print Layout.");
            // Set display mode to print layout.
            rptViewer.SetDisplayMode(DisplayMode.PrintLayout);

            if (null != reportSource && null != reportSource.Definition &&
                null != reportSource.Definition.RdlcInstance)
            {
                try
                {
                    reportSource.Definition.Dispose();
                }
                catch { }
            }

            med.Info("======== END LOAD REPORT ========");
        }
        /// <summary>
        /// Print Report to specificed printer.
        /// </summary>
        /// <param name="rptViewer">The instance of Report Viewer Control.</param>
        /// <param name="documentName">The document name.</param>
        /// <param name="printerName">The target printer name.</param>
        /// <param name="fromPage">-1 for print all</param>
        /// <param name="toPage">-1 for print all</param>
        /// <param name="noOfCopies">The Number of Copies.</param>
        /// <returns>Returns instance of RdlcPrintResult.</returns>
        public static RdlcPrintResult PrintTo(this ReportViewer rptViewer, 
            string documentName,
            string printerName,
            int fromPage = -1, int toPage = -1, 
            short noOfCopies = 1)
        {
            RdlcPrintResult result = new RdlcPrintResult();
            result.Success = false;

            if (string.IsNullOrWhiteSpace(printerName))
            {
                result.Message = "No printer selected.";

                RdlcMessageService.Instance.SendMessage(result.Message);

                return result;
            }

            if (null == rptViewer.LocalReport ||
                null == rptViewer.LocalReport.DataSources ||
                rptViewer.LocalReport.DataSources.Count <= 0)
            {
                result.Message = "Report is not loaded or report has no data.";

                RdlcMessageService.Instance.SendMessage(result.Message);

                return result;
            }

            LocalReportRenderer prt = null;
            try
            {
                LocalReport lr = rptViewer.LocalReport;
                lr.DisplayName = documentName;
                ReportPageSettings settings = lr.GetDefaultPageSettings();

                if (null == settings)
                {
                    result.Message = "Cannot get page settings.";

                    RdlcMessageService.Instance.SendMessage(result.Message);

                    return result;
                }

                #region Assign page setting
                
                LocalReportPageSettings pageSettings = new LocalReportPageSettings();

                if (!settings.IsLandscape)
                {
                    pageSettings.PageHeight = Convert.ToDouble(
                        (double)settings.PaperSize.Height / (double)100);
                    pageSettings.PageWidth = Convert.ToDouble(
                        (double)settings.PaperSize.Width / (double)100);
                }
                else
                {
                    pageSettings.PageHeight = Convert.ToDouble(
                        (double)settings.PaperSize.Width / (double)100);
                    pageSettings.PageWidth = Convert.ToDouble(
                        (double)settings.PaperSize.Height / (double)100);
                }
                pageSettings.MarginLeft = Convert.ToDouble(
                        (double)settings.Margins.Left / (double)100);
                pageSettings.MarginRight = Convert.ToDouble(
                        (double)settings.Margins.Right / (double)100);
                pageSettings.MarginTop = Convert.ToDouble(
                        (double)settings.Margins.Top / (double)100);
                pageSettings.MarginBottom = Convert.ToDouble(
                        (double)settings.Margins.Bottom / (double)100);

                //pageSettings.MarginLeft = 0;
                //pageSettings.MarginRight = 0;
                //pageSettings.MarginTop = 0;
                //pageSettings.MarginBottom = 0;

                pageSettings.Landscape = settings.IsLandscape;

                pageSettings.FromPage = fromPage;
                pageSettings.ToPage = toPage;

#if WYSIWYG
                // Read Screen Resolution
                int dx, dy;
                System.Drawing.Graphics g = rptViewer.CreateGraphics();
                try
                {
                    dx = Convert.ToInt32(g.DpiX);
                    dy = Convert.ToInt32(g.DpiY);
                }
                finally
                {
                    g.Dispose();
                }
                pageSettings.DpiX = dx;
                pageSettings.DpiY = dy;                

                // Create dialog and re read resolution.
                System.Windows.Forms.PrintDialog pd = new System.Windows.Forms.PrintDialog();
                pd.PrinterSettings.PrinterName = printerName;
                System.Drawing.Printing.PrinterSettings ps = pd.PrinterSettings;

                pageSettings.PrintDpiX = ps.DefaultPageSettings.PrinterResolution.X;
                pageSettings.PrintDpiY = ps.DefaultPageSettings.PrinterResolution.Y;                
#endif
                #endregion

                prt = new LocalReportRenderer();
                prt.Print(lr, documentName, printerName, pageSettings, noOfCopies);

                result.Success = true;
                result.Message = "Print Success.";
                RdlcMessageService.Instance.SendMessage(result.Message);
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
            }
            finally
            {
                if (null != prt)
                {
                    prt.Dispose();
                }
                prt = null;
            }

            return result;
        }

        #endregion
    }

    #endregion
}