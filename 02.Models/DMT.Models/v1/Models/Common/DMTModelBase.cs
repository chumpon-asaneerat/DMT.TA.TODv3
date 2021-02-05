#region Using

using System.ComponentModel;
using NLib;

// required for JsonIgnore attribute.
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

#endregion

namespace DMT.Models
{
    #region DMTModelBase (abstract)

    /// <summary>
    /// The DMTModelBase abstract class.
    /// Provide basic implementation of INotifyPropertyChanged interface.
    /// </summary>
    public abstract class DMTModelBase : INotifyPropertyChanged
    {
        #region Internal Variables

        private bool _lock = false;

        #endregion

        #region Private Methods

        /// <summary>
        /// Raise Property Changed Event.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        protected void RaiseChanged(string propertyName)
        {
            if (!_lock)
            {
                PropertyChanged.Call(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Enable Notify Change Event.
        /// </summary>
        public void EnableNotify()
        {
            _lock = true;
        }
        /// <summary>
        /// Disable Notify Change Event.
        /// </summary>
        public void DisableNotify()
        {
            _lock = false;
        }
        /// <summary>
        /// Checks is notifiy enabled or disable.
        /// </summary>
        /// <returns>Returns true if enabled.</returns>
        public bool Notified() { return _lock;  }

        #endregion

        #region Public Events

        /// <summary>
        /// The PropertyChanged event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }

    #endregion
}
