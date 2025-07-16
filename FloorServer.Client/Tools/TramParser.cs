using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;

namespace FloorServer.Client.Tools
{
    public class TramParser
    {
        /// <summary>
        /// Parses the trame for fill the dictionary.
        /// </summary>
        /// <param name="trame">The trame.</param>
        /// <returns></returns>
        public static StringDictionary ParseTrame(string trame)
        {
            var dico = new StringDictionary();
            //var regTrame = new Regex("(?<key>\\S*)=\"(?<value>\\s*\\S*)\"");
            var regTrame = new Regex("(?<key>\\w+)=\"(?<value>[^\"]+)\"");
            var matchTrame = regTrame.Match(trame);
            while (matchTrame.Success)
            {
                var key = matchTrame.Groups["key"].Value;
                if (!dico.ContainsKey(key))
                    dico.Add(key, matchTrame.Groups["value"].Value);
                matchTrame = matchTrame.NextMatch();
            }
            return dico;
        }

        /// <summary>
        /// Return the string without first end last "
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string GetValue(string value)
        {
            var regTrame = new Regex("^\"(?<value>\\S*)\"$");
            var matchTrame = regTrame.Match(value);

            return matchTrame.Success ? matchTrame.Groups["value"].Value : String.Empty;

        }
    }
}
