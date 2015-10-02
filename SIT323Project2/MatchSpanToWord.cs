using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIT323.Models;
using SIT323Project2.Models;

namespace SIT323Project2
{
    [Flags]
    enum MatchType
    {
        Left,
        Center,
        Right,
        Simple = Left | Center | Right,
    }
    class Match
    {
        private Span _span;
        private Position _position;
        private Word _word;
        private MatchType _matchType;
    }
    class MatchSpanToWord
    {
        private List<Span> _spans;
        private List<string> _wordsNotAddedList;
        private int _maxWordLength;

        public MatchSpanToWord(List<Span> spans, List<string> wordsNotAddedList)
        {
            _spans = spans;
            _wordsNotAddedList = wordsNotAddedList;
            _maxWordLength = wordsNotAddedList.FirstOrDefault().Count();
            SimpleMatches();
        }

        private void ComplexMatches()
        {
            //Left 

            //Center

            //Right


        }

        private void SimpleMatches()
        {
            List<string> matches = new List<string>();
            foreach (Span span in _spans)
            {
                if (span.PreCharacterPlaceable.Count == 0)
                {
                    var tt = _wordsNotAddedList.Where(w => w.StartsWith(span.Character.ToString()));
                }
                else if (span.PostCharacterPlaceable.Count == 0)
                {

                }
                else
                {
                    var tt = _wordsNotAddedList.Where(w => w.StartsWith(span.Character.ToString()));

                }
            }
            //Left 

            //Center

            //Right


        }

    }
}
