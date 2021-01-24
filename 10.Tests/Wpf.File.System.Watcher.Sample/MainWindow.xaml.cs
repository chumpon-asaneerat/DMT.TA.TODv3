#region Using

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using DMT;

#endregion

namespace Wpf.FileSystemWatchers.Sample
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

        #region Internal Variables

        private FileSystemWatcher watcher = null;
        private SampleJson data = new SampleJson();

        #endregion

        #region Loaded/Unloaded

        private void Window_Loaded(Object sender, RoutedEventArgs e)
        {
            txtFileName.Text = "sample.json";
            txtFileName.IsEnabled = false;

            cmdStart.IsEnabled = true;
            cmdShutdown.IsEnabled = !cmdStart.IsEnabled;

            txtPath.Text = PathName;
        }

        private void Window_Unloaded(Object sender, RoutedEventArgs e)
        {
            Shutdown();
        }

        #endregion

        #region Button Handlers

        private void cmdStart_Click(Object sender, RoutedEventArgs e)
        {
            cmdStart.IsEnabled = false;
            cmdShutdown.IsEnabled = !cmdStart.IsEnabled;

            Start();
        }

        private void cmdShutdown_Click(Object sender, RoutedEventArgs e)
        {
            Shutdown();

            cmdStart.IsEnabled = true;
            cmdShutdown.IsEnabled = !cmdStart.IsEnabled;
        }

        private void cmdLoad_Click(Object sender, RoutedEventArgs e)
        {

        }

        private void cmdSave_Click(Object sender, RoutedEventArgs e)
        {
            string fileName = FileName;
            if (string.IsNullOrEmpty(fileName)) return;
            data.SaveToFile(fileName);
        }

        #endregion

        #region File System Watcher Handlers

        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.FullPath) && !string.IsNullOrWhiteSpace(this.FileName) &&
                e.FullPath.Trim().ToLower() == this.FileName.Trim().ToLower())
            {
                /*
                // Gets Last Write Time.
                DateTime lastWriteTime = File.GetLastWriteTime(e.FullPath);
                TimeSpan ts = lastWriteTime - _lastRead;
                if (ts.TotalMilliseconds > 0)
                {
                    Console.WriteLine("Detected File '{0}' Changed.", e.Name);
                    // Reload config.
                    this.LoadConfig();
                    // Set last read.
                    _lastRead = lastWriteTime;
                }
                */
            }
        }

        #endregion

        #region Private Methods

        private string PathName
        {
            get { return NJson.LocalConfigFolder; }
        }

        private string FileName
        {
            get
            {
                string fileName = txtFileName.Text.Trim();
                if (string.IsNullOrWhiteSpace(fileName)) return string.Empty;
                return Path.Combine(PathName, fileName);
            }
        }

        private void Start()
        {
            if (null != watcher) return;
            if (string.IsNullOrEmpty(PathName)) return;

            watcher = new FileSystemWatcher();
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Path = PathName;
            watcher.Filter = "*.json";
;
            watcher.EnableRaisingEvents = true;
            watcher.Changed += Watcher_Changed;
        }

        private void Shutdown()
        {
            if (null != watcher)
            {
                watcher.EnableRaisingEvents = false;
                watcher.Changed -= Watcher_Changed;
                watcher.Dispose();
            }
            watcher = null;
        }

        #endregion
    }

    public class SampleJson
    {
        public SampleJson()
        {
            this.IP = new IP();
            this.Items = new List<EditItem>();
            this.Items.Add(new EditItem() { Name = "Item 1", UpdateDateTime = DateTime.Now });
            this.Items.Add(new EditItem() { Name = "Item 2", UpdateDateTime = new DateTime?() }); ;
            this.Items.Add(new EditItem() { Name = "Item 3", UpdateDateTime = DateTime.Now });
        }

        public IP IP { get; set; }
        public List<EditItem> Items { get; set; }
    }

    public class IP
    {
        public string HostName { get; set; }
    }

    public class EditItem
    {
        public string Name { get; set; }
        public DateTime? UpdateDateTime { get; set; }
    }
}
