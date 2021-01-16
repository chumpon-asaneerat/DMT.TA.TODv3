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
        
        private List<TSBItem> TODitems = new List<TSBItem>();
        private List<TSBItem> TAAitems = new List<TSBItem>();

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

        private void cmdChangeActiveTSB_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void cmdChangeActiveTSB_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // Set Active.
            var item = (e.OriginalSource as Button).DataContext;
            if (null != item && item is TSBItem)
            {
                if (tabs.SelectedIndex == 0)
                    todops.TSB.SetActive(item as TSB);
                else taaops.TSB.SetActive(item as TSB);
                RefreshTree();
            }
        }

        private void cmdSetActiveTSB_Click(object sender, RoutedEventArgs e)
        {
            // Set Active.
            var item = (sender as Button).DataContext;
            if (null != item && item is TSBItem)
            {
                if (tabs.SelectedIndex == 0)
                    todops.TSB.SetActive(item as TSB);
                else taaops.TSB.SetActive(item as TSB);
                RefreshTree();
            }
        }

        #region TOD

        private void cmdSaveTOD_Click(object sender, RoutedEventArgs e)
        {
            if (null == pgridTOD.SelectedObject) return;
            if (tabs.SelectedIndex != 0) return;
                if (pgridTOD.SelectedObject is TSBItem) SaveTSBTOD();
            if (pgridTOD.SelectedObject is PlazaItem) SavePlazaTOD();
            if (pgridTOD.SelectedObject is LaneItem) SaveLaneTOD();
        }

        #endregion

        #region TAA

        private void cmdSaveTAA_Click(object sender, RoutedEventArgs e)
        {
            if (null == pgridTAA.SelectedObject) return;
            if (tabs.SelectedIndex != 1) return;
            if (pgridTAA.SelectedObject is TSBItem) SaveTSBTAA();
            if (pgridTAA.SelectedObject is PlazaItem) SavePlazaTAA();
            if (pgridTAA.SelectedObject is LaneItem) SaveLaneTAA();
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
            treeTOD.ItemsSource = null;

            TODitems.Clear();
            var tsbs = todops.TSB.Gets().Value();
            if (null == tsbs)
                return;

            tsbs.ForEach(tsb =>
            {
                var tItem = new TSBItem(tsb);
                TODitems.Add(tItem);

                var plazas = todops.Plaza.Search.ByTSB(tsb).Value();
                if (null != plazas)
                {
                    plazas.ForEach(plaza =>
                    {
                        var pItem = new PlazaItem(plaza);
                        tItem.Plazas.Add(pItem);

                        var lanes = todops.Lane.Search.ByPlaza(plaza).Value();
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

            treeTOD.ItemsSource = TODitems;
        }

        private void SaveTSBTOD()
        {
            var value = (pgridTOD.SelectedObject as TSB);
            if (null == value)
            {
                MessageBox.Show("No item selected");
                return;
            }
            var ret = todops.TSB.Save(value);
            if (ret.Failed)
            {
                MessageBox.Show("TOD Application WS Save TSB Error.");
            }
            else
            {
                MessageBox.Show("TOD Application WS Save TSB Success.");
                RefreshTree();
            }
        }

        private void SavePlazaTOD()
        {
            var value = (pgridTOD.SelectedObject as Plaza);
            if (null == value)
            {
                MessageBox.Show("No item selected");
                return;
            }
            var ret = todops.Plaza.Save(value);
            if (ret.Failed)
            {
                MessageBox.Show("TOD Application WS Save Plaza Error.");
            }
            else
            {
                MessageBox.Show("TOD Application WS Save Plaza Success.");
                RefreshTree();
            }
        }

        private void SaveLaneTOD()
        {
            var value = (pgridTOD.SelectedObject as Lane);
            if (null == value)
            {
                MessageBox.Show("No item selected");
                return;
            }
            var ret = todops.Lane.Save(value);
            if (ret.Failed)
            {
                MessageBox.Show("TOD Application WS Save Lane Error.");
            }
            else
            {
                MessageBox.Show("TOD Application WS Save Lane Success.");
                RefreshTree();
            }
        }

        #endregion

        #region TAA

        private void RefreshTAA()
        {
            treeTAA.ItemsSource = null;

            TAAitems.Clear();
            var tsbs = taaops.TSB.Gets().Value();
            if (null == tsbs)
                return;

            tsbs.ForEach(tsb =>
            {
                var tItem = new TSBItem(tsb);
                TAAitems.Add(tItem);

                var plazas = taaops.Plaza.Search.ByTSB(tsb).Value();
                if (null != plazas)
                {
                    plazas.ForEach(plaza =>
                    {
                        var pItem = new PlazaItem(plaza);
                        tItem.Plazas.Add(pItem);

                        var lanes = taaops.Lane.Search.ByPlaza(plaza).Value();
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

            treeTAA.ItemsSource = TAAitems;
        }

        private void SaveTSBTAA()
        {
            var value = (pgridTAA.SelectedObject as TSB);
            if (null == value)
            {
                MessageBox.Show("No item selected");
                return;
            }
            var ret = taaops.TSB.Save(value);
            if (ret.Failed)
            {
                MessageBox.Show("TOD Application WS Save TSB Error.");
            }
            else
            {
                MessageBox.Show("TOD Application WS Save TSB Success.");
                RefreshTree();
            }
        }

        private void SavePlazaTAA()
        {
            var value = (pgridTAA.SelectedObject as Plaza);
            if (null == value)
            {
                MessageBox.Show("No item selected");
                return;
            }
            var ret = taaops.Plaza.Save(value);
            if (ret.Failed)
            {
                MessageBox.Show("TOD Application WS Save Plaza Error.");
            }
            else
            {
                MessageBox.Show("TOD Application WS Save Plaza Success.");
                RefreshTree();
            }
        }

        private void SaveLaneTAA()
        {
            var value = (pgridTAA.SelectedObject as Lane);
            if (null == value)
            {
                MessageBox.Show("No item selected");
                return;
            }
            var ret = taaops.Lane.Save(value);
            if (ret.Failed)
            {
                MessageBox.Show("TOD Application WS Save Lane Error.");
            }
            else
            {
                MessageBox.Show("TOD Application WS Save Lane Success.");
                RefreshTree();
            }
        }

        #endregion

        #endregion
    }
}
