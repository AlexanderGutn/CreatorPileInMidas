﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatorPileInMidas
{
    class LayerSoil
    {
        public int Number { get; set; }
        public string Name { get; set; }
        public GeologocalElement GeologocalElement { get; set; }        
        public double Thickness { get => LevelTop - LevelBot;}
        public double LevelTop { get; set; }
        public double LevelBot { get; set; }

        public LayerSoil(int number, string name, GeologocalElement geologocalElement, double levelTop, double levelDown)
        {
            Number = number;                      //Можно убрать
            Name = name;
            GeologocalElement = geologocalElement;            
            LevelTop = levelTop;
            LevelBot = levelDown;
        }
        
        public LayerSoil(string name)
        {
            Name = name;
        }

        //public override string ToString()
        //{
        //    return Number + " " + Name + " " + GeologocalElement.ToString() + ", Top: " + LevelTop + ", Down: " + LevelBot + ", Мощность: " + Thickness;
        //}
    }

}
