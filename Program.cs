using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Test01
{
    class Program
    {
        static void Main(string[] args)
        {
            //Enter your filepath here:
            string filePath = @"c:\\NET Test 00.txt";
            //Read the words into string array
            string[] listOfWords = ReadWordsToStringArray(filePath);
            //Get the longest word
            string longestWord = GetLongestWords(listOfWords);
            Console.WriteLine("Longest word in the file: \n" + longestWord + "\n");
            //Get the second longest word
            string secondLongestWord = GetSecondLongestWord(listOfWords, longestWord);
            Console.WriteLine("Second longest word in the file: \n" + secondLongestWord + "\n");
            //Get the total count
            int totalCount = GetTotalCount(listOfWords);
            Console.WriteLine("Total count of words in the list that can be constructed of other words: \n" + totalCount + "\n");

        }
        
        public static string[] ReadWordsToStringArray(string filePath)
        {
            string[] wordList = null;
            try
            {
                System.IO.StreamReader streamReader = new System.IO.StreamReader(filePath); //get the file
                string stringWithSpaces = streamReader.ReadToEnd(); //load file to string
                streamReader.Close();

                Regex r = new Regex(@"\r\n"); //specify delimiter: Here we are considering the next line
                wordList = r.Split(stringWithSpaces);
            }
            catch (Exception ex)
            {
                //write the exception to logs
                return wordList; //returning the null word list to continue 
            }

            return wordList;
        }
        public static string GetLongestWords(string[] listOfWords)
        {
            if (listOfWords == null) throw new ArgumentNullException("listOfWords");
            //Sort the words in descending order
            var sortedListOfWords = listOfWords.OrderByDescending(word => word.Length).ToList();
            //Get the first longest word (listed in descending order)
            return sortedListOfWords.FirstOrDefault(word => CheckMadeOfWords(word, sortedListOfWords));
        }
        private static bool CheckMadeOfWords(string word, ICollection<string> coll)
        {
            if (String.IsNullOrEmpty(word)) return false;
            if (word.Length == 1)
            {
                return coll.Contains(word);
            }
            foreach (var pair in GetPairs(word).Where(pair => coll.Contains(pair.Item1)))
            {
                return coll.Contains(pair.Item2) || CheckMadeOfWords(pair.Item2, coll);
            }
            return false;
        }
        public static int GetTotalCount(string[] listOfWords)
        {
            int count = 0;
            int counter = listOfWords.Length;
            string tempLongest = string.Empty;
            try
            {
                for (int i = 0; i < counter; i++)
                {
                    tempLongest = GetLongestWords(listOfWords);

                    if (string.IsNullOrEmpty(tempLongest) || listOfWords.Length >1)
                    {
                        break;
                    }
                    else
                    {
                        //replace the longest word with an empty string to find the second longest word
                        listOfWords = listOfWords.Select(x => x.Replace(tempLongest, string.Empty)).ToArray();
                        count++;
                        //Console.WriteLine(i + ",  " + count);
                    }
                }

            }
            catch (Exception ex)
            {
                //write the exception to log;
                return count; //retun zero
            }
            return count;
        }
       

        public static string GetSecondLongestWord(string[] listOfWords, string longestWord)
        {
            string secondLongestWord = string.Empty;

            try
            {
                //replace the longest word with an empty string to find the second longest word
                string[] listOfWords_without_longestWord = listOfWords.Select(x => x.Replace(longestWord, string.Empty)).ToArray();
                //now find the longest
                secondLongestWord = GetLongestWords(listOfWords_without_longestWord);
            }
            catch (Exception ex)
            {
                //write the exception to log;
                return secondLongestWord; //retun an empty string
            }

            return secondLongestWord;
        }

        private static List<Tuple<string, string>> GetPairs(string word)
        {
            var pairedWord = new List<Tuple<string, string>>();
            for (int x = 1; x < word.Length; x++)
            {
                pairedWord.Add(Tuple.Create(word.Substring(0, x), word.Substring(x)));
            }
            return pairedWord;
        }
    }
}
