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
using System.Windows.Shapes;

namespace Slagalica
{
    /// <summary>
    /// Interaction logic for GameOverWindow.xaml
    /// </summary>
    public partial class GameOverWindow : Window
    {
        private const string WIN_MSG = "Congratulations, you won! Remained time:";
        private const string LOSE_MSG = "Sorry, you lost!";
        private const string FASTEST_GAME = "Fastest game:";
        private const string PATH = "pack://application:,,,/resources/";

        public GameOverWindow()
        {
            InitializeComponent();
        }

        public GameOverWindow(Boolean isWin, int remainedTime, List<string> combination) 
        {
            InitializeComponent();
            if (isWin)
            {
                messageLbl.Content = WIN_MSG + remainedTime;
            }
            else
            {
                messageLbl.Content = LOSE_MSG;
            }

            for (int i = 0; i < 4; i++)
            {
                Image img = new Image();
                img.Source = new BitmapImage(new Uri(PATH+combination[i]));
                img.Margin = new Thickness(5, 5, 5, 5);

                Grid.SetColumn(img, i);
                Grid.SetRow(img, 0);
                combinationGrid.Children.Add(img);
            }
        }

        private void Exit_Game(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void Replay_Game(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
