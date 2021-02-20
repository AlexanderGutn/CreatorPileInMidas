using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatorPileInMidas
{
    class PileAnalyticalScheme
    {
        public Pile Pile { get; set; }
        public double Step { get; set; }
        public SpringStiffnesVert SpringStiffnesVert { get; }

        public PileAnalyticalScheme(Pile pile, double step)
        {
            Pile = pile;
            Step = step;
            SpringStiffnesVert = new SpringStiffnesVert(Pile.UnderlyingLayer,Pile.Length);

        }
    }
}
