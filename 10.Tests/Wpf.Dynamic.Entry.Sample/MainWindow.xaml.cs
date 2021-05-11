#region Using

using System;
using System.Linq;
using System.Windows;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Text;

using DMT.Models;

#endregion

namespace Wpf.Dynamic.Entry.Sample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        #endregion
    }
}
