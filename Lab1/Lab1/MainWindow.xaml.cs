using Lab1.Model;
using Lab1.Model.Game;
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

namespace Lab1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int xSize = 4;
        int ySize = 4;

        public MainWindow()
        {
            InitializeComponent();
            InitField(gameField);
            InitField(targetField);
        }

        private void InitField(Grid field)
        {
            for (int i = 0; i < xSize; i++)
            {
                field.ColumnDefinitions.Add(new ColumnDefinition());

            }
            for (int j = 0; j < ySize; j++)
            {
                field.RowDefinitions.Add(new RowDefinition());
            }

            for (int i = 0; i < xSize; i++)
            {
                for (int j = 0; j < ySize; j++)
                {
                    Rectangle rectangle = new Rectangle();
                    rectangle.Height = rectangle.Width = 44;
                    field.Children.Add(rectangle);
                    Grid.SetColumn(rectangle, i);
                    Grid.SetRow(rectangle, j);
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Game game = new AStarGame(xSize, ySize);
            game.NextStateReady += Game_NextStateReady;

            ShowState(game.Current, gameField.Children);
            ShowState(game.Target, targetField.Children);

            Task.Run(() => game.Start());
        }

        private void Game_NextStateReady(object sender, NextStateEventArgs e)
        {
            Dispatcher.Invoke(() => ShowState(e.State, gameField.Children));
        }

        private void ShowState(State state, UIElementCollection field)
        {
            bool[] rawStateAsList = state.Select(b => b).ToArray();

            for (int i = 0; i < rawStateAsList.Length; i++)
            {
                if (rawStateAsList[i])
                {
                    ((Rectangle)field[i]).Fill = Brushes.Red;
                }
                else
                {
                    ((Rectangle)field[i]).Fill = Brushes.Black;
                }
            }
        }
    }


}
