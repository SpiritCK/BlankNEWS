using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace BlankNEWS.Models
{
    public class StringMatch
    {
        private string text;
        private string pattern;
        private string textInLowerCase;
        private int[] failureTable;

        public void makeFailureTable()
        {
            failureTable = new int[pattern.Length];
            failureTable[0] = 0;
            int currentLength = 0;
            int index = 1; //start index

            while (index < pattern.Length)
            {
                if (pattern[index] == pattern[currentLength])
                {
                    currentLength++;
                    failureTable[index] = currentLength;
                    index++;
                }
                else
                {
                    if (currentLength != 0)
                    {
                        currentLength = failureTable[currentLength - 1];
                    }
                    else
                    {
                        failureTable[index] = 0;
                        index++;
                    }
                }
            }
        }

        public StringMatch(string _text, string _pattern)
        {
            text = _text;
            textInLowerCase = _text.ToLower();

            pattern = _pattern.ToLower();

            makeFailureTable();
        }

        public string KMPsearch()
        {
            int index_text = 0;
            int index_pattern = 0;
            string answer = "";

            while (index_text < text.Length)
            {
                if (pattern[index_pattern] == textInLowerCase[index_text])
                {
                    index_text = index_text + 1;
                    index_pattern = index_pattern + 1;
                }

                if (index_pattern == pattern.Length)
                {
                    int awal = index_text - pattern.Length;
                    int akhir = index_text;

                    while (textInLowerCase[awal] != '.' && textInLowerCase[awal] != '?' && textInLowerCase[awal] != '!' && awal > 0)
                    {
                        awal--;
                    }

                    while (textInLowerCase[akhir] != '.' && textInLowerCase[akhir] != '?' && textInLowerCase[akhir] != '!' && akhir < textInLowerCase.Length - 1)
                    {
                        akhir++;
                    }

                    answer = text.Substring(awal, akhir - awal + 1);

                    return answer;
                }
                else if (index_text < textInLowerCase.Length && pattern[index_pattern] != textInLowerCase[index_text])
                {
                    if (index_pattern != 0)
                    {
                        index_pattern = failureTable[index_pattern - 1];
                    }
                    else
                    {
                        index_text = index_text + 1;
                    }
                }
            }

            return answer;
        }

        private static void settingUp(string pattern, int size, ref int[] lambda)
        {
            int i;
            for (i = 0; i < 256; i++)
            {
                lambda[i] = -1;
            }
            for (i = 0; i < size; i++)
            {
                lambda[(int)pattern[i]] = i;
            }
        }

        public static int[] searchString(string text, string pattern)
        {
            List<int> retVal = new List<int>();
            int m = pattern.Length;
            int n = text.Length;
            int[] heuristic = new int[256];
            settingUp(pattern, m, ref heuristic);
            int s = 0;
            while (s <= (n - m))
            {
                int j = m - 1;
                while (j >= 0 && pattern[j] == text[s + j])
                {
                    --j;
                }
                if (j < 0)
                {
                    retVal.Add(s);
                    if (s + m < n)
                    {
                        s = s + m - heuristic[text[s + m]];
                    }
                    else
                    {
                        s = s + 1;
                    }
                }
                else
                {
                    s += Math.Max(1, j - heuristic[text[s + j]]);
                }
            }
            return retVal.ToArray();
        }

        public static void convertToString(string text, string pattern, ref string output, int choice)
        {
            int[] found = new int[1];

            if (choice == 2)
            {
                found = searchString(text.ToLower(), pattern.ToLower());
            }

            if (found == null || found.Length == 0)
            {
                output = "";

            }
            else
            {
                List<char> result = new List<char>();
                bool dot = false;
                int i;
                i = found[0];
                while (!dot && i != 0)
                {
                    if (text[i] != '.' && text[i] != '?' && text[i] != '!')
                    {
                        i = i - 1;
                    }
                    else
                    {
                        dot = true;
                    }
                }
                dot = false;
                if (i != 0)
                {
                    i = i + 2;
                }
                while (!dot)
                {
                    if (text[i] != '.' && text[i] != '?' && text[i] != '!')
                    {
                        result.Add(text[i]);
                        i++;
                    }
                    else
                    {
                        dot = true;
                    }
                }
                output = string.Join("", result.ToArray());
            }

        }

        public string BoyerMooreSearch()
        {
            String answer = "";
            convertToString(text, pattern, ref answer, 2);
            return answer;
        }

        public string RegexSearch()
        {
            string[] parseResult = pattern.Split(null);
            int count = 0;
            int smallestIndex = textInLowerCase.Length - 1;

            for (int i = 0; i < parseResult.Length; i++)
            {
                Match match = Regex.Match(textInLowerCase, parseResult[i]);
                if (match.Success)
                {
                    count++;
                    if (match.Index < smallestIndex)
                    {
                        smallestIndex = match.Index;
                    }
                }
            }


            if (count == parseResult.Length)
            {
                int awal = smallestIndex;
                int akhir = smallestIndex;
                while (textInLowerCase[awal] != '.' && textInLowerCase[awal] != '?' && textInLowerCase[awal] != '!' && awal > 0)
                {
                    awal--;
                }

                while (textInLowerCase[akhir] != '.' && textInLowerCase[akhir] != '?' && textInLowerCase[akhir] != '!' && akhir < textInLowerCase.Length - 1)
                {
                    akhir++;
                }

                string answer = text.Substring(awal, akhir - awal + 1);

                return answer;
            }
            else
            {
                return "";
            }
        }
    }

    class Test
    {
        static void Main(string[] args)
        {
            string text = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged.";

            string pattern = "has";

            StringMatch testSearch = new StringMatch(text, pattern);
            Console.WriteLine(testSearch.KMPsearch());
            Console.WriteLine(testSearch.RegexSearch());
            Console.WriteLine(testSearch.BoyerMooreSearch());
            //Tinggal pake tiga method ini untuk dapetin string nya
            Console.ReadKey();
        }
    }
}
