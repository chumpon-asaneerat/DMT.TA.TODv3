#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Threading;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

using DMT.Configurations;
using DMT.Models;
using DMT.Models.ExtensionMethods;
using DMT.Services;

using NLib;
using NLib.Services;
using NLib.Reports.Rdlc;
using NLib.Reflection;
using System.Windows.Threading;
using System.Threading.Tasks;

#endregion

namespace DMT.TOD.Pages.Revenue
{
    using scwOps = Services.Operations.SCW.TOD;

    /// <summary>
    /// Interaction logic for CollectorRevenueEntryPage.xaml
    /// </summary>
    public partial class CollectorRevenueEntryPage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public CollectorRevenueEntryPage()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        //private CultureInfo culture = new CultureInfo("th-TH") { DateTimeFormat = { Calendar = new ThaiBuddhistCalendar() } };
        //private CultureInfo culture = new CultureInfo("th-TH");

        private User _user = null;
        private RevenueEntryManager manager = null;

        #endregion

        #region Loaded/Unloaded

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //Thread.CurrentThread.CurrentCulture = culture;
            //Thread.CurrentThread.CurrentUICulture = culture;
            TODConfigManager.Instance.ConfigChanged += Instance_ConfigChanged;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            TODConfigManager.Instance.ConfigChanged -= Instance_ConfigChanged;
        }

        #endregion

        #region Config Changed Handler

        private void Instance_ConfigChanged(object sender, EventArgs e)
        {
            // Re assign User because when config change the CurrentTSBManager will reset all data
            // before this handler.
            if (null != manager) manager.User = _user;
            Setup(_user); // Reload.
        }

        #endregion

        #region Combobox Handlers

        private void cbPlazas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var plazaGroup = cbPlazas.SelectedItem as PlazaGroup;
            if (null != manager && null == plazaGroup) return;
            if (null != manager) manager.PlazaGroup = plazaGroup;
            RefreshJobList();
        }

        #endregion

        #region Button Handlers

        private void cmdBack_Click(object sender, RoutedEventArgs e)
        {
            GotoMainMenu();
        }

        private void cmdGotoRevenueEntry_Click(object sender, RoutedEventArgs e)
        {
            GotoRevenueEntry();
        }

        private void cmdBack2_Click(object sender, RoutedEventArgs e)
        {
            GotoMainMenu();
            // Used below code if need to go back to select date.
            //GotoPrevious();
        }

        private void cmdGotoRevenueEntryPreview_Click(object sender, RoutedEventArgs e)
        {
            GotoPrintPreview();
        }

        private void cmdBack3_Click(object sender, RoutedEventArgs e)
        {
            GotoPrevious();
        }

        private void cmdOk_Click(object sender, RoutedEventArgs e)
        {
            // Printing.
            PrintReport();
        }

        #endregion

        #region Private Methods

        #region Tab Navigate methods

        private void GotoMainMenu()
        {
            // Main Menu Page
            var page = TODApp.Pages.MainMenu;
            PageContentManager.Instance.Current = page;
        }

        private void GotoPrevious()
        {
            if (tabs.SelectedIndex == 0)
            {
                GotoMainMenu();
            }
            else if (tabs.SelectedIndex == 1)
            {
                tabs.SelectedIndex = 0;
            }
            else if (tabs.SelectedIndex == 2)
            {
                tabs.SelectedIndex = 1;
            }
        }

        private void GotoRevenueEntry()
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            string msg;

            #region Check RevenueEntryManager

            if (null == manager)
            {
                var win = TODApp.Windows.MessageBox;
                win.Setup("Program Error: RevenueEntryManager is null.", "DMT - Tour of Duty");
                win.ShowDialog();
                return;
            }

            #endregion

            #region Check Select PlazaGroup

            if (null == manager.PlazaGroup)
            {
                var win = TODApp.Windows.MessageBox;
                win.Setup("กรุณาเลือกด่านของรายได้", "DMT - Tour of Duty");
                win.ShowDialog();
                cbPlazas.Focus();
                return;
            }

            #endregion

            #region Check Has Revenue Shift (for collector)

            manager.CheckRevenueShift();

            if (null != manager.RevenueShift)
            {
                if (manager.RevenueShift.RevenueDate.HasValue &&
                    manager.RevenueShift.RevenueDate.Value != DateTime.MinValue)
                {
                    var win = TODApp.Windows.MessageBox;
                    msg = "กะของพนักงานนี้ ถูกป้อนรายได้แล้ว";
                    med.Info("REVENUE ENTRY UI - " + msg); // Write log
                    win.Setup(msg, "DMT - Tour of Duty");
                    win.ShowDialog();
                    return;
                }
            }
            else
            {
                if (manager.IsNewRevenueShift)
                {
                    var win = TODApp.Windows.MessageBox;
                    msg = "ไม่สามารถนำส่งรายได้ เนื่องจากไม่พบข้อมูลการทำงาน";
                    med.Info("REVENUE ENTRY UI - " + msg); // Write log
                    win.Setup(msg, "DMT - Tour of Duty");
                    win.ShowDialog();
                    return;
                }
                else
                {
                    var win = TODApp.Windows.MessageBox;
                    msg = "กะนี้ถูกจัดเก็บรายได้แล้ว.";
                    med.Info("REVENUE ENTRY UI - " + msg); // Write log
                    win.Setup(msg, "DMT - Tour of Duty");
                    win.ShowDialog();
                    return;
                }
            }

            #endregion

            #region Check Jobs if online

            if (null != manager.Jobs && 
                manager.Jobs.SCWOnline && 
                (manager.Jobs.PlazaGroupJobs == null || manager.Jobs.PlazaGroupJobs.Count <= 0))
            {
                // Online but no jobs on current plaza group.
                var win = TODApp.Windows.MessageBox;
                msg = "ไม่พบข้อมูลเลนที่ยังไม่ถูกป้อนรายได้";
                med.Info("REVENUE ENTRY UI - " + msg); // Write log
                win.Setup(msg, "DMT - Tour of Duty");
                win.ShowDialog();
                return;
            }

            #endregion

            #region Check Sup Adjust

            if (SupAdjClient.Enabled)
            {
                bool connected = false;

                var subAdj = TODApp.Windows.SupAdjStatus;
                msg = "เริ่มดำเนินการเชื่อมต่อ ระบบ SUP ADJUST";
                subAdj.Notify(msg);
                med.Info("REVENUE ENTRY UI - " + msg); // Write log
                subAdj.Show();

                ApplicationManager.Instance.DoEvents();
                System.Threading.Thread.Sleep(10);

                SupAdjClient wcli = new SupAdjClient();
                wcli.Connect();

                ApplicationManager.Instance.DoEvents();
                System.Threading.Thread.Sleep(10);

                if (wcli.Connected)
                {
                    msg = "เชื่อมต่อ ระบบ SUP ADJUST สำเร็จ";
                    subAdj.Notify(msg);
                    med.Info("REVENUE ENTRY UI - " + msg); // Write log

                    connected = true;

                    ApplicationManager.Instance.DoEvents();
                    System.Threading.Thread.Sleep(10);

                    msg = "ตรวจสอบ จํานวนเหตุการณ์ ที่ยังไม่ทำการ ADJUST";
                    subAdj.Notify(msg);
                    med.Info("REVENUE ENTRY UI - " + msg); // Write log
                    wcli.Send(_user, manager.UserShift.Begin.Value);

                    // wait for timeout or message arrive.
                    while (!wcli.IsTimeout && !wcli.AllAck &&
                        !ApplicationManager.Instance.IsExit)
                    {
                        ApplicationManager.Instance.Wait(50);
                        ApplicationManager.Instance.DoEvents();
                    }

                    if (!wcli.AllAck && wcli.IsTimeout)
                    {
                        msg = "ไม่พบการตอบกลับ (TIMEOUT) จากระบบ SUP ADJUST";
                        med.Info("REVENUE ENTRY UI - " + msg); // Write log
                    }

                    med.Info("REVENUE ENTRY UI - INFO");
                    med.Info(string.Format("     IsTimeout: {0}", wcli.IsTimeout));
                    med.Info(string.Format("     AllAck: {0} ", wcli.AllAck));
                    med.Info(string.Format("     SendCount: {0} ", wcli.SendCount));
                    med.Info(string.Format("     ReceiveCount: {0} ", wcli.RecvCount));
                }
                else
                {
                    msg = "เชื่อมต่อ ระบบ SUP ADJUST ไม่สำเร็จ";
                    subAdj.Notify(msg);
                    med.Info("REVENUE ENTRY UI - " + msg); // Write log

                    ApplicationManager.Instance.DoEvents();
                    System.Threading.Thread.Sleep(10);
                }

                msg = "จบการเชื่อมต่อ ระบบ SUP ADJUST";
                subAdj.Notify(msg);
                med.Info("REVENUE ENTRY UI - " + msg); // Write log

                wcli.Disconnect();

                ApplicationManager.Instance.DoEvents();
                System.Threading.Thread.Sleep(10);

                bool hasAdjust = wcli.HasAdjustEvents();
                subAdj.Close();

                if (connected)
                {
                    if (hasAdjust)
                    {
                        var win = TODApp.Windows.MessageBox;
                        var ht = win.Height;
                        win.Height = ht + 50; // set new height.
                        
                        //msg = "ระบบตรวจพบว่ายังมีรายการเหตุการณ์ ที่ยังไม่ได้ทำการ Adjust. กรุณาติดต่อพนักงานควบคุม.";
                        var iCnt = wcli.AdjustCount;
                        msg = string.Format("ระบบตรวจพบว่ายังมีรายการเหตุการณ์ ที่ยังไม่ได้ทำการ Adjust จำนวน {0:n0} รายการ. กรุณาติดต่อพนักงานควบคุม.", iCnt);
                        med.Info("REVENUE ENTRY UI - " + msg); // Write log
                        win.Setup(msg, "DMT - Tour of Duty");
                        win.ShowDialog();

                        win.Height = ht; // restore height.
                        return;
                    }
                    else
                    {
                        if (!wcli.AllAck && wcli.IsTimeout)
                        {
                            var win = TODApp.Windows.MessageBoxYesNo;
                            msg = "ไม่สามารถติดต่อกับ Sup Adjust ได้ ต้องการนำส่งรายได้ ต่อ หรือไม่?";
                            med.Info("REVENUE ENTRY UI - " + msg); // Write log
                            win.Setup(msg, "DMT - Tour of Duty");
                            if (win.ShowDialog() == false)
                            {
                                // Write log
                                med.Info("REVENUE ENTRY UI - ยืนยันการดำเนินการนำส่งรายได้ (กรณี SUPADJ recv timeout).");
                                med.Info("     ผู้ใช้ยืนยัน: ไม่ดำเนินการต่อ");
                                return; // stay on current page.
                            }
                            else
                            {
                                // allow to do revenue entry.
                                // Write log
                                med.Info("REVENUE ENTRY UI - ยืนยันการดำเนินการนำส่งรายได้ (กรณี SUPADJ recv timeout).");
                                med.Info("     ผู้ใช้ยืนยัน: ดำเนินการต่อ");
                            }
                        }
                        else
                        {
                            msg = "ระบบตรวจพบว่าไม่มีรายการเหตุการณ์ ดำเนินการต่อไปได้.";
                            med.Info("REVENUE ENTRY UI - " + msg); // Write log
                        }
                    }
                }
                else
                {
                    /*
                    var win = TODApp.Windows.MessageBox;
                    msg = "เชื่อมต่อ ระบบ SUP ADJUST ไม่สำเร็จ.";
                    med.Info("REVENUE ENTRY UI - " + msg); // Write log
                    win.Setup(msg, "DMT - Tour of Duty");
                    win.ShowDialog();
                    //return; // allow to do revenue entry.
                    */
                    var win = TODApp.Windows.MessageBoxYesNo;
                    msg = "ไม่สามารถติดต่อกับ Sup Adjust ได้ ต้องการนำส่งรายได้ ต่อ หรือไม่?";
                    med.Info("REVENUE ENTRY UI - " + msg); // Write log
                    win.Setup(msg, "DMT - Tour of Duty");
                    if (win.ShowDialog() == false)
                    {
                        // Write log
                        med.Info("REVENUE ENTRY UI - ยืนยันการดำเนินการนำส่งรายได้ (กรณี SUPADJ connect failed).");
                        med.Info("     ผู้ใช้ยืนยัน: ไม่ดำเนินการต่อ");
                        return; // stay on current page.
                    }
                    else
                    {
                        // allow to do revenue entry.
                        // Write log
                        med.Info("REVENUE ENTRY UI - ยืนยันการดำเนินการนำส่งรายได้ (กรณี SUPADJ connect failed).");
                        med.Info("     ผู้ใช้ยืนยัน: ดำเนินการต่อ");
                    }
                }
            }
            else
            {
                msg = "ตรวจสอบพบว่า config มีการปิดการเชื่อมต่อ ระบบ SUP ADJUST ไว้.";
                med.Info("REVENUE ENTRY UI - " + msg); // Write log
            }

            #endregion

            #region Check Return Bag

            var usrCdtBalStatus = manager.GetUserCreditBalanceStatus();

            if (null == usrCdtBalStatus)
            {
                // Critical error. Must not execute this code. This should be imposible case.
                return;
            }

            if (null != usrCdtBalStatus && !usrCdtBalStatus.IsReturnBag)
            {
                if (usrCdtBalStatus.WSStatus != HttpStatus.Success)
                {
                    // Call WS has HTTP error or timeout.
                    //var win = TODApp.Windows.MessageBox;
                    var win = TODApp.Windows.MessageBoxYesNo;
                    msg = "ไม่สามารถติดต่อกับ Toll Admin ได้ ต้องการนำส่งรายได้ ต่อ หรือไม่?";
                    med.Info("REVENUE ENTRY UI - " + msg); // Write log
                    win.Setup(msg, "DMT - Tour of Duty");
                    if (win.ShowDialog() == false)
                    {
                        // Write log
                        med.Info("REVENUE ENTRY UI - ยืนยันการดำเนินการนำส่งรายได้ (กรณีเรียก WS ของ TA APP timeout หรือ HTTP error).");
                        med.Info("     ผู้ใช้ยืนยัน: ไม่ดำเนินการต่อ");
                        return;
                    }
                    else
                    {
                        // allow to do revenue entry.
                        // Write log
                        med.Info("REVENUE ENTRY UI - ยืนยันการดำเนินการนำส่งรายได้ (กรณีเรียก WS ของ TA APP timeout หรือ HTTP error).");
                        med.Info("     ผู้ใช้ยืนยัน: ดำเนินการต่อ");
                    }
                }
                else
                {
                    // Call WS is success. But bag status is not complted.
                    var win = TODApp.Windows.MessageBox;
                    msg = "ระบบตรวจพบว่า ยังไม่มีการคืนเงินยืมทอน กรุณาคืนเงินยืมทอนก่อนป้อนรายได้";
                    med.Info("REVENUE ENTRY UI - " + msg); // Write log
                    win.Setup(msg, "DMT - Tour of Duty");
                    win.ShowDialog();
                    return;
                }
            }

            #endregion

            #region Check Return Coupons

            var usrCoupBalStatus = manager.GetUserCouponBalanceStatus();

            if (null == usrCoupBalStatus)
            {
                // Critical error. Must not execute this code. This should be imposible case.
                return;
            }

            if (null != usrCoupBalStatus && !usrCoupBalStatus.IsReturnCoupon)
            {
                if (usrCoupBalStatus.WSStatus != HttpStatus.Success)
                {
                    // Call WS has HTTP error or timeout.
                    //var win = TODApp.Windows.MessageBox;
                    var win = TODApp.Windows.MessageBoxYesNo;
                    msg = "ไม่สามารถติดต่อกับ Toll Admin ได้ ต้องการนำส่งรายได้ ต่อ หรือไม่?";
                    med.Info("REVENUE ENTRY UI - " + msg); // Write log
                    win.Setup(msg, "DMT - Tour of Duty");
                    if (win.ShowDialog() == false)
                    {
                        // Write log
                        med.Info("REVENUE ENTRY UI - ยืนยันการดำเนินการนำส่งรายได้ (กรณีเรียก WS ของ TA APP timeout หรือ HTTP error).");
                        med.Info("     ผู้ใช้ยืนยัน: ไม่ดำเนินการต่อ");
                        return;
                    }
                    else
                    {
                        // allow to do revenue entry.
                        // Write log
                        med.Info("REVENUE ENTRY UI - ยืนยันการดำเนินการนำส่งรายได้ (กรณีเรียก WS ของ TA APP timeout หรือ HTTP error).");
                        med.Info("     ผู้ใช้ยืนยัน: ดำเนินการต่อ");
                    }
                }
                else
                {
                    // Call WS is success. But bag status is not complted.
                    var win = TODApp.Windows.MessageBox;
                    msg = "ระบบตรวจพบว่า ยังมีคูปองที่ยังคืนไม่ครบ กรุณาคืนคูปองก่อนป้อนรายได้";
                    med.Info("REVENUE ENTRY UI - " + msg); // Write log
                    win.Setup(msg, "DMT - Tour of Duty");
                    win.ShowDialog();
                    return;
                }
            }

            #endregion

            if (!manager.NewRevenueEntry())
            {
                var win = TODApp.Windows.MessageBox;
                msg = "ไม่พบข้อมูลกะรายได้ของพนักงาน หรือไม่พบข้อมูลที่เกี่ยวข้อง";
                med.Info("REVENUE ENTRY UI - " + msg); // Write log
                win.Setup(msg, "DMT - Tour of Duty");
                win.ShowDialog();
                return;
            }

            entry.Setup(manager); // Reset Context.

            // All check condition OK.
            tabs.SelectedIndex = 1; // goto next tab.
        }

        private void GotoPrintPreview()
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            med.Info("[<<<<   START PRINT PREVIEW   >>>>]");

            #region Check Has BagNo/BeltNo

            if (!entry.HasBagNo)
            {
                var win = TODApp.Windows.MessageBox;
                win.Setup("กรุณาระบุ หมายเลขถุงเงิน", "DMT - Tour of Duty");
                win.ShowDialog();

                entry.FocusBagNo();

                return;
            }
            // No need to enter BeltNo
            /*
            if (!entry.HasBeltNo)
            {
                var win = TODApp.Windows.MessageBox;
                win.Setup("กรุณาระบุ หมายเลขเข็มขัดนิรภัย", "DMT - Tour of Duty");
                win.ShowDialog();

                entry.FocusBeltNo();

                return;
            }
            */

            #endregion

            #region Check Valid and Duplicatate BagNo, BeltNo

            if (!entry.IsValidBagNo)
            {
                var win = TODApp.Windows.MessageBox;
                win.Setup("หมายเลขถุงเงิน ต้องเป็นตัวเลขเท่านั้น กรุณาตรวจสอบข้อมูล", "DMT - Tour of Duty");
                win.ShowDialog();

                entry.FocusBagNo();

                return;
            }
            if (!entry.IsValidBeltNo)
            {
                var win = TODApp.Windows.MessageBox;
                win.Setup("หมายเลขเข็มขัดนิรภัย ต้องเป็นตัวเลขเท่านั้น กรุณาตรวจสอบข้อมูล", "DMT - Tour of Duty");
                win.ShowDialog();

                entry.FocusBeltNo();

                return;
            }
            // New Requirement no need to check duplicate
            /*
            if (entry.HasDuplicateBagNo)
            {
                var win = TODApp.Windows.MessageBox;
                var oldHt = win.Height;
                win.Height += 50;
                win.Setup("ไม่สามารถใช้ หมายเลขถุงเงินซ้ำ ภายในวันเดียวกัน กรุณาเปลี่ยนเลขใหม่", "DMT - Tour of Duty");
                win.ShowDialog();
                win.Height = oldHt;

                entry.FocusBagNo();

                return;
            }
            if (entry.HasDuplicateBeltNo)
            {
                var win = TODApp.Windows.MessageBox;
                var oldHt = win.Height;
                win.Height += 50;
                win.Setup("ไม่สามารถใช้ หมายเลขเข็มขัดนิรภัยซ้ำ ภายในวันเดียวกัน กรุณาเปลี่ยนเลขใหม่", "DMT - Tour of Duty");
                win.ShowDialog();
                win.Height = oldHt;

                entry.FocusBeltNo();

                return;
            }
            */
            #endregion

            #region Check NonRevenue, Other Amount BHT

            if (!entry.IsValidOtherAmount)
            {
                var win = TODApp.Windows.MessageBox;
                var oldHt = win.Height;
                win.Height += 50;
                string msg = "ยอดรายได้อื่น" + Environment.NewLine;
                msg += "ต้องน้อยกว่า 1 ล้านบาท" + Environment.NewLine;
                msg += "กรุณาตรวจสอบ";
                win.Setup(msg, "DMT - Tour of Duty");
                win.ShowDialog();
                win.Height = oldHt;

                return;
            }

            if (!entry.IsValidNonRevenueAmount)
            {
                var win = TODApp.Windows.MessageBox;
                var oldHt = win.Height;
                win.Height += 50;
                string msg = "ยอดเงินรับฝาก" + Environment.NewLine;
                msg += "ต้องน้อยกว่า 1 ล้านบาท" + Environment.NewLine;
                msg += "กรุณาตรวจสอบ";
                win.Setup(msg, "DMT - Tour of Duty");
                win.ShowDialog();
                win.Height = oldHt;

                return;
            }

            #endregion

            // Update EndShift to current time.
            manager.UpdateEndShift(DateTime.Now);

            // Slip Preview
            if (!PrepareReport())
            {
                var win = TODApp.Windows.MessageBox;
                win.Setup("พบปัญหาในการเตรียมข้อมูล สำหรับจัดพิมพ์", "DMT - Tour of Duty");
                win.ShowDialog();
                return;
            }

            med.Info("[<<<<   END PRINT PREVIEW   >>>>]");

            // All OK so goto next tab.
            tabs.SelectedIndex = 2;
        }

        #endregion

        #region Load Masters

        private void LoadPlazaGroups()
        {
            cbPlazas.ItemsSource = null;
            if (null != manager && null != manager.Current)
            {
                cbPlazas.ItemsSource = manager.Current.TODPlazaGroups;
                if (null != manager.Current.TODPlazaGroups && manager.Current.TODPlazaGroups.Count > 0)
                {
                    cbPlazas.SelectedIndex = 0;
                }
            }
        }

        private void RefreshJobList()
        {
            grid.ItemsSource = null;

            var plazaGroup = cbPlazas.SelectedItem as PlazaGroup;
            if (null == manager || null == manager.UserShift || null == plazaGroup) return;

            // Refresh jobs.
            manager.Jobs.ViewMode = ViewModes.TOD; // Show by PlazaId in TOD config Only
            manager.Jobs.OnlyJobInShift = true;
            manager.Jobs.UserShift = manager.UserShift; // assign current user shift.
            manager.Jobs.PlazaGroup = plazaGroup; // assign selected plaza group.
            manager.Jobs.Refresh();
            // Bind to ListView
            grid.ItemsSource = manager.Jobs.PlazaGroupJobs;
        }

        #endregion

        #region Reset

        private void Reset()
        {
            // Reset Bindings On Tab - Date Selection.
            txtRevDate.DataContext = null;
            txtEntryDate.DataContext = null;
            // Reset Bindings On Tab - Revenue Entry.
            txtPlazaName.DataContext = null;
            txtShiftName.DataContext = null;
            txtRevDate2.DataContext = null;
            txtUserId2.DataContext = null;
            txtUserName2.DataContext = null;

            // Reset Plaza.
            cbPlazas.SelectedIndex = -1;
            LoadPlazaGroups();

            // Setup entry date.
            manager.EntryDate = DateTime.Now;

            // Load Job.
            RefreshJobList();

            // Set Bindings On Tab - Date Selection.
            txtRevDate.DataContext = manager;
            txtEntryDate.DataContext = manager;
            // Set Bindings On Tab - Revenue Entry.
            txtPlazaName.DataContext = manager;
            txtShiftName.DataContext = manager;
            txtRevDate2.DataContext = manager;
            txtUserId2.DataContext = manager;
            txtUserName2.DataContext = manager;
        }

        #endregion

        #region Report methods

        private string ReportDisplayName
        {
            get { return "revenue." + DateTime.Now.ToThaiDateTimeString("ddMMyyyyHHmmssfff"); }
        }

        private bool PrepareReport()
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            if (null == manager || !manager.CanBuildRevenueSlipReport) return false;

            med.Info("<<<<<<<<<  Generate Revenue Slip Report Model  >>>>>>>>>");
            var model = manager.GetRevenueSlipReportModel();

            if (null == model ||
                null == model.DataSources || model.DataSources.Count <= 0 ||
                null == model.DataSources[0] || null == model.DataSources[0].Items)
            {
                var win = TODApp.Windows.MessageBox;
                win.Setup("ไม่พบข้อมูลในการจัดพิมพ์รายงาน.", "DMT - Tour of Duty");
                win.ShowDialog();
                this.rptViewer.ClearReport();
            }
            else
            {
                // Set Display Name (default file name).
                model.DisplayName = ReportDisplayName;

                try
                {
                    Dispatcher.Invoke(() =>
                    {
                        this.rptViewer.LoadReport(model);
                    }, DispatcherPriority.Background);
                }
                catch (Exception ex)
                {
                    med.Err(ex);
                }
            }

            return true;
        }

        private void PrintReport()
        {
            if (null == manager || null == manager.Entry)
            {
                var win = TODApp.Windows.MessageBox;
                win.Setup("Revenue Entry is not found.", "DMT - Tour of Duty");
                win.ShowDialog();
                return;
            }

            if (null == manager.Entry ||
                !manager.Entry.RevenueDate.HasValue ||
                manager.Entry.RevenueDate.Value == DateTime.MinValue ||
                !manager.Entry.EntryDate.HasValue ||
                manager.Entry.EntryDate.Value == DateTime.MinValue)
            {
                var win = TODApp.Windows.MessageBox;
                win.Setup("Entry Date or Revenue Date is not set.", "DMT - Tour of Duty");
                win.ShowDialog();
                return;
            }

            MethodBase med = MethodBase.GetCurrentMethod();

            cmdOk.Visibility = Visibility.Collapsed; // Hide button.

            bool hasActivitied = manager.SaveRevenueEntry();

            if (manager.Entry.RevenueDate.HasValue && manager.Entry.RevenueDate.Value != DateTime.MinValue &&
                manager.Entry.EntryDate.HasValue && manager.Entry.EntryDate.Value != DateTime.MinValue)
            {
                try
                {
                    // print reports only date exists.
                    this.rptViewer.Print(ReportDisplayName);
                }
                catch (Exception ex)
                {
                    med.Err(ex);
                }
            }

            cmdOk.Visibility = Visibility.Visible; // Show button

            if (!hasActivitied || null == manager.User)
            {
                GotoMainMenu();
            }
            else
            {
                // Still has more jobs on another Plaza Group.
                var win = TODApp.Windows.MessageBoxYesNo;
                win.Setup("กะปัจจุบันยังป้อนรายได้ไม่ครบ ต้องการป้อนรายได้ต่อหรือไม่ ?", "DMT - Tour of Duty");
                if (win.ShowDialog() == true)
                {
                    var user = manager.User;
                    Setup(user); // Goback to first page.
                }
                else
                {
                    GotoMainMenu();
                }
            }
        }

        #endregion

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        /// <param name="user"></param>
        public void Setup(User user)
        {
            if (null == manager)
            {
                manager = new RevenueEntryManager();
            }

            tabs.SelectedIndex = 0;
            cmdOk.Visibility = Visibility.Visible;

            _user = user;
            if (null != _user)
            {

            }

            if (null != manager)
            {
                manager.User = null; // Reset User
                manager.ByChief = false;
                // Refresh.
                manager.Refresh();

                manager.User = user;
            }

            Reset();
        }

        #endregion
    }
}
