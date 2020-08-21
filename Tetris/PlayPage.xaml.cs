using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace Tetris
{
    /// <summary>
    /// The class bound to the WPF page to be displayed in the play state.
    /// </summary>
    public partial class PlayPage : Page
    {
        /// <summary>
        /// Constructor to bind the Page with the appropriate ViewModel instance.
        /// </summary>
        public PlayPage()
        {
            InitializeComponent();
            DataContext = new PlayViewModel(this);
        }

        /// <summary>
        /// Event handler for the KeyDown event - logic contained in the KeyDown function.
        /// </summary>
        /// <param name="sender">The object raising the event.</param>
        /// <param name="e">The event arguments provided.</param>
        public void KeyDownHandler(object sender, KeyEventArgs e)
        {
            ((PlayViewModel)DataContext).KeyDown(e.Key);
        }

        /// <summary>
        /// Event handler for the LostFocus event.
        /// </summary>
        /// <param name="sender">The object raising the event.</param>
        /// <param name="e">The event arguments provided.</param>
        public void LostFocusHandler(object sender, EventArgs e)
        {
            ((PlayViewModel)DataContext).IsActive = false;
        }

        /// <summary>
        /// Event handler for the GotFocus event.
        /// </summary>
        /// <param name="sender">The object raising the event.</param>
        /// <param name="e">The event arguments provided.</param>
        public void GotFocusHandler(object sender, EventArgs e)
        {
            ((PlayViewModel)DataContext).IsActive = true;
        }
    }
}
