using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatorPileInMidas
{
    static class CreatorNode
    { 
        public static Node CreateNode(double x, double y, double z)
        {
            return new Node(Node.Count,x,y,z);
        }
    }
}
