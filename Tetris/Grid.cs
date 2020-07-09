using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Tetris
{
    /// <summary>
    /// The class representing the playable grid area.
    /// </summary>
    public class Grid
    {
        /// <summary>
        /// The size of individual cells.
        /// </summary>
        private const int SIZE = 20;

        /// <summary>
        /// The brush colour to be used when a cell is empty.
        /// </summary>
        public static readonly Brush DEFAULT_FILL = Brushes.LightSlateGray;

        /// <summary>
        /// Number of columns in the grid.
        /// </summary>
        public int Cols { get; }

        /// <summary>
        /// Number of rows in the grid.
        /// </summary>
        public int Rows { get; }

        /// <summary>
        /// The contents of the grid, as drawn rectangles.
        /// </summary>
        public Rectangle[][] Cells { get; }

        /// <summary>
        /// A basic constructor for the grid.
        /// </summary>
        /// <param name="rows">The number of rows.</param>
        /// <param name="cols">The number of columns.</param>
        /// <param name="Parent">The parent element for the grid.</param>
        public Grid(int rows, int cols, Canvas Parent)
        {
            Rows = rows;
            Cols = cols;

            Cells = new Rectangle[rows][];
            for (int row = 0; row < Rows; row++)
            {
                Cells[row] = new Rectangle[cols];
                for (int col = 0; col < Cols; col++)
                {
                    Cells[row][col] = new Rectangle()
                    {
                        Width = SIZE - 1,
                        Height = SIZE - 1,
                        Fill = DEFAULT_FILL,
                        Stroke = Brushes.Black,
                        StrokeThickness = 0,
                    };

                    Parent.Children.Add(Cells[row][col]);
                    Canvas.SetTop(Cells[row][col], SIZE * (rows - row - 1));
                    Canvas.SetLeft(Cells[row][col], SIZE * col);
                }
            }
        }
    }
}
