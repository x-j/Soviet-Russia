using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

#region
/*
    According to research from Dr.Richard Haier, et al.prolonged Tetris activity can also lead to more efficient brain activity during play.
When first playing Tetris, brain function and activity increases, along with greater cerebral energy consumption, measured by glucose metabolic rate.
As Tetris players become more proficient, their brains show a reduced consumption of glucose, indicating more efficient brain activity for this task.
Even moderate playing of Tetris (half-an-hour a day for three months) boosts general cognitive functions such as "critical thinking, reasoning, language and processing" and increases cerebral cortex thickness.

    In January 2009, an Oxford University research group headed by Dr.Emily Holmes reported in PLoS ONE that for healthy volunteers, playing Tetris soon after viewing traumatic material in the laboratory reduced the number of flashbacks to those scenes in the following week.
They believe that the computer game may disrupt the memories that are retained of the sights and sounds witnessed at the time, and which are later re-experienced through involuntary, distressing flashbacks of that moment.
The group hopes to develop this approach further as a potential intervention to reduce the flashbacks experienced in posttraumatic stress disorder, but emphasized that these are only preliminary results.

    Professor Jackie Andrade and Jon May, from Plymouth University's Cognition Institute, and PhD student Jessica Skorka-Brown have conducted research that shows that playing Tetris could give a “quick and manageable" fix for people struggling to stick to diets, or quit smoking or drinking.

    Another notable effect is that, according to a Canadian study in April 2013, playing Tetris has been found to treat older adolescents with amblyopia (lazy eye), which was better than patching a victim's well eye to train his weaker eye. 
Dr. Robert Hess of the research team said "It's much better than patching – much more enjoyable; it's faster, and it seems to work better.". Tested in United Kingdom, this experiment also appears to help children with that problem.

The game has been noted to cause the brain to involuntarily picture Tetris combinations even when the player is not playing(the Tetris effect), although this can occur with any computer game or situation showcasing repeated images or scenarios, such as a jigsaw puzzle.
*/
#endregion <- interesting

namespace Soviet_Russia {
    /// <summary>
    /// Interaction logic for MainMenuWindow.xaml
    /// </summary>
    public partial class MainMenuWindow : Window, INotifyPropertyChanged {

        //Properties:
        private bool _startable;
        public bool Startable
        {
            get
            {
                return _startable;
            }
            set
            {
                _startable = value;
                OnPropertyChanged("Startable");
            }
        }

        ShapeSelectorWindow ssw;

        public MainMenuWindow() {

            InitializeComponent();

            ssw = new ShapeSelectorWindow(); 

            DataContext = this;

            Startable = false;

        }

        #region event handlers for buttons

        private void startButton_Click(object sender, RoutedEventArgs e) {
            GameWindow gw = new GameWindow();
            gw.Show();
            ssw.Close();
            Close();
        }

        private void shapesButton_Click(object sender, RoutedEventArgs e) {

            if (ssw == null) MessageBox.Show("????");
            this.RemoveVisualChild(ssw);
            ssw = null;
            if (Tetromino.POSSIBLE_TETROMINOS.Count == 4) {
                this.RemoveVisualChild(ssw);
                ssw = new ShapeSelectorWindow(Tetromino.POSSIBLE_TETROMINOS);
            } else ssw = new ShapeSelectorWindow();
            this.RemoveVisualChild(ssw);
            ssw.ShowDialog();            
            //here be dragons
            if (ssw.ShapesMadeCount == 4) {
                Startable = true;
                Tetromino.GenerateShapes(ssw.ExportSelection());
            }
        }

        private void exitButton_Click(object sender, RoutedEventArgs e) {
            Close();
            ssw.Close();
        }

        #endregion

        #region Property change handling
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion

    }
}
