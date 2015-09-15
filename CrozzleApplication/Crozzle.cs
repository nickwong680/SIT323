using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace CrozzleApplication
{
    class Crozzle
    {
        const int headerLength = 4;
        const int minimumWords = 10;
        const int maximumWords = 1000;
        const int minimumRows = 4;
        const int maximumRows = 400;
        const int minimumColumns = 4;
        const int maximumColumns = 400;
        const String difficultyEasy = "EASY";
        const String difficultyMedium = "MEDIUM";
        const String difficultyHard = "HARD";
        const String difficultyExtreme = "EXTREME";
        const int lengthLocation = 0;
        const int rowLocation = 1;
        const int columnLocation = 2;
        const int difficultyLocation = 3;

        private Boolean wordlistFileValid;
        private Boolean crozzleFileValid;
        private Boolean crozzleValid;

        private String[] header;
        private List<String> wordList;
        private List<String[]> crozzleRows;
        private List<String[]> crozzleColumns;
        private CrozzleWordData crozzleWordData;

        #region constructors
        public Crozzle()
        {
            wordlistFileValid = false;
            crozzleFileValid = false;
            crozzleValid = false;
        }
        #endregion

        #region properties
        public Boolean WordlistFileValid
        {
            get
            {
                return (wordlistFileValid);
            }
        }

        public Boolean CrozzleFileValid
        {
            get
            {
                return (crozzleFileValid);
            }
        }

        public Boolean CrozzleValid
        {
            get
            {
                return (crozzleValid);
            }
        }

        public String Difficulty
        {
            get
            {
                if (wordlistFileValid)
                    return (header[difficultyLocation]);
                else
                    return ("unknown");
            }
        }
        #endregion

        #region Open and validate wordlist File
        public void readWordlistFile(String path)
        {
            String fileName = Path.GetFileName(path);
            StreamReader fileIn = new StreamReader(path);
            StreamWriter logfile = new StreamWriter(Path.GetDirectoryName(path) + @"\assignment 1 - log.txt", true);

            // start log
            logfile.WriteLine("START PROCESSING FILE: " + fileName);
            Console.WriteLine("START PROCESSING FILE: " + fileName);

            // get potential header and words
            char[] separators = { ',' };
            String line = fileIn.ReadLine();
            String[] fields = line.Split(separators);
            header = new String[headerLength];
            String[] wordlist = new String[fields.Length - headerLength];
            if (fields.Length >= 4)
            {
                for (int i = 0; i < headerLength; i++)
                    header[i] = fields[i];

                for (int i = 0; i < fields.Length - headerLength; i++)
                    wordlist[i] = fields[i + headerLength];
            }
            else
            {
                for (int i = 0; i < fields.Length; i++)
                    header[i] = fields[i];

                wordlist = null;
            }

            // validate file
            int errorCountHeader = validateHeader(header, logfile);
            int errorCountWordlist = validateWordlist(wordlist, logfile);
            if (errorCountHeader == 0 && errorCountWordlist == 0)
            {
                wordList = new List<String>();
                for (int i = 0; i < wordlist.Length; i++)
                    wordList.Add(wordlist[i]);

                int listSize;
                Int32.TryParse(header[0], out listSize);
                if (listSize != wordlist.Length)
                {
                    logfile.WriteLine("error: field 1: list size (" + header[0] + ") doesn't match the number (" + wordlist.Length.ToString() + ") of words found");
                    Console.WriteLine("error: field 1: list size (" + header[0] + ") doesn't match the number (" + wordlist.Length.ToString() + ") of words found");
                }
            }

            // end log
            logfile.WriteLine("END PROCESSING FILE: " + fileName);
            Console.WriteLine("END PROCESSING FILE: " + fileName);

            // close files
            fileIn.Close();
            logfile.Close();

            // store validity
            if (errorCountHeader == 0 && errorCountWordlist == 0)
                wordlistFileValid = true;
            else
                wordlistFileValid = false;
        }

        public int validateHeader(String[] header, StreamWriter logfile)
        {
            int errorCount = 0;
            int fieldNumber = 0;
            String message;

            // check that the header has correct length
            if (header.Length != headerLength)
            {
                errorCount++;
                logfile.WriteLine("error: header contains " + header.Length.ToString() + " fields, instead of " + headerLength.ToString());
                Console.WriteLine("error: header contains " + header.Length.ToString() + " fields, instead of " + headerLength.ToString());
            }

            if (header.Length > 0)
            {
                // check that each field is not empty
                for (int field = 0; field < headerLength; field++)
                {
                    if (header[field].Length == 0)
                    {
                        errorCount++;
                        logfile.WriteLine("error: field " + field.ToString() + " is empty");
                        Console.WriteLine("error: field " + field.ToString() + " is empty");
                    }
                }

                // check that the 1st field is an integer and in range
                fieldNumber = 0;
                message = checkHeaderField(header[fieldNumber], minimumWords, maximumWords);
                if (message.Length > 0)
                {
                    errorCount++;
                    logfile.WriteLine("error: field " + fieldNumber.ToString() + ": number of words value (" + header[fieldNumber] + ") is " + message);
                    Console.WriteLine("error: field " + fieldNumber.ToString() + ": number of words value (" + header[fieldNumber] + ") is " + message);
                }

                // check that the 2nd field is an integer and in range
                fieldNumber = 1;
                message = checkHeaderField(header[fieldNumber], minimumRows, maximumRows);
                if (message.Length > 0)
                {
                    errorCount++;
                    logfile.WriteLine("error: field " + fieldNumber.ToString() + ": row size value (" + header[fieldNumber] + ") is " + message);
                    Console.WriteLine("error: field " + fieldNumber.ToString() + ": row size value (" + header[fieldNumber] + ") is " + message);
                }

                // check that the 3rd field is an integer and in range
                fieldNumber = 2;
                message = checkHeaderField(header[fieldNumber], minimumColumns, maximumColumns);
                if (message.Length > 0)
                {
                    errorCount++;
                    logfile.WriteLine("error: field " + fieldNumber.ToString() + ": column size value (" + header[fieldNumber] + ") is " + message);
                    Console.WriteLine("error: field " + fieldNumber.ToString() + ": column size value (" + header[fieldNumber] + ") is " + message);
                }

                // check that the 4th field is a difficulty level
                fieldNumber = 3;
                if (!header[fieldNumber].Equals(difficultyEasy, StringComparison.Ordinal) &&
                    !header[fieldNumber].Equals(difficultyMedium, StringComparison.Ordinal) &&
                    !header[fieldNumber].Equals(difficultyHard, StringComparison.Ordinal) &&
                    !header[fieldNumber].Equals(difficultyExtreme, StringComparison.Ordinal))
                {
                    errorCount++;
                    logfile.WriteLine("error: field " + fieldNumber.ToString() + ": difficulty level value (" + header[fieldNumber] + ") is incorrect");
                    Console.WriteLine("error: field " + fieldNumber.ToString() + ": difficulty level value (" + header[fieldNumber] + ") is incorrect");
                }
            }

            return (errorCount);
        }

        // check that the numeric header field is an integer and in range
        private String checkHeaderField(String field, int lowerLimit, int upperLimit)
        {
            String errorMessage = "";
            int n;

            if (Int32.TryParse(field, out n))
            {
                if (n < lowerLimit || n > upperLimit)
                {
                    errorMessage = "not in range";
                }
            }
            else
            {
                errorMessage = "not an integer";
            }
            return (errorMessage);
        }

        private int validateWordlist(String[] wordlist, StreamWriter logfile)
        {
            int errorCount = 0;
            int wordCount = 0;

            foreach (String field in wordlist)
            {
                // check that the field is alphabetic
                if (Regex.IsMatch(field, "^[a-zA-Z]+$"))
                {
                    wordCount++;

                    // check the maximum word limit
                    if (wordCount == maximumWords + 1)
                    {
                        errorCount++;
                        logfile.WriteLine("error: wordlist contains more than " + maximumWords + " words");
                        Console.WriteLine("error: wordlist contains more than " + maximumWords + " words");
                    }
                }
                else
                {
                    errorCount++;
                    logfile.WriteLine("error: word value (" + field + ") is not alphabetic");
                    Console.WriteLine("error: word value (" + field + ") is not alphabetic");
                }

            }

            // check the minimmum word limit
            if (wordCount < minimumWords)
            {
                errorCount++;
                logfile.WriteLine("error: wordlist contains less than " + minimumWords + " words");
                Console.WriteLine("error: wordlist contains less than " + minimumWords + " words");
            }

            return (errorCount);
        }
        #endregion

        #region Open and validate crozzle File
        public void readCrozzleFile(String path)
        {
            String fileName = Path.GetFileName(path);
            StreamReader fileIn = new StreamReader(path);
            StreamWriter logfile = new StreamWriter(Path.GetDirectoryName(path) + @"\assignment 1 - log.txt", true);
            int errorCount = 0;
            String line;
            char[] separators = { ',' };
            String[] row;
            int rowNumber = 0;
            int columnNumber = 0;
            crozzleRows = new List<String[]>();


            // start log
            logfile.WriteLine("START PROCESSING FILE: " + fileName);
            Console.WriteLine("START PROCESSING FILE: " + fileName);

            // validate file
            while (!fileIn.EndOfStream)
            {
                line = fileIn.ReadLine();
                rowNumber++;

                row = new String[line.Length];
                int i = 0;
                foreach (Char c in line)
                {
                    row[i] = new String(c, 1);
                    i++;
                }

                crozzleRows.Add(row);

                // check that the number of fields/columns match the specification in the header
                int columns;
                Int32.TryParse(header[columnLocation], out columns);
                if (row.Length != columns)
                {
                    errorCount++;
                    logfile.WriteLine("error: line " + rowNumber.ToString() + ": row contains " + row.Length.ToString() + " columns, instead of " + header[columnLocation] + " columns");
                    Console.WriteLine("error: line " + rowNumber.ToString() + ": row contains " + row.Length.ToString() + " columns, instead of " + header[columnLocation] + " columns");
                }

                for (columnNumber = 0; columnNumber < row.Length; columnNumber++)
                {
                    // check that the cell contains only 1 alphabetic character or a space
                    if (!Regex.IsMatch(row[columnNumber], "^[a-zA-Z ]$"))
                    {
                        errorCount++;
                        logfile.WriteLine("error: line " + rowNumber.ToString() + ": cell[" + rowNumber.ToString() + ", " + (columnNumber + 1).ToString() + "] contains \"" + row[columnNumber] + "\", it should contain exactly 1 alphabetic character or a space");
                        Console.WriteLine("error: line " + rowNumber.ToString() + ": cell[" + rowNumber.ToString() + ", " + (columnNumber + 1).ToString() + "] contains \"" + row[columnNumber] + "\", it should contain exactly 1 alphabetic character or a space");
                    }
                }
            }

            // check that the number of rows satisfy the required minimum
            if (rowNumber < minimumRows)
            {
                errorCount++;
                logfile.WriteLine("error: line " + rowNumber.ToString() + ": file contains " + rowNumber.ToString() + " rows, which is lower than the required minimum of " + minimumRows + " rows");
                Console.WriteLine("error: line " + rowNumber.ToString() + ": file contains " + rowNumber.ToString() + " rows, which is lower than the required minimum of " + minimumRows + " rows");
            }

            // check that the number of rows is less than the required maximum
            if (rowNumber > maximumRows)
            {
                errorCount++;
                logfile.WriteLine("error: line " + rowNumber.ToString() + ": file contains " + rowNumber.ToString() + " rows, which is greater than the required maximum of " + maximumRows + " rows");
                Console.WriteLine("error: line " + rowNumber.ToString() + ": file contains " + rowNumber.ToString() + " rows, which is greater than the required maximum of " + maximumRows + " rows");
            }

            // check that the number of rows match the specification in the header
            int rows;
            Int32.TryParse(header[rowLocation], out rows);
            if (rowNumber != rows)
            {
                errorCount++;
                logfile.WriteLine("error: line " + rowNumber.ToString() + ": file contains " + rowNumber.ToString() + " rows, but the header specified " + header[rowLocation] + " rows");
                Console.WriteLine("error: line " + rowNumber.ToString() + ": file contains " + rowNumber.ToString() + " rows, but the header specified " + header[rowLocation] + " rows");
            }

            // end log
            logfile.WriteLine("END PROCESSING FILE: " + fileName);
            Console.WriteLine("END PROCESSING FILE: " + fileName);

            // close files
            fileIn.Close();
            logfile.Close();

            // store validity
            crozzleFileValid = (errorCount <= 0);
        }
        #endregion

        #region validate crozzle
        public void validate(String path)
        {
            String fileName = Path.GetFileName(path);
            StreamReader fileIn = new StreamReader(path);
            StreamWriter logfile = new StreamWriter(Path.GetDirectoryName(path) + @"\assignment 1 - log.txt", true);

            // Start log.
            logfile.WriteLine("START PROCESSING CROZZLE: " + fileName);
            Console.WriteLine("START PROCESSING CROZZLE: " + fileName);

            // Crozzle rows were obtained when reading the crozzle file,
            // but crozzle columns are required too.
            crozzleColumns = createCrozzleColumns();

            // Common Constraint 2 - Get all sequences of 2 or more letters, duplicates are reported as an error.
            crozzleWordData = new CrozzleWordData(crozzleRows, crozzleColumns);

            // Common Constraint 1 - Check that each sequence in 'crozzleWordData' is in the wordlist.
            crozzleWordData.FindMissingWords(wordList);

            // Common Constraints 3 and 4 - Check for words running backwards.
            crozzleWordData.CheckWordDirection(wordList);

            // Check whether each word intersects the correct number of words.
            if (header[difficultyLocation].Equals(difficultyEasy, StringComparison.Ordinal) ||
                header[difficultyLocation].Equals(difficultyMedium, StringComparison.Ordinal))
            {
                // EASY and MEDIUM Constraints 1 and 2:
                // Check that each word intersects a number of words in the range [1, 2].
                crozzleWordData.CheckIntersections(1, 2);
            }
            else if (header[difficultyLocation].Equals(difficultyHard, StringComparison.Ordinal) ||
                header[difficultyLocation].Equals(difficultyExtreme, StringComparison.Ordinal))
            {
                // HARD and EXTREME Constraints 1 and 2:
                // Check that each word intersects one or more words.
                crozzleWordData.CheckIntersections(1);
            }

            // EASY Constraint 4 and 5 - Check whether each word doesn't touch another word with the same orientation.
            if (header[difficultyLocation].Equals(difficultyEasy, StringComparison.Ordinal))
            {
                crozzleWordData.CheckTouchingWords();
            }

            // Check the overall connectivity.
            // An EXTREME crozzle can contain only 1 group of connected words.
            // EASY, MEDIUM and HARD crozzles can contain one or more groups of words.
            if (header[difficultyLocation].Equals(difficultyExtreme, StringComparison.Ordinal))
            {
                // EXTRENE Constraint 3 - Check that there is only one group of connected words.
                crozzleWordData.CheckConnectivity(crozzleRows, crozzleColumns);
            }

            // Is this crozzle valid?
            if (crozzleWordData.ErrorsDetected)
            {
                crozzleValid = false;
                foreach (String message in crozzleWordData.ErrorMessages)
                {
                    logfile.WriteLine(message);
                    Console.WriteLine(message);
                }
            }
            else
            {
                crozzleValid = true;
            }

            // end log
            logfile.WriteLine("END PROCESSING CROZZLE: " + fileName);
            Console.WriteLine("END PROCESSING CROZZLE: " + fileName);

            // close files
            fileIn.Close();
            logfile.Close();
        }
        #endregion

        #region scoring
        public int score()
        {
            int score = -1;

            if (header[difficultyLocation].Equals(difficultyEasy, StringComparison.Ordinal))
            {
                score = EasyScore();
            }
            else if (header[difficultyLocation].Equals(difficultyMedium, StringComparison.Ordinal))
            {
                score = MediumScore();
            }
            else if (header[difficultyLocation].Equals(difficultyHard, StringComparison.Ordinal))
            {
                score = HardScore();
            }
            else if (header[difficultyLocation].Equals(difficultyExtreme, StringComparison.Ordinal))
            {
                score = ExtremeScore();
            }
            return (score);
        }

        private int EasyScore()
        {
            // One point is scored for each letter within a valid crozzle.
            int score = 0;

            // Increase the score for each non-space letter.
            foreach (String[] letters in crozzleRows)
            {
                foreach (String letter in letters)
                {
                    if (letter[0] != ' ')
                        score++;
                }
            }
            return (score);
        }

        private int MediumScore()
        {
            // Points are scored for each letter within a valid crozzle based on the following equivalences:
            // A=1, B=2, C=3, D=4… X=24, Y=25, and Z=26.
            int score = 0;
            int[] letterValues = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 };

            // Increase the score for each non-space letter.
            foreach (String[] letters in crozzleRows)
            {
                foreach (String letter in letters)
                {
                    if (letter[0] != ' ')
                        score = score + letterValues[(int)letter[0] - (int)'A'];
                }
            }
            return (score);
        }

        private int HardScore()
        {
            // 10 points are scored for each word placed from the word list into a valid crozzle.
            // Points are scored for each letter within a valid crozzle based on the following equivalences:
            // A=1, B=2, C=3, D=4… X=24, Y=25, and Z=26.
            int score = 0;
            int[] letterValues = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 };
            const int pointsPerWord = 10;

            // Increase the score for each word.
            score = score + crozzleWordData.Count * pointsPerWord;

            // Increase the score for each non-space letter.
            foreach (String[] letters in crozzleRows)
            {
                foreach (String letter in letters)
                {
                    if (letter[0] != ' ')
                        score = score + letterValues[(int)letter[0] - (int)'A'];
                }
            }

            return (score);
        }

        private int ExtremeScore()
        {
            // 10 points are scored for each word placed from the word list into a valid crozzle.
            // Each letter at the intersection of two words is awarded points as follows:
            // 1 point for A, E, I, O, U
            // 2 points for B, C, D, F, G
            // 4 points for H, J, K, L, M
            // 8 points for N, P, Q, R
            // 16 points for S, T, V,
            // 32 points for W, X, Y
            // 64 points for Z
            int score = 0;
            int[] letterValues = { 1, 2, 2, 2, 1, 2, 2, 4, 1, 4, 4, 4, 4, 8, 1, 8, 8, 8, 16, 16, 1, 16, 32, 32, 32, 64 };
            const int pointsPerWord = 10;

            // increase the score for each word.
            score = score + crozzleWordData.Count * pointsPerWord;

            // Increase the score for intersecting letters.
            List<Char> intersectingLetters = crozzleWordData.GetIntersectingLetters();
            foreach (Char letter in intersectingLetters)
            {
                score = score + letterValues[(int)letter - (int)'A'];
            }

            return (score);
        }
        #endregion

        #region crozzle representation
        private List<String[]> createCrozzleColumns()
        {
            // get the number of columns
            int numberOfColumns;
            Int32.TryParse(header[columnLocation], out numberOfColumns);

            // create a List to store strings, one string for each column
            List<String[]> columns = new List<String[]>();

            // create each column and add to crozzleColumns
            for (int x = 0; x < numberOfColumns; x++)
            {
                int y = 0;
                String[] column = new String[crozzleRows.Count];
                foreach (String[] row in crozzleRows)
                {
                    column[y++] = row[x];
                }
                columns.Add(column);
            }

            return (columns);
        }

        public override String ToString()
        {
            String s = "";

            foreach (String[] letters in crozzleRows)
            {
                foreach (String letter in letters)
                    s = s + letter;
                s = s + "\r\n";
            }
            return (s);
        }
        #endregion
    }
}