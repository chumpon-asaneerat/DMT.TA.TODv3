#region Using

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using DMT.Models;
using DMT.Services;
using NLib.Services;
using NLib.Reflection;
using System.Linq;

#endregion

namespace DMT.TA.Windows.Reservation
{
    using ops = DMT.Services.Operations.TAxTOD.SAP2;

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

        private Storagelocation storage;
        private ReserveRunningNo runningNo;

        private void LoadMasterData()
        {
            var tsb = TSB.GetCurrent().Value();
            var year = DateTime.Now.Year.ToString();
            storage = ops.GetStorageLocations(tsb.TSBId).Value().FirstOrDefault();
            Console.WriteLine(storage);
            runningNo = ops.GetReservationCurrentRunningNo(tsb.TSBId, year).Value().FirstOrDefault();
            Console.WriteLine(runningNo);
        }

        private void LoadComboboxs()
        {
            var couponTypes = ops.GetCouponMasters().Value();
            cbCouponMasters.ItemsSource = couponTypes;
        }

        private void SaveReservationToQueue()
        {

        }

        #endregion

        #region Public Methods

        public void Setup()
        {
            LoadMasterData();
            LoadComboboxs();
        }

        #endregion
    }
}
