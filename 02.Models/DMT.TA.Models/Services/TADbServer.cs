//#define USE_PROGRAM_DATA

#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.CompilerServices;
using System.Reflection;

// SQLite
using SQLite;
//using SQLiteNetExtensions.Attributes;
//using SQLiteNetExtensions.Extensions;

using NLib;
using NLib.IO;

using DMT.Models;
using DMT.Views;

#endregion

namespace DMT.Services
{
    #region TALocalDbServer

    /// <summary>
    /// Local Database Server.
    /// </summary>
    public class TALocalDbServer
    {
        #region Singelton

        private static TALocalDbServer _instance = null;
        /// <summary>
        /// Singelton Access.
        /// </summary>
        public static TALocalDbServer Instance
        {
            get
            {
                if (null == _instance)
                {
                    lock (typeof(TALocalDbServer))
                    {
                        _instance = new TALocalDbServer();
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        private TALocalDbServer() : base()
        {
            this.FileName = "TA.db";
        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~TALocalDbServer()
        {
            Shutdown();
        }

        #endregion

        #region Private Properties

        /// <summary>
        /// Gets local json folder path name.
        /// </summary>
        private static string LocalFolder
        {
            get
            {
#if !USE_PROGRAM_DATA
                string localFilder = Folders.Combine(
                    Folders.Assemblies.CurrentExecutingAssembly, "data");
#else
                // Stored in C:\ProgarmData\DMT\Data\ folder
                string localFilder = ApplicationManager.Instance.Environments.Company.Data.FullName;
#endif
                if (!Folders.Exists(localFilder))
                {
                    Folders.Create(localFilder);
                }
                return localFilder;
            }
        }

        #endregion

        #region Private Methods

        private void InitTables()
        {
            Db.CreateTable<ViewHistory>();
            Db.CreateTable<UniqueCode>();

            Db.CreateTable<MCurrency>();
            Db.CreateTable<MCoupon>();
            Db.CreateTable<MCouponBook>();
            Db.CreateTable<MCardAllow>();

            Db.CreateTable<TSB>();
            Db.CreateTable<PlazaGroup>();
            Db.CreateTable<Plaza>();
            Db.CreateTable<Lane>();

            Db.CreateTable<Shift>();

            Db.CreateTable<Role>();
            Db.CreateTable<User>();
            Db.CreateTable<UserAccess>();

            Db.CreateTable<TSBShift>();

            Db.CreateTable<RevenueEntry>(); // Collect all revenue entry from various TOD client (by plaza id).

            Db.CreateTable<TSBCreditTransaction>();

            Db.CreateTable<UserCreditBalance>();
            Db.CreateTable<UserCreditTransaction>();

            Db.CreateTable<TSBExchangeGroup>();
            Db.CreateTable<TSBExchangeTransaction>();

            Db.CreateTable<TSBCouponTransaction>();

            ////Db.CreateTable<UserCouponBalance>();
            //Db.CreateTable<UserCouponTransaction>();
        }

        private void InitDefaults()
        {
            InitMCurrency();
            InitMCoupon();
            InitMCouponBook();
            InitMCardAllow();
            InitTSBAndPlazaAndLanes();
            InitShifts();
            InitRoleAndUsers();
        }

        private void InitMCurrency()
        {
            if (null == Db) return;

            if (Db.Table<MCurrency>().Count() > 0) return; // already exists.

            MCurrency item;
            item = new MCurrency()
            {
                currencyDenomId = 1,
                abbreviation = "Satang25",
                description = "25 Satang",
                denomValue = (decimal)0.25,
                currencyId = 1,
                denomTypeId = 2 // coin
            };
            if (!MCurrency.Exists(item)) MCurrency.Save(item);
            item = new MCurrency()
            {
                currencyDenomId = 2,
                abbreviation = "Satang50",
                description = "50 Satang",
                denomValue = (decimal)0.5,
                currencyId = 1,
                denomTypeId = 2 // coin
            };
            if (!MCurrency.Exists(item)) MCurrency.Save(item);
            item = new MCurrency()
            {
                currencyDenomId = 3,
                abbreviation = "Baht1",
                description = "1 Baht",
                denomValue = 1,
                currencyId = 1,
                denomTypeId = 2 // coin
            };
            if (!MCurrency.Exists(item)) MCurrency.Save(item);
            item = new MCurrency()
            {
                currencyDenomId = 4,
                abbreviation = "Baht2",
                description = "2 Baht",
                denomValue = 2,
                currencyId = 1,
                denomTypeId = 2 // coin
            };
            if (!MCurrency.Exists(item)) MCurrency.Save(item);
            item = new MCurrency()
            {
                currencyDenomId = 5,
                abbreviation = "Baht5",
                description = "5 Baht",
                denomValue = 5,
                currencyId = 1,
                denomTypeId = 2 // coin
            };
            if (!MCurrency.Exists(item)) MCurrency.Save(item);
            item = new MCurrency()
            {
                currencyDenomId = 6,
                abbreviation = "CBaht10",
                description = "10 Baht",
                denomValue = 10,
                currencyId = 1,
                denomTypeId = 2 // coin
            };
            if (!MCurrency.Exists(item)) MCurrency.Save(item);
            item = new MCurrency()
            {
                currencyDenomId = 7,
                abbreviation = "NBaht10",
                description = "10 Baht",
                denomValue = 10,
                currencyId = 1,
                denomTypeId = 1 // Note
            };
            if (!MCurrency.Exists(item)) MCurrency.Save(item);
            item = new MCurrency()
            {
                currencyDenomId = 8,
                abbreviation = "NBaht20",
                description = "20 Baht",
                denomValue = 20,
                currencyId = 1,
                denomTypeId = 1 // Note
            };
            if (!MCurrency.Exists(item)) MCurrency.Save(item);
            item = new MCurrency()
            {
                currencyDenomId = 9,
                abbreviation = "NBaht50",
                description = "50 Baht",
                denomValue = 50,
                currencyId = 1,
                denomTypeId = 1 // Note
            };
            if (!MCurrency.Exists(item)) MCurrency.Save(item);
            item = new MCurrency()
            {
                currencyDenomId = 10,
                abbreviation = "NBaht100",
                description = "100 Baht",
                denomValue = 100,
                currencyId = 1,
                denomTypeId = 1 // Note
            };
            if (!MCurrency.Exists(item)) MCurrency.Save(item);
            item = new MCurrency()
            {
                currencyDenomId = 11,
                abbreviation = "NBaht500",
                description = "500 Baht",
                denomValue = 500,
                currencyId = 1,
                denomTypeId = 1 // Note
            };
            if (!MCurrency.Exists(item)) MCurrency.Save(item);
            item = new MCurrency()
            {
                currencyDenomId = 12,
                abbreviation = "NBaht1000",
                description = "1000 Baht",
                denomValue = 1000,
                currencyId = 1,
                denomTypeId = 1 // Note
            };
            if (!MCurrency.Exists(item)) MCurrency.Save(item);
        }

        private void InitMCoupon()
        {
            if (null == Db) return;

            if (Db.Table<MCoupon>().Count() > 0) return; // already exists.

            MCoupon item;
            item = new MCoupon()
            {
                couponId = 1,
                couponValue = 30,
                abbreviation = "30",
                description = "30 บาท"
            };
            if (!MCoupon.Exists(item)) MCoupon.Save(item);
            item = new MCoupon()
            {
                couponId = 2,
                couponValue = 35,
                abbreviation = "35",
                description = "35 บาท"
            };
            if (!MCoupon.Exists(item)) MCoupon.Save(item);
            item = new MCoupon()
            {
                couponId = 3,
                couponValue = 70,
                abbreviation = "70",
                description = "70 บาท"
            };
            if (!MCoupon.Exists(item)) MCoupon.Save(item);
            item = new MCoupon()
            {
                couponId = 4,
                couponValue = 80,
                abbreviation = "80",
                description = "80 บาท"
            };
            if (!MCoupon.Exists(item)) MCoupon.Save(item);
        }

        private void InitMCouponBook()
        {
            if (null == Db) return;

            if (Db.Table<MCouponBook>().Count() > 0) return; // already exists.
            MCouponBook item;
            item = new MCouponBook()
            {
                couponBookId = 1,
                couponBookValue = 665,
                abbreviation = "35",
                description = "35 บาท"
            };
            if (!MCouponBook.Exists(item)) MCouponBook.Save(item);
            item = new MCouponBook()
            {
                couponBookId = 2,
                couponBookValue = 1520,
                abbreviation = "80",
                description = "80 บาท"
            };
            if (!MCouponBook.Exists(item)) MCouponBook.Save(item);
        }

        private void InitMCardAllow()
        {
            if (null == Db) return;

            if (Db.Table<MCardAllow>().Count() > 0) return; // already exists.

            MCardAllow item;
            item = new MCardAllow()
            {
                cardAllowId = 1,
                abbreviation = "Card DMT P1",
                description = "บัตร DMT (ป 1)"
            };
            if (!MCardAllow.Exists(item)) MCardAllow.Save(item);
            item = new MCardAllow()
            {
                cardAllowId = 2,
                abbreviation = "Card DMT P2",
                description = "บัตร DMT (ป 2)"
            };
            if (!MCardAllow.Exists(item)) MCardAllow.Save(item);
        }

        private void InitTSBAndPlazaAndLanes()
        {
            if (null == Db) return;

            if (Db.Table<TSB>().Count() > 0) return; // already exists.

            TSB item;
            PlazaGroup plazaGroup;
            Plaza plaza;
            Lane lane;

            #region DIN DAENG

            #region TSB

            item = new TSB();
            item.NetworkId = "31";
            item.TSBId = "311";
            item.TSBNameEN = "DIN DAENG";
            item.TSBNameTH = "ดินแดง";
            item.Active = false;
            if (!TSB.Exists(item)) TSB.Save(item);
            // init default credit value.
            InitTSBCreditInitializeTransaction(item);

            #endregion

            #region PlazaGroup DIN DAENG

            plazaGroup = new PlazaGroup()
            {
                PlazaGroupId = "DD",
                PlazaGroupNameEN = "DIN DAENG",
                PlazaGroupNameTH = "ดินแดง",
                Direction = "?",
                TSBId = item.TSBId
            };
            if (!PlazaGroup.Exists(plazaGroup)) PlazaGroup.Save(plazaGroup);

            #region Plaza DIN DAENG 1

            plaza = new Plaza()
            {
                PlazaId = "3101",
                SCWPlazaId = 1,
                PlazaNameEN = "DIN DAENG 1",
                PlazaNameTH = "ดินแดง 1",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId
            };
            if (!Plaza.Exists(plaza)) Plaza.Save(plaza);

            #region Lanes

            lane = new Lane()
            {
                LaneNo = 11,
                LaneId = "DD11",
                LaneType = "?",
                LaneAbbr = "DD11",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId,
                PlazaId = plaza.PlazaId
            };
            if (!Lane.Exists(lane)) Lane.Save(lane);
            lane = new Lane()
            {
                LaneNo = 12,
                LaneId = "DD12",
                LaneType = "?",
                LaneAbbr = "DD12",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId,
                PlazaId = plaza.PlazaId
            };
            if (!Lane.Exists(lane)) Lane.Save(lane);
            lane = new Lane()
            {
                LaneNo = 13,
                LaneId = "DD13",
                LaneType = "?",
                LaneAbbr = "DD13",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId,
                PlazaId = plaza.PlazaId
            };
            if (!Lane.Exists(lane)) Lane.Save(lane);
            lane = new Lane()
            {
                LaneNo = 14,
                LaneId = "DD14",
                LaneType = "?",
                LaneAbbr = "DD14",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId,
                PlazaId = plaza.PlazaId
            };
            if (!Lane.Exists(lane)) Lane.Save(lane);
            lane = new Lane()
            {
                LaneNo = 15,
                LaneId = "DD15",
                LaneType = "?",
                LaneAbbr = "DD15",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId,
                PlazaId = plaza.PlazaId
            };
            if (!Lane.Exists(lane)) Lane.Save(lane);
            lane = new Lane()
            {
                LaneNo = 16,
                LaneId = "DD16",
                LaneType = "?",
                LaneAbbr = "DD16",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId,
                PlazaId = plaza.PlazaId
            };
            if (!Lane.Exists(lane)) Lane.Save(lane);

            #endregion

            #endregion

            #region Plaza DIN DAENG 2

            plaza = new Plaza()
            {
                PlazaId = "3102",
                SCWPlazaId = 2,
                PlazaNameEN = "DIN DAENG 2",
                PlazaNameTH = "ดินแดง 2",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId
            };
            if (!Plaza.Exists(plaza)) Plaza.Save(plaza);

            #region Lanes

            lane = new Lane()
            {
                LaneNo = 1,
                LaneId = "DD01",
                LaneType = "MTC",
                LaneAbbr = "DD01",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId,
                PlazaId = plaza.PlazaId
            };
            if (!Lane.Exists(lane)) Lane.Save(lane);
            lane = new Lane()
            {
                LaneNo = 2,
                LaneId = "DD02",
                LaneType = "MTC",
                LaneAbbr = "DD02",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId,
                PlazaId = plaza.PlazaId
            };
            if (!Lane.Exists(lane)) Lane.Save(lane);
            lane = new Lane()
            {
                LaneNo = 3,
                LaneId = "DD03",
                LaneType = "A/M",
                LaneAbbr = "DD03",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId,
                PlazaId = plaza.PlazaId
            };
            if (!Lane.Exists(lane)) Lane.Save(lane);
            lane = new Lane()
            {
                LaneNo = 4,
                LaneId = "DD04",
                LaneType = "ETC",
                LaneAbbr = "DD04",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId,
                PlazaId = plaza.PlazaId
            };
            if (!Lane.Exists(lane)) Lane.Save(lane);
            lane = new Lane()
            {
                LaneNo = 5,
                LaneId = "DD05",
                LaneType = "MTC",
                LaneAbbr = "DD05",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId,
                PlazaId = plaza.PlazaId
            };
            if (!Lane.Exists(lane)) Lane.Save(lane);
            lane = new Lane()
            {
                LaneNo = 6,
                LaneId = "DD06",
                LaneType = "MTC",
                LaneAbbr = "DD06",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId,
                PlazaId = plaza.PlazaId
            };
            if (!Lane.Exists(lane)) Lane.Save(lane);

            #endregion

            #endregion

            #endregion

            #endregion

            #region SUTHISARN

            #region TSB

            item = new TSB();
            item.NetworkId = "31";
            item.TSBId = "312";
            item.TSBNameEN = "SUTHISARN";
            item.TSBNameTH = "สุทธิสาร";
            item.Active = false;
            if (!TSB.Exists(item)) TSB.Save(item);
            // init default credit value.
            InitTSBCreditInitializeTransaction(item);

            #endregion

            #region PlazaGroup SUTHISARN

            plazaGroup = new PlazaGroup()
            {
                PlazaGroupId = "SS",
                PlazaGroupNameEN = "SUTHISARN",
                PlazaGroupNameTH = "สุทธิสาร",
                Direction = "?",
                TSBId = item.TSBId
            };
            if (!PlazaGroup.Exists(plazaGroup)) PlazaGroup.Save(plazaGroup);

            #region Plaza SUTHISARN

            plaza = new Plaza()
            {
                PlazaId = "3103",
                SCWPlazaId = 3,
                PlazaNameEN = "SUTHISARN",
                PlazaNameTH = "สุทธิสาร",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId
            };
            if (!Plaza.Exists(plaza)) Plaza.Save(plaza);

            #region Lanes

            lane = new Lane()
            {
                LaneNo = 1,
                LaneId = "SS01",
                LaneType = "?",
                LaneAbbr = "SS01",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId,
                PlazaId = plaza.PlazaId
            };
            if (!Lane.Exists(lane)) Lane.Save(lane);
            lane = new Lane()
            {
                LaneNo = 2,
                LaneId = "SS02",
                LaneType = "?",
                LaneAbbr = "SS02",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId,
                PlazaId = plaza.PlazaId
            };
            if (!Lane.Exists(lane)) Lane.Save(lane);
            lane = new Lane()
            {
                LaneNo = 3,
                LaneId = "SS03",
                LaneType = "?",
                LaneAbbr = "SS03",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId,
                PlazaId = plaza.PlazaId
            };
            if (!Lane.Exists(lane)) Lane.Save(lane);


            #endregion

            #endregion

            #endregion

            #endregion

            #region LAD PRAO

            #region TSB

            item = new TSB();
            item.NetworkId = "31";
            item.TSBId = "313";
            item.TSBNameEN = "LAD PRAO";
            item.TSBNameTH = "ลาดพร้าว";
            item.Active = false;
            if (!TSB.Exists(item)) TSB.Save(item);
            // init default credit value.
            InitTSBCreditInitializeTransaction(item);

            #endregion

            #region PlazaGroup LAD PRAO INBOUND

            plazaGroup = new PlazaGroup()
            {
                PlazaGroupId = "LPS",
                PlazaGroupNameEN = "LAD PRAO INBOUND",
                PlazaGroupNameTH = "ลาดพร้าว ขาเข้า",
                Direction = "IN",
                TSBId = item.TSBId
            };
            if (!PlazaGroup.Exists(plazaGroup)) PlazaGroup.Save(plazaGroup);

            #region Plaza LAD PRAO INBOUND

            plaza = new Plaza()
            {
                PlazaId = "3104",
                SCWPlazaId = 4,
                PlazaNameEN = "LAD PRAO INBOUND",
                PlazaNameTH = "ลาดพร้าว ขาเข้า",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId
            };
            if (!Plaza.Exists(plaza)) Plaza.Save(plaza);

            #region Lanes

            lane = new Lane()
            {
                LaneNo = 21,
                LaneId = "LP21",
                LaneType = "?",
                LaneAbbr = "LP21",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId,
                PlazaId = plaza.PlazaId
            };
            if (!Lane.Exists(lane)) Lane.Save(lane);

            lane = new Lane()
            {
                LaneNo = 22,
                LaneId = "LP22",
                LaneType = "?",
                LaneAbbr = "LP22",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId,
                PlazaId = plaza.PlazaId
            };
            if (!Lane.Exists(lane)) Lane.Save(lane);

            lane = new Lane()
            {
                LaneNo = 23,
                LaneId = "LP23",
                LaneType = "?",
                LaneAbbr = "LP23",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId,
                PlazaId = plaza.PlazaId
            };
            if (!Lane.Exists(lane)) Lane.Save(lane);

            #endregion

            #endregion

            #endregion

            #region PlazaGroup LAD PRAO OUTBOUND

            plazaGroup = new PlazaGroup()
            {
                PlazaGroupId = "LPN",
                PlazaGroupNameEN = "LAD PRAO OUTBOUND",
                PlazaGroupNameTH = "ลาดพร้าว ขาออก",
                Direction = "OUT",
                TSBId = item.TSBId
            };
            if (!PlazaGroup.Exists(plazaGroup)) PlazaGroup.Save(plazaGroup);

            #region Plaza LAD PRAO OUTBOUND

            plaza = new Plaza()
            {
                PlazaId = "3105",
                SCWPlazaId = 5,
                PlazaNameEN = "LAD PRAO OUTBOUND",
                PlazaNameTH = "ลาดพร้าว ขาออก",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId
            };
            if (!Plaza.Exists(plaza)) Plaza.Save(plaza);

            #region Lanes

            lane = new Lane()
            {
                LaneNo = 1,
                LaneId = "LP01",
                LaneType = "?",
                LaneAbbr = "LP01",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId,
                PlazaId = plaza.PlazaId
            };
            if (!Lane.Exists(lane)) Lane.Save(lane);
            lane = new Lane()
            {
                LaneNo = 2,
                LaneId = "LP02",
                LaneType = "?",
                LaneAbbr = "LP02",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId,
                PlazaId = plaza.PlazaId
            };
            if (!Lane.Exists(lane)) Lane.Save(lane);
            lane = new Lane()
            {
                LaneNo = 3,
                LaneId = "LP03",
                LaneType = "?",
                LaneAbbr = "LP03",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId,
                PlazaId = plaza.PlazaId
            };
            if (!Lane.Exists(lane)) Lane.Save(lane);
            lane = new Lane()
            {
                LaneNo = 4,
                LaneId = "LP04",
                LaneType = "?",
                LaneAbbr = "LP04",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId,
                PlazaId = plaza.PlazaId
            };
            if (!Lane.Exists(lane)) Lane.Save(lane);

            #endregion

            #endregion

            #endregion

            #endregion

            #region RATCHADA PHISEK

            #region TSB

            item = new TSB();
            item.NetworkId = "31";
            item.TSBId = "314";
            item.TSBNameEN = "RATCHADA PHISEK";
            item.TSBNameTH = "รัชดาภิเษก";
            item.Active = false;
            if (!TSB.Exists(item)) TSB.Save(item);
            // init default credit value.
            InitTSBCreditInitializeTransaction(item);

            #endregion

            #region PlazaGroup RATCHADA PHISEK

            plazaGroup = new PlazaGroup()
            {
                PlazaGroupId = "RP",
                PlazaGroupNameEN = "RATCHADA PHISEK",
                PlazaGroupNameTH = "รัชดาภิเษก",
                Direction = "?",
                TSBId = item.TSBId
            };
            if (!PlazaGroup.Exists(plazaGroup)) PlazaGroup.Save(plazaGroup);

            #region Plaza RATCHADA PHISEK 1

            plaza = new Plaza()
            {
                PlazaId = "3106",
                SCWPlazaId = 6,
                PlazaNameEN = "RATCHADA PHISEK 1",
                PlazaNameTH = "รัชดาภิเษก 1",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId
            };
            if (!Plaza.Exists(plaza)) Plaza.Save(plaza);

            #region Lanes

            lane = new Lane()
            {
                LaneNo = 1,
                LaneId = "RP01",
                LaneType = "?",
                LaneAbbr = "RP01",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId,
                PlazaId = plaza.PlazaId
            };
            if (!Lane.Exists(lane)) Lane.Save(lane);
            lane = new Lane()
            {
                LaneNo = 2,
                LaneId = "RP02",
                LaneType = "?",
                LaneAbbr = "RP02",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId,
                PlazaId = plaza.PlazaId
            };
            if (!Lane.Exists(lane)) Lane.Save(lane);
            lane = new Lane()
            {
                LaneNo = 3,
                LaneId = "RP03",
                LaneType = "?",
                LaneAbbr = "RP03",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId,
                PlazaId = plaza.PlazaId
            };
            if (!Lane.Exists(lane)) Lane.Save(lane);

            #endregion

            #endregion

            #region Plaza RATCHADA PHISEK 2

            plaza = new Plaza()
            {
                PlazaId = "3107",
                SCWPlazaId = 7,
                PlazaNameEN = "RATCHADA PHISEK 2",
                PlazaNameTH = "รัชดาภิเษก 2",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId
            };
            if (!Plaza.Exists(plaza)) Plaza.Save(plaza);

            #region Lanes

            lane = new Lane()
            {
                LaneNo = 11,
                LaneId = "RP11",
                LaneType = "?",
                LaneAbbr = "RP11",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId,
                PlazaId = plaza.PlazaId
            };
            if (!Lane.Exists(lane)) Lane.Save(lane);
            lane = new Lane()
            {
                LaneNo = 12,
                LaneId = "RP12",
                LaneType = "?",
                LaneAbbr = "RP12",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId,
                PlazaId = plaza.PlazaId
            };
            if (!Lane.Exists(lane)) Lane.Save(lane);
            lane = new Lane()
            {
                LaneNo = 13,
                LaneId = "RP13",
                LaneType = "?",
                LaneAbbr = "RP13",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId,
                PlazaId = plaza.PlazaId
            };
            if (!Lane.Exists(lane)) Lane.Save(lane);

            #endregion

            #endregion

            #endregion

            #endregion

            #region BANGKHEN

            #region TSB

            item = new TSB();
            item.NetworkId = "31";
            item.TSBId = "315";
            item.TSBNameEN = "BANGKHEN";
            item.TSBNameTH = "บางเขน";
            item.Active = false;
            if (!TSB.Exists(item)) TSB.Save(item);
            // init default credit value.
            InitTSBCreditInitializeTransaction(item);

            #endregion

            #region PlazaGroup BANGKHEN

            plazaGroup = new PlazaGroup()
            {
                PlazaGroupId = "BK",
                PlazaGroupNameEN = "BANGKHEN",
                PlazaGroupNameTH = "บางเขน",
                Direction = "?",
                TSBId = item.TSBId
            };
            if (!PlazaGroup.Exists(plazaGroup)) PlazaGroup.Save(plazaGroup);

            #region Plaza BANGKHEN

            plaza = new Plaza()
            {
                PlazaId = "3108",
                SCWPlazaId = 8,
                PlazaNameEN = "BANGKHEN",
                PlazaNameTH = "บางเขน",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId
            };
            if (!Plaza.Exists(plaza)) Plaza.Save(plaza);

            #endregion

            #region Lanes

            lane = new Lane()
            {
                LaneNo = 1,
                LaneId = "BK01",
                LaneType = "?",
                LaneAbbr = "BK01",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId,
                PlazaId = plaza.PlazaId
            };
            if (!Lane.Exists(lane)) Lane.Save(lane);
            lane = new Lane()
            {
                LaneNo = 2,
                LaneId = "BK02",
                LaneType = "?",
                LaneAbbr = "BK02",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId,
                PlazaId = plaza.PlazaId
            };
            if (!Lane.Exists(lane)) Lane.Save(lane);

            #endregion

            #endregion

            #endregion

            #region CHANGEWATTANA

            #region TSB

            item = new TSB();
            item.NetworkId = "31";
            item.TSBId = "316";
            item.TSBNameEN = "CHANGEWATTANA";
            item.TSBNameTH = "แจ้งวัฒนะ";
            item.Active = false;
            if (!TSB.Exists(item)) TSB.Save(item);
            // init default credit value.
            InitTSBCreditInitializeTransaction(item);

            #endregion

            #region PlazaGroup CHANGEWATTANA

            plazaGroup = new PlazaGroup()
            {
                PlazaGroupId = "CW",
                PlazaGroupNameEN = "CHANGEWATTANA",
                PlazaGroupNameTH = "แจ้งวัฒนะ",
                Direction = "?",
                TSBId = item.TSBId
            };
            if (!PlazaGroup.Exists(plazaGroup)) PlazaGroup.Save(plazaGroup);

            #region Plaza CHANGEWATTANA 1

            plaza = new Plaza()
            {
                PlazaId = "3109",
                SCWPlazaId = 9,
                PlazaNameEN = "CHANGEWATTANA 1",
                PlazaNameTH = "แจ้งวัฒนะ 1",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId
            };
            if (!Plaza.Exists(plaza)) Plaza.Save(plaza);

            #region Lanes

            lane = new Lane()
            {
                LaneNo = 21,
                LaneId = "CW21",
                LaneType = "?",
                LaneAbbr = "CW21",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId,
                PlazaId = plaza.PlazaId
            };
            if (!Lane.Exists(lane)) Lane.Save(lane);
            lane = new Lane()
            {
                LaneNo = 22,
                LaneId = "CW22",
                LaneType = "?",
                LaneAbbr = "CW22",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId,
                PlazaId = plaza.PlazaId
            };
            if (!Lane.Exists(lane)) Lane.Save(lane);
            lane = new Lane()
            {
                LaneNo = 23,
                LaneId = "CW23",
                LaneType = "?",
                LaneAbbr = "CW23",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId,
                PlazaId = plaza.PlazaId
            };
            if (!Lane.Exists(lane)) Lane.Save(lane);

            #endregion

            #endregion

            #region Plaza CHANGEWATTANA 2

            plaza = new Plaza()
            {
                PlazaId = "3110",
                SCWPlazaId = 10,
                PlazaNameEN = "CHANGEWATTANA 2",
                PlazaNameTH = "แจ้งวัฒนะ 2",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId
            };
            if (!Plaza.Exists(plaza)) Plaza.Save(plaza);

            #region Lanes

            lane = new Lane()
            {
                LaneNo = 31,
                LaneId = "CW31",
                LaneType = "?",
                LaneAbbr = "CW31",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId,
                PlazaId = plaza.PlazaId
            };
            if (!Lane.Exists(lane)) Lane.Save(lane);
            lane = new Lane()
            {
                LaneNo = 32,
                LaneId = "CW32",
                LaneType = "?",
                LaneAbbr = "CW32",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId,
                PlazaId = plaza.PlazaId
            };
            if (!Lane.Exists(lane)) Lane.Save(lane);

            #endregion

            #endregion

            #endregion

            #endregion

            #region LAKSI

            #region TSB

            item = new TSB();
            item.NetworkId = "31";
            item.TSBId = "317";
            item.TSBNameEN = "LAKSI";
            item.TSBNameTH = "หลักสี่";
            item.Active = false;
            if (!TSB.Exists(item)) TSB.Save(item);
            // init default credit value.
            InitTSBCreditInitializeTransaction(item);

            #endregion

            #region PlazaGroup LAKSI INBOUND

            plazaGroup = new PlazaGroup()
            {
                PlazaGroupId = "LKS",
                PlazaGroupNameEN = "LAKSI INBOUND",
                PlazaGroupNameTH = "หลักสี่ ขาเข้า",
                Direction = "IN",
                TSBId = item.TSBId
            };
            if (!PlazaGroup.Exists(plazaGroup)) PlazaGroup.Save(plazaGroup);

            #region Plaza LAKSI INBOUND

            plaza = new Plaza()
            {
                PlazaId = "3111",
                SCWPlazaId = 11,
                PlazaNameEN = "LAKSI INBOUND",
                PlazaNameTH = "หลักสี่ ขาเข้า",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId
            };
            if (!Plaza.Exists(plaza)) Plaza.Save(plaza);

            #region Lanes

            lane = new Lane()
            {
                LaneNo = 21,
                LaneId = "LK21",
                LaneType = "?",
                LaneAbbr = "LK21",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId,
                PlazaId = plaza.PlazaId
            };
            if (!Lane.Exists(lane)) Lane.Save(lane);
            lane = new Lane()
            {
                LaneNo = 22,
                LaneId = "LK22",
                LaneType = "?",
                LaneAbbr = "LK22",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId,
                PlazaId = plaza.PlazaId
            };
            if (!Lane.Exists(lane)) Lane.Save(lane);
            lane = new Lane()
            {
                LaneNo = 23,
                LaneId = "LK23",
                LaneType = "?",
                LaneAbbr = "LK23",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId,
                PlazaId = plaza.PlazaId
            };
            if (!Lane.Exists(lane)) Lane.Save(lane);
            lane = new Lane()
            {
                LaneNo = 24,
                LaneId = "LK24",
                LaneType = "?",
                LaneAbbr = "LK24",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId,
                PlazaId = plaza.PlazaId
            };
            if (!Lane.Exists(lane)) Lane.Save(lane);

            #endregion

            #endregion

            #endregion

            #region PlazaGroup LAKSI OUTBOUND

            plazaGroup = new PlazaGroup()
            {
                PlazaGroupId = "LKN",
                PlazaGroupNameEN = "LAKSI OUTBOUND",
                PlazaGroupNameTH = "หลักสี่ ขาออก",
                Direction = "OUT",
                TSBId = item.TSBId
            };
            if (!PlazaGroup.Exists(plazaGroup)) PlazaGroup.Save(plazaGroup);

            #region Plaza LAKSI OUTBOUND

            plaza = new Plaza()
            {
                PlazaId = "3112",
                SCWPlazaId = 12,
                PlazaNameEN = "LAKSI OUTBOUND",
                PlazaNameTH = "หลักสี่ ขาออก",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId
            };
            if (!Plaza.Exists(plaza)) Plaza.Save(plaza);

            #region Lanes

            lane = new Lane()
            {
                LaneNo = 1,
                LaneId = "LK01",
                LaneType = "?",
                LaneAbbr = "LK01",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId,
                PlazaId = plaza.PlazaId
            };
            if (!Lane.Exists(lane)) Lane.Save(lane);
            lane = new Lane()
            {
                LaneNo = 2,
                LaneId = "LK02",
                LaneType = "?",
                LaneAbbr = "LK02",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId,
                PlazaId = plaza.PlazaId
            };
            if (!Lane.Exists(lane)) Lane.Save(lane);

            #endregion

            #endregion

            #endregion

            #endregion

            #region DON MUANG

            #region TSB

            item = new TSB();
            item.NetworkId = "31";
            item.TSBId = "318";
            item.TSBNameEN = "DON MUANG";
            item.TSBNameTH = "ดอนเมือง";
            item.Active = false;
            if (!TSB.Exists(item)) TSB.Save(item);
            // init default credit value.
            InitTSBCreditInitializeTransaction(item);

            #endregion

            #region PlazaGroup DON MUANG

            plazaGroup = new PlazaGroup()
            {
                PlazaGroupId = "DM",
                PlazaGroupNameEN = "DON MUANG",
                PlazaGroupNameTH = "ดอนเมือง",
                Direction = "?",
                TSBId = item.TSBId
            };
            if (!PlazaGroup.Exists(plazaGroup)) PlazaGroup.Save(plazaGroup);

            #region Plaza DON MUANG 1

            plaza = new Plaza()
            {
                PlazaId = "3113",
                SCWPlazaId = 13,
                PlazaNameEN = "DON MUANG 1",
                PlazaNameTH = "ดอนเมือง 1",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId
            };
            if (!Plaza.Exists(plaza)) Plaza.Save(plaza);

            #region Lanes

            lane = new Lane()
            {
                LaneNo = 31,
                LaneId = "DM31",
                LaneType = "?",
                LaneAbbr = "DM31",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId,
                PlazaId = plaza.PlazaId
            };
            if (!Lane.Exists(lane)) Lane.Save(lane);
            lane = new Lane()
            {
                LaneNo = 32,
                LaneId = "DM32",
                LaneType = "?",
                LaneAbbr = "DM32",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId,
                PlazaId = plaza.PlazaId
            };
            if (!Lane.Exists(lane)) Lane.Save(lane);
            lane = new Lane()
            {
                LaneNo = 33,
                LaneId = "DM33",
                LaneType = "?",
                LaneAbbr = "DM33",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId,
                PlazaId = plaza.PlazaId
            };
            if (!Lane.Exists(lane)) Lane.Save(lane);
            lane = new Lane()
            {
                LaneNo = 34,
                LaneId = "DM34",
                LaneType = "?",
                LaneAbbr = "DM34",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId,
                PlazaId = plaza.PlazaId
            };
            if (!Lane.Exists(lane)) Lane.Save(lane);
            lane = new Lane()
            {
                LaneNo = 35,
                LaneId = "DM35",
                LaneType = "?",
                LaneAbbr = "DM35",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId,
                PlazaId = plaza.PlazaId
            };
            if (!Lane.Exists(lane)) Lane.Save(lane);

            #endregion

            #endregion

            #region Plaza DON MUANG 2

            plaza = new Plaza()
            {
                PlazaId = "3114",
                SCWPlazaId = 14,
                PlazaNameEN = "DON MUANG 2",
                PlazaNameTH = "ดอนเมือง 2",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId
            };
            if (!Plaza.Exists(plaza)) Plaza.Save(plaza);

            #region Lanes

            lane = new Lane()
            {
                LaneNo = 21,
                LaneId = "DM21",
                LaneType = "?",
                LaneAbbr = "DM21",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId,
                PlazaId = plaza.PlazaId
            };
            if (!Lane.Exists(lane)) Lane.Save(lane);
            lane = new Lane()
            {
                LaneNo = 22,
                LaneId = "DM22",
                LaneType = "?",
                LaneAbbr = "DM22",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId,
                PlazaId = plaza.PlazaId
            };
            if (!Lane.Exists(lane)) Lane.Save(lane);
            lane = new Lane()
            {
                LaneNo = 23,
                LaneId = "DM23",
                LaneType = "?",
                LaneAbbr = "DM23",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId,
                PlazaId = plaza.PlazaId
            };
            if (!Lane.Exists(lane)) Lane.Save(lane);
            lane = new Lane()
            {
                LaneNo = 24,
                LaneId = "DM24",
                LaneType = "?",
                LaneAbbr = "DM24",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId,
                PlazaId = plaza.PlazaId
            };
            if (!Lane.Exists(lane)) Lane.Save(lane);
            lane = new Lane()
            {
                LaneNo = 25,
                LaneId = "DM25",
                LaneType = "?",
                LaneAbbr = "DM25",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId,
                PlazaId = plaza.PlazaId
            };
            if (!Lane.Exists(lane)) Lane.Save(lane);

            #endregion

            #endregion

            #endregion

            #endregion

            #region ANUSORN SATHAN

            #region TSB

            item = new TSB();
            item.NetworkId = "31";
            item.TSBId = "319";
            item.TSBNameEN = "ANUSORN SATHAN";
            item.TSBNameTH = "อนุสรน์สถาน";
            item.Active = true;
            if (!TSB.Exists(item)) TSB.Save(item);
            // init default credit value.
            InitTSBCreditInitializeTransaction(item);

            #endregion

            #region PlazaGroup ANUSORN SATHAN

            plazaGroup = new PlazaGroup()
            {
                PlazaGroupId = "AS",
                PlazaGroupNameEN = "ANUSORN SATHAN",
                PlazaGroupNameTH = "อนุสรน์สถาน",
                Direction = "?",
                TSBId = item.TSBId
            };
            if (!PlazaGroup.Exists(plazaGroup)) PlazaGroup.Save(plazaGroup);

            #region Plaza ANUSORN SATHAN 1

            plaza = new Plaza()
            {
                PlazaId = "3115",
                SCWPlazaId = 15,
                PlazaNameEN = "ANUSORN SATHAN 1",
                PlazaNameTH = "อนุสรน์สถาน 1",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId
            };
            if (!Plaza.Exists(plaza)) Plaza.Save(plaza);

            #region Lanes

            lane = new Lane()
            {
                LaneNo = 1,
                LaneId = "AN01",
                LaneType = "?",
                LaneAbbr = "AN01",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId,
                PlazaId = plaza.PlazaId
            };
            if (!Lane.Exists(lane)) Lane.Save(lane);
            lane = new Lane()
            {
                LaneNo = 2,
                LaneId = "AN02",
                LaneType = "?",
                LaneAbbr = "AN02",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId,
                PlazaId = plaza.PlazaId
            };
            if (!Lane.Exists(lane)) Lane.Save(lane);
            lane = new Lane()
            {
                LaneNo = 3,
                LaneId = "AN03",
                LaneType = "?",
                LaneAbbr = "AN03",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId,
                PlazaId = plaza.PlazaId
            };
            if (!Lane.Exists(lane)) Lane.Save(lane);
            lane = new Lane()
            {
                LaneNo = 4,
                LaneId = "AN04",
                LaneType = "?",
                LaneAbbr = "AN04",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId,
                PlazaId = plaza.PlazaId
            };
            if (!Lane.Exists(lane)) Lane.Save(lane);
            lane = new Lane()
            {
                LaneNo = 5,
                LaneId = "AN05",
                LaneType = "?",
                LaneAbbr = "AN05",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId,
                PlazaId = plaza.PlazaId
            };
            if (!Lane.Exists(lane)) Lane.Save(lane);

            #endregion

            #endregion

            #region Plaza ANUSORN SATHAN 2

            plaza = new Plaza()
            {
                PlazaId = "3116",
                SCWPlazaId = 16,
                PlazaNameEN = "ANUSORN SATHAN 2",
                PlazaNameTH = "อนุสรน์สถาน 2",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId
            };
            if (!Plaza.Exists(plaza)) Plaza.Save(plaza);

            #region Lanes

            lane = new Lane()
            {
                LaneNo = 11,
                LaneId = "AN11",
                LaneType = "?",
                LaneAbbr = "AN11",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId,
                PlazaId = plaza.PlazaId
            };
            if (!Lane.Exists(lane)) Lane.Save(lane);
            lane = new Lane()
            {
                LaneNo = 12,
                LaneId = "AN12",
                LaneType = "?",
                LaneAbbr = "AN12",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId,
                PlazaId = plaza.PlazaId
            };
            if (!Lane.Exists(lane)) Lane.Save(lane);
            lane = new Lane()
            {
                LaneNo = 13,
                LaneId = "AN13",
                LaneType = "?",
                LaneAbbr = "AN13",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId,
                PlazaId = plaza.PlazaId
            };
            if (!Lane.Exists(lane)) Lane.Save(lane);
            lane = new Lane()
            {
                LaneNo = 14,
                LaneId = "AN14",
                LaneType = "?",
                LaneAbbr = "AN14",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId,
                PlazaId = plaza.PlazaId
            };
            if (!Lane.Exists(lane)) Lane.Save(lane);
            lane = new Lane()
            {
                LaneNo = 15,
                LaneId = "AN15",
                LaneType = "?",
                LaneAbbr = "AN15",
                TSBId = item.TSBId,
                PlazaGroupId = plazaGroup.PlazaGroupId,
                PlazaId = plaza.PlazaId
            };
            if (!Lane.Exists(lane)) Lane.Save(lane);

            #endregion

            #endregion

            #endregion

            #endregion
        }

        private void InitTSBCreditInitializeTransaction(TSB value)
        {
            TSBCreditTransaction tran;
            tran = TSBCreditTransaction.GetInitialTransaction().Value();
            if (null == tran)
            {
                tran = new TSBCreditTransaction();
                tran.TSBId = value.TSBId;
                tran.TSBNameEN = value.TSBNameEN;
                tran.TSBNameTH = value.TSBNameTH;
                tran.TransactionDate = DateTime.Now;
                tran.TransactionType = TSBCreditTransaction.TransactionTypes.Initial;
                TSBCreditTransaction.SaveTransaction(tran);
            }
        }

        private void InitShifts()
        {
            if (null == Db) return;

            if (Db.Table<Shift>().Count() > 0) return; // already exists.

            Shift item;
            item = new Shift()
            {
                ShiftId = 1,
                ShiftNameEN = "Morning",
                ShiftNameTH = "เช้า"
            };
            if (!Shift.Exists(item)) Shift.Save(item);
            item = new Shift()
            {
                ShiftId = 2,
                ShiftNameEN = "Afternoon",
                ShiftNameTH = "บ่าย"
            };
            if (!Shift.Exists(item)) Shift.Save(item);
            item = new Shift()
            {
                ShiftId = 3,
                ShiftNameEN = "Midnight",
                ShiftNameTH = "ดึก"
            };
            if (!Shift.Exists(item)) Shift.Save(item);
        }

        private void InitRoleAndUsers()
        {
            if (null == Db) return;

            if (Db.Table<User>().Count() > 0) return; // has user data so not insert dummy.

            Role item;
            User user;
            string prefix;
            string fName;
            string mName;
            string lName;

            #region ADMINS

            item = new Role()
            {
                RoleId = "ADMINS",
                RoleNameEN = "Administrator",
                RoleNameTH = "ผู้ดูแลระบบ",
                GroupId = 10
            };
            if (!Role.Exists(item)) Role.Save(item);

            prefix = "Mr.";
            fName = "killer1115";
            mName = "";
            lName = "killer1115";
            user = new User()
            {
                UserId = "00112",
                PrefixEN = prefix,
                FirstNameEN = fName,
                MiddleNameEN = mName,
                LastNameEN = lName,
                PrefixTH = prefix,
                FirstNameTH = fName,
                MiddleNameTH = mName,
                LastNameTH = lName,
                Password = Utils.MD5.Encrypt("123456"),
                CardId = "",
                AccountStatus = User.AccountFlags.Avaliable,
                IsDummy = true,
                RoleId = item.RoleId
            };
            if (!User.Exists(user)) User.Save(user);

            #endregion

            #region ACCOUNT

            item = new Role()
            {
                RoleId = "ACCOUNT",
                RoleNameEN = "Account",
                RoleNameTH = "บัญชี",
                GroupId = 63
            };
            if (!Role.Exists(item)) Role.Save(item);

            #endregion

            #region CTC

            item = new Role()
            {
                RoleId = "CTC",
                RoleNameEN = "Chief Toll Collector",
                RoleNameTH = "หัวหน้าพนักงานจัดเก็บค่าผ่านทาง",
                GroupId = 40
            };
            if (!Role.Exists(item)) Role.Save(item);

            prefix = "Mr.";
            fName = "CTC";
            mName = "";
            lName = "Test";
            user = new User()
            {
                UserId = "00444",
                PrefixEN = prefix,
                FirstNameEN = fName,
                MiddleNameEN = mName,
                LastNameEN = lName,
                PrefixTH = prefix,
                FirstNameTH = fName,
                MiddleNameTH = mName,
                LastNameTH = lName,
                Password = Utils.MD5.Encrypt("123456"),
                CardId = "",
                AccountStatus = User.AccountFlags.Avaliable,
                IsDummy = true,
                RoleId = item.RoleId
            };
            if (!User.Exists(user)) User.Save(user);

            #endregion

            #region TC

            item = new Role()
            {
                RoleId = "TC",
                RoleNameEN = "Toll Collector",
                RoleNameTH = "พนักงาน",
                GroupId = 20
            };
            if (!Role.Exists(item)) Role.Save(item);

            prefix = "Mr.";
            fName = "Hussakorn";
            mName = "";
            lName = "VRS";
            user = new User()
            {
                UserId = "20001",
                PrefixEN = prefix,
                FirstNameEN = fName,
                MiddleNameEN = mName,
                LastNameEN = lName,
                PrefixTH = prefix,
                FirstNameTH = fName,
                MiddleNameTH = mName,
                LastNameTH = lName,
                Password = Utils.MD5.Encrypt("123456"),
                CardId = "",
                AccountStatus = User.AccountFlags.Avaliable,
                IsDummy = true,
                RoleId = item.RoleId
            };
            if (!User.Exists(user)) User.Save(user);

            #endregion

            #region MT_ADMIN

            item = new Role()
            {
                RoleId = "MT_ADMIN",
                RoleNameEN = "Maintenance Administrator",
                RoleNameTH = "ทีมซ่อมบำรุง กลุ่ม Admin",
                GroupId = 10
            };
            if (!Role.Exists(item)) Role.Save(item);

            #endregion

            #region MT_TECH

            item = new Role()
            {
                RoleId = "MT_TECH",
                RoleNameEN = "Maintenance Technical",
                RoleNameTH = "ทีมซ่อมบำรุง กลุ่มช่าง",
                GroupId = 51
            };
            if (!Role.Exists(item)) Role.Save(item);

            #endregion

            #region CTC_MGR

            item = new Role()
            {
                RoleId = "CTC_MGR",
                RoleNameEN = "Chief Toll Manager",
                RoleNameTH = "หัวหน้าแผนก",
                GroupId = 49
            };
            if (!Role.Exists(item)) Role.Save(item);

            #endregion

            #region FINANCE

            item = new Role()
            {
                RoleId = "FINANCE",
                RoleNameEN = "Finance",
                RoleNameTH = "การเงิน",
                GroupId = 64
            };
            if (!Role.Exists(item)) Role.Save(item);

            #endregion

            #region SV

            item = new Role()
            {
                RoleId = "SV",
                RoleNameEN = "Supervisor",
                RoleNameTH = "พนักงานควบคุม",
                GroupId = 30
            };
            if (!Role.Exists(item)) Role.Save(item);

            #endregion

            #region RAD_MGR

            item = new Role()
            {
                RoleId = "RAD_MGR",
                RoleNameEN = "Revenue Audit Division (Manager)",
                RoleNameTH = "แผนกตรวจสอบรายได้ค่าผ่านทาง (Manager)",
                GroupId = 60
            };
            if (!Role.Exists(item)) Role.Save(item);

            #endregion

            #region RAD_SUP

            item = new Role()
            {
                RoleId = "RAD_SUP",
                RoleNameEN = "Revenue Audit Division (Supervisor)",
                RoleNameTH = "แผนกตรวจสอบรายได้ค่าผ่านทาง (Supervisor)",
                GroupId = 61
            };
            if (!Role.Exists(item)) Role.Save(item);

            #endregion
        }

        private void InitViews()
        {
            if (null == Db) return;

            string prefix;

            // Infrastructures - Embeded resource used . instead / to access sub contents.
            prefix = @"Infrastructures";
            InitView("PlazaGroupView", 1, prefix);
            InitView("PlazaView", 1, prefix);
            InitView("LaneView", 1, prefix);

            // Users - Embeded resource used . instead / to access sub contents.
            prefix = @"Users";
            InitView("UserView", 1, prefix);

            // Shifts - Embeded resource used . instead / to access sub contents.
            prefix = @"Shifts";
            InitView("TSBShiftView", 1, prefix);

            // Revenues - Embeded resource used . instead / to access sub contents.
            prefix = @"Revenues";
            InitView("RevenueEntryView", 1, prefix);

            // Credits - Embeded resource used . instead / to access sub contents.
            prefix = @"Credits";
            InitView("UserCreditTransactionView", 1, prefix);
            InitView("UserCreditBorrowSummaryView", 1, prefix);
            InitView("UserCreditReturnSummaryView", 1, prefix);
            // !!! Required UserCreditBorrowSummaryView and UserCreditBorrowSummaryView
            InitView("UserCreditSummaryView", 1, prefix);

            InitView("TSBCreditTransactionView", 1, prefix);
            // User Total.
            InitView("TSBCreditUserBHTTotalSummaryView", 1, prefix);
            // TSB Amount(s).
            InitView("TSBCreditST25SummaryView", 1, prefix);
            InitView("TSBCreditST50SummaryView", 1, prefix);
            InitView("TSBCreditBHT1SummaryView", 1, prefix);
            InitView("TSBCreditBHT2SummaryView", 1, prefix);
            InitView("TSBCreditBHT5SummaryView", 1, prefix);
            InitView("TSBCreditBHT10SummaryView", 1, prefix);
            InitView("TSBCreditBHT20SummaryView", 1, prefix);
            InitView("TSBCreditBHT50SummaryView", 1, prefix);
            InitView("TSBCreditBHT100SummaryView", 1, prefix);
            InitView("TSBCreditBHT500SummaryView", 1, prefix);
            InitView("TSBCreditBHT1000SummaryView", 1, prefix);
            // !!! Required Above views
            InitView("TSBCreditSummaryView", 1, prefix);

            // Coupons - Embeded resource used . instead / to access sub contents.
            prefix = @"Coupons";
            InitView("TSBCouponTransactionView", 1, prefix);

            InitView("TSBCouponSummarryView", 1, prefix);
            InitView("TSBCouponStockBalanceView", 1, prefix);
            InitView("TSBCouponLaneBalanceView", 1, prefix);
            InitView("TSBCouponSoldByLaneBalanceView", 1, prefix);
            InitView("TSBCouponSoldByTSBBalanceView", 1, prefix);
            // !!! Required 
            // - TSBCouponSummarryView
            // - TSBCouponStockBalanceView
            // - TSBCouponLaneBalanceView
            // - TSBCouponSoldByLaneBalanceView
            // - TSBCouponSoldByTSBBalanceView
            InitView("TSBCouponBalanceView", 1, prefix);

            /*
            InitView("UserCouponBalanceView", 1, prefix);

            InitView("UserCouponSoldBalanceView", 1, prefix);

            InitView("UserCoupon35SummaryView", 1, prefix);
            InitView("UserCoupon80SummaryView", prefix);
            InitView("UserCouponSummaryView", 1, prefix);
            InitView("UserCouponTransactionView", 1, prefix);
            */

            // Exchanges - Embeded resource used . instead / to access sub contents.
            prefix = @"Exchanges";
            InitView("TSBExchangeGroupView", 1, prefix);
            InitView("TSBExchangeTransactionView", 1, prefix);
        }

        class ViewInfo
        {
            public string Name { get; set; }
        }

        private void InitView(string viewName, int version, string resourcePrefix = "")
        {
            if (null == Db) return;

            var hist = ViewHistory.GetWithChildren(viewName, false).Value();

            string checkViewCmd = "SELECT Name FROM sqlite_master WHERE Type = 'view' AND Name = ?";
            var rets = Db.Query<ViewInfo>(checkViewCmd, viewName);
            bool exists = (null != rets && rets.Count > 0);

            //bool exists = (null != info) ? info.Count > 0 : false;

            if (!exists || null == hist || hist.VersionId < version)
            {
                string script = string.Empty;
                MethodBase med = MethodBase.GetCurrentMethod();
                try
                {
                    string dropCmd = string.Empty;
                    dropCmd += "DROP VIEW IF EXISTS " + viewName;
                    Db.BeginTransaction();
                    try { Db.Execute(dropCmd); }
                    catch (Exception dropEx)
                    {
                        med.Err(dropEx);
                        med.Err("Drop Failed:" + Environment.NewLine + viewName);
                        Db.Rollback();
                    }
                    finally { Db.Commit(); }

                    string resourceName = viewName + ".sql";
                    // Note: 
                    // -----------------------------------------------------------
                    // Embeded resource used . instead / to access sub contents.
                    // -----------------------------------------------------------
                    string embededResourceName;
                    if (!string.IsNullOrWhiteSpace(resourcePrefix))
                    {
                        // Has prefix
                        if (!resourcePrefix.Trim().EndsWith("."))
                        {
                            // Not end with . so append . and concat full name.
                            embededResourceName = @"DMT.Views.Scripts." + resourcePrefix + "." + resourceName;
                        }
                        else
                        {
                            // Already end with . so concat to full name.
                            embededResourceName = @"DMT.Views.Scripts." + resourcePrefix + resourceName;
                        }
                    }
                    else
                    {
                        // No prefix.
                        embededResourceName = @"DMT.Views.Scripts." + resourceName;
                    }

                    script = TASqliteScriptManager.GetScript(embededResourceName);

                    if (!string.IsNullOrEmpty(script))
                    {
                        var ret = Db.Execute(script);
                        //Console.WriteLine("Returns: {0}", ret);

                        if (null == hist) hist = new ViewHistory();
                        hist.ViewName = viewName;
                        hist.VersionId = version;
                        ViewHistory.Save(hist);

                        string msg = string.Format("Update View {0}, version {1}.", hist.ViewName, hist.VersionId);
                        Console.WriteLine(msg);
                        med.Info(msg);
                    }
                    else
                    {
                        Console.WriteLine("{0} Has Empty Scripts.", viewName);
                    }
                }
                catch (Exception ex)
                {
                    //Console.WriteLine(ex);
                    med.Err(ex);
                    med.Err("Script:" + Environment.NewLine + script);
                    //Console.WriteLine(script);
                }
            }
        }

        #endregion

        #region Public Methods (Start/Shutdown)

        /// <summary>
        /// Start.
        /// </summary>
        public void Start()
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            if (null == Db)
            {
                lock (typeof(TALocalDbServer))
                {
                    try
                    {
                        // ---------------------------------------------------------------
                        // NOTE:
                        // ---------------------------------------------------------------
                        // If Exception due to version mismatch here
                        // Please rebuild only this project and try again
                        // VS Should Solve mismatch version properly (maybe)
                        // See: https://nickcraver.com/blog/2020/02/11/binding-redirects/
                        // for more information.
                        // ---------------------------------------------------------------

                        string path = Path.Combine(LocalFolder, FileName);
                        Db = new SQLiteConnection(path,
                            SQLiteOpenFlags.Create |
                            SQLiteOpenFlags.SharedCache |
                            SQLiteOpenFlags.ReadWrite |
                            SQLiteOpenFlags.FullMutex,
                            storeDateTimeAsTicks: false);
                        Db.BusyTimeout = new TimeSpan(0, 0, 5); // set busy timeout.
                    }
                    catch (Exception ex)
                    {
                        med.Err(ex);
                        Db = null;

                        OnConectError.Call(this, EventArgs.Empty);
                    }
                    if (null != Db)
                    {
                        // Set Default connection 
                        // (be careful to make sure that we only has single database
                        // for all domain otherwise call static method with user connnection
                        // in each domain class instead omit connection version).

                        NTable.Default = Db;
                        NQuery.Default = Db;
                        InitTables(); // Init Tables.
                        InitDefaults(); // init default data.
                        InitViews(); // init views.

                        OnConnected.Call(this, EventArgs.Empty);
                    }
                }
            }
        }
        /// <summary>
        /// Shutdown.
        /// </summary>
        public void Shutdown()
        {
            if (null != Db)
            {
                Db.Dispose();
            }
            Db = null;
            OnDisconnected.Call(this, EventArgs.Empty);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets database file name.
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// Gets SQLite Connection.
        /// </summary>
        public SQLiteConnection Db { get; private set; }
        /// <summary>
        /// Gets is database connected.
        /// </summary>
        public bool Connected { get { return (null != this.Db); } }

        #endregion

        #region Public Events

        /// <summary>
        /// OnConnected event.
        /// </summary>
        public event EventHandler OnConnected;
        /// <summary>
        /// OnDisconnected event.
        /// </summary>
        public event EventHandler OnDisconnected;
        /// <summary>
        /// OnConectError event.
        /// </summary>
        public event EventHandler OnConectError;

        #endregion
    }

    #endregion
}
