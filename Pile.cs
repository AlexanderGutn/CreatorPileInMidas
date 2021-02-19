using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatorPileInMidas
{
    class Pile
    {        
        public double Diameter { get; set; }
        public double Length { get; set; }
        public double LevelTop { get; set; }
        public double LevelBot { get => LevelTop - Length; }
        public Borehole Borehole { get; set; }
        public double LevelOfLocalErosion { get; set; }
        public double bp { get => Diameter < 0.8 ? 1.5 * Diameter + 0.5 : Diameter + 1; }

        public LayerSoils LayerSoilsBellowGrillage    //Грунты ниже ростверка
        {
            get
            {
                LayerSoils LayersTemp = new LayerSoils();
                foreach (var layer in Borehole.LayerSoils)
                {
                    //if(layer.LevelTop > LevelTop && layer.LevelBot < LevelTop)
                    //    LayersTemp.Add(new LayerSoil(layer.Number,layer.GeologocalElement, LevelBot, layer.LevelBot));
                    //if (layer.LevelTop < LevelTop)
                    //    LayersTemp.Add(layer);
                }
                return LayersTemp;
            }
        }

        //public List<LayerSoil> LayerSoilsBellowGrillage1
        //{
        //    get
        //    {
        //        //layerSoilsBellowGrillage1 = new List<LayerSoil>();
        //        LayerSoilsBellowGrillage1.AddRange(layerSoilsBellowGrillage);
        //        return LayerSoilsBellowGrillage1;
        //    }
        //}



        public Pile(double diameter, double levelTop, double length, Borehole borehole, double levelOfLocalErosion)
        {
            Diameter = diameter;
            LevelTop = levelTop;
            Length = length;
            Borehole = borehole;
            LevelOfLocalErosion = levelOfLocalErosion;   
        }


        public LayerSoils GetLayerSoilsBellowGrillage()    //Грунты ниже ростверка
        {
                LayerSoils LayersTemp = new LayerSoils();
                foreach (var layer in Borehole.LayerSoils)
                {
                    //if (layer.LevelTop > LevelTop && layer.LevelBot < LevelTop)
                    //    LayersTemp.Add(new LayerSoil(layer.Number, layer.GeologocalElement, LevelBot, layer.LevelBot));
                    //if (layer.LevelTop < LevelTop)
                    //    LayersTemp.Add(layer);
                }
                return LayersTemp;
        }
    }
}
