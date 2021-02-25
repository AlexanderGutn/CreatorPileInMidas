using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatorPileInMidas
{
    interface INode
    {
        int Number { get; }
        double X { get; }
        double Y { get; }
        double Z { get; }
    }
}
