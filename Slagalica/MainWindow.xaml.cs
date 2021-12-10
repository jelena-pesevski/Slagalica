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
using System.Collections;
using System.Diagnostics;
using System.Windows.Threading;

namespace Slagalica
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int NUM_OF_ELEMENTS = 6;
        private const int GAME_DURATION = 120;
        private const string PATH = "pack://application:,,,/resources/";
        private List<string> combination;
        private List<string> guess;
        private IDictionary<int, string> mappings;
        private int currentRow;
        private DispatcherTimer timer;
        private int remainedTime;

        public MainWindow()
        {
            InitializeComponent();
            PrepareGame();
        }
        private BitmapImage GetImage(int i)
        {
            switch (i)
            {
                case 0:
                    return new BitmapImage(new Uri(PATH+"smiley.png"));
                case 1:
                    return new BitmapImage(new Uri(PATH + "trefle.png"));
                case 2:
                    return new BitmapImage(new Uri(PATH + "heart.png"));
                case 3:
                    return new BitmapImage(new Uri(PATH + "spade.png"));
                case 4:
                    return new BitmapImage(new Uri(PATH + "diamond.png"));
                case 5:
                    return new BitmapImage(new Uri(PATH + "star.png"));
                default:
                    return null;
            }
        }


        private void InitDictionary()
        {
            mappings.Add(0, "smiley.png");
            mappings.Add(1, "trefle.png");
            mappings.Add(2, "heart.png");
            mappings.Add(3, "spade.png");
            mappings.Add(4, "diamond.png");
            mappings.Add(5, "star.png");
        }


        private void MakeCombination()
        {
            Random rand = new Random();
            //make combination
            for (int i = 0; i < 4; i++)
            {
                int index = rand.Next(0, NUM_OF_ELEMENTS);
                combination.Add(mappings[index]);
                Debug.WriteLine(mappings[index]);
            }
        }

        private void Check_Combination(object sender, EventArgs e)
        {
            int redCount = CountRightPlace();
           
            int yellowCount = CountElementsGuessed();
            yellowCount -= redCount;

            PaintGuessGrid(yellowCount,redCount);

            if (redCount == 4)
            {
                FinishGame(true);
                return;
            }

            DisableButtonsCurrentRow();
            currentRow++;
            guess.Clear();
            okBtn.IsEnabled = false;

            if (currentRow == 6)
            {
                FinishGame(false);
                return;
            }
        }

        private void FinishGame(Boolean isWin)
        {
            DisposeTimer();
            GameOverWindow gameOverWindow = new GameOverWindow(isWin, remainedTime, combination);
            gameOverWindow.Closed += (sender, e) => PrepareGame();
            gameOverWindow.Show();
            return;
        }

        private void Element_Click(object sender,EventArgs e)
        {
            int firstNullElement = -1;
            int currColumn = -1;
            for (int i = 0; i < guess.Count(); i++)
            {
                if (guess[i] == null)
                {
                    firstNullElement = i;
                    currColumn = i;
                    break;
                }
            }
            //if there is no Null and everything is full, ok is not yet pressed
            if (firstNullElement == -1 && guess.Count()==4)
            {
                return;
            }else if (firstNullElement==-1) //no null elements
            {
                currColumn = guess.Count();
            }

            Button btn = sender as Button;
            if (btn != null)
            {
                int col = Grid.GetColumn(btn);

                string iconName = mappings[col];

                Button cell =mainGrid.Children
                    .Cast<UIElement>()
                    .First(el => Grid.GetRow(el) == currentRow && Grid.GetColumn(el) == currColumn) as Button;
                if(cell!= null)
                {
                    StackPanel panel = cell.Content as StackPanel;
                    if (panel != null)
                    {
                        Image img = panel.Children[panel.Children.Count - 1] as Image;
                        img.Source = GetImage(col);
                       // panel.Children.Add(img);
                        //if null than replace, otherwise add to the end
                        if (firstNullElement == currColumn)
                        {
                            guess[firstNullElement] = mappings[col];
                        }
                        else
                        {
                            guess.Add(mappings[col]);
                        }
                        cell.IsEnabled = true;

                        //enable ok if now all are not null
                        int nullElements = guess.Where(el=>el == null).Count();

                        if (nullElements==0 && guess.Count==4)
                        {
                            okBtn.IsEnabled = true;
                        }
                    }
                }
            }
        }



        private int CountRightPlace() {
            int redCount = 0;
            for (int i = 0; i < 4; i++)
            {
                if (combination[i].Equals(guess[i]))
                {
                    redCount++;
                }
            }
            return redCount;
        }

        private int CountElementsGuessed()
        {
            int yellowCount=0;
            foreach (string element in combination)
            {
                if (guess.Contains(element))
                {
                    yellowCount++;
                    guess.Remove(element);
                }
            }
            return yellowCount;
        }


        private void PaintGuessGrid(int yellowCount, int redCount)
        {
            for (int i = 0; i < redCount; i++)
            {
                Image cell = guessGrid.Children
                    .Cast<UIElement>()
                    .First(el => Grid.GetRow(el) == currentRow && Grid.GetColumn(el) == i) as Image;
                if (cell != null)
                {
                    cell.Source = new BitmapImage(new Uri(PATH + "red.png"));
                }
            }

            for (int i = redCount; i < yellowCount + redCount; i++)
            {
                Image cell = guessGrid.Children
                   .Cast<UIElement>()
                   .First(el => Grid.GetRow(el) == currentRow && Grid.GetColumn(el) == i) as Image;
                if (cell != null)
                {
                    cell.Source = new BitmapImage(new Uri(PATH + "yellow.jpg"));
                }
            }
        }


        private void DisableButtonsCurrentRow()
        {
            for(int i=0; i<4; i++)
            {
                Button cell = mainGrid.Children
                  .Cast<UIElement>()
                  .First(el => Grid.GetRow(el) == currentRow && Grid.GetColumn(el) == i) as Button;

                if (cell != null)
                {
                    StackPanel panel = cell.Content as StackPanel;
                    if (panel != null)
                    {
                        Image img= panel.Children[panel.Children.Count - 1] as Image;
                      
                        panel.Children.Clear();
                        Grid.SetColumn(img, i);
                        Grid.SetRow(img, currentRow);
                        mainGrid.Children.Remove(cell);
                        mainGrid.Children.Add(img);
                    }
                }
            }
        }

        private void Grid_Cell_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                int col = Grid.GetColumn(btn);
                int row = Grid.GetColumn(btn);

                StackPanel panel = btn.Content as StackPanel;
                if (panel != null)
                {
                    Image img = panel.Children[panel.Children.Count-1] as Image;
                    img.Source = new BitmapImage(new Uri(PATH + "blue.jpg"));
                   
                    guess[col] = null;
                    okBtn.IsEnabled = false;
                }
            }
        }

        private void SetTimer()
        {
            timer = new DispatcherTimer();
            timer.Tick += TimerTicked;
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Start();
        }

        private void TimerTicked(object sender, EventArgs e)
        {
            remainedTimelbl.Content = --remainedTime;

            if (remainedTime == 0)
            {
                FinishGame(false);
            }
        }

        private void DisposeTimer()
        {
            timer.Tick -= TimerTicked;
            timer.Stop();
        }

        private void PrepareGame()
        {
            combination = new List<string>();
            guess = new List<string>();
            mappings = new Dictionary<int, string>();
            InitDictionary();
            MakeCombination();
            SetTimer();

            remainedTime = GAME_DURATION;
            remainedTimelbl.Content = remainedTime;
            currentRow = 0;

            mainGrid.Children.Clear();
            guessGrid.Children.Clear();
            iconsGrid.Children.Clear();

            //set empty cells
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Button newCell = new Button();
                    newCell.IsEnabled = false;
                    newCell.Margin = new Thickness(2, 2, 2, 2);
                    newCell.BorderBrush = Brushes.Transparent;
                    newCell.Background = Brushes.Transparent;
                    newCell.Style = (Style)FindResource("TransparentStyle");
                    newCell.Click += Grid_Cell_Click;

                    Image img = new Image();
                    img.Source = new BitmapImage(new Uri(PATH + "blue.jpg"));

                    StackPanel stackPnl = new StackPanel();
                    stackPnl.HorizontalAlignment = HorizontalAlignment.Stretch;
                    stackPnl.VerticalAlignment = VerticalAlignment.Stretch;
                    stackPnl.Background = Brushes.Transparent;
                    stackPnl.Children.Add(img);

                    newCell.Content = stackPnl;

                    Grid.SetColumn(newCell, j);
                    Grid.SetRow(newCell, i);
                    mainGrid.Children.Add(newCell);
                }
            }


            //set guesses cells
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Image img = new Image();
                    img.Source = new BitmapImage(new Uri(PATH+"black.png"));
                    img.Margin = new Thickness(5, 5, 5, 5);

                    Grid.SetColumn(img, j);
                    Grid.SetRow(img, i);
                    guessGrid.Children.Add(img);
                }
            }

            for (int i = 0; i < 6; i++)
            {
                Button newCell = new Button();
                newCell.IsEnabled = true;
                newCell.Margin = new Thickness(5, 5, 5, 5);
                newCell.BorderBrush = Brushes.White;
                newCell.BorderThickness = new Thickness(2);
                newCell.Background = Brushes.Transparent;
                newCell.Click += Element_Click;

                Image img = new Image();
                img.Source = GetImage(i);

                StackPanel stackPnl = new StackPanel();
                stackPnl.HorizontalAlignment = HorizontalAlignment.Stretch;
                stackPnl.VerticalAlignment = VerticalAlignment.Stretch;
                stackPnl.Background = Brushes.Transparent;
                stackPnl.Children.Add(img);

                newCell.Content = stackPnl;

                Grid.SetColumn(newCell, i);
                Grid.SetRow(newCell, 0);
                iconsGrid.Children.Add(newCell);
            }

            //disable Ok btn
            okBtn.IsEnabled = false;
        }
           
    }

}
