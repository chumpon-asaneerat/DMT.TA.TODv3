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
using System.Threading;

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

            Db.CreateTable<Shift>();

            Db.CreateTable<Role>();
            Db.CreateTable<User>();
            Db.CreateTable<UserAccess>();

            Db.CreateTable<TSB>();
            Db.CreateTable<PlazaGroup>();
            Db.CreateTable<Plaza>();
            Db.CreateTable<Lane>();
        }

        private void InitDefaults()
        {
            InitShifts();
            InitRoleAndUsers();
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
                PasswordDate = DateTime.Now,
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
                PasswordDate = DateTime.Now,
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
                PasswordDate = DateTime.Now,
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

        #region Private Methods (backup db every hour)

        private Thread _th;
        private DateTime _lastCheck = DateTime.Now;
        private bool _onbackup = false;

        private void CreateThread()
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            if (null != _th)
            {
                med.Info("Auto backup thread is alreary created.");
                return;
            }

            _th = new Thread(AutoBackup);
            _th.Priority = ThreadPriority.BelowNormal;
            _th.Start();
        }

        private void FreeThread()
        {
            try
            {
                if (null != _th) _th.Abort();
            }
            catch (ThreadAbortException)
            {
                Thread.ResetAbort();
            }
            _th = null;
        }

        private void AutoBackup()
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            while (_th != null)
            {
                var ts = DateTime.Now - _lastCheck;
                if (ts.TotalMinutes <= 20) continue; // check every 20 minutes.

                if (_onbackup) continue;

                _onbackup = true;
                try
                {
                    Backup();
                }
                catch (Exception ex)
                {
                    med.Err(ex);
                }
                finally
                {
                    _onbackup = false;

                    _lastCheck = DateTime.Now;
                }
            }
            FreeThread();
        }

        private void Backup()
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            try
            {
                RemoveOldFiles();

                string srcFile = Path.Combine(LocalFolder, FileName);
                string bakDir = Path.Combine(LocalFolder, "Backup");
                string backupFileName = string.Format("{0:yyyy.MM.dd.HH}.{1}", DateTime.Now, FileName);
                string bakFile = Path.Combine(bakDir, backupFileName);

                if (File.Exists(srcFile))
                {
                    if (!Directory.Exists(bakDir))
                    {
                        Directory.CreateDirectory(bakDir);
                    }
                    if (!File.Exists(bakFile))
                    {
                        // copy only when no backup found
                        med.Info("Backup local db:" + backupFileName);
                        File.Copy(srcFile, bakFile, true);
                    }
                    else
                    {
                        med.Info("Backup local db:" + backupFileName + " alrady exists.");
                    }
                }
            }
            catch (Exception ex)
            {
                med.Err("Fail to create backup db");
                med.Err(ex);
            }
        }

        private void RemoveOldFiles()
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            try
            {
                string bakDir = Path.Combine(LocalFolder, "Backup");
                DirectoryInfo di = new DirectoryInfo(bakDir);
                FileInfo[] files = di.GetFiles("*.db");
                if (null == files || files.Length <= 24)
                    return;

                List<string> fileNames = new List<string>();
                foreach (var item in files)
                    fileNames.Add(item.FullName);
                fileNames.Sort();

                med.Info("Remove Backup local db:" + fileNames[0]);

                File.Delete(fileNames[0]); // remove first file.
            }
            catch (Exception ex)
            {
                med.Err(ex);
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

                        CreateThread();
                    }
                }
            }
        }
        /// <summary>
        /// Shutdown.
        /// </summary>
        public void Shutdown()
        {
            FreeThread();

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
