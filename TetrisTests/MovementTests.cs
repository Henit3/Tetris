using NUnit.Framework;
using NUnit.Framework.Internal;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using Tetris;

namespace TetrisTests
{
    public class MovementTests: Tests
    {
        [Test, Apartment(ApartmentState.STA)]
        public void TestPieceMovement()
        {
            StartGame();
            Point initialPosition = (Point)vModel.Session.CurrentPiece.Position;
            Move(-1);
            Assert.AreEqual((Point)vModel.Session.CurrentPiece.Position,
                initialPosition + new Vector(-1, 0));
            Move(1);
            Assert.AreEqual((Point)vModel.Session.CurrentPiece.Position,
                initialPosition);
        }

        [Test, Apartment(ApartmentState.STA)]
        public void TestPieceHardDrop()
        {
            Queue<Tetrimino> queue = new Queue<Tetrimino>(new Tetrimino[]
            {
                Tetrimino.Types['J']
            });
            vModel.Session = new Game(vModel.Arena, null, queue);

            StartGame();
            Point initialPosition = (Point)vModel.Session.CurrentPiece.Position;
            Brush colour = vModel.Session.CurrentPiece.Colour;
            SetPiece();
            Assert.AreEqual(colour, vModel.Arena.Cells[0][(int)initialPosition.X].Fill);
        }

        [Test, Apartment(ApartmentState.STA)]
        public void TestPieceAboveGrid()
        {
            Queue<Tetrimino> queue = new Queue<Tetrimino>(new Tetrimino[]
            {
                Tetrimino.Types['L'],
                Tetrimino.Types['S']
            });
            for (int col = 0; col < vModel.Arena.Cols - 1; col++)
            {
                vModel.Arena.Cells[17][col].Fill = Brushes.Black;
            }
            vModel.Session = new Game(vModel.Arena, null, queue);

            StartGame();
            Rotate(1);
            Move(-3);
            SetPiece();
            StepGame();
            Rotate(1);
            Move(5);
            Assert.AreEqual(0, vModel.Session.Lines);
            SetPiece();

            Assert.AreEqual(1, vModel.Session.Lines);
            Assert.AreEqual(Tetrimino.Types['L'].Colour, vModel.Arena.Cells[19][0].Fill);
        }
    }
}