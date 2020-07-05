using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Tetris;

namespace TetrisTests
{
    public class Tests
    {
        private MainWindow MWindow;
        private ViewModel VModel;
        private readonly BrushConverter ColourConvertor = new BrushConverter();

        [SetUp]
        public void Setup()
        {
            MWindow = new MainWindow();
            VModel = (ViewModel)MWindow.DataContext;
        }

        private void TestKeyPress(Key key)
        {
            VModel.KeyDown(null, new KeyEventArgs(
                Keyboard.PrimaryDevice,
                new Mock<PresentationSource>().Object,
                0,
                key));
            Assert.AreEqual(VModel.Output, "Key (" + key.ToString() + ") Down");
            Assert.AreEqual(VModel.Arena.Cells[0][0].Fill,
                (Brush) ColourConvertor.ConvertFromString(
                    VModel.KeyColourMap.GetValueOrDefault(key) ?? "Black"));
        }

        [Test, Apartment(ApartmentState.STA)]
        public void TestNoKeyPressed()
        {
            Assert.AreEqual(VModel.Output, "Key () Down");
            Assert.AreEqual(VModel.Arena.Cells[0][0].Fill, Brushes.Transparent);
        }

        [Test, Apartment(ApartmentState.STA)]
        public void TestMultipleKeyPresses()
        {
            // Keys to test key press for
            Key[] keys = { Key.W, Key.A, Key.S, Key.D, Key.Space, Key.NumPad0, Key.D0 };
            foreach (Key key in keys)
            {
                TestKeyPress(key);
            }
        }

        [Test, Apartment(ApartmentState.STA)]
        public void TestWindowInFocus()
        {
            // Defaults to being enabled
            Assert.True(VModel.OutEnabled);

            // Behaviour when put into focus
            MWindow.Show();
            Assert.True(MWindow.IsKeyboardFocused);
            Assert.True(VModel.OutEnabled);
        }

        [Test, Apartment(ApartmentState.STA)]
        public void TestWindowOutOfFocus()
        {
            // Behaviour when put into focus
            MWindow.Show();
            Assert.True(MWindow.IsKeyboardFocused);
            Assert.True(VModel.OutEnabled);

            // Behaviour when put out of focus
            MWindow.Hide();
            Assert.False(MWindow.IsKeyboardFocused);
            Assert.False(VModel.OutEnabled);
        }
    }
}