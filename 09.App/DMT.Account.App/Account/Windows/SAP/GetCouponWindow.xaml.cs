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

        #endregion

        #region ConfigInfo

        public class ConfigInfo
        {
            public string FolderToUpload { get; set; }
          
        }

        #endregion

        #region SaveConfig

        private bool SaveConfig()
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            try
            {
                string folderToUpload = @"D:\Test_INF_SAP\export\";
                
                ConfigInfo config = new ConfigInfo
                {
                    FolderToUpload = @folderToUpload,
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


            if (tollWayId != null)
            {
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
                    chkSFTP = false;
                }

                try
                {

                    string lastDate = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                    File.WriteAllText(newFileName, lastDate);
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
                            chkSFTP = false;
                            break;
                        }

                        try
                        {
                            string lastDate = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                            File.WriteAllText(newFileName, lastDate);
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
