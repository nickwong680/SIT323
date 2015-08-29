using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIT323.Models
{
    public class Word
    {
        public bool IsValid { get; set; }
        public int Score { get; set; }
        public Direction Direction { get; set; }
        public List<Character> CharacterList { get; set; }
        public List<Word> IntersectWords { get; set; }

        public Word(Direction d)
        {
            Direction = d;
            CharacterList = new List<Character>();
            IntersectWords = new List<Word>();
        }

        public Word(Direction d, Character c)
            : this(d)
        {
            CharacterList.Add(c);
        }

        public override string ToString()
        {
            return String.Format("{0} : {1}", String.Join("", CharacterList.Select(a => a.Alphabetic).ToArray()), Direction);
        }
    }

    public class Character
    {
        public char Alphabetic;
        public Position Position;
        public int Score;

        public Character(char alphabetic, Position position)
        {
            Alphabetic = alphabetic;
            Position = position;
        }
        public Character(char alphabetic, int score)
        {
            Alphabetic = alphabetic;
            Score = score;
        }
    }

    public struct Position
    {
        public int Width;
        public int Height;
    }

    public enum Direction
    {
        Horizontal,
        Vertical
    }
}
