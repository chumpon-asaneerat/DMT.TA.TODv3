#region Using

using System;
using System.Collections.Generic;
using NLib.Reflection;

#endregion

namespace DMT.Models
{
    #region Jobs

    #region SCWBOJ/SCWBOJResult

    #region Parameter class

    /// <summary>
    /// SCW BOJ class.
    /// </summary>
    public class SCWBOJ
    {
        /// <summary>Gets or sets networkId.</summary>
        [PropertyMapName("networkId")]
        public int? networkId { get; set; }

        /// <summary>Gets or sets plazaId.</summary>
        [PropertyMapName("plazaId")]
        public int? plazaId { get; set; }

        /// <summary>Gets or sets laneId.</summary>
        [PropertyMapName("laneId")]
        public int? laneId { get; set; }

        /// <summary>Gets or sets jobNo.</summary>
        [PropertyMapName("jobNo")]
        public int? jobNo { get; set; }

        /// <summary>Gets or sets staffId.</summary>
        [PropertyMapName("staffId")]
        public string staffId { get; set; }
    }

    #endregion

    #region Result class

    /// <summary>
    /// SCW BOJ Result class.
    /// </summary>
    public class SCWBOJResult : SCWResult
    {

    }

    #endregion

    #endregion

    #region SCWEOJ/SCWEOJResult

    #region Parameter class

    /// <summary>
    /// SCW EOJ class.
    /// </summary>
    public class SCWEOJ
    {
        /// <summary>Gets or sets networkId.</summary>
        [PropertyMapName("networkId")]
        public int? networkId { get; set; }

        /// <summary>Gets or sets plazaId.</summary>
        [PropertyMapName("plazaId")]
        public int? plazaId { get; set; }

        /// <summary>Gets or sets laneId.</summary>
        [PropertyMapName("laneId")]
        public int? laneId { get; set; }

        /// <summary>Gets or sets jobNo.</summary>
        [PropertyMapName("jobNo")]
        public int? jobNo { get; set; }

        /// <summary>Gets or sets staffId.</summary>
        [PropertyMapName("staffId")]
        public string staffId { get; set; }
    }

    #endregion

    #region Result class

    /// <summary>
    /// SCW EOJ Result class.
    /// </summary>
    public class SCWEOJResult : SCWResult
    {

    }

    #endregion

    #endregion

    #region SCWRemoveJobs/SCWRemoveJobsResult

    #region Parameter class

    /// <summary>
    /// The SCWRemoveJobs class.
    /// </summary>
    public class SCWRemoveJobs
    {
        public SCWRemoveJobs() : base()
        {
            jobs = new List<SCWJob>();
        }
        public List<SCWJob> jobs { get; set; }
    }

    #endregion

    #region Result class

    /// <summary>
    /// The SCWRemoveJobsResult class
    /// </summary>
    public class SCWRemoveJobsResult : SCWResult
    {

    }

    #endregion

    #endregion

    #region SCWClearJobs/SCWClearJobsResult

    #region Parameter class

    /// <summary>
    /// The SCWClearJobs class.
    /// </summary>
    public class SCWClearJobs { }

    #endregion

    #region Result class

    /// <summary>
    /// The SCWClearJobsResult class.
    /// </summary>
    public class SCWClearJobsResult : SCWResult
    {

    }

    #endregion

    #endregion

    #region SCWAllJob/SCWAllJobResult

    #region Parameter class

    /// <summary>
    /// SCW AllJob class.
    /// </summary>
    public class SCWAllJob
    {
        /// <summary>Gets or sets networkId.</summary>
        [PropertyMapName("networkId")]
        public int? networkId { get; set; }

        /// <summary>Gets or sets plazaId.</summary>
        [PropertyMapName("plazaId")]
        public int? plazaId { get; set; }
    }

    #endregion

    #region Result class

    /// <summary>
    /// SCW AllJob Result class.
    /// </summary>
    public class SCWAllJobResult : SCWResult
    {
        /// <summary>Gets or sets list.</summary>
        //[PropertyMapName("list")]
        public List<SCWJob> list { get; set; }
    }

    #endregion

    #endregion

    #endregion

    #region EMV

    #region SCWAddEMV/SCWAddEMVResult

    #region Parameter Class

    /// <summary>
    /// The SCWAddEMV class
    /// </summary>
    public class SCWAddEMV
    {
        /// <summary>Gets or sets networkId.</summary>
        [PropertyMapName("networkId")]
        public int? networkId { get; set; }

        /// <summary>Gets or sets plazaId.</summary>
        [PropertyMapName("plazaId")]
        public int? plazaId { get; set; }

        /// <summary>Gets or sets laneId.</summary>
        [PropertyMapName("laneId")]
        public int? laneId { get; set; }

