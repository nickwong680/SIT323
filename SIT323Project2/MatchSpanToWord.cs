using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SIT323.Models;
using SIT323Project2.Models;

namespace SIT323Project2
{
    [Flags]
    internal enum MatchType : byte
    {
        PreMatch = 0,
        CenterMatch = 1 << 0,
        PostMatch =1 << 1,
        PreComplexMatch =1 << 2,
        CenterComplexMatch =1 << 3,
        PostComplexMatch=1 << 4,

//        Simple = PreMatch | CenterMatch | PostMatch,
//        Complex = PreComplexMatch | CenterComplexMatch | PostComplexMatch,
//
//   
    }

    internal class WordMatch
    {
        private MatchType _matchType;
        private Span _span;
        private string _word;
        private int _matchIndex;
        private string _regexMatched;
        private int _point;

        public int Point
        {
            get { return _point; }
        }

        public string RegexMatched
        {
            get { return _regexMatched; }
        }

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

        public WordMatch(MatchType matchType, Span span, string word, int matchIndex, string regexMatched, int point)
        {
            _matchIndex = matchIndex;
            _span = span;
            _word = word;
            _matchType = matchType;
            _regexMatched = regexMatched;
            _point = point;
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
        

                var simple =
                    (String.Join("", span.PreCharacterPlaceable) + String.Join("", span.PostCharacterPlaceable)).Replace
                        ("\0", "");
                if (!String.IsNullOrEmpty(simple))
                {
                    matches.AddRange(ComplexMatches(span));
                }
                else
                {
                    matches.AddRange(SimpleMatch(span));
                }
            }

            return matches;
        }

        private List<string> _ComplexMatchRegexStringAndPointBuilder(List<Character> placeable, ref int point)
        {
            List<string> strList = new List<string>();

            bool beginWithSpace = true;
            foreach (Character c in placeable)
            {
                if (beginWithSpace == true && c.Alphabetic == default(char))
                {
                    strList.Add(@"\w?");
                }
                else
                {
                    beginWithSpace = false;
//                    strList.Add((c == '\0') ? @"\w" : c.ToString());
                    if (c.Alphabetic == '\0')
                    {
                        strList.Add(@"\w");
                    }
                    else
                    {
                        strList.Add(c.Alphabetic.ToString());
                        point += c.Score;
                    }
                }
            }
            return strList;
        }

        private List<WordMatch> ComplexMatches(Span span)
        {
            int point = 0;
            string regexPattern = "^";
            MatchType matchType;
            if (span.PreCharacterPlaceable.Count != 0 && span.PostCharacterPlaceable.Count != 0)
            {
                regexPattern += String.Join("", _ComplexMatchRegexStringAndPointBuilder(span.PreCharacterPlaceable, ref point));

                regexPattern += string.Format("({0})", span.Character.Alphabetic);
                point += span.Character.Score;

                var reversedCopy = new List<Character>(span.PostCharacterPlaceable);
                reversedCopy.Reverse();
                var reversedCopyBuilded = _ComplexMatchRegexStringAndPointBuilder(reversedCopy, ref point);
                reversedCopyBuilded.Reverse();
                var reversedString = String.Join("", reversedCopyBuilded);
                regexPattern += reversedString;

                matchType = MatchType.CenterComplexMatch;

            }
            else if (span.PreCharacterPlaceable.Count != 0)
            {
                regexPattern += String.Join("", _ComplexMatchRegexStringAndPointBuilder(span.PreCharacterPlaceable, ref point));
                regexPattern += string.Format("({0})", span.Character.Alphabetic);
                point += span.Character.Score;

                matchType = MatchType.PreComplexMatch;
            }
            else
            {
                regexPattern += string.Format("({0})", span.Character.Alphabetic);
                point += span.Character.Score;

                var reversedCopy = new List<Character>(span.PostCharacterPlaceable);
                reversedCopy.Reverse();
                var reversedCopyBuilded = _ComplexMatchRegexStringAndPointBuilder(reversedCopy, ref point);
                reversedCopyBuilded.Reverse();
                var reversedString = String.Join("", reversedCopyBuilded);
                regexPattern += reversedString;

                matchType = MatchType.PostComplexMatch;
            }
            regexPattern += "$";
         
            return MatchRegex(span, regexPattern, matchType, point);
            

        }

        public List<WordMatch> SimpleMatch(Span span)
        {
            string regexPattern;
            MatchType matchType;
            int point = 0;

            if (span.PreCharacterPlaceable.Count != 0 && span.PostCharacterPlaceable.Count != 0)
            {
                regexPattern = string.Format(@"^\w{{0,{0}}}({1})\w{{0,{2}}}$",
                    span.PreCharacterPlaceable.Count,
                    span.Character.Alphabetic,
                    span.PostCharacterPlaceable.Count);
                matchType = MatchType.CenterMatch;
                point += span.Character.Score;
            }
            else if (span.PreCharacterPlaceable.Count != 0)
            {
//                    matches.AddRange(
//                        _wordsNotAddedList.Where(w => 
//                            w.StartsWith(span.Character.ToString()) && 
//                            w.Length < maxLength));
                regexPattern = string.Format(@"^\w{{0,{0}}}({1})$",
                    span.PreCharacterPlaceable.Count,
                    span.Character.Alphabetic);
                matchType = MatchType.PreMatch;
                point += span.Character.Score;

            }
            else
            {
                regexPattern = string.Format(@"^({0})\w{{0,{1}}}$",
                    span.Character.Alphabetic,
                    span.PostCharacterPlaceable.Count);
                matchType = MatchType.PostMatch;
                point += span.Character.Score;

            }

            return MatchRegex(span, regexPattern, matchType, point);
        }

        public List<WordMatch> MatchRegex(Span span, string regexPattern, MatchType matchType, int point)
        {
            List<WordMatch> matchList = new List<WordMatch>();

            Regex regex = new Regex(regexPattern);
            foreach (string word in _wordsNotAddedList)
            {
                var wordMatch = regex.Match(word);
                if (wordMatch.Success)
                {
                    if (matchType.HasFlag(MatchType.CenterComplexMatch))
                    {
                        var tt = 0;
                    }
                    matchList.Add(new WordMatch(matchType, span, word, wordMatch.Groups[1].Index, regexPattern, point));
                }
            }
            return matchList;
        }

        public List<WordMatch> MatchAndOrderByPints()
        {
            return Match().OrderByDescending(m => m.Point).ToList();
        }
    }
}