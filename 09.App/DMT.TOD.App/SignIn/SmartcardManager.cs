#region Using

using System;

using NLib;
using System.Reflection;

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

            MethodBase med = MethodBase.GetCurrentMethod();
            string msg = string.Format("SMARTCARD - DETECT S/N : {0}", this.CardId);
            med.Info(msg);

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
            MethodBase med = MethodBase.GetCurrentMethod();
            string msg = string.Empty;

            if (string.IsNullOrWhiteSpace(this.CardId))
            {
                msg = string.Format("SMARTCARD - S/N is empty string. S/N: {0}", this.CardId);
                med.Info(msg);
                this.User = null;
            }
            else
            {
                msg = string.Format("SMARTCARD - FIND USER BY CARD S/N. S/N: {0}", this.CardId);
                med.Info(msg);

                var usr = User.GetByCardId(this.CardId).Value();                
                // write log
                if (null == usr)
                {
                    msg = string.Format("SMARTCARD - NOT FOUND USER BY S/N: {0}", this.CardId);
                    med.Info(msg);
                }
                else
                {
                    msg = string.Format("SMARTCARD - FOUND USER BY S/N: {0}, USERNAME: {1}", 
                        this.CardId, usr.FullNameTH);
                    med.Info(msg);
                }

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
                    // CardId Not found  on database.
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
