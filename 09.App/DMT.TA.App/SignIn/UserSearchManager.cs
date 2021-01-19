#region Using

using DMT.Models;
using DMT.Services;

#endregion

namespace DMT.Controls
{
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
        public User SelectUser(string userId, params string[] roles)
        {
            User ret = null;

            var users = User.FilterByUserId(userId, roles).Value();
            if (null != users)
            {
                if (users.Count == 1)
                {
                    ret = users[0];
                }
                else if (users.Count > 1)
                {
                    var win = TAApp.Windows.UserSearch;
                    // change title.
                    if (!string.IsNullOrEmpty(this.Title)) win.Title = this.Title;
                    // setup user list for selection.
                    win.Setup(users);
                    if (win.ShowDialog() == false || null == win.SelectedUser)
                    {
                        // No user selected.
                        ret = null;
                    }
                    else
                    {
                        // User selected.
                        ret = win.SelectedUser;
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
