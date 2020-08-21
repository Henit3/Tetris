using NUnit.Framework;
using NUnit.Framework.Internal;
using System.Collections.Generic;
using System.Threading;
using Tetris;

namespace TetrisTests
{
    public class GridTests : GameTests
    {
        [Test, Apartment(ApartmentState.STA)]
        public void TestLineClearSingle()
        {
            Queue<Tetrimino> queue = new Queue<Tetrimino>(new Tetrimino[]
            {
                Tetrimino.Types['I']
            });
            SetUpArena(new string[]
            {
                " #########"
            });
            vModel.Session = new Game(vModel.Arena, null, queue);

            StartGame();
            Rotate(-1);
            Move(-5);
            Assert.AreEqual(0, vModel.Session.Lines);
            SetPiece();
            Assert.AreEqual(1, vModel.Session.Lines);
        }

        [Test, Apartment(ApartmentState.STA)]
        public void TestLineClearMultiple()
        {
            Queue<Tetrimino> queue = new Queue<Tetrimino>(new Tetrimino[]
            {
                Tetrimino.Types['I']
            });
            SetUpArena(new string[]
            {
                " #########",
                " #########",
                " #########",
                " #########"
            });
            vModel.Session = new Game(vModel.Arena, null, queue);

            StartGame();
            Rotate(-1);
            Move(-5);
            Assert.AreEqual(0, vModel.Session.Lines);
            SetPiece();
            Assert.AreEqual(4, vModel.Session.Lines);
        }
    }
}