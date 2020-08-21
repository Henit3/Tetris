using System.Windows;
using System.Windows.Controls;

namespace Tetris
{
    /// <summary>
    /// The class bound to the WPF page to be displayed in the main menu state.
    /// </summary>
    public partial class MenuPage : Page
    {
        /// <summary>
        /// Constructor to bind the Page with the appropriate ViewModel instance.
        /// </summary>
        public MenuPage()
        {
            InitializeComponent();
            DataContext = new MenuViewModel();
        }

        /// <summary>
        /// Event handler for the ButtonClick event, sets the application state to "play".
        /// </summary>
        /// <param name="sender">The object raising the event.</param>
        /// <param name="e">The event arguments provided.</param>
        public void ButtonClickHandler(object sender, RoutedEventArgs e)
        {
            ((MainWindow)App.Current.MainWindow).PushReflectState(typeof(PlayState));
        }
    }
}
