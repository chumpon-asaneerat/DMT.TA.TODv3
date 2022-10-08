#region Using

using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using System.Reflection;

using DMT.Models;
using DMT.Services;

using NLib;
using NLib.Services;
using NLib.Reflection;

using System.IO;
using Renci.SshNet;
using Newtonsoft.Json;
using System.Linq;

#endregion

namespace DMT.Account.Pages.Coupon
{
    using ops = DMT.Services.Operations.TAxTOD.Coupon;

    /// <summary>
    /// Interaction logic for CouponHistoryViewPage.xaml
    /// </summary>
    public partial class CouponHistoryViewPage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public CouponHistoryViewPage()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private User _chief = null;

        #endregion

        #region Button Handlers

        private void cmdBack_Click(object sender, RoutedEventArgs e)
        {
            GotoMainMenu();
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }

        private void cmdClear_Click(object sender, RoutedEventArgs e)
        {
            ClearInputs();
        }

        private void cmdGetCoupon_Click(object sender, RoutedEventArgs e)
        {
            if (cbTSBs.SelectedIndex > 0)
                GenFileToSFTP();
            else
                MessageBox.Show("โปรดเลือกด่านเก็บเงิน");
        }

        #endregion

        #region Private Methods

        private void GotoMainMenu()
        {
            // Main Menu Page
            var page = AccountApp.Pages.MainMenu;
            PageContentManager.Instance.Current = page;
        }

        private void LoadTSBs()
        {
            var tsbs = TSB.GetTSBs().Value();
            if (null != tsbs)
            {
                tsbs.Insert(0, new Models.TSB() { TSBId = "00", TSBNameEN = "[All]", TSBNameTH = "[ไม่ระบุด่าน]" });
            }
            cbTSBs.ItemsSource = tsbs;
            if (null != tsbs) cbTSBs.SelectedIndex = 0;
        }

        private void LoadShifts()
        {
            var shifts = AccountAPI.Shifts;
            if (null != shifts)
            {
                shifts.Insert(0, new Models.Shift() { ShiftId = 0, ShiftNameEN = "[All]", ShiftNameTH = "[เลือกทั้งหมด]" });
            }
            cbShifts.ItemsSource = shifts;
            if (null != shifts) cbShifts.SelectedIndex = 0;
        }

        private void LoadInquiryStatus()
        {
            var status = InquiryStatus.Gets();
            cbStatus.ItemsSource = status;
            if (null != status) cbStatus.SelectedIndex = 0;
        }

        private void Reset()
        {
            txtSAPItemCode.Text = string.Empty;
            txtSAPIntrSerial.Text = string.Empty;
            txtSAPTransferNo.Text = string.Empty;
            txtSAPARInvoice.Text = string.Empty;

            dtWorkDateFrom.Value = new DateTime?();
            dtWorkDateTo.Value = new DateTime?();
        }

        private void ClearInputs()
        {
            Reset();

            CheckComboDefaultItem(cbTSBs);
            CheckComboDefaultItem(cbStatus);
            CheckComboDefaultItem(cbShifts);

            txtStockBalance.Text = "0"; // clear stock.
            grid.ItemsSource = null; // clear grid.
        }

        private void CheckComboDefaultItem(ComboBox cb)
        {
            if (null == cb) return;
            cb.SelectedIndex = -1;
            if (null != cb.ItemsSource)
            {
                var lst = cb.ItemsSource as IList;
                if (null != lst && lst.Count > 0) cb.SelectedIndex = 0;
            }
        }

        private void Search()
        {
            string sapItemCode = string.IsNullOrWhiteSpace(txtSAPItemCode.Text) ? null : txtSAPItemCode.Text.Trim();
            string sapIntrSerial = string.IsNullOrWhiteSpace(txtSAPIntrSerial.Text) ? null : txtSAPIntrSerial.Text.Trim();
            string sapTransferNo = string.IsNullOrWhiteSpace(txtSAPTransferNo.Text) ? null : txtSAPTransferNo.Text.Trim();
            string sapARInvoice = string.IsNullOrWhiteSpace(txtSAPARInvoice.Text) ? null : txtSAPARInvoice.Text.Trim();

            var status = (null != cbStatus.SelectedItem && cbStatus.SelectedItem is Models.InquiryStatus) ?
                cbStatus.SelectedItem as Models.InquiryStatus : null;
            int? itemStatusDigit = (null != status) ? status.ItemStatusDigit : new int?();

            DateTime? workingDateFrom = (dtWorkDateFrom.Value.HasValue) ? dtWorkDateFrom.Value.Value : new DateTime?();
            DateTime? workingDateTo = (dtWorkDateTo.Value.HasValue) ? dtWorkDateTo.Value.Value : new DateTime?();

            var tsb = (null != cbTSBs.SelectedItem && cbTSBs.SelectedItem is Models.TSB) ?
                cbTSBs.SelectedItem as Models.TSB : null;
            int? tollWayId = (null != tsb && tsb.TSBId != "00") ? Convert.ToInt32(tsb.TSBId) : new int?();

            var shift = (null != cbShifts.SelectedItem && cbShifts.SelectedItem is Models.Shift) ?
                cbShifts.SelectedItem as Models.Shift : null;
            int? shiftId = (null != shift) ? shift.ShiftId : new int?();

            var searchOp = Models.Search.TAxTOD.Coupon.Inquiry.Create(sapItemCode, sapIntrSerial, sapTransferNo,
                sapARInvoice, itemStatusDigit, tollWayId, shiftId, workingDateFrom, workingDateTo);
            var ret = ops.Inquiry(searchOp);
            var coupons = ret.Value();
            grid.ItemsSource = coupons;

            if (null != coupons)
            {
                int cnt = coupons.Count;
                /*
                int cnt = coupons.Count(coupon => 
                { 
                    return (coupon.ItemStatusDigit.HasValue && coupon.ItemStatusDigit.Value == 1);
                });
                */
                txtStockBalance.Text = string.Format("{0:n0}", cnt);
            }
            else txtStockBalance.Text = "0";
        }

