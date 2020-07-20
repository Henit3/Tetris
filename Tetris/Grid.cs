using System.Linq;
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
        private const int Size = 20;

        /// <summary>
        /// The brush colour to be used when a cell is empty.
        /// </summary>
        public static readonly Brush DefaultFill = Brushes.LightSlateGray;

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
                        Width = Size - 1,
                        Height = Size - 1,
                        Fill = DefaultFill,
                        Stroke = Brushes.Black,
                        StrokeThickness = 0,
                    };

                    Parent.Children.Add(Cells[row][col]);
                    Canvas.SetTop(Cells[row][col], Size * (rows - row - 1));
                    Canvas.SetLeft(Cells[row][col], Size * col);
                }
            }
        }

        /// <summary>
        /// Resets the board back to its original state with the default fill.
        /// </summary>
        public void Reset()
        {
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Cols; col++)
                {
                    Cells[row][col].Fill = DefaultFill;
                }
            }
        }

        public int Clean()
        {
            int count = 0;
            for (int row = 0; row < Rows; row++)
            {
                bool filled = true;
                for (int col = 0; col < Cols; col++)
                {
                    if (Cells[row][col].Fill == DefaultFill)
                    {
                        filled = false;
                        break;
                    }
                }
                if (filled)
                {
                    // Clean row, shift rows above down
                    // Recursive - if not top, copy top, else clear
                    ShiftLine(row);
                    row--;
                    count++;
                }
            }
            return count;
        }

        private Brush[] ShiftLine(int row)
        {
            Brush[] output = new Brush[Cols];
            if (row == Rows - 1)
            {
                // Clear this one
                for (int col = 0; col < Cols; col++)
                {
                    output[col] = Cells[row][col].Fill;
                    Cells[row][col].Fill = DefaultFill;
                }
            }
            else
            {
                Brush[] input = ShiftLine(row + 1);
                for (int col = 0; col < Cols; col++)
                {
                    output[col] = Cells[row][col].Fill;
                    Cells[row][col].Fill = input[col];
                }
            }
            return output;
        }
    }
}
