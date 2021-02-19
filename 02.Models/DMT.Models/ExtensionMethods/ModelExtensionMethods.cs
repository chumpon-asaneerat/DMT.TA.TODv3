#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using DMT.Models;
using Newtonsoft.Json.Serialization;
using NLib.Reflection;
using NLib.Utils;

#endregion

namespace DMT.Models.ExtensionMethods
{
    #region SCW Server <-> Local Extension Methods

    /// <summary>
    /// The SCW Extension Methods
    /// </summary>
    public static class SCWExtensionMethods
    {
        #region RevenueEntry -> SCWDeclare

        // Cash
        public static SCWDeclareCash Create(this List<MCurrency> currencies,
            decimal value, int number)
        {
            if (null == currencies) return null;
            if (number <= 0) return null;

            var match = currencies.Find((obj) => { return obj.denomValue == value; });
            if (null == match)
                return null;

            SCWDeclareCash inst = new SCWDeclareCash();
            inst.currencyDenomId = match.currencyDenomId;
            inst.currencyId = match.currencyId;
            inst.denomValue = match.denomValue;
            inst.number = number;
            inst.total = inst.denomValue * number;
            return inst;
        }
        // Coupon
        public static SCWDeclareCoupon Create(this List<MCoupon> coupons,
            decimal value, int number)
        {
            if (null == coupons) return null;
            if (number <= 0) return null;

            var match = coupons.Find((obj) => { return obj.couponValue == value; });
            if (null == match)
                return null;

            SCWDeclareCoupon inst = new SCWDeclareCoupon();
            inst.couponId = match.couponId;
            inst.couponValue = match.couponValue;
            inst.number = number;
            inst.total = inst.couponValue * number;
            return inst;
        }
        // Declare
        public static SCWDeclare ToServer(this RevenueEntry value,
            int networkId,
            List<MCurrency> currencies, List<MCoupon> coupons, List<MCardAllow> cardAllows,
            List<SCWJob> jobs,
            List<SCWEMVTransaction> emvs,
            List<SCWQRCodeTransaction> qrcodes,
            int plazaId)
        {
            if (null == value) return null;
            if (null == currencies) return null;
            if (null == coupons) return null;
            if (null == cardAllows) return null;

            var inst = new SCWDeclare();

            inst.networkId = networkId;
            inst.plazaId = plazaId;
            inst.staffId = value.UserId;

            inst.chiefId = value.SupervisorId;
            inst.chiefName = value.SupervisorNameTH;

            inst.bagNumber = value.BagNo;
            inst.safetyBeltNumber = value.BeltNo;

            inst.shiftTypeId = value.ShiftId;
            inst.declareDateTime = value.EntryDate;
            inst.operationDate = value.RevenueDate.Value.Date; // date part only.

            inst.declareById = value.UserId;
            inst.declareByName = value.TSBNameTH;

            // Lane information - Job List
            inst.attendanceDateTime = value.ShiftBegin;
            inst.departureDateTime = value.ShiftEnd;
            inst.jobList = new List<SCWJob>();
            if (null != jobs)
            {
                jobs.ForEach(job =>
                {
                    job.networkId = inst.networkId;
                    inst.jobList.Add(job);
                });
            }
            // Traffic
            inst.cashTotalAmount = value.TrafficBHTTotal;
            inst.cashRemark = value.TrafficRemark;
            inst.cashList = new List<SCWDeclareCash>();
            // helper action for traffic
            Action<List<SCWDeclareCash>, decimal, int> addToCashList = (list, bhtVal, num) =>
            {
                if (null == list) return;
                var item = currencies.Create(bhtVal, num);
                if (null == item) return;
                list.Add(item);
            };
            if (inst.cashTotalAmount > 0)
            {
                addToCashList(inst.cashList, (decimal).25, value.TrafficST25);
                addToCashList(inst.cashList, (decimal).5, value.TrafficST50);
                addToCashList(inst.cashList, 1, value.TrafficBHT1);
                addToCashList(inst.cashList, 2, value.TrafficBHT2);
                addToCashList(inst.cashList, 5, value.TrafficBHT5);
                addToCashList(inst.cashList, 10, value.TrafficBHT10);
                addToCashList(inst.cashList, 20, value.TrafficBHT20);
                addToCashList(inst.cashList, 50, value.TrafficBHT50);
                addToCashList(inst.cashList, 100, value.TrafficBHT100);
                addToCashList(inst.cashList, 500, value.TrafficBHT500);
                addToCashList(inst.cashList, 1000, value.TrafficBHT1000);
            }
            // Coupon Sold (coupon book)
            inst.couponBookTotalAmount = value.CouponSoldBHTTotal;
            inst.couponBookList = new List<SCWDeclareCouponBook>();
            if (inst.couponBookTotalAmount > 0)
            {
                if (value.CouponSoldBHT35 > 0)
                {
                    inst.couponBookList.Add(new SCWDeclareCouponBook()
                    {
                        couponBookId = 1,
                        couponBookValue = 35,
                        number = value.CouponSoldBHT35,
                        total = value.CouponSoldBHT35Total
                    });
                }
                if (value.CouponSoldBHT80 > 0)
                {
                    inst.couponBookList.Add(new SCWDeclareCouponBook()
                    {
                        couponBookId = 2,
                        couponBookValue = 80,
                        number = value.CouponSoldBHT80,
                        total = value.CouponSoldBHT80Total
                    });
                }
            }
            // Coupon Usage (coupon)
            inst.couponTotalAmount = (value.CouponUsageBHT30 * 30) +
                (value.CouponUsageBHT35 * 35) +
                (value.CouponUsageBHT60 * 60) +
                (value.CouponUsageBHT70 * 70) +
                (value.CouponUsageBHT80 * 80);
            inst.couponList = new List<SCWDeclareCoupon>();
            // helper action for coupon usage
            Action<List<SCWDeclareCoupon>, decimal, int> addToCouponList = (list, couponVal, num) =>
            {
                if (null == list) return;
                var item = coupons.Create(couponVal, num);
                if (null == item) return;
                list.Add(item);
            };
            if (inst.couponTotalAmount > 0)
            {
                addToCouponList(inst.couponList, 30, value.CouponUsageBHT30);
                addToCouponList(inst.couponList, 35, value.CouponUsageBHT35);
                addToCouponList(inst.couponList, 60, value.CouponUsageBHT60);
                addToCouponList(inst.couponList, 70, value.CouponUsageBHT70);
                addToCouponList(inst.couponList, 80, value.CouponUsageBHT80);
            }
            // Free Pass.
            inst.cardAllowTotalAmount = value.FreePassUsageClassA +
                value.FreePassUsageOther;
            inst.cardAllowList = new List<SCWDeclareFreePass>();
            if (inst.cardAllowTotalAmount > 0)
            {
                if (value.FreePassUsageClassA > 0)
                    inst.cardAllowList.Add(new SCWDeclareFreePass()
                    {
                        cardAllowId = 1,
                        number = value.FreePassUsageClassA
                    });
                if (value.FreePassUsageOther > 0)
                    inst.cardAllowList.Add(new SCWDeclareFreePass()
                    {
                        cardAllowId = 2,
                        number = value.FreePassUsageOther
                    });
            }
            // Other
            inst.otherTotalAmount = value.OtherBHTTotal;
            inst.otherRemark = value.OtherRemark;
            // QR Code
            inst.qrcodeTotalAmount = 0; // Amount in BHT
            inst.qrcodeList = new List<SCWDeclareQRCode>();
            if (null != qrcodes && qrcodes.Count > 0)
            {
                qrcodes.ForEach(item =>
                {
                    if (item.trxDateTime.HasValue && item.amount.HasValue)
                    {
                        inst.qrcodeList.Add(new SCWDeclareQRCode()
                        {
                            trxDate = item.trxDateTime.Value,
                            approvalCode = item.approvCode,
                            amount = item.amount.Value
                        });
                        inst.qrcodeTotalAmount += item.amount.Value;
                    }
                });
            }
            // EMV
            inst.emvTotalAmount = 0; // Amount in BHT
            inst.emvList = new List<SCWDeclareEMV>();
            if (null != emvs && emvs.Count > 0)
            {
                emvs.ForEach(item =>
                {
                    if (item.trxDateTime.HasValue && item.amount.HasValue)
                    {
                        inst.emvList.Add(new SCWDeclareEMV()
                        {
                            trxDate = item.trxDateTime.Value,
                            approvalCode = item.approvCode,
                            amount = item.amount.Value
                        });
                        inst.emvTotalAmount += item.amount.Value;
                    }
                });
            }

            return inst;
        }
    }

