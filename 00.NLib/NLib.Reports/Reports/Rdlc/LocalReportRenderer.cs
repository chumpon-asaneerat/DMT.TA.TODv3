//#define WYSIWYG

#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Reflection;

using NLib;
using NLib.Xml;

using Microsoft.Reporting.WinForms;
using System.Drawing;
using System.Drawing.Printing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Printing;

#endregion

using NLib.Controls;

namespace NLib.Reports.Rdlc
{
    #region LocalReportRenderer

    /// <summary>
    /// LocalReport Renderer class. 
    /// See https://msdn.microsoft.com/en-us/library/ms252091(v=VS.80).aspx
    /// for more information.
    /// </summary>
    public class LocalReportRenderer : IDisposable
    {
        #region Internal Variables

        private int m_currentPageIndex;
        private IList<Stream> m_streams;
        private LocalReportPageSettings _ps = null;

        #endregion

        #region Private Methods

        /// <summary>
        /// Routine to provide to the report renderer, in order to save an image for each page of the report.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fileNameExtension"></param>
        /// <param name="encoding"></param>
        /// <param name="mimeType"></param>
        /// <param name="willSeek"></param>
        /// <returns></returns>
        private Stream CreateStream(string name,
          string fileNameExtension, Encoding encoding,
          string mimeType, bool willSeek)
        {
            //string pathName = @"..\..\";
            string pathName = Path.GetTempPath();
            string fileName = name + "." + fileNameExtension;
            string printFileName = Path.GetFullPath(Path.Combine(pathName, fileName));
            Stream stream = new FileStream(printFileName, FileMode.Create);
            m_streams.Add(stream);
            return stream;
        }
        /// <summary>
        /// Free all print image (EMF) streams.
        /// </summary>
        private void FreeStreams()
        {
            if (m_streams != null)
            {
                foreach (Stream stream in m_streams)
                    stream.Close();
                m_streams = null;
            }
        }
        /// <summary>
        /// Export the given report as an EMF (Enhanced Metafile) file.
        /// </summary>
        /// <param name="report">The local report instance.</param>
        /// <param name="pageSettings">The page setting.</param>
        /// <returns>Returns true if success.</returns>
        private bool Export(LocalReport report, LocalReportPageSettings pageSettings)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            string deviceInfo = string.Empty;
            LocalReportPageSettings pgSettings = pageSettings;
            if (null == pgSettings)
            {
                pgSettings = new LocalReportPageSettings();
            }

            _ps = pgSettings;

            string pageTags = string.Empty;

            if (pageSettings.FromPage != -1 && pageSettings.ToPage != -1)
            {
                string pageTagsFmt = string.Empty;
                pageTagsFmt += "<StartPage>{0}</StartPage>";
                pageTagsFmt += "<EndPage>{1}</EndPage>";
                pageTags = string.Format(pageTagsFmt,
                    pageSettings.FromPage, pageSettings.ToPage);
            }

            string deviceInfofmt =
              "<DeviceInfo>" +
              "  <OutputFormat>EMF</OutputFormat>" +
              "  <PageWidth>{0}in</PageWidth>" +
              "  <PageHeight>{1}in</PageHeight>" +
              "  <MarginTop>{2}in</MarginTop>" +
              "  <MarginLeft>{3}in</MarginLeft>" +
              "  <MarginRight>{4}in</MarginRight>" +
              "  <MarginBottom>{5}in</MarginBottom>" +
              //"  <DpiX>{6}</DpiX>" +
              //"  <DpiY>{7}</DpiY>" +
              //"  <PrintDpiX>{8}</PrintDpiX>" +
              //"  <PrintDpiY>{9}</PrintDpiY>" +
              pageTags +
              "</DeviceInfo>";
            deviceInfo = string.Format(deviceInfofmt,
                pgSettings.PageWidth,
                pgSettings.PageHeight,
                pgSettings.MarginTop,
                pgSettings.MarginLeft,
                pgSettings.MarginRight,
                pgSettings.MarginBottom//,
                //pgSettings.DpiX,
                //pgSettings.DpiY,
                //pgSettings.PrintDpiX,
                //pgSettings.PrintDpiY
            );
            try
            {
                FreeStreams(); // Free exist image streams.
                Warning[] warnings;
                m_streams = new List<Stream>();
                report.Render("Image", deviceInfo, CreateStream, out warnings);
                foreach (Stream stream in m_streams)
                    stream.Position = 0;
            }
            catch (Exception ex)
            {
                med.Err(ex);

                RdlcMessageService.Instance.SendMessage(ex.ToString());

                FreeStreams();
            }

