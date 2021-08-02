using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sudoku2.Models
{
    public class Solver
    {
        int[,] grid = new int[9, 9];
        int[] board { get; set; } = new int[81];
        int[] RandomIndex { get; set; }

        
        private void Init(ref int[,] grid)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    grid[i, j] = (i * 3 + i / 3 + j) % 9 + 1;

                }
            }
        }
        private void Draw(ref int[,] grid)
        {
            int index = 0;
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    board[index] = grid[x, y];
                    index++;
                }
            }
        }
        private void RandomStartBoard()
        {
            Random random = new Random();
            int[] TheIndexOfStartSquares = new int[20];
            for (int i = 0; i < 20; i++)
            {
                int value = random.Next(0, 80);
                if (!TheIndexOfStartSquares.Contains(value))
                    TheIndexOfStartSquares[i] = value;
                else
                    i--;
            }
            RandomIndex = TheIndexOfStartSquares;
        }
        private void ChangeTwoCell(ref int[,] grid, int findValue1, int findValue2)
        {
            int xParm1, yParm1, xParm2, yParm2;
            xParm1 = yParm1 = xParm2 = yParm2 = 0;
            for (int i = 0; i < 9; i += 3)
            {
                for (int k = 0; k < 9; k += 3)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        for (int z = 0; z < 3; z++)
                        {
                            if (grid[i + j, k + z] == findValue1)
                            {
                                xParm1 = i + j;
                                yParm1 = k + z;

                            }
                            if (grid[i + j, k + z] == findValue2)
                            {
                                xParm2 = i + j;
                                yParm2 = k + z;

                            }
                        }
                    }
                    grid[xParm1, yParm1] = findValue2;
                    grid[xParm2, yParm2] = findValue1;
                }
            }
        }

        private void Update(ref int[,] grid, int shuffleLevel)
        {
            for (int repeat = 0; repeat < shuffleLevel; repeat++)
            {
                Random rand = new Random(Guid.NewGuid().GetHashCode());
                Random rand2 = new Random(Guid.NewGuid().GetHashCode());
                ChangeTwoCell(ref grid, rand.Next(1, 9), rand2.Next(1, 9));
            }
        }

        public Solver()
        {
            Init(ref grid);
            Update(ref grid, 10);
            Draw(ref grid);
            RandomStartBoard();
        }
        public int[] GetBoard() => board;
        public int[] GetRandom() => RandomIndex;
    }
}