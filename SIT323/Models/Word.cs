using System;
using System.Collections.Generic;
using System.Linq;

namespace SIT323.Models
{
    #region Word Class

    /// <summary>
    ///     Word class for each Crozzle word
    /// </summary>
    public class Word
    {
        public Word(string character)
            : this(Direction.None)
        {
            foreach (char c in character)
            {
                CharacterList.Add(new Character(c));
            }
        } 
        /// <summary>
        ///     Default constructor
        /// </summary>
        /// <param name="direction">direction of word</param>
        public Word(Direction direction)
        {
            Direction = direction;
            CharacterList = new List<Character>();
            IntersectWords = new List<Word>();
        }

        /// <summary>
        ///     Constructor that takes the first the chracter of the word and the direction
        /// </summary>
        /// <param name="direction">direction of word</param>
        /// <param name="character">character </param>
        public Word(Direction direction, Character character)
            : this(direction)
        {
            CharacterList.Add(character);
        }
        /// <summary>
        /// Constructor to take direction and full string
        /// </summary>
        /// <param name="direction">direction of word</param>
        /// <param name="character">full string</param>
        public Word(Direction direction, string character) 
            : this(direction)
        {
            foreach (char c in character)
            {
                CharacterList.Add(new Character(c));
            }
        }

        /// <summary>
        ///     Determines if word is valued after validatation
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        ///     Number of score this word is calculated from Score class based on Crozzle difficulty level
        /// </summary>
        public int Score { get; set; }

        public Direction Direction { get; set; }

        /// <summary>
        ///     List of alphabet character that made up this word
        /// </summary>
        public List<Character> CharacterList { get; set; }

        /// <summary>
        ///     List of any words that intersect this word
        /// </summary>
        public List<Word> IntersectWords { get; set; }

        /// <summary>
        ///     This property is created solely for DFS sorting algorithm
        /// </summary>
        public bool Visited { get; set; }

        /// <summary>
        ///     Overrides to show each alphabet concatenated
        /// </summary>
        /// <returns>strings of all alphabet</returns>
        public override string ToString()
        {
            return string.Format("{0}", string.Join("", CharacterList.Select(a => a.Alphabetic).ToArray()));
            //return String.Format("{0} : {1}", String.Join("", CharacterList.Select(a => a.Alphabetic).ToArray()), Direction);
        }
    }

    #endregion

    #region Character Class

    /// <summary>
    ///     Character class
    /// </summary>
    public class Character
    {
        public override string ToString()
        {
            return Alphabetic.ToString();
        }

        /// <summary>
        ///     the alphabetic character in char
        /// </summary>
        public char Alphabetic;

        /// <summary>
        ///     Position this character in the array
        /// </summary>
        public Position Position;

        /// <summary>
        ///     Number of score this character is calculated from Score class based on Crozzle difficulty level
        ///     Score are used as intersection points in Project 2
        /// </summary>
        public int Score;

        public Character(char alphabetic)
        {
            Alphabetic = alphabetic;
        }
        /// <summary>
        ///     Constructor takes in alphabetic and positon
        /// </summary>
        /// <param name="alphabetic">alphabetic char</param>
        /// <param name="position">position enum</param>
        public Character(char alphabetic, Position position) : 
            this (alphabetic)
        {
            Position = position;
        }

        /// <summary>
        ///     Constructor takes in alphabetic and score
        /// </summary>
        /// <param name="alphabetic">alphabetic char</param>
        /// <param name="score">score int</param>
        public Character(char alphabetic, int score):
             this (alphabetic)

        {
            Score = score;
        }
    }

    #endregion

    #region Position struct

    /// <summary>
    ///     Struct to hold the height and width of the position
    /// </summary>
    public struct Position
    {
        public int Height;
        public int Width;
    }

    #endregion

    #region Direction enum

    /// <summary>
    ///     enum of Direction in Horizontal or Vertical
    /// </summary>
    [Flags]
    public enum Direction
    {
        None = 0,
        Horizontal = 1,
        Vertical = 2,
        All = Horizontal | Vertical,
    }

    #endregion
}