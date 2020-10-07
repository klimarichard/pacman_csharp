using System;
using System.IO;
using System.Text;

namespace pacman {
    /// <summary>
    /// The <c>LevelLoader</c> class controls loading of the level map
    /// from resource map file. It contains static methods only.
    /// </summary>
    public class LevelLoader {
        /// <summary>
        /// Loads the level map from resource file to a single string.
        /// </summary>
        /// <param name="path">path to resource map file</param>
        /// <returns>single string containing the loaded map</returns>
        public static string loadMapToString(String path) {
            StringBuilder builder = new StringBuilder();

            try {
                StreamReader sr = new StreamReader(path);
                string line;
                while ((line = sr.ReadLine()) != null) {
                    builder.Append(line);
                    builder.Append('\n');
                }
            } catch (IOException e) {
                Console.WriteLine(e.Message);
            }

            return builder.ToString();
        }

        /// <summary>
        /// Converts a character to a corresponding integer value
        /// used in the game.
        /// </summary>
        /// <param name="c">a character</param>
        /// <returns>corresponding integer value</returns>
        public static int charToInt(char c) {
            int i = "XFBCSPVRYQ".IndexOf(c);

            // if unknown char is examined, return EMPTY value
            if (i == -1)
                return 31;
            else
                return i;
        }

        /// <summary>
        /// Gets an integer value from a string containing a number.
        /// If the string cannot be converted to a number (it contains
        /// other characters than numbers and unary minus) it returns
        /// <c>-1</c>.
        /// </summary>
        /// <param name="number">a string containing a number</param>
        /// <returns>integer value of the string</returns>
        public static int parseInt(String number) {
            try {
                return int.Parse(number);
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                return -1;
            }
        }
    }
}
