using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;


namespace Sudoku2.Models
{
    public class BoardDbInitializer:DropCreateDatabaseAlways<SudokuContext>
    {
        protected override void Seed(SudokuContext context)
        {            

            base.Seed(context);
        }
    }
}