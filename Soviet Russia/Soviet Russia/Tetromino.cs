using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Soviet_Russia {
    class Tetromino {

        int leftBound;
        int bottomBound;
        int rightBound;
        List<Rectangle> squares;

        static List<Tuple<List<int[]>, Brush>> POSSIBLE_TETROMINOS = new List<Tuple<List<int[]>, Brush>>();

        internal static void GenerateShapes(List<Tuple<List<int[]>, Brush>> list) {
            MessageBox.Show(list.ToString());
            POSSIBLE_TETROMINOS = list;
        }

        public Tetromino(int column) {

            Random r = new Random();
            int chosenShape = r.Next(0, 3);
            Brush color = POSSIBLE_TETROMINOS[chosenShape].Item2;

            int topmost = 0;
            foreach (int[] coordinates in POSSIBLE_TETROMINOS[chosenShape].Item1)
                topmost = Math.Max(topmost, coordinates[1]);

            foreach (int[] coordinates in POSSIBLE_TETROMINOS[chosenShape].Item1) {
                Rectangle square = new Rectangle();
                square.Fill = color;
                square.HorizontalAlignment = HorizontalAlignment.Stretch;
                square.VerticalAlignment = VerticalAlignment.Stretch;

                Grid.SetRow(square, coordinates[1] - topmost);
                Grid.SetColumn(square, coordinates[0] + column);
            }

        }
    }
}
