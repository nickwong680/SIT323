using System;
using System.Collections.Generic;
using System.Linq;
using SIT323;
using SIT323.Models;
using SIT323Project2.Models;

namespace SIT323Project2
{

    public class CrozzleGenerator
    {
        private CrozzleProject2 crozzle;
        private Wordlist wordlist;
        private readonly List<string> wordsNotAddedList;
        private Difficulty difficulty;

        public CrozzleGenerator(CrozzleProject2 crozzle, Wordlist wordlist, Difficulty difficulty)
        {
            this.crozzle = crozzle;
            this.wordlist = wordlist;
            this.difficulty = difficulty;
            this.wordsNotAddedList = new List<string>(wordlist.WordList.OrderByDescending(w => w.Count()));
        }

        public void PlaceWordToGrid()
        {
            Word word;
            if (crozzle.Wordlist.Count == 0)
            {
                word = new Word(Direction.Horizontal, wordsNotAddedList.LastOrDefault());
                AddWord(word, wordlist.Width / 2, wordlist.Height /2);
            }
            do
            {

            } while (wordsNotAddedList.Count > 0);

            Console.WriteLine(crozzle.ToString());
        }

        private bool DoesWordFit(Word word, int width, int height)
        {
            if (word.Direction == Direction.Vertical)
            {
                for (int i = 0; i < word.CharacterList.Count; i++)
                {
                    
                }
            }

            return true;
        }

        public void AddWord(Word word, int width, int height)
        {
            crozzle.Wordlist.Add(word);
            if (word.Direction == Direction.Vertical)
            {
                int span = height;
                for (int i = 0; i < word.CharacterList.Count; i++)
                {
                    Grid grid = crozzle.CrozzleArray[span][width];
                    grid.Character = word.CharacterList[i].Alphabetic;
                    word.CharacterList[i].Position = new Position() {Height = span, Width = width};
                    grid.HorizontalWord = word;
                    grid.SpannableDirection = (grid.SpannableDirection == Direction.All)
                        ? Direction.Horizontal
                        : Direction.None;
                    span++;
                }
            }
            else
            {
                int span = width;
                for (int i = 0; i < word.CharacterList.Count; i++)
                {
                    Grid grid = crozzle.CrozzleArray[span][height];
                    grid.Character = word.CharacterList[i].Alphabetic;
                    word.CharacterList[i].Position = new Position() { Height = height, Width = span };
                    grid.HorizontalWord = word;
                    grid.SpannableDirection = (grid.SpannableDirection == Direction.All)
                        ? Direction.Horizontal
                        : Direction.None;
                    span++;
                }
            }
            UpdateGrid( word,  width,  height);
        }

        /// <summary>
        /// EEEEEEEE
        /// EXWORDXE
        /// EEEEEEEE
        /// </summary>
        void UpdateHeadAndTailGrid(Word word, int width, int height)
        {
            Grid headGrid;
            Grid tailGrid;
            if (word.Direction == Direction.Vertical)
            {
                headGrid = crozzle.CrozzleArray[width][height - 1];
                tailGrid = crozzle.CrozzleArray[width][height + word.CharacterList.Count];
            }
            else
            {
                headGrid = crozzle.CrozzleArray[width - 1][height];
                tailGrid = crozzle.CrozzleArray[width + word.CharacterList.Count][height];
            }
            headGrid.SpannableDirection = tailGrid.SpannableDirection = Direction.None;
        }
        /// <summary>
        /// EEEEEEEE
        /// EXXXXXXE
        /// EEWORDEE
        /// EXXXXXXE
        /// EEEEEEEE
        /// </summary>
        void UpdateSurroundedGrids(Word word, int width, int height)
        {
            for (int i = -1; i < 2; i++)
            {
                if (i == 0) continue;
                int span = 0;
                for (int j = 0; j < word.CharacterList.Count; j++)
                {
                    if (word.Direction == Direction.Vertical)
                    {
                        Grid grid = crozzle.CrozzleArray[width + i][height + j];
                        grid.SpannableDirection = (grid.SpannableDirection == Direction.All)
                            ? Direction.Vertical
                            : Direction.None;
                        span++;
                    }
                    else
                    {
                        Grid grid = crozzle.CrozzleArray[width + j][height + i];
                        grid.SpannableDirection = (grid.SpannableDirection == Direction.All)
                            ? Direction.Horizontal
                            : Direction.None;
                        span++;
                    }
                }
            }
        }

        void UpdateGrid(Word word, int width, int height)
        {
            switch (difficulty)
            {
                case Difficulty.Easy:
                    UpdateHeadAndTailGrid(word, width,  height);
                    goto case Difficulty.Medium;
                case Difficulty.Medium:
                case Difficulty.Hard:
                case Difficulty.Extreme:
                    UpdateHeadAndTailGrid(word, width, height);
                    UpdateSurroundedGrids( word,  width,  height);
                    break;
            }
        }
    }
}