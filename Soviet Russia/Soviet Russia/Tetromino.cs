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


        public int leftBound;
        public int bottomBound;
        public int rightBound;
        public List<Rectangle> squares;

        public static List<Tuple<List<int[]>, Brush>> POSSIBLE_TETROMINOS = new List<Tuple<List<int[]>, Brush>>();

        internal static void GenerateShapes(List<Tuple<List<int[]>, Brush>> list) {
            POSSIBLE_TETROMINOS = list;
        }

        public Tetromino(Grid grid, int column) {

            squares = new List<Rectangle>();

            Random r = new Random();
            int chosenShape = r.Next(0, 3);
            Brush color = POSSIBLE_TETROMINOS[chosenShape].Item2;

            int topmost = POSSIBLE_TETROMINOS.Min(tuple => tuple.Item1.Min(cord => cord[0]));
            int leftmost = POSSIBLE_TETROMINOS.Min(tuple => tuple.Item1.Min(cord => cord[1]));

            foreach (int[] coordinates in POSSIBLE_TETROMINOS[chosenShape].Item1) {

                Rectangle square = new Rectangle();
                square.Fill = color;
                square.HorizontalAlignment = HorizontalAlignment.Stretch;
                square.VerticalAlignment = VerticalAlignment.Stretch;

                squares.Add(square);

                grid.Children.Add(square);
                Grid.SetRow(square, coordinates[0] - topmost);
                Grid.SetColumn(square, coordinates[1] - leftmost  + column);
            }

            leftBound = squares.Min(sq => Grid.GetColumn(sq));
            rightBound = squares.Max(sq => Grid.GetColumn(sq));
            bottomBound = squares.Max(sq => Grid.GetRow(sq));

        }
    }
}
