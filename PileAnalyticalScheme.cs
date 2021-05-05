using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatorPileInMidas
{
    class PileAnalyticalScheme
    {
        private List<Node> nodes;
        private List<MidasNode> midasNodes;
        private List<MidasBeamElement> midasBeamElements;
        public Pile Pile { get; set; }
        public double Step { get; set; }
        public SpringStiffnesVert SpringStiffnesVert { get; }
        public List<Node> Nodes { get => nodes; }
        public List<MidasNode> MidasNodes { get => midasNodes; }
        public List<MidasBeamElement> MidasBeamElements { get => midasBeamElements; }

        public List<SpringStiffnesHoriz> SpringStiffnesHoriz  //можно переписать по данным Nodes
        {
            get
            {
                List<SpringStiffnesHoriz> SpringStiffnesHorizTemp = new List<SpringStiffnesHoriz>();
                //Определение поверхности грунта (с учетом уровня размыва)
                double levelTopGround = (Pile.LevelOfLocalErosion == 0) ? Pile.LevelTopPile : Pile.LevelOfLocalErosion;

                //Определение абсолютных отметок пружин, относительно низа ростверка
                //int num = Int32.Parse((Pile.Length / Step + 1).ToString());
                int num = (int)((levelTopGround - Pile.LevelBotPile) / Step + 1);

                double[] coordAbsZ = new double[num];
                double[] coordRelativeZ = new double[num];

                double remainsPile = levelTopGround - Pile.LevelBotPile;

                coordAbsZ[0] = levelTopGround - Step / 2;
                coordRelativeZ[0] = Step / 2;
                remainsPile -= coordRelativeZ[0];

                int index = 1;
                double zTemp;
                while (remainsPile - Step > 0)
                {
                    zTemp = coordAbsZ[index - 1] - Step;
                    coordAbsZ[index] = zTemp;
                    coordRelativeZ[index] = levelTopGround - coordAbsZ[index];
                    remainsPile -= Step;
                    index++;
                }

                //Заполнение последнего элемента
                if (remainsPile > Step / 2)
                {
                    coordRelativeZ[index] = coordRelativeZ[index - 1] + remainsPile / 2;
                    coordAbsZ[index] = levelTopGround - coordRelativeZ[index];
                }

                int quantity = 0;
                for (int i = 0; i < coordRelativeZ.Length; i++)
                {
                    if (coordRelativeZ[i] > 0)
                        quantity++;
                }

                if (coordRelativeZ.Length > quantity)
                {
                    Array.Resize(ref coordAbsZ, quantity);
                    Array.Resize(ref coordRelativeZ, quantity);
                }

                double[] t = new double[quantity];
                double[] z = coordRelativeZ;

                for (int i = 1; i < coordAbsZ.Length; i++)
                {
                    if (i == 1)
                    {
                        t[i - 1] = levelTopGround - (coordAbsZ[i - 1] * 0.5 + coordAbsZ[i] * 0.5);
                        t[i] += (coordAbsZ[i - 1] * 0.5 + coordAbsZ[i] * 0.5) - coordAbsZ[i];
                    }
                    else
                    {
                        t[i - 1] += coordAbsZ[i - 1] - (coordAbsZ[i - 1] * 0.5 + coordAbsZ[i] * 0.5);
                        t[i] += (coordAbsZ[i - 1] * 0.5 + coordAbsZ[i] * 0.5) - coordAbsZ[i];
                    }
                }
                t[coordAbsZ.Length - 1] += coordAbsZ[coordAbsZ.Length - 1] - Pile.LevelBotPile;

                double[] Kh = new double[quantity];
                List<DimmyElementT> ListDimmyElementT = new List<DimmyElementT>();

                for (int i = 0; i < t.Length; i++)
                {
                    ListDimmyElementT.Add(new DimmyElementT(coordAbsZ[i], t[i]));
                }



                for (int i = 0; i < coordAbsZ.Length; i++)
                {
                    foreach (var layer in Pile.LayerSoilsAtPileLevel)
                    {
                        //Элемент е пересекает только 1 слой ИГЭ
                        if (layer.LevelTop > ListDimmyElementT[i].LevelTop && layer.LevelBot < ListDimmyElementT[i].LevelBot)
                        {
                            var a = ListDimmyElementT[i].Height;
                            ListDimmyElementT[i].ListH.Add(ListDimmyElementT[i].Height);
                            ListDimmyElementT[i].ListK.Add(layer.GeologocalElement.K);
                            break;
                        }
                        //слой сверху
                        if (layer.LevelTop > ListDimmyElementT[i].LevelTop && layer.LevelBot < ListDimmyElementT[i].LevelTop)
                        {
                            ListDimmyElementT[i].ListH.Add(ListDimmyElementT[i].LevelTop - layer.LevelBot);
                            ListDimmyElementT[i].ListK.Add(layer.GeologocalElement.K);
                        }
                        //Слой уместился внутри
                        if (layer.LevelTop < ListDimmyElementT[i].LevelTop && layer.LevelBot > ListDimmyElementT[i].LevelBot)
                        {
                            ListDimmyElementT[i].ListH.Add(layer.LevelTop - layer.LevelBot);
                            ListDimmyElementT[i].ListK.Add(layer.GeologocalElement.K);
                        }
                        //Слой снизу
                        if (layer.LevelTop > ListDimmyElementT[i].LevelBot && layer.LevelBot < ListDimmyElementT[i].LevelBot)
                        {
                            ListDimmyElementT[i].ListH.Add(layer.LevelTop - ListDimmyElementT[i].LevelBot);
                            ListDimmyElementT[i].ListK.Add(layer.GeologocalElement.K);
                        }
                    }
                }

                for (int i = 0; i < coordAbsZ.Length; i++)
                {
                    SpringStiffnesHorizTemp.Add(new SpringStiffnesHoriz(ListDimmyElementT[i].KAverage, z[i], Pile.bpx, Pile.bpy, t[i]));
                }
                return SpringStiffnesHorizTemp;
            }
        }

        public PileAnalyticalScheme(Pile pile, double step)
        {
            Pile = pile;
            Step = step;
            SpringStiffnesVert = new SpringStiffnesVert(Pile.UnderlyingLayer, Pile.Length);
            Initialaze();
        }

        private void Initialaze()
        {
            FillingNodes();
            FillingMidasNodes();
            FillingMidasElements();
        }

        private void FillingNodes()
        {
            nodes = new List<Node>();
            int n = 1;
            //Определение поверхности грунта (с учетом уровня размыва)
            double levelTopGround = (Pile.LevelOfLocalErosion == 0) ? Pile.LevelTopPile : Pile.LevelOfLocalErosion;

            //nodes.Add(Node.CreateNode(n, Pile.CoordinateTopX, Pile.CoordinateTopY, Pile.LevelTopPile)); n++;
            nodes.Add(Node.CreateNode(n, Pile.CoordinateTopX, Pile.CoordinateTopY, Pile.CoordinateTopZ)); n++;
            nodes.Add(Node.CreateNode(n, Pile.CoordinateTopX, Pile.CoordinateTopY, Pile.CoordinateTopZ - Pile.LevelTopPile + levelTopGround - Step / 2)); n++;

            double remainsPile = levelTopGround - Pile.LevelBotPile - Step / 2;
            int index = 2;

            while (remainsPile - Step > 0)
            {
                nodes.Add(Node.CreateNode(n, Pile.CoordinateTopX, Pile.CoordinateTopY, nodes[index - 1].Z - Step)); n++;
                remainsPile -= Step;
                index++;
            }

            //Заполнение последнего элемента 56КЭ
            if (remainsPile > Step / 2)
                nodes.Add(Node.CreateNode(n, Pile.CoordinateTopX, Pile.CoordinateTopY, nodes[index - 1].Z - remainsPile / 2)); n++;

            nodes.Add(Node.CreateNode(n, Pile.CoordinateTopX, Pile.CoordinateTopY, Pile.CoordinateTopZ - (Pile.LevelTopPile - Pile.LevelBotPile)));
        }

        private void FillingMidasNodes()
        {
            midasNodes = new List<MidasNode>();

            foreach (var node in nodes)
            {
                midasNodes.Add(new MidasNode(node.Number, node.X, node.Y, node.Z));
            }
        }

        private void FillingMidasElements()
        {
            midasBeamElements = new List<MidasBeamElement>();

            for (int i = 1; i < nodes.Count; i++)
            {
                midasBeamElements.Add(new MidasBeamElement(i, 100, 100, i, i + 1));
            }
        }
    }


    enum MaterialEnum
    {
        B20,
        B25,
        B30,
        B35,
        B40
    }


}


