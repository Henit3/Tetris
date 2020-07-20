using System;
using System.Collections.Generic;
using System.Linq;
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
            Vector[][] defaultOffsets = new Vector[][]
            {
                new Vector[4] {
                    new Vector(0, 0),
                    new Vector(0, 0),
                    new Vector(0, 0),
                    new Vector(0, 0)
                }, new Vector[4] {
                    new Vector(0, 0),
                    new Vector(1, 0),
                    new Vector(0, 0),
                    new Vector(-1, 0)
                }, new Vector[4] {
                    new Vector(0, 0),
                    new Vector(1, 1),
                    new Vector(0, 0),
                    new Vector(-1, 1)
                }, new Vector[4] {
                    new Vector(0, 0),
                    new Vector(0, -2),
                    new Vector(0, 0),
                    new Vector(0, -2)
                }, new Vector[4] {
                    new Vector(0, 0),
                    new Vector(1, -2),
                    new Vector(0, 0),
                    new Vector(-1, -2)
                }
            };
            new Tetrimino('O', Brushes.Yellow, new Vector[][]
            {
                new Vector[4] {
                    new Vector(0, 0),
                    new Vector(0, 1),
                    new Vector(-1, 1),
                    new Vector(-1, 0)
                }
            }, new Point[][]
            {
                new Point[4] {
                    new Point(0, 0),
                    new Point(0, -1),
                    new Point(1, 0),
                    new Point(1, -1)
                }, new Point[4] {
                    new Point(0, -1),
                    new Point(0, -2),
                    new Point(1, -1),
                    new Point(1, -2)
                }, new Point[4] {
                    new Point(1, -1),
                    new Point(1, -2),
                    new Point(2, -1),
                    new Point(2, -2)
                }, new Point[4] {
                    new Point(1, 0),
                    new Point(1, -1),
                    new Point(2, 0),
                    new Point(2, -1)
                }
            }, new Vector(1, 0));
            new Tetrimino('I', Brushes.Cyan, new Vector[][]
            {
                new Vector[4] {
                    new Vector(0, 0),
                    new Vector(-1, 0),
                    new Vector(-1, -1),
                    new Vector(0, -1)
                }, new Vector[4] {
                    new Vector(-1, 0),
                    new Vector(0, 0),
                    new Vector(1, -1),
                    new Vector(0, -1)
                }, new Vector[4] {
                    new Vector(2, 0),
                    new Vector(0, 0),
                    new Vector(-2, -1),
                    new Vector(0, -1)
                }, new Vector[4] {
                    new Vector(-1, 0),
                    new Vector(0, -1),
                    new Vector(1, 0),
                    new Vector(0, 1)
                }, new Vector[4] {
                    new Vector(2, 0),
                    new Vector(0, 2),
                    new Vector(-2, 0),
                    new Vector(0, -2)
                }
            }, new Point[][]
            {
                new Point[4] {
                    new Point(0, -2),
                    new Point(1, -2),
                    new Point(2, -2),
                    new Point(3, -2)
                }, new Point[4] {
                    new Point(2, -1),
                    new Point(2, -2),
                    new Point(2, -3),
                    new Point(2, -4)
                }, new Point[4] {
                    new Point(1, -2),
                    new Point(2, -2),
                    new Point(3, -2),
                    new Point(4, -2)
                }, new Point[4] {
                    new Point(2, 0),
                    new Point(2, -1),
                    new Point(2, -2),
                    new Point(2, -3)
                }
            }, new Vector(0, 2));
            new Tetrimino('L', Brushes.Orange, defaultOffsets, new Point[][]
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
            new Tetrimino('J', Brushes.Blue, defaultOffsets, new Point[][]
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
            new Tetrimino('T', Brushes.Magenta, defaultOffsets, new Point[][]
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
            new Tetrimino('S', Brushes.Lime, defaultOffsets, new Point[][]
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
            new Tetrimino('Z', Brushes.Red, defaultOffsets, new Point[][]
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

        /// <summary>
        /// Vectors applied to give alternate rotations when blocked
        /// </summary>
        private readonly Vector[][] offsets;
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
        /// Property marking the currently occupied cells by the Tetrimino.
        /// </summary>
        public Point[] CurrentOccupied { get; private set; } = null;

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
        public Tetrimino(char letter, Brush colour, Vector[][] _offsets, Point[][] _states,
            Vector? _shiftSpawn = null)
        {
            Type = letter;
            Colour = colour;
            offsets = _offsets;
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
        public bool Spawn(Grid Arena, Point spawnpoint)
        {
            Position = spawnpoint;
            if (shiftSpawn != null)
            {
                Position += (Vector) shiftSpawn;
            }
            currentStateNo = 0;
            CurrentOccupied = GetPoints(Arena);
            return true;
        }

        /// <summary>
        /// Rotates the current active piece in a clockwise direction, handling rendering on the grid.
        /// </summary>
        /// <param name="rotation">The number of clockwise right angled rotations to make.</param>
        public bool Rotate(int rotation, Grid Arena, Point[] lastLocation)
        {
            Vector positionOffset = new Vector(Position.Value.X, Position.Value.Y);

            int lastStateNo = currentStateNo;
            currentStateNo += rotation;
            currentStateNo -= states.Length * (int)Math.Floor(currentStateNo / (double)states.Length);

            // For every offset, attempt to place. If it fails, go to next offset until all fail.
            foreach (Vector[] offset in offsets)
            {
                // Apply offset and position, fail if out of bounds of the arena
                // srcOffset - destOffset is the translation
                Vector totalOffset = positionOffset + offset[currentStateNo] - offset[lastStateNo];
                Point[] points = ApplyOffset(Arena, totalOffset);
                if (points == null)
                {
                    continue;
                }

                bool blocked = false;
                for (int i = 0; i < points.Length; i++)
                {
                    // Fail conditions due to the piece being blocked by existing garbage
                    if (Arena.Cells[(int)points[i].Y][(int)points[i].X].Fill != Grid.DEFAULT_FILL
                        && !lastLocation.Contains(points[i]))
                    {
                        blocked = true;
                        break;
                    }
                }
                if (blocked)
                {
                    continue;
                }

                // A rotation has been found
                CurrentOccupied = points;
                Position += offset[currentStateNo] - offset[lastStateNo];
                return true;
            }
            
            // If failed to rotate, reset the state number
            currentStateNo = lastStateNo;
            return false;
        }

        /// <summary>
        /// Gets the array of points describing the spaces the Tetrimino occupies.
        /// </summary>
        /// <param name="rows">The number of rows in the grid to be drawn on.</param>
        /// <param name="cols">The number of rows in the grid to be drawn on.</param>
        /// <returns>An array of points occupied by the Tetrimino.</returns>
        public Point[] GetPoints(Grid Arena)
        {
            return ApplyOffset(Arena, new Vector(Position.Value.X, Position.Value.Y));
        }

        /// <summary>
        /// Applies a vector offset to the current state of the Tetrimino.
        /// </summary>
        /// <param name="Arena">The Grid the piece is being handled in.</param>
        /// <param name="offset">The vector offset to be applied.</param>
        /// <returns>The resulting points of the Tetrimino after applying the offset.</returns>
        private Point[] ApplyOffset(Grid Arena, Vector offset)
        {
            Point[] points = new Point[4];
            for (int i = 0; i < CurrentState.Length; i++)
            {
                points[i] = CurrentState[i] + offset;
                if (points[i].X >= Arena.Cols || points[i].Y >= Arena.Rows
                    || points[i].X < 0 || points[i].Y < 0)
                {
                    return null;
                }
            }
            return points;
        }
    }
}
