using Lab1.Model;
using Lab1.Model.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        int xSize = 3;
        int ySize = 4;

        const String ITERATION_PREFIX = "Количество итераций поиска: {0}";
        const String PATH_LENGTH_PREFIX = "Длина пути: {0}";

        public MainWindow()
        {
            InitializeComponent();
            InitField(aStarGameField);
            InitField(depthSearchGameField);
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
                    Ellipse cell = new Ellipse();
                    cell.Height = cell.Width = 44;
                    field.Children.Add(cell);
                    Grid.SetColumn(cell, i);
                    Grid.SetRow(cell, j);
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GamePreset gamePreset = new GamePreset(xSize, ySize);
            StartAllGames(gamePreset);
        }

        private void StartAllGames(GamePreset gamePreset)
        {
            Game aStarGame = new AStarGame(xSize, ySize, gamePreset);
            aStarGame.NextStateReady += aStarGame_NextStateReady;

            Game depthSearchGame = new DepthSearchGame(xSize, ySize, gamePreset);
            depthSearchGame.NextStateReady += depthSearchGame_NextStateReady;

            ShowState(gamePreset.Target, targetField.Children);
            ShowState(gamePreset.Initial, aStarGameField.Children);
            ShowState(gamePreset.Initial, depthSearchGameField.Children);

            Task<Stack<State>> aStarPath = Task.Run(() => aStarGame.Start());
            aStarPath.ContinueWith((path) =>
                Dispatcher.Invoke(() =>
                    aStarPathLength.Text = String.Format(PATH_LENGTH_PREFIX, path.Result.Count)
            ));

            Task<Stack<State>> depthPath = Task.Run(() => depthSearchGame.Start());
            depthPath.ContinueWith((path) =>
                Dispatcher.Invoke(() =>
                   depthSearchPathLength.Text = String.Format(PATH_LENGTH_PREFIX, path.Result.Count)
            ));
        }

        private void aStarGame_NextStateReady(object sender, NextStateEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                ShowState(e.State, aStarGameField.Children);
                aStarIterations.Text = String.Format(ITERATION_PREFIX, e.Iterations);
            })); 
        }

        private void depthSearchGame_NextStateReady(object sender, NextStateEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                ShowState(e.State, depthSearchGameField.Children);
                depthSearchIterations.Text = String.Format(ITERATION_PREFIX, e.Iterations);
            }));
        }

        private void ShowState(State state, UIElementCollection field)
        {
            bool[] rawStateAsList = state.Select(b => b).ToArray();

            for (int i = 0; i < rawStateAsList.Length; i++)
            {
                if (rawStateAsList[i])
                {
                    ((Shape)field[i]).Fill = Brushes.Red;
                }
                else
                {
                    ((Shape)field[i]).Fill = Brushes.Black;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StartAllGames(new GamePreset(xSize, ySize));
        }
    }


}
