using System;
using System.Collections.Generic;
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

namespace DMT.Simulator.Pages
{
    /// <summary>
    /// Interaction logic for BankNotEntryPage.xaml
    /// </summary>
    public partial class BankNotEntryPage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public BankNotEntryPage()
        {
            InitializeComponent();
        }

        #endregion

        public void Setup()
        {
            var tran = new Models.TSBCreditTransaction();
            entry.DataContext = tran;

            var tran2 = new Models.TSBCreditTransaction();
            entry2.DataContext = tran2;

            var tran3 = new Models.TSBCreditTransaction();
            entry3.DataContext = tran3;
        }
    }
}
