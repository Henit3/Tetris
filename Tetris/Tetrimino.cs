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
        public static Vector[][] DefaultOffsets;

        /// <summary>
        /// A unit vector in the down direction used to assist piece falling.
        /// </summary>
        private static readonly Vector gravity = new Vector(0, -1);

        /// <summary>
        /// Static constructor to initialise each of the 7 Tetrimino types.
        /// </summary>
        static Tetrimino()
        {
            DefaultOffsets = new Vector[][]
            {
                new Vector[4] { // R => 2 is R - 2, which is R
                    new Vector(0, 0),
                    new Vector(0, 0),
                    new Vector(0, 0), // (0, 0)
                    new Vector(0, 0)
                }, new Vector[4] {
                    new Vector(0, 0),
                    new Vector(1, 0),
                    new Vector(0, 0), // (1, 0)
                    new Vector(-1, 0)
                }, new Vector[4] {
                    new Vector(0, 0),
                    new Vector(1, -1),
                    new Vector(0, 0), // (1, 1)
                    new Vector(-1, -1)
                }, new Vector[4] {
                    new Vector(0, 0),
                    new Vector(0, 2),
                    new Vector(0, 0), // (0, -2)
                    new Vector(0, 2)
                }, new Vector[4] {
                    new Vector(0, 0),
                    new Vector(1, 2),
                    new Vector(0, 0), // (1, -2)
                    new Vector(-1, 2)
                }
            };
            new Tetrimino('O', Brushes.Yellow, new Vector[][]
            {
                new Vector[4] {
                    new Vector(0, 0),
                    new Vector(0, -1),
                    new Vector(-1, -1),
                    new Vector(-1, 0)
                }
            }, new Point[][]
            {
                new Point[4] { // Top Right
                    new Point(1, 0),
                    new Point(1, -1),
                    new Point(2, 0),
                    new Point(2, -1)
                }, new Point[4] { // Bottom Right
                    new Point(1, -1),
                    new Point(1, -2),
                    new Point(2, -1),
                    new Point(2, -2)
                }, new Point[4] { // Bottom Right
                    new Point(0, -1),
                    new Point(0, -2),
                    new Point(1, -1),
                    new Point(1, -2)
                }, new Point[4] { // Top Left
                    new Point(0, 0),
                    new Point(0, -1),
                    new Point(1, 0),
                    new Point(1, -1)
                }
            });
            new Tetrimino('I', Brushes.Cyan, new Vector[][]
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
            }, new Vector(-1, 2));
            new Tetrimino('L', Brushes.Orange, DefaultOffsets, new Point[][]
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
            new Tetrimino('J', Brushes.Blue, DefaultOffsets, new Point[][]
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
            new Tetrimino('T', Brushes.Magenta, DefaultOffsets, new Point[][]
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
            new Tetrimino('S', Brushes.Lime, DefaultOffsets, new Point[][]
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
            new Tetrimino('Z', Brushes.Red, DefaultOffsets, new Point[][]
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
        /// The state the Tetrimino spawns in.
        /// </summary>
        private readonly int startState;

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
        /// <param name="offsets">The offsets used when rotating in the SRS system.</param>
        /// <param name="colour">The brush colour to be used when rendering it.</param>
        /// <param name="states">The states the Tetrimino cylces through in rotation.</param>
        /// <param name="shiftSpawn">Correction vector for spawning if one exists.</param>
        /// <param name="startState">The state the Tetrimino should spawn in.</param>
        /// <remarks>
        /// Only called in the static constructor, with existing Tetriminos obtained using dictionary.
        /// </remarks>
        public Tetrimino(char letter, Brush colour, Vector[][] offsets, Point[][] states,
            Vector? shiftSpawn = null, int startState = 0)
        {
            Type = letter;
            Colour = colour;
            this.offsets = offsets;
            this.states = states;
            this.shiftSpawn = shiftSpawn;
            this.startState = startState;
            Position = null;
            Types.TryAdd(letter, this);
        }

        /// <summary>
        /// Handles logic relating to the spawning of the Tetrimio.
        /// </summary>
        /// <param name="arena">The Grid the Tetrimino will spawn in.</param>
        /// <param name="spawnpoint">The position to spawn the Tetrimino at.</param>
        /// <returns>
        /// Whether the spawn action can be completed successfully.
        /// </returns>
        public bool Spawn(Grid arena, Point spawnpoint)
        {
            Position = spawnpoint;
            if (shiftSpawn != null)
            {
                Position += (Vector)shiftSpawn;
            }
            currentStateNo = startState;
            Point[] points = ApplyPositionOffset(arena);
            if (points == null || IsBlocked(arena, points))
            {
                return false;
            }
            CurrentOccupied = points;
            return true;
        }

        /// <summary>
        /// Rotates the current active piece in a clockwise direction, handling rendering on the grid.
        /// </summary>
        /// <param name="arena">The Grid to rotate the Tetrimino in.</param>
        /// <param name="rotation">The number of clockwise right angled rotations to make.</param>
        /// <returns>
        /// Whether the rotation is successful or not.
        /// </returns>
        public bool Rotate(Grid arena, int rotation)
        {
            // TODO:
            // When using rotation to go up, increase a rotationDelayCounter
            // After a certain point, use it to stop that
            Vector positionOffset = new Vector(Position.Value.X, Position.Value.Y);

            // Not using destStateNo because ApplyOffset uses curentStateNo
            int lastStateNo = currentStateNo;
            currentStateNo += rotation;
            currentStateNo -= states.Length * (int)Math.Floor(currentStateNo / (double)states.Length);

            // For every offset, attempt to place. If it fails, go to next offset until all fail.
            foreach (Vector[] offset in offsets)
            {
                // Apply offset and position, fail if out of bounds of the arena
                // srcOffset - destOffset is the translation
                Vector kick = offset[lastStateNo] - offset[currentStateNo];
                Vector totalOffset = positionOffset + kick;
                Point[] points = ApplyOffset(arena, totalOffset);
                if (points == null)
                {
                    continue;
                }

                if (IsBlocked(arena, points))
                {
                    continue;
                }

                // A rotation has been found
                CurrentOccupied = points;
                Position += kick;
                return true;
            }

            // If failed to rotate, reset the state number
            currentStateNo = lastStateNo;
            return false;
        }

        /// <summary>
        /// Checks if the proposed new location is blocked in the Grid it is in.
        /// </summary>
        /// <param name="arena">The Grid the Tetrimino is in.</param>
        /// <param name="points">The proposed new location as a set of points.</param>
        /// <returns>
        /// Whether the new location is blocked by other pieces.
        /// </returns>
        private bool IsBlocked(Grid arena, Point[] points)
        {
            for (int i = 0; i < points.Length; i++)
            {
                if (arena.Cells[(int)points[i].Y][(int)points[i].X].Fill != Grid.DefaultFill)
                {
                    // If it has spawned already, check it isn't overlapping with itself
                    if (CurrentOccupied == null || !CurrentOccupied.Contains(points[i]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Gets the array of points describing the spaces the Tetrimino occupies.
        /// </summary>
        /// <param name="arena">The Grid to show the points occupied by the Tetrimino.</param>
        /// <returns>
        /// An array of points occupied by the Tetrimino.
        /// </returns>
        public Point[] ApplyPositionOffset(Grid arena)
        {
            return ApplyOffset(arena, new Vector(Position.Value.X, Position.Value.Y));
        }

        /// <summary>
        /// Attempts to move the piece horizontally.
        /// </summary>
        /// <param name="arena">The Grid to move the Tetrimino in.</param>
        /// <param name="x">The horizontal displacement.</param>
        /// <returns>
        /// The new location as a set of points or null if unsuccessful.
        /// </returns>
        public Point[] Move(Grid arena, int x)
        {
            Point[] points = ApplyOffset(arena, new Vector(Position.Value.X + x, Position.Value.Y));
            // If out of bounds or blocked by the movement of one cell horizontally
            if (points == null || IsBlocked(arena, points))
            {
                return null;
            }
            CurrentOccupied = points;
            Position += new Vector(x, 0);
            return points;
        }

        /// <summary>
        /// Applies gravity and makes the piece fall by a cell, checking to see if it is blocked.
        /// </summary>
        /// <param name="arena">The Grid to move the Tetrimino in.</param>
        /// <returns>
        /// The new location as a set of points or null if unsuccessful.
        /// </returns>
        public Point[] Fall(Grid arena)
        {
            Point[] points = ApplyOffset(arena, new Vector(Position.Value.X, Position.Value.Y - 1));
            // If out of bounds or blocked by the drop of one cell
            if (points == null || IsBlocked(arena, points))
            {
                return null;
            }
            CurrentOccupied = points;
            Position += gravity;
            return points;
        }

        /// <summary>
        /// Sets the cell to the bottom most place it can fall to.
        /// </summary>
        /// <param name="arena">The Grid to move the Tetrimino in.</param>
        /// <returns>
        /// The new location as a set of points.
        /// </returns>
        public Point[] HardDrop(Grid arena)
        {
            // From each point, scan down and find the minimum distance from them all
            int[] distances = new int[] { 0, 0, 0, 0 };
            for (int i = 0; i < CurrentOccupied.Length; i++)
            {
                // Scan down until the bottom, or another piece is detected
                while (CurrentOccupied[i].Y - distances[i] >= 0)
                {
                    // If not empty, check if it is part of the current piece
                    if (arena.Cells[(int)CurrentOccupied[i].Y - distances[i]][(int)CurrentOccupied[i].X]
                    .Fill != Grid.DefaultFill)
                    {
                        if (!CurrentOccupied.Contains(
                        new Point(CurrentOccupied[i].X, CurrentOccupied[i].Y - distances[i])))
                        {
                            break;
                        }
                    }
                    distances[i]++;
                }
            }
            int minDistance = distances.Min() - 1;
            CurrentOccupied = ApplyOffset(arena,
                new Vector(Position.Value.X, Position.Value.Y - minDistance));
            Position += new Vector(0, -minDistance);
            return CurrentOccupied;
        }

        /// <summary>
        /// Applies a vector offset to the current state of the Tetrimino.
        /// </summary>
        /// <param name="arena">The Grid the piece is being handled in.</param>
        /// <param name="offset">The vector offset to be applied.</param>
        /// <returns>
        /// The resulting points of the Tetrimino after applying the offset.
        /// </returns>
        private Point[] ApplyOffset(Grid arena, Vector offset)
        {
            Point[] points = new Point[4];
            for (int i = 0; i < CurrentState.Length; i++)
            {
                points[i] = CurrentState[i] + offset;
                if (points[i].X >= arena.Cols || points[i].Y >= arena.Rows + arena.ExtraRows
                    || points[i].X < 0 || points[i].Y < 0)
                {
                    return null;
                }
            }
            return points;
        }
    }
}
