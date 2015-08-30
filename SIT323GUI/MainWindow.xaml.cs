using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
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
        private void CreateCrozzle(Constraints con)
        {
            //Height="334"  Width="585"

            int widthSize = (int) (CrozzleDataGrid.Width/_wordlist.Width);
            int heighSize = (int)(CrozzleDataGrid.Height / _wordlist.Height);


            for (int i = 0; i < _crozzle.Height; i++)
            {
                DataGridTextColumn col = new DataGridTextColumn();
                col.Header = i.ToString();
                col.Width = widthSize;
                col.Binding = new Binding(i.ToString());
                CrozzleDataGrid.Columns.Add(col);

            }

            for (int i = 0; i < _crozzle.Width; i++)
            {
                dynamic row = new ExpandoObject();
                for (int j = 0; j < _crozzle[i].Length; j++)
                {
                    ((IDictionary<String, Object>)row)[j.ToString()] = _crozzle[i][j];

                }
                CrozzleDataGrid.Items.Add(row);
            }
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
            ScoreBox.Background = Brushes.White;
            WordListBox.Items.Clear();
            CrozzleDataGrid.Columns.Clear();
            CrozzleDataGrid.Items.Clear();
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
        
        private void MenuClearLog_Click(object sender, RoutedEventArgs e)
        {
            TextBlockLog.Text = string.Empty;
        }
        private void MenuSaveLog_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text file (*.txt)|*.txt";
            if (saveFileDialog.ShowDialog() == true)
                File.WriteAllText(saveFileDialog.FileName, TextBlockLog.Text);
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
                    CreateCrozzle(con);
                    ScoreBox.Background = Brushes.LightSeaGreen;
                }
            }
            else
            {
                TextBlockLog.Text += _crozzle.LogListInString();
                ScoreBox.Background = Brushes.PaleVioletRed;
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