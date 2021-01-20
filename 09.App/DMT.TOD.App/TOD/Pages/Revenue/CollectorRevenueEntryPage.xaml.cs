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

#endregion

namespace DMT.TOD.Pages.Revenue
{
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
        private CultureInfo culture = new CultureInfo("th-TH");

        private User _user = null;
        private TSB _tsb = null;
        private List<PlazaGroup> _plazaGroups = null;

        private DateTime _entryDate = DateTime.Now;
        private DateTime _revDate = DateTime.Now;

        private UserShift _userShift = null;
        private UserShift _revenueShift = null;

        #endregion

        #region Loaded

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region Combobox Handlers

        private void cbPlazas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var plazaGroup = cbPlazas.SelectedItem as PlazaGroup;
            if (null == plazaGroup) return;
            LoadLanes();
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
            GotoMainMenu();
            // Used below code if need to go back to select date.
            //GotoBack();
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
            // Update entry date and revenue date.
            _entryDate = DateTime.Now;
            _revDate = DateTime.Now;
            txtEntryDate.Text = _entryDate.ToThaiDateTimeString("dd/MM/yyyy HH:mm:ss");
            txtRevDate.Text = _revDate.ToThaiDateTimeString("dd/MM/yyyy");
        }

        private void LoadPlazaGroups()
        {
            cbPlazas.ItemsSource = null;
            if (null != _plazaGroups)
            {
                cbPlazas.ItemsSource = _plazaGroups;
                if (_plazaGroups.Count > 0) cbPlazas.SelectedIndex = 0;
            }
        }

        private void LoadLanes()
        {
            _userShift = null;
            _revenueShift = null;
            var plazaGroup = cbPlazas.SelectedItem as PlazaGroup;
            if (null == plazaGroup)
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
