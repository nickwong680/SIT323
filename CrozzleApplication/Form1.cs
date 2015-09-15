using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;

namespace CrozzleApplication
{
    public partial class SIT323AssignmentForm : Form
    {
        private Crozzle aCrozzle;

        #region constructors
        public SIT323AssignmentForm()
        {
            InitializeComponent();

            aCrozzle = new Crozzle();
        }
        #endregion

        #region File menu event handlers
        private void openWordlistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result;

            // as we are opening a wordlist file,
            // indicate wordlist file, crozzle file and crozzle are not valid, and clear GUI
            openWordlistToolStripMenuItem.Checked = false;
            openCrozzleToolStripMenuItem.Checked = false;
            crozzleToolStripMenuItem.Checked = false;
            crozzleToolStripMenuItem.Enabled = false;
            difficultyLevel.Text = "";
            scoreLabel.Text = "";
            crozzleTextBox.Text = "";

            // process wordlist file
            openFileDialog1.InitialDirectory = Application.StartupPath + @"\..\..";
            result = openFileDialog1.ShowDialog();            
            if (result == DialogResult.OK)
            {
                aCrozzle.readWordlistFile(openFileDialog1.FileName);
                if (aCrozzle.WordlistFileValid)
                {
                    openWordlistToolStripMenuItem.Checked = true;
                    difficultyLevel.Text = aCrozzle.Difficulty;
                }
                else
                    MessageBox.Show("wordlist file is invalid");
                openCrozzleToolStripMenuItem.Enabled = true;
            }
        }

        private void openCrozzleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result;

            // as we are opening a crozzle file,
            // indicate crozzle file and crozzle are not valid, and clear GUI
            openCrozzleToolStripMenuItem.Checked = false;
            crozzleToolStripMenuItem.Checked = false;
            crozzleToolStripMenuItem.Enabled = false;
            scoreLabel.Text = "";
            crozzleTextBox.Text = "";

            // process crozzle file
            result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                // read crozzle file and validate file
                aCrozzle.readCrozzleFile(openFileDialog1.FileName);
                if (aCrozzle.CrozzleFileValid)
                    openCrozzleToolStripMenuItem.Checked = true;
                else
                    MessageBox.Show("crozzle file is invalid");
                if (openWordlistToolStripMenuItem.Checked && openCrozzleToolStripMenuItem.Checked)
                    crozzleToolStripMenuItem.Enabled = true;

                // display crozzle whether valid or invalid
                crozzleTextBox.Text = aCrozzle.ToString();
            }
        }

        private void exitToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region Validate menu event handlers
        private void crozzleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // check if the crozzle is valid
            aCrozzle.validate(openFileDialog1.FileName);

            // get score
            String score = "";
            if (aCrozzle.CrozzleValid)
            {
                crozzleToolStripMenuItem.Checked = true;
                score = aCrozzle.score().ToString();
            }
            else
            {
                crozzleToolStripMenuItem.Checked = false;
                score = "INVALID";
            }

            // display score
            scoreLabel.Text = score;
        }
        #endregion

        #region Help menu event handlers
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Crozzle Creator - 2015", "SIT323 Assignment");
        }
        #endregion

    }
}
