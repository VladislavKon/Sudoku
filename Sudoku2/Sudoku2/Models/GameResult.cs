using System;


namespace Sudoku2.Models
{
    public class GameResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Squares { get; set; }
        public TimeSpan Time { get; set; }
    }
}