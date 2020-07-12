using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using Tetris;

namespace TetrisTests
{
    public class Tests
    {
        private MainWindow MWindow;
        private ViewModel VModel;

        [SetUp]
        public void Setup()
        {
            MWindow = new MainWindow();
            VModel = (ViewModel)MWindow.DataContext;
        }

        [Test, Apartment(ApartmentState.STA)]
        public void TestBagGeneration()
        {
            HashSet<Tetrimino> pieces = new HashSet<Tetrimino>();
            for (int i = 0; i < 7; i++)
            {
                VModel.KeyDown(Key.Enter);
                pieces.Add(VModel.Session.CurrentPiece);
            }
            Assert.AreEqual(pieces.Count, 7);
        }

        [Test, Apartment(ApartmentState.STA)]
        public void TestPieceRotation()
        {
            HashSet<Tetrimino> pieces = new HashSet<Tetrimino>();
            do
            {
                VModel.KeyDown(Key.Enter);
                Tetrimino piece = VModel.Session.CurrentPiece;
                if (pieces.Contains(VModel.Session.CurrentPiece))
                {
                    continue;
                }
                if (piece.Type != 'O' && piece.Type != 'I')
                {
                    Point[][] states = new Point[4][];
                    states[0] = piece.CurrentState;
                    int stateNo = 1;
                    // Get states during clockwise rotation
                    for (; stateNo < 4; stateNo++)
                    {
                        VModel.KeyDown(Key.Right);
                        states[stateNo] = piece.CurrentState;
                    }
                    // Check states during counterclockwise rotation
                    for (stateNo = 3; stateNo >= 0; stateNo--)
                    {
                        Assert.AreEqual(states[stateNo], piece.CurrentState);
                        VModel.KeyDown(Key.Left);
                    }
                }
                pieces.Add(piece);
            } while (pieces.Count < 7);
        }

        [Test, Apartment(ApartmentState.STA)]
        public void TestPieceRotationReset()
        {
            VModel.KeyDown(Key.Enter);
            Tetrimino referencePiece = VModel.Session.CurrentPiece;
            VModel.KeyDown(Key.Left);
            Point[] rotatedState = referencePiece.CurrentState;
            do
            {
                VModel.KeyDown(Key.Enter);
            } while (referencePiece.Type == VModel.Session.CurrentPiece.Type);
            Assert.AreNotEqual(rotatedState, VModel.Session.CurrentPiece.CurrentState);
        }

        [Test, Apartment(ApartmentState.STA)]
        public void TestWindowInFocus()
        {
            // Defaults to being enabled
            Assert.True(VModel.IsActive);

            // Behaviour when put into focus
            MWindow.Show();
            Assert.True(MWindow.IsKeyboardFocused);
            Assert.True(VModel.IsActive);
        }

        [Test, Apartment(ApartmentState.STA)]
        public void TestWindowOutOfFocus()
        {
            // Behaviour when put into focus
            MWindow.Show();
            Assert.True(MWindow.IsKeyboardFocused);
            Assert.True(VModel.IsActive);

            // Behaviour when put out of focus
            MWindow.Hide();
            Assert.False(MWindow.IsKeyboardFocused);
            Assert.False(VModel.IsActive);
        }
    }
}