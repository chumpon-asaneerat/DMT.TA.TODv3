#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using DMT.Models;
using DMT.Services;
using System.Collections.ObjectModel;
using NLib.Reflection;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using System.Runtime.InteropServices;

#endregion

namespace DMT.Config.Pages
{
    using todops = Services.Operations.TOD.Shift; // reference to static class.
    using taaops = Services.Operations.TA.Shift; // reference to static class.

    /// <summary>
    /// Interaction logic for ShiftViewPage.xaml
    /// </summary>
    public partial class ShiftViewPage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public ShiftViewPage()
        {
            InitializeComponent();
        }

        #endregion

        #region Loaded/Unloaded

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshList();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region ListView Handler

        private void listViewTOD_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            pgridTOD.SelectedObject = listViewTOD.SelectedItem;
        }

        private void listViewTAA_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            pgridTAA.SelectedObject = listViewTAA.SelectedItem;
        }

        #endregion

        #region Button Handlers

        private void cmdSaveTOD_Click(object sender, RoutedEventArgs e)
        {
            SaveShiftTOD();
        }

        private void cmdSaveTAA_Click(object sender, RoutedEventArgs e)
        {
            SaveShiftTAA();
        }

        #endregion

        #region Private Methods

        private void RefreshListTOD()
        {
            var shifts = todops.Gets().Value();
            listViewTOD.ItemsSource = shifts;
        }

        private void RefreshListTAA()
        {
            var shifts = taaops.Gets().Value();
            listViewTAA.ItemsSource = shifts;
        }

        private void RefreshList()
        {
            RefreshListTOD();
            RefreshListTAA();
        }

        private void SaveShiftTOD()
        {
            var value = (pgridTOD.SelectedObject as Shift);
            if (null == value)
            {
                MessageBox.Show("No item selected");
                return;
            }
            //todops
            MessageBox.Show("TOD Application WS not implements Shift's Save operation");
        }

        private void SaveShiftTAA()
        {
            var value = (pgridTAA.SelectedObject as Shift);
            if (null == value)
            {
                MessageBox.Show("No item selected");
                return;
            }
            //taaops
            MessageBox.Show("TA Application WS not implements Shift's Save operation");
        }

        #endregion
    }
}
