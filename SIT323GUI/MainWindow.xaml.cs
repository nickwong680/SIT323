using System;
using System.Windows;
using Microsoft.Win32;
using SIT323;
using SIT323.Models;

namespace SIT323GUI
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Crozzle _crozzle;
        private Wordlist _wordlist;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ClearAll()
        {
            _crozzle = null;
            _wordlist = null;
            MenuOpenCrozzle.IsEnabled = false;
            LevelBox.Text = string.Empty;
            RowsBox.Text = string.Empty;
            ColumnsBox.Text = string.Empty;
            TextBlockLog.Text = string.Empty;
            ScoreBox.Text = string.Empty;
            WordListBox.Items.Clear();
        }

        private void MenuOpenWordList_Click(object sender, RoutedEventArgs e)
        {
            ClearAll();
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV files|*.csv";
            if (openFileDialog.ShowDialog() == true)
            {
                _wordlist = new Wordlist(openFileDialog.FileName);
                TextBlockLog.Text += "Processing" + openFileDialog.FileName + Environment.NewLine;
            }
            if (_wordlist == null) return;
            if (_wordlist.LogList.Count == 0)
            {
                MenuOpenCrozzle.IsEnabled = true;
                LevelBox.Text = _wordlist.Level.ToString();
                RowsBox.Text = _wordlist.Width.ToString();
                ColumnsBox.Text = _wordlist.Height.ToString();

            }
            else
            {
                TextBlockLog.Text += _wordlist.LogListInString();
                MenuOpenCrozzle.IsEnabled = false;
            }
        }

        private void MenuOpenCrozzle_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "TXT files|*.txt";

            if (openFileDialog.ShowDialog() == true)
            {
                _crozzle = new Crozzle(openFileDialog.FileName, _wordlist);
                TextBlockLog.Text += "Processing" + openFileDialog.FileName + Environment.NewLine;
            }
            if (_crozzle == null) return;
            if (_crozzle.LogList.Count == 0)
            {
                Constraints con;
                ScoreBox.Text = ApplyConstraintsAndGetScore(out con).ToString();
                if (con != null)
                {
                    WordListBox.Items.Clear();
                    foreach (var word in con.WordsFromCrozzle)
                    {
                        WordListBox.Items.Add(word.ToString());
                    }
                }
                DataGrid.ItemsSource = _wordlist.WordList;
            }
            else
            {
                TextBlockLog.Text += _crozzle.LogListInString();
            }
        }

        private int ApplyConstraintsAndGetScore(out Constraints con)
        {
            con = null;
            var pointScheme = default(PointScheme);
            switch (_wordlist.Level)
            {
                case Difficulty.Easy:
                    con = new EasyConstraints(_crozzle, _wordlist);
                    pointScheme = PointScheme.OneEach;
                    break;
                case Difficulty.Medium:
                    con = new MediumConstraints(_crozzle, _wordlist);
                    pointScheme = PointScheme.Incremental;
                    break;
                case Difficulty.Hard:
                    con = new HardConstraints(_crozzle, _wordlist);
                    pointScheme = PointScheme.IncrementalWithBonusPerWord;
                    break;
                case Difficulty.Extreme:
                    con = new ExtremeConstraints(_crozzle, _wordlist);
                    pointScheme = PointScheme.CustomWithBonusPerIntersection;
                    break;
            }
            if (con.LogList.Count == 0)
            {
                return Score.PointsFactory(con.WordsFromCrozzle, pointScheme).TotalScore;
            }
            TextBlockLog.Text += con.LogListInString();
            return 0;
        }
    }
}