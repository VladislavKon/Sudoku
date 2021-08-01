using System;
using System.Collections.Generic;
using System.Linq;

namespace Sudoku2.Models
{
    public class SolverBoard
    {
        
        readonly Board SudokuBoard;
               
        private readonly int[] Numbers = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        
        private List<int> TheIndexesOfFilledSquares = new List<int>();

        private List<List<int>> BlackListsOfSquares;
       
        private readonly Random Random = new Random();
         
        
        private void InitializeBlackList()
        {
            BlackListsOfSquares = new List<List<int>>(SudokuBoard.AllSquares);
            for (int index = 0; index < BlackListsOfSquares.Capacity; index++)
            {
                BlackListsOfSquares.Add(new List<int>());
            }
        }
        public int[] RandomStartBoard(int startSquaresValue)
        {
            int[] TheIndexOfStartSquares = new int[startSquaresValue];
            for (int i = 0; i < startSquaresValue; i++)
            {
                int value = Random.Next(0, 80);
                if (!TheIndexOfStartSquares.Contains(value))
                    TheIndexOfStartSquares[i]=value;
                else
                    i--;
            }
            return TheIndexOfStartSquares;
        }
        
        public SolverBoard(Board sudoku)
        {
            SudokuBoard = sudoku ?? new Board();

            InitializeBlackList();
        }

        
        public bool SolveThePuzzle(bool UseRandomGenerator = true)
        {            
            if (!CheckTableStateIsValid()) return false;
            
            InitIndexListOfTheAlreadyFilledSquares();

           
            ClearBlackList();

            int currentSquareIndex = 0;
            
            while (currentSquareIndex < SudokuBoard.AllSquares)
            {               
                if (TheIndexesOfFilledSquares.Contains(currentSquareIndex))
                {
                    ++currentSquareIndex;
                    continue;
                }
                               
                ClearBlackList(startCleaningFromThisIndex: currentSquareIndex + 1);

                Square currentSquar = SudokuBoard.GetSquare(SquareIndex: currentSquareIndex);

                int theFoundValidNumber = GetValidNumberForTheSquare(currentSquareIndex, UseRandomGenerator);
                               
                if (theFoundValidNumber == 0)
                {                   
                    currentSquareIndex = BacktrackTo(currentSquareIndex);
                }
                else
                {                    
                    SudokuBoard.SetSquareValue(theFoundValidNumber, currentSquar.Index);
                    ++currentSquareIndex;
                }
            }

            return true;
        }
               
        public bool CheckTableStateIsValid(bool ignoreEmptySquares = false) =>
            SudokuBoard.Squares
            .Where(square => !ignoreEmptySquares || square.Value != -1)
            .FirstOrDefault(square => square.Value != -1 && !IsValidValueForTheSquare(square.Value, square)) == null;
                
        public bool IsValidValueForTheSquare(int val, Square square)
        {            
            if (SudokuBoard.Squares.Where(c => c.Index != square.Index && c.GroupNo == square.GroupNo).FirstOrDefault(c2 => c2.Value == val) != null)
                return false;
                        
            if (SudokuBoard.Squares.Where(c => c.Index != square.Index && c.Position.Row == square.Position.Row).FirstOrDefault(c2 => c2.Value == val) != null)
                return false;
                        
            if (SudokuBoard.Squares.Where(c => c.Index != square.Index && c.Position.Column == square.Position.Column).FirstOrDefault(c2 => c2.Value == val) != null)
                return false;

            return true;
        }
                
        public void InitIndexListOfTheAlreadyFilledSquares()
        {
            TheIndexesOfFilledSquares.Clear();
            TheIndexesOfFilledSquares.AddRange(SudokuBoard.Squares
                .FindAll(square => square.Value != -1)
                .Select(square => square.Index));
        }
               
        private int BacktrackTo(int index)
        {            
            while (TheIndexesOfFilledSquares.Contains(--index)) ;
                        
            Square backTrackedSquare = SudokuBoard.GetSquare(index);
                        
            AddToBlacklist(backTrackedSquare.Value, squareIndex: index);
                        
            backTrackedSquare.Value = -1;
                        
            ClearBlackList(startCleaningFromThisIndex: index + 1);

            return index;
        }
               
        private int GetValidNumberForTheSquare(int squareIndex, bool useRandomFactor)
        {
            int theFoundValidNumber = 0;
                        
            var validNumbers = Numbers.Where(x => !BlackListsOfSquares[squareIndex].Contains(x)).ToArray();

            if (validNumbers.Length > 0)
            {                
                int choosenIndex = useRandomFactor ? Random.Next(validNumbers.Length) : 0;
                theFoundValidNumber = validNumbers[choosenIndex];
            }
                        
            do
            {
                Square currentSquare = SudokuBoard.GetSquare(squareIndex);
                                
                if (theFoundValidNumber != 0 && !SudokuBoard.Solver.IsValidValueForTheSquare(theFoundValidNumber, currentSquare))
                    AddToBlacklist(theFoundValidNumber, squareIndex);
                else
                    break;
                                
                theFoundValidNumber = GetValidNumberForTheSquare(squareIndex: squareIndex, useRandomFactor: useRandomFactor);
            } while (theFoundValidNumber != 0);

            return theFoundValidNumber;
        }
                
        private void AddToBlacklist(int value, int squareIndex) => BlackListsOfSquares[squareIndex].Add(value);
               
        private void ClearBlackList(int startCleaningFromThisIndex = 0)
        {
            for (int index = startCleaningFromThisIndex; index < BlackListsOfSquares.Count; index++)
                BlackListsOfSquares[index].Clear();
        }
    }
}