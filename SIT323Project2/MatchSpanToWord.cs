using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SIT323.Models;
using SIT323Project2.Models;

namespace SIT323Project2
{
    [Flags]
    internal enum MatchType
    {
        PreMatch,
        CenterMatch,
        PostMatch,
        Simple = PreMatch | CenterMatch | PostMatch
    }

    internal class WordMatch
    {
        private MatchType _matchType;
        private Span _span;
        private string _word;
        private int _matchIndex;

        public MatchType MatchType
        {
            get { return _matchType; }
        }

        public Span Span
        {
            get { return _span; }
        }

        public string Word
        {
            get { return _word; }
        }

        public int MatchIndex
        {
            get { return _matchIndex; }
        }

        public WordMatch(MatchType matchType, Span span, string word, int matchIndex)
        {
            _matchIndex = matchIndex;
            _span = span;
            _word = word;
            _matchType = matchType;
        }
    }

    internal class MatchSpanToWord
    {
        private int _maxWordLength;
        private readonly List<Span> _spans;
        private readonly List<string> _wordsNotAddedList;

        public MatchSpanToWord(List<Span> spans, List<string> wordsNotAddedList)
        {
            _spans = spans;
            _wordsNotAddedList = wordsNotAddedList;
            _maxWordLength = wordsNotAddedList.FirstOrDefault().Count();
        }

        public List<WordMatch> Match()
        {
            List<WordMatch> matches = new List<WordMatch>();
            foreach (Span span in _spans)
            {
                if (span.Character.ToString() == "DASH")
                {
                    var tt = 3;
                }

                var simple =
                    (String.Join("", span.PreCharacterPlaceable) + String.Join("", span.PostCharacterPlaceable)).Replace
                        ("\0", "");
                if (String.IsNullOrEmpty(simple))
                {
                    matches.AddRange(SimpleMatche(span));
                }
                else
                {
                    matches.AddRange(ComplexMatches(span));
                }
            }

            return matches;
        }

        private List<WordMatch> ComplexMatches(Span span)
        {
            List<WordMatch> matches = new List<WordMatch>();

            string regexPattern;
            MatchType matchType;


        }

        public List<WordMatch> SimpleMatche(Span span)
        {
            List<WordMatch> matches = new List<WordMatch>();

            string regexPattern;
            MatchType matchType;

            if (span.PreCharacterPlaceable.Count != 0 && span.PostCharacterPlaceable.Count != 0)
            {
                regexPattern = string.Format(@"^\w{{0,{0}}}({1})\w{{0,{2}}}$",
                    span.PreCharacterPlaceable.Count,
                    span.Character,
                    span.PostCharacterPlaceable.Count);
                matchType = MatchType.CenterMatch;
            }
            else if (span.PreCharacterPlaceable.Count != 0)
            {
//                    matches.AddRange(
//                        _wordsNotAddedList.Where(w => 
//                            w.StartsWith(span.Character.ToString()) && 
//                            w.Length < maxLength));
                regexPattern = string.Format(@"^\w{{0,{0}}}({1})$",
                    span.PreCharacterPlaceable.Count,
                    span.Character);
                matchType = MatchType.PreMatch;
            }
            else
            {
                regexPattern = string.Format(@"^({0})\w{{0,{1}}}$",
                    span.Character,
                    span.PostCharacterPlaceable.Count);
                matchType = MatchType.PostMatch;
            }


            Regex regex = new Regex(regexPattern);
            foreach (string word in _wordsNotAddedList)
            {
                var wordMatch = regex.Match(word);
                if (wordMatch.Success)
                {
                    matches.Add(new WordMatch(matchType, span, word, wordMatch.Groups[1].Index));
                }

//                    MatchCollection wordMatch = regex.Matches(word);
//                    if (wordMatch.Count > 0)
//                    {
//                        foreach (Match m in wordMatch)
//                        {
//                            matches.Add(new WordMatch(matchType, span, word, m.Index));
//                        }
//                    }
            }
            return matches;
        }
    }
}