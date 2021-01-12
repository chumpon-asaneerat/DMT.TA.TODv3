#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Reporting.WinForms;
using NLib.Reports.Rdlc;

#endregion

namespace NLib.Controls
{
    #region Extension Methods for Local Report class

    /// <summary>
    /// Windows Forms Local Report Extension Methods.
    /// </summary>
    public static class LocalReportExtensionMethods
    {
        #region Local Report

        /// <summary>
        /// Find Parameter.
        /// </summary>
        /// <param name="localReport">The local report instance.</param>
        /// <param name="name">The parameter's name.</param>
        /// <returns>Returns instance of ReportParameterInfo if found.</returns>
        private static ReportParameterInfo FindParameter(this LocalReport localReport,
            string name)
        {
            ReportParameterInfo result = null;

            if (null == localReport)
            {
                return result;
            }

            ReportParameterInfoCollection parameters = localReport.GetParameters();
            if (null != parameters && parameters.Count > 0)
            {
                result = parameters.FirstOrDefault(
                    (ReportParameterInfo para) =>
                    {
                        return string.Compare(para.Name, name, true) == 0;
                    });
            }

            return result;
        }
        /// <summary>
        /// Set Parameter Value.
        /// </summary>
        /// <param name="localReport">The local report instance.</param>
        /// <param name="name">The parameter's name.</param>
        /// <param name="value">The parameter's value.</param>
        public static void SetValue(this LocalReport localReport,
            string name, string value)
        {
            if (null == localReport)
                return;

            ReportParameterInfo para = localReport.FindParameter(name);
            if (null != para)
            {
                localReport.SetParameters(new ReportParameter(para.Name, value));
            }
        }

        #endregion
    }

    #endregion
}
