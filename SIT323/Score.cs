using System;
using System.Collections.Generic;
using System.Linq;
using SIT323.Models;

namespace SIT323
{
    /// <summary>
    ///     PointScore struct to hold point of cell
    /// </summary>
    public struct PointScore
    {
        public int Height;
        public int Point;
        public int Width;
    }

    /// <summary>
    ///     Score class give points to words according to PointScheme defined in difficulty level of Crozzle
    /// </summary>
    public class Score
    {
        private readonly PointScheme _pointScheme;
        private readonly List<Character> _pointsList;
        private readonly List<Word> _wordsFromCrozzle;

        /// <summary>
        ///     Constructor for Score class.
        ///     Brings the list of valid words from Crozzle and score points to scope
        /// </summary>
        /// <param name="wlist">Generic List of Word </param>
        /// <param name="points">>Generic List of Character</param>
        /// <param name="ps">PointScheme enum</param>
        public Score(List<Word> wlist, List<Character> points, PointScheme ps)
        {
            _pointScheme = ps;
            _wordsFromCrozzle = wlist;
            _pointsList = points;
            ApplyPoints();
        }

        /// <summary>
        ///     public get porperty of Total score of word
        /// </summary>
        public int TotalScore { get; private set; }

        /// <summary>
        ///     Calauate points.
        ///     Method could use some refactoring.
        /// </summary>
        private void ApplyPoints()
        {
            var pointSet = new HashSet<PointScore>();
            var pointList = new List<PointScore>();

            foreach (var word in _wordsFromCrozzle.Where(v => v.IsValid))
            {
                TotalScore += (_pointScheme == PointScheme.IncrementalWithBonusPerWord ||
                               _pointScheme == PointScheme.CustomWithBonusPerIntersection)
                    ? 10
                    : 0;
                word.CharacterList.ForEach(
                    c =>
                    {
                        var point = _pointsList.FirstOrDefault(p => p.Alphabetic == c.Alphabetic);
                        c.Score = point.Score;
                        word.Score += point.Score;

                        var Point = new PointScore
                        {
                            Height = c.Position.Height,
                            Width = c.Position.Width,
                            Point = point.Score
                        };
                        pointSet.Add(Point);
                        if (_pointScheme == PointScheme.CustomWithBonusPerIntersection) pointList.Add(Point);
                    });
            }
            if (_pointScheme == PointScheme.CustomWithBonusPerIntersection)
            {
                var dupes = pointList.GroupBy(w => w)
                    .Where(group => group.Count() > 1)
                    .Select(group => group.Key);
                foreach (var pointScore in dupes)
                {
                    TotalScore += pointScore.Point;
                }
            }
            else
            {
                foreach (var pointScore in pointSet)
                {
                    TotalScore += pointScore.Point;
                }
            }
        }

        public static List<Character> PointsMatrix(PointScheme pointScheme)
        {
            var alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            var points = new List<Character>();

            switch (pointScheme)
            {
                case PointScheme.Incremental:
                case PointScheme.IncrementalWithBonusPerWord:
                    for (var i = 0; i < 26; i++)
                    {
                        points.Add(new Character(alphabet[i], i + 1));
                    }
                    break;
                case PointScheme.OneEach:
                    for (var i = 0; i < 26; i++)
                    {
                        points.Add(new Character(alphabet[i], 1));
                    }
                    break;
                case PointScheme.Custom:
                case PointScheme.CustomWithBonusPerIntersection:
                    var customPoints = new[] { 1, 2, 4, 8, 16, 32, 64 };
                    for (var i = 0; i < customPoints.Length; i++)
                    {
                        char[] chars = { };
                        switch (customPoints[i])
                        {
                            case 1:
                                chars = "AEIOU".ToCharArray();
                                break;
                            case 2:
                                chars = "BCDFG".ToCharArray();
                                break;
                            case 4:
                                chars = "HJKLM".ToCharArray();
                                break;
                            case 8:
                                chars = "NPQR".ToCharArray();
                                break;
                            case 16:
                                chars = "STV".ToCharArray();
                                break;
                            case 32:
                                chars = "WXY".ToCharArray();
                                break;
                            case 64:
                                chars = "Z".ToCharArray();
                                break;
                        }
                        Array.ForEach(chars, a => points.Add(new Character(a, customPoints[i])));
                    }
                    break;
            }
            return points;
        }

        /// <summary>
        ///     Uses factory pattern for creating points base on alphabet character determined by scheme
        ///     and constructs the Score class.
        /// </summary>
        /// <param name="wordlist">Generic List of Words</param>
        /// <param name="point">Scheme of points</param>
        /// <returns></returns>
        public static Score PointsFactory(List<Word> wlist, PointScheme pointScheme)
        {
            return new Score(wlist, PointsMatrix(pointScheme), pointScheme);
        }
    }

    /// <summary>
    ///     PointScheme reflects on the difficulty level of Crozzle
    /// </summary>
    public enum PointScheme
    {
        OneEach,
        Incremental,
        IncrementalWithBonusPerWord,
        Custom,
        CustomWithBonusPerIntersection
    }
}