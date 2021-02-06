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
    #region PlazaDbServer

    /// <summary>
    /// Plaza Database Server.
    /// </summary>
    public class PlazaDbServer
    {
        #region Singelton

        private static PlazaDbServer _instance = null;
        /// <summary>
        /// Singelton Access.
        /// </summary>
        public static PlazaDbServer Instance
        {
            get
            {
                if (null == _instance)
                {
                    lock (typeof(PlazaDbServer))
                    {
                        _instance = new PlazaDbServer();
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
        private PlazaDbServer() : base()
        {
            this.FileName = "Plaza.db";
        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~PlazaDbServer()
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

            Db.CreateTable<Shift>();

            Db.CreateTable<Role>();
            Db.CreateTable<User>();
            Db.CreateTable<UserAccess>();

            //Db.CreateTable<TSB>();
            //Db.CreateTable<PlazaGroup>();
            //Db.CreateTable<Plaza>();
            //Db.CreateTable<Lane>();

            //Db.CreateTable<TSBShift>();
        }

        private void InitDefaults()
        {
            InitMCurrency();
            InitMCoupon();
            InitMCouponBook();
            InitMCardAllow();
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
                couponValue = 60,
                abbreviation = "60",
                description = "60 บาท"
            };
            if (!MCoupon.Exists(item)) MCoupon.Save(item);
            item = new MCoupon()
            {
                couponId = 4,
                couponValue = 70,
                abbreviation = "70",
                description = "70 บาท"
            };
            if (!MCoupon.Exists(item)) MCoupon.Save(item);
            item = new MCoupon()
            {
                couponId = 5,
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
            //InitView("PlazaGroupView", 1, prefix);
            //InitView("PlazaView", 1, prefix);
            //InitView("LaneView", 1, prefix);

            // Users - Embeded resource used . instead / to access sub contents.
            prefix = @"Users";
            InitView("UserView", 1, prefix);

            // Shifts - Embeded resource used . instead / to access sub contents.
            prefix = @"Shifts";
            //InitView("TSBShiftView", 1, prefix);
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

                    script = PlazaSqliteScriptManager.GetScript(embededResourceName);

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
                lock (typeof(PlazaDbServer))
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
