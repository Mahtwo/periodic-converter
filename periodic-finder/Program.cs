namespace periodic_finder
{
    internal class Program
    {
        /// <summary>
        /// Array of the periodic table
        /// The atomic number of an element is index + 1
        /// </summary>
        private static readonly string[] periodicTable = new string[]
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
        private static readonly string[] periodicTableUppercase = new string[]
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

        static void Main(string[] args)
        {
            string[] lPeriodicTable = periodicTable;

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

            foreach (string word in words)
            {

            }
        }
    }
}
