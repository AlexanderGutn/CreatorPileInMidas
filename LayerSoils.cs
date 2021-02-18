using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatorPileInMidas
{
    class LayerSoils:IEnumerable
    {
        List<LayerSoil> layerSoils = new List<LayerSoil>();

        public LayerSoils()
        {

        }
        public void Add(LayerSoil layerSoil)
        {
            layerSoils.Add(layerSoil);
        }
        public void Remove(LayerSoil layerSoil)
        {
            layerSoils.Remove(layerSoil);
        }

        public void Clear()
        {
            layerSoils.Clear();
        }

        public IEnumerator GetEnumerator()
        {
            return layerSoils.GetEnumerator();
        }       
    }
}
