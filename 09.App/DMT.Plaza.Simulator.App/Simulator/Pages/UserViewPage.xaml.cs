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

namespace DMT.Simulator.Pages
{
    /// <summary>
    /// Interaction logic for UserViewPage.xaml
    /// </summary>
    public partial class UserViewPage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public UserViewPage()
        {
            InitializeComponent();
        }

        #endregion

        public class UserItem : User
        {
            private UserShift _Shift = null;

            [Category("Shift")]
            [Browsable(false)]
            public UserShift Shift
            {
                get { return _Shift; }
                set
                {
                    _Shift = value;
                    RaiseChanged("BeginDateString");
                    RaiseChanged("BeginTimeString");
                }
            }

            [Category("Shift")]
            [ReadOnly(true)]
            public string BeginDateString
            {
                get { return (null != Shift) ? Shift.BeginDateString : string.Empty; }
                set { }
            }
            [Category("Shift")]
            [ReadOnly(true)]
            public string BeginTimeString
            {
                get { return (null != Shift) ? Shift.BeginTimeString : string.Empty; }
                set { }
            }
        }

        public class LaneItem : Lane
        {
            [Category("User")]
            [ReadOnly(true)]
            public string UserId { get; set; }
            [Category("User")]
            [ReadOnly(true)]
            public string FullNameTH { get; set; }
        }

        private LocalOperations ops = LocalServiceOperations.Instance.Plaza;

        private List<UserItem> users = new List<UserItem>();

        #region Loaded/Unloaderd

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshUsers();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region Private Methods

        private void RefreshUsers()
        {
            lstUsers.ItemsSource = null;

            users.Clear();

            var role = ops.Users.GetRole(Search.Roles.ById.Create("TC")).Value();
            if (null != role)
            {
                var usrs = ops.Users.GetUsers(role).Value();
                if (null != usrs)
                {
                    usrs.ForEach(usr =>
                    {
                        var inst = new UserItem();
                        usr.AssignTo(inst);
                        users.Add(inst);
                    });
                }
            }
            lstUsers.ItemsSource = users;
        }

        private void RefreshUserShifts(UserItem user)
        {
            lvUserShifts.ItemsSource = null;

            if (null == user) return;
            var userShifts = ops.UserShifts.GetUserShifts(user).Value();

            lvUserShifts.ItemsSource = userShifts;
        }

        #endregion

        #region ListBox Handler(s)

        private void lstUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = lstUsers.SelectedItem as UserItem;
            pgrid.SelectedObject = item;

            RefreshUserShifts(item);
        }

        #endregion

        #region Button Handler(s)

        #endregion
    }
}
