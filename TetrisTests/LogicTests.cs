using NUnit.Framework;
using NUnit.Framework.Internal;
using System.Collections.Generic;
using System.Threading;
using Tetris;

namespace TetrisTests
{
    public class LogicTests : GameTests
    {
        [Test, Apartment(ApartmentState.STA)]
        public void TestBagGeneration()
        {
            StartGame();
            HashSet<Tetrimino> pieces = new HashSet<Tetrimino>();
            for (int i = 0; i < 7; i++)
            {
                pieces.Add(vModel.Session.CurrentPiece);
                SetPiece();
                vModel.Arena.Reset();
                StepGame();
            }
            Assert.AreEqual(7, pieces.Count);
        }
    }
}