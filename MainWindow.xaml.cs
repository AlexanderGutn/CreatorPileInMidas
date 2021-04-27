﻿using System;
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
        List<GeologocalElement> ListGeoElements = new List<GeologocalElement>();
        GeologocalElement prevDGIGESelectGeologocalElement;
        int prevDGIGEIndexColumn = -1;

        Borehole currentBorehol;
        

        List<Borehole> ListBoreholes = new List<Borehole>();

        
        bool canDeleteItemIGE = false;
        bool canDeleteItemBoreholes = false;
        bool canDeleteItemCurrentBoreholes = false;

        List<LayerSoil> ListlayerSoilsSelectBorehole = new List<LayerSoil>();        

        DocForMidas docForMidas;

        int numbIGEinBore;

        List<string> ListIGE = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
            Initialization();
            //GetTB();
           

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

        private void timerTick(object sender, EventArgs e)
        {
            //calcDataGrid();
            //DGTableIGE.Items.Refresh();
        }

        //Установка значений по умолчанию и связывание
        private void Initialization()
        {
            DGWinForm dGWinForm = new DGWinForm();

            DGTableIGE.ItemsSource = ListGeoElements;
            DGBoreholes.ItemsSource = ListBoreholes;
            ListBoreholes.Add(new Borehole("Скв1"));


            currentBorehol = ListBoreholes[0];
            //DGCurrentBorehole.ItemsSource = currentBorehol.LayerSoils;
            DGCurrentBorehole.ItemsSource = ListlayerSoilsSelectBorehole;
            ListlayerSoilsSelectBorehole.Clear();
              
            DGCurrentBorehole.CanUserAddRows = false;

            //Привязка списка доступных значений для ComboBox
            (DGCurrentBorehole.Columns[1] as DataGridComboBoxColumn).ItemsSource = ListIGE;
        }

        


        //Считать данные с элементов управления
        private void GetTB()
        {
            int.TryParse(tbNumbIGE.Text, out numbIGE);
            numbIGE = numbIGE > 0 ? numbIGE : 0;

            int.TryParse(tbNumbIGEInBore.Text, out numbIGEinBore);
            numbIGEinBore = numbIGEinBore > 0 ? numbIGEinBore : 0;

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
            //foreach (var layerSoil in layerSoilsSelectBorehole)
            foreach (var layerSoil in ListlayerSoilsSelectBorehole) //currentBorehol.LayerSoils
            {
                layerSoil.Number = num;
                num++;
                var n = layerSoil.Name;

                
                //{
                //    int iIGE = ListGeoElements.Where(x => x.NumberNameIGE == layerSoil.Name).ToList()[0].Number;
                //    layerSoil.GeologocalElement = new GeologocalElement(ListGeoElements[iIGE].NameIGE, 
                //                            ListGeoElements[iIGE].GroundEnum, ListGeoElements[iIGE].e, ListGeoElements[iIGE].KUser);
                //}
                


                //layerSoil.Name
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
            //select = DGBoreholes.SelectedItem;
            if (ListBoreholes.Count > 0)
            {
                currentBorehol = DGBoreholes.CurrentItem as Borehole;
                if (currentBorehol != null)
                    tbNameBorehole.Text = currentBorehol.Number + " " + currentBorehol.Name;
                else
                    tbNameBorehole.Text = "";
            }            
            //DGBoreholes.SelectedItem = select;
            canDeleteItemBoreholes = true;

            //ListlayerSoilsSelectBorehole = ListBoreholes.Where(x => x.Number == currentBorehol.Number).ToList()[0].LayerSoils;


        }

        private void DGCurrentBorehole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            canDeleteItemCurrentBoreholes = true;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            currentBorehol.ClearLayerSoil();
            currentBorehol.AddLayerSoils(ListlayerSoilsSelectBorehole);

            ListBoreholes.Where(x => x.Number == currentBorehol.Number).ToList()[0].AddLayerSoils(ListlayerSoilsSelectBorehole); 

        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {


            int i = 1;
        }

        private void cbAddIGE_Click(object sender, RoutedEventArgs e)
        {
            ListGeoElements.Add(new GeologocalElement("ИГЭ", GroundEnum.Песок_крупный, 0, 0));
            DGTableIGE_SelectedCellsChanged(this, null);
            calcDataGrid();
        }

        private void cbAdBoreholes_Click(object sender, RoutedEventArgs e)
        {
            ListBoreholes.Add(new Borehole("Скв"));
            calcDataGrid();
        }

        private void cbAdLayerInBore_Click(object sender, RoutedEventArgs e)
        {
            //ListlayerSoilsSelectBorehole.Add(new LayerSoil(0, new GeologocalElement("123", GroundEnum.Глина, 0, 0), 0, 0));
            ListlayerSoilsSelectBorehole.Add(new LayerSoil("new"));

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
            if (ListlayerSoilsSelectBorehole.Count < numbIGEinBore)
            {
                for (int i = ListlayerSoilsSelectBorehole.Count; i < numbIGEinBore; i++)
                {
                    //ListlayerSoilsSelectBorehole.Add(new LayerSoil(0, new GeologocalElement("123", GroundEnum.Глина, 0, 0), 0, 0));
                    ListlayerSoilsSelectBorehole.Add(new LayerSoil("123"));

                }
            }
            else
            {
                for (int i = ListlayerSoilsSelectBorehole.Count; i > numbIGEinBore; i--)
                {
                    ListlayerSoilsSelectBorehole.RemoveAt(ListlayerSoilsSelectBorehole.Count - 1);
                }
            }
            calcDataGrid();
        }

        private void cbDelIGE_Click(object sender, RoutedEventArgs e)
        {
            //Для случая выбора SelectionUnit="FullRow"
            //int numSelect = DGTableIGE.SelectedItems.Count;
            //var a = DGTableIGE.SelectedItems as GeologocalElement;

            //for (int i = 0; i < numSelect; i++)
            //{
            //    ListGeologocalElements.Remove(DGTableIGE.SelectedItems[i] as GeologocalElement);
            //} 

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
            if (canDeleteItemBoreholes)
            {
                int numSelect = DGBoreholes.SelectedItems.Count;

                for (int i = 0; i < numSelect; i++)
                {
                    ListBoreholes.Remove(DGBoreholes.SelectedItems[i] as Borehole);
                }
                canDeleteItemBoreholes = false;                
            }
            else if (ListBoreholes.Count > 0)
            {
                ListBoreholes.RemoveAt(ListBoreholes.Count - 1);
            }
            calcDataGrid();
        }

        private void cbDelCurrentBore_Click(object sender, RoutedEventArgs e)
        {
            if (canDeleteItemCurrentBoreholes)
            {
                int numSelect = DGCurrentBorehole.SelectedItems.Count;

                for (int i = 0; i < numSelect; i++)
                {
                    ListlayerSoilsSelectBorehole.Remove(DGCurrentBorehole.SelectedItems[i] as LayerSoil);
                }
                canDeleteItemCurrentBoreholes = false;
            }
            else if (ListlayerSoilsSelectBorehole.Count > 0)
            {
                ListlayerSoilsSelectBorehole.RemoveAt(ListlayerSoilsSelectBorehole.Count - 1);
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
            DGCurrentBorehole.Items.Refresh();
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


    }
}
