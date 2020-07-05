using System.Windows;

namespace Tetris
{
    /// <summary>
    /// The class bound to the WPF view.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Constructor to bind the MainWindow with a new ViewModel instance.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ViewModel();
        }
    }
}
