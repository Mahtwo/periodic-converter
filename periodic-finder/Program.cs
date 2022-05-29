﻿namespace periodic_finder
{
    internal class Program
    {
        /// <summary>
        /// Array of the periodic table
        /// The atomic number of an element is index + 1
        /// </summary>
        private static readonly string[] periodicTable = new string[]
        {
            "H",
            "He",
            "Li",
            "Be",
            "B",
            "C",
            "N",
            "O",
            "F",
            "Ne",
            "Na",
            "Mg",
            "Al",
            "Si",
            "P",
            "S",
            "Cl",
            "Ar",
            "K",
            "Ca",
            "Sc",
            "Ti",
            "V",
            "Cr",
            "Mn",
            "Fe",
            "Co",
            "Ni",
            "Cu",
            "Zn",
            "Ga",
            "Ge",
            "As",
            "Se",
            "Br",
            "Kr",
            "Rb",
            "Sr",
            "Y",
            "Zr",
            "Nb",
            "Mo",
            "Tc",
            "Ru",
            "Rh",
            "Pd",
            "Ag",
            "Cd",
            "In",
            "Sn",
            "Sb",
            "Te",
            "I",
            "Xe",
            "Cs",
            "Ba",
            "La",
            "Ce",
            "Pr",
            "Nd",
            "Pm",
            "Sm",
            "Eu",
            "Gd",
            "Tb",
            "Dy",
            "Ho",
            "Er",
            "Tm",
            "Yb",
            "Lu",
            "Hf",
            "Ta",
            "W",
            "Re",
            "Os",
            "Ir",
            "Pt",
            "Au",
            "Hg",
            "Tl",
            "Pb",
            "Bi",
            "Po",
            "At",
            "Rn",
            "Fr",
            "Ra",
            "Ac",
            "Th",
            "Pa",
            "U",
            "Np",
            "Pu",
            "Am",
            "Cm",
            "Bk",
            "Cf",
            "Es",
            "Fm",
            "Md",
            "No",
            "Lr",
            "Rf",
            "Db",
            "Sg",
            "Bh",
            "Hs",
            "Mt",
            "Ds",
            "Rg",
            "Cn",
            "Nh",
            "Fl",
            "Mc",
            "Lv",
            "Ts",
            "Og",
        };

        static void Main(string[] args)
        {
            string[] lPeriodicTable = periodicTable;

            Console.Write("Enter a text (letters and spaces only) to convert : ");
            string? enteredText = Console.ReadLine();

            //While until the entered text is correct
            while (enteredText is null || !enteredText.Any(Char.IsLetter) || !enteredText.All(x => Char.IsLetter(x) || Char.IsWhiteSpace(x)))
            {
                if (enteredText is null)
                {
                    Console.Write("A problem occured, enter a text again : ");
                }
                else if (!enteredText.Any(Char.IsLetter))
                {
                    Console.Write("The entered text doesn't contain any letters, enter a text again : ");
                }
                else
                {
                    Console.Write("The entered text contains characters other than letters and spaces, enter a text again : ");
                }
                enteredText = Console.ReadLine();
            }
            //Remove white-spaces at the beginning and end
            string text = enteredText.Trim();

            //Add all the words to a list witout the white spaces
            List<string> listWords = new();
            string word = "";
            foreach (char c in text)
            {
                if (Char.IsWhiteSpace(c))
                {
                    if (word != "")
                    {
                        listWords.Add(word);
                        word = "";
                    }
                }
                else
                {
                    word += c;
                }
            }
            if (word != "")
            {
                listWords.Add(word);
            }


        }
    }
}
