#region Using

using System;
using System.Windows;
using System.Windows.Threading;
using NLib;

#endregion

namespace DMT.Account.Windows
{
    /// <summary>
    /// Interaction logic for SplashScreenWindow.xaml
    /// </summary>
    public partial class SplashScreenWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public SplashScreenWindow()
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

        #region Public Methods

        /// <summary>
        /// Init step.
        /// </summary>
        /// <param name="maxStep">The max step.</param>
        public void Setup(int maxStep)
        {
            txtMsg.Text = string.Empty;
            progress.Minimum = 0;
            progress.Maximum = (maxStep > 0) ? maxStep : 1;
            progress.Value = 0;
        }
        /// <summary>
        /// Next step.
        /// </summary>
        /// <param name="msg"></param>
        public void Next(string msg)
        {
            AllowUIToUpdate();
            System.Threading.Thread.Sleep(10);

            Dispatcher.BeginInvoke(new Action(() =>
            {
                txtMsg.Text = msg;
                if (progress.Value + 1 < progress.Maximum)
                    progress.Value = progress.Value + 1;
                else progress.Value = progress.Maximum;
            }), DispatcherPriority.Normal);

            System.Threading.Thread.Sleep(10);
        }
        /// <summary>
        /// Gets is completed all step.
        /// </summary>
        public bool Completed { get { return progress.Value == progress.Maximum; } }

        #endregion
    }
}
