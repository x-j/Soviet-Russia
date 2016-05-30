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
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window {

        private const int COLUMN_COUNT = 15;

        DispatcherTimer timer;

        Tetromino currentTetro;
        List<Rectangle> squares;

        public GameWindow() {
            InitializeComponent();

            this.Closed += GameWindow_Closed;

            squares = new List<Rectangle>();

            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            timer.Tick += Timer_Tick;

            Random rand = new Random();

            currentTetro = new Tetromino(gameGrid, rand.Next(0, COLUMN_COUNT - 1));

            timer.Start();



        }

        private void GameWindow_Closed(object sender, EventArgs e) {

            Environment.Exit(0);

        }

        private void Timer_Tick(object sender, EventArgs e) {

            MoveDown();

        }

        private void MoveDown() {

            if (!CheckCollisionDown()) {

                foreach (var sq in currentTetro.squares) {
                    int i = Grid.GetRow(sq);
                    int j = Grid.GetColumn(sq);
                    SetLocation(sq, i + 1, j);
                }
                gameGrid.InvalidateArrange();
                currentTetro.bottomBound = currentTetro.squares.Max(r => Grid.GetRow(r));

            } else {
                timer.Stop();
                foreach (var sq in currentTetro.squares) squares.Add(sq);
                Random rand = new Random();

                Tetromino t = new Tetromino(gameGrid, rand.Next(0, ROW_COUNT - 4));
                currentTetro = t;

                ClearFilledRows();

                timer.Start();
            }
        }

        private void ClearFilledRows() {
            
            for (int i = ROW_COUNT - 1; i >= 0; i--) {

                List<Rectangle> row = squares.FindAll(rect => Grid.GetRow(rect) == i);
                if (row.Count == COLUMN_COUNT) {
                    foreach (var sq in row) {
                        squares.Remove(sq);
                        gameGrid.Children.Remove(sq);
                    }

                    for (int j = i - 1; j >= 0; j--) {
                        List<Rectangle> aboveRow = squares.FindAll(rect => Grid.GetRow(rect) == j);
                        foreach (var sq in aboveRow) {
                            SetLocation(sq, Grid.GetRow(sq) + 1, Grid.GetColumn(sq));
                        }
                    }

                }


            }

        }


        private bool CheckCollisionDown() {

            if (currentTetro.bottomBound == ROW_COUNT - 1) {
                //MessageBox.Show("bottom???");
                return true;

            }
            foreach (var sq in squares) {

                int row = Grid.GetRow(sq);
                int col = Grid.GetColumn(sq);

                foreach (var tet in currentTetro.squares) {

                    if (Grid.GetColumn(tet) == col && Grid.GetRow(tet) == row - 1) return true;

                }

            }

            return false;

        }

        private void SetLocation(Rectangle r, int i, int j) {
            Grid.SetColumn(r, j);
            Grid.SetRow(r, i);
        }
        private const int ROW_COUNT = COLUMN_COUNT + 3;
        private void quitButton_Click(object sender, RoutedEventArgs e) {
            timer.Stop();
            Close();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e) {
            timer.Stop();
            ShapeSelectorWindow ssw = new ShapeSelectorWindow(Tetromino.POSSIBLE_TETROMINOS);
            ssw.ShowDialog();
            ssw.Close();
            Tetromino.GenerateShapes(ssw.ExportSelection());
            timer.Start();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e) {

            switch (e.Key) {

                case Key.Left:
                    MoveLeft();
                    break;

                case Key.Right:
                    MoveRight();
                    break;

                case Key.Space:
                    MessageBox.Show("Oh well done you pressed the spacebar");
                    break;

                case Key.Down:
                    MoveDown();
                    break;
            }

        }

        private void MoveRight() {
            if (!CheckCollisionRight()) {
                foreach (var sq in currentTetro.squares) {
                    int i = Grid.GetRow(sq);
                    int j = Grid.GetColumn(sq);
                    SetLocation(sq, i, j + 1);
                    gameGrid.InvalidateArrange();
                }
            }
            currentTetro.rightBound = currentTetro.squares.Max(r => Grid.GetColumn(r));
        }

        private bool CheckCollisionRight() {
            if (currentTetro.rightBound == COLUMN_COUNT - 1)
                return true;
            foreach (var sq in squares) {

                int row = Grid.GetRow(sq);
                int col = Grid.GetColumn(sq);

                foreach (var tet in currentTetro.squares) {

                    if (Grid.GetColumn(tet) + 1 == col && Grid.GetRow(tet) == row) return true;

                }

            }

            return false;
        }

        

        private void MoveLeft() {
            if (!CheckCollisionLeft()) {
                foreach (var sq in currentTetro.squares) {
                    int i = Grid.GetRow(sq);
                    int j = Grid.GetColumn(sq);
                    SetLocation(sq, i, j - 1);
                }
                gameGrid.InvalidateArrange();

            }
            currentTetro.leftBound = currentTetro.squares.Min(r => Grid.GetColumn(r));
        }

        private bool CheckCollisionLeft() {
            if (currentTetro.leftBound == 0)
                return true;
            foreach (var sq in squares) {

                int row = Grid.GetRow(sq);
                int col = Grid.GetColumn(sq);

                foreach (var tet in currentTetro.squares) {

                    if (Grid.GetColumn(tet) - 1 == col && Grid.GetRow(tet) == row) return true;

                }

            }

            return false;
        }

        

    }



    
}
