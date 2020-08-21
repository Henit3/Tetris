using System.Windows.Controls;

namespace Tetris
{
    /// <summary>
    /// The state associated with the main menu. Mediates between the Page and the Frame.
    /// </summary>
    public class MenuState : IState
    {
        /// <summary>
        /// The page this state manages.
        /// </summary>
        public readonly MenuPage Page;
        /// <summary>
        /// The frame this state manages.
        /// </summary>
        private readonly Frame frame;

        /// <summary>
        /// Constructor to initialize both the Page and Frame that it will mediate.
        /// </summary>
        /// <param name="window">The window whose frame it will manage.</param>
        public MenuState(MainWindow window)
        {
            Page = new MenuPage();
            frame = window.ContentFrame;
        }

        /// <summary>
        /// Displays the Page on the Frame when pushing onto the stateStack.
        /// </summary>
        public void Enter()
        {
            frame.NavigationService.Navigate(Page);
        }

        /// <summary>
        /// Behaviour to be executed when popped off the stateStack.
        /// </summary>
        /// <remarks>This remains unimplemented.</remarks>
        public void Exit() { }
    }
}
