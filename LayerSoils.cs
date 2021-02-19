using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatorPileInMidas
{
    class LayerSoils : IEnumerable, IEnumerator
    {        
        List<LayerSoil> layerSoils = new List<LayerSoil>();

        public int Count { get => layerSoils.Count; }

        public object Current => throw new NotImplementedException();

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

        public bool MoveNext()
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)layerSoils).GetEnumerator();
        }

        //public IEnumerator GetEnumerator()
        //{
        //    return layerSoils.GetEnumerator();
        //}
    }
}
