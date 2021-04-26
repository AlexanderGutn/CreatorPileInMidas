using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatorPileInMidas
{
    class LayerSoil
    {
        public string Name { get; set; }
        public GeologocalElement GeologocalElement { get; set; }
        public int Number { get; set; } 
        public double Thickness { get => LevelTop - LevelBot;}
        public double LevelTop { get; set; }
        public double LevelBot { get; set; }

        public LayerSoil(int number, GeologocalElement geologocalElement, double levelTop, double levelDown)
        {
            Number = number;                      //Можно убрать
            GeologocalElement = geologocalElement;            
            LevelTop = levelTop;
            LevelBot = levelDown;
        }

        public LayerSoil()
        {

        }
        public override string ToString()
        {
            return Number + " " + GeologocalElement.ToString() + ", Top: " + LevelTop + ", Down: " + LevelBot + ", Мощность: " + Thickness;
        }
    }

    class Test
    {
        //public string Name { get; set; }
        public string IGE { get; set; }
        public int Number { get; set; }
        //public double Thickness { get => LevelTop - LevelBot; }
        public double LevelTop { get; set; }
        public double LevelBot { get; set; }

        public Test(int number, string IGE, double levelTop, double levelDown)
        {
            Number = number;                      //Можно убрать
            this.IGE = IGE;
            LevelTop = levelTop;
            LevelBot = levelDown;
        }

        public Test()
        {

        }
        //public override string ToString()
        //{
        //    return Number + " " + GeologocalElement.ToString() + ", Top: " + LevelTop + ", Down: " + LevelBot + ", Мощность: " + Thickness;
        //}
    }
}
