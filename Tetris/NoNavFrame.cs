using System.Windows.Controls;

namespace Tetris
{
    /// <summary>
    /// Extension of the Frame User Control to disallow unwanted user navigation.
    /// </summary>
    public class NoNavFrame : Frame
    {
        /// <summary>
        /// Constructor to attach a method to remove the old history on navigation.
        /// </summary>
        public NoNavFrame()
        {
            this.Navigated += new System.Windows.Navigation.NavigatedEventHandler(
                (sender, e) => { this.NavigationService.RemoveBackEntry(); }
            );
        }
    }
}