    #endregion

    #endregion

    #region TAxTOD Server <-> Local Extension Methods

    /// <summary>
    /// The TAxTOD Extension Methods
    /// </summary>
    public static class TAxTODExtensionMethods
    {
        #region Coupons

        /// <summary>
        /// Convert to Local Model.
        /// </summary>
        /// <param name="value">The TAServerCouponTransaction instance.</param>
        /// <returns>Returns TSBCouponTransaction instance.</returns>
        public static TSBCouponTransaction ToLocal(this TAServerCouponTransaction value)
        {
            if (null == value) return null;
            var inst = new TSBCouponTransaction();

            inst.TransactionDate = value.TransactionDate.Value();
            inst.TransactionType = (TSBCouponTransactionTypes)value.CouponStatus.Value();
            inst.CouponId = value.SerialNo;
            // Server Fields
            inst.CouponPK = value.CouponPK;
            inst.SapChooseFlag = value.SapChooseFlag;
            inst.SapChooseDate = value.SapChooseDate;
            inst.SAPSysSerial = value.SAPSysSerial;
            inst.SAPWhsCode = value.SAPWhsCode;
            inst.TollWayId = value.TollWayId;
            inst.SAPItemName = value.SAPItemName;
            inst.sendtaflag = value.sendtaflag;

            inst.CouponType = (CouponType)value.CouponType.Value();
            inst.FinishFlag = (TSBCouponFinishedFlags)value.FinishFlag.Value();
            inst.Price = value.Price.Value();

            inst.LaneId = value.LaneId;

            inst.SoldBy = value.SoldBy;
            var soldUsr = (!string.IsNullOrWhiteSpace(value.SoldBy)) ?
                User.GetByUserId(value.SoldBy).Value() : null;
            if (null != soldUsr)
            {
                inst.SoldByFullNameEN = soldUsr.FullNameEN;
                inst.SoldByFullNameTH = soldUsr.FullNameTH;
            }
            inst.SoldDate = value.SoldDate;
            inst.TSBId = value.TSBId;
            if (inst.TransactionType == TSBCouponTransactionTypes.Stock)
            {
                inst.UserId = string.Empty;
                inst.FullNameEN = string.Empty;
                inst.FullNameTH = string.Empty;
            }
            else
            {
                inst.UserId = value.UserId;
                var user = (!string.IsNullOrWhiteSpace(value.UserId)) ?
                    User.GetByUserId(value.UserId).Value() : null;
                if (null != user)
                {
                    inst.FullNameEN = user.FullNameEN;
                    inst.FullNameTH = user.FullNameTH;
                }
            }
            inst.UserReceiveDate = value.UserReceiveDate;

            return inst;
        }

