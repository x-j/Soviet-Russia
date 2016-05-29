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
using System.Windows.Threading;

namespace Soviet_Russia {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        //somewhere in here is ssw

        DispatcherTimer timer;

        Tetromino currentTetro;

        public MainWindow() {
            InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += Timer_Tick;


        }

        private void Timer_Tick(object sender, EventArgs e) {
           // currentTetro.Down();
        }

        private void SetLocation(Rectangle r, int i, int j) {
            Grid.SetColumn(r, j);
            Grid.SetRow(r, i);
        }

        private void quitButton_Click(object sender, RoutedEventArgs e) {
            Close();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e) {

            //show the ssw

        }
    }
}
