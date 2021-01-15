#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    using taaops = Services.Operations.TA.Infrastructure; // reference to static class.
    using todops = Services.Operations.TOD.Infrastructure; // reference to static class.

    /// <summary>
    /// Interaction logic for TSBViewPage.xaml
    /// </summary>
    public partial class TSBViewPage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public TSBViewPage()
        {
            InitializeComponent();
        }

        #endregion
        
        private List<TSBItem> items = new List<TSBItem>();

        #region Loaded/Unloaded

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshTree();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
        }

        #endregion

        private void RefreshTree()
        {
            /*
            tree.ItemsSource = null;

            items.Clear();
            var tsbs = ops.TSB.Gets().Value();
            if (null == tsbs)
                return;

            tsbs.ForEach(tsb =>
            {
                var tItem = new TSBItem(tsb);
                items.Add(tItem);

                var plazas = ops.Plaza.Search.ByTSB(tsb).Value();
                if (null != plazas)
                {
                    plazas.ForEach(plaza =>
                    {
                        var pItem = new PlazaItem(plaza);
                        tItem.Plazas.Add(pItem);

                        var lanes = ops.Lane.Search.ByPlaza(plaza).Value();
                        if (null != lanes)
                        {
                            lanes.ForEach(lane =>
                            {
                                var lItem = new LaneItem(lane);
                                pItem.Lanes.Add(lItem);
                            });
                        }
                    });
                }
            });

            tree.ItemsSource = items;
            */
        }

        #region Button Handler

        private void cmdSetActiveTSB_Click(object sender, RoutedEventArgs e)
        {
            // Set Active.
            /*
            var item = (sender as Button).DataContext;
            if (null != item && item is TSBItem)
            {
                ops.TSB.SetActive(item as TSB);
                RefreshTree();
            }
            */
        }

        #endregion

        #region TreeView Handler

        private void tree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            pgrid.SelectedObject = e.NewValue;
        }

        #endregion

        private void SaveTSB()
        {
            /*
            var value = (pgrid.SelectedObject as TSB);
            if (null != value)
            {
                var ret = ops.TSB.Save(value);
                if (ret.Failed)
                {
                    MessageBox.Show("Save TSB Error.");
                }
                else
                {
                    MessageBox.Show("Save TSB Success.");
                    RefreshTree();
                }
            }
            */
        }

        private void SavePlaza()
        {
            /*
            var value = (pgrid.SelectedObject as Plaza);
            if (null != value)
            {
                var ret = ops.Plaza.Save(value);
                if (ret.Failed)
                {
                    MessageBox.Show("Save Plaza Error.");
                }
                else
                {
                    MessageBox.Show("Save Plaza Success.");
                    RefreshTree();
                }
            }
            */
        }

        private void SaveLane()
        {
            /*
            var value = (pgrid.SelectedObject as Lane);
            if (null != value)
            {
                var ret = ops.Lane.Save(value);
                if (ret.Failed)
                {
                    MessageBox.Show("Save Lane Error.");
                }
                else
                {
                    MessageBox.Show("Save Lane Success.");
                    RefreshTree();
                }
            }
            */
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            /*
            if (null == pgrid.SelectedObject) return;
            if (pgrid.SelectedObject is TSBItem) SaveTSB();
            if (pgrid.SelectedObject is PlazaItem) SavePlaza();
            if (pgrid.SelectedObject is LaneItem) SaveLane();
            */
        }

        private void cmdChangeActiveTSB_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // Set Active.
            /*
            var item = (e.OriginalSource as Button).DataContext;
            if (null != item && item is TSBItem)
            {
                ops.TSB.SetActive(item as TSB);
                RefreshTree();
            }
            */
        }

        private void cmdChangeActiveTSB_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
    }
}
