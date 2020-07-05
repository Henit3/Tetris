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
        /// Allows conversion from string to media brush types.
        /// </summary>
        private static readonly BrushConverter ColourConvertor = new BrushConverter();

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
                        Width = SIZE,
                        Height = SIZE,
                        Fill = Brushes.Transparent,
                        Stroke = Brushes.Black,
                        StrokeThickness = 0,
                    };

                    Parent.Children.Add(Cells[row][col]);
                    Canvas.SetTop(Cells[row][col], SIZE * (rows - row - 1));
                    Canvas.SetLeft(Cells[row][col], SIZE * col);
                }
            }
        }

        /// <summary>
        /// Assists the process of filling a cell by converting a string colour to the brush type.
        /// </summary>
        /// <param name="row">The row the cell is on.</param>
        /// <param name="col">The column the cell is on.</param>
        /// <param name="colour">The colour to fill the cell with.</param>
        public void FillCell(int row, int col, string colour)
        {
            Cells[row][col].Fill = (Brush) ColourConvertor.ConvertFromString(colour);
        }
    }
}
