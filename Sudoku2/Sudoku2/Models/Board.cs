using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sudoku2.Models
{
    public class Board
    {
        public List<Square> Squares { get; }
        public List<Square> StartSquares { get; }
        public SolverBoard Solver { get; }

        public int AllRaws { get; } = 9;
        public int AllColumns { get; } = 9;
        public int AllSquares { get => AllRaws * AllColumns; }
        public List<int> ArraySquares { get; } = new List<int>();


        private void InitializeSquares()
        {
            for (int x = 0; x < AllRaws; x++)
            {
                for (int y = 0; y < AllColumns; y++)
                {
                    Squares.Add(new Square(
                        index: x * AllRaws + y,
                        groupNo: (x / 3) + 3 * (y / 3) + 1,
                        value: -1,
                        position: new Square.RCPosition(row: x + 1, column: y + 1)
                    ));
                    StartSquares.Add(new Square(
                        index: x * AllRaws + y,
                        groupNo: (x / 3) + 3 * (y / 3) + 1,
                        value: -1,
                        position: new Square.RCPosition(row: x + 1, column: y + 1)
                    ));
                }
            }            
        }
        public void SetStartSquares()
        {            
            int[] Random20 = Solver.RandomStartBoard(20);
            for (int i = 0; i < 20; i++)
            {
                StartSquares[Random20[i]] = Squares[Random20[i]];
            }            
        }

        public void SetArraySquares()
        {            
            for (int i = 0; i < 81; i++)
            {
                ArraySquares.Add(Squares[i].Value);
            }
        }
        public Board()
        {
            Squares = new List<Square>(AllSquares);
            StartSquares = new List<Square>(AllSquares);
            Solver = new SolverBoard(this);
            InitializeSquares();            
            Solver.SolveThePuzzle();
            SetStartSquares();
            SetArraySquares();
        }


        public Square GetSquare(int SquareIndex) => Squares[SquareIndex];

        public Square GetSquare(Square.RCPosition squarePosition)
        {
            int cellIndex = (squarePosition.Row - 1) * AllSquares + squarePosition.Column - 1;

            return GetSquare(cellIndex);
        }

        public void SetSquareValue(int value, int squareIndex) => Squares[squareIndex].Value = value;

        public void SetSquareValue(int value, Square.RCPosition squarePosition)
        {

            int squareIndex = (squarePosition.Row - 1) * AllRaws + squarePosition.Column - 1;

            SetSquareValue(value, squareIndex);
        }

        public bool IsBoardFilled() => Squares.FirstOrDefault(square => square.Value == -1) == null;

        public bool IsTableEmpty() => Squares.FirstOrDefault(square => square.Value != -1) == null;

        public void Clear() => Squares.ForEach(square => SetSquareValue(-1, square.Index));



    }
}