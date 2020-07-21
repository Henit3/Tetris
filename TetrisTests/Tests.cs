using NUnit.Framework;
using System;
using System.Windows.Input;
using System.Windows.Media;
using Tetris;

namespace TetrisTests
{
    public class Tests
    {
        protected MainWindow mWindow;
        protected ViewModel vModel;

        protected void StartGame()
        {
            vModel.Session.Start(true, true, false);
        }
        protected void StepGame()
        {
            vModel.Session.GameLoop();
        }
        protected void SetPiece()
        {
            vModel.KeyDown(Key.W);
        }
        protected void Move(int x)
        {
            if (x < 0)
            {
                for (; x != 0; x++)
                {
                    vModel.KeyDown(Key.A);
                }
            }
            else if (x > 0)
            {
                for (; x != 0; x--)
                {
                    vModel.KeyDown(Key.D);
                }
            }
        }
        protected void Rotate(int x)
        {
            if (x < 0)
            {
                for (; x != 0; x++)
                {
                    vModel.KeyDown(Key.Left);
                }
            }
            else if (x > 0)
            {
                for (; x != 0; x--)
                {
                    vModel.KeyDown(Key.Right);
                }
            }
        }

        protected void SetUpArena(string[] arena)
        {
            vModel.Arena.Reset();
            if (arena == null)
            {
                return;
            }
            for (int row = 0; row < arena.Length; row++)
            {
                for (int col = 0; col < arena[row].Length; col++)
                {
                    vModel.Arena.Cells[arena.Length - row - 1][col].Fill = (arena[row][col]) switch
                    {
                        ' ' => Grid.DefaultFill,
                        '#' => Brushes.Black,
                        _ => throw new ArgumentException(),
                    };
                }
            }
        }

        [SetUp]
        public void Setup()
        {
            mWindow = new MainWindow();
            vModel = (ViewModel)mWindow.DataContext;
        }
    }
}