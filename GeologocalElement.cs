using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatorPileInMidas
{
    class GeologocalElement
    {
        public int Number { get; set; }
        public string NameIGE { get; set; }
        public string NumberNameIGE => Number +" "+ NameIGE;
        public GroundEnum GroundEnum { get; set; }
        public double e { get; set; }
        public double IL { get; set; }
        public double KUser { get; set; }
        public double KApply { get => KUser > 0 ? KUser : K; }

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

        public double K //kH/м4
        {
            get
            {
                //if (KUser > 0)
                //{
                //    return KUser;
                //}
                //else
                //{
                    double result = 0;
                    switch (GroundEnum)
                    {
                        case GroundEnum.Глина:
                            if (-1 <= IL && IL <= 0)
                                result = InterpolationSingle(-1, 0, 10000, 6000, IL);
                            else if (0 <= IL && IL <= 0.5)
                                result = InterpolationSingle(0, 0.5, 6000, 4000, IL);
                            else if (0.5 <= IL && IL <= 0.75)
                                result = InterpolationSingle(0.5, 0.75, 4000, 2350, IL);
                            else if (0.75 <= IL && IL <= 1)
                                result = InterpolationSingle(0.75, 1, 2350, 1350, IL);
                            break;
                        case GroundEnum.Суглинок:
                            if (-1 <= IL && IL <= 0)
                                result = InterpolationSingle(-1, 0, 10000, 6000, IL);
                            else if (0 <= IL && IL <= 0.5)
                                result = InterpolationSingle(0, 0.5, 6000, 4000, IL);
                            else if (0.5 <= IL && IL <= 0.75)
                                result = InterpolationSingle(0.5, 0.75, 4000, 2350, IL);
                            else if (0.75 <= IL && IL <= 1)
                                result = InterpolationSingle(0.75, 1, 2350, 1350, IL);
                            break;
                    case GroundEnum.Супесь:
                            if (-1 <= IL && IL <= 0)
                                result = InterpolationSingle(-1, 0, 6000, 4000, IL);
                            else if (0 <= IL && IL <= 0.5)
                                result = InterpolationSingle(0, 0.5, 4000, 2889, IL);
                            else if (0.5 <= IL && IL <= 0.75)
                                result = InterpolationSingle(0.5, 0.75, 2889, 2350, IL);
                            break;
                        case GroundEnum.Лёссовый_грунт:  //Принял как супесь
                        if (-1 <= IL && IL <= 0)
                            result = InterpolationSingle(-1, 0, 6000, 4000, IL);
                        else if (0 <= IL && IL <= 0.5)
                            result = InterpolationSingle(0, 0.5, 4000, 2889, IL);
                        else if (0.5 <= IL && IL <= 0.75)
                            result = InterpolationSingle(0.5, 0.75, 2889, 2350, IL);
                        break;
                    case GroundEnum.Песок_пылеватый:
                            result = InterpolationSingle(0.6, 0.8, 4000, 2350, e);
                            break;
                        case GroundEnum.Песок_мелкий:
                            result = InterpolationSingle(0.6, 0.75, 6000, 4000, e);
                            break;
                        case GroundEnum.Песок_средней_крупности:
                            result = InterpolationSingle(0.55, 0.7, 6000, 4000, e);
                            break;
                        case GroundEnum.Песок_крупный:
                            result = InterpolationSingle(0.55, 0.7, 10000, 6000, e);
                            break;
                        case GroundEnum.Песок_гравилистый:
                            result = InterpolationSingle(0.55, 0.7, 33350, 16750, e);
                            break;
                        case GroundEnum.Торф_ил:
                        result = 10;
                            break;
                        default:
                            break;
                    }
                    return result;
            //    }
            }
        }


        public GeologocalElement(string numberIGE, GroundEnum groundEnum, double e, double kUser, double IL)
        {
            this.NameIGE = numberIGE;
            this.GroundEnum = groundEnum;
            this.e = e;
            this.IL = IL;
            this.KUser = kUser;
        }
        public GeologocalElement(string numberIGE, GroundEnum groundEnum, double e, double kUser)
        {
            this.NameIGE = numberIGE;
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
            return NameIGE + " " + GroundEnum + " e=" + e + " IL=" + IL + " K=" + K;
        }

    }

    public enum GroundEnum
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

