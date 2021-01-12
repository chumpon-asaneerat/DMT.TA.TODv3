#region Using

using System;

using NLib;

using DMT.Smartcard;
using DMT.Models;
using DMT.Services;

#endregion

namespace DMT.Controls
{
    /// <summary>
    /// The Smartcard Manager helper controls.
    /// </summary>
    public class SmartcardManager
    {
        #region Singelton

        private static SmartcardManager _instance = null;
        /// <summary>
        /// Singelton Access.
        /// </summary>
        public static SmartcardManager Instance
        {
            get
            {
                if (null == _instance)
                {
                    lock (typeof(SmartcardManager))
                    {
                        _instance = new SmartcardManager();
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
        private SmartcardManager() : base()
        {
            // Read serial only.
            SmartcardService.Instance.ReadSerialNoOnly = true;
            // Set Secure Key.
            SmartcardService.Instance.SecureKey = SL600SDK.DefaultKey;
            // Init Event Handlers.
            SmartcardService.Instance.OnIdle += SmartcardService_OnIdle;
            SmartcardService.Instance.OnCardReadSerial += SmartcardService_OnCardReadSerial;
        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~SmartcardManager()
        {
            // Release Event Handlers.
            SmartcardService.Instance.OnCardReadSerial -= SmartcardService_OnCardReadSerial;
            SmartcardService.Instance.OnIdle -= SmartcardService_OnIdle;
            SmartcardService.Instance.Shutdown();
        }

        #endregion

        #region Private Methods

        #region SmartcardService Event Handlers

        private void SmartcardService_OnIdle(object sender, EventArgs e)
        {
            this.CardId = string.Empty;
            if (null != this.User)
            {
                this.User = null;
                RaiseUserChanged();
            }
        }

        private void SmartcardService_OnCardReadSerial(object sender, M1CardReadSerialEventArgs e)
        {
            this.CardId = e.SerialNo.Replace(" ", string.Empty);
            FindUserByCardId();
        }

        #endregion

        #region Raise Event(s)

        private void RaiseUserChanged()
        {
            UserChanged.Call(this, EventArgs.Empty);
        }

        #endregion

        #region Search User By Card Id

        private void FindUserByCardId()
        {
            if (string.IsNullOrWhiteSpace(this.CardId))
            {
                this.User = null;
            }
            else
            {
                var usr = User.GetByCardId(this.CardId).Value();
                if (null == this.User && null != usr)
                {
                    this.User = usr;
                    RaiseUserChanged();
                }
                else if (null != this.User && null == usr)
                {
                    this.User = usr;
                    RaiseUserChanged();
                }
                else if (null != this.User && null != usr)
                {
                    if (this.User.UserId != usr.UserId)
                    {
                        this.User = usr;
                        RaiseUserChanged();
                    }
                }
                else
                {
                    // CardId Not found  on dataase.
                    this.User = usr;
                    RaiseUserChanged();
                }
            }
        }

        #endregion

        #endregion

        #region Public Methods

        /// <summary>
        /// Start listen device.
        /// </summary>
        public void Start()
        {
            SmartcardService.Instance.Start();
        }
        /// <summary>
        /// Stop listen device.
        /// </summary>
        public void Shutdown()
        {
            SmartcardService.Instance.Shutdown();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets Card Id.
        /// </summary>
        public string CardId { get; private set; }
        /// <summary>
        /// Gets User.
        /// </summary>
        public User User { get; private set; }

        #endregion

        #region Public Events

        /// <summary>
        /// The User Changed event.
        /// </summary>
        public event EventHandler UserChanged;

        #endregion
    }
}
