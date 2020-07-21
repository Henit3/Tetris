using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using Tetris;

namespace TetrisTests
{
    public class RotationTests: Tests
    {
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

        private void TestKick(TestData testCase)
        {
            Queue<Tetrimino> queue = new Queue<Tetrimino>(testCase.TestPiece);
            SetUpArena(testCase.InputArena);
            vModel.Session = new Game(vModel.Arena, testCase.SpawnPoint, queue);

            StartGame();
            Rotate(testCase.Rotation);
            SetPiece();

            AssertArenaState(testCase.OutputArena, testCase.TestPiece[0].Colour);
        }

        private void AssertArenaState(string[] arena, Brush pieceColour)
        {
            for (int row = 0; row < arena.Length; row++)
            {
                for (int col = 0; col < arena[row].Length; col++)
                {
                    Assert.AreEqual((arena[row][col]) switch
                    {
                        ' ' => Grid.DefaultFill,
                        '#' => Brushes.Black,
                        'O' => pieceColour,
                        _ => throw new ArgumentException(),
                    }, vModel.Arena.Cells[arena.Length - row - 1][col].Fill);
                }
            }
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
        public void TestKicksDefault0()
        {
            TestData[] testCases = new TestData[]
            {
                new TestData // (0, 0)
                (
                    new Tetrimino[] { Tetrimino.Types['L'] },
                    null, new Point(3, 2), 1, new string[]
                    {
                        "    O     ",
                        "    O     ",
                        "    OO    "
                    }
                ), new TestData // (-1, 0)
                (
                    new Tetrimino[] { Tetrimino.Types['L'] },
                    new string[]
                    {
                        "     #    "
                    }, new Point(3, 2), 1, new string[]
                    {
                        "   O     ",
                        "   O     ",
                        "   OO#   "
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
                        "   O      ",
                        "   O#     ",
                        "   OO     "
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
                        "    O     ",
                        "    O     ",
                        "    OO    "
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
                        "   O#     ",
                        "   O      ",
                        "   OO     "
                    }
                )
            };

            foreach (TestData testCase in testCases)
            {
                TestKick(testCase);
            }
        }

        [Test, Apartment(ApartmentState.STA)]
        public void TestKicksDefaultR()
        {
            Tetrimino LRight = new Tetrimino('L', Brushes.Orange, Tetrimino.DefaultOffsets, new Point[][]
            {
                new Point[4] { // L on its left
                    new Point(0, -1),
                    new Point(1, -1),
                    new Point(2, 0),
                    new Point(2, -1)
                }, new Point[4] { // L upright
                    new Point(1, 0),
                    new Point(1, -1),
                    new Point(1, -2),
                    new Point(2, -2)
                }, new Point[4] { // L on its right
                    new Point(0, -1),
                    new Point(0, -2),
                    new Point(1, -1),
                    new Point(2, -1)
                }, new Point[4] { // L upside down
                    new Point(0, 0),
                    new Point(1, 0),
                    new Point(1, -1),
                    new Point(1, -2)
                }
            }, null, 1);

            TestData[] testCases = new TestData[]
            {
                new TestData // (0, 0)
                (
                    new Tetrimino[] { LRight },
                    null, new Point(3, 2), 1, new string[]
                    {
                        "   OOO    ",
                        "   O      "
                    }
                ), new TestData // (-1, 0)
                (
                    new Tetrimino[] { LRight },
                    new string[]
                    {
                        "   #      "
                    }, new Point(3, 2), 1, new string[]
                    {
                        "    OOO   ",
                        "   #O     "
                    }
                ), new TestData // (-1, 1)
                (
                    new Tetrimino[] { LRight },
                    new string[]
                    {
                        "     #    ",
                        "   #      ",
                        "          "
                    }, new Point(3, 3), 1, new string[]
                    {
                        "     #    ",
                        "   #OOO   ",
                        "    O     "
                    }
                ), new TestData // (0, -2)
                (
                    new Tetrimino[] { LRight },
                    new string[]
                    {
                        "   # #    ",
                        "   #      ",
                        "    #     "
                    }, new Point(3, 3), 1, new string[]
                    {
                        "   OOO    ",
                        "   O      ",
                        "   # #    ",
                        "   #      ",
                        "    #     "
                    }
                ), new TestData // (-1, -2)
                (
                    new Tetrimino[] { LRight },
                    new string[]
                    {
                        "   # #    ",
                        "     #    ",
                        "   #      ",
                        "    #     "
                    }, new Point(3, 3), 1, new string[]
                    {
                        "    OOO   ",
                        "   #O#    ",
                        "     #    ",
                        "   #      ",
                        "    #     "
                    }
                )
            };

            foreach (TestData testCase in testCases)
            {
                TestKick(testCase);
            }
        }

        [Test, Apartment(ApartmentState.STA)]
        public void TestKicksDefault2()
        {
            Tetrimino LUpsideDown = new Tetrimino('L', Brushes.Orange, Tetrimino.DefaultOffsets, new Point[][]
            {
                new Point[4] { // L on its left
                    new Point(0, -1),
                    new Point(1, -1),
                    new Point(2, 0),
                    new Point(2, -1)
                }, new Point[4] { // L upright
                    new Point(1, 0),
                    new Point(1, -1),
                    new Point(1, -2),
                    new Point(2, -2)
                }, new Point[4] { // L on its right
                    new Point(0, -1),
                    new Point(0, -2),
                    new Point(1, -1),
                    new Point(2, -1)
                }, new Point[4] { // L upside down
                    new Point(0, 0),
                    new Point(1, 0),
                    new Point(1, -1),
                    new Point(1, -2)
                }
            }, null, 2);

            TestData[] testCases = new TestData[]
            {
                new TestData // (0, 0)
                (
                    new Tetrimino[] { LUpsideDown },
                    null, new Point(3, 2), 1, new string[]
                    {
                        "   OO     ",
                        "    O     ",
                        "    O     "
                    }
                ), new TestData // (-1, 0)
                (
                    new Tetrimino[] { LUpsideDown },
                    new string[]
                    {
                        "   #      ",
                        "          ",
                        "          "
                    }, new Point(3, 2), 1, new string[]
                    {
                        "   #OO    ",
                        "     O    ",
                        "     O    "
                    }
                ), new TestData // (-1, 1)
                (
                    new Tetrimino[] { LUpsideDown },
                    new string[]
                    {
                        "   ##     ",
                        "          ",
                        "          "
                    }, new Point(3, 2), 1, new string[]
                    {
                        "    OO    ",
                        "   ##O    ",
                        "     O    ",
                        "          "
                    }
                ), new TestData // (0, -2)
                (
                    new Tetrimino[] { LUpsideDown },
                    new string[]
                    {
                        "   ###    ",
                        "          ",
                        "          ",
                        "          ",
                        "          "
                    }, new Point(3, 4), 1, new string[]
                    {
                        "   ###    ",
                        "          ",
                        "   OO     ",
                        "    O     ",
                        "    O     "
                    }
                ), new TestData // (-1, -2)
                (
                    new Tetrimino[] { LUpsideDown },
                    new string[]
                    {
                        "   ###    ",
                        "          ",
                        "          ",
                        "    #     ",
                        "          "
                    }, new Point(3, 4), 1, new string[]
                    {
                        "   ###    ",
                        "          ",
                        "    OO    ",
                        "    #O    ",
                        "     O    "
                    }
                )
            };

            foreach (TestData testCase in testCases)
            {
                TestKick(testCase);
            }
        }

        [Test, Apartment(ApartmentState.STA)]
        public void TestKicksDefaultL()
        {
            Tetrimino LLeft = new Tetrimino('L', Brushes.Orange, Tetrimino.DefaultOffsets, new Point[][]
            {
                new Point[4] { // L on its left
                    new Point(0, -1),
                    new Point(1, -1),
                    new Point(2, 0),
                    new Point(2, -1)
                }, new Point[4] { // L upright
                    new Point(1, 0),
                    new Point(1, -1),
                    new Point(1, -2),
                    new Point(2, -2)
                }, new Point[4] { // L on its right
                    new Point(0, -1),
                    new Point(0, -2),
                    new Point(1, -1),
                    new Point(2, -1)
                }, new Point[4] { // L upside down
                    new Point(0, 0),
                    new Point(1, 0),
                    new Point(1, -1),
                    new Point(1, -2)
                }
            }, null, 3);

            TestData[] testCases = new TestData[]
            {
                new TestData // (0, 0)
                (
                    new Tetrimino[] { LLeft },
                    new string[]
                    {
                        "     #    "
                    }, new Point(3, 2), 1, new string[]
                    {
                        "     O    ",
                        "   OOO    ",
                        "     #    "
                    }
                ), new TestData // (-1, 0)
                (
                    new Tetrimino[] { LLeft },
                    new string[]
                    {
                        "     #    ",
                        "     #    "
                    }, new Point(3, 2), 1, new string[]
                    {
                        "    O#    ",
                        "  OOO#    "
                    }
                ), new TestData // (-1, 1)
                (
                    new Tetrimino[] { LLeft },
                    new string[]
                    {
                        "     #    ",
                        "  #  #    "
                    }, new Point(3, 2), 1, new string[]
                    {
                        "    O     ",
                        "  OOO#    ",
                        "  #  #    "
                    }
                ), new TestData // (0, -2)
                (
                    new Tetrimino[] { LLeft },
                    new string[]
                    {
                        "     #   ",
                        "  #  #    ",
                        "  #  #    "
                    }, new Point(3, 2), 1, new string[]
                    {
                        "     O    ",
                        "   OOO    ",
                        "     #    ",
                        "  #  #    ",
                        "  #  #    "
                    }
                ), new TestData // (-1, -2)
                (
                    new Tetrimino[] { LLeft },
                    new string[]
                    {
                        "     #    ",
                        "  #  #    ",
                        "  #  #    ",
                        "  #  #    "
                    }, new Point(3, 2), 1, new string[]
                    {
                        "    O     ",
                        "  OOO#    ",
                        "  #  #    ",
                        "  #  #    ",
                        "  #  #    "
                    }
                )
            };

            foreach (TestData testCase in testCases)
            {
                TestKick(testCase);
            }
        }
    }
}