#region Using

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;

#endregion

namespace DMT.Controls.Buttons
{
    /// <summary>
    /// Interaction logic for ImageButton.xaml
    /// </summary>
    [ContentProperty("Items")]
    public partial class ImageButton : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public ImageButton()
        {
            InitializeComponent();
            // Set DataContext of child elements.
            _Image.DataContext = this;
            _ItemsControl.DataContext = this;
            // Create UI Collection.
            Items = new ObservableCollection<UIElement>();
        }

        #endregion

        #region Loaded

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region Button Handlers

        private void _Button_Click(object sender, RoutedEventArgs e)
        {
            RaiseClickEvent(sender, e);
        }

        #endregion

        #region Private Methods

        private void RaiseClickEvent(object sender, RoutedEventArgs e)
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(ImageButton.ClickEvent, sender);
            RaiseEvent(newEventArgs);
        }

        #endregion

        #region Public Properties

        #region Items

        /// <summary>The ItemsProperty variable.</summary>
        public static readonly DependencyProperty ItemsProperty =
                DependencyProperty.Register("Items", typeof(ObservableCollection<UIElement>), typeof(ImageButton), new PropertyMetadata(null));
        /// <summary>
        /// Gets or sets Items.
        /// </summary>
        public ObservableCollection<UIElement> Items
        {
            get { return (ObservableCollection<UIElement>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        #endregion

        #region ImageSource

        /// <summary>The ImageSourceProperty variable.</summary>
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(ImageButton));

        /// <summary>Gets or sets image source.</summary>
        public ImageSource ImageSource
        {
            get
            {
                return (ImageSource)GetValue(ImageSourceProperty);
            }
            set
            {
                SetValue(ImageSourceProperty, value);
            }
        }

        #endregion

        #endregion

        #region Public Events

        #region Click

        /// <summary>The ClickEvent variable.</summary>
        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent(
            "Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ImageButton));

        /// <summary>The Click Event Handler.</summary>
        public event RoutedEventHandler Click
        {
            add { AddHandler(ClickEvent, value); }
            remove { RemoveHandler(ClickEvent, value); }
        }

        #endregion

        #endregion
    }
}
