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
        public double LevelTopPile { get; set; }
        public double LevelBotPile { get => LevelTopPile - Length; }
        public Borehole Borehole { get; set; }
        public double LevelOfLocalErosion { get; set; }
        public double bp { get => Diameter < 0.8 ? 1.5 * Diameter + 0.5 : Diameter + 1; }

        //public double LengthInGround { get = >;}

        public LayerSoil UnderlyingLayer
        {
            get
            {
                if (LayerSoilsBellowPile.Count > 0)
                {
                    return LayerSoilsBellowPile[0];
                }
                return null;
            }
        }

        public List<LayerSoil> LayerSoilsBellowGrillage    //Грунты ниже ростверка
        {
            get
            {
                List <LayerSoil> LayersTemp = new List<LayerSoil>();
                int n = 0;
                foreach (var layer in Borehole.LayerSoils)
                {
                    if (layer.LevelTop > LevelTopPile && layer.LevelBot < LevelTopPile)
                        LayersTemp.Add(new LayerSoil(n,layer.GeologocalElement, LevelTopPile, layer.LevelBot));
                    else if (layer.LevelTop < LevelTopPile)
                        LayersTemp.Add(new LayerSoil(n, layer.GeologocalElement, layer.LevelTop, layer.LevelBot));
                    n++;
                }
                return LayersTemp;
            }
        }

        public List<LayerSoil> LayerSoilsAtPileLevel    //Грунты прорезаемые сваей
        {
            get
            {
                List<LayerSoil> LayersTemp = new List<LayerSoil>();
                int n = 0;
                foreach (var layer in Borehole.LayerSoils)
                {
                    if (layer.LevelTop > LevelTopPile && layer.LevelBot < LevelTopPile)
                        LayersTemp.Add(new LayerSoil(n,layer.GeologocalElement, LevelTopPile, layer.LevelBot));                    
                    else if (layer.LevelTop < LevelTopPile && layer.LevelBot > LevelBotPile)
                        LayersTemp.Add(new LayerSoil(n, layer.GeologocalElement, layer.LevelTop, layer.LevelBot));
                    else if (layer.LevelTop > LevelBotPile && layer.LevelBot < LevelBotPile)
                        LayersTemp.Add(new LayerSoil(n,layer.GeologocalElement, layer.LevelTop, LevelBotPile));
                    n++;
                }
                return LayersTemp;
            }
        }

        public List<LayerSoil> LayerSoilsBellowPile    //Грунты ниже сваи
        {
            get
            {
                List<LayerSoil> LayersTemp = new List<LayerSoil>();
                int n = 0;
                foreach (var layer in Borehole.LayerSoils)
                {
                    if (layer.LevelTop > LevelBotPile && layer.LevelBot < LevelBotPile)
                        LayersTemp.Add(new LayerSoil(n, layer.GeologocalElement, LevelBotPile, layer.LevelBot));
                    else if (layer.LevelTop < LevelBotPile)
                        LayersTemp.Add(new LayerSoil(n, layer.GeologocalElement, layer.LevelTop, layer.LevelBot));
                    n++;
                }
                return LayersTemp;
            }
        }

        //public List<LayerSoil> LayerSoilsBellowGrillage1
        //{
        //    get
        //    {
        //        layerSoilsBellowGrillage1 = new List<LayerSoil>();
        //        LayerSoilsBellowGrillage1.AddRange(layerSoilsBellowGrillage);
        //        return LayerSoilsBellowGrillage1;
        //    }
        //}



        public Pile(double diameter, double levelTop, double length, Borehole borehole, double levelOfLocalErosion)
        {
            Diameter = diameter;
            LevelTopPile = levelTop;
            Length = length;
            Borehole = borehole;
            LevelOfLocalErosion = levelOfLocalErosion;   
        }



    }
}
