using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CreatorPileInMidas
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            GeologocalElement geologocalElement1 = new GeologocalElement("ИГЭ1", GroundEnum.Песок_крупный, 0.55, 100);
            GeologocalElement geologocalElement2 = new GeologocalElement("ИГЭ2", GroundEnum.Глина, 0.5, 0, 0.4);
            GeologocalElement geologocalElement3 = new GeologocalElement("ИГЭ3", GroundEnum.Песок_мелкий, 0.6, 0);
            GeologocalElement geologocalElement4 = new GeologocalElement("ИГЭ4", GroundEnum.Песок_пылеватый, 0.625, 0);
            GeologocalElement geologocalElement5 = new GeologocalElement("ИГЭ5", GroundEnum.Песок_средней_крупности, 0.7, 0);
            GeologocalElement geologocalElement6 = new GeologocalElement("ИГЭ6", GroundEnum.Суглинок, 0.71, 0, 0.6);

            LayerSoil layerSoil1 = new LayerSoil(1, geologocalElement1, 100, 90);
            LayerSoil layerSoil2 = new LayerSoil(2, geologocalElement2, 90, 80);
            LayerSoil layerSoil3 = new LayerSoil(3, geologocalElement6, 80, 50);

            List<LayerSoil> layerSoils = new List<LayerSoil>();
            layerSoils.Add(layerSoil1);
            layerSoils.Add(layerSoil2);
            layerSoils.Add(layerSoil3);

            Borehole borehole1 = new Borehole("Скв1");
            borehole1.AddLayerSoil(layerSoil1);
            borehole1.AddLayerSoil(layerSoil2);
            borehole1.AddLayerSoil(layerSoil3);


            Borehole borehole2 = new Borehole("Скв2");
            borehole2.AddLayerSoils(layerSoils);

            Borehole borehole3 = new Borehole("Скв2", layerSoils);


            Pile pile1 = new Pile(1, 85, 10.2, borehole1, 80);
            var a = pile1.LayerSoilsBellowGrillage;
            var a1 = pile1.LayerSoilsAtPileLevel;

            PileAnalyticalScheme pileAnalyticalScheme = new PileAnalyticalScheme(pile1, 1);
            var temp = pileAnalyticalScheme.SpringStiffnesHoriz;
            int i = 1;
        }


    }
}
