using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace Tetris
{
    /// <summary>
    /// The state associated with the play screen. Mediates between the Page and the Frame.
    /// </summary>
    public class PlayState : IState
    {
        /// <summary>
        /// The page this state manages.
        /// </summary>
        public readonly PlayPage Page;
        /// <summary>
        /// The frame this state manages.
        /// </summary>
        private readonly Frame frame;
        /// <summary>
        /// Whether the application is being tested; won't automatically start the game on entry.
        /// </summary>
        private readonly bool testing = false;

        /// <summary>
        /// Basic constructor to initialize both the Page and Frame that it will mediate.
        /// </summary>
        /// <param name="window">The window whose frame it will manage.</param>
        public PlayState(MainWindow window)
        {
            Page = new PlayPage();
            frame = window.ContentFrame;
        }

        /// <summary>
        /// Alterative constructor used in testing to initialize the testing field.
        /// </summary>
        /// <param name="window">The window whose frame it will manage.</param>
        /// <param name="testing">Whether the state is in a test environment.</param>
        public PlayState(MainWindow window, bool testing)
        {
            Page = new PlayPage();
            frame = window.ContentFrame;
            this.testing = testing;
        }

        /// <summary>
        /// Displays the Page on the Frame when pushing onto the stateStack.
        /// Also initializes the game unless run in a test environment.
        /// </summary>
        public void Enter()
        {
            frame.NavigationService.Navigate(Page);
            if (!testing)
            {
                ((PlayViewModel)Page.DataContext).Session.Start();
            }
        }

        /// <summary>
        /// Behaviour to be executed when popped off the stateStack.
        /// </summary>
        /// <remarks>This remains unimplemented.</remarks>
        public void Exit() { }

        /// <summary>
        /// Redirects KeyDown events to the handler in the Page.
        /// </summary>
        /// <param name="sender">The object raising the event.</param>
        /// <param name="e">The event arguments provided.</param>
        /// <remarks>
        /// The existance of this method indicates an implemented handler to the reflection system.
        /// </remarks>
        public void KeyDownHandler(object sender, KeyEventArgs e)
        {
            Page.KeyDownHandler(sender, e);
        }

        /// <summary>
        /// Redirects LostFocus events to the handler in the Page.
        /// </summary>
        /// <param name="sender">The object raising the event.</param>
        /// <param name="e">The event arguments provided.</param>
        /// <remarks>
        /// The existance of this method indicates an implemented handler to the reflection system.
        /// </remarks>
        public void LostFocusHandler(object sender, EventArgs e)
        {
            Page.LostFocusHandler(sender, e);
        }

        /// <summary>
        /// Redirects GotFocus events to the handler in the Page.
        /// </summary>
        /// <param name="sender">The object raising the event.</param>
        /// <param name="e">The event arguments provided.</param>
        /// <remarks>
        /// The existance of this method indicates an implemented handler to the reflection system.
        /// </remarks>
        public void GotFocusHandler(object sender, EventArgs e)
        {
            Page.GotFocusHandler(sender, e);
        }
    }
}
