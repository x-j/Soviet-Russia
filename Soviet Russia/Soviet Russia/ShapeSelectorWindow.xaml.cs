using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Soviet_Russia {
    /// <summary>
    /// Interaction logic for ShapeSelectorWindow.xaml
    /// </summary>
    public partial class ShapeSelectorWindow : Window, INotifyPropertyChanged {

        //Properties
        private bool _finishable;
        public bool Finishable {
            get {
                return _finishable;
            }
            set {
                _finishable = value;
                OnPropertyChanged("Finishable");
            }
        }

        private int _gridSize;
        public int GridSize {
            get {
                return _gridSize;
            }
            set {
                _gridSize = value;
                OnPropertyChanged("GridSize");
            }
        }

        private int _shapesMadeCount;
        public int ShapesMadeCount {
            get {
                return _shapesMadeCount;
            }
            private set {
                _shapesMadeCount = value;
                OnPropertyChanged("ShapesMadeCount");
            }
        }

        //Constants
        private const byte DEFAULT_GRID_SIZE = 5;

        List<Tuple<Grid, ComboBox, List<ToggleButton>>> controls;

        public ShapeSelectorWindow() {
            InitializeComponent();

            DataContext = this;

            GridSize = DEFAULT_GRID_SIZE;

            PopulateLists();

            MakeButtons();

        }

        private void PopulateLists() {

            controls = new List<Tuple<Grid, ComboBox, List<ToggleButton>>>();
            controls.Add(new Tuple<Grid, ComboBox, List<ToggleButton>>(nwGrid, nwComboBox, new List<ToggleButton>()));
            controls.Add(new Tuple<Grid, ComboBox, List<ToggleButton>>(neGrid, neComboBox, new List<ToggleButton>()));
            controls.Add(new Tuple<Grid, ComboBox, List<ToggleButton>>(seGrid, seComboBox, new List<ToggleButton>()));
            controls.Add(new Tuple<Grid, ComboBox, List<ToggleButton>>(swGrid, swComboBox, new List<ToggleButton>()));

        }

        private void MakeButtons() {

            foreach (var tuple in controls) {
                for (int i = 0; i < GridSize; i++) {
                    for (int j = 0; j < GridSize; j++) {
                        ToggleButton b = new ToggleButton();
                        b.HorizontalAlignment = HorizontalAlignment.Stretch;
                        b.VerticalAlignment = VerticalAlignment.Stretch;
                        int[] coordinates = { i, j };
                        b.Tag = coordinates;
                        b.Background = null;
                        /*b.Style = (Style)FindResource("toggleButtonStyle");
                        
                        Binding buttonColorBinding = new Binding();
                        buttonColorBinding.Source = tuple.Item2;
                        buttonColorBinding.Path = new PropertyPath("SelectedItem");
                        buttonColorBinding.ConverterParameter = b.IsChecked;
                        buttonColorBinding.Converter = new SelectedItemToBrushConverter();

                        BindingOperations.SetBinding(b, ToggleButton.BackgroundProperty, buttonColorBinding);*/

                        Grid g = tuple.Item1;
                        g.Children.Add(b);
                        Grid.SetRow(b, i);
                        Grid.SetColumn(b, j);

                        tuple.Item3.Add(b);
                    }
                }
            }

        }

        private int CountShapesMade() {

            int counter = 0;

            foreach (var tuple in controls) {

                if (tuple.Item2.SelectedIndex == -1) break;
                else {
                    foreach (var b in tuple.Item3) {
                        if ((bool)b.IsChecked) {
                            counter++;
                            break;
                        }
                    }
                }
            }

            ShapesMadeCount = counter;
            return counter;

        }

        public List<Tuple<List<int[]>, Brush>> ExportSelection() {

            List<Tuple<List<int[]>, Brush>> list = new List<Tuple<List<int[]>, Brush>>();

            foreach (var tuple in controls) {

                List<int[]> coordinates = new List<int[]>();

                foreach (var button in tuple.Item3) {
                    if ((bool)button.IsChecked)
                        coordinates.Add(button.Tag as int[]);
                }

                Brush brush = (Brush)new BrushConverter().ConvertFromString(tuple.Item2.SelectedItem.ToString().Split(' ')[1]);

                list.Add(new Tuple<List<int[]>, Brush>(coordinates, brush));

            }

            return list;

        }

        private void getOnWithItButton_Click(object sender, RoutedEventArgs e) {
            if (CountShapesMade() != 4) MessageBox.Show("Comrade, please make four shapes first!");
            else Visibility = Visibility.Hidden;
        }

        #region Property change handling
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion

    }

}
