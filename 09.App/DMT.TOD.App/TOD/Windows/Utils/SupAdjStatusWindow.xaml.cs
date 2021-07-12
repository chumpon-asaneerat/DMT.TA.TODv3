#region Using

using System;
using System.Windows;
using System.Windows.Threading;
using NLib;

#endregion

namespace DMT.Windows
{
    /// <summary>
    /// Interaction logic for SupAdjStatusWindow.xaml
    /// </summary>
    public partial class SupAdjStatusWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public SupAdjStatusWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Public Method

        /// <summary>
        /// Notify message.
        /// </summary>
        /// <param name="msg">The message.</param>
        public void Notify(string msg)
        {
            ApplicationManager.Instance.DoEvents();
            System.Threading.Thread.Sleep(10);

            Dispatcher.BeginInvoke(new Action(() =>
            {
                txtMsg.Text = msg;
            }), DispatcherPriority.Normal);

            ApplicationManager.Instance.DoEvents();
            System.Threading.Thread.Sleep(10);
        }

        #endregion
    }
}
