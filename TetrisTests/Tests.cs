using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Tetris;

namespace TetrisTests
{
    public class Tests
    {
        private MainWindow mWindow;
        private ViewModel vModel;

        private void SpawnPiece()
        {
            vModel.KeyDown(Key.S);
        }
        private void SetPiece()
        {
            vModel.KeyDown(Key.W);
        }

        [SetUp]
        public void Setup()
        {
            mWindow = new MainWindow();
            vModel = (ViewModel)mWindow.DataContext;
        }

        [Test, Apartment(ApartmentState.STA)]
        public void TestBagGeneration()
        {
            HashSet<Tetrimino> pieces = new HashSet<Tetrimino>();
            for (int i = 0; i < 7; i++)
            {
                SpawnPiece();
                pieces.Add(vModel.Session.CurrentPiece);
                SetPiece();
            }
            Assert.AreEqual(pieces.Count, 7);
        }

        [Test, Apartment(ApartmentState.STA)]
        public void TestPieceRotationAllStates()
        {
            Queue<Tetrimino> queue = new Queue<Tetrimino>(Tetrimino.Types.Values.ToArray());
            vModel.Session = new Game(vModel.Arena, null, queue);

            HashSet<Tetrimino> pieces = new HashSet<Tetrimino>();
            for (int i = 0; i < 7; i++)
            {
                SpawnPiece();
                Tetrimino piece = vModel.Session.CurrentPiece;
                Point[][] states = new Point[4][];
                states[0] = piece.CurrentState;
                int stateNo = 1;
                // Get states during clockwise rotation
                for (; stateNo < 4; stateNo++)
                {
                    vModel.KeyDown(Key.Right);
                    states[stateNo] = piece.CurrentState;
                }
                // Check states during counterclockwise rotation
                for (stateNo = 3; stateNo >= 0; stateNo--)
                {
                    Assert.AreEqual(states[stateNo], piece.CurrentState);
                    vModel.KeyDown(Key.Left);
                }
                pieces.Add(piece);
                SetPiece();
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
            vModel.Session = new Game(vModel.Arena, null, queue);

            SpawnPiece();
            Tetrimino referencePiece = vModel.Session.CurrentPiece;
            vModel.KeyDown(Key.Left);
            SetPiece();
            Point[] rotatedState = referencePiece.CurrentState;
            // Shuffle back to the first piece
            SpawnPiece();
            SetPiece();
            SpawnPiece();
            Assert.AreNotEqual(rotatedState, vModel.Session.CurrentPiece.CurrentState);
        }

        [Test, Apartment(ApartmentState.STA)]
        public void TestPieceMovement()
        {
            SpawnPiece();
            Point initialPosition = (Point)vModel.Session.CurrentPiece.Position;
            vModel.KeyDown(Key.A);
            Assert.AreEqual((Point)vModel.Session.CurrentPiece.Position,
                initialPosition + new Vector(-1, 0));
            vModel.KeyDown(Key.D);
            Assert.AreEqual((Point)vModel.Session.CurrentPiece.Position,
                initialPosition);
        }

        [Test, Apartment(ApartmentState.STA)]
        public void TestHardDrop()
        {
            Queue<Tetrimino> queue = new Queue<Tetrimino>(new Tetrimino[]
            {
                Tetrimino.Types['O']
            });
            vModel.Session = new Game(vModel.Arena, null, queue);

            SpawnPiece();
            Point initialPosition = (Point)vModel.Session.CurrentPiece.Position;
            Brush colour = vModel.Session.CurrentPiece.Colour;
            SetPiece();
            Assert.AreEqual(colour, vModel.Arena.Cells[0][(int)initialPosition.X].Fill);
        }

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