#region Using

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using NLib.Reflection;

#endregion

namespace DMT.Controls.Utils
{
    #region FocusOptions

    /// <summary>
    /// The Focus Options class.
    /// </summary>
    public class FocusOptions
    {
        #region SelectAll

        /// <summary>The SelectAllProperty variable</summary>
        public static readonly DependencyProperty SelectAllProperty = DependencyProperty.RegisterAttached(
            "SelectAll",
            typeof(bool),
            typeof(FocusOptions),
            new PropertyMetadata(false, SelectAllPropertyChanged));

        /// <summary>
        /// Gets SelectAll Value.
        /// </summary>
        /// <param name="obj">The target object.</param>
        /// <returns>Returns current proeprty value.</returns>
        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        [AttachedPropertyBrowsableForType(typeof(PasswordBox))]
        public static bool GetSelectAll(DependencyObject obj)
        {
            return (bool)obj.GetValue(SelectAllProperty);
        }
        /// <summary>
        /// Sets SelectAll Value.
        /// </summary>
        /// <param name="obj">The target object.</param>
        /// <param name="value">The new value.</param>
        public static void SetSelectAll(DependencyObject obj, bool value)
        {
            obj.SetValue(SelectAllProperty, value);
        }

        #endregion

        #region SelectAll Changed Handler

