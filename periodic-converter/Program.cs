﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace periodic_converter
{
    internal class Program
    {
        /// <summary>
        /// Array of the periodic table
        /// The atomic number of an element is index + 1
        /// </summary>
        public static readonly string[] periodicTable = new string[]
        {
            "H","He","Li","Be","B","C","N","O","F","Ne",
            "Na","Mg","Al","Si","P","S","Cl","Ar","K","Ca",
            "Sc","Ti","V","Cr","Mn","Fe","Co","Ni","Cu","Zn",
            "Ga","Ge","As","Se","Br","Kr","Rb","Sr","Y","Zr",
            "Nb","Mo","Tc","Ru","Rh","Pd","Ag","Cd","In","Sn",
            "Sb","Te","I","Xe","Cs","Ba","La","Ce","Pr","Nd",
            "Pm","Sm","Eu","Gd","Tb","Dy","Ho","Er","Tm","Yb",
            "Lu","Hf","Ta","W","Re","Os","Ir","Pt","Au","Hg",
            "Tl","Pb","Bi","Po","At","Rn","Fr","Ra","Ac","Th",
            "Pa","U","Np","Pu","Am","Cm","Bk","Cf","Es","Fm",
            "Md","No","Lr","Rf","Db","Sg","Bh","Hs","Mt","Ds",
            "Rg","Cn","Nh","Fl","Mc","Lv","Ts","Og"
        };

        /// <summary>
        /// Array of the periodic table in uppercase
        /// The atomic number of an element is index + 1
        /// </summary>
        public static readonly string[] periodicTableUppercase = new string[]
        {
            "H","HE","LI","BE","B","C","N","O","F","NE",
            "NA","MG","AL","SI","P","S","CL","AR","K","CA",
            "SC","TI","V","CR","MN","FE","CO","NI","CU","ZN",
            "GA","GE","AS","SE","BR","KR","RB","SR","Y","ZR",
            "NB","MO","TC","RU","RH","PD","AG","CD","IN","SN",
            "SB","TE","I","XE","CS","BA","LA","CE","PR","ND",
            "PM","SM","EU","GD","TB","DY","HO","ER","TM","YB",
            "LU","HF","TA","W","RE","OS","IR","PT","AU","HG",
            "TL","PB","BI","PO","AT","RN","FR","RA","AC","TH",
            "PA","U","NP","PU","AM","CM","BK","CF","ES","FM",
            "MD","NO","LR","RF","DB","SG","BH","HS","MT","DS",
            "RG","CN","NH","FL","MC","LV","TS","OG"
        };

        /// <summary>
        /// Dictionary of uppercased accented characters with their unaccented version
        /// </summary>
        /// <remarks>
        /// Value is a string as some characters like Œ correspond to more than one character
        /// </remarks>
        public static readonly ReadOnlyDictionary<char, string> uppercaseAccentConversion = new ReadOnlyDictionary<char, string>(
            new Dictionary<char, string>
            {
                {'À',"A"},{'Á',"A"},{'Â',"A"},{'Ä',"A"},{'Å',"A"},{'Ã',"A"},
                {'É',"E"},{'È',"E"},{'Ê',"E"},{'Ë',"E"},
                {'Í',"I"},{'Ì',"I"},{'Î',"I"},{'Ï',"I"},
                {'Ó',"O"},{'Ò',"O"},{'Ô',"O"},{'Ö',"O"},{'Õ',"O"},
                {'Ú',"U"},{'Ù',"U"},{'Û',"U"},{'Ü',"U"},
                {'Ý',"Y"},
                {'Ç',"C"},{'Ñ',"N"},{'Æ',"AE"},{'Œ',"OE"}
            }
        );

        /// <summary>
        /// Whether to show only the converted words
        /// </summary>
        public static bool onlyConverted = false;

        /// <summary>
        /// Whether to show only the unique words
        /// </summary>
        public static bool uniqueWords = false;

        /// <summary>
        /// Whether to ask for a save location after the conversion
        /// </summary>
        public static bool saveToFile = false;

        [STAThread]
        static void Main(/*string[] args*/)
        {
            //Detect if a pause is needed at the end of the program (.exe ran directly)
            bool willCloseTerminal = false;
            if ((Console.CursorLeft == 0) && (Console.CursorTop == 0))
            {
                willCloseTerminal = true;
            }

            List<string> words = Menu();

            //Exit the program if Menu() returned null
            if (words == null)
            {
                return;
            }

            ConvertAndOutputWords(words);

            Console.Out.Flush();
            if (willCloseTerminal && !saveToFile)
            {
                Console.WriteLine("\n[PRESS ANY KEY TO QUIT]");
                Console.ReadKey(true);
            }
        }

        /// <summary>
        /// Display the main menu of the program
        /// </summary>
        /// <returns></returns>
        private static List<string> Menu()
        {
            List<string> words = null;

            //(Re)display the menu until we get words
            do
            {
                if (!Console.IsOutputRedirected)
                {
                    Console.Clear();  //Clear the console each time the menu needs to be shown
                }
                else
                {
                    Console.OutputEncoding = Encoding.UTF8;
                    saveToFile = false;  //Output already redirected
                }
                //Display the menu
                Console.WriteLine("PERIODIC-CONVERTER\n");
                Console.WriteLine("1. Enter a text manually");
                Console.WriteLine("2. Select a text file");
                Console.WriteLine("3. Show only the converted words [" + onlyConverted.ToString().ToUpper() + "]");
                Console.WriteLine("4. Show only the unique words [" + uniqueWords.ToString().ToUpper() + "]");
                Console.WriteLine("5. Save conversion to a text file [" + saveToFile.ToString().ToUpper() + "]");
                Console.WriteLine("0. Quit\n");
                Console.Write("Enter your choice : ");

                int choice = int.TryParse(Console.ReadLine(), out choice) ? choice : -1;

                //While the entered number isn't valid, ask to enter another number
                while (choice < 0 || choice > 5)
                {
                    Console.Write("Not valid, enter a number again : ");
                    choice = int.TryParse(Console.ReadLine(), out choice) ? choice : -1;
                }

                switch (choice)
                {
                    case 1:
                        Console.WriteLine();
                        string text = EnterText();
                        words = GetWordsFromText(text);
                        break;
                    case 2:
                        Console.WriteLine();
                        words = SelectFile();

                        //If no file was selected or the file didn't contain words
                        if (words.Count == 0)
                        {
                            words = null;

                            Console.WriteLine("Invalid file, no words were found");
                            Console.WriteLine("[PRESS ANY KEY TO CONTINUE]");
                            Console.ReadKey(true);
                        }
                        break;
                    case 3:
                        onlyConverted = !onlyConverted;
                        break;
                    case 4:
                        uniqueWords = !uniqueWords;
                        break;
                    case 5:
                        saveToFile = !saveToFile;
                        break;
                    case 0:
                        //Exit the method by returning null, which exits the program
                        return null;
                }
            } while (words is null);

            return words;
        }

        /// <summary>
        /// Get all the words from a selected text file
        /// </summary>
        /// <returns>List of words</returns>
        private static List<string> SelectFile()
        {
            List<string> words = new List<string>();

            Console.WriteLine("Select a text file");

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
#if DEBUG
                openFileDialog.InitialDirectory = Path.GetFullPath(Directory.GetCurrentDirectory() + @"\..\..\..\Resources");
#endif
                openFileDialog.FilterIndex = 2;

                DialogResult dialogResult = openFileDialog.ShowDialog();
                Console.WriteLine();

                if (dialogResult == DialogResult.OK)
                {
                    Console.WriteLine("Getting all the words from the file...");

                    //Read the contents of the file into a stream
                    Stream fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        long fileLength = reader.BaseStream.Length;
                        long lastPercentage = -1;
                        while (!reader.EndOfStream)
                        {
                            long percentage = reader.BaseStream.Position * 100 / fileLength;

                            //Actualise the progression only if needed, otherwise it would slow down the program
                            if (percentage > lastPercentage)
                            {
                                lastPercentage = percentage;
                                ShowProgression((byte)percentage);
                            }

                            //Add all the words from the current line to the list of words
                            foreach (string word in GetWordsFromText(reader.ReadLine()))
                            {
                                words.Add(word);
                            }
                        }
                    }

                    //Always actualise the progression to 100% at the end
                    ShowProgression(100);
                    Console.WriteLine('\n');
                }
            }

            return words;
        }

        /// <summary>
        /// Ask the user to enter a text and return the words from it
        /// </summary>
        /// <returns>List of words</returns>
        private static string EnterText()
        {
            Console.Write("Enter a text to convert : ");
            string enteredText = Console.ReadLine();

            //While until the entered text is correct
            while (enteredText is null || !enteredText.Any(Char.IsLetter))
            {
                if (enteredText is null)
                {
                    Console.Write("A problem occured, enter a text again : ");
                }
                else
                {
                    Console.Write("The entered text doesn't contain any letters, enter a text again : ");
                }
                enteredText = Console.ReadLine();
            }

            //Remove white-spaces at the beginning and end
            return enteredText.Trim();
        }

        /// <summary>
        /// Get all the words from a text
        /// </summary>
        /// <param name="text">Text to get the words from</param>
        /// <returns>List of words</returns>
        private static List<string> GetWordsFromText(string text)
        {
            List<string> words = new List<string>();
            string tmpWord = "";
            foreach (char c in text)
            {
                if (Char.IsLetter(c))
                {
                    tmpWord += c;
                }
                else
                {
                    if (tmpWord != "")
                    {
                        words.Add(tmpWord);
                        tmpWord = "";
                    }
                }
            }
            if (tmpWord != "")
            {
                words.Add(tmpWord);
            }

            return words;
        }

        /// <summary>
        /// Convert to atomic numbers the word specified in parameter
        /// </summary>
        /// <param name="word">Word to convert</param>
        /// <returns>Atomic numbers</returns>
        private static List<int> ConvertToAtomicNumbers(string word)
        {
            List<int> atomicNumbers = new List<int>();
            List<int> lettersUsed = new List<int>();
            int index = 0;  //Part of the word to search in the periodic table
            int letters;  //Whether to search one or two letters in the periodic table
            if (word.Length == 1)
            {
                letters = 1;
            }
            else
            {
                letters = 2;
            }
            bool end = false;  //End the while loop when we got all the atomic numbers

            while (!end)
            {
                string subWord = word.Substring(index, letters);
                bool gotAtomicNumber = false;

                //For all elements
                for (int i = 0; i < 118; i++)
                {
                    if (subWord == periodicTableUppercase[i])
                    {
                        atomicNumbers.Add(i + 1);
                        lettersUsed.Add(letters);
                        index += letters;
                        gotAtomicNumber = true;

                        //If the index is at the end of the word, we got all the atomic numbers
                        if (index == word.Length)
                        {
                            end = true;
                        }
                        else
                        {
                            if (index + 2 <= word.Length)
                            {
                                letters = 2;
                            }
                            else
                            {
                                letters = 1;
                            }
                        }

                        break;
                    }
                }

                if (!gotAtomicNumber)
                {
                    if (letters == 2)
                    {
                        letters = 1;
                    }
                    else
                    {
                        //Go back to the lastWord where we used 2 letters and try with one letter instead
                        if (lettersUsed.Contains(2))
                        {
                            int lastTwoIndex = lettersUsed.LastIndexOf(2);

                            //Remove the subWords after and including the last subWords of two letters
                            lettersUsed.RemoveRange(lastTwoIndex, lettersUsed.Count - lastTwoIndex);
                            atomicNumbers.RemoveRange(lastTwoIndex, atomicNumbers.Count - lastTwoIndex);

                            //Index to before the last subWord with two letters
                            index = 0;
                            foreach (int lLetters in lettersUsed)
                            {
                                index += lLetters;
                            }

                            //Try with one letter
                            letters = 1;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }

            return atomicNumbers;
        }

        /// <summary>
        /// Convert a word to make it fully uppercase and remove accents
        /// </summary>
        /// <param name="word">Word to convert</param>
        /// <returns>Converted word</returns>
        private static string ConvertWord(string word)
        {
            string uppercaseWord = word.ToUpper();

            string convertedWord = "";
            foreach (char c in uppercaseWord)
            {
                //If character has an accent, get the unaccented version
                if (uppercaseAccentConversion.ContainsKey(c))
                {
                    convertedWord += uppercaseAccentConversion[c];
                }
                else
                {
                    convertedWord += c;
                }
            }

            return convertedWord;
        }

        /// <summary>
        /// Convert words with the periodic table and output the conversion
        /// </summary>
        /// <param name="words">List of words to convert and display</param>
        private static void ConvertAndOutputWords(List<string> words)
        {
            //Keep only the unique words if the option was selected
            if (uniqueWords)
            {
                words = words.Distinct().ToList();
            }

            //Convert words to make them fully uppercase and remove accents
            List<string> convertedWords = new List<string>();
            foreach (string word in words)
            {
                convertedWords.Add(ConvertWord(word));
            }

            //Ask for a save location if the option was selected
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
#if DEBUG
                saveFileDialog.InitialDirectory = Path.GetFullPath(Directory.GetCurrentDirectory() + @"\..\..\..\Resources");
#endif
                saveFileDialog.FilterIndex = 2;

                DialogResult dialogResult = saveFileDialog.ShowDialog();

                if (dialogResult == DialogResult.OK)
                {
                    Stream fileStream = saveFileDialog.OpenFile();
                    Console.SetOut(new StreamWriter(fileStream));
                }
            }

            //For each words
            for (int i = 0; i < words.Count; i++)
            {
                List<int> atomicNumbers = ConvertToAtomicNumbers(convertedWords[i]);

                if (atomicNumbers is null)
                {
                    //Don't display the not-convertible words if the option was selected
                    if (!onlyConverted)
                    {
                        Console.WriteLine(words[i] + " : Conversion impossible");
                    }
                }
                else
                {
                    StringBuilder line = new StringBuilder(words[i] + " : ");

                    //Display the atomic numbers
                    foreach (int number in atomicNumbers)
                    {
                        line.Append(number + " ");
                    }

                    line.Append('(');
                    //Display the symbols
                    foreach (int number in atomicNumbers)
                    {
                        line.Append(periodicTable[number - 1]);
                    }
                    line.Append(')');

                    Console.WriteLine(line.ToString());
                }
            }
        }

        /// <summary>
        /// Show the progression using a bar progressively filled with asterisks
        /// </summary>
        /// <param name="percentage">Percentage of progression</param>
        private static void ShowProgression(byte percentage)
        {
            StringBuilder bar = new StringBuilder();
            for (int i = 0; i < 10; i++)
            {
                if (percentage / 10 > i)
                {
                    bar.Append('*');
                }
                else
                {
                    bar.Append(' ');
                }
            }
            Console.Write("\r[" + bar.ToString() + "] " + percentage + "%");
        }
    }
}