        /// <summary>
        /// Convert to Local Model list.
        /// </summary>
        /// <param name="values">The list of TAServerCouponTransaction.</param>
        /// <returns>Returns List of TSBCouponTransaction</returns>
        public static List<TSBCouponTransaction> ToLocals(this List<TAServerCouponTransaction> values)
        {
            var rets = new List<TSBCouponTransaction>();
            if (null != values)
            {
                values.ForEach(inst =>
                {
                    rets.Add(inst.ToLocal());
                });
            }
            return rets;

        }

        /// <summary>
        /// Convert to Server Model.
        /// </summary>
        /// <param name="value">The TSBCouponTransaction instance.</param>
        /// <returns>Returns TAServerCouponTransaction instance.</returns>
        public static TAServerCouponTransaction ToServer(this TSBCouponTransaction value)
        {
            if (null == value) return null;

            var inst = new TAServerCouponTransaction();

            inst.TransactionDate = value.TransactionDate;
            inst.CouponStatus = (int)value.TransactionType;
            inst.SerialNo = value.CouponId;
            // Server Fields
            inst.CouponPK = value.CouponPK;
            inst.SapChooseFlag = value.SapChooseFlag;
            inst.SapChooseDate = value.SapChooseDate;
            inst.SAPSysSerial = value.SAPSysSerial;
            inst.SAPWhsCode = value.SAPWhsCode;
            inst.TollWayId = value.TollWayId;
            inst.SAPItemName = value.SAPItemName;
            //inst.sendtaflag = value.sendtaflag;

            inst.LaneId = (!string.IsNullOrEmpty(value.LaneId)) ? value.LaneId : null;

            inst.CouponType = (int)value.CouponType;
            inst.FinishFlag = (int)value.FinishFlag;
            inst.Price = value.Price;
            inst.SoldBy = (!string.IsNullOrEmpty(value.SoldBy)) ? value.SoldBy : null;
            inst.SoldDate = value.SoldDate;
            inst.TSBId = value.TSBId;
            if (value.TransactionType == TSBCouponTransactionTypes.Stock)
            {
                inst.UserId = null;
            }
            else
            {
                inst.UserId = (!string.IsNullOrEmpty(value.UserId)) ? value.UserId : null;
            }
            inst.UserReceiveDate = value.UserReceiveDate;

            return inst;
        }

        /// <summary>
        /// Convert to Server Model list.
        /// </summary>
        /// <param name="values">The list of TSBCouponTransaction.</param>
        /// <returns>Returns list of TAServerCouponTransaction.</returns>
        public static List<TAServerCouponTransaction> ToServers(this List<TSBCouponTransaction> values)
        {
            var rets = new List<TAServerCouponTransaction>();
            if (null != values)
            {
                values.ForEach(inst =>
                {
                    rets.Add(inst.ToServer());
                });
            }
            return rets;

        }

        #endregion
    }

    #endregion
}
