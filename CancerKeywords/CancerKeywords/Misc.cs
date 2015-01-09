using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CancerKeywords
{
    static class Misc
    {

        /// <summary>
        /// Returns a color based on a number between 1 and 14
        /// </summary>
        /// <param name="number">An int between 1 and 14</param>
        /// <returns>A color</returns>
        public static Color colorFromNumber(int number)
        {
            switch (number)
            {
                case 1:
                    return Color.LightBlue;
                case 2:
                    return Color.Blue;
                case 3:
                    return Color.Cyan;
                case 4:
                    return Color.Brown;
                case 5:
                    return Color.Teal;
                case 6:
                    return Color.Lavender;
                case 7:
                    return Color.Green;
                case 8:
                    return Color.Yellow;
                case 9:
                    return Color.Gold;
                case 10:
                    return Color.Orange;
                case 11:
                    return Color.Pink;
                case 12:
                    return Color.Firebrick;
                case 13:
                    return Color.Red;
                case 14:
                    return Color.Black;
                default:
                    return Color.White;
            }
        }

        /// <summary>
        /// Checks wheter a given string is included in the cancer types definded within
        /// </summary>
        /// <param name="potentialCancer">a potential cancer type</param>
        /// <returns>True if the input string is on the list</returns>
        public static bool isInCancerList(string potentialCancer)
        {
            potentialCancer = potentialCancer.ToLower();

            //List of potentional cancer types
            string[] knownTypes = new string[] {"adenocarcinoma",
                                                "adrenocortical",
                                                "astrocytic",
                                                "bladder",
                                                "brain",
                                                "breast",
                                                "colerectal",
                                                "colon",
                                                "colorectal",
                                                "dysplasia",
                                                "embryonal",
                                                "endometrial",
                                                "endometrioid ",
                                                "epithelial",
                                                "esophageal",
                                                "fibrosarcoma",
                                                "gallbladder",
                                                "gastric",
                                                "gastrointestinal",
                                                "hepatic",
                                                "hapatocellular",
                                                "hepatocellular",
                                                "intestine",
                                                "intraepithelial",
                                                "liver",
                                                "lung",
                                                "mammary",
                                                "mediastinal",
                                                "melanoma",
                                                "mesenchymal",
                                                "nasopharyngeal",
                                                "neuroendocrine",
                                                "ovarian",
                                                "pancreatic",
                                                "papillary",
                                                "pituitary",
                                                "prostate",
                                                "rectal",
                                                "renal",
                                                "retinoblastoma",
                                                "squamous",
                                                "testicular",
                                                "thymic",
                                                "thyroid",
                                                "tonsillar",
                                                "urothelial"};

            return knownTypes.Contains(potentialCancer);
        }

        /// <summary>
        /// Converts the bubbles to a more readable format
        /// </summary>
        /// <param name="cancerBubbles">The directionary of cancer bubbles</param>
        /// <returns>A string with the different bubbles and their score</returns>
        public static string formatScores(SortedDictionary<string, CancerBubble> cancerBubbles)
        {
            string result = "";

            foreach (KeyValuePair<string, CancerBubble> kvp in cancerBubbles)
            {
                result += kvp.Key + ": " + kvp.Value.Cases + "\r\n";
            }

            return result;
        }


        /// <summary>
        /// Takes a text and splits it up in individual words. Then returns a list of words the are just before "cancer"
        /// </summary>
        /// <param name="abstractText">Text to investigate</param>
        /// <returns>a list of potential cancer types</returns>
        public static List<string> getCancerTypes(string abstractText)
        {
            abstractText = abstractText.ToLower();
            //Replace any special char with space
            char[] punctuations = new char[] { ',', '.', '?', '-', '!', '(', ')', '/', '"', '*', '[', ']' };
            foreach (char c in punctuations)
            {
                abstractText = abstractText.Replace(c, ' ');
            }

            //Split the text
            char[] delimiters = new char[] { '\r', '\n', ' ' };
            string[] split = abstractText.Split(delimiters);

            //Look for potential cancer types
            List<string> types = new List<string>();
            for (int i = 1; i < split.Length; i++)
            {
                if (split[i].Equals("cancer") || split[i].Equals("cancers") || split[i].Equals("carcinoma") || split[i].Equals("carcinomas") || split[i].Equals("tumor") || split[i].Equals("tumors"))
                {

                    if (!types.Contains(split[i - 1]))
                    {
                        types.Add(split[i - 1]);
                    }
                }
            }
            return types;
        }
    }
}
