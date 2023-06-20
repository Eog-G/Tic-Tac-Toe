using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.Contracts;
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

namespace Xs___Os
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Global Variables
        PackIconKind playerIcon = new PackIconKind();
        bool gameOver = false;
        bool playerTurn = true;
        bool?[,] board = new bool?[3, 3];

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            playerIcon = PackIconKind.Close;
        }
        


        private void gridClick(object sender, MouseButtonEventArgs e)
        {
            if (gameOver) { return; }

            Border gridSquare = sender as Border;

            // Get Row-Column from border ID
            int row = Convert.ToInt16(gridSquare.Name.Substring(1,1));
            int column = Convert.ToInt16(gridSquare.Name.Substring(2, 1));

            if (board[row,column] == null)
            {
                board[row, column] = playerTurn;

                PackIcon packIcon = FindVisualChildren<PackIcon>(gridSquare).FirstOrDefault();

                packIcon.Foreground = Brushes.Black;
                packIcon.Kind = playerIcon;

                CheckWin(row, column);

                playerIcon = playerTurn ? PackIconKind.CircleOutline : PackIconKind.Close;
                playerTurn = !playerTurn;
            }

            
        }

        private void gridEnter(object sender, MouseEventArgs e)
        {
            if (gameOver) { return; }

            Border gridSquare = sender as Border;

            PackIcon packIcon = FindVisualChildren<PackIcon>(gridSquare).FirstOrDefault();

            if(packIcon.Foreground == Brushes.Black) { return; }

            packIcon.Foreground = Brushes.LightGray;
            packIcon.Kind = playerIcon;
        }

        private void gridLeave(object sender, MouseEventArgs e)
        {
            if (gameOver) { return; }

            Border gridSquare = sender as Border;

            PackIcon packIcon = FindVisualChildren<PackIcon>(gridSquare).FirstOrDefault();

            if (packIcon.Foreground == Brushes.Black) { return; }

            packIcon.Kind = PackIconKind.None;
        }

        // Helper method to find child elements of a specific type in the visual tree
        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject parent) where T : DependencyObject
        {
            if (parent != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(parent, i);

                    if (child is T typedChild)
                    {
                        yield return typedChild;
                    }

                    foreach (T foundChild in FindVisualChildren<T>(child))
                    {
                        yield return foundChild;
                    }
                }
            }
        }

        // Check if game has been won
        private void CheckWin(int row, int column)
        {
            // Check for horizontal win
            if(board[row,0] == board[row,1] && board[row,0] == board[row, 2])
            {
                gameOver = true;
                grid.IsEnabled = false;

                HighlightWin(BuildGridID(row, 0), BuildGridID(row, 1), BuildGridID(0, 2));

            }

            // Check for vertical win
            if (board[0, column] == board[1, column] && board[0, column] == board[2, column])
            {
                gameOver = true;
                grid.IsEnabled = false;

                HighlightWin(BuildGridID(0, column), BuildGridID(1, column), BuildGridID(2, column));
            }

            // Check for diagonal win 1
            if (board[0,0] == board[1,1] && board[0,0] == board[2, 2] && board[0,0] != null && board[1,1] != null && board[2,2] != null)
            {
                gameOver = true;
                grid.IsEnabled = false;

                HighlightWin("_00", "_11", "_22");

            }

            // Check for diagonal win 2
            if (board[0,2] == board[1,1] && board[0,2] == board[2, 0] && board[0,2] != null && board[1,1] != null && board[0,2] != null)
            {
                gameOver = true;
                grid.IsEnabled = false;

                HighlightWin("_02", "_11", "_20");
            }
        }

        private void HighlightWin(string id1, string id2, string id3)
        {
            Border grid1 = (Border)FindName(id1);
            Border grid2 = (Border)FindName(id2);
            Border grid3 = (Border)FindName(id3);

            PackIcon icon1 = FindVisualChildren<PackIcon>(grid1).FirstOrDefault();
            PackIcon icon2 = FindVisualChildren<PackIcon>(grid2).FirstOrDefault();
            PackIcon icon3 = FindVisualChildren<PackIcon>(grid3).FirstOrDefault();

            icon1.Foreground = Brushes.LimeGreen;
            icon2.Foreground = Brushes.LimeGreen;
            icon3.Foreground = Brushes.LimeGreen;
        }

        private string BuildGridID(int row, int column)
        {
            return "_" + row.ToString() + column.ToString();
        }
    }
}
