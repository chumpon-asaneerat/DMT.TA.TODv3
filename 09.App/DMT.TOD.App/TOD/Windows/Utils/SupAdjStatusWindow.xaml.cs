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

        #region Private Method

        private void AllowUIToUpdate()
        {
            DispatcherFrame frame = new DispatcherFrame();
            if (null != Dispatcher.CurrentDispatcher)
            {
                Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Render,
                new DispatcherOperationCallback((object parameter) =>
                {
                    frame.Continue = false;
                    return null;
                }), null);

                Dispatcher.PushFrame(frame);
            }

            if (null != Application.Current)
            {
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => { }));
            }
        }

        #endregion

        #region Public Method

        /// <summary>
        /// Notify message.
        /// </summary>
        /// <param name="msg">The message.</param>
        public void Notify(string msg)
        {
            AllowUIToUpdate();
            System.Threading.Thread.Sleep(10);

            Dispatcher.BeginInvoke(new Action(() =>
            {
                txtMsg.Text = msg;
            }), DispatcherPriority.Normal);

            System.Threading.Thread.Sleep(10);
        }

        #endregion
    }
}
