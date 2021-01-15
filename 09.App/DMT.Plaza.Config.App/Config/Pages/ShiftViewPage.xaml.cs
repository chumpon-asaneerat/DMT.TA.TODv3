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
    using taaops = Services.Operations.TA.Shift; // reference to static class.
    using todops = Services.Operations.TOD.Shift; // reference to static class.

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

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            pgrid.SelectedObject = listView.SelectedItem;
        }

        #endregion

        private void RefreshList()
        {
            /*
            var shifts = ops.Gets().Value();
            listView.ItemsSource = shifts;
            */
        }
    }
}
