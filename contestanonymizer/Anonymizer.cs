using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace contestanonymizer
{
    public static class Anonymizer
    {
        private static Random rng = new Random();
        private static void Shuffle<T>(this IList<T> list) //method for shuffling lists
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static List<string> GenerateKey(string adjFile, string nounFile, int amount, out string keyD)
        {
            List<string> nounList = new List<string>();
            List<string> adjectiveList = new List<string>();

            using (StreamReader _textStreamReader = new StreamReader(string.Format("Nouns\\{0}",nounFile))) //retrieving nouns from selected file
            {
                while (_textStreamReader.Peek() >= 0)
                {
                    nounList.Add((_textStreamReader.ReadLine()));
                }
            }

            using (StreamReader _textStreamReader = new StreamReader(string.Format("Adjectives\\{0}", adjFile))) //retrieving adjectives from selected file
            {
                while (_textStreamReader.Peek() >= 0)
                {
                    adjectiveList.Add((_textStreamReader.ReadLine()));
                }
            }

            if (nounList.Count < amount) //making sure there are enough words to anonymize all selected entries
                throw new IndexOutOfRangeException("Amount of selected beatmaps is larger than the amount of nouns in the .txt file!");
            if (adjectiveList.Count < amount)
                throw new IndexOutOfRangeException("Amount of selected beatmaps is larger than the amount of adjectives in the .txt file!");

            nounList.Shuffle(); //randomizing word list order
            adjectiveList.Shuffle();
            
            List<string> anonList = new List<string>(); //list for full anon words (adj + noun combination)
            string[] keyList = new string[amount]; //list for the keys per anonymized entry for later deanonymization
            string anonString; //anon word, combination of random adjective + noun

            for (int i = 0; i < amount; i++)
            {
                if (adjectiveList[i] == "") //making sure the adjective and noun aren't empty
                    throw new ArgumentNullException("Error: Attempted to add a key with missing adjective. Make sure your adjectives list doesn't contain any empty new lines.");
                else if (nounList[i] == "")
                    throw new ArgumentNullException("Error: Attempted to add a key with missing noun. Make sure your nouns list doesn't contain any empty new lines.");
                else
                    anonString = string.Format("{0} {1}", adjectiveList[i], nounList[i]);

                if (anonList.Contains(anonString)) //making sure there are no duplicate anon entries
                    throw new AmbiguousMatchException(string.Format("Error: Attempted to add a duplicate key '{0}' to the anonymized key list. Please check that word .txt files don't contain duplicates.", anonString));
                else
                    anonList.Add(anonString);
                keyList[i] = string.Format("{0},{1}", BeatmapParser.BeatmapList[i].Version, anonList[i]);
            }

            string keyDirectory = string.Format(@"{0}\anonkey_{1:yyyyMMdd_HHmmss}.txt", Directory.GetCurrentDirectory(), DateTime.Now); //key directory and filename, created based on current time
            keyD = keyDirectory; //outing key back to view for displaying the directory for the key

            File.WriteAllLines(keyDirectory, keyList); //creating key

            return anonList;
        }
    }
}
