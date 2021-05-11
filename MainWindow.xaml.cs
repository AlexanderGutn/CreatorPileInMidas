using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CreatorPileInMidas
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MaterialEnum materialEnum;
        TypeCrossSectionEnum typeCrossSectionEnum;
        double sidePileX;
        double sidePileY;
        double lengthPile;
        double levelTopPile;
        double levelOfLocalErosion;
        bool isCreateMaterial;
        bool isCreateCrossSection;
        double step;
        double CoordX;
        double CoordY;
        double CoordZ;

        int numbIGE;
        List<GeologocalElement> ListGeoElements = new List<GeologocalElement>();
        List<string> ListIGE = new List<string>();
        GeologocalElement prevDGIGESelectGeologocalElement;
        int prevDGIGEIndexColumn = -1;

        List<Borehole> ListBoreholes = new List<Borehole>();
        Borehole currentBorehol;
        int numCurrentBorehol = -1;
        int numbIGEinBore;
        Pile pile;

        bool canDeleteItemIGE = false;
        bool canDeleteItemBoreholes = false;
        bool canDeleteItemCurrentBoreholes = false; 
        DocForMidas docForMidas;
        int NumbStartNode;
        int NumbStartElement;

        public MainWindow()
        {
            InitializeComponent();
            Initialization();  

            Debug.Print("Начало");
            //GeologocalElement geologocalElement1 = new GeologocalElement("ИГЭ1", GroundEnum.Песок_крупный, 0.55, 100);
            //GeologocalElement geologocalElement2 = new GeologocalElement("ИГЭ2", GroundEnum.Глина, 0.5, 0, 0.4);
            //GeologocalElement geologocalElement3 = new GeologocalElement("ИГЭ3", GroundEnum.Песок_мелкий, 0.6, 0);
            //GeologocalElement geologocalElement4 = new GeologocalElement("ИГЭ4", GroundEnum.Песок_пылеватый, 0.625, 0);
            //GeologocalElement geologocalElement5 = new GeologocalElement("ИГЭ5", GroundEnum.Песок_средней_крупности, 0.7, 0);
            //GeologocalElement geologocalElement6 = new GeologocalElement("ИГЭ6", GroundEnum.Суглинок, 0.71, 0, 0.6);

            //LayerSoil layerSoil1 = new LayerSoil(1, geologocalElement1, 100, 90);
            //LayerSoil layerSoil2 = new LayerSoil(2, geologocalElement2, 90, 80);
            //LayerSoil layerSoil3 = new LayerSoil(3, geologocalElement6, 80, 50);

            //List<LayerSoil> layerSoils = new List<LayerSoil>();
            //layerSoils.Add(layerSoil1);
            //layerSoils.Add(layerSoil2);
            //layerSoils.Add(layerSoil3);

            //Borehole borehole1 = new Borehole("Скв1");
            //borehole1.AddLayerSoil(layerSoil1);
            //borehole1.AddLayerSoil(layerSoil2);
            //borehole1.AddLayerSoil(layerSoil3);


            //Borehole borehole2 = new Borehole("Скв2");
            //borehole2.AddLayerSoils(layerSoils);

            //Borehole borehole3 = new Borehole("Скв2", layerSoils);

            //PileRectangular pile1 = new PileRectangular(0.3, 0.8, 85, 10.2, borehole1, 80);

            //var a = pile1.LayerSoilsBelowGrillage;
            //var a1 = pile1.LayerSoilsAtPileLevel;

            //PileAnalyticalScheme pileAnalyticalScheme = new PileAnalyticalScheme(pile1, 1);
            //var temp = pileAnalyticalScheme.SpringStiffnesHoriz;

            //docForMidas = new DocForMidas(pileAnalyticalScheme, MaterialEnum.B25, pile1.SideX, pile1.SideY);

            //tbCommand.Text = docForMidas.WriteDoc();
            //int i = 1;
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
        }

        //Установка значений по умолчанию и связывание
        private void Initialization()
        {   
            DGTableIGE.ItemsSource = ListGeoElements;
            DGBoreholes.ItemsSource = ListBoreholes;
            ListBoreholes.Add(new Borehole("Скв1"));

            currentBorehol = ListBoreholes[0];
            numCurrentBorehol = 0;           
            DGCurrentBorehole.ItemsSource = ListBoreholes[numCurrentBorehol].LayerSoils;
            DGCurrentBorehole.CanUserAddRows = false;

            //Привязка списка доступных значений для ComboBox
            (DGCurrentBorehole.Columns[1] as DataGridComboBoxColumn).ItemsSource = ListIGE;


            ListGeoElements.Add(new GeologocalElement("ИГЭ1", GroundEnum.Песок_крупный, 0.6, 0));
            ListBoreholes[0].AddLayerSoil(new LayerSoil(1, "1", ListGeoElements[0], 100, 20));
            tbLevelOfLocalErosion.Text = "0";
            
        }

        //Считать данные с элементов управления
        private void GetTB()
        {
            int.TryParse(tbNumbIGE.Text, out numbIGE);
            numbIGE = numbIGE > 0 ? numbIGE : 0;

            int.TryParse(tbNumbIGEInBore.Text, out numbIGEinBore);
            numbIGEinBore = numbIGEinBore > 0 ? numbIGEinBore : 0;
        }

        private void calcDataGrid()
        {
            GetTB();
            int num = 1;
            foreach (var geoElement in ListGeoElements)
            {
                geoElement.Number = num;
                num++;
            }

            this.DGTableIGE.CommitEdit();
            this.DGTableIGE.CommitEdit();
            DGTableIGE.Items.Refresh();

            num = 1;
            foreach (var borehole in ListBoreholes)
            {
                borehole.Number = num;
                num++;
            }

            this.DGBoreholes.CommitEdit();
            this.DGBoreholes.CommitEdit();
            DGBoreholes.Items.Refresh();

            num = 1;
            if (ListBoreholes.Count != 0)
            {
                foreach (var layerSoil in ListBoreholes[numCurrentBorehol].LayerSoils)
                {
                    layerSoil.Number = num;
                    num++;
                    foreach (var IGE in ListGeoElements)
                    {
                        if (IGE.NumberNameIGE == layerSoil.Name)
                            layerSoil.GeologocalElement = IGE;
                    }
                }
                //добавление к текущей скважене слоев грунта
                currentBorehol = ListBoreholes[numCurrentBorehol];//////////////////////////////////////////////
            }

            DGCurrentBorehole.CommitEdit();
            DGCurrentBorehole.CommitEdit();
            DGCurrentBorehole.Items.Refresh(); ;

            //Изменение листа доступных ИГЭ для скважин
            ListIGE.Clear();
            foreach (var item in ListGeoElements)
            {
                ListIGE.Add(item.NumberNameIGE);
            }
        }

        void MainMethod()
        {
            calcDataGrid();
            Enum.TryParse(cbClassBeton.Text, out materialEnum);
            Enum.TryParse(cbTypeCrossSection.Text, out typeCrossSectionEnum);
            Double.TryParse(tbDim1.Text, out sidePileX);
            Double.TryParse(tbDim2.Text, out sidePileY);
            Double.TryParse(tbLenghtPile.Text, out lengthPile);
            Double.TryParse(tbTopPile.Text, out levelTopPile);
            Double.TryParse(tbLevelOfLocalErosion.Text, out levelOfLocalErosion);
            isCreateMaterial = (bool)chbCreateMaterial.IsChecked;
            isCreateCrossSection = (bool)chbCreateCrossSection.IsChecked;
            Double.TryParse(tbStep.Text, out step);
            Double.TryParse(tbCoordX.Text, out CoordX);
            Double.TryParse(tbCoordY.Text, out CoordY);
            Double.TryParse(tbCoordZ.Text, out CoordZ);
            Int32.TryParse(tbNumbStartNode.Text, out NumbStartNode);
            Int32.TryParse(tbNumbStartElement.Text, out NumbStartElement);            

            var curbore = ListBoreholes[numCurrentBorehol];
            switch (typeCrossSectionEnum)
            {
                case TypeCrossSectionEnum.Круглое:
                    pile = new PileRound(sidePileX, levelTopPile, lengthPile, ListBoreholes[numCurrentBorehol], levelOfLocalErosion);
                    break;
                case TypeCrossSectionEnum.Прямоугольное:
                    pile = new PileRectangular(sidePileX, sidePileY, levelTopPile, lengthPile, ListBoreholes[numCurrentBorehol], levelOfLocalErosion);
                    break;
                default:
                    break;
            }
            var ww = pile.Borehole;
            bool isLayerForPile = ListBoreholes[numCurrentBorehol].LevelTop > levelTopPile - lengthPile;
            bool isLayerBelowPile = ListBoreholes[numCurrentBorehol].LevelBot < levelTopPile - lengthPile;

            if (isLayerForPile && isLayerBelowPile) //Ниже сваи есть грунты
            {
                pile.CoordinateTopX = CoordX;
                pile.CoordinateTopY = CoordY;
                pile.CoordinateTopZ = CoordZ;

                var a = pile.LayerSoilsBelowGrillage;
                var a1 = pile.LayerSoilsAtPileLevel;

                PileAnalyticalScheme pileAnalyticalScheme = new PileAnalyticalScheme(pile, step, NumbStartNode, NumbStartElement);
                var temp = pileAnalyticalScheme.SpringStiffnesHoriz;

                docForMidas = new DocForMidas(pileAnalyticalScheme, materialEnum, sidePileX, sidePileY, NumbStartNode);                
                tbCommand.Text = docForMidas.WriteDoc();
                tbNumbStartNode.Text = (NumbStartNode + docForMidas.PileAnalyticalScheme.Nodes.Count).ToString();    //////////////////////
                tbNumbStartElement.Text = (NumbStartElement + docForMidas.PileAnalyticalScheme.MidasBeamElements.Count).ToString();    //////////////////////
            }
            else
            {
                tbCommand.Text = "Проверьте отметки сваи и грунтов";
            }
        }        

        private void cbTypeCrossSection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (labDim1 != null && labDim2 != null && tbDim1 != null && tbDim2 != null)
            {
                var a = cbTypeCrossSection.SelectedIndex;
                if (cbTypeCrossSection.SelectedIndex == 0) //"Круглое"
                {
                    var b = this.labDim1;
                    var c = labDim1;
                    this.labDim1.Content = "Диаметр";
                    tbDim1.Text = "0.8";
                    labDim2.Content = "";
                    this.tbDim2.IsEnabled = false;
                    this.labDim2.IsEnabled = false;
                    tbDim2.Visibility = Visibility.Hidden;
                    labDim2.Visibility = Visibility.Hidden;
                }
                if (cbTypeCrossSection.SelectedIndex == 1) //"Прямоугольное"
                {
                    labDim1.Content = "Размер Х";
                    labDim2.Content = "Размер Y";
                    tbDim1.Text = "0.35";
                    tbDim2.Text = "0.35";
                    tbDim2.IsEnabled = true;
                    labDim2.IsEnabled = true;
                    tbDim2.Visibility = Visibility.Visible;
                    labDim2.Visibility = Visibility.Visible;
                }
            }

        }

        private void bCopyCodeForMidas_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.Clear();
            Clipboard.SetText(tbCommand.Text);
        }        

        //Событие при изменении данных в DGTableIGE
        private void DGTableIGE_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (DGTableIGE.CurrentCell.Column != null)
            {
                if (prevDGIGEIndexColumn != DGTableIGE.CurrentCell.Column.DisplayIndex || prevDGIGESelectGeologocalElement != DGTableIGE.CurrentCell.Item)
                {
                    //calcDataGrid();
                }

                prevDGIGEIndexColumn = DGTableIGE.CurrentCell.Column.DisplayIndex;
                prevDGIGESelectGeologocalElement = DGTableIGE.CurrentCell.Item as GeologocalElement;
                canDeleteItemIGE = true;
            }

            ListIGE.Clear();
            foreach (var item in ListGeoElements)
            {
                ListIGE.Add(item.Number + " " + item.NameIGE);
            }

            DGCurrentBorehole.Items.Refresh();            
        }

        private void DGBoreholes_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {           
            if (ListBoreholes.Count > 0 && DGBoreholes.CurrentItem!=null)
            {
                currentBorehol = DGBoreholes.CurrentItem as Borehole;
                numCurrentBorehol = currentBorehol.Number-1;
                
                if (currentBorehol != null && numCurrentBorehol < ListBoreholes.Count)
                {
                    tbNameBorehole.Text = currentBorehol.NumberName;
                    DGCurrentBorehole.ItemsSource = ListBoreholes[numCurrentBorehol].LayerSoils;                    
                }
                else
                {
                    tbNameBorehole.Text = "";
                    numCurrentBorehol = -1;
                }                    
            }            
            
            canDeleteItemBoreholes = true;
            calcDataGrid(); 
        }

        private void DGCurrentBorehole_SelectionChanged(object sender, SelectionChangedEventArgs e)///////////////////////////////////////
        {
            canDeleteItemCurrentBoreholes = true;
            int num = 1;
            if (ListBoreholes.Count != 0)
            {
                foreach (var layerSoil in ListBoreholes[numCurrentBorehol].LayerSoils)
                {
                    layerSoil.Number = num;
                    num++;
                }
                //добавление к текущей скважене слоев грунта
                currentBorehol = ListBoreholes[numCurrentBorehol];

                double levelBotPrevLayer = 0;
                for (int i = 0; i < ListBoreholes[numCurrentBorehol].LayerSoils.Count; i++)
                {
                    if (i > 0)
                    {
                        ListBoreholes[numCurrentBorehol].LayerSoils[i].LevelTop = levelBotPrevLayer;
                    }
                    levelBotPrevLayer = ListBoreholes[numCurrentBorehol].LayerSoils[i].LevelBot;
                }
            }  
        }

        private void cbAddIGE_Click(object sender, RoutedEventArgs e)
        {
            ListGeoElements.Add(new GeologocalElement("ИГЭ", GroundEnum.Песок_крупный, 0, 0));
            DGTableIGE_SelectedCellsChanged(this, null);
            calcDataGrid();
        }

        private void cbAddBoreholes_Click(object sender, RoutedEventArgs e)
        {
            ListBoreholes.Add(new Borehole("Скв"));
            calcDataGrid();
        }

        private void cbAddLayerInBore_Click(object sender, RoutedEventArgs e)
        {
            ListBoreholes[numCurrentBorehol].AddLayerSoil(new LayerSoil(0,"слой1" ,new GeologocalElement("123", GroundEnum.Глина, 0, 0), 0, 0));            
            DGCurrentBorehole.Items.Refresh(); ;
            calcDataGrid();
        }

        private void cbApplyIGE_Click(object sender, RoutedEventArgs e)
        {
            GetTB();
            if (ListGeoElements.Count < numbIGE)
            {
                for (int i = ListGeoElements.Count; i < numbIGE; i++)
                {
                    ListGeoElements.Add(new GeologocalElement("", GroundEnum.Песок_крупный, 0, 0));
                }
            }
            else
            {
                for (int i = ListGeoElements.Count; i > numbIGE; i--)
                {
                    ListGeoElements.RemoveAt(ListGeoElements.Count - 1);
                }
            }
            calcDataGrid();
        }

        private void cbApplyLayer_Click(object sender, RoutedEventArgs e)
        {
            GetTB();
            if (ListBoreholes[numCurrentBorehol].LayerSoils.Count < numbIGEinBore)
            {
                for (int i = ListBoreholes[numCurrentBorehol].LayerSoils.Count; i < numbIGEinBore; i++)
                {
                    
                    ListBoreholes[numCurrentBorehol].LayerSoils.Add(new LayerSoil("123"));
                }
            }
            else
            {
                for (int i = ListBoreholes[numCurrentBorehol].LayerSoils.Count; i > numbIGEinBore; i--)
                {                    
                    ListBoreholes[numCurrentBorehol].LayerSoils.RemoveAt(ListBoreholes[numCurrentBorehol].LayerSoils.Count - 1);
                }
            }
            calcDataGrid();
        }

        private void cbDelIGE_Click(object sender, RoutedEventArgs e)
        {
            if (canDeleteItemIGE)
            {
                ListGeoElements.Remove(prevDGIGESelectGeologocalElement);
                calcDataGrid();
                canDeleteItemIGE = false;
            }
            else if(ListGeoElements.Count > 0)
            {                
                ListGeoElements.RemoveAt(ListGeoElements.Count - 1);
                calcDataGrid();
            }
        }

        private void cbDelBoreholes_Click(object sender, RoutedEventArgs e)
        {
            if (ListBoreholes.Count > 1)
            {
                if (canDeleteItemBoreholes)
                {
                    int numSelect = DGBoreholes.SelectedItems.Count;

                    for (int i = 0; i < numSelect; i++)
                    {
                        ListBoreholes.Remove(DGBoreholes.SelectedItems[i] as Borehole);
                    }

                    if (numSelect == 0 && ListBoreholes.Count > 0)
                        ListBoreholes.RemoveAt(ListBoreholes.Count - 1);

                    canDeleteItemBoreholes = false;
                }
                else if (ListBoreholes.Count > 0)
                {
                    ListBoreholes.RemoveAt(ListBoreholes.Count - 1);
                }

                if (numCurrentBorehol >= ListBoreholes.Count)
                    numCurrentBorehol = 0;
                calcDataGrid();
            }            
        }

        private void cbDelCurrentBore_Click(object sender, RoutedEventArgs e)
        {
            if (canDeleteItemCurrentBoreholes)
            {
                int numSelect = DGCurrentBorehole.SelectedItems.Count;

                for (int i = 0; i < numSelect; i++)
                {                    
                    ListBoreholes[numCurrentBorehol].RemoveLayerSoil(DGCurrentBorehole.SelectedItems[i] as LayerSoil);
                }
                canDeleteItemCurrentBoreholes = false;
            }
            else if (ListBoreholes[numCurrentBorehol].LayerSoils.Count > 0)
            {
                ListBoreholes[numCurrentBorehol].LayerSoils.RemoveAt(ListBoreholes[numCurrentBorehol].LayerSoils.Count - 1);
            }
            calcDataGrid();
        }

        private void DataGrid_GotFocus(object sender, RoutedEventArgs e)
        {
            // Lookup for the source to be DataGridCell
            if (e.OriginalSource.GetType() == typeof(DataGridCell))
            {
                // Starts the Edit on the row;
                DataGrid grd = (DataGrid)sender;
                grd.BeginEdit(e);

            }
        }

        private void DGCurrentBorehole_CurrentCellChanged(object sender, EventArgs e)
        {
            var v = DGCurrentBorehole.SelectedCells;
        }
        

        private void bCreateCodeForMidas_Click(object sender, RoutedEventArgs e)
        {
            MainMethod();
            Clipboard.Clear();
            Clipboard.SetText(tbCommand.Text);
        }

        private void timerTick(object sender, EventArgs e)
        {
        }


    }
}
