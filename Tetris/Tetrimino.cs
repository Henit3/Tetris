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
            new Tetrimino('O', Brushes.Yellow/*, empty*/, new Point[][]
            {
                new Point[4] {
                    new Point(0, 0),
                    new Point(0, -1),
                    new Point(1, 0),
                    new Point(1, -1)
                }
            }, new Vector(1, 0));
            new Tetrimino('I', Brushes.Cyan/*, empty*/, new Point[][]
            {
                new Point[4] {
                    new Point(0, -1),
                    new Point(1, -1),
                    new Point(2, -1),
                    new Point(3, -1)
                }, new Point[4] {
                    new Point(2, 0),
                    new Point(2, -1),
                    new Point(2, -2),
                    new Point(2, -3)
                }, new Point[4] {
                    new Point(0, -2),
                    new Point(1, -2),
                    new Point(2, -2),
                    new Point(3, -2)
                }, new Point[4] {
                    new Point(1, 0),
                    new Point(1, -1),
                    new Point(1, -2),
                    new Point(1, -3)
                }
            }, new Vector(0, 1));
            new Tetrimino('L', Brushes.Orange/*, empty*/, new Point[][]
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
            });
            new Tetrimino('J', Brushes.Blue/*, empty*/, new Point[][]
            {
                new Point[4] { // J on its right
                    new Point(0, 0),
                    new Point(0, -1),
                    new Point(1, -1),
                    new Point(2, -1)
                }, new Point[4] { // J upside down
                    new Point(1, 0),
                    new Point(1, -1),
                    new Point(1, -2),
                    new Point(2, 0)
                }, new Point[4] { // J on its left
                    new Point(0, -1),
                    new Point(1, -1),
                    new Point(2, -1),
                    new Point(2, -2)
                }, new Point[4] { // J upright
                    new Point(0, -2),
                    new Point(1, 0),
                    new Point(1, -1),
                    new Point(1, -2)
                }
            });
            new Tetrimino('T', Brushes.Magenta/*, empty*/, new Point[][]
            {
                new Point[4] { // T pointing up
                    new Point(0, -1),
                    new Point(1, 0),
                    new Point(1, -1),
                    new Point(2, -1)
                }, new Point[4] { // T pointing right
                    new Point(1, 0),
                    new Point(1, -1),
                    new Point(1, -2),
                    new Point(2, -1)
                }, new Point[4] { // T pointing down
                    new Point(0, -1),
                    new Point(1, -1),
                    new Point(1, -2),
                    new Point(2, -1)
                }, new Point[4] { // T pointing left
                    new Point(0, -1),
                    new Point(1, 0),
                    new Point(1, -1),
                    new Point(1, -2)
                }
            });
            new Tetrimino('S', Brushes.Lime/*, empty*/, new Point[][]
            {
                new Point[4] { // S on top
                    new Point(0, -1),
                    new Point(1, 0),
                    new Point(1, -1),
                    new Point(2, 0)
                }, new Point[4] { // S on right
                    new Point(1, 0),
                    new Point(1, -1),
                    new Point(2, -1),
                    new Point(2, -2)
                }, new Point[4] { // S on bottom
                    new Point(0, -2),
                    new Point(1, -1),
                    new Point(1, -2),
                    new Point(2, -1)
                }, new Point[4] { // S on right
                    new Point(0, 0),
                    new Point(0, -1),
                    new Point(1, -1),
                    new Point(1, -2)
                }
            });
            new Tetrimino('Z', Brushes.Red/*, empty*/, new Point[][]
            {
                new Point[4] { // Z on top
                    new Point(0, 0),
                    new Point(1, 0),
                    new Point(1, -1),
                    new Point(2, -1)
                }, new Point[4] { // Z on right
                    new Point(1, -1),
                    new Point(1, -2),
                    new Point(2, 0),
                    new Point(2, -1)
                }, new Point[4] { // Z on bottom
                    new Point(0, -1),
                    new Point(1, -1),
                    new Point(1, -2),
                    new Point(2, -2)
                }, new Point[4] { // Z on left
                    new Point(0, -1),
                    new Point(0, -2),
                    new Point(1, 0),
                    new Point(1, -1)
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
        private readonly Point[][] states;

        /// <summary>
        /// Used to get the current state from the array of states above.
        /// </summary>
        private int currentStateNo = 0;
        /// <summary>
        /// Property for the current state in use by the Tetrimino.
        /// </summary>
        public Point[] CurrentState
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
        public Tetrimino(char letter, Brush colour, /*Vector[,] _offsets,*/ Point[][] _states,
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
            if (shiftSpawn != null) // BAD, only need on spawn
            {
                Position += (Vector) shiftSpawn;
            }
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
        /// Gets the array of points describing the spaces the Tetrimino occupies.
        /// </summary>
        /// <param name="rows">The number of rows in the grid to be drawn on.</param>
        /// <param name="cols">The number of rows in the grid to be drawn on.</param>
        /// <returns>An array of points occupied by the Tetrimino.</returns>
        public Point[] GetPoints(int rows, int cols)
        {
            Point[] points = new Point[4];
            Vector positionOffset = new Vector(Position.Value.X, Position.Value.Y);

            for (int i = 0; i < CurrentState.Length; i++)
            {
                points[i] = CurrentState[i] + positionOffset;
                if (points[i].X >= cols || points[i].Y >= rows
                    || points[i].X < 0 || points[i].Y < 0)
                {
                    return null;
                }
            }
            return points;
        }
    }
}
