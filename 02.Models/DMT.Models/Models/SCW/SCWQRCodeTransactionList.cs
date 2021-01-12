#region Using

using System;
using System.Collections.Generic;
using NLib.Reflection;
using Newtonsoft.Json;

#endregion

namespace DMT.Models
{
    #region Parameter class

    /// <summary>The SCWQRCodeTransactionList class.</summary>
    public class SCWQRCodeTransactionList
    {
        /// <summary>Gets or sets networkId.</summary>
        [PropertyMapName("networkId")]
        public int? networkId { get; set; }

        /// <summary>Gets or sets plazaId.</summary>
        [PropertyMapName("plazaId")]
        public int? plazaId { get; set; }

        /// <summary>Gets or sets staffId.</summary>
        [PropertyMapName("staffId")]
        public string staffId { get; set; }

        /// <summary>Gets or sets startDateTime.</summary>
        [PropertyMapName("startDateTime")]
        public DateTime? startDateTime { get; set; }

        /// <summary>Gets or sets endDateTime.</summary>
        [PropertyMapName("endDateTime")]
        public DateTime? endDateTime { get; set; }
    }

    #endregion

    #region Result related classes

    /// <summary>The SCWQRCodeTransaction class.</summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class SCWQRCodeTransaction
    {
        /// <summary>Gets or sets trxDateTime.</summary>
        [PropertyMapName("trxDateTime")]
        public DateTime? trxDateTime { get; set; }

        /// <summary>Gets or sets amount.</summary>
        [PropertyMapName("amount")]
        public decimal? amount { get; set; }

        /// <summary>Gets or sets approvCode.</summary>
        [PropertyMapName("approvCode")]
        public string approvCode { get; set; }

        /// <summary>Gets or sets refNo.</summary>
        [PropertyMapName("refNo")]
        public string refNo { get; set; }

        /// <summary>Gets or sets staffId.</summary>
        [PropertyMapName("staffId")]
        public string staffId { get; set; }

        /// <summary>Gets or sets staffNameTh.</summary>
        [PropertyMapName("staffNameTh")]
        public string staffNameTh { get; set; }

        /// <summary>Gets or sets staffNameEn.</summary>
        [PropertyMapName("staffNameEn")]
        public string staffNameEn { get; set; }

        /// <summary>Gets or sets laneId.</summary>
        [PropertyMapName("laneId")]
        public int laneId { get; set; }

        /// <summary>Gets trxDateTimeString.</summary>
        [JsonIgnore]
        public string trxDateTimeString
        {
            get
            {
                if (!trxDateTime.HasValue) return string.Empty;
                return (trxDateTime.Value == DateTime.MinValue) ? string.Empty : trxDateTime.Value.ToThaiDateTimeString("dd/MM/yyyy HH:mm:ss");
            }
            set { }
        }
        /// <summary>
        /// GetHashCode.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            decimal amt = (amount.HasValue) ? amount.Value : decimal.Zero;
            DateTime dt = (trxDateTime.HasValue) ?
                trxDateTime.Value : DateTime.MinValue;
            string dtStr = dt.ToDateTimeString();
            string value = string.Format("{0}_{1}_{2}_{3}_{4}_{5}_{6}_{7}",
                this.staffId, this.staffNameEn, this.staffNameTh,
                this.laneId,
                this.refNo, this.approvCode, amt,
                dtStr);
            return value.GetHashCode();
        }
        /// <summary>
        /// Equals.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (null == obj) return false;
            return obj.GetHashCode() == this.GetHashCode();
        }
    }

    /// <summary>The SCWQRCodeTransactionListResult class.</summary>
    public class SCWQRCodeTransactionListResult
    {
        /// <summary>Gets or sets list.</summary>
        //[PropertyMapName("list")]
        public List<SCWQRCodeTransaction> list { get; set; }

        /// <summary>Gets or sets status.</summary>
        [PropertyMapName("status")]
        public SCWStatus status { get; set; }
    }

    #endregion
}
