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
        PostMatch = 1 << 1,
        PreComplexMatch = 1 << 2,
        CenterComplexMatch = 1 << 3,
        PostComplexMatch = 1 << 4

//        Simple = PreMatch | CenterMatch | PostMatch,
//        Complex = PreComplexMatch | CenterComplexMatch | PostComplexMatch,
//
//   
    }

    internal class WordMatch
    {
        public WordMatch(MatchType matchType, SpanWithCharater spanWithCharater, string word, int matchIndex,
            string regexMatched, int point)
        {
            MatchIndex = matchIndex;
            SpanWithCharater = spanWithCharater;
            Word = word;
            MatchType = matchType;
            RegexMatched = regexMatched;
            Point = point;
        }

        public int Point { get; private set; }

        public string RegexMatched { get; private set; }

        public MatchType MatchType { get; private set; }

        public SpanWithCharater SpanWithCharater { get; private set; }

        public string Word { get; private set; }

        public int MatchIndex { get; private set; }
    }

    internal class MatchSpanToWord
    {
        private readonly List<SpanWithCharater> _spans;
        private readonly List<string> _wordsNotAddedList;
        private int _maxWordLength;

        public MatchSpanToWord(List<SpanWithCharater> spans, List<string> wordsNotAddedList)
        {
            _spans = spans;
            _wordsNotAddedList = wordsNotAddedList;
            _maxWordLength = wordsNotAddedList.FirstOrDefault().Count();
        }

        public List<WordMatch> Match()
        {
            var matches = new List<WordMatch>();
            foreach (var span in _spans)
            {
                var simple =
                    (string.Join("", span.PreCharacterPlaceable) + string.Join("", span.PostCharacterPlaceable)).Replace
                        ("\0", "");
                if (!string.IsNullOrEmpty(simple))
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
            var strList = new List<string>();

            var beginWithSpace = true;
            foreach (var c in placeable)
            {
                if (beginWithSpace && c.Alphabetic == default(char))
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

        private List<WordMatch> ComplexMatches(SpanWithCharater spanWithCharater)
        {
            var point = 0;
            var regexPattern = "^";
            MatchType matchType;
            if (spanWithCharater.PreCharacterPlaceable.Count != 0 && spanWithCharater.PostCharacterPlaceable.Count != 0)
            {
                regexPattern += string.Join("",
                    _ComplexMatchRegexStringAndPointBuilder(spanWithCharater.PreCharacterPlaceable, ref point));

                regexPattern += string.Format("({0})", spanWithCharater.Character.Alphabetic);
                point += spanWithCharater.Character.Score;

                var reversedCopy = new List<Character>(spanWithCharater.PostCharacterPlaceable);
                reversedCopy.Reverse();
                var reversedCopyBuilded = _ComplexMatchRegexStringAndPointBuilder(reversedCopy, ref point);
                reversedCopyBuilded.Reverse();
                var reversedString = string.Join("", reversedCopyBuilded);
                regexPattern += reversedString;

                matchType = MatchType.CenterComplexMatch;
            }
            else if (spanWithCharater.PreCharacterPlaceable.Count != 0)
            {
                regexPattern += string.Join("",
                    _ComplexMatchRegexStringAndPointBuilder(spanWithCharater.PreCharacterPlaceable, ref point));
                regexPattern += string.Format("({0})", spanWithCharater.Character.Alphabetic);
                point += spanWithCharater.Character.Score;

                matchType = MatchType.PreComplexMatch;
            }
            else
            {
                regexPattern += string.Format("({0})", spanWithCharater.Character.Alphabetic);
                point += spanWithCharater.Character.Score;

                var reversedCopy = new List<Character>(spanWithCharater.PostCharacterPlaceable);
                reversedCopy.Reverse();
                var reversedCopyBuilded = _ComplexMatchRegexStringAndPointBuilder(reversedCopy, ref point);
                reversedCopyBuilded.Reverse();
                var reversedString = string.Join("", reversedCopyBuilded);
                regexPattern += reversedString;

                matchType = MatchType.PostComplexMatch;
            }
            regexPattern += "$";

            return MatchRegex(spanWithCharater, regexPattern, matchType, point);
        }

        public List<WordMatch> SimpleMatch(SpanWithCharater spanWithCharater)
        {
            string regexPattern;
            MatchType matchType;
            var point = 0;

            if (spanWithCharater.PreCharacterPlaceable.Count != 0 && spanWithCharater.PostCharacterPlaceable.Count != 0)
            {
                regexPattern = string.Format(@"^\w{{0,{0}}}({1})\w{{0,{2}}}$",
                    spanWithCharater.PreCharacterPlaceable.Count,
                    spanWithCharater.Character.Alphabetic,
                    spanWithCharater.PostCharacterPlaceable.Count);
                matchType = MatchType.CenterMatch;
                point += spanWithCharater.Character.Score;
            }
            else if (spanWithCharater.PreCharacterPlaceable.Count != 0)
            {
//                    matches.AddRange(
//                        _wordsNotAddedList.Where(w => 
//                            w.StartsWith(span.Character.ToString()) && 
//                            w.Length < maxLength));
                regexPattern = string.Format(@"^\w{{0,{0}}}({1})$",
                    spanWithCharater.PreCharacterPlaceable.Count,
                    spanWithCharater.Character.Alphabetic);
                matchType = MatchType.PreMatch;
                point += spanWithCharater.Character.Score;
            }
            else
            {
                regexPattern = string.Format(@"^({0})\w{{0,{1}}}$",
                    spanWithCharater.Character.Alphabetic,
                    spanWithCharater.PostCharacterPlaceable.Count);
                matchType = MatchType.PostMatch;
                point += spanWithCharater.Character.Score;
            }

            return MatchRegex(spanWithCharater, regexPattern, matchType, point);
        }

        public List<WordMatch> MatchRegex(SpanWithCharater spanWithCharater, string regexPattern, MatchType matchType,
            int point)
        {
            var matchList = new List<WordMatch>();

            var regex = new Regex(regexPattern);
            foreach (var word in _wordsNotAddedList)
            {
                var wordMatch = regex.Match(word);
                if (wordMatch.Success)
                {
                    matchList.Add(new WordMatch(matchType, spanWithCharater, word, wordMatch.Groups[1].Index,
                        regexPattern, point));
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