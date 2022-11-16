#region Using

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using DMT.Models;
using DMT.Services;
using NLib.Services;
using NLib.Reflection;
using System.Windows.Media;
using System.Windows.Threading;


using System.Reflection;
using NLib;

using System.Globalization;
using System.Threading;
using System.IO;
using Newtonsoft.Json;
using System.Linq;

using Renci.SshNet;

#endregion

namespace DMT.Windows
{
    /// <summary>
    /// Interaction logic for GetCouponWindow.xaml
    /// </summary>
    public partial class GetCouponWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public GetCouponWindow()
        {
            InitializeComponent();

            CultureInfo ci = CultureInfo.CreateSpecificCulture("en-US");
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yy";
            Thread.CurrentThread.CurrentCulture = ci;
            IFormatProvider culture = new System.Globalization.CultureInfo("en-US", true);

            MethodBase med = MethodBase.GetCurrentMethod();
            try
            {
                if (File.Exists(Directory.GetCurrentDirectory() + @"\\configs\\configSFTP.json"))
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
        }

        #endregion

        #region Window Handlers

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                e.Handled = true;
                DialogResult = false;
            }
        }

        #endregion

        #region Buton Handlers

        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            if (GenFileToSFTP() == true)
                DialogResult = true;
        }
        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        #endregion

        #region Private Methods

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

        #endregion

        #region Private Methods - FTP Coupon CSVs

        #region Default

        private string FolderToUpload = string.Empty;
        private string SFTPUploadFolder = string.Empty;
        public string Host = string.Empty;
        public string UserName = string.Empty;
        public string Password = string.Empty;
        public Int32? Port = null;
        #endregion

        #region ConfigInfo

        public class ConfigInfo
        {
            public string FolderToUpload { get; set; }
            public string SFTPUploadFolder { get; set; }
            public string Host { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
            public Int32? Port { get; set; }
        }

        #endregion

        #region SaveConfig

        private bool SaveConfig()
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            try
            {
                string folderToUpload = @"D:\Test_INF_SAP\export\";
                string sftpUploadFolder = @"/mnt/share-documents/DOCUMENTS/SAPUAT/SyncCoupon/";
                string host = "172.30.76.21";

                string userName = "sapftp";
                string password = "$apF+p";
                int? port = 22;

                ConfigInfo config = new ConfigInfo
                {
                    FolderToUpload = @folderToUpload,
                    SFTPUploadFolder = @sftpUploadFolder,
                    Host = @host,
                    UserName = @userName,
                    Password = @password,
                    Port = port,
                };

                try
                {
                    if (File.Exists(Directory.GetCurrentDirectory() + @"\\configs\\configSFTP.json"))
                    {
                        FileInfo fileCheck = new FileInfo(Directory.GetCurrentDirectory() + @"\\configs\\configSFTP.json");
                        fileCheck.Delete();
                    }
                }
                catch (Exception ex)
                {
                    med.Err(ex);
                }

                string json = JsonConvert.SerializeObject(config, Formatting.Indented);

                string path = Directory.GetCurrentDirectory() + @"\\configs\\configSFTP.json";
                //export data to json file. 
                using (TextWriter tw = new StreamWriter(path))
                {
                    tw.WriteLine(json);
                };

                FolderToUpload = @folderToUpload;
                SFTPUploadFolder = @sftpUploadFolder;
                Host = @host;
                UserName = @userName;
                Password = @password;
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
                if (File.Exists(Directory.GetCurrentDirectory() + @"\\configs\\configSFTP.json"))
                {
                    using (StreamReader file = File.OpenText(Directory.GetCurrentDirectory() + @"\\configs\\configSFTP.json"))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        ConfigInfo config = (ConfigInfo)serializer.Deserialize(file, typeof(ConfigInfo));

                        FolderToUpload = config.FolderToUpload;
                        SFTPUploadFolder = config.SFTPUploadFolder;
                        Host = config.Host;
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

        private bool GenFileToSFTP()
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            var tsb = (null != cbTSBs.SelectedItem && cbTSBs.SelectedItem is Models.TSB) ?
                cbTSBs.SelectedItem as Models.TSB : null;
            int? tollWayId = (null != tsb && tsb.TSBId != "00") ? Convert.ToInt32(tsb.TSBId) : new int?();

            string toll = string.Empty;
            bool chkSFTP = true;
            string newFileName = string.Empty;

            if (tollWayId != null)
            {
                if (tollWayId <= 9)
                    toll = "0" + tollWayId.ToString();
                else
                    toll = tollWayId.ToString();

                string dateString = DateTime.Now.ToString("yyyy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("HH") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss");

                string path = FolderToUpload;
                FileInfo fileInfo = new FileInfo(path + @"\T" + toll + "_" + dateString + ".txt");

                newFileName = fileInfo.FullName;
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
                    chkSFTP = false;
                }

                try
                {

                    string lastDate = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                    File.WriteAllText(newFileName, lastDate);

                    if (SendFileSFTP(newFileName) == true)
                    {
                        try
                        {
                            if (File.Exists(newFileName))
                            {
                                FileInfo fileCheck = new FileInfo(newFileName);
                                fileCheck.Delete();
                            }

                            chkSFTP = true;
                        }
                        catch (Exception ex)
                        {
                            med.Err(ex);
                            chkSFTP = false;
                        }
                    }
                    else
                        chkSFTP = false;
                }
                catch (Exception ex)
                {
                    med.Err(ex);
                    MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    chkSFTP = false;
                }


            }
            else
            {
                var tsbs = TSB.GetTSBs().Value();

                for (int i = 0; i < tsbs.Count(); i++)
                {
                    if (!string.IsNullOrEmpty(tsbs[i].TSBId))
                    {
                        toll = tsbs[i].TSBId;

                        string dateString = DateTime.Now.ToString("yyyy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("HH") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss");

                        string path = FolderToUpload;
                        FileInfo fileInfo = new FileInfo(path + @"\T" + toll + "_" + dateString + ".txt");

                        newFileName = fileInfo.FullName;
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
                            chkSFTP = false;
                            break;
                        }

                        try
                        {
                            string lastDate = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                            File.WriteAllText(newFileName, lastDate);

                            if (SendFileSFTP(newFileName) == true)
                            {
                                try
                                {
                                    if (File.Exists(newFileName))
                                    {
                                        FileInfo fileCheck = new FileInfo(newFileName);
                                        fileCheck.Delete();
                                    }

                                    chkSFTP = true;
                                }
                                catch (Exception ex)
                                {
                                    med.Err(ex);
                                    chkSFTP = false;
                                    break;
                                }
                            }
                            else
                                chkSFTP = false;
                        }
                        catch (Exception ex)
                        {
                            med.Err(ex);
                            MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            chkSFTP = false;
                            break;
                        }
                    }

                }
            }

            if (chkSFTP == true)
            {
                return true;
                //MessageBox.Show("Sync coupon Complete", "Complete");
            }
            else
            {
                MessageBox.Show("Can't Sync coupon", "Fail");
                return false;
            }

        }

        #endregion


        #endregion

        private bool SendFileSFTP(string newFileName)
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            string msg = string.Empty;
            bool chkSend = true;
            try
            {
                int port = 22;

                if (Port != null)
                    port = Port.Value;

                using (var sftp = new SftpClient(Host, port, UserName, Password))
                {
                    sftp.KeepAliveInterval = TimeSpan.FromSeconds(60);
                    sftp.ConnectionInfo.Timeout = TimeSpan.FromMinutes(180);
                    sftp.OperationTimeout = TimeSpan.FromMinutes(180);
                    sftp.Connect();

                    if (sftp.IsConnected == true)
                    {
                        med.Info("Connect success.");

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
                                                sftp.UploadFile(inputStream, SFTPUploadFolder + System.IO.Path.GetFileName(file));
                                                

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
                                chkSend = false;
                                break;
                            }
                        }
                    }

                    sftp.Disconnect();
                    sftp.Dispose();

                    med.Info("Disconnect.");
                }
                        return chkSend;
            }
            catch (Exception ex)
            {
                med.Err(ex);
                return false;
            }
        }

        #region Setup
        /// <summary>
        /// Setup.
        /// </summary>
        /// <param name="msg"></param>
        public void Setup()
        {
            LoadTSBs();
        }

        #endregion
    }
}
