namespace Tetris
{
    /// <summary>
    /// An interface to be implemented by all application states.
    /// </summary>
    public interface IState
    {
        /// <summary>
        /// Behaviour to be executed upon pushing onto the stateStack.
        /// </summary>
        public void Enter();
        /// <summary>
        /// Behaviour to be executed when popped off the stateStack.
        /// </summary>
        public void Exit();
    }
}
