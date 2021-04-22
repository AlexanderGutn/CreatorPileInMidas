using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatorPileInMidas
{
    class Borehole
    {
        public int Number { get; set; }
        public List<LayerSoil> LayerSoils { get; }
        public string Name { get; set; }
        public int CountLayer { get => LayerSoils.Count; }
        public double LevelTop
        {
            get
            {
                double result = 0;
                foreach (var item in LayerSoils)
                {                   
                    
                    if (item.LevelTop > result)
                        result = item.LevelTop;
                }
                return result;
            }
        }

        public double LevelBot
        {
            get
            {
                if(CountLayer > 0)
                {
                    double result = LayerSoils[0].LevelBot;
                    foreach (var item in LayerSoils)
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
            LayerSoils = new List<LayerSoil>();
            Name = name;            
        }

        public Borehole(string name, List<LayerSoil> layerSoils)
        {            
            Name = name;
            LayerSoils = layerSoils;
        }

        public void AddLayerSoil(LayerSoil layerSoil)
        {
            LayerSoils.Add(layerSoil);
        }

        public void AddLayerSoils(List<LayerSoil> layerSoils1)
        {
            foreach (var item in layerSoils1)
            {
                LayerSoils.Add(item as LayerSoil);
            }
        }

        public void RemoveLayerSoil(LayerSoil layerSoil)
        {
            LayerSoils.Remove(layerSoil);
        }

        public void ClearLayerSoil()
        {
            LayerSoils.Clear();
        }

        //public void MyMethod()
        //{
        //    while (LayerSoils.MoveNext())
        //    {
        //        var a = LayerSoils.Current;
        //    }            
        //}

        public override string ToString()
        {
            return "Скважина: " + Name + ", верх: " + LevelTop.ToString() + ", низ: " + LevelBot.ToString() + ", слоев: " + CountLayer + ", глубина: " + Depth;
        }
    }
}
