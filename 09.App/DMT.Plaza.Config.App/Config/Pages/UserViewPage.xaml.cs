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
    using ops = Services.Operations.Plaza.Security; // reference to static class.

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

        private void RefreshTree()
        {
            tree.ItemsSource = null;

            items.Clear();

            /*
            var roles = ops.Role.Gets().Value();
            var users = ops.User.Gets().Value();
            if (null != roles)
            {
                roles.ForEach(role =>
                {
                    RoleItem item = role.CloneTo<RoleItem>();
                    items.Add(item);
                    if (null != users)
                    {
                        users.ForEach(user =>
                        {
                            // Add if match role.
                            if (user.RoleId == role.RoleId)
                            {
                                UserItem uItem = user.CloneTo<UserItem>();
                                item.Users.Add(uItem);
                            }
                        });
                    }
                });
            }
            */

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
            tree.ItemsSource = items;
        }

        #endregion

        #region TreeView Handler

        private void tree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            pgrid.SelectedObject = e.NewValue;
        }

        #endregion

        private void SaveRule()
        {
            var value = (pgrid.SelectedObject as Role);
            if (null != value)
            {
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
            }
        }

        private void SaveUser()
        {
            var value = (pgrid.SelectedObject as User);
            if (null != value)
            {
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
            }
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (null == pgrid.SelectedObject) return;
            if (pgrid.SelectedObject is RoleItem) SaveRule();
            if (pgrid.SelectedObject is UserItem) SaveUser();
        }
    }

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
}