        #region Get CSV

        #region Default

        string FolderToUpload = string.Empty;
        string SFTPUploadFolder = string.Empty;

        string Host = string.Empty;
        string Host2 = string.Empty;
        string UserName = string.Empty;
        string Password = string.Empty;
        int? Port = 22;


        #endregion

        #region ConfigInfo
        public class ConfigInfo
        {
            public System.String FolderToUpload { get; set; }
            public System.String SFTPUploadFolder { get; set; }

            public System.String Host { get; set; }
            public System.String Host2 { get; set; }
            public System.String UserName { get; set; }
            public System.String Password { get; set; }
            public System.Int32? Port { get; set; }

        }
        #endregion

        #region SaveConfig
        private bool SaveConfig()
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            try
            {
                string folderToUpload = @"D:\Test_INF_SAP\export\";
                string sftpUploadFolder = @"/home/sapftp/SAP/coupon/";

                string host = "203.114.69.22";
                string host2 = "203.114.69.6";
                string userName = "sapftp";
                string password = "$apF+p";
                int? port = 22;

                ConfigInfo config = new ConfigInfo
                {
                    FolderToUpload = @folderToUpload,
                    SFTPUploadFolder = @sftpUploadFolder,

                    Host = host,
                    Host2 = host2,
                    UserName = userName,
                    Password = password,
                    Port = port
                };

                try
                {
                    if (File.Exists(Directory.GetCurrentDirectory() + @"\\configSFTP.json"))
                    {
                        FileInfo fileCheck = new FileInfo(Directory.GetCurrentDirectory() + @"\\configSFTP.json");
                        fileCheck.Delete();
                    }
                }
                catch (Exception ex)
                {
                    med.Err(ex);
                }

                string json = JsonConvert.SerializeObject(config, Formatting.Indented);

                string path = Directory.GetCurrentDirectory() + @"\\configSFTP.json";
                //export data to json file. 
                using (TextWriter tw = new StreamWriter(path))
                {
                    tw.WriteLine(json);
                };

                FolderToUpload = @folderToUpload;
                SFTPUploadFolder = @sftpUploadFolder;

                Host = host;
                Host2 = host2;
                UserName = userName;
                Password = password;
                Port = port;

                return true;
            }
            catch (Exception ex)
            {
                med.Err(ex);
                return false;
            }
        }
        #endregion

        #region LoadConfig
        private bool LoadConfig()
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            try
            {
                if (File.Exists(Directory.GetCurrentDirectory() + @"\\configSFTP.json"))
                {
                    using (StreamReader file = File.OpenText(Directory.GetCurrentDirectory() + @"\\configSFTP.json"))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        ConfigInfo config = (ConfigInfo)serializer.Deserialize(file, typeof(ConfigInfo));

                        FolderToUpload = config.FolderToUpload;
                        SFTPUploadFolder = config.SFTPUploadFolder;

                        Host = config.Host;
                        Host2 = config.Host2;
                        UserName = config.UserName;
                        Password = config.Password;
                        Port = config.Port;

                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                med.Err(ex);
                return false;
            }
        }
        #endregion

        #region GenFileToSFTP
        private void GenFileToSFTP()
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            try
            {
                if (File.Exists(Directory.GetCurrentDirectory() + @"\\configSFTP.json"))
                {
                    LoadConfig();
                }
                else
                {
                    SaveConfig();
                }

            }
            catch (Exception ex)
            {
                med.Err(ex);
            }

            var tsb = (null != cbTSBs.SelectedItem && cbTSBs.SelectedItem is Models.TSB) ?
                cbTSBs.SelectedItem as Models.TSB : null;
            int? tollWayId = (null != tsb && tsb.TSBId != "00") ? Convert.ToInt32(tsb.TSBId) : new int?();

            if (tollWayId != null)
            {
                string toll = string.Empty;

                if (tollWayId <= 9)
                    toll = "0" + tollWayId.ToString();
                else
                    toll = tollWayId.ToString();

                string dateString = DateTime.Now.ToString("yyyy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("HH") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss");

                string path = FolderToUpload;
                FileInfo fileInfo = new FileInfo(path + @"\T" + toll + "_" + dateString + ".txt");

                string newFileName = fileInfo.FullName;
                try
                {
                    if (File.Exists(newFileName))
                    {
                        FileInfo fileCheck = new FileInfo(newFileName);
                        fileCheck.Delete();
                    }
                }
                catch (Exception ex)
                {
                    med.Err(ex);
                }

                try
                {
                    bool chkSFTP = false;

                    string lastDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    File.WriteAllText(newFileName, lastDate);

                    if (!string.IsNullOrEmpty(Host) || !string.IsNullOrEmpty(Host2))
                    {
                        #region SFTPAllFile
                        if (SFTPAllFile(Host, SFTPUploadFolder) == false)
                        {
                            if (!string.IsNullOrEmpty(Host2))
                            {
                                if (SFTPAllFile(Host2, SFTPUploadFolder) == true)
                                {
                                    chkSFTP = true;
                                }
                            }

                        }
                        else
                        {
                            chkSFTP = true;
                        }
                        #endregion
                    }

                    if (chkSFTP == true)
                        MessageBox.Show(newFileName, "Complete");
                    else
                        MessageBox.Show("Can't SFTP File Name : "+ newFileName, "Fail");
                }
                catch (Exception ex)
                {
                    med.Err(ex);
                    MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        #endregion

        #region SFTPAllFile
        private bool SFTPAllFile(string host, string ftpDirectory)
        {

            string msg = string.Empty;
            MethodBase med = MethodBase.GetCurrentMethod();
            try
            {
                bool chkSFTP = true;
                int? countRow = 0;

                string username = UserName;
                string password = Password;
                int port = 22;

                if (Port != null)
                    port = Port.Value;

                SftpClient client = new SftpClient(host, port, username, password);
                client.KeepAliveInterval = TimeSpan.FromSeconds(60);
                client.ConnectionInfo.Timeout = TimeSpan.FromMinutes(180);
                client.OperationTimeout = TimeSpan.FromMinutes(180);
                client.Connect();

                if (client.IsConnected == true)
                {
                    msg = "Connect success.";
                    med.Info(msg);

                    string localDirectory = FolderToUpload;
                    //string localPattern = "*.*";
                    //string[] files = Directory.GetFiles(localDirectory, localPattern);

                    var files = Directory.GetFiles(localDirectory, "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith(".txt") || s.EndsWith(".csv"));

                    foreach (string file in files)
                    {
                        try
                        {
                            using (Stream inputStream = new FileStream(file, FileMode.Open))
                            {
                                string remoteFileName = System.IO.Path.GetFileName(file);

                                if (!remoteFileName.StartsWith("."))
                                {
                                    if (!string.IsNullOrEmpty(remoteFileName) && remoteFileName.Length > 4)
                                    {
                                        if (System.IO.Path.GetExtension(remoteFileName) == ".txt" || System.IO.Path.GetExtension(remoteFileName) == ".csv")
                                        {
                                            client.UploadFile(inputStream, ftpDirectory + System.IO.Path.GetFileName(file));

                                            countRow++;

                                            msg = string.Format("Success Upload file to : {0}.", System.IO.Path.GetFileName(file));
                                            med.Info(msg);
                                        }
                                    }
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            med.Err(ex);

                            chkSFTP = false;
                            break;
                        }
                    }

                    client.Disconnect();
                    client.Dispose();

                    msg = "Disconnect.";
                    med.Info(msg);


                    if (chkSFTP == true && countRow > 0)
                    {
                        foreach (string file in files)
                        {
                            try
                            {
                                string remoteFileName = System.IO.Path.GetFileName(file);

                                if (!remoteFileName.StartsWith("."))
                                {
                                    if (!string.IsNullOrEmpty(remoteFileName) && remoteFileName.Length > 4)
                                    {
                                        if (System.IO.Path.GetExtension(remoteFileName) == ".txt" || System.IO.Path.GetExtension(remoteFileName) == ".csv")
                                        {
                                            if (File.Exists(file))
                                            {
                                                FileInfo fileCheck = new FileInfo(file);
                                                fileCheck.Delete();

                                                msg = string.Format("Delete file: {0}.", file);
                                                med.Info(msg);
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                med.Err(ex);

                                chkSFTP = false;
                                break;
                            }
                        }
                    }
                }

                return chkSFTP;
            }
            catch (Exception ex)
            {
                med.Err(ex);

                return false;
            }
        }
        #endregion

        #endregion

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup
        /// </summary>
        /// <param name="chief">The Current User.</param>
        public void Setup(User chief)
        {
            _chief = chief;

            if (null != _chief)
            {

            }

            LoadTSBs(); // TollwayId
            LoadShifts();
            LoadInquiryStatus();
            ClearInputs();

            // Focus on search textbox.
            Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                txtSAPItemCode.SelectAll();
                txtSAPItemCode.Focus();
            }));
        }

        #endregion

        
    }
}
