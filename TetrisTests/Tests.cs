using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
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
        public void TestPieceRotationAllStates()
        {
            Queue<Tetrimino> queue = new Queue<Tetrimino>(Tetrimino.Types.Values.ToArray());
            VModel.Session = new Game(VModel.Arena, null, queue);
            HashSet<Tetrimino> pieces = new HashSet<Tetrimino>();
            for (int i = 0; i < 7; i++)
            {
                VModel.KeyDown(Key.Enter);
                Tetrimino piece = VModel.Session.CurrentPiece;
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
                pieces.Add(piece);
            }
            Assert.AreEqual(7, pieces.Count);
        }

        [Test, Apartment(ApartmentState.STA)]
        public void TestPieceRotationReset()
        {
            Queue<Tetrimino> queue = new Queue<Tetrimino>(new Tetrimino[]
            {
                Tetrimino.Types['S'],
                Tetrimino.Types['Z'],
                Tetrimino.Types['S']
            });
            VModel.Session = new Game(VModel.Arena, null, queue);
            VModel.KeyDown(Key.Enter);
            Tetrimino referencePiece = VModel.Session.CurrentPiece;
            VModel.KeyDown(Key.Left);
            Point[] rotatedState = referencePiece.CurrentState;
            // Shuffle back to the first piece
            VModel.KeyDown(Key.Enter);
            VModel.KeyDown(Key.Enter);
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