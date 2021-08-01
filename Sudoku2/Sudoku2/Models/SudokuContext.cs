using System.Data.Entity;



namespace Sudoku2.Models
{
    public class SudokuContext : DbContext
    {        
        public DbSet<GameResult> Results { get; set; }
    }
}