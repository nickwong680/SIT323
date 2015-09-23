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
using System.Text.RegularExpressions;

namespace Practical2
{
    public partial class Practical2Week3 : Form
    {
        String fileName;
        String directoryName;

        public Practical2Week3()
        {
            InitializeComponent();


            char[] separators = { ',' };
            String line = ",R,O,B,E,R,T,,,,,,,,";

            String xxx = line.Substring(0, 1);

            String[] fields = line.Split(separators);    //, StringSplitOptions.RemoveEmptyEntries);
            int size = fields.Length;

            foreach (string fieldValue in fields)
            {
                Console.WriteLine(fieldValue);
            }


        }

        /// <summary>
        /// This event handler uses an OpenFileDialog to allow the user 
        /// to browser for a file, obtain and store the filename, and
        /// obtain and store the directory name containng that file.
        /// It confirms that the filename is obtained by displaying a MessageBox.
        /// </summary>
        /// <param name="sender">An object.</param>
        /// <param name="e">An EventArgs.</param>
        private void obtainFilename_Click(object sender, EventArgs e)
        {
            Boolean closingForm = false;
            DialogResult result;

            result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                fileName = openFileDialog1.FileName;
                directoryName = Path.GetDirectoryName(fileName);
                MessageBox.Show(fileName);

                if (!File.Exists(fileName))
                {
                    MessageBox.Show("error: file missing");
                    closingForm = true;
                }
            }
            else
            {
                fileName = "";
                MessageBox.Show("file not selected");
            }

            // close this form
            if (closingForm)
            {
                this.Close();
            }
        }

        private void readCharacters_Click(object sender, EventArgs e)
        {
            if (File.Exists(fileName))
            {
                StreamReader fileIn = new StreamReader(fileName);
                StreamWriter fileOut = new StreamWriter(directoryName + @"\practical 2 out.csv");
                char ch;

                // read and write character by character
                while (!fileIn.EndOfStream)
                {
                    ch = (char)fileIn.Read();
                    fileOut.Write(ch);
                    Console.Write(ch);
                }

                // close files
                fileIn.Close();
                fileOut.Close();
            }
            else
            {
                MessageBox.Show("error: file missing");
            }
        }

        private void readLines_Click(object sender, EventArgs e)
        {
            if (File.Exists(fileName))
            {
                StreamReader fileIn = new StreamReader(fileName);
                StreamWriter fileOut = new StreamWriter(directoryName + @"\practical 2 out.csv");
                String line;

                // read and write line by line
                while (!fileIn.EndOfStream)
                {
                    line = fileIn.ReadLine();
                    fileOut.WriteLine(line);
                    Console.WriteLine(line);
                }

                // close files
                fileIn.Close();
                fileOut.Close();
            }
            else
            {
                MessageBox.Show("error: file missing");
            }
        }

        private void readValues_Click(object sender, EventArgs e)
        {
            if (File.Exists(fileName))
            {
                StreamReader fileIn = new StreamReader(fileName);
                StreamWriter fileOut = new StreamWriter(directoryName + @"\practical 2 out.csv");
                String line;
                char[] separators = { ',' };
                String[] fields;

                // read line by line, and write field by field
                while (!fileIn.EndOfStream)
                {
                    line = fileIn.ReadLine();
                    fields = line.Split(separators);

                    // for a line, write field by field
                    foreach (string fieldValue in fields)
                    {
                        fileOut.Write(fieldValue);
                        Console.Write(fieldValue);
                    }
                }

                // close files
                fileIn.Close();
                fileOut.Close();
            }
            else
            {
                MessageBox.Show("error: file missing");
            }
        }

        private void validate_Click(object sender, EventArgs e)
        {
            if (File.Exists(fileName))
            {
                validateFile();
            }
            else
            {
                MessageBox.Show("error: file missing");
            }

        }

        private void validateFile()
        {
            StreamReader fileIn = new StreamReader(fileName);
            StreamWriter logfile = new StreamWriter(directoryName + @"\practical 2 log.txt");
            String line;
            char[] separators = { ',' };
            String[] fields;
            const int numberOfFields = 4;
            int lineNumber = 0;

            // validate file line by line
            while (!fileIn.EndOfStream)
            {
                line = fileIn.ReadLine();
                lineNumber++;
                fields = line.Split(separators);

                // check that the line has numberOfFields fields
                if (fields.Length != numberOfFields)
                {
                    logfile.WriteLine("error: line " + lineNumber.ToString() + " does not contain " + numberOfFields.ToString() + " fields");
                    Console.WriteLine("error: line " + lineNumber.ToString() + " does not contain " + numberOfFields.ToString() + " fields");

                    // as we don't know which field is missing, continue to the next line
                    continue;
                }

                // check that each field is not empty
                for (int i = 0; i < numberOfFields; i++)
                {
                    if (fields[i].Length == 0)
                    {
                        logfile.WriteLine("error: line " + lineNumber.ToString() + ": field " + (i + 1).ToString() + " is empty");
                        Console.WriteLine("error: line " + lineNumber.ToString() + ": field " + (i + 1).ToString() + " is empty");
                    }
                }

                // check that the 1st and 2nd fields are integers and in range
                int n;
                for (int i = 0; i < 2; i++)
                {
                    if (Int32.TryParse(fields[i], out n))
                    {
                        if(n >= 1000)
                        {
                            logfile.WriteLine("error: line " + lineNumber.ToString() + ": value of field " + (i + 1).ToString() + " is not in range");
                            Console.WriteLine("error: line " + lineNumber.ToString() + ": value of field " + (i + 1).ToString() + " is not in range");
                        }
                    }
                    else
                    {
                        logfile.WriteLine("error: line " + lineNumber.ToString() + ": field " + (i + 1).ToString() + " is not an integer");
                        Console.WriteLine("error: line " + lineNumber.ToString() + ": field " + (i + 1).ToString() + " is not an integer");
                    }
                }

                // check that the 3rd and 4th fields are alphabetic
                for (int i = 2; i < 4; i++)
                {
                    foreach(Char c in fields[i])
                    {
                        if(!Char.IsLower(c) && !Char.IsUpper(c))
                        {
                            logfile.WriteLine("error: line " + lineNumber.ToString() + ": field " + (i + 1).ToString() + " is not alphabetic");
                            Console.WriteLine("error: line " + lineNumber.ToString() + ": field " + (i + 1).ToString() + " is not alphabetic");
                            break;
                        }
                    }

                    // or use a regular expresion
                    //if (!Regex.IsMatch(fields[i], "^[a-zA-Z]+$"))
                    //{
                    //    logfile.WriteLine("error: line " + lineNumber.ToString() + ": field " + (i + 1).ToString() + " is not alphabetic");
                    //    Console.WriteLine("error: line " + lineNumber.ToString() + ": field " + (i + 1).ToString() + " is not alphabetic");
                    //}
                }
            }

            // close files
            fileIn.Close();
            logfile.Close();
        }
    }
}
