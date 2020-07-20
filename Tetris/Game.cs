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

        /*public int Points { get; set; }
        public int Lines { get; set; }
        public int Pieces { get; set; }*/

        /// <summary>
        /// The point in the grid where pieces should spawn.
        /// </summary>
        private readonly Point spawnpoint = new Point(3, 19);
        /// <summary>
        /// The grid to be used while placing and moving pieces.
        /// </summary>
        private readonly Grid Arena;
        /// <summary>
        /// The queue of pieces to be placed next.
        /// </summary>
        private readonly Queue<Tetrimino> PieceQueue = new Queue<Tetrimino>();
        /// <summary>
        /// A random generator to assist with generating the pieces to be spawned.
        /// </summary>
        private readonly Random randomGenerator = new Random();

        /// <summary>
        /// The points where the last piece was rendered, so that it can be unrendered.
        /// </summary>
        private Point[] lastLocation;
        /*private bool hasLost;*/

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
            Arena = arena;
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
            Arena = arena;
            if (spawn == null)
            {
                spawn = new Point((arena.Cols / 2) - 2, arena.Rows - 1);
            }
            spawnpoint = (Point)spawn;
            if (preQueue == null)
            {
                preQueue = new Queue<Tetrimino>();
            }
            PieceQueue = (Queue<Tetrimino>)preQueue;
        }

        /*public void Start()
        {
            Points = 0;
            Lines = 0;
            Pieces = 0;
            hasLost = false;
            GameLoop();
        }*/

        /// <summary>
        /// Contains the game loop logic that will be repeatedly executed.
        /// </summary>
        /// <remarks>
        /// For now, this is a singular loop to assist development.
        /// </remarks>
        public void GameLoop()
        {
            /*while (!hasLost)*/
            {
                if (PieceQueue.Count == 0)
                {
                    GenerateBag();
                }

                CurrentPiece = SpawnPiece();

                // TODO: Piece Logic for falling, control and checking if it is to be set
            }
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
            if (CurrentPiece.Rotate(rotation, Arena, lastLocation))
            {
                RenderPiece(CurrentPiece);
            }
        }

        /// <summary>
        /// Spawns a new piece by dequeing from the piece queue.
        /// </summary>
        /// <returns>The next Tetrimino piece that was spawned.</returns>
        private Tetrimino SpawnPiece()
        {
            Tetrimino piece = PieceQueue.Dequeue();
            piece.Spawn(Arena, spawnpoint);
            RenderPiece(piece);
            return piece;
        }

        /// <summary>
        /// Renders the piece onto the associated grid.
        /// </summary>
        /// <param name="piece">The Tetrimino to be rendered.</param>
        /// <returns>Whether the piece could successfully be rendered or not.</returns>
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
                Arena.Cells[(int)point.Y][(int)point.X].Fill = piece.Colour;
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
                Arena.Cells[(int)point.Y][(int)point.X].Fill = Grid.DEFAULT_FILL;
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
                PieceQueue.Enqueue(bag[randomNumber]);
                bag.RemoveAt(randomNumber);
            }
        }
    }
}
