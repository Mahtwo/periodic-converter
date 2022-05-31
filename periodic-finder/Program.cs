using System.Collections.ObjectModel;

namespace periodic_finder
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
        public static readonly ReadOnlyDictionary<char, string> uppercaseAccentConversion = new(
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

        static void Main(string[] args)
        {
            Console.Write("Enter a text to convert : ");
            string? enteredText = Console.ReadLine();

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
            string text = enteredText.Trim();

            //Add all the words to a list witout the white spaces
            List<string> words = new();
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

            //Convert words to make them fully uppercase and remove accents
            List<string> convertedWords = new();
            foreach (string word in words)
            {
                convertedWords.Add(ConvertWord(word));
            }

            //For each words
            for (int i = 0; i < words.Count; i++)
            {
                Console.Write(words[i] + " : ");
                List<int>? atomicNumbers = ConvertToAtomicNumbers(convertedWords[i]);

                if (atomicNumbers is null)
                {
                    Console.WriteLine("Conversion impossible");
                }
                else
                {
                    //Display the atomic numbers
                    foreach (int number in atomicNumbers)
                    {
                        Console.Write(number + " ");
                    }

                    Console.Write('(');
                    //Display the symbols
                    foreach (int number in atomicNumbers)
                    {
                        Console.Write(periodicTable[number - 1]);
                    }
                    Console.WriteLine(')');
                }
            }
        }

        /// <summary>
        /// Convert to atomic numbers the word specified in parameter
        /// </summary>
        /// <param name="word">Word to convert</param>
        /// <returns>Atomic numbers</returns>
        private static List<int>? ConvertToAtomicNumbers(string word)
        {
            List<int> atomicNumbers = new();
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
                        return null;
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
    }
}
