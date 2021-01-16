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
    using todops = Services.Operations.TOD.Infrastructure; // reference to static class.
    using taaops = Services.Operations.TA.Infrastructure; // reference to static class.

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

        #region Button Handler

        #region TOD

        private void cmdChangeActiveTSBTOD_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void cmdChangeActiveTSBTOD_Executed(object sender, ExecutedRoutedEventArgs e)
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

        private void cmdSetActiveTSBTOD_Click(object sender, RoutedEventArgs e)
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

        private void cmdSaveTOD_Click(object sender, RoutedEventArgs e)
        {
            /*
            if (null == pgrid.SelectedObject) return;
            if (pgrid.SelectedObject is TSBItem) SaveTSB();
            if (pgrid.SelectedObject is PlazaItem) SavePlaza();
            if (pgrid.SelectedObject is LaneItem) SaveLane();
            */
        }

        #endregion

        #region TAA

        private void cmdChangeActiveTSBTAA_Executed(object sender, ExecutedRoutedEventArgs e)
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

        private void cmdChangeActiveTSBTAA_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void cmdSetActiveTSBTAA_Click(object sender, RoutedEventArgs e)
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

        private void cmdSaveTAA_Click(object sender, RoutedEventArgs e)
        {
            /*
            if (null == pgrid.SelectedObject) return;
            if (pgrid.SelectedObject is TSBItem) SaveTSB();
            if (pgrid.SelectedObject is PlazaItem) SavePlaza();
            if (pgrid.SelectedObject is LaneItem) SaveLane();
            */
        }

        #endregion

        #endregion

        #region TreeView Handler

        private void treeTOD_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            pgridTOD.SelectedObject = e.NewValue;
        }

        private void treeTAA_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            pgridTAA.SelectedObject = e.NewValue;
        }

        #endregion

        #region Private Methods

        private void RefreshTree()
        {
            RefreshTOD();
            RefreshTAA();
        }

        #region TOD

        private void RefreshTOD()
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

        private void SaveTSBTOD()
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

        private void SavePlazaTOD()
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

        private void SaveLaneTOD()
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

        #endregion

        #region TAA

        private void RefreshTAA()
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

        private void SaveTSBTAA()
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

        private void SavePlazaTAA()
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

        private void SaveLaneTAA()
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

        #endregion

        #endregion
    }
}