        /// <summary>Gets or sets staffId.</summary>
        [PropertyMapName("staffId")]
        public string staffId { get; set; }

        [PropertyMapName("staffNameTh")]
        public string staffNameTh { get; set; }

        [PropertyMapName("staffNameEn")]
        public string staffNameEn { get; set; }

        [PropertyMapName("trxDateTime")]
        public DateTime? trxDateTime { get; set; }

        [PropertyMapName("amount")]
        public decimal? amount { get; set; }

        [PropertyMapName("approvalCode")]
        public string approvalCode { get; set; }

        [PropertyMapName("refNo")]
        public string refNo { get; set; }
    }

    #endregion

    #region Result Class

    /// <summary>
    /// The SCWAddEMVResult class
    /// </summary>
    public class SCWAddEMVResult : SCWResult
    {

    }

    #endregion

    #endregion

    #region SCWRemoveEMV/SCWRemoveEMVResult

    #region Parameter class

    /// <summary>
    /// The SCWRemoveEMV class.
    /// </summary>
    public class SCWRemoveEMV
    {
        /// <summary>Gets or sets trxDateTime.</summary>
        [PropertyMapName("trxDateTime")]
        public DateTime? trxDateTime { get; set; }

        /// <summary>Gets or sets approvalCode.</summary>
        [PropertyMapName("approvalCode")]
        public string approvalCode { get; set; }
    }

    #endregion

    #region Result class

    /// <summary>
    /// The SCWRemoveEMVResult class
    /// </summary>
    public class SCWRemoveEMVResult : SCWResult
    {

    }

    #endregion

    #endregion

    #region SCWClearEMVs/SCWClearEMVsResult

    #region Parameter class

    /// <summary>
    /// The SCWClearEMVs class.
    /// </summary>
    public class SCWClearEMVs { }

    #endregion

    #region Result class

    /// <summary>
    /// The SCWClearEMVsResult class.
    /// </summary>
    public class SCWClearEMVsResult : SCWResult
    {

    }

    #endregion

    #endregion

    #endregion

    #region QRCode

    #region SCWAddQRCode/SCWAddQRCodeResult

    #region Parameter Class

    /// <summary>
    /// The SCWAddQRCode class
    /// </summary>
    public class SCWAddQRCode
    {
        /// <summary>Gets or sets networkId.</summary>
        [PropertyMapName("networkId")]
        public int? networkId { get; set; }

        /// <summary>Gets or sets plazaId.</summary>
        [PropertyMapName("plazaId")]
        public int? plazaId { get; set; }

        /// <summary>Gets or sets laneId.</summary>
        [PropertyMapName("laneId")]
        public int? laneId { get; set; }

        /// <summary>Gets or sets staffId.</summary>
        [PropertyMapName("staffId")]
        public string staffId { get; set; }

        [PropertyMapName("staffNameTh")]
        public string staffNameTh { get; set; }

        [PropertyMapName("staffNameEn")]
        public string staffNameEn { get; set; }

        [PropertyMapName("trxDateTime")]
        public DateTime? trxDateTime { get; set; }

        [PropertyMapName("amount")]
        public decimal? amount { get; set; }

        [PropertyMapName("approvalCode")]
        public string approvalCode { get; set; }

        [PropertyMapName("refNo")]
        public string refNo { get; set; }
    }

    #endregion

    #region Result Class

    /// <summary>
    /// The SCWAddQRCodeResult class
    /// </summary>
    public class SCWAddQRCodeResult : SCWResult
    {

    }

    #endregion

    #endregion

    #region SCWRemoveQRCode/SCWRemoveQRCodeResult

    #region Parameter class

    /// <summary>
    /// The SCWRemoveQRCode class.
    /// </summary>
    public class SCWRemoveQRCode
    {
        /// <summary>Gets or sets trxDateTime.</summary>
        [PropertyMapName("trxDateTime")]
        public DateTime? trxDateTime { get; set; }

        /// <summary>Gets or sets approvalCode.</summary>
        [PropertyMapName("approvalCode")]
        public string approvalCode { get; set; }
    }

    #endregion

    #region Result class

    /// <summary>
    /// The SCWRemoveQRCodeResult class
    /// </summary>
    public class SCWRemoveQRCodeResult : SCWResult
    {

    }

    #endregion

    #endregion

    #region SCWClearQRCodes/SCWClearQRCodesResult

    #region Parameter class

    /// <summary>
    /// The SCWClearJobs class.
    /// </summary>
    public class SCWClearQRCodes { }

    #endregion

    #region Result class

    /// <summary>
    /// The SCWClearJobsResult class.
    /// </summary>
    public class SCWClearQRCodesResult : SCWResult
    {

    }

    #endregion

    #endregion

    #endregion
}
