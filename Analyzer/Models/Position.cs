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

        public static Position operator-(Position pos1, Position pos2)
        {
            return new Position(pos1.X - pos2.X, pos1.Y - pos2.Y);
        } 
        
        public static Position operator+(Position pos1, Position pos2)
        {
            return new Position(pos1.X + pos2.X, pos1.Y + pos2.Y);
        }

        public static bool operator==(Position pos1, Position pos2)
        {
            return (pos1.X == pos2.X) && (pos1.Y) == pos2.Y;
        }

        public static bool operator!=(Position pos1, Position pos2)
        {
            return (pos1.X != pos2.X) || (pos1.Y) != pos2.Y;
        }
    }
}
