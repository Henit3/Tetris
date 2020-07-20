using System;
using System.Collections.Generic;
using System.Windows;

namespace Tetris
{
    /// <summary>
    /// Contains the overall game logic, mediating the pieces, player and the playable grid.
    /// </summary>
    public class Game
    {
        /*public int Points { get; set; }*/
        /// <summary>
        /// The number of lines cleared in this game session.
        /// </summary>
        public int Lines { get; set; }
        /// <summary>
        /// The number of pieces dropped in this game session.
        /// </summary>
        public int Pieces { get; set; }

        /// <summary>
        /// The point in the grid where pieces should spawn.
        /// </summary>
        private readonly Point spawnpoint = new Point(3, 19);
        /// <summary>
        /// The grid to be used while placing and moving pieces.
        /// </summary>
        private readonly Grid arena;
        /// <summary>
        /// The queue of pieces to be placed next.
        /// </summary>
        private readonly Queue<Tetrimino> pieceQueue = new Queue<Tetrimino>();
        /// <summary>
        /// A random generator to assist with generating the pieces to be spawned.
        /// </summary>
        private readonly Random randomGenerator = new Random();

        /// <summary>
        /// The points where the last piece was rendered, so that it can be unrendered.
        /// </summary>
        private Point[] lastLocation;
        /// <summary>
        /// Whether the game is in a playable state.
        /// </summary>
        private bool isPlayable = true;
        /// <summary>
        /// The number of times an unsuccessful fall has been attempted, used to lock a piece.
        /// </summary>
        private int groundCounter = 0;

        /// <summary>
        /// A property for the current active piece that is being controlled.
        /// </summary>
        public Tetrimino CurrentPiece { get; set; }

        /// <summary>
        /// Basic constructor for the game object.
        /// </summary>
        /// <param name="arena">The grid to be used for the game session.</param>
        public Game(Grid arena)
        {
            this.arena = arena;
        }

#nullable enable
        /// <summary>
        /// Alternate constructor for the game object taking in extra preset parameters.
        /// </summary>
        /// <param name="arena">The grid to be used for the game session.</param>
        /// <param name="spawn">The spawnpoint to be used for Tetriminos.</param>
        /// <param name="preQueue">A queue of Tetriminos to be used initially.</param>
        /// <remarks>
        /// This constructor will mainly see use in testing for dependency injection.
        /// </remarks>
        public Game(Grid arena, Point? spawn = null, Queue<Tetrimino>? preQueue = null)
        {
#nullable disable
            this.arena = arena;
            if (spawn == null)
            {
                spawn = new Point((arena.Cols / 2) - 2, arena.Rows - 1);
            }
            spawnpoint = (Point)spawn;
            if (preQueue == null)
            {
                preQueue = new Queue<Tetrimino>();
            }
            pieceQueue = (Queue<Tetrimino>)preQueue;
        }

        /// <summary>
        /// Starts the game session, resetting all associated variables.
        /// </summary>
        public void Start()
        {
            /*Points = 0;*/
            Lines = 0;
            Pieces = 0;
            arena.Reset();
            CurrentPiece = null;
            lastLocation = null;
            pieceQueue.Clear();
            isPlayable = true;
        }

        /// <summary>
        /// Contains the game loop logic that will be repeatedly executed.
        /// </summary>
        /// <remarks>
        /// For now, this is a singular loop to assist development.
        /// </remarks>
        public void GameLoop()
        {
            if (!isPlayable)
            {
                return;
            }
            /*while (!hasLost)*/
            {
                if (CurrentPiece != null && !DropAndLock())
                {
                    return;
                }

                if (pieceQueue.Count == 0)
                {
                    GenerateBag();
                }

                CurrentPiece = SpawnPiece();
            }
        }

        /// <summary>
        /// Applies gravity to a piece and determines whether the piece should be locked.
        /// </summary>
        /// <returns>
        /// Whether a new piece is required to be spawned.
        /// </returns>
        public bool DropAndLock()
        {
            // True if action required - there is no piece or the piece has been set
            if (CurrentPiece == null)
            {
                return true;
            }
            if (CurrentPiece.Fall(arena) == null)
            {
                groundCounter++;
                if (groundCounter > 3)
                {
                    LockPiece();
                    return true;
                }
            }
            RenderPiece(CurrentPiece);
            return false;
        }

