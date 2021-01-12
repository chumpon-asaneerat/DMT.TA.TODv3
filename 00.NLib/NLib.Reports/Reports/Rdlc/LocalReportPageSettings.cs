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
using System.Drawing.Printing;
using System.Drawing.Imaging;
using System.Printing;

#endregion

namespace NLib.Reports.Rdlc
{
    #region LocalReportPageSettings

    /// <summary>
    /// LocalReport Page Settings class.
    /// </summary>
    [Serializable]
    public class LocalReportPageSettings
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public LocalReportPageSettings()
            : base()
        {
            // Letter size.
            this.PageWidth = 8.5;
            this.PageHeight = 11;

            this.MarginLeft = 1;
            this.MarginTop = 1;
            this.MarginRight = 1;
            this.MarginBottom = 1;

            this.Landscape = false;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets Page Width in inch.
        /// </summary>
        [XmlAttribute]
        public double PageWidth { get; set; }
        /// <summary>
        /// Gets or sets Page Height in inch.
        /// </summary>
        [XmlAttribute]
        public double PageHeight { get; set; }
        /// <summary>
        /// Gets or sets Page Margin Left in inch.
        /// </summary>
        [XmlAttribute]
        public double MarginLeft { get; set; }
        /// <summary>
        /// Gets or sets Page Margin Top in inch.
        /// </summary>
        [XmlAttribute]
        public double MarginTop { get; set; }
        /// <summary>
        /// Gets or sets Page Margin Right in inch.
        /// </summary>
        [XmlAttribute]
        public double MarginRight { get; set; }
        /// <summary>
        /// Gets or sets Page Margin Bottom in inch.
        /// </summary>
        [XmlAttribute]
        public double MarginBottom { get; set; }
        /// <summary>
        /// Gets or sets is landscape.
        /// </summary>
        [XmlAttribute]
        public bool Landscape { get; set; }
        /// <summary>
        /// Gets or sets From Page. -1 for all.
        /// </summary>
        [XmlIgnore]
        public int FromPage { get; set; }
        /// <summary>
        /// Gets or sets To Page. -1 for all.
        /// </summary>
        [XmlIgnore]
        public int ToPage { get; set; }
#if WYSIWYG
        /// <summary>
        /// Gets or sets Dpi X.
        /// </summary>
        [XmlIgnore]
        public int DpiX { get; set; }
        /// <summary>
        /// Gets or sets Dpi Y.
        /// </summary>
        [XmlIgnore]
        public int DpiY { get; set; }
        /// <summary>
        /// Gets or sets Printer Dpi X.
        /// </summary>
        [XmlIgnore]
        public int PrintDpiX { get; set; }
        /// <summary>
        /// Gets or sets Printer Dpi Y.
        /// </summary>
        [XmlIgnore]
        public int PrintDpiY { get; set; }
#endif
        #endregion
    }

    #endregion
}