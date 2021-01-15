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
    using todops = Services.Operations.TOD.Security; // reference to static class.
    using taaops = Services.Operations.TA.Security; // reference to static class.

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

        private List<RoleItem> items = new List<RoleItem>();

        #region Loaded/Unloaded

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshTree();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region Private Methods

        private void RefreshTreeTOD()
        {
            treeTOD.ItemsSource = null;

            items.Clear();
            /*
            var roles = ops.Role.Gets().Value();
            if (null != roles)
            {
                roles.ForEach(role => 
                {
                    RoleItem item = role.CloneTo<RoleItem>();
                    items.Add(item);

                    var search = Search.User.ByRoleId.Create(role.RoleId);
                    var users = ops.User.Search.ByRoleId(search).Value();
                    if (null != users)
                    {
                        users.ForEach(user =>
                        {
                            UserItem uItem = user.CloneTo<UserItem>();
                            item.Users.Add(uItem);
                        });
                    }
                });
            }
            */
            treeTOD.ItemsSource = items;
        }

        private void RefreshTreeTAA()
        {
            treeTAA.ItemsSource = null;

            items.Clear();
            /*
            var roles = ops.Role.Gets().Value();
            if (null != roles)
            {
                roles.ForEach(role => 
                {
                    RoleItem item = role.CloneTo<RoleItem>();
                    items.Add(item);

                    var search = Search.User.ByRoleId.Create(role.RoleId);
                    var users = ops.User.Search.ByRoleId(search).Value();
                    if (null != users)
                    {
                        users.ForEach(user =>
                        {
                            UserItem uItem = user.CloneTo<UserItem>();
                            item.Users.Add(uItem);
                        });
                    }
                });
            }
            */
            treeTAA.ItemsSource = items;
        }

        private void RefreshTree()
        {
            RefreshTreeTOD();
            RefreshTreeTAA();
        }

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

        #region Button Handlers

        private void cmdSaveTOD_Click(object sender, RoutedEventArgs e)
        {
            if (null == pgridTOD.SelectedObject) return;
            if (pgridTOD.SelectedObject is RoleItem) SaveRuleTOD();
            if (pgridTOD.SelectedObject is UserItem) SaveUserTOD();
        }

        private void cmdSaveTAA_Click(object sender, RoutedEventArgs e)
        {
            if (null == pgridTAA.SelectedObject) return;
            if (pgridTAA.SelectedObject is RoleItem) SaveRuleTAA();
            if (pgridTAA.SelectedObject is UserItem) SaveUserTAA();
        }

        #endregion

        #region Private Methods

        private void SaveRuleTOD()
        {
            var value = (pgridTOD.SelectedObject as Role);
            if (null == value)
            {
                MessageBox.Show("No item selected");
                return;
            }
            /*
            var ret = ops.User.Save(value);
            if (ret.Failed)
            {
                MessageBox.Show("Save User Error.");
            }
            else
            {
                MessageBox.Show("Save User Success.");
                RefreshTree();
            }
            */
        }

        private void SaveUserTOD()
        {
            var value = (pgridTOD.SelectedObject as User);
            if (null == value)
            {
                MessageBox.Show("No item selected");
                return;
            }
            /*
            var ret = ops.User.Save(value);
            if (ret.Failed)
            {
                MessageBox.Show("Save User Error.");
            }
            else
            {
                MessageBox.Show("Save User Success.");
                RefreshTree();
            }
            */
        }

        private void SaveRuleTAA()
        {
            var value = (pgridTAA.SelectedObject as Role);
            if (null == value)
            {
                MessageBox.Show("No item selected");
                return;
            }
            /*
            var ret = ops.Role.Save(value);
            if (ret.Failed)
            {
                MessageBox.Show("Save Role Error.");
            }
            else
            {
                MessageBox.Show("Save Role Success.");
                RefreshTree();
            }
            */
        }

        private void SaveUserTAA()
        {
            var value = (pgridTAA.SelectedObject as Role);
            if (null == value)
            {
                MessageBox.Show("No item selected");
                return;
            }
            /*
            var ret = ops.User.Save(value);
            if (ret.Failed)
            {
                MessageBox.Show("Save User Error.");
            }
            else
            {
                MessageBox.Show("Save User Success.");
                RefreshTree();
            }
            */
        }

        #endregion
    }

    #region Model classes.

    public class RoleItem : Role
    {
        public RoleItem()
        {
            this.Users = new ObservableCollection<UserItem>();
        }

        [Browsable(false)]
        public ObservableCollection<UserItem> Users { get; set; }
    }

    public class UserItem : User { }

    #endregion
}
