using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatorPileInMidas
{
    class SpringStiffnesHoriz
    {
        public double K { get; }
        public double Z { get; }
        public double BpX { get; }
        public double BpY { get; }
        public double T { get; }
        public double RX { get => K * Z * BpX * T; }
        public double RY { get => K * Z * BpY * T; }

        public SpringStiffnesHoriz(double K, double z, double bpX, double bpY, double t)
        {
            this.K = K;
            this.Z = z;
            this.BpX = bpX;
            this.BpY = bpY;
            this.T = t;
        }
        public override string ToString()
        {
            return $"RX={RX}kH, RY={RY}kH, z={Z}, K={K}";
        }
    }
}
