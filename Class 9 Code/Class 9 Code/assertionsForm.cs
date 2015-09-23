using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Diagnostics;

namespace Monday29
{
    public partial class assertionsForm : Form
    {
        public assertionsForm()
        {
            InitializeComponent();
        }


        private void debugEG_Click(object sender, EventArgs e)
        {
            int y = 0;

            Debug.Assert(y != 0);

            Debug.Assert(y != 0, "y should be non zero ....");

            Debug.WriteLineIf(y == 0, "y should not be 0");
        }

        private void simpleEG_Click(object sender, EventArgs e)
        {
            // Simple MessageBox

            int y = 0;

            if (y == 0)
                MessageBox.Show("y should not be 0");
        }

        private void okEG_Click(object sender, EventArgs e)
        {
            // Responding to clicking OK

            int y = 0;

            if (y == 0)
            {
                DialogResult result = MessageBox.Show("y should not be 0");
                if (result == DialogResult.OK)
                    Console.WriteLine("OK .....");
            }
        }

        private void customEG_Click(object sender, EventArgs e)
        {
            // Customising a MessageBox

            int y = 0;

            if (y == 0)
            {
                DialogResult result = MessageBox.Show("y should not be 0", "SIT323 message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (result == DialogResult.Cancel)
                    Console.WriteLine("CANCEL .....");
            }
        }

        private void anyButtonEG_Click(object sender, EventArgs e)
        {
            // Responding to any MessageBox button

            int y = 0;

            if (y == 0)
            {
                DialogResult result = MessageBox.Show("y should not be 0", "SIT323 message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                switch (result)
                {
                    case DialogResult.Yes: Console.WriteLine("YES .....");
                        break;
                    case DialogResult.No: Console.WriteLine("NO .....");
                        break;
                    case DialogResult.Cancel: Console.WriteLine("CANCEL .....");
                        break;
                }
            }
        }
    }
}
