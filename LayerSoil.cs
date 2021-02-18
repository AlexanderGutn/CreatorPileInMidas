using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatorPileInMidas
{
    class LayerSoil
    {
        public GeologocalElement GeologocalElement { get; set; }
        public int Number { get; } 
        public double Thickness { get => LevelTop- LevelBot;}
        public double LevelTop { get; set; }
        public double LevelBot { get; set; }

        public LayerSoil(int number, GeologocalElement geologocalElement, double levelTop, double levelDown)
        {
            Number = number;                      //Можно убрать
            GeologocalElement = geologocalElement;            
            LevelTop = levelTop;
            LevelBot = levelDown;
        }
        public override string ToString()
        {
            return Number + " " + GeologocalElement.ToString() + ", Top: " + LevelTop + ", Down: " + LevelBot + ", Мощность: " + Thickness;
        }
    }
}
