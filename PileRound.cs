using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatorPileInMidas
{
    class PileRound : Pile
    {
        //public new TypeCrossSectionEnum TypeCrossSection { get; set; } = TypeCrossSectionEnum.Round;
        public double Diameter { get; set; }
        public override double bpx { get => Diameter < 0.8 ? 1.5 * Diameter + 0.5 : Diameter + 1; }
        public override double bpy => bpx;

        public PileRound(double diameter, double levelTop, double length, Borehole borehole, double levelOfLocalErosion) 
            : base(TypeCrossSectionEnum.Круглое, levelTop, length, borehole, levelOfLocalErosion)
        {
            Diameter = diameter;
        }
    }
}
