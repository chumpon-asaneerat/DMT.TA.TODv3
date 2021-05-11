#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Text;

using DMT.Models;

#endregion

namespace Wpf.Dynamic.Entry.Sample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Button Handlers

        private void cmdGetMaster_Click(object sender, RoutedEventArgs e)
        {
            gridMaster.ItemsSource = MCurrency.GetCurrencies();
            // auto change tab
            tabs.SelectedIndex = 0;
        }

        private void cmdGetDetails_Click(object sender, RoutedEventArgs e)
        {
            //entry.DataContext = Detail.GetDetails();
            entry.DataContext = Detail.GetSamples();
        }

        private void cmdRetriveData_Click(object sender, RoutedEventArgs e)
        {
            List<Detail> details = entry.DataContext as List<Detail>;
            gridDetail.ItemsSource = details.Compact();
            // auto change tab
            tabs.SelectedIndex = 1;
        }

        #endregion
    }
}
