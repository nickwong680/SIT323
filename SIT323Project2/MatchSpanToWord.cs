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
        PreComplexMatch,
        CenterComplexMatch,
        PostComplexMatch,
        Simple = PreMatch | CenterMatch | PostMatch,
        Complex = PreComplexMatch | CenterComplexMatch | PostComplexMatch
    }

    internal class WordMatch
    {
        private MatchType _matchType;
        private Span _span;
        private string _word;
        private int _matchIndex;
        private string _regexMatched;


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

        public WordMatch(MatchType matchType, Span span, string word, int matchIndex, string regexMatched)
        {
            _matchIndex = matchIndex;
            _span = span;
            _word = word;
            _matchType = matchType;
            _regexMatched = regexMatched;
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

        private List<string> _ComplexMatchRegexStringBuilder(List<Char> placeable)
        {
            List<string> strList = new List<string>();

            bool beginWithSpace = true;
            foreach (char c in placeable)
            {
                if (beginWithSpace == true && c == default(char))
                {
                    strList.Add(@"\w?");
                }
                else
                {
                    beginWithSpace = false;
                    strList.Add((c == '\0') ? @"\w" : c.ToString());
                }
            }
            return strList;
        }

        private List<WordMatch> ComplexMatches(Span span)
        {
            string regexPattern = "^";
            MatchType matchType;
            if (span.PreCharacterPlaceable.Count != 0 && span.PostCharacterPlaceable.Count != 0)
            {
                regexPattern += String.Join("", _ComplexMatchRegexStringBuilder(span.PreCharacterPlaceable));

                regexPattern += string.Format("({0})", span.Character.Alphabetic);

                var reversedCopy = new List<char>(span.PostCharacterPlaceable);
                reversedCopy.Reverse();
                var reversedCopyBuilded = _ComplexMatchRegexStringBuilder(reversedCopy);
                reversedCopyBuilded.Reverse();
                var reversedString = String.Join("", reversedCopyBuilded);
                regexPattern += reversedString;

                matchType = MatchType.CenterComplexMatch;

            }
            else if (span.PreCharacterPlaceable.Count != 0)
            {
                regexPattern += String.Join("", _ComplexMatchRegexStringBuilder(span.PreCharacterPlaceable));
                regexPattern += string.Format("({0})", span.Character.Alphabetic);
                
                matchType = MatchType.PreComplexMatch;
            }
            else
            {
                regexPattern += string.Format("({0})", span.Character.Alphabetic);

                var reversedCopy = new List<char>(span.PostCharacterPlaceable);
                reversedCopy.Reverse();
                var reversedCopyBuilded = _ComplexMatchRegexStringBuilder(reversedCopy);
                reversedCopyBuilded.Reverse();
                var reversedString = String.Join("", reversedCopyBuilded);
                regexPattern += reversedString;

                matchType = MatchType.PostComplexMatch;
            }
            regexPattern += "$";
            return MatchRegex(span, regexPattern, matchType);
            

        }

        public List<WordMatch> SimpleMatch(Span span)
        {
            string regexPattern;
            MatchType matchType;

            if (span.PreCharacterPlaceable.Count != 0 && span.PostCharacterPlaceable.Count != 0)
            {
                regexPattern = string.Format(@"^\w{{0,{0}}}({1})\w{{0,{2}}}$",
                    span.PreCharacterPlaceable.Count,
                    span.Character.Alphabetic,
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
                    span.Character.Alphabetic);
                matchType = MatchType.PreMatch;
            }
            else
            {
                regexPattern = string.Format(@"^({0})\w{{0,{1}}}$",
                    span.Character.Alphabetic,
                    span.PostCharacterPlaceable.Count);
                matchType = MatchType.PostMatch;
            }

            return MatchRegex(span, regexPattern, matchType);
        }

        public List<WordMatch> MatchRegex(Span span, string regexPattern, MatchType matchType)
        {
            if (regexPattern == @"^\\w?\\w?\\w?\\w?TO\\w?\\w?\\w?\\w?$")
            {
                var tt = 1;
            }
            if (matchType == MatchType.CenterComplexMatch)
            {
                var tt = 1;
            }

            List<WordMatch> matchList = new List<WordMatch>();

            Regex regex = new Regex(regexPattern);
            foreach (string word in _wordsNotAddedList)
            {
                var wordMatch = regex.Match(word);
                if (wordMatch.Success)
                {
                    matchList.Add(new WordMatch(matchType, span, word, wordMatch.Groups[1].Index, regexPattern));
                }
            }
            return matchList;
        }
    }
}