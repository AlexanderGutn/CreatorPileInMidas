using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatorPileInMidas
{
    class GeologocalElement
    {        
        public string NumberIGE { get; }
        public GroundEnum GroundEnum { get; }
        public double e { get; }
        public double IL { get; }
        public double KUser { get; set; }

        public TypeGroundEnum TypeGroundEnum
        {
            get
            {
                TypeGroundEnum s = TypeGroundEnum.TypeSoftSoil;
                switch (GroundEnum)
                {
                    case GroundEnum.Глина:
                        s = TypeGroundEnum.TypeClay;
                        break;
                    case GroundEnum.Суглинок:
                        s = TypeGroundEnum.TypeClay;
                        break;
                    case GroundEnum.Супесь:
                        s = TypeGroundEnum.TypeClay;
                        break;
                    case GroundEnum.Лёссовый_грунт:
                        s = TypeGroundEnum.TypeClay;
                        break;
                    case GroundEnum.Песок_пылеватый:
                        s = TypeGroundEnum.TypeSand;
                        break;
                    case GroundEnum.Песок_мелкий:
                        s = TypeGroundEnum.TypeSand;
                        break;
                    case GroundEnum.Песок_средней_крупности:
                        s = TypeGroundEnum.TypeSand;
                        break;
                    case GroundEnum.Песок_крупный:
                        s = TypeGroundEnum.TypeSand;
                        break;
                    case GroundEnum.Песок_гравилистый:
                        s = TypeGroundEnum.TypeSand;
                        break;
                    case GroundEnum.Торф_ил:
                        s = TypeGroundEnum.TypeSoftSoil;
                        break;
                    default:
                        break;
                }
                return s;
            }
        }

        double K //kH/м4
        {
            get
            {                
                if (KUser > 0)
                {
                    return KUser;
                }
                else
                {
                    double result = 0;
                    switch (GroundEnum)
                    {
                        case GroundEnum.Глина:
                            if(-1 <= IL && IL <= 0)
                                result = InterpolationSingle(-1, 0, 30000, 18000, e);
                            else if(0 <= IL && IL <= 0.5)                            
                                result = InterpolationSingle(0, 0.5, 18000, 12000, e);
                            else if (0.5 <= IL && IL <= 0.75)
                                result = InterpolationSingle(0.5, 0.75, 12000, 7000, e);
                            else if (0.75 <= IL && IL <= 1)
                                result = InterpolationSingle(0.75, 1, 7000, 4000, e);
                            break;
                        case GroundEnum.Суглинок:
                            if (-1 <= IL && IL <= 0)
                                result = InterpolationSingle(-1, 0, 30000, 18000, e);
                            else if (0 <= IL && IL <= 0.5)
                                result = InterpolationSingle(0, 0.5, 18000, 12000, e);
                            else if (0.5 <= IL && IL <= 0.75)
                                result = InterpolationSingle(0.5, 0.75, 12000, 7000, e);
                            else if (0.75 <= IL && IL <= 1)
                                result = InterpolationSingle(0.75, 1, 7000, 4000, e);
                            break;
                        case GroundEnum.Супесь:
                            if (-1 <= IL && IL <= 0)
                                result = InterpolationSingle(-1, 0, 18000, 12000, e);
                            else if (0 <= IL && IL <= 0.5)
                                result = InterpolationSingle(0, 0.5, 12000, 8667, e);
                            else if (0.5 <= IL && IL <= 0.75)
                                result = InterpolationSingle(0.5, 0.75, 8667, 7000, e);                            
                            break;
                        case GroundEnum.Лёссовый_грунт:  //Принял как супесь
                            if (-1 <= IL && IL <= 0)
                                result = InterpolationSingle(-1, 0, 18000, 12000, e);
                            else if (0 <= IL && IL <= 0.5)
                                result = InterpolationSingle(0, 0.5, 12000, 8667, e);
                            else if (0.5 <= IL && IL <= 0.75)
                                result = InterpolationSingle(0.5, 0.75, 8667, 7000, e);
                            break;
                        case GroundEnum.Песок_пылеватый:
                            result = InterpolationSingle(0.6, 0.8, 12000, 7000, e);
                            break;
                        case GroundEnum.Песок_мелкий:
                            result = InterpolationSingle(0.6, 0.75, 18000, 12000, e);
                            break;
                        case GroundEnum.Песок_средней_крупности:
                            result = InterpolationSingle(0.55, 0.7, 18000, 12000, e);
                            break;
                        case GroundEnum.Песок_крупный:
                            result = InterpolationSingle(0.55, 0.7, 30000, 18000, e);
                            break;
                        case GroundEnum.Песок_гравилистый:
                            result = InterpolationSingle(0.55, 0.7, 100000, 50000, e);
                            break;
                        case GroundEnum.Торф_ил:
                            break;
                        default:
                            break;
                    }
                    return result;
                }                
            }
        }


        public GeologocalElement(string NumberIGE, GroundEnum groundEnum, double e,double kUser, double IL)
        {
            this.NumberIGE = NumberIGE;
            this.GroundEnum = GroundEnum;
            this.e = e;
            this.IL = IL;
            this.KUser = kUser;
        }
        public GeologocalElement(string numberIGE, GroundEnum groundEnum, double e, double kUser)
        {
            this.NumberIGE = numberIGE;
            this.GroundEnum = groundEnum;
            this.e = e;
            this.IL = 0;
            this.KUser = kUser;            
        }

        double InterpolationSingle(double x1, double x2, double y1, double y2, double x)
        {
            double xmin = Math.Min(x1, x2);
            double xmax = Math.Max(x1, x2);

            if (xmin <= x && x <= xmax)
            {
                return y1 + (y2 - y1) / (x2 - x1) * (x - x1);
            }
            else
                return 0;
        }

        public override string ToString()
        {
            return NumberIGE + " " + GroundEnum + " e=" + e + " IL=" + IL + " K=" + K;
        }

    }

    enum GroundEnum
    {
        Глина,
        Суглинок,
        Супесь,
        Лёссовый_грунт,
        Песок_пылеватый,
        Песок_мелкий,
        Песок_средней_крупности,
        Песок_крупный,
        Песок_гравилистый,
        Торф_ил
    }

    enum TypeGroundEnum
    {
        TypeClay,
        TypeSand,
        TypeSoftSoil
    }

}

