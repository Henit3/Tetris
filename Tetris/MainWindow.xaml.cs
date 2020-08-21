using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace Tetris
{
    /// <summary>
    /// The class bound to the WPF window, containing a frame to display all content.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// A Stack to hold the states of the application, asssisting navigation.
        /// </summary>
        public Stack<IState> stateStack = new Stack<IState>();

        /// <summary>
        /// Constructor to bind the MainWindow with a new ViewModel instance.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            PushState(new MenuState(this));
        }

        /// <summary>
        /// Uses reflection to take in a type of state and construct it before calling PushState.
        /// </summary>
        /// <param name="type">The type (class) of state to push on.</param>
        /// <see cref="PushState(IState)"/>
        public void PushReflectState(Type type)
        {
            PushState((IState)Activator.CreateInstance(type, this));
        }

        /// <summary>
        /// Takes in a state instance, invokes its Enter method and pushes it onto the stateStack.
        /// </summary>
        /// <param name="type">The state to push onto the stateStack.</param>
        public void PushState(IState state)
        {
            stateStack.Push(state);
            state.Enter();
        }

        /// <summary>
        /// Pops the topmost state off the stateStack, invoking the appropriate Enter and Exit methods.
        /// </summary>
        /// <returns>The state that was popped off the stateStack.</returns>
        public IState PopState()
        {
            IState state = stateStack.Pop();
            state.Exit();
            stateStack.Peek().Enter();
            return state;
        }

        /// <summary>
        /// Redirects KeyDown events to any handler in the ContentFrame.
        /// </summary>
        /// <param name="sender">The object raising the event.</param>
        /// <param name="e">The event arguments provided.</param>
        /// <see cref="ReflectHandler(string, object[])"/>
        public void KeyDownHandler(object sender, KeyEventArgs e)
        {
            ReflectHandler("KeyDownHandler", new[] { sender, e });
        }

        /// <summary>
        /// Redirects LostFocus events to any handler in the ContentFrame.
        /// </summary>
        /// <param name="sender">The object raising the event.</param>
        /// <param name="e">The event arguments provided.</param>
        /// <see cref="ReflectHandler(string, object[])"/>
        public void LostFocusHandler(object sender, EventArgs e)
        {
            ReflectHandler("LostFocusHandler", new[] { sender, e });
        }

        /// <summary>
        /// Redirects GotFocus events to any handler in the ContentFrame.
        /// </summary>
        /// <param name="sender">The object raising the event.</param>
        /// <param name="e">The event arguments provided.</param>
        /// <see cref="ReflectHandler(string, object[])"/>
        public void GotFocusHandler(object sender, EventArgs e)
        {
            ReflectHandler("GotFocusHandler", new[] { sender, e });
        }

        /// <summary>
        /// Uses reflection to delegate the event if the appropriate event handler exists.
        /// </summary>
        /// <param name="sender">The expected name of the event handler.</param>
        /// <param name="param">The arguments to pass to the event handler.</param>
        private void ReflectHandler(string name, object[] param)
        {
            Type type = stateStack.Peek().GetType();
            MethodInfo handler = type.GetMethod(name);
            if (handler != null)
            {
                handler.Invoke(stateStack.Peek(), param);
            }
        }
    }
}
