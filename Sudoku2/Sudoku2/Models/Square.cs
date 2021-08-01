using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sudoku2.Models
{
    public class Square
    {
        public int SquareId { get; set; }
        public int Index { get; set; }
        public int Value { get; set; }
        public int GroupNo { get; }

        public struct RCPosition
        {

            public int Row { get; internal set; }


            public int Column { get; internal set; }

            public RCPosition(int row, int column)
            {
                Row = row;
                Column = column;
            }
        }




        public RCPosition Position { get; }

        public Square(int value, int index, int groupNo, RCPosition position)
        {
            Value = value;
            Index = index;
            GroupNo = groupNo;
            Position = position;
        }
    }
}