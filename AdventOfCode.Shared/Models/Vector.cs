using AdventOfCode.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Shared.Models
{
    public record Vector
    {
        public Vector() { }

        public Vector(int x, int y, int z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public Vector(string vector)
        {
            var parts = vector.Split(',').Select(v => int.Parse(v));
            this.X = parts.ElementAt(0);
            this.Y = parts.ElementAt(1);
            this.Z = parts.ElementAt(2);
        }

        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public string Reference => $"{X}_{Y}_{Z}";

        public double Distance(Vector vector)
        {
            var x = Math.Pow((vector.X - this.X), 2);
            var y = Math.Pow((vector.Y - this.Y), 2);
            var z = Math.Pow((vector.Z - this.Z), 2);

            return Math.Sqrt(x + y + z);
        }
    }
}
