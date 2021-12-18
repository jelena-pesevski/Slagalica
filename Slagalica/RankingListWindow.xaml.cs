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
using System.IO;

namespace Slagalica
{
    /// <summary>
    /// Interaction logic for RankingListWindow.xaml
    /// </summary>
    public partial class RankingListWindow : Window
    {
        List<Player> items = new List<Player>();
        public RankingListWindow()
        {
            InitializeComponent();
            ReadLines();
            lvPlayers.ItemsSource = items;
        }


        private void ReadLines()
        {
            var projectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string scoreFilePath = projectPath + "/resources/score.txt";

            foreach(string line in System.IO.File.ReadLines(scoreFilePath))
            {
                string[] parts = line.Split(new[] { '#' });
                Player p = new Player(parts[0], Int32.Parse(parts[1]));
                items.Add(p);
            }

            items.Sort((delegate (Player x, Player y)
            {
                return y.Points.CompareTo(x.Points);
            }));
        }
    }
}
