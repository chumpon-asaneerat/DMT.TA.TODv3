#region Using

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using DMT.Models;
using DMT.Services;
using NLib.Services;
using NLib.Reflection;

#endregion

namespace DMT.TA.Windows.Reservation
{
    /// <summary>
    /// Interaction logic for NewReservationWindow.xaml
    /// </summary>
    public partial class NewReservationWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public NewReservationWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Button Handlers

        private void cmdOk_Click(object sender, RoutedEventArgs e)
        {
            SaveReservationToQueue();
            DialogResult = true;
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        #endregion

        #region Private Methods

        private void LoadComboboxs()
        {

        }

        private void SaveReservationToQueue()
        {

        }

        #endregion

        #region Public Methods

        public void Setup()
        {
            LoadComboboxs();
        }

        #endregion
    }
}
