using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructures
{
    public interface IStringSearchAlgorithm

    {
        IEnumerable<ISearchMatch> Search(string pattern, string toSerach);
    }

    public interface ISearchMatch
    {
        public int Start { get; }

        public int Length { get; }
    }

    public class StringSearchMatch : ISearchMatch
    {
        private int _start;

        private int _length;

        public StringSearchMatch(int start, int length)
        {
            _start = start;
            _length = length;
        }

        public int Start { get { return _start; } }

        public int Length { get { return _length; } }
    }

    public class NaiveSearch : IStringSearchAlgorithm
    {
        public IEnumerable<ISearchMatch> Search(string pattern, string toSerach)
        {
            var matchCount = 0;
            for (int i = 0; i < toSerach.Length - pattern.Length; i++)
            {
                while (toSerach[i + matchCount] == pattern[matchCount])
                {
                    matchCount++;
                    if (pattern.Length == matchCount)
                    {
                        yield return new StringSearchMatch(i, matchCount);
                        i += matchCount - 1;
                    }
                }
            }
        }
    }

    public class BadMatchTable
    {
        readonly int missing;
        readonly Dictionary<int, int> table;

        public BadMatchTable(string pattern)
        {
            missing = pattern.Length;
            table = new Dictionary<int, int>();
            for (int i = 0; i < pattern.Length - 1; i++)
            {
                table[pattern[i]] = pattern.Length - i - 1;
            }
        }

        public int this[int index]
        {
            get { return table.GetValueOrDefault(index, missing); }
        }

    }

    public class BoyerSearch : IStringSearchAlgorithm
    {
        public IEnumerable<ISearchMatch> Search(string pattern, string toSearch)
        {

            if (pattern == null)
            {
                throw new ArgumentNullException("ToFind");
            }
            if (toSearch == null)
            {
                throw new ArgumentNullException("toSearch");
            }

            if (pattern.Length>0 && toSearch.Length > 0)
            {
                var badMatchTable = new BadMatchTable(pattern);
                int currentIndex = 0;
                while (currentIndex <= toSearch.Length-pattern.Length)
                {
                    int charLeft = pattern.Length - 1;
                    while (charLeft>=0 && pattern[charLeft] == toSearch[currentIndex+charLeft])
                    {
                        charLeft--;
                    }

                    if (charLeft < 0)
                    {
                        yield return new StringSearchMatch(currentIndex, pattern.Length);
                        currentIndex += pattern.Length;
                    }
                    else
                    {
                        currentIndex += badMatchTable[toSearch[currentIndex + pattern.Length - 1]];   
                    }
                }
            }
        }

        public static class StringReplace
        {
            public static string Replace(IStringSearchAlgorithm alg, string input, string pattern, string replace)
            {
                var strBuilder = new StringBuilder();
                int previousStart = 0;
                foreach (var match in alg.Search(pattern, input))
                {
                    strBuilder.Append(input.Substring(previousStart, match.Start - previousStart));
                    strBuilder.Append(replace);

                    previousStart = match.Start + match.Length;
                }
                strBuilder.Append(input.Substring(previousStart));
                return strBuilder.ToString();

            }
        }
    }
}
