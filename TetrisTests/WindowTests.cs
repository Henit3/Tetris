using NUnit.Framework;
using NUnit.Framework.Internal;
using System.Threading;

namespace TetrisTests
{
    public class WindowTests: Tests
    {
        [Test, Apartment(ApartmentState.STA)]
        public void TestWindowInFocus()
        {
            // Defaults to being enabled
            Assert.True(vModel.IsActive);

            // Behaviour when put into focus
            mWindow.Show();
            Assert.True(mWindow.IsKeyboardFocused);
            Assert.True(vModel.IsActive);
        }

        [Test, Apartment(ApartmentState.STA)]
        public void TestWindowOutOfFocus()
        {
            // Behaviour when put into focus
            mWindow.Show();
            Assert.True(mWindow.IsKeyboardFocused);
            Assert.True(vModel.IsActive);

            // Behaviour when put out of focus
            mWindow.Hide();
            Assert.False(mWindow.IsKeyboardFocused);
            Assert.False(vModel.IsActive);
        }
    }
}