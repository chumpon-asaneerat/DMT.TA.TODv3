#region Using

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

// Required for custom command in UI
using System.Windows.Input;

using NLib.Reflection;

#endregion

namespace DMT.Models
{
    #region UI Models for Infrastructure Tree

    #region TSBItem

    /// <summary>
    /// The TSBItem model class.
    /// </summary>
    public class TSBItem : TSB
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        private TSBItem() : base()
        {
            Plazas = new ObservableCollection<PlazaItem>();
        }
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="value">The TSB instance.</param>
        public TSBItem(TSB value) : this() 
        {
            if (null != value) value.AssignTo(this);
        }

        #endregion

        #region Public Properties

        /// <summary>Gets Is Active in string.</summary>
        [Browsable(false)]
        public string IsActive { get { return (Active) ? "[A]" : string.Empty; } set { } }
        /// <summary>Gets Plazas</summary>
        [Browsable(false)]
        public ObservableCollection<PlazaItem> Plazas { get; set; }

        #endregion

        #region UI Command

        /// <summary>
        /// Route Command for Change Active TSB.
        /// </summary>
        public static readonly RoutedUICommand ChangeActiveTSB = new RoutedUICommand(
            "ChangeActiveTSB",
            "ChangeActiveTSB",
            typeof(TSBItem),
            new InputGestureCollection()
            {
                //new KeyGesture(Key.F4, ModifierKeys.Alt) 
            });

        #endregion
    }

    #endregion

    #region PlazaItem

    /// <summary>
    /// The PlazaItem model class.
    /// </summary>
    public class PlazaItem : Plaza
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        private PlazaItem()
        {
            Lanes = new ObservableCollection<LaneItem>();
        }
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="value">The Plaza instance.</param>
        public PlazaItem(Plaza value) : this()
        {
            if (null != value) value.AssignTo(this);
        }

        #endregion

        #region Public Properties

        /// <summary>Gets Lanes</summary>
        [Browsable(false)]
        public ObservableCollection<LaneItem> Lanes { get; set; }

        #endregion
    }

    #endregion

    #region LaneItem

    /// <summary>
    /// The LaneItem model class.
    /// </summary>
    //[Browsable(false)]
    public class LaneItem : Lane 
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        private LaneItem() : base() { }
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="value">The Lane instance.</param>
        public LaneItem(Lane value) : this()
        {
            if (null != value) value.AssignTo(this);
        }

        #endregion
    }

    #endregion

    #endregion

    static partial class UICommands
    {
        /// <summary>The Infrastructure's commands class</summary>
        public static partial class Infrastructure
        {
            /// <summary>The ChangeActiveTSB commands class</summary>
            public static class ChangeActiveTSB
            {
                /// <summary>The Command Name.</summary>
                public static readonly string Name = "ChangeActiveTSB";
                /// <summary>The Command Text.</summary>
                public static readonly string Text = "Change Active TSB";

                /// <summary>The RouteUICommand instance.</summary>
                public static readonly RoutedUICommand Command = new RoutedUICommand(
                    Text,
                    Name,
                    typeof(TSBItem),
                    new InputGestureCollection()
                    {
                        //new KeyGesture(Key.F4, ModifierKeys.Alt) 
                    });
            }
        }
    }
}
