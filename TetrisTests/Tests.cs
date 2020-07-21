using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
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

        private void StartGame()
        {
            vModel.Session.Start(true, true, false);
        }
        private void StepGame()
        {
            vModel.Session.GameLoop();
        }
        private void SetPiece()
        {
            vModel.KeyDown(Key.W);
        }
        private void Move(int x)
        {
            if (x == -1)
            {
                vModel.KeyDown(Key.A);
            }
            else if (x == 1)
            {
                vModel.KeyDown(Key.D);
            }
            else
            {
                throw new ArgumentException();
            }
        }
        private void Rotate(int x)
        {
            if (x == -1)
            {
                vModel.KeyDown(Key.Left);
            }
            else if (x == 1)
            {
                vModel.KeyDown(Key.Right);
            }
            else
            {
                if (x == 0)
                {
                    return;
                }
                throw new ArgumentException();
            }
        }
        private void SetUpArena(string[] arena)
        {
            vModel.Arena.Reset();
            if (arena == null)
            {
                return;
            }
            for (int row = 0; row < arena.Length; row++)
            {
                for (int col = 0; col < arena[row].Length; col++)
                {
                    vModel.Arena.Cells[arena.Length - row - 1][col].Fill = (arena[row][col]) switch
                    {
                        ' ' => Grid.DefaultFill,
                        '#' => Brushes.Black,
                        _ => throw new ArgumentException(),
                    };
                }
            }
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
            StartGame();
            HashSet<Tetrimino> pieces = new HashSet<Tetrimino>();
            for (int i = 0; i < 7; i++)
            {
                pieces.Add(vModel.Session.CurrentPiece);
                SetPiece();
                vModel.Arena.Reset();
                StepGame();
            }
            Assert.AreEqual(pieces.Count, 7);
        }

        [Test, Apartment(ApartmentState.STA)]
        public void TestPieceRotationAllStates()
        {
            Queue<Tetrimino> queue = new Queue<Tetrimino>(Tetrimino.Types.Values.ToArray());
            vModel.Session = new Game(vModel.Arena, null, queue);

            StartGame();
            HashSet<Tetrimino> pieces = new HashSet<Tetrimino>();
            for (int i = 0; i < 7; i++)
            {
                Tetrimino piece = vModel.Session.CurrentPiece;
                Point[][] states = new Point[4][];
                states[0] = piece.CurrentState;
                int stateNo = 1;
                // Get states during clockwise rotation
                for (; stateNo < 4; stateNo++)
                {
                    Rotate(1);
                    states[stateNo] = piece.CurrentState;
                }
                // Check states during counterclockwise rotation
                for (stateNo = 3; stateNo >= 0; stateNo--)
                {
                    Assert.AreEqual(states[stateNo], piece.CurrentState);
                    Rotate(-1);
                }
                pieces.Add(piece);
                SetPiece();
                vModel.Arena.Reset();
                StepGame();
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

            StartGame();
            Tetrimino referencePiece = vModel.Session.CurrentPiece;
            Rotate(-1);
            Point[] rotatedState = referencePiece.CurrentState;
            SetPiece();
            StepGame();
            // Shuffle back to the first piece
            SetPiece();
            StepGame();
            Assert.AreNotEqual(rotatedState, vModel.Session.CurrentPiece.CurrentState);
        }

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
            for (int i = 0; i < 5; i++)
            {
                Move(-1);
            }
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
            for (int i = 0; i < 5; i++)
            {
                Move(-1);
            }
            Assert.AreEqual(0, vModel.Session.Lines);
            SetPiece();
            Assert.AreEqual(4, vModel.Session.Lines);
        }

        private void TestKick(TestData testCase)
        {
            // Give the queue input (holds piece)
            // Give the arena input (sets up environment)
            // Give the spawnpoint? (allows easier placement)
            // Give initial rotation?? (construct piece with initial state != 0)
            // Give the rotation (allows easy testing)
            // Give the expected output (asserts output)
            Queue<Tetrimino> queue = new Queue<Tetrimino>(testCase.TestPiece);
            SetUpArena(testCase.InputArena);
            vModel.Session = new Game(vModel.Arena, testCase.SpawnPoint, queue);

            StartGame();
            Rotate(testCase.Rotation);
            SetPiece();

            AssertArenaState(testCase.OutputArena);
        }

        private struct TestData
        {
            public Tetrimino[] TestPiece;
            public string[] InputArena;
            public Point SpawnPoint;
            public int Rotation;
            public string[] OutputArena;

            public TestData(Tetrimino[] testPiece, string[] inputArena, Point spawnPoint,
                int rotation, string[] outputArena)
            {
                TestPiece = testPiece;
                InputArena = inputArena;
                SpawnPoint = spawnPoint;
                Rotation = rotation;
                OutputArena = outputArena;
            }
        }

        [Test, Apartment(ApartmentState.STA)]
        public void TestKicksL()
        {
            TestData[] testCases = new TestData[]
            {
                new TestData // (0, 0)
                (
                    new Tetrimino[] { Tetrimino.Types['L'] },
                    null, new Point(3, 2), 1, new string[]
                    {
                        "    #     ",
                        "    #     ",
                        "    ##    "
                    }
                ), new TestData // (-1, 0)
                (
                    new Tetrimino[] { Tetrimino.Types['L'] },
                    new string[]
                    {
                        "     #    "
                    }, new Point(3, 2), 1, new string[]
                    {
                        "   #     ",
                        "   #     ",
                        "   ###   "
                    }
                ), new TestData // (-1, 1)
                (
                    new Tetrimino[] { Tetrimino.Types['L'] },
                    new string[]
                    {
                        "    #     ",
                        "          "
                    }, new Point(3, 1), 1, new string[]
                    {
                        "   #      ",
                        "   ##     ",
                        "   ##     "
                    }
                ), new TestData // (0, -2)
                (
                    new Tetrimino[] { Tetrimino.Types['L'] },
                    new string[]
                    {
                        "   ##     ",
                        "          ",
                        "          ",
                        "          ",
                        "          "
                    }, new Point(3, 4), 1, new string[]
                    {
                        "   ##     ",
                        "          ",
                        "    #     ",
                        "    #     ",
                        "    ##    "
                    }
                ), new TestData // (-1, -2)
                (
                    new Tetrimino[] { Tetrimino.Types['L'] },
                    new string[]
                    {
                        "   ##     ",
                        "          ",
                        "    #     ",
                        "          ",
                        "          "
                    }, new Point(3, 4), 1, new string[]
                    {
                        "   ##     ",
                        "          ",
                        "   ##     ",
                        "   #      ",
                        "   ##     "
                    }
                )
            };

            foreach (TestData testCase in testCases)
            {
                TestKick(testCase);
            }
        }

        private void AssertArenaState(string[] arena)
        {
            for (int row = 0; row < arena.Length; row++)
            {
                for (int col = 0; col < arena[row].Length; col++)
                {
                    if (arena[row][col] == '#')
                    {
                        Assert.AreNotEqual(Grid.DefaultFill,
                        vModel.Arena.Cells[arena.Length - row - 1][col].Fill);
                    }
                    else
                    {
                        Assert.AreEqual(Grid.DefaultFill,
                        vModel.Arena.Cells[arena.Length - row - 1][col].Fill);
                    }
                }
            }
        }



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