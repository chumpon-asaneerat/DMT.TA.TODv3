#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

using DMT.Models;
using DMT.Services;
using NLib.Services;
using NLib.Reflection;
using System.Threading;

#endregion

namespace DMT.TOD.Pages.Revenue
{
    /// <summary>
    /// Interaction logic for ChiefRevenueEntryPage.xaml
    /// </summary>
    public partial class ChiefRevenueEntryPage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public ChiefRevenueEntryPage()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        //private CultureInfo culture = new CultureInfo("th-TH") { DateTimeFormat = { Calendar = new ThaiBuddhistCalendar() } };
        private CultureInfo culture = new CultureInfo("th-TH");

        private User _user = null; // Supervisor
        private User _selectUser = null; // Collector

        private TSB _tsb = null;
        private List<PlazaGroup> _plazaGroups = null;
        private List<Models.Shift> _shifts = null;

        private UserShift _userShift = null;
        private UserShift _revenueShift = null;

        #endregion

        #region Loaded

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Setup DateTime Picker.
            dtEntryDate.CultureInfo = culture;
            dtRevDate.CultureInfo = culture;
            //Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        #endregion

        #region Combobox Handlers

        private void cbPlazas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var plazaGroup = cbPlazas.SelectedItem as PlazaGroup;
            if (null == plazaGroup) return;
        }

        private void cbShifts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var shift = cbShifts.SelectedItem as Models.Shift;
            if (null == shift) return;
        }

        #endregion

        #region Button Handlers

        private void cmdBack_Click(object sender, RoutedEventArgs e)
        {
            GotoMainMenu();
        }

        private void cmdGotoRevenueEntry_Click(object sender, RoutedEventArgs e)
        {
            tabs.SelectedIndex = 1; // goto next tab.
        }

        private void cmdBack2_Click(object sender, RoutedEventArgs e)
        {
            GotoBack();
        }

        private void cmdGotoRevenueEntryPreview_Click(object sender, RoutedEventArgs e)
        {
            tabs.SelectedIndex = 2; // goto next tab.
        }

        private void cmdBack3_Click(object sender, RoutedEventArgs e)
        {
            GotoBack();
        }

        private void cmdOk_Click(object sender, RoutedEventArgs e)
        {
            // Printing.

            // Go Back to Main Menu.
            GotoMainMenu();
        }

        #endregion

        #region Private Methods

        private void GotoMainMenu()
        {
            // Main Menu Page
            var page = new Menu.MainMenu();
            PageContentManager.Instance.Current = page;
        }

        private void GotoBack()
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

        private void Reset()
        {
            // Reset Plaza.
            cbPlazas.SelectedIndex = -1;
            LoadPlazaGroups();
            // Reset Shift.
            cbShifts.SelectedIndex = -1;
            LoadShifts();
            // Update entry date and revenue date.
            dtEntryDate.Value = DateTime.Now;
            dtRevDate.Value = DateTime.Now;
        }

        private void LoadShifts() 
        {
            cbPlazas.ItemsSource = null;
            if (null != _shifts)
            {
                cbPlazas.ItemsSource = _shifts;
                if (_shifts.Count > 0) cbShifts.SelectedIndex = 0;
            }
            LoadLanes();
        }

        private void LoadPlazaGroups() 
        {
            cbPlazas.ItemsSource = null;
            if (null != _plazaGroups)
            {
                cbPlazas.ItemsSource = _plazaGroups;
                if (_plazaGroups.Count > 0) cbPlazas.SelectedIndex = 0;
            }
            LoadLanes();
        }

        private void LoadLanes()
        {
            _userShift = null;
            _revenueShift = null;
            var plazaGroup = cbPlazas.SelectedItem as PlazaGroup;
            var shift = cbShifts.SelectedItem as Models.Shift;
            if (null == plazaGroup || null == shift)
            {
                return;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        /// <param name="user"></param>
        public void Setup(User user)
        {
            _user = user;
            if (null != _user)
            {
                _shifts = Models.Shift.GetShifts().Value();

                _tsb = TSB.GetCurrent().Value();
                if (null != _tsb)
                {
                    _plazaGroups = PlazaGroup.GetTSBPlazaGroups(_tsb).Value();
                }
            }
            Reset();
        }

        #endregion
    }
}