        /// <summary>
        /// Drops the Tetrimino to the last available position in the downward direction.
        /// </summary>
        public void HardDrop()
        {
            if (CurrentPiece == null)
            {
                return;
            }
            CurrentPiece.HardDrop(arena);
            RenderPiece(CurrentPiece);
            LockPiece();
            // GameLoop();
        }

        /// <summary>
        /// Locks the piece, resetting all piece related fields and cleans the grid of lines.
        /// </summary>
        private void LockPiece()
        {
            groundCounter = 0;
            lastLocation = null;
            CurrentPiece = null;
            Lines += arena.Clean();
        }

        /// <summary>
        /// Wrapper for MoveHorizontal in the left direction.
        /// </summary>
        /// <see cref="MoveHorizontal(int)"/>
        public void MoveLeft()
        {
            MoveHorizontal(-1);
        }

        /// <summary>
        /// Wrapper for MoveHorizontal in the right direction.
        /// </summary>
        /// <see cref="MoveHorizontal(int)"/>
        public void MoveRight()
        {
            MoveHorizontal(1);
        }

        /// <summary>
        /// Moves the current piece horizontally.
        /// </summary>
        /// <param name="rotation">The number of cells to move right.</param>
        private void MoveHorizontal(int x)
        {
            if (CurrentPiece == null)
            {
                return;
            }
            CurrentPiece.Move(arena, x);
            RenderPiece(CurrentPiece);
        }

        /// <summary>
        /// Wrapper for RotatePiece in the clockwise direction.
        /// </summary>
        /// <see cref="RotatePiece(int)"/>
        public void RotatePieceCW()
        {
            RotatePiece(1);
        }

        /// <summary>
        /// Wrapper for RotatePiece in the counterclockwise direction.
        /// </summary>
        /// <see cref="RotatePiece(int)"/>
        public void RotatePieceCCW()
        {
            RotatePiece(-1);
        }

        /// <summary>
        /// Rotates the current active piece in a clockwise direction, handling rendering on the grid.
        /// </summary>
        /// <param name="rotation">The number of clockwise right angled rotations to make.</param>
        private void RotatePiece(int rotation)
        {
            if (CurrentPiece == null)
            {
                return;
            }
            if (CurrentPiece.Rotate(arena, rotation))
            {
                RenderPiece(CurrentPiece);
            }
        }

        /// <summary>
        /// Spawns a new piece by dequeing from the piece queue.
        /// </summary>
        /// <returns>
        /// The next Tetrimino piece that was spawned, and null if it was unsuccesful.
        /// </returns>
        private Tetrimino SpawnPiece()
        {
            Tetrimino piece = pieceQueue.Dequeue();
            if (!piece.Spawn(arena, spawnpoint))
            {
                isPlayable = false;
                return null;
            }
            Pieces++;
            RenderPiece(piece);
            return piece;
        }

        /// <summary>
        /// Renders the piece onto the associated grid.
        /// </summary>
        /// <param name="piece">The Tetrimino to be rendered.</param>
        /// <returns>
        /// Whether the piece could successfully be rendered or not.
        /// </returns>
        private bool RenderPiece(Tetrimino piece)
        {
            if (piece == null)
            {
                return false;
            }
            Point[] points = piece.CurrentOccupied;
            if (points == null)
            {
                return false;
            }
            UnrenderLast();
            foreach (Point point in points)
            {
                arena.Cells[(int)point.Y][(int)point.X].Fill = piece.Colour;
            }
            lastLocation = points;
            return true;
        }

        /// <summary>
        /// Unrenders the last stable position of the piece from the associated grid.
        /// </summary>
        private void UnrenderLast()
        {
            if (lastLocation == null)
            {
                return;
            }
            foreach (Point point in lastLocation)
            {
                arena.Cells[(int)point.Y][(int)point.X].Fill = Grid.DefaultFill;
            }
        }

        /// <summary>
        /// Generates a randomized bag of all the pieces to be added to the queue.
        /// </summary>
        private void GenerateBag()
        {
            List<Tetrimino> bag = new List<Tetrimino>(Tetrimino.Types.Values);
            int randomNumber;
            while (bag.Count != 0)
            {
                randomNumber = randomGenerator.Next(0, bag.Count);
                pieceQueue.Enqueue(bag[randomNumber]);
                bag.RemoveAt(randomNumber);
            }
        }
    }
}
