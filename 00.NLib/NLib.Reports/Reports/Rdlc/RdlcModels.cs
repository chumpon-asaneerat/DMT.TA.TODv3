#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

using Microsoft.Reporting.WinForms;

#endregion

namespace NLib.Reports.Rdlc
{
    public static class RdlcReportUtils
    {
        /// <summary>
        /// Get Embeded Report stream.
        /// </summary>
        /// <param name="assembly">The assembly that has embedded report.</param>
        /// <param name="embededReportName">
        /// The full namespace of the rdcl report (inclide extension) 
        /// and the report should be set as embeded resource.
        /// </param>
        /// <returns>Returns the rdlc report stream.</returns>
        public static Stream GetEmbededReport(Assembly assembly, string embededReportName)
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            Stream stream = null;
            if (null != assembly)
            {
                try { stream = assembly.GetManifestResourceStream(embededReportName); }
                catch (Exception ex)
                {
                    med.Err(ex);
                }
            }
            return stream;
        }
    }

    #region RdlcReportManager

    /// <summary>
    /// The Rdlc Report Manager.
    /// </summary>
    public abstract class RdlcReportManager
    {
        #region Public Methods

        /// <summary>
        /// Get Embeded Report stream.
        /// </summary>
        /// <param name="embededReportName">
        /// The full namespace of the rdcl report (inclide extension) 
        /// and the report should be set as embeded resource.
        /// </param>
        /// <returns>Returns the rdlc report stream.</returns>
        public Stream GetEmbededReport(string embededReportName)
        {
            /*
            MethodBase med = MethodBase.GetCurrentMethod();
            Assembly assembly = this.GetType().Assembly;
            Stream stream = null;
            if (null != assembly)
            {
                try { stream = assembly.GetManifestResourceStream(embededReportName); }
                catch (Exception ex)
                {
                    med.Err(ex);
                }
            }
            */
            return RdlcReportUtils.GetEmbededReport(this.GetType().Assembly, 
                embededReportName);
        }

        #endregion
    }

    #endregion

    #region RdlcReportCriteria

    /// <summary>
    /// The RdlcReportCriteria class.
    /// </summary>
    public abstract class RdlcReportCriteria
    {
        #region Abstract Methods

        /// <summary>
        /// Verify.
        /// </summary>
        /// <returns>Retruns true if criteria is valid.</returns>
        public abstract bool Verify();

        #endregion
    }

    /// <summary>
    /// The RdlcReportDateCriteria class.
    /// </summary>
    public class RdlcReportDateCriteria : RdlcReportCriteria
    {
        #region Override Methods

        /// <summary>
        /// Verify.
        /// </summary>
        /// <returns>Retruns true if criteria is valid.</returns>
        public override bool Verify()
        {
            bool result = false;
            MethodBase med = MethodBase.GetCurrentMethod();

            if (FromDate.HasValue && ToDate.HasValue)
            {
                result = FromDate.Value <= ToDate.Value;
                if (!result)
                {
                    "From Date is greater than To Date.".Err(med);
                }
            }
            else if (!FromDate.HasValue && ToDate.HasValue)
            {
                "From Date is null but To Date has value.".Err(med);
                result = false;
            }
            else
            {
                result = true; // show all
            }

            return result;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets from date.
        /// </summary>
        public DateTime? FromDate { get; set; }
        /// <summary>
        /// Gets or sets to date.
        /// </summary>
        public DateTime? ToDate { get; set; }

        #endregion
    }

    #endregion

    #region RdlcReportDefinition

    /// <summary>
    /// The RdlcReportDefinition class.
    /// </summary>
    public class RdlcReportDefinition : IDisposable
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public RdlcReportDefinition() : base() { }

        #endregion

        #region Dispose

        /// <summary>
        /// Dispose.
        /// </summary>
        public void Dispose()
        {
            if (null != RdlcInstance)
            {
                try
                {
                    RdlcInstance.Close();
                    RdlcInstance.Dispose();
                }
                catch { }
            }
            RdlcInstance = null;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets embeded report name. 
        /// Used full namespace and the rdcl report should be set as embeded resource.
        /// </summary>
        public string EmbededReportName { get; set; }
        /// <summary>
        /// Gets or sets report stream.
        /// </summary>
        public Stream RdlcInstance { get; set; }

        #endregion
    }

    #endregion

    #region RdlcReportParameter

    /// <summary>
    /// The RdlcReportParameter class.
    /// </summary>
    public class RdlcReportParameter
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public RdlcReportParameter()
            : base()
        {
            this.Name = string.Empty;
            this.Value = string.Empty;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets value.
        /// </summary>
        public string Value { get; set; }

        #endregion

        #region Static Methods

        /// <summary>
        /// Create new report parameter.
        /// </summary>
        /// <param name="name">The parameter's name.</param>
        /// <param name="value">The parameter's value.</param>
        /// <returns>Returns instance of RdlcReportParameter.</returns>
        public static RdlcReportParameter Create(string name, string value)
        {
            return new RdlcReportParameter() { Name = name, Value = value };
        }

        #endregion
    }

    #endregion

    #region RdlcReportDataSource

    /// <summary>
    /// The RdlcReportDataSource class.
    /// </summary>
    public class RdlcReportDataSource
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public RdlcReportDataSource()
            : base()
        {
            this.Name = "main";
            this.Items = null;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets Datasource name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets item source.
        /// </summary>
        public object Items { get; set; }

        #endregion
    }

    #endregion

    #region RdlcReportModel

    /// <summary>
    /// The RdlcReportModel class.
    /// </summary>
    public class RdlcReportModel
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public RdlcReportModel()
            : base()
        {
            this.Criteria = null;
            this.Definition = new RdlcReportDefinition();
            this.DataSources = new List<RdlcReportDataSource>();
            this.Parameters = new List<RdlcReportParameter>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets criteria.
        /// </summary>
        public RdlcReportCriteria Criteria { get; set; }
        /// <summary>
        /// Gets report definition.
        /// </summary>
        public RdlcReportDefinition Definition { get; private set; }
        /// <summary>
        /// Gets or sets data source.
        /// </summary>
        public List<RdlcReportDataSource> DataSources { get; private set; }
        /// <summary>
        /// Gets the parameter list.
        /// </summary>
        public List<RdlcReportParameter> Parameters { get; private set; }
        /// <summary>
        /// Gets or sets current document display name.
        /// </summary>
        public string DisplayName { get; set; }

        #endregion
    }

    #endregion

    #region RdlcPrintResult

    /// <summary>
    /// The RdlcReportModel class.
    /// </summary>
    public class RdlcPrintResult
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public RdlcPrintResult()
            : base()
        {
            this.Success = false;
            this.Message = string.Empty;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets is success.
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// Gets the parameter list.
        /// </summary>
        public string Message { get; set; }

        #endregion
    }

    #endregion
}
