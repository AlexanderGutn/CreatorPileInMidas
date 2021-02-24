using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatorPileInMidas
{
    class DimmyElementT
    {
        double Count { get => (ListH != null)? ListH.Count : 0; }
        public double Height { get;}
        public double LevelTop { get;}
        public double LevelBot { get => LevelTop - Height; }
        public List<double> ListH { get; set; }
        public List<double> ListK { get; set; }

        public double KAverage
        {
            get
            {
                if (ListH == null)
                    return 0;
                else
                {
                    double sum = 0;
                    for (int i = 0; i < ListH.Count; i++)
                    {
                        sum += ListH[i] * ListK[i];
                    }
                    return sum / ListH.Count;
                }
            }
        }
        public DimmyElementT(double levelTopT, double height)
        {
            ListH = new List<double>();
            ListK = new List<double>();
            Height = height;
            LevelTop = levelTopT;
        }
        public override string ToString()
        {
            return $"Толщина {Height}, {LevelTop}, {LevelBot}, кол-во слоев {Count}";
            //return "{Height}, {LevelTopT}, {LevelBotT}, кол-во слоев {ListH.Count}";
            //return $"R={R}kH, z={z};, K={K}";
        }


    }
}
