using System;
using System.ComponentModel;
using System.Windows.Input;

namespace Tetris
{
    /// <summary>
    /// The ViewModel linked to the main view class.
    /// Handles the logic and mediates between the view and model.
    /// </summary>
    public class ViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Basic constructor for ViewModel.
        /// </summary>
        public ViewModel() { }

        /// <summary>
        /// Internal value for the output message - assists the property.
        /// </summary>
        private string _output = "Key () Down";
        /// <summary>
        /// Property for the output message - fires OnPropertyChanged() on set.
        /// </summary>
        public string Output
        {
            get { return _output; }
            set
            {
                _output = value;
                OnPropertyChanged("Output");
            }
        }

        /// <summary>
        /// Internal value indicating if output is enabled - assists the property.
        /// </summary>
        private bool _outEnabled = true;
        /// <summary>
        /// Property indicating if output is enabled - fires OnPropertyChanged() on set.
        /// </summary>
        public bool OutEnabled
        {
            get { return _outEnabled; }
            set
            {
                _outEnabled = value;
                OnPropertyChanged("OutEnabled");
            }
        }

        /// <summary>
        /// Handles cases where a property is changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Event handler for the KeyDown event.
        /// </summary>
        /// <param name="sender">The object raising the event.</param>
        /// <param name="e">The event arguments provided.</param>
        public void KeyDown(object sender, KeyEventArgs e)
        {
            Output = "Key (" + e.Key.ToString() + ") Down";
        }

        /// <summary>
        /// Event handler for the LostFocus event.
        /// </summary>
        /// <param name="sender">The object raising the event.</param>
        /// <param name="e">The event arguments provided.</param>
        public void LostFocus(object sender, EventArgs e)
        {
            OutEnabled = false;
        }

        /// <summary>
        /// Event handler for the GotFocus event.
        /// </summary>
        /// <param name="sender">The object raising the event.</param>
        /// <param name="e">The event arguments provided.</param>
        public void GotFocus(object sender, EventArgs e)
        {
            OutEnabled = true;
        }

        /// <summary>
        /// Makes use of the property changed handler if it exists.
        /// </summary>
        /// <param name="info">The information passed about the property.</param>
        private void OnPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }
    }
}