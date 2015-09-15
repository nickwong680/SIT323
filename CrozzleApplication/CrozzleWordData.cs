using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrozzleApplication
{
    class CrozzleWordData
    {
        private List<WordData> wordData;
        private List<WordData> horizontalWordData;
        private List<WordData> verticalWordData;
        private List<String> errorMessages;

        #region constructors
        public CrozzleWordData(List<String[]> crozzleRows, List<String[]> crozzleColumns)
        {
            wordData = new List<WordData>();
            horizontalWordData = new List<WordData>();
            verticalWordData = new List<WordData>();
            errorMessages = new List<string>();

            this.AddHorizontalWords(crozzleRows);
            this.AddVericalWords(crozzleColumns);
        }
        #endregion

        #region properties
        public int Count
        {
            get
            {
                return (horizontalWordData.Count + verticalWordData.Count);
            }
        }

        public Boolean ErrorsDetected
        {
            get
            {
                return (errorMessages.Count > 0);
            }
        }

        public List<String> ErrorMessages
        {
            get
            {
                return (errorMessages);
            }
        }
        #endregion

        #region identify words
        private void AddHorizontalWords(List<String[]> crozzleRows)
        {
            int rowNumber = 0;
            int columnIndex;
            String row;
            foreach (String[] crozzleRow in crozzleRows)
            {
                rowNumber++;
                columnIndex = 0;

                // place all letters into one string, so that we can split it later
                row = "";
                foreach (String letter in crozzleRow)
                    row = row + letter;

                // use split to collect all sequences of letters
                char[] separators = { ' ' };
                String[] letterSequences = row.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                // collect and store data about each letter sequence of length > 1, as a sequence of one letter is not a word
                foreach (String sequence in letterSequences)
                {
                    if (sequence.Length > 1)
                    {
                        // check for duplicate word
                        if (this.Contains(sequence))
                        {
                            errorMessages.Add("error: \"" + sequence + "\" already exists in the crozzle");
                        }

                        // collect data about the word, and 
                        // update the index for the next substring search
                        WordData word = new WordData(sequence, rowNumber, row.IndexOf(sequence, columnIndex) + 1, true);
                        columnIndex = word.location.column - 1 + sequence.Length;

                        // store data about the word
                        wordData.Add(word);
                        horizontalWordData.Add(word);
                    }
                }
            }
        }

        private void AddVericalWords(List<String[]> crozzleColumns)
        {
            int columnNumber = 0;
            int rowIndex;
            String column;
            foreach (String[] crozzleColumn in crozzleColumns)
            {
                columnNumber++;
                rowIndex = 0;

                // place all letters into one string, so that we can split it later
                column = "";
                foreach (String letter in crozzleColumn)
                    column = column + letter;

                // use split to collect all sequences of letters
                char[] separators = { ' ' };
                String[] letterSequences = column.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                // collect and store data about each letter sequence of length > 1, as a sequence of one letter is not a word
                foreach (String sequence in letterSequences)
                {
                    if (sequence.Length > 1)
                    {
                        // check for duplicate word
                        if (this.Contains(sequence))
                        {
                            errorMessages.Add("error: \"" + sequence + "\" already exists in the crozzle");
                        }

                        // collect data about the word, and 
                        // update the index for the next substring search
                        WordData word = new WordData(sequence, column.IndexOf(sequence, rowIndex) + 1, columnNumber, false);
                        rowIndex = word.location.row - 1 + sequence.Length;

                        // store data about the word
                        wordData.Add(word);
                        verticalWordData.Add(word);
                    }
                }
            }
        }

        private Boolean Contains(String sequence)
        {
            Boolean found = false;

            foreach (WordData word in wordData)
            {
                if (word.letters.Equals(sequence))
                {
                    found = true;
                    break;
                }
            }
            return (found);
        }
        #endregion

        #region intersecting letters
        public List<Char> GetIntersectingLetters()
        {
            List<Char> intersectingLetters = new List<Char>();

            foreach (WordData horizontalWord in horizontalWordData)
            {
                intersectingLetters.AddRange(GetIntersectingLetters(horizontalWord));
            }
            return (intersectingLetters);
        }

        private List<Char> GetIntersectingLetters(WordData horizontalWord)
        {
            List<Char> intersectingLetters = new List<Char>();

            foreach (WordData verticalWord in verticalWordData)
            {
                if (verticalWord.location.row == horizontalWord.location.row)
                {
                    if (verticalWord.location.column >= horizontalWord.location.column && verticalWord.location.column < horizontalWord.location.column + horizontalWord.letters.Length)
                    {
                        intersectingLetters.Add(verticalWord.letters[0]);
                    }
                }
                else if (verticalWord.location.row < horizontalWord.location.row)
                {
                    if (verticalWord.location.column >= horizontalWord.location.column && verticalWord.location.column < horizontalWord.location.column + horizontalWord.letters.Length)
                    {
                        if (verticalWord.location.row + verticalWord.letters.Length > horizontalWord.location.row)
                        {
                            intersectingLetters.Add(verticalWord.letters[horizontalWord.location.row - verticalWord.location.row]);
                        }
                    }
                }
            }
            return (intersectingLetters);
        }
        #endregion

        #region missing words
        public void FindMissingWords(List<String> wordList)
        {
            foreach (WordData word in wordData)
            {
                if (!wordList.Contains(word.letters))
                {
                    errorMessages.Add("error: \"" + word.letters + "\" at (" + word.location.row.ToString() + ", " + word.location.column.ToString() + ") does not exist in the wordlist");
                }
            }
        }
        #endregion

        #region word direction
        public void CheckWordDirection(List<String> wordList)
        {
            foreach (WordData word in wordData)
            {
                String mirrorSequence = new String(word.letters.Reverse().ToArray());

                // ignore palindromes
                if (mirrorSequence.Equals(word.letters))
                {
                    continue;
                }

                if (!wordList.Contains(word.letters) && wordList.Contains(mirrorSequence))
                {
                    errorMessages.Add("error: \"" + word.letters + "\" at (" + word.location.row.ToString() + ", " + word.location.column.ToString() + ") exists in the wordlist but is backwords");
                }
            }
        }
        #endregion

        #region check intersections
        public void CheckIntersections(int lowerLimit)
        {
            foreach (WordData word in wordData)
            {
                if (word.horizontal)
                {
                    if (GetVerticalIntersectingWords(word).Count < lowerLimit)
                    {
                        errorMessages.Add("error: \"" + word.letters + "\" at (" + word.location.row.ToString() + ", " + word.location.column.ToString() + ") is horizontal but does not intersect the correct number of vertical words");
                    }
                }
                else
                {
                    if (GetHorizontalIntersectingWords(word).Count < lowerLimit)
                    {
                        errorMessages.Add("error: \"" + word.letters + "\" at (" + word.location.row.ToString() + ", " + word.location.column.ToString() + ") is vertical but does not intersect the correct number of horizontal words");
                    }
                }
            }
        }
        public void CheckIntersections(int lowerLimit, int upperLimit)
        {
            foreach (WordData word in wordData)
            {
                if (word.horizontal)
                {
                    int verticalIntersections = GetVerticalIntersectingWords(word).Count;
                    if (verticalIntersections < lowerLimit || verticalIntersections > upperLimit)
                    {
                        errorMessages.Add("error: \"" + word.letters + "\" at (" + word.location.row.ToString() + ", " + word.location.column.ToString() + ") is horizontal but does not intersect the correct number of vertical words");
                    }
                }
                else
                {
                    int horizontalIntersections = GetHorizontalIntersectingWords(word).Count;
                    if (horizontalIntersections < lowerLimit || horizontalIntersections > upperLimit)
                    {
                        errorMessages.Add("error: \"" + word.letters + "\" at (" + word.location.row.ToString() + ", " + word.location.column.ToString() + ") is vertical but does not intersect the correct number of horizontal words");
                    }
                }
            }
        }

        private List<WordData> GetVerticalIntersectingWords(WordData horizontalWord)
        {
            List<WordData> verticalWords = new List<WordData>();

            foreach (WordData verticalWord in verticalWordData)
            {
                if (verticalWord.location.row == horizontalWord.location.row)
                {
                    if (verticalWord.location.column >= horizontalWord.location.column && verticalWord.location.column < horizontalWord.location.column + horizontalWord.letters.Length)
                    {
                        verticalWords.Add(horizontalWord);
                    }
                }
                else if (verticalWord.location.row < horizontalWord.location.row)
                {
                    if (verticalWord.location.column >= horizontalWord.location.column && verticalWord.location.column < horizontalWord.location.column + horizontalWord.letters.Length)
                    {
                        if (verticalWord.location.row + verticalWord.letters.Length > horizontalWord.location.row)
                        {
                            verticalWords.Add(horizontalWord);
                        }
                    }
                }
            }
            return (verticalWords);
        }

        private List<WordData> GetHorizontalIntersectingWords(WordData verticalWord)
        {
            List<WordData> horizontalWords = new List<WordData>();

            foreach (WordData horizontalWord in horizontalWordData)
            {
                if (horizontalWord.location.column == verticalWord.location.column)
                {
                    if (horizontalWord.location.row >= verticalWord.location.row && horizontalWord.location.row < verticalWord.location.row + verticalWord.letters.Length)
                    {
                        horizontalWords.Add(horizontalWord);
                    }
                }
                else if (horizontalWord.location.column < verticalWord.location.column)
                {
                    if (horizontalWord.location.row >= verticalWord.location.row && horizontalWord.location.row < verticalWord.location.row + verticalWord.letters.Length)
                    {
                        if (horizontalWord.location.column + horizontalWord.letters.Length > verticalWord.location.column)
                        {
                            horizontalWords.Add(horizontalWord);
                        }
                    }
                }
            }
            return (horizontalWords);
        }
        #endregion

        #region check touching words
        public void CheckTouchingWords()
        {
            CheckTouchingHorizontalWords();
            CheckTouchingVerticalWords();
        }

        private void CheckTouchingHorizontalWords()
        {
            WordData word;
            WordData word2;

            for (int i = 0; i < horizontalWordData.Count; i++)
            {
                word = horizontalWordData[i];

                for (int j = i + 1; j < horizontalWordData.Count; j++)
                {
                    word2 = horizontalWordData[j];

                    if (word.letters.Equals(word2.letters, StringComparison.Ordinal))
                        continue;

                    if (word2.location.row >= word.location.row - 1 && word2.location.row <= word.location.row + 1)
                    {
                        if (word2.location.column < word.location.column - 1 && word2.location.column + word2.letters.Length >= word.location.column)
                        {
                            errorMessages.Add("error: the horizontal word \"" + word.letters + "\" on row " + word.location.row.ToString() + " is touching the horizontal word \"" + word2.letters + "\" on row " + word2.location.row.ToString());
                        }
                        else if (word2.location.column >= word.location.column - 1 && word2.location.column <= word.location.column + word.letters.Length)
                        {
                            errorMessages.Add("error: the horizontal word \"" + word.letters + "\" on row " + word.location.row.ToString() + " is touching the horizontal word \"" + word2.letters + "\" on row " + word2.location.row.ToString());
                        }
                    }
                }
            }
        }

        private void CheckTouchingVerticalWords()
        {
            WordData word;
            WordData word2;

            for (int i = 0; i < verticalWordData.Count; i++)
            {
                word = verticalWordData[i];

                for (int j = i + 1; j < verticalWordData.Count; j++)
                {
                    word2 = verticalWordData[j];

                    if (word.letters.Equals(word2.letters, StringComparison.Ordinal))
                        continue;

                    if (word2.location.column >= word.location.column - 1 && word2.location.column <= word.location.column + 1)
                    {
                        if (word2.location.row < word.location.row - 1 && word2.location.row + word2.letters.Length >= word.location.row)
                        {
                            errorMessages.Add("error: the vertical word \"" + word.letters + "\" on column " + word.location.column.ToString() + " is touching the vertical word \"" + word2.letters + "\" on column " + word2.location.column.ToString());
                        }
                        else if (word2.location.row >= word.location.row - 1 && word2.location.row <= word.location.row + word.letters.Length)
                        {
                            errorMessages.Add("error: the vertical word \"" + word.letters + "\" on column " + word.location.column.ToString() + " is touching the vertical word \"" + word2.letters + "\" on column " + word2.location.column.ToString());
                        }
                    }
                }
            }
        }
        #endregion

        #region word-group connectivity
        public void CheckConnectivity(List<String[]> crozzleRows, List<String[]> crozzleColumns)
        {
            CrozzleMap map = new CrozzleMap(crozzleRows, crozzleColumns);

            // remove a group (representing a group of words) from the map
            map.RemoveGroup();

            // Does the map contain other groups, after removing one group?
            if (map.ContainsGroup)
            {
                errorMessages.Add("error: this crozzle contains more than one group of words");
            }
        }
        #endregion
    }
}