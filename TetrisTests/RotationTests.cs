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
    public class RotationTests : Tests
    {
        private struct TestData
        {
            public string[] InputArena;
            public Point SpawnPoint;
            public string[] OutputArena;

            public TestData(string[] inputArena, Point spawnPoint, string[] outputArena)
            {
                InputArena = inputArena;
                SpawnPoint = spawnPoint;
                OutputArena = outputArena;
            }
        }

        private void TestKick(TestData testCase, Tetrimino testPiece, int rotation)
        {
            Queue<Tetrimino> queue = new Queue<Tetrimino>(new Tetrimino[] { testPiece });
            SetUpArena(testCase.InputArena);
            vModel.Session = new Game(vModel.Arena, testCase.SpawnPoint, queue);

            StartGame();
            Rotate(rotation);
            SetPiece();

            AssertArenaState(testCase.OutputArena, testPiece.Colour);
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

        private Tetrimino GetTetriminoRotated(char letter, int rotation = 0)
        {
            if (rotation == 0)
            {
                return Tetrimino.Types[letter];
            }
            return letter switch
            {
                'L' => new Tetrimino('L', Brushes.Orange, Tetrimino.DefaultOffsets, new Point[][]
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
                        }, null, rotation),
                'I' => new Tetrimino('I', Brushes.Cyan, new Vector[][]
                        {
                            new Vector[4] {
                                new Vector(0, 0),
                                new Vector(-1, 0),
                                new Vector(-1, 1),
                                new Vector(0, 1)
                            }, new Vector[4] {
                                new Vector(-1, 0),
                                new Vector(0, 0),
                                new Vector(1, 1),
                                new Vector(0, 1)
                            }, new Vector[4] {
                                new Vector(2, 0),
                                new Vector(0, 0),
                                new Vector(-2, 1),
                                new Vector(0, 1)
                            }, new Vector[4] {
                                new Vector(-1, 0),
                                new Vector(0, 1),
                                new Vector(1, 0),
                                new Vector(0, -1)
                            }, new Vector[4] {
                                new Vector(2, 0),
                                new Vector(0, -2),
                                new Vector(-2, 0),
                                new Vector(0, 2)
                            }
                        }, new Point[][]
                        {
                            new Point[4] { // Right
                                new Point(1, -2),
                                new Point(2, -2),
                                new Point(3, -2),
                                new Point(4, -2)
                            }, new Point[4] { // Down
                                new Point(2, -1),
                                new Point(2, -2),
                                new Point(2, -3),
                                new Point(2, -4)
                            }, new Point[4] { // Left
                                new Point(0, -2),
                                new Point(1, -2),
                                new Point(2, -2),
                                new Point(3, -2)
                            }, new Point[4] { // Up
                                new Point(2, 0),
                                new Point(2, -1),
                                new Point(2, -2),
                                new Point(2, -3)
                            }
                        }, new Vector(-1, 2), rotation),
                _ => throw new ArgumentException(),
            };
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
            Tetrimino testPiece = GetTetriminoRotated('L');
            TestData[] testCases = new TestData[]
            {
                new TestData // (0, 0)
                (
                    null, new Point(3, 2), new string[]
                    {
                        "    O     ",
                        "    O     ",
                        "    OO    "
                    }
                ), new TestData // (-1, 0)
                (
                    new string[]
                    {
                        "     #    "
                    }, new Point(3, 2), new string[]
                    {
                        "   O     ",
                        "   O     ",
                        "   OO#   "
                    }
                ), new TestData // (-1, 1)
                (
                    new string[]
                    {
                        "    #     ",
                        "          "
                    }, new Point(3, 1), new string[]
                    {
                        "   O      ",
                        "   O#     ",
                        "   OO     "
                    }
                ), new TestData // (0, -2)
                (
                    new string[]
                    {
                        "   ##     ",
                        "          ",
                        "          ",
                        "          ",
                        "          "
                    }, new Point(3, 4), new string[]
                    {
                        "   ##     ",
                        "          ",
                        "    O     ",
                        "    O     ",
                        "    OO    "
                    }
                ), new TestData // (-1, -2)
                (
                    new string[]
                    {
                        "   ##     ",
                        "          ",
                        "    #     ",
                        "          ",
                        "          "
                    }, new Point(3, 4), new string[]
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
                TestKick(testCase, testPiece, 1);
            }
        }

        [Test, Apartment(ApartmentState.STA)]
        public void TestKicksDefaultR()
        {
            Tetrimino testPiece = GetTetriminoRotated('L', 1);

            TestData[] testCases = new TestData[]
            {
                new TestData // (0, 0)
                (
                    null, new Point(3, 2), new string[]
                    {
                        "   OOO    ",
                        "   O      "
                    }
                ), new TestData // (1, 0)
                (
                    new string[]
                    {
                        "   #      "
                    }, new Point(3, 2), new string[]
                    {
                        "    OOO   ",
                        "   #O     "
                    }
                ), new TestData // (1, -1)
                (
                    new string[]
                    {
                        "     #    ",
                        "   #      ",
                        "          "
                    }, new Point(3, 3), new string[]
                    {
                        "     #    ",
                        "   #OOO   ",
                        "    O     "
                    }
                ), new TestData // (0, 2)
                (
                    new string[]
                    {
                        "   # #    ",
                        "   #      ",
                        "    #     "
                    }, new Point(3, 3), new string[]
                    {
                        "   OOO    ",
                        "   O      ",
                        "   # #    ",
                        "   #      ",
                        "    #     "
                    }
                ), new TestData // (1, 2)
                (
                    new string[]
                    {
                        "   # #    ",
                        "     #    ",
                        "   #      ",
                        "    #     "
                    }, new Point(3, 3), new string[]
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
                TestKick(testCase, testPiece, 1);
            }
        }

        [Test, Apartment(ApartmentState.STA)]
        public void TestKicksDefault2()
        {
            Tetrimino testPiece = GetTetriminoRotated('L', 2);

            TestData[] testCases = new TestData[]
            {
                new TestData // (0, 0)
                (
                    null, new Point(3, 2), new string[]
                    {
                        "   OO     ",
                        "    O     ",
                        "    O     "
                    }
                ), new TestData // (1, 0)
                (
                    new string[]
                    {
                        "   #      ",
                        "          ",
                        "          "
                    }, new Point(3, 2), new string[]
                    {
                        "   #OO    ",
                        "     O    ",
                        "     O    "
                    }
                ), new TestData // (1, 1)
                (
                    new string[]
                    {
                        "   ##     ",
                        "          ",
                        "          "
                    }, new Point(3, 2), new string[]
                    {
                        "    OO    ",
                        "   ##O    ",
                        "     O    ",
                        "          "
                    }
                ), new TestData // (0, -2)
                (
                    new string[]
                    {
                        "   ###    ",
                        "          ",
                        "          ",
                        "          ",
                        "          "
                    }, new Point(3, 4), new string[]
                    {
                        "   ###    ",
                        "          ",
                        "   OO     ",
                        "    O     ",
                        "    O     "
                    }
                ), new TestData // (1, -2)
                (
                    new string[]
                    {
                        "   ###    ",
                        "          ",
                        "          ",
                        "    #     ",
                        "          "
                    }, new Point(3, 4), new string[]
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
                TestKick(testCase, testPiece, 1);
            }
        }

        [Test, Apartment(ApartmentState.STA)]
        public void TestKicksDefaultL()
        {
            Tetrimino testPiece = GetTetriminoRotated('L', 3);

            TestData[] testCases = new TestData[]
            {
                new TestData // (0, 0)
                (
                    new string[]
                    {
                        "     #    "
                    }, new Point(3, 2), new string[]
                    {
                        "     O    ",
                        "   OOO    ",
                        "     #    "
                    }
                ), new TestData // (-1, 0)
                (
                    new string[]
                    {
                        "     #    ",
                        "     #    "
                    }, new Point(3, 2), new string[]
                    {
                        "    O#    ",
                        "  OOO#    "
                    }
                ), new TestData // (-1, -1)
                (
                    new string[]
                    {
                        "     #    ",
                        "  #  #    "
                    }, new Point(3, 2), new string[]
                    {
                        "    O     ",
                        "  OOO#    ",
                        "  #  #    "
                    }
                ), new TestData // (0, 2)
                (
                    new string[]
                    {
                        "     #   ",
                        "  #  #    ",
                        "  #  #    "
                    }, new Point(3, 2), new string[]
                    {
                        "     O    ",
                        "   OOO    ",
                        "     #    ",
                        "  #  #    ",
                        "  #  #    "
                    }
                ), new TestData // (-1, 2)
                (
                    new string[]
                    {
                        "     #    ",
                        "  #  #    ",
                        "  #  #    ",
                        "  #  #    "
                    }, new Point(3, 2), new string[]
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
                TestKick(testCase, testPiece, 1);
            }
        }


        [Test, Apartment(ApartmentState.STA)]
        public void TestKicksICW0()
        {
            Tetrimino testPiece = GetTetriminoRotated('I');
            // Spawnpoint (0, 0) off left of I

            TestData[] testCases = new TestData[]
            {
                new TestData // (0, 0)
                (
                    null, new Point(3, 2), new string[]
                    {
                        "     O    ",
                        "     O    ",
                        "     O    ",
                        "     O    "
                    }
                ), new TestData // (-2, 0)
                (
                    new string[]
                    {
                        "     #    "
                    }, new Point(3, 2), new string[]
                    {
                        "   O     ",
                        "   O     ",
                        "   O     ",
                        "   O #   "
                    }
                ), new TestData // (1, 0)
                (
                    new string[]
                    {
                        "   # #    "
                    }, new Point(3, 2), new string[]
                    {
                        "      O  ",
                        "      O  ",
                        "      O  ",
                        "   # #O  "
                    }
                ), new TestData // (-2, -1)
                (
                    new string[]
                    {
                        "   #      ",
                        "          ",
                        "          ",
                        "     ##   ",
                        "          "
                    }, new Point(3, 3), new string[]
                    {
                        "   #      ",
                        "   O      ",
                        "   O      ",
                        "   O ##   ",
                        "   O      "
                    }
                ), new TestData // (1, 2)
                (
                    new string[]
                    {
                        "   # ##   "
                    }, new Point(3, 2), new string[]
                    {
                        "      O   ",
                        "      O   ",
                        "      O   ",
                        "      O   ",
                        "   # ##   "
                    }
                )
            };

            foreach (TestData testCase in testCases)
            {
                TestKick(testCase, testPiece, 1);
            }
        }

        [Test, Apartment(ApartmentState.STA)]
        public void TestKicksICWR()
        {
            Tetrimino testPiece = GetTetriminoRotated('I', 1);
            // Spawnpoint (-1, 2) off bottom of I

            TestData[] testCases = new TestData[]
            {
                new TestData // (0, 0)
                (
                    new string[]
                    {
                        "   #      "
                    }, new Point(3, 2), new string[]
                    {
                        "  OOOO    ",
                        "   #      "
                    }
                ), new TestData // (-1, 0)
                (
                    new string[]
                    {
                        "     #    ",
                        "   #      "
                    }, new Point(3, 2), new string[]
                    {
                        " OOOO#    ",
                        "   #      "
                    }
                ), new TestData // (2, 0)
                (
                    new string[]
                    {
                        "   #      ",
                        "     #    "
                    }, new Point(3, 2), new string[]
                    {
                        "   #OOOO  ",
                        "     #    "
                    }
                ), new TestData // (-1, 2)
                (
                    new string[]
                    {
                        "   #      ",
                        "   # #    ",
                        "          "
                    }, new Point(3, 2), new string[]
                    {
                        " OOOO     ",
                        "   #      ",
                        "   # #    ",
                        "          "
                    }
                ), new TestData // (2, -1)
                (
                    new string[]
                    {
                        "   #      ",
                        "   #      ",
                        "   # #    ",
                        "          "
                    }, new Point(3, 2), new string[]
                    {
                        "   #      ",
                        "   #      ",
                        "   # #    ",
                        "    OOOO  "
                    }
                )
            };

            foreach (TestData testCase in testCases)
            {
                TestKick(testCase, testPiece, 1);
            }
        }

        [Test, Apartment(ApartmentState.STA)]
        public void TestKicksICW2()
        {
            Tetrimino testPiece = GetTetriminoRotated('I', 2);
            // Spawnpoint (1, 0) off left of I

            TestData[] testCases = new TestData[]
            {
                new TestData // (0, 0)
                (
                    null, new Point(3, 1), new string[]
                    {
                        "   O      ",
                        "   O      ",
                        "   O      ",
                        "   O      "
                    }
                ), new TestData // (2, 0)
                (
                    new string[]
                    {
                        "   #      "
                    }, new Point(3, 1), new string[]
                    {
                        "     O    ",
                        "     O    ",
                        "     O    ",
                        "   # O    "
                    }
                ), new TestData // (-1, 0)
                (
                    new string[]
                    {
                        "   # #    "
                    }, new Point(3, 1), new string[]
                    {
                        "  O       ",
                        "  O       ",
                        "  O       ",
                        "  O# #    "
                    }
                ), new TestData // (2, 1)
                (
                    new string[]
                    {
                        "  ## #    "
                    }, new Point(3, 1), new string[]
                    {
                        "     O    ",
                        "     O    ",
                        "     O    ",
                        "     O    ",
                        "  ## #    "
                    }
                ), new TestData // (-1, -2)
                (
                    new string[]
                    {
                        "  #  #    ",
                        "          ",
                        "   # #    ",
                        "          ",
                        "          "
                    }, new Point(3, 3), new string[]
                    {
                        "  #  #    ",
                        "  O       ",
                        "  O# #    ",
                        "  O       ",
                        "  O       "
                    }
                )
            };

            foreach (TestData testCase in testCases)
            {
                TestKick(testCase, testPiece, 1);
            }
        }

        [Test, Apartment(ApartmentState.STA)]
        public void TestKicksICWL()
        {
            Tetrimino testPiece = GetTetriminoRotated('I', 3);
            // Spawnpoint (-1, 1) off bottom of I

            TestData[] testCases = new TestData[]
            {
                new TestData // (0, 0)
                (
                    new string[]
                    {
                        "     #    ",
                        "          "
                    }, new Point(2, 1), new string[]
                    {
                        "  OOOO    ",
                        "     #    ",
                        "          "
                    }
                ), new TestData // (1, 0)
                (
                    new string[]
                    {
                        "  #       ",
                        "     #    ",
                        "          "
                    }, new Point(2, 1), new string[]
                    {
                        "  #OOOO   ",
                        "     #    ",
                        "          "
                    }
                ), new TestData // (-2, 0)
                (
                    new string[]
                    {
                        "    #     ",
                        " #        ",
                        "          "
                    }, new Point(2, 1), new string[]
                    {
                        "OOOO#     ",
                        " #        ",
                        "          "
                    }
                ), new TestData // (1, -2)
                (
                    new string[]
                    {
                        "  # #     ",
                        " #        ",
                        "          "
                    }, new Point(2, 1), new string[]
                    {
                        "  # #     ",
                        " #        ",
                        "   OOOO   "
                    }
                ), new TestData // (-2, 1)
                (
                    new string[]
                    {
                        "  # #     ",
                        " #        ",
                        "    #     "
                    }, new Point(2, 1), new string[]
                    {
                        "OOOO      ",
                        "  # #     ",
                        " #        ",
                        "    #     "
                    }
                )
            };

            foreach (TestData testCase in testCases)
            {
                TestKick(testCase, testPiece, 1);
            }
        }


        [Test, Apartment(ApartmentState.STA)]
        public void TestKicksIACW0()
        {
            Tetrimino testPiece = GetTetriminoRotated('I');
            // Spawnpoint (0, 0) off left of I

            TestData[] testCases = new TestData[]
            {
                new TestData // (0, 0)
                (
                    null, new Point(3, 2), new string[]
                    {
                        "    O     ",
                        "    O     ",
                        "    O     ",
                        "    O     "
                    }
                ), new TestData // (-1, 0)
                (
                    new string[]
                    {
                        "    #     "
                    }, new Point(3, 2), new string[]
                    {
                        "   O     ",
                        "   O     ",
                        "   O     ",
                        "   O#    "
                    }
                ), new TestData // (2, 0)
                (
                    new string[]
                    {
                        "   ##     "
                    }, new Point(3, 2), new string[]
                    {
                        "      O  ",
                        "      O  ",
                        "      O  ",
                        "   ## O  "
                    }
                ), new TestData // (-1, 2)
                (
                    null, new Point(3, 0), new string[]
                    {
                        "   O      ",
                        "   O      ",
                        "   O      ",
                        "   O      "
                    }
                ), new TestData // (2, -1)
                (
                    new string[]
                    {
                        "   ## #   ",
                        "          ",
                        "          ",
                        "          ",
                        "          "
                    }, new Point(3, 3), new string[]
                    {
                        "   ## #   ",
                        "      O   ",
                        "      O   ",
                        "      O   ",
                        "      O   "
                    }
                )
            };

            foreach (TestData testCase in testCases)
            {
                TestKick(testCase, testPiece, -1);
            }
        }

        [Test, Apartment(ApartmentState.STA)]
        public void TestKicksIACWL()
        {
            Tetrimino testPiece = GetTetriminoRotated('I', 3);
            // Spawnpoint (-1, 1) off bottom of I

            TestData[] testCases = new TestData[]
            {
                new TestData // (0, 0)
                (
                    new string[]
                    {
                        "   #      "
                    }, new Point(3, 1), new string[]
                    {
                        "   OOOO   ",
                        "   #      "
                    }
                ), new TestData // (-2, 0)
                (
                    new string[]
                    {
                        "     #    ",
                        "   #      "
                    }, new Point(3, 1), new string[]
                    {
                        " OOOO#    ",
                        "   #      "
                    }
                ), new TestData // (1, 0)
                (
                    new string[]
                    {
                        "   #      ",
                        "     #    "
                    }, new Point(3, 1), new string[]
                    {
                        "   #OOOO  ",
                        "     #    "
                    }
                ), new TestData // (-2, -1)
                (
                    new string[]
                    {
                        "   # #    ",
                        "          "
                    }, new Point(3, 1), new string[]
                    {
                        "   # #    ",
                        " OOOO     "
                    }
                ), new TestData // (1, 2)
                (
                    new string[]
                    {
                        "   # #    ",
                        "   # #    ",
                        "   #      "
                    }, new Point(3, 1), new string[]
                    {
                        "    OOOO  ",
                        "   # #    ",
                        "   # #    ",
                        "   #      "
                    }
                )
            };

            foreach (TestData testCase in testCases)
            {
                TestKick(testCase, testPiece, -1);
            }
        }

        [Test, Apartment(ApartmentState.STA)]
        public void TestKicksIACW2()
        {
            Tetrimino testPiece = GetTetriminoRotated('I', 2);
            // Spawnpoint (1, 0) off left of I

            TestData[] testCases = new TestData[]
            {
                new TestData // (0, 0)
                (
                    null, new Point(3, 1), new string[]
                    {
                        "    O     ",
                        "    O     ",
                        "    O     ",
                        "    O     "
                    }
                ), new TestData // (1, 0)
                (
                    new string[]
                    {
                        "    #     "
                    }, new Point(3, 1), new string[]
                    {
                        "     O    ",
                        "     O    ",
                        "     O    ",
                        "    #O    "
                    }
                ), new TestData // (-2, 0)
                (
                    new string[]
                    {
                        "    ##    "
                    }, new Point(3, 1), new string[]
                    {
                        "  O       ",
                        "  O       ",
                        "  O       ",
                        "  O ##    "
                    }
                ), new TestData // (1, -2)
                (
                    new string[]
                    {
                        "  # ##    ",
                        "          ",
                        "          ",
                        "          ",
                        "          "
                    }, new Point(3, 3), new string[]
                    {
                        "  # ##    ",
                        "     O    ",
                        "     O    ",
                        "     O    ",
                        "     O    "
                    }
                ), new TestData // (-2, 1)
                (
                    null, new Point(3, 0), new string[]
                    {
                        "  O       ",
                        "  O       ",
                        "  O       ",
                        "  O       "
                    }
                )
            };

            foreach (TestData testCase in testCases)
            {
                TestKick(testCase, testPiece, -1);
            }
        }

        [Test, Apartment(ApartmentState.STA)]
        public void TestKicksIACWR()
        {
            Tetrimino testPiece = GetTetriminoRotated('I', 1);
            // Spawnpoint (-1, 2) off bottom of I

            TestData[] testCases = new TestData[]
            {
                new TestData // (0, 0)
                (
                    new string[]
                    {
                        "   # #    ",
                        "          "
                    }, new Point(3, 2), new string[]
                    {
                        "  OOOO    ",
                        "   # #    ",
                        "          "
                    }
                ), new TestData // (2, 0)
                (
                    new string[]
                    {
                        "   #      ",
                        "   # #    ",
                        "          "
                    }, new Point(3, 2), new string[]
                    {
                        "   #OOOO  ",
                        "   # #    ",
                        "          "
                    }
                ), new TestData // (-1, 0)
                (
                    new string[]
                    {
                        "     #    ",
                        "   # #    ",
                        "          "
                    }, new Point(3, 2), new string[]
                    {
                        " OOOO#    ",
                        "   # #    ",
                        "          "
                    }
                ), new TestData // (2, 1)
                (
                    new string[]
                    {
                        "   # #    ",
                        "   # #    ",
                        "          "
                    }, new Point(3, 2), new string[]
                    {
                        "    OOOO  ",
                        "   # #    ",
                        "   # #    ",
                        "          "
                    }
                ), new TestData // (-1, -2)
                (
                    new string[]
                    {
                        "     #    ",
                        "   # #    ",
                        "   # #    ",
                        "          "
                    }, new Point(3, 2), new string[]
                    {
                        "     #    ",
                        "   # #    ",
                        "   # #    ",
                        " OOOO     "
                    }
                )
            };

            foreach (TestData testCase in testCases)
            {
                TestKick(testCase, testPiece, -1);
            }
        }
    }
}