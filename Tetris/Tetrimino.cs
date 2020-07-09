using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Tetris
{
    /// <summary>
    /// Represents the 4-cell adjacently joined pieces that the player controls.
    /// </summary>
    public class Tetrimino
    {
        /// <summary>
        /// Maps each of the Tetriminos with their corresponding letter shapes.
        /// </summary>
        public static Dictionary<char, Tetrimino> Types = new Dictionary<char, Tetrimino>();

        /// <summary>
        /// Static constructor to initialise each of the 7 Tetrimino types.
        /// </summary>
        static Tetrimino()
        {
            /*Vector[,] empty = new Vector[4, 4];*/
            bool[][,] states = new bool[4][,];
            for (int i = 0; i < states.Length; i++)
            {
                states[i] = new bool[4, 4];
            }
            new Tetrimino('O', Brushes.Yellow/*, empty*/, new bool[][,]
            {
                new bool[2, 2] {
                    { true, true },
                    { true, true }
                }
            }, new Vector(1, 0));
            new Tetrimino('I', Brushes.Cyan/*, empty*/, new bool[][,]
            {
                new bool[,] {
                    { false, false, false, false},
                    { true, true, true, true },
                    { false, false, false, false },
                    { false, false, false, false}
                }, new bool[,]  {
                    { false, false, true, false},
                    { false, false, true, false },
                    { false, false, true, false },
                    { false, false, true, false}
                }, new bool[,]  {
                    { false, false, false, false},
                    { false, false, false, false },
                    { true, true, true, true },
                    { false, false, false, false}
                }, new bool[,]  {
                    { false, true, false, false},
                    { false, true, false, false },
                    { false, true, false, false },
                    { false, true, false, false}
                }
            }, new Vector(0, 1));
            new Tetrimino('L', Brushes.Orange/*, empty*/, new bool[][,]
            {
                new bool[3, 3] {
                    { false, false, true },
                    { true, true, true },
                    { false, false, false }
                }, new bool[3, 3] {
                    { false, true, false },
                    { false, true, false },
                    { false, true, true }
                }, new bool[3, 3] {
                    { false, false, false },
                    { true, true, true },
                    { true, false, false }
                }, new bool[3, 3] {
                    { true, true, false },
                    { false, true, false },
                    { false, true, false }
                }
            });
            new Tetrimino('J', Brushes.Blue/*, empty*/, new bool[][,]
            {
                new bool[3, 3] {
                    { true, false, false },
                    { true, true, true },
                    { false, false, false }
                }, new bool[3, 3] {
                    { false, true, true },
                    { false, true, false },
                    { false, true, false }
                }, new bool[3, 3] {
                    { false, false, false },
                    { true, true, true },
                    { false, false, true }
                }, new bool[3, 3] {
                    { false, true, false },
                    { false, true, false },
                    { true, true, false }
                }
            });
            new Tetrimino('T', Brushes.Magenta/*, empty*/, new bool[][,]
            {
                new bool[3, 3] {
                    { false, true, false },
                    { true, true, true },
                    { false, false, false }
                }, new bool[3, 3] {
                    { false, true, false },
                    { false, true, true },
                    { false, true, false }
                }, new bool[3, 3] {
                    { false, false, false },
                    { true, true, true },
                    { false, true, false }
                }, new bool[3, 3] {
                    { false, true, false },
                    { true, true, false },
                    { false, true, false }
                }
            });
            new Tetrimino('S', Brushes.Lime/*, empty*/, new bool[][,]
            {
                new bool[3, 3] {
                    { false, true, true },
                    { true, true, false },
                    { false, false, false }
                }, new bool[3, 3] {
                    { false, true, false },
                    { false, true, true },
                    { false, false, true }
                }, new bool[3, 3] {
                    { false, false, false },
                    { false, true, true },
                    { true, true, false }
                }, new bool[3, 3] {
                    { true, false, false },
                    { true, true, false },
                    { false, true, false }
                }
            });
            new Tetrimino('Z', Brushes.Red/*, empty*/, new bool[][,]
            {
                new bool[3, 3] {
                    { true, true, false },
                    { false, true, true },
                    { false, false, false }
                }, new bool[3, 3] {
                    { false, false, true },
                    { false, true, true },
                    { false, true, false }
                }, new bool[3, 3] {
                    { false, false, false },
                    { true, true, false },
                    { false, true, true }
                }, new bool[3, 3] {
                    { false, true, false },
                    { true, true, false },
                    { true, false, false }
                }
            });
        }

        /// <summary>
        /// Property for the brush colour used by the Tetrimino.
        /// </summary>
        public Brush Colour { get; set; }
        /// <summary>
        /// Property for the position of a Tetrimino on the grid (top left cell).
        /// </summary>
        public Point? Position { get; set; }
        /// <summary>
        /// Property for the type of Tetrimino (characterised by the letter shape).
        /// </summary>
        public char Type { get; }

        /*private readonly Vector[,] offsets;*/
        /// <summary>
        /// A vector applied to correct the spawn position.
        /// </summary>
        private readonly Vector? shiftSpawn;
        /// <summary>
        /// Contains all the states a Tetrimino can have in each of its rotations.
        /// </summary>
        private readonly bool[][,] states;

        /// <summary>
        /// Used to get the current state from the array of states above.
        /// </summary>
        private int currentStateNo = 0;
        /// <summary>
        /// Property for the current state in use by the Tetrimino.
        /// </summary>
        public bool[,] CurrentState
        {
            get { return states[currentStateNo]; }
        }

        /// <summary>
        /// The constructor for the Tetrimino class.
        /// </summary>
        /// <param name="letter">The letter shape (type) of the Tetrimino.</param>
        /// <param name="colour">The brush colour to be used when rendering it.</param>
        /// <param name="_states">The states the Tetrimino cylces through in rotation.</param>
        /// <param name="_shiftSpawn">Correction vector for spawning if one exists.</param>
        /// <remarks>
        /// Only called in the static constructor, with existing Tetriminos obtained using dictionary.
        /// </remarks>
        public Tetrimino(char letter, Brush colour, /*Vector[,] _offsets,*/ bool[][,] _states,
            Vector? _shiftSpawn = null)
        {
            Type = letter;
            Colour = colour;
            /*offsets = _offsets;*/
            states = _states;
            shiftSpawn = _shiftSpawn;
            Position = null;
            Types.Add(letter, this);
        }

        /// <summary>
        /// Handles logic relating to the spawning of the Tetrimio.
        /// </summary>
        /// <param name="spawnpoint"></param>
        /// <returns>Whether the spawn action can be completed successfully.</returns>
        /// <remarks>
        /// Currently only returns true.
        /// </remarks>
        public bool Spawn(Point spawnpoint)
        {
            Position = spawnpoint;
            currentStateNo = 0;
            return true;
        }

        // TODO: Handle OOB with kicks
        /// <summary>
        /// Rotates the current active piece in a clockwise direction, handling rendering on the grid.
        /// </summary>
        /// <param name="rotation">The number of clockwise right angled rotations to make.</param>
        public void Rotate(int rotation)
        {
            currentStateNo += rotation;
            currentStateNo -= states.Length * (int)Math.Floor(currentStateNo / (double)states.Length);
        }

        /// <summary>
        /// Gets an array of points describing the spaces the Tetrimino occupies.
        /// </summary>
        /// <param name="rows">The number of rows in the grid to be drawn on.</param>
        /// <param name="cols">The number of rows in the grid to be drawn on.</param>
        /// <returns>An array of points occupied by the Tetrimino.</returns>
        public Point[] GetPoints(int rows, int cols)
        {
            Point[] points = new Point[4];
            int counter = 0;
            Vector topLeftOffset = new Vector(Position.Value.X, Position.Value.Y);
            if (shiftSpawn != null)
            {
                topLeftOffset += (Vector)shiftSpawn;
            }

            for (int i = 0; i < CurrentState.GetLength(0); i++)
            {
                for (int j = 0; j < CurrentState.GetLength(1); j++)
                {
                    if (CurrentState[i, j])
                    {
                        Point p = new Point(j, -i);
                        points[counter] = p + topLeftOffset;
                        if (points[counter].X >= cols || points[counter].Y >= rows
                            || points[counter].X < 0 || points[counter].Y < 0)
                        {
                            return null;
                        }
                        counter += 1;
                    }
                }
            }
            return points;
        }
    }
}
