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
        int numbIGE;
        List<GeologocalElement> ListGeologocalElements = new List<GeologocalElement>();
        List<Borehole> ListBoreholes = new List<Borehole>();
        DocForMidas docForMidas;


        public MainWindow()
        {
            InitializeComponent();
            Initialization();
            GetTB();


            Debug.Print("Начало");
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
            
            PileRectangular pile1 = new PileRectangular(0.3,0.8, 85, 10.2, borehole1, 80);

            var a = pile1.LayerSoilsBelowGrillage;
            var a1 = pile1.LayerSoilsAtPileLevel;

            PileAnalyticalScheme pileAnalyticalScheme = new PileAnalyticalScheme(pile1, 1);
            var temp = pileAnalyticalScheme.SpringStiffnesHoriz;

            docForMidas = new DocForMidas(pileAnalyticalScheme, MaterialEnum.B25, pile1.SideX, pile1.SideY);

            tbCommand.Text = docForMidas.WriteDoc();
            int i = 1;
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 1);            
            timer.Start();
            

        }

        private void timerTick(object sender, EventArgs e)
        {
            //calcDataGrid();
            //DGTableIGE.Items.Refresh();
        }

        //Установка значений по умолчанию и связывание
        private void Initialization()
        {
            DGTableIGE.ItemsSource = ListGeologocalElements;
            DGBoreholes.ItemsSource = ListBoreholes;
        }

        //Считать данные с элементов управления
        private void GetTB()
        {
            int.TryParse(tbNumbIGE.Text, out numbIGE);
        }

        private void cbTypeCrossSection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (labDim1 !=null && labDim2!=null && tbDim1 != null && tbDim2 != null)
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

        private void bCreateCodeForMidas_Click(object sender, RoutedEventArgs e)
        {
            docForMidas.WriteDoc();
        }
        private void bCopyCodeForMidas_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.Clear();
            Clipboard.SetText(tbCommand.Text);
        }

        

        private void calcDataGrid()
        {
            int num = 1;
            foreach (var geoElement in ListGeologocalElements)
            {
                geoElement.Number = num;
                num++;
            }

            this.DGTableIGE.CommitEdit();
            this.DGTableIGE.CommitEdit();
            DGTableIGE.Items.Refresh();


            DGBoreholes.Items.Refresh();
            
        }

        private void cbAdd_Click(object sender, RoutedEventArgs e)
        {
            ListGeologocalElements.Add(new GeologocalElement("ИГЭ", GroundEnum.Песок_крупный, 0, 0));            
            calcDataGrid();
        }

        private void cbApply_Click(object sender, RoutedEventArgs e)
        {    
            if (ListGeologocalElements.Count < numbIGE)
            {
                for (int i = ListGeologocalElements.Count; i < numbIGE; i++)
                {
                    ListGeologocalElements.Add(new GeologocalElement("", GroundEnum.Песок_крупный, 0, 0));
                }                
            }
            calcDataGrid();
        }

        private void cbDel_Click(object sender, RoutedEventArgs e)
        {            
            int numSelect = DGTableIGE.SelectedItems.Count;
            var a = DGTableIGE.SelectedItems as GeologocalElement;

            for (int i = 0; i < numSelect; i++)
            {
                ListGeologocalElements.Remove(DGTableIGE.SelectedItems[i] as GeologocalElement);
            }            
            calcDataGrid();
        }

        DataGridCellInfo prevCell=new DataGridCellInfo();
        DataGridCellInfo b;
        int t=-1;
        private void DGTableIGE_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {

            if (t != DGTableIGE.CurrentCell.Column.DisplayIndex)
            {
                calcDataGrid();
                //MessageBox.Show("Одинаковые");
            }
            //a = DGTableIGE.SelectedCells;
            //a = DGTableIGE.CurrentCell;
            var c = (sender as DataGrid).SelectedIndex;
            var d = (sender as DataGrid).SelectedCells;
            
            //DGTableIGE.;
            //DGTableIGE.CurrentCell = b;
            //DataGrid
            //var b = (sender as DataGrid).;
            prevCell = DGTableIGE.CurrentCell;
            t = DGTableIGE.CurrentCell.Column.DisplayIndex;
        }

        private void cbAdBore_Click(object sender, RoutedEventArgs e)
        {
            //ListBorehole.Add(new Borehole())
            //ListBo.Add(new GeologocalElement("ИГЭ", GroundEnum.Песок_крупный, 0, 0));
            //DGTableIGE.ItemsSource = ListGeologocalElements;
            //calcDataGrid();
        }

        private void cbAdBoreholes_Click(object sender, RoutedEventArgs e)
        {            
            ListBoreholes.Add(new Borehole("Скв"));            
            calcDataGrid();
        }

        private void cbDelBoreholes_Click(object sender, RoutedEventArgs e)
        {            
            int numSelect = DGBoreholes.SelectedItems.Count;            

            for (int i = 0; i < numSelect; i++)
            {
                ListBoreholes.Remove(DGBoreholes.SelectedItems[i] as Borehole);
            }
            calcDataGrid();
        }

        private void DGTableIGE_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void DGTableIGE_KeyUp(object sender, KeyEventArgs e)
        {
            //calcDataGrid();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            calcDataGrid();
            DGTableIGE.Items.Refresh();
            DGBoreholes.Items.Refresh();
        }

        private void DGTableIGE_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            //DGTableIGE.Acc
            //this.DGTableIGE.CommitEdit();
            //this.DGTableIGE.CommitEdit();
            //DGTableIGE.Items.Refresh();
            //try
            //{
            //    DGTableIGE.Items.Refresh();
            //    DGBoreholes.Items.Refresh();
            //}
            //catch (Exception)
            //{


            //}

            //WPFdataGrid.CancelEdit(DataGridEditingUnit.Row);


        }
    }
}
