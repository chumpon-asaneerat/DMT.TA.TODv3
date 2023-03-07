#region Using

using DMT.Models;
using DMT.Services;
using System.Windows.Navigation;

#endregion

namespace DMT.Controls
{
    /// <summary>
    /// The UserSearchResult Class.
    /// </summary>
    public class UserSearchResult
    {
        /// <summary>Gets or sets User.</summary>
        public User User { get; set; }
        /// <summary>Gets or sets is user cancel selection.</summary>
        public bool IsCanceled { get; set; }
    }

    /// <summary>
    /// The User Search Manager helper controls.
    /// </summary>
    public class UserSearchManager
    {
        #region Singelton

        private static UserSearchManager _instance = null;
        /// <summary>
        /// Singelton Access.
        /// </summary>
        public static UserSearchManager Instance
        {
            get
            {
                if (null == _instance)
                {
                    lock (typeof(UserSearchManager))
                    {
                        _instance = new UserSearchManager();
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        private UserSearchManager() : base() { }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~UserSearchManager() { }

        #endregion

        #region Public Methods and Properties

        /// <summary>
        /// Select User.
        /// </summary>
        /// <param name="userId">The User Id.</param>
        /// <param name="roles">The Roles.</param>
        /// <returns></returns>
        public UserSearchResult SelectUser(string userId, params string[] roles)
        {
            UserSearchResult ret = new UserSearchResult();
            ret.User = null;
            ret.IsCanceled = false;

            var results = User.FilterByUserId(userId, roles).Value();
            var users = results.FindAll((usr) => 
            { 
                return usr.AccountStatus == User.AccountFlags.Avaliable; 
            });

            if (null != users)
            {
                if (users.Count == 1)
                {
                    ret.User = users[0];
                }
                else if (users.Count > 1)
                {
                    var win = AccountApp.Windows.UserSearch;
                    // change title.
                    if (!string.IsNullOrEmpty(this.Title)) win.Title = this.Title;
                    // setup user list for selection.
                    win.Setup(users);
                    if (win.ShowDialog() == false)
                    {
                        // No user selected.
                        ret.User = null;
                        ret.IsCanceled = true;
                    }
                    else
                    {
                        // User selected.
                        ret.User = win.SelectedUser;
                    }
                }
            }
            return ret;
        }
        /// <summary>
        /// Gets or sets Popup window title.
        /// </summary>
        public string Title { get; set; }

        #endregion
    }
}
