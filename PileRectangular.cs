using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatorPileInMidas
{
    class PileRectangular : Pile
    {
        //public new TypeCrossSectionEnum TypeCrossSection { get; set; } = TypeCrossSectionEnum.Rectangular;
        public double SideX { get; set; }
        public double SideY { get; set; }
        public override double bpx => SideX < 0.8 ? 1.5 * SideX + 0.5 : SideX + 1;
        public override double bpy => SideY < 0.8 ? 1.5 * SideY + 0.5 : SideY + 1;

        public PileRectangular(double sideX, double sideY, double levelTop, double length, Borehole borehole, double levelOfLocalErosion) 
            : base(TypeCrossSectionEnum.Rectangular, levelTop, length, borehole, levelOfLocalErosion)
        {
            SideX = sideX;
            SideY = sideY;            
        }
    }
}
