using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatorPileInMidas
{
    class Borehole
    {
        public LayerSoils LayerSoils { get; set; }
        public string Name { get; set; }
        public int CountLayer { get => LayerSoils.Count; }
        double LevelTop
        {
            get
            {
                double result = 0;
                //foreach (var item in LayerSoils)
                //{
                //    var b = item as LayerSoil; ////
                //    if (item.LevelTop > result)
                //        result = item.LevelTop;
                //}
                

                while (LayerSoils.MoveNext())
                {
                    
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
                    //double result = LayerSoils[0].LevelBot;
                    //foreach (var item in LayerSoils)
                    //{
                    //    if (item.LevelBot < result)
                    //        result = item.LevelBot;
                    //}
                    //return result;
                }
                return 0;
            }
        }

        public double Depth { get => LevelTop - LevelBot; }

        public Borehole(string name)
        {
            LayerSoils = new LayerSoils();
            Name = name;
            MyMethod();
        }

        public Borehole(string name, LayerSoils layerSoils)
        {
            //LayerSoils = new LayerSoils();
            Name = name;
            LayerSoils = layerSoils;
        }

        public void AddLayerSoil(LayerSoil layerSoil)
        {
            LayerSoils.Add(layerSoil);
        }

        public void AddLayerSoils(LayerSoils layerSoils1)
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

        public void MyMethod()
        {
            while (LayerSoils.MoveNext())
            {
                var a = LayerSoils.Current;
            }            
        }

        public override string ToString()
        {
            return "Скважина: " + Name + ", верх: " + LevelTop.ToString() + ", низ: " + LevelBot.ToString() + ", слоев: " + CountLayer + ", глубина: " + Depth;
        }
    }
}
