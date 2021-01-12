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
    /// <summary>
    /// Interaction logic for TSBCreditViewPage.xaml
    /// </summary>
    public partial class TSBCreditViewPage : UserControl
    {
        #region Constructor

        public TSBCreditViewPage()
        {
            InitializeComponent();
        }

        #endregion

        private LocalOperations ops = LocalServiceOperations.Instance.Plaza;
        private TSBCreditInitManager initMgr = new TSBCreditInitManager();

        #region Loaded/Unloaded

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshTSBs();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region ListView Handlers

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = listView.SelectedItem as TSB;
            pgrid.SelectedObject = null;
            if (null == item) return;

            initMgr.TSB = item;
            initMgr.Refresh();
            pgrid.SelectedObject = initMgr.Current;
        }

        #endregion

        #region Button Handlers

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            var item = pgrid.SelectedObject as TSBCreditTransaction;
            if (null == item) return;
            ops.Credits.SaveTSBCreditTransaction(item);
            // clear
            pgrid.SelectedObject = null;
        }

        #endregion

        private void RefreshTSBs()
        {
            listView.ItemsSource = null;

            var ret = ops.TSB.GetTSBs();
            if (null == ret || ret.errors.hasError)
                return;

            var tsbs = ret.data;
            listView.ItemsSource = tsbs;
        }
    }
}
