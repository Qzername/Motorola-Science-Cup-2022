using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer.Models
{
    public struct Position
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Position(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public static Position operator-(Position pos, int value)
        {
            pos.X -= value;
            pos.Y -= value;
            return pos;
        }
    }
}
