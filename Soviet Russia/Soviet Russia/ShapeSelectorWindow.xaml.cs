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
    public partial class ShapeSelectorWindow : Window {



        private int _shapesMadeCount;
        public int ShapesMadeCount {
            get {
                return _shapesMadeCount;
            }
            private set {
                _shapesMadeCount = value;
                //OnPropertyChanged("ShapesMadeCount");
            }
        }

        public byte GridSize { get; private set; }

        //Constants
        private const byte DEFAULT_GRID_SIZE = 5;   //BETTER NOT TOUCH THIS

        List<Tuple<Grid, ComboBox, List<Rectangle>>> controls;

        private readonly Brush SOVIET_RED;

        public ShapeSelectorWindow() {
            InitializeComponent();

            GridSize = DEFAULT_GRID_SIZE;
            SOVIET_RED = (Brush)FindResource("sovietRedBackground");

            PopulateLists();

            MakeRectangles();

            this.Closing += ShapeSelectorWindow_Closing;
            this.Closed += ShapeSelectorWindow_Closed;

        }

        private void ShapeSelectorWindow_Closed(object sender, EventArgs e) {
            Close();
        }

        private void ShapeSelectorWindow_Closing(object sender, CancelEventArgs e) {
            e.Cancel = false;
        }

        public ShapeSelectorWindow(List<Tuple<List<int[]>, Brush>> pOSSIBLE_TETROMINOS) {

            InitializeComponent();

            DataContext = this;

            GridSize = DEFAULT_GRID_SIZE;
            SOVIET_RED = (Brush)FindResource("sovietRedBackground");

            PopulateLists();
            MakeRectangles();

            if (pOSSIBLE_TETROMINOS.Count == 4) {

                for (int i = 0; i < 4; i++) {   //this is a comment
                    foreach (var coords in pOSSIBLE_TETROMINOS[i].Item1) {
                        Rectangle r = FindSquare(controls[i].Item3, coords);
                        if (pOSSIBLE_TETROMINOS[i].Item2 != null) {
                            r.Fill = pOSSIBLE_TETROMINOS[i].Item2;
                            r.Tag = true;
                        } else r.Fill = Brushes.White;
                    }
                    controls[i].Item2.SelectedIndex = findColorInCB(controls[i].Item2, pOSSIBLE_TETROMINOS[i].Item2);
                }
            }


        }

        private int findColorInCB(ComboBox item21, Brush item22) {

            for (int i = 0; i < item21.Items.Count; i++) {
                var item = item21.Items[i];
                Brush b = (Brush)new BrushConverter().ConvertFromString(item.ToString().Split(' ')[1]);

                if (b.Equals(item22)) return i;
            }

            return 0;
        }

        private Rectangle FindSquare(List<Rectangle> item3, int[] coords) {

            Rectangle found = null;

            foreach (var sq in item3) {
                int i = Grid.GetRow(sq);
                int j = Grid.GetColumn(sq);
                if (i == coords[0] && j == coords[1]) found = sq;
            }

            if (found == null) throw (new Exception("No matching square found?!"));
            return found;
        }

        private void PopulateLists() {

            controls = new List<Tuple<Grid, ComboBox, List<Rectangle>>>();
            controls.Add(new Tuple<Grid, ComboBox, List<Rectangle>>(nwGrid, nwComboBox, new List<Rectangle>()));
            nwComboBox.Tag = 0;
            controls.Add(new Tuple<Grid, ComboBox, List<Rectangle>>(neGrid, neComboBox, new List<Rectangle>()));
            neComboBox.Tag = 1;
            controls.Add(new Tuple<Grid, ComboBox, List<Rectangle>>(seGrid, seComboBox, new List<Rectangle>()));
            seComboBox.Tag = 2;
            controls.Add(new Tuple<Grid, ComboBox, List<Rectangle>>(swGrid, swComboBox, new List<Rectangle>()));
            swComboBox.Tag = 3;

        }

        private void MakeRectangles() {

            foreach (var tuple in controls) {
                for (int i = 0; i < GridSize; i++) {
                    for (int j = 0; j < GridSize; j++) {
                        Rectangle b = new Rectangle();
                        b.HorizontalAlignment = HorizontalAlignment.Stretch;
                        b.VerticalAlignment = VerticalAlignment.Stretch;
                        //int[] coordinates = { i, j };
                        b.Tag = false;
                        b.MouseLeftButtonDown += B_MouseLeftButtonDown;
                        b.Fill = SOVIET_RED;
                        b.ToolTip = tuple.Item2;    //i am very smart therefore i hid the respective combobox in the tooltip XDDD

                        tuple.Item1.Children.Add(b);
                        Grid.SetRow(b, i);
                        Grid.SetColumn(b, j);

                        tuple.Item3.Add(b);
                    }
                }
            }

        }

        private void B_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {

            Rectangle r = sender as Rectangle;
            if ((bool)r.Tag) {
                r.Tag = false;
                r.Fill = SOVIET_RED;
            } else {
                r.Tag = true;
                UpdateFill(r);
            }

        }

        private void comboBox_SelectionChanged(object sender, EventArgs a) {

            ComboBox cb = sender as ComboBox;
            int index = (int)cb.Tag;
            //if(controls[index].)
            foreach (var rect in controls[index].Item3) {
                if ((bool)rect.Tag) UpdateFill(rect);
            }
        }

        private void UpdateFill(Rectangle r) {

            ComboBox cb = r.ToolTip as ComboBox;
            if (cb.SelectedIndex > -1) {
                r.Fill = (Brush)new BrushConverter().ConvertFromString(cb.SelectedItem.ToString().Split(' ')[1]);
            } else r.Fill = Brushes.White;

        }

        private int CountShapesMade() {

            int counter = 0;

            foreach (var tuple in controls) {


                foreach (var b in tuple.Item3) {
                    if ((bool)b.Tag) {
                        counter++;
                        break;
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

                foreach (var rectangle in tuple.Item3) {
                    if ((bool)rectangle.Tag) {
                        int i = Grid.GetRow(rectangle);
                        int j = Grid.GetColumn(rectangle);
                        int[] temp = { i, j };
                        coordinates.Add(temp);
                    }
                }

                Brush brush;
                if (tuple.Item2.SelectedIndex > -1)
                    brush = (Brush)new BrushConverter().ConvertFromString(tuple.Item2.SelectedItem.ToString().Split(' ')[1]);
                else brush = Brushes.White;

                list.Add(new Tuple<List<int[]>, Brush>(coordinates, brush));

            }

            return list;

        }

        private void getOnWithItButton_Click(object sender, RoutedEventArgs e) {
            if (CountShapesMade() != 4) MessageBox.Show("Comrade, please make four shapes first!");
            else Close();
        }



    }

}