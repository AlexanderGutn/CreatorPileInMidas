using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatorPileInMidas
{
    enum TypeCrossSectionEnum
    {
        Круглое,
        Прямоугольное
    }
    class Pile
    {
        public double CoordinateTopX { get; set; } = 0;
        public double CoordinateTopY { get; set; }       
        public double Length { get; set; }
        public double LevelTopPile { get; set; }
        public double LevelBotPile { get => LevelTopPile - Length; }
        public Borehole Borehole { get; set; }
        public double LevelOfLocalErosion { get; set; }
        public double LengthInGround => (LevelOfLocalErosion == 0) ? LevelTopPile - LevelBotPile : LevelOfLocalErosion - LevelBotPile;
        public virtual TypeCrossSectionEnum TypeCrossSection { get;}
        public virtual double bpx { get; }
        public virtual double bpy { get; }

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

        public List<LayerSoil> LayerSoilsBelowGrillage    //Грунты ниже ростверка
        {
            get
            {
                List<LayerSoil> LayersTemp = new List<LayerSoil>();
                int n = 0;
                foreach (var layer in Borehole.LayerSoils)
                {
                    if (layer.LevelTop > LevelTopPile && layer.LevelBot < LevelTopPile)
                        LayersTemp.Add(new LayerSoil(n,"new", layer.GeologocalElement, LevelTopPile, layer.LevelBot));
                    else if (layer.LevelTop < LevelTopPile)
                        LayersTemp.Add(new LayerSoil(n, "new", layer.GeologocalElement, layer.LevelTop, layer.LevelBot));
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
                        LayersTemp.Add(new LayerSoil(n, "new", layer.GeologocalElement, LevelTopPile, layer.LevelBot));
                    else if (layer.LevelTop < LevelTopPile && layer.LevelBot > LevelBotPile)
                        LayersTemp.Add(new LayerSoil(n, "new", layer.GeologocalElement, layer.LevelTop, layer.LevelBot));
                    else if (layer.LevelTop > LevelBotPile && layer.LevelBot < LevelBotPile)
                        LayersTemp.Add(new LayerSoil(n, "new", layer.GeologocalElement, layer.LevelTop, LevelBotPile));
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
                        LayersTemp.Add(new LayerSoil(n, "new", layer.GeologocalElement, LevelBotPile, layer.LevelBot));
                    else if (layer.LevelTop < LevelBotPile)
                        LayersTemp.Add(new LayerSoil(n, "new", layer.GeologocalElement, layer.LevelTop, layer.LevelBot));
                    n++;
                }
                return LayersTemp;
            }
        }       

        public Pile(TypeCrossSectionEnum typeCrossSection, double levelTop, double length, Borehole borehole, double levelOfLocalErosion)
        {
            //Diameter = diameter;
            TypeCrossSection = typeCrossSection;
            LevelTopPile = levelTop;
            Length = length;
            Borehole = borehole;
            LevelOfLocalErosion = levelOfLocalErosion;
        }
    }
}