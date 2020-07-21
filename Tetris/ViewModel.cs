using System;
using System.Collections.Generic;
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
        /// Handles cases where a property is changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Maps key inputs to colours to fill the rectangle with.
        /// </summary>
        public Dictionary<Key, string> KeyColourMap = new Dictionary<Key, string>
        {
            { Key.W, "Blue" },
            { Key.A, "Red" },
            { Key.S, "Yellow" },
            { Key.D, "Green" }
        };

        /// <summary>
        /// Basic constructor for ViewModel.
        /// </summary>
        /// <param name="parent">The parent view to access named elements.</param>
        public ViewModel(MainWindow parent)
        {
            Arena = new Grid(20, 10, parent.MyCanvas);
            Session = new Game(Arena);
        }

        /// <summary>
        /// The grid of rectangles serving as the playable arena.
        /// </summary>
        public Grid Arena { get; set; }

        /// <summary>
        /// The game session holding all the main logic.
        /// </summary>
        public Game Session { get; set; }

        /// <summary>
        /// Internal value indicating if output is enabled - assists the property.
        /// </summary>
        private bool isActive = true;
        /// <summary>
        /// Property indicating if output is enabled - fires OnPropertyChanged() on set.
        /// </summary>
        public bool IsActive
        {
            get { return isActive; }
            set
            {
                isActive = value;
                OnPropertyChanged("IsActive");
            }
        }

        /// <summary>
        /// Event handler for the KeyDown event - logic contained in the KeyDown function.
        /// </summary>
        /// <param name="sender">The object raising the event.</param>
        /// <param name="e">The event arguments provided.</param>
        /// <see cref="KeyDown(Key)"/>
        public void KeyDownHandler(object sender, KeyEventArgs e)
        {
            KeyDown(e.Key);
        }

        /// <summary>
        /// Logic to handle the KeyDown event using the key.
        /// </summary>
        /// <remarks>
        /// This is separate from the event handler to allow tests to easily mimic key input.
        /// </remarks>
        /// <param name="key">The key that had been pressed.</param>
        public void KeyDown(Key key)
        {
            switch (key)
            {
                case Key.Enter:
                    Session.Start();
                    break;
                case Key.Oem5: // For testing
                    Session.Start(false, true);
                    break;
                case Key.W:
                    Session.HardDrop();
                    break;
                case Key.A:
                    Session.MoveLeft();
                    break;
                case Key.S:
                    Session.SoftDrop();
                    break;
                case Key.D:
                    Session.MoveRight();
                    break;
                case Key.Left:
                    Session.RotatePieceACW();
                    break;
                case Key.Right:
                    Session.RotatePieceCW();
                    break;
                default:
                    System.Windows.MessageBox.Show(key.ToString());
                    break;
            }
        }

        /// <summary>
        /// Event handler for the LostFocus event.
        /// </summary>
        /// <param name="sender">The object raising the event.</param>
        /// <param name="e">The event arguments provided.</param>
        public void LostFocusHandler(object sender, EventArgs e)
        {
            IsActive = false;
        }

        /// <summary>
        /// Event handler for the GotFocus event.
        /// </summary>
        /// <param name="sender">The object raising the event.</param>
        /// <param name="e">The event arguments provided.</param>
        public void GotFocusHandler(object sender, EventArgs e)
        {
            IsActive = true;
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