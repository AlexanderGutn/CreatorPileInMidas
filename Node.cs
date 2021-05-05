using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatorPileInMidas
{
    class Node : INode
    {
        //public static int Count = 1;
        public int Number { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public Node(int number, double x, double y, double z)
        {
            Number = number;
            X = x;
            Y = y;
            Z = z;
            //Count++;
        }

        public override string ToString()
        {
            return $"{Number},{X},{Y},{Z}";
        }

        public static Node CreateNode(int number, double x, double y, double z)
        {
            return new Node(number, x, y, z);
        }
    }
}
