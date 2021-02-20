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
        public double z { get; }
        public double bp { get; }
        public double t { get; }
        public double R { get => K * z * bp * t; }

        public SpringStiffnesHoriz(double K, double z, double bp, double t, double R)
        {
            this.K = K;
            this.z = z;
            this.bp = bp;
            this.t = R;
        }
    }
}
