using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.IO;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Microsoft.Win32;
using SIT323;
using SIT323.Models;
using SIT323Project2;
using SIT323Project2.Models;

namespace SIT323GUI
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Crozzle _crozzle;
        private Wordlist _wordlist;
        private int _highScore;

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
                    string str = (_crozzle[i][j] == default(char)) ? " " : _crozzle[i][j].ToString();
                    ((IDictionary<String, Object>)row)[j.ToString()] = str;

                }
                CrozzleDataGrid.Items.Add(row);
            }
        }
        private void ClearAll()
        {
            _crozzle = null;
            _wordlist = null;
            _highScore = 0;
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

        private void ClearPostGenerated()
        {
            
            _crozzle = null;
            ScoreBox.Text = string.Empty;
            ScoreBox.Background = Brushes.White;
            WordListBox.Items.Clear();
            CrozzleDataGrid.Columns.Clear();
            CrozzleDataGrid.Items.Clear();
            MenuOpenSaveCrozzle.IsEnabled = true;
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
                ShowGeneratorMessageBox();
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
        private void MenuOptionSaveCrozzle_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text file (*.txt)|*.txt";
            if (saveFileDialog.ShowDialog() == true)
                File.WriteAllText(saveFileDialog.FileName, _crozzle.PrintCharacter());
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

            UpdateGUIAndScore();

        }

        private int UpdateGUIAndScore()
        {
            int score = 0;
            if (_crozzle.LogList.Count == 0)
            {
                Constraints con;
                score = ApplyConstraintsAndGetScore(out con);
                ScoreBox.Text = score.ToString();
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
            return score;
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            TimeSpan timeout = TimeSpan.FromMinutes(5);
            DateTime start_time = DateTime.Now;


            BackgroundWorker worker = sender as BackgroundWorker;

            while (DateTime.Now - start_time < timeout)
            {
                var crozzleProject2 = new CrozzleProject2(_wordlist);
                var gen = new CrozzleGenerator(crozzleProject2, _wordlist);
                gen.PlaceWordsToGrid();

                var crozzle = new Crozzle(crozzleProject2.CrozzleArrayOfChar(), _wordlist);
                //TextBlockLog.AppendText("Processing Generated Crozzle" + Environment.NewLine);

                if (crozzle == null) return;
                int score;

                Constraints con;
                score = ApplyConstraintsAndGetScore(out con, crozzle);
                if (score > _highScore)
                {
                    _highScore = score;
                    _crozzle = crozzle;
                    worker.ReportProgress(_highScore, crozzle);
                }
            }
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ClearPostGenerated();
            TextBlockLog.AppendText(String.Format("Current High Score: {0}%", e.ProgressPercentage) + Environment.NewLine);
            _crozzle = (Crozzle) e.UserState;
            UpdateGUIAndScore();
        }

        private void ShowGeneratorMessageBox()
        {
            MessageBoxResult result = MessageBox.Show("Do you want to Generate Crozzle?",
              "Generate Crozzle", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                BackgroundWorker worker = new BackgroundWorker();
                worker.WorkerReportsProgress = true;
                worker.DoWork += new DoWorkEventHandler(worker_DoWork);
                worker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
                worker.RunWorkerAsync();
            }
            else if (result == MessageBoxResult.No)
            {
                
            }
        }

        private int ApplyConstraintsAndGetScore(out Constraints con, Crozzle crozzle)
        {
            con = null;
            var pointScheme = default(PointScheme);
            switch (_wordlist.Level)
            {
                case Difficulty.Easy:
                    con = new EasyConstraints(crozzle, _wordlist);
                    pointScheme = PointScheme.OneEach;
                    break;
                case Difficulty.Medium:
                    con = new MediumConstraints(crozzle, _wordlist);
                    pointScheme = PointScheme.Incremental;
                    break;
                case Difficulty.Hard:
                    con = new HardConstraints(crozzle, _wordlist);
                    pointScheme = PointScheme.IncrementalWithBonusPerWord;
                    break;
                case Difficulty.Extreme:
                    con = new ExtremeConstraints(crozzle, _wordlist);
                    pointScheme = PointScheme.CustomWithBonusPerIntersection;
                    break;
            }
            if (con.LogList.Count == 0)
            {
                return Score.PointsFactory(con.WordsFromCrozzle, pointScheme).TotalScore;
            }
            return 0;
        }

        private int ApplyConstraintsAndGetScore(out Constraints con )
        {
            return ApplyConstraintsAndGetScore(out con, _crozzle);
        }
    }
}