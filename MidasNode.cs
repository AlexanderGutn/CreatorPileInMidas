using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatorPileInMidas
{
    class MidasNode :INode
    {
        public static string Header = "*NODE    ; Nodes ; iNO, X, Y, Z";
        public int Number { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        
        public MidasNode(int number, double x, double y, double z)
        {
            Number = number;
            X = x;
            Y = y;
            Z = z;
        }       

        public override string ToString()
        {
            return $"{Number},{X},{Y},{Z}";
        }
    }

}



