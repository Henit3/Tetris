using System.ComponentModel;
using System.Windows.Input;

namespace Tetris
{
    /// <summary>
    /// The ViewModel linked to the play view.
    /// Handles the logic and mediates between the view and model.
    /// </summary>
    public class PlayViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Handles cases where a property is changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Value indicating if output is enabled - assists the property.
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
        /// Makes use of the property changed handler if it exists.
        /// </summary>
        /// <param name="info">The information passed about the property.</param>
        private void OnPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        /// <summary>
        /// Basic constructor for the PlayViewModel.
        /// </summary>
        /// <param name="parent">The parent page to access named elements.</param>
        public PlayViewModel(PlayPage parent)
        {
            Arena = new Grid(20, 10, parent.ArenaCanvas);
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
        /// Logic to handle the KeyDown event using the key.
        /// </summary>
        /// <remarks>
        /// This is separate from the event handler to allow tests to easily mimic key input.
        /// Being in the ViewModel also allows it to more easily access the Session.
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
                    //MessageBox.Show(key.ToString());
                    break;
            }
        }
    }
}