using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatorPileInMidas
{
    class SpringStiffnesVert
    {
        public LayerSoil LayerSoilsBellowPile { get; }
        public double H { get; }
        public double Rz { get => H < 10 ? LayerSoilsBellowPile.GeologocalElement.K * 10 : LayerSoilsBellowPile.GeologocalElement.K * H; }

        public SpringStiffnesVert(LayerSoil layerSoilsBellowPile, double h)
        {
            LayerSoilsBellowPile = layerSoilsBellowPile;
            H = h;
        }
    }
}
