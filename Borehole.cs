using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatorPileInMidas
{
    class Borehole
    {
        List<LayerSoil> layerSoils = new List<LayerSoil>();
        public string Name { get; set; }
        public int CountLayer { get => layerSoils.Count; }
        double LevelTop
        {
            get
            {
                double result = 0;
                foreach (var item in layerSoils)
                {
                    if (item.LevelTop > result)
                        result = item.LevelTop;
                }
                return result;
            }
        }

        double LevelBot
        {
            get
            {
                if(CountLayer > 0)
                {
                    double result = layerSoils[0].LevelBot;
                    foreach (var item in layerSoils)
                    {
                        if (item.LevelBot < result)
                            result = item.LevelBot;
                    }
                    return result;
                }
                return 0;
            }
        }

        public double Depth { get => LevelTop - LevelBot; }

        public Borehole(string name)
        {
            Name = name;
        }
        public void AddLayerSoil(LayerSoil layerSoil)
        {
            layerSoils.Add(layerSoil);
        }

        public void AddLayerSoils(LayerSoils layerSoils1)
        {
            foreach (var item in layerSoils1)
            {
                layerSoils.Add(item as LayerSoil);
            }

        }

        public void RemoveLayerSoil(LayerSoil layerSoil)
        {
            layerSoils.Remove(layerSoil);
        }

        public void ClearLayerSoil()
        {
            layerSoils.Clear();
        }

        public override string ToString()
        {
            return "Скважина: " + Name + ", верх: " + LevelTop.ToString() + ", низ: " + LevelBot.ToString() + ", слоев: " + CountLayer + ", глубина: " + Depth;
        }
    }
}
