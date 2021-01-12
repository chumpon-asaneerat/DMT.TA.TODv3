using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NLib.Controls
{
    /// <summary>
    /// The Rdlc Message Event Handler.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The RdlcMessageEventArgs instance.</param>
    public delegate void RdlcMessageEventHandler(object sender, RdlcMessageEventArgs e);

    /// <summary>
    /// The Rdlc Message Event Args class.
    /// </summary>
    public class RdlcMessageEventArgs
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public RdlcMessageEventArgs() : base() { }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets Message.
        /// </summary>
        public string Message { get; set; }

        #endregion
    }

    /// <summary>
    /// Rdlc Message Service.
    /// </summary>
    public class RdlcMessageService
    {
        #region Singelton Access
        
        private static RdlcMessageService _instance = null;
        /// <summary>
        /// Singelton Access
        /// </summary>
        public static RdlcMessageService Instance
        {
            get
            {
                if (null == _instance)
                {
                    lock (typeof(RdlcMessageService))
                    {
                        _instance = new RdlcMessageService();
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        private RdlcMessageService()
            : base()
        {

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Send Message.
        /// </summary>
        /// <param name="message">The message</param>
        public void SendMessage(string message)
        {
            if (null != MessageArrived)
            {
                MessageArrived.Call(this, new RdlcMessageEventArgs() { Message = message });
            }
        }

        #endregion

        #region Public Events

        /// <summary>
        /// The MessageArrived event.
        /// </summary>
        public event RdlcMessageEventHandler MessageArrived;

        #endregion
    }
}