        private static void SelectAllPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is TextBox || obj is PasswordBox)
            {
                if ((e.NewValue as bool?).GetValueOrDefault(false))
                {
                    HookEvents(obj as TextBox);
                    HookEvents(obj as PasswordBox);
                }
                else
                {
                    UnhookEvents(obj as TextBox);
                    UnhookEvents(obj as PasswordBox);
                }
            }
        }

        #region TextBox

        private static void HookEvents(TextBox ctrl) 
        {
            if (null == ctrl) return;
            ctrl.GotKeyboardFocus += OnGotKeyboardFocus;
            ctrl.PreviewMouseLeftButtonDown += OnPreviewMouseLeftButtonDown;
        }
        private static void UnhookEvents(TextBox ctrl) 
        {
            if (null == ctrl) return;
            ctrl.GotKeyboardFocus -= OnGotKeyboardFocus;
            ctrl.PreviewMouseLeftButtonDown -= OnPreviewMouseLeftButtonDown;
        }

        #endregion

        #region PasswordBox

        private static void HookEvents(PasswordBox ctrl) 
        {
            if (null == ctrl) return;
            ctrl.GotKeyboardFocus += OnGotKeyboardFocus;
            ctrl.PreviewMouseLeftButtonDown += OnPreviewMouseLeftButtonDown;
        }
        private static void UnhookEvents(PasswordBox ctrl) 
        {
            if (null == ctrl) return;
            ctrl.GotKeyboardFocus -= OnGotKeyboardFocus;
            ctrl.PreviewMouseLeftButtonDown -= OnPreviewMouseLeftButtonDown;
        }

        #endregion

        #endregion

        #region Control Event Handlers

        private static void OnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (e.OriginalSource is TextBox || e.OriginalSource is PasswordBox)
            {
                KeyboardFocusSelectText(e.OriginalSource as TextBox, e);
                KeyboardFocusSelectText(e.OriginalSource as PasswordBox, e);
            }
        }

        private static void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is TextBox || e.OriginalSource is PasswordBox)
            {
                MouseLeftButtonDownFocus(e.OriginalSource as TextBox, e);
                MouseLeftButtonDownFocus(e.OriginalSource as PasswordBox, e);
            }
        }

        #region TextBox

        private static void KeyboardFocusSelectText(TextBox ctrl, KeyboardFocusChangedEventArgs e)
        {
            if (null != ctrl && ctrl.IsKeyboardFocusWithin)
            {
                ctrl.Dispatcher.BeginInvoke(new Action(() =>
                {
                    ctrl.SelectAll();
                }));
            }
        }

        private static void MouseLeftButtonDownFocus(TextBox ctrl, MouseButtonEventArgs e)
        {
            if (ctrl == null) return;
            if (!ctrl.IsFocused)
            {
                ctrl.Dispatcher.BeginInvoke(new Action(() =>
                {
                    ctrl.Focus();
                    ctrl.ReleaseMouseCapture();
                }));
                e.Handled = true;
            }
        }

        #endregion

        #region PasswordBox

        private static void KeyboardFocusSelectText(PasswordBox ctrl, KeyboardFocusChangedEventArgs e)
        {
            if (null != ctrl && ctrl.IsKeyboardFocusWithin)
            {
                ctrl.Dispatcher.BeginInvoke(new Action(() =>
                {
                    ctrl.SelectAll();
                }));
            }
        }

        private static void MouseLeftButtonDownFocus(PasswordBox ctrl, MouseButtonEventArgs e)
        {
            if (null == ctrl) return;
            if (!ctrl.IsFocused)
            {
                ctrl.Dispatcher.BeginInvoke(new Action(() =>
                {
                    ctrl.Focus();
                    ctrl.ReleaseMouseCapture();
                }));
                e.Handled = true;
            }
        }

        #endregion

        #endregion
    }

    #endregion

    #region KeyboardOptions

    /// <summary>
    /// The Keyboard Options class.
    /// </summary>
    public class KeyboardOptions
    {
        #region Enabled

        #region Public Dependency Properties and methods

        /// <summary>The EnabledProperty variable</summary>
        public static readonly DependencyProperty EnabledProperty = DependencyProperty.RegisterAttached(
            "Enabled",
            typeof(bool),
            typeof(KeyboardOptions),
            new UIPropertyMetadata(false, EnabledChanged));
        /// <summary>
        /// Gets Enabled Value.
        /// </summary>
        /// <param name="obj">The target object.</param>
        /// <returns>Returns current proeprty value.</returns>
        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        [AttachedPropertyBrowsableForType(typeof(PasswordBox))]
        public static bool GetEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(EnabledProperty);
        }
        /// <summary>
        /// Sets Enabled Value.
        /// </summary>
        /// <param name="obj">The target object.</param>
        /// <param name="value">The new value.</param>
        public static void SetEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(EnabledProperty, value);
        }

        #endregion

        #region Enabled Changed Handler

        private static void EnabledChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var ue = obj as FrameworkElement;

            if (!(ue is TextBox || ue is PasswordBox)) return; // only TextBox, PasswordBox
            if (null != (ue as TextBox) && (ue as TextBox).AcceptsReturn) return; // TextBox has AcceptsReturn property = true so ignore it. 

            if (ue == null) return;

            if ((bool)e.NewValue)
            {
                //ue.Unloaded += ue_Unloaded;
                ue.PreviewKeyDown += ue_PreviewKeyDown;
            }
            else
            {
                ue.PreviewKeyDown -= ue_PreviewKeyDown;
            }
        }

        #endregion

        #region Control Event Handlers

        private static void ue_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var ue = e.OriginalSource as FrameworkElement;

            if (!(ue is TextBox || ue is PasswordBox)) return;

            var sys = Keyboard.IsKeyDown(Key.System);
            var alt = Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt);
            var ctrl = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
            var shf = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);

            if (sys || alt || ctrl || shf) return; // ignore if Ctrl/Alt/Shift/System key hold.

            if ((e.Key == Key.Enter || e.Key == Key.Return) && GetEnterAsTab(ue))
            {
                e.Handled = true;
                // Move to next tab order.
                if (!ue.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next)))
                {
                    //ue.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
                }
            }
            else if (e.Key == Key.Up && GetUpDownNavigation(ue))
            {
                // UP Arrow
                e.Handled = true;
                // Get the element which would receive focus if focus were changed.
                var obj = ue.PredictFocus(FocusNavigationDirection.Up);
                var next = (obj is TextBox || obj is PasswordBox) ? obj as Control : null;
                if (null != next)
                {
                    // Move focus to another focusable element upwards from the currently focused element.
                    ue.MoveFocus(new TraversalRequest(FocusNavigationDirection.Up));
                }
                else
                {
                    // Move to previous tab order.
                    //ue.MoveFocus(new TraversalRequest(FocusNavigationDirection.Previous));
                }
            }
            else if (e.Key == Key.Down && GetUpDownNavigation(ue))
            {
                // DOWN Arrow
                e.Handled = true;
                // Get the element which would receive focus if focus were changed.
                var obj = ue.PredictFocus(FocusNavigationDirection.Down);
                var next = (obj is TextBox || obj is PasswordBox) ? obj as Control : null;
                if (null != next)
                {
                    // Move focus to another focusable element downwards from the currently focused element.
                    ue.MoveFocus(new TraversalRequest(FocusNavigationDirection.Down));
                }
                else
                {
                    // Move to next tab order.
                    //ue.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }
            }
            else if (e.Key == Key.Left && GetLeftRightNavigation(ue) && CanMoveLeft(ue))
            {
                // LEFT Arrow
                e.Handled = true;
                // Get the element which would receive focus if focus were changed.
                var obj = ue.PredictFocus(FocusNavigationDirection.Left);
                var next = (obj is TextBox || obj is PasswordBox) ? obj as Control : null;
                if (null != next)
                {
                    // Move focus to another focusable element to the left of the currently focused element.
                    ue.MoveFocus(new TraversalRequest(FocusNavigationDirection.Left));
                }
                else
                {
                    // Move focus to another focusable element upwards from the currently focused element.
                    //ue.MoveFocus(new TraversalRequest(FocusNavigationDirection.Up));
                }
            }
            else if (e.Key == Key.Right && GetLeftRightNavigation(ue) && CanMoveRight(ue))
            {
                // RIGHT Arrow
                e.Handled = true;
                // Get the element which would receive focus if focus were changed.
                var obj = ue.PredictFocus(FocusNavigationDirection.Right);
                var next = (obj is TextBox || obj is PasswordBox) ? obj as Control : null;
                if (null != next)
                {
                    // Move focus to another focusable element to the right of the currently focused element.
                    ue.MoveFocus(new TraversalRequest(FocusNavigationDirection.Right));
                }
                else
                {
                    // Move focus to another focusable element downwards from the currently focused element.
                    //ue.MoveFocus(new TraversalRequest(FocusNavigationDirection.Down));
                }
            }
        }

        /*
        private static void ue_Unloaded(object sender, RoutedEventArgs e)
        {
            var ue = sender as FrameworkElement;
            if (ue == null) return;

            ue.Unloaded -= ue_Unloaded;
            ue.PreviewKeyDown -= ue_PreviewKeyDown;
        }
        */

        #region CanMoveLeft

        private static bool CanMoveLeft(FrameworkElement ctrl)
        {
            bool ret = false;
            if (null == ctrl) return ret;
            if (!ret) ret = CanMoveLeft(ctrl as TextBox);
            if (!ret) ret = CanMoveLeft(ctrl as PasswordBox);
            return ret;
        }

        private static bool CanMoveLeft(TextBox ctrl)
        {
            bool ret = false;
            if (null == ctrl) return ret;

            // Select All so cursor position is not matter.
            if (ctrl.SelectionLength == ctrl.Text.Length &&
                (ctrl.CaretIndex == 0 || ctrl.CaretIndex == ctrl.Text.Length)) ret = true;

            // No selection need to check cusror position at left most.
            if (ctrl.SelectionLength == 0 && ctrl.CaretIndex == 0) ret = true;

            return ret;
        }

        private static bool CanMoveLeft(PasswordBox ctrl)
        {
            bool ret = false;
            if (null == ctrl) return ret;
            // Password Has No API to get Selection Text.
            return ret;
        }

        #endregion

        #region CanMoveRight

        private static bool CanMoveRight(FrameworkElement ctrl)
        {
            bool ret = false;
            if (null == ctrl) return ret;
            if (!ret) ret = CanMoveRight(ctrl as TextBox);
            if (!ret) ret = CanMoveRight(ctrl as PasswordBox);
            return ret;
        }

        private static bool CanMoveRight(TextBox ctrl)
        {
            bool ret = false;
            if (null == ctrl) return ret;

            // Select All so cursor position is not matter.
            if (ctrl.SelectionLength == ctrl.Text.Length &&
                (ctrl.CaretIndex == 0 || ctrl.CaretIndex == ctrl.Text.Length)) ret = true;

            // No selection need to check cusror position at right most.
            if (ctrl.SelectionLength == 0 && ctrl.CaretIndex == ctrl.Text.Length) ret = true;

            return ret;
        }

        private static bool CanMoveRight(PasswordBox ctrl)
        {
            bool ret = false;
            if (null == ctrl) return ret;
            // Password Has No API to get Selection Text.
            return ret;
        }

        #endregion

        #endregion

        #endregion

        #region Enter As Tab

        #region Public Dependency Properties and methods

        /// <summary>The EnterAsTabProperty variable</summary>
        public static readonly DependencyProperty EnterAsTabProperty = DependencyProperty.RegisterAttached(
            "EnterAsTab",
            typeof(bool),
            typeof(KeyboardOptions),
            null);
        /// <summary>
        /// Gets EnterAsTab Value.
        /// </summary>
        /// <param name="obj">The target object.</param>
        /// <returns>Returns current proeprty value.</returns>
        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        [AttachedPropertyBrowsableForType(typeof(PasswordBox))]
        public static bool GetEnterAsTab(DependencyObject obj)
        {
            return (bool)obj.GetValue(EnterAsTabProperty);
        }
        /// <summary>
        /// Sets EnterAsTab Value.
        /// </summary>
        /// <param name="obj">The target object.</param>
        /// <param name="value">The new value.</param>
        public static void SetEnterAsTab(DependencyObject obj, bool value)
        {
            obj.SetValue(EnterAsTabProperty, value);
        }

        #endregion

        #endregion

        #region Up/Down Navigation

        #region Public Dependency Properties and methods

        /// <summary>The UpDownNavigationProperty variable</summary>
        public static readonly DependencyProperty UpDownNavigationProperty = DependencyProperty.RegisterAttached(
            "UpDownNavigation",
            typeof(bool),
            typeof(KeyboardOptions),
            null);
        /// <summary>
        /// Gets UpDownNavigation Value.
        /// </summary>
        /// <param name="obj">The target object.</param>
        /// <returns>Returns current proeprty value.</returns>
        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        [AttachedPropertyBrowsableForType(typeof(PasswordBox))]
        public static bool GetUpDownNavigation(DependencyObject obj)
        {
            return (bool)obj.GetValue(UpDownNavigationProperty);
        }
        /// <summary>
        /// Sets UpDownNavigation Value.
        /// </summary>
        /// <param name="obj">The target object.</param>
        /// <param name="value">The new value.</param>
        public static void SetUpDownNavigation(DependencyObject obj, bool value)
        {
            obj.SetValue(UpDownNavigationProperty, value);
        }

        #endregion

        #endregion

        #region Left/Right Navigation

        #region Public Dependency Properties and methods

        /// <summary>The LeftRightNavigationProperty variable</summary>
        public static readonly DependencyProperty LeftRightNavigationProperty = DependencyProperty.RegisterAttached(
            "LeftRightNavigation",
            typeof(bool),
            typeof(KeyboardOptions),
            null);
        /// <summary>
        /// Gets UpDownNavigation Value.
        /// </summary>
        /// <param name="obj">The target object.</param>
        /// <returns>Returns current proeprty value.</returns>
        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        [AttachedPropertyBrowsableForType(typeof(PasswordBox))]
        public static bool GetLeftRightNavigation(DependencyObject obj)
        {
            return (bool)obj.GetValue(LeftRightNavigationProperty);
        }
        /// <summary>
        /// Sets UpDownNavigation Value.
        /// </summary>
        /// <param name="obj">The target object.</param>
        /// <param name="value">The new value.</param>
        public static void SetLeftRightNavigation(DependencyObject obj, bool value)
        {
            obj.SetValue(LeftRightNavigationProperty, value);
        }

        #endregion

        #endregion
    }

    #endregion
}