            return true;
        }
        /// <summary>
        /// Handler for PrintPage Events.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ev"></param>
        private void PrintPage(object sender, PrintPageEventArgs ev)
        {
            //1. Avoid unintended rounding issues
            //   by specifying units directly as 100ths of a mm (GDI)
            //   This can be done by reading directly as a stream with no HDC
            Metafile pageImage = new
               Metafile(m_streams[m_currentPageIndex]);
#if WYSIWYG
            //2. Apply scaling to correct for dpi differences
            //   E.g. the mf.Width needs the PrintDpiX in order
            //        to be translated to a real width (in mm)
            var mfh = pageImage.GetMetafileHeader();
            ev.Graphics.ScaleTransform(mfh.DpiX / _ps.PrintDpiX, mfh.DpiY /
                                      _ps.PrintDpiY, MatrixOrder.Prepend);
            //3. Apply scaling to fit to current page by shortest difference
            //   by comparing real page size with available printable area in 100ths of an inch
            var size = new PointF((float)pageImage.Size.Width / _ps.PrintDpiX * 100.0f,
                                  (float)pageImage.Size.Height / _ps.PrintDpiY * 100.0f);
            size.X = (float)Math.Floor(ev.PageSettings.PrintableArea.Width) / size.X;
            size.Y = (float)Math.Floor(ev.PageSettings.PrintableArea.Height) / size.Y;
            var scale = Math.Min(size.X, size.Y);
            ev.Graphics.ScaleTransform(scale, scale, MatrixOrder.Append);

            //4. Draw the image at the adjusted coordinates
            //   i.e. these are real coordinates so need to reverse the transform that is about
            //        to be applied so that when it is, these real coordinates will be the result.
            var points = new[] { new Point(ev.PageSettings.Margins.Left, ev.PageSettings.Margins.Top) };
            var matrix = ev.Graphics.Transform;
            matrix.Invert();
            matrix.TransformPoints(points);

            ev.Graphics.DrawImageUnscaled(pageImage, points[0]);
#else
            // Adjust rectangular area with printer margins.
            //Rectangle adjustedRect = new Rectangle(
            //    (int)(_ps.MarginLeft * 100.0f),
            //    (int)(_ps.MarginTop * 100.0f),
            //    (int)((_ps.PageWidth - _ps.MarginLeft - _ps.MarginRight) * 100.0f),
            //    (int)((_ps.PageHeight - _ps.MarginTop - _ps.MarginBottom) * 100.0f));
            //ev.Graphics.DrawImage(pageImage, adjustedRect); // Adjust
            
            ev.Graphics.DrawImage(pageImage, ev.PageBounds); // Original

#endif
            m_currentPageIndex++;
            ev.HasMorePages = (m_currentPageIndex < m_streams.Count);
        }
        /// <summary>
        /// Print.
        /// </summary>
        /// <param name="report"></param>
        /// <param name="documentName"></param>
        /// <param name="printerName"></param>
        /// <param name="pageSettings"></param>
        /// <param name="noOfCopies"></param>
        /// <returns></returns>
        public bool Print(LocalReport report, string documentName, string printerName,
            LocalReportPageSettings pageSettings, short noOfCopies = 1)
        {
            RdlcMessageService.Instance.SendMessage("Begin Render/Print process");

            MethodBase med = MethodBase.GetCurrentMethod();

            bool result = false;

            if (!Export(report, pageSettings))
            {
                RdlcMessageService.Instance.SendMessage("Cannot Render output.");
                return result;
            }
            else
            {
                RdlcMessageService.Instance.SendMessage("Render output completed.");
            }

            string sPrinterName = printerName;
            
            //const string printerName = "Microsoft Office Document Image Writer";
            if (m_streams == null || m_streams.Count == 0)
                return result;

            PrintDocument printDoc = new PrintDocument();

            if (!string.IsNullOrWhiteSpace(sPrinterName))
            {
                printDoc.PrinterSettings.PrinterName = sPrinterName;

                RdlcMessageService.Instance.SendMessage("Target Printer : " + printerName);
            }
            else
            {
                // It's should used default printer.
                RdlcMessageService.Instance
                    .SendMessage("Not specificed printer name so Default printer should be used instead.");
            }
            if (!printDoc.PrinterSettings.IsValid)
            {
                string msg = String.Format(
                   "Can't find printer \"{0}\".", printerName);
                med.Err(msg);

                RdlcMessageService.Instance.SendMessage(msg);

                return result;
            }

            // Set land scape.
            printDoc.DefaultPageSettings.Landscape = pageSettings.Landscape;
            // Set No of Copies.
            printDoc.DefaultPageSettings.PrinterSettings.Copies = noOfCopies;
            // Set From Page/To Page.
            //if (pageSettings.FromPage != -1)
            //    printDoc.DefaultPageSettings.PrinterSettings.FromPage = pageSettings.FromPage;
            //if (pageSettings.ToPage != -1)
            //    printDoc.DefaultPageSettings.PrinterSettings.ToPage = pageSettings.ToPage;

            printDoc.DocumentName = documentName; // Set document name.

            // Hook handlers
            printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
            try
            {
                //for (int i = 0; i < noOfCopies; ++i)
                {
                    m_currentPageIndex = 0; // Reset print index.
                    printDoc.Print();
                }
                // print OK.
                result = true;
            }
            catch (Exception ex)
            {
                med.Err(ex);

                RdlcMessageService.Instance.SendMessage(ex.ToString());
            }
            finally
            {
                FreeStreams();
            }

            RdlcMessageService.Instance.SendMessage("End Render/Print process");

            return result;
        }
        /// <summary>
        /// Dispose.
        /// </summary>
        public void Dispose()
        {
            FreeStreams();
        }

        #endregion
    }

    #endregion
}