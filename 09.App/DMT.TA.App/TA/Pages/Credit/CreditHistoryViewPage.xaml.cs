﻿#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;

using DMT.Configurations;
using DMT.Models;
using DMT.Services;
using DMT.Controls;

using NLib.Services;
using NLib.Reflection;

#endregion

namespace DMT.TA.Pages.Credit
{
    /// <summary>
    /// Interaction logic for CreditHistoryViewPage.xaml
    /// </summary>
    public partial class CreditHistoryViewPage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public CreditHistoryViewPage()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        //private CultureInfo culture = new CultureInfo("th-TH") { DateTimeFormat = { Calendar = new ThaiBuddhistCalendar() } };
        private CultureInfo culture = new CultureInfo("th-TH");
        private XmlLanguage language = XmlLanguage.GetLanguage("th-TH");

        private UserCreditTransactionManager manager = null;

        #endregion

        #region Loaded/Unloaded

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Setup DateTime Picker.
            dtEntryDate.CultureInfo = culture;
            dtEntryDate.Language = language;
            //Thread.CurrentThread.CurrentCulture = culture;
            //Thread.CurrentThread.CurrentUICulture = culture;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region Button Handlers

        private void cmdBack_Click(object sender, RoutedEventArgs e)
        {
            GotoMainMenu();
        }

        private void cmdCreditSearch_Click(object sender, RoutedEventArgs e)
        {
            RefreshCreditTransactions();
        }

        private void cmdUserSearch_Click(object sender, RoutedEventArgs e)
        {
            SearchUser();
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            CancelTransaction();
        }

        #endregion

        #region DateTime Piecker Handelers

        private void dtEntryDate_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            RefreshCreditTransactions();
        }

        #endregion

        #region TextBox Handlers

        private void txtSearchUserId_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                SearchUser();
                e.Handled = true;
            }
            else if (e.Key == Key.Escape)
            {
                ResetSelectUser();
                RefreshCreditTransactions();
                e.Handled = true;
            }
        }

        #endregion

        #region ListView Handlers

        private void grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateSelection();
        }

        #endregion

        #region Private Methods

        #region Navigate Methods

        private void GotoMainMenu()
        {
            // Main Menu Page
            var page = TAApp.Pages.MainMenu;
            PageContentManager.Instance.Current = page;
        }

        #endregion

        #region Resets and Search User method

        private void Reset()
        {
            manager.SetUser(null);

            // Date Picker
            dtEntryDate.DefaultValue = DateTime.Now;
            dtEntryDate.Value = DateTime.Now.Date;
            // Set Bindings User Selection.
            txtUserId.DataContext = manager;
            txtUserName.DataContext = manager;
            // Entry
            entry.DataContext = null;
            // Reset List view
            grid.ItemsSource = null;
        }

        private void ResetSelectUser()
        {
            manager.SetUser(null);
            txtSearchUserId.Text = string.Empty;
        }

        private void SearchUser()
        {
            string userId = txtSearchUserId.Text.Trim();
            var result = TAAPI.SearchUser(userId, TAApp.Permissions.TC);
            if (!result.IsCanceled && null != manager)
            {
                manager.SetUser(result.User);
                if (null != manager.User)
                {
                    txtSearchUserId.Text = string.Empty;
                }
                RefreshCreditTransactions();
            }
        }

        #endregion

        #region Credit Transaction methods

        private void RefreshCreditTransactions()
        {
            grid.ItemsSource = null;

            if (!dtEntryDate.Value.HasValue)
            {
                dtEntryDate.Focus();
                return;
            }

            if (null == manager) return;
            // Update Search Date.
            manager.SearchDate = dtEntryDate.Value;
            // Refresh
            manager.Refresh();

            grid.ItemsSource = manager.Transactions;
        }

        private void UpdateSelection()
        {
            entry.DataContext = null;
            if (null == grid.ItemsSource || null == grid.SelectedItem) return;
            var item = grid.SelectedItem as UserCreditTransaction;
            if (null == item) return;
            item.Description = "รายการ " + item.TransactionTypeString;
            item.HasRemark = true;
            entry.DataContext = item;
        }

        private void CancelTransaction()
        {
            if (null == grid.ItemsSource || null == grid.SelectedItem) return;
            var item = grid.SelectedItem as UserCreditTransaction;
            if (null == item) return;

            string msg1 = "ยืนยันการยกเลิก รายการ ยืม/คืน เงินทอน";
            string usr = item.FullNameTH;
            string amt = item.BHTTotal.ToString("n0");

            var win = TAApp.Windows.MessageBoxYesNo1;
            win.Setup(msg1, usr, amt, "DMT - Toll Admin");
            if (win.ShowDialog() == true)
            {
                manager.CancelTransaction(item);
                RefreshCreditTransactions();
            }
        }

        #endregion

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        public void Setup()
        {
            if (null == manager)
            {
                manager = new UserCreditTransactionManager();
            }

            Reset();
            ResetSelectUser();
            // Refresh List View.
            RefreshCreditTransactions();

            // Focus on search textbox.
            Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                txtSearchUserId.SelectAll();
                txtSearchUserId.Focus();
            }));
        }

        #endregion
    }
}
