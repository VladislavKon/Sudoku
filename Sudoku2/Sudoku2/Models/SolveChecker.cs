using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sudoku2.Models
{
    public class SolveChecker
    {
        private int[,] To2dArray(int[] array)
        {
            int[,] array2d = new int[9,9];
            int index = 0;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    array2d[i, j] = array[index++];
                }
            }
            return array2d;
        }
        public bool Check(int[] array)
        {
            int[,] array2d = To2dArray(array);
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (!isValid(array2d, i, j, array2d[i, j]))
                    {
                        return false;
                    }                     
                }
            }
            return true;
        }
        private bool isValid(int[,] board, int row, int col, int c)
        {
            for (int i = 0; i < 9; i++)
            {
                //check row  
                if (row!=i && board[i, col] == c)
                    return false;
                //check column  
                if (col!=i && board[row, i] == c)
                    return false;
                //check 3*3 block  
                if (row!= (3 * (row / 3) + i / 3) && col!= (3 * (col / 3) + i % 3) && board[3 * (row / 3) + i / 3, 3 * (col / 3) + i % 3] == c)
                    return false;
            }
            return true;
        }
    }
}