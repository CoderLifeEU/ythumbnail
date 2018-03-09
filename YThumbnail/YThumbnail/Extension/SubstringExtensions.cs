using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YThumbnail.Extension
{
    static class SubstringExtensions
    {
        /// <summary>
        /// Get string value between [first] a and [last] b.
        /// </summary>
        public static string Between(this string value, string after, string before)
        {
            int posA = value.IndexOf(after);
            int posB = value.LastIndexOf(before);
            if (posA == -1)
            {
                return "";
            }
            if (posB == -1)
            {
                return "";
            }
            int adjustedPosA = posA + after.Length;
            if (adjustedPosA >= posB)
            {
                return "";
            }
            return value.Substring(adjustedPosA, posB - adjustedPosA);
        }

        /// <summary>
        /// Get string value after [first] a.
        /// </summary>
        public static string Before(this string value, string before)
        {
            int posA = value.IndexOf(before);
            if (posA == -1)
            {
                return "";
            }
            return value.Substring(0, posA);
        }

        /// <summary>
        /// Get string value after [last] a.
        /// </summary>
        public static string After(this string value, string after)
        {
            int posA = value.LastIndexOf(after);
            if (posA == -1)
            {
                return "";
            }
            int adjustedPosA = posA + after.Length;
            if (adjustedPosA >= value.Length)
            {
                return "";
            }
            return value.Substring(adjustedPosA);
        }

        /// <summary>
        /// Get string value after [last] a.
        /// </summary>
        public static string BeforeIfContains(this string value, string before)
        {
            int posA = value.IndexOf(before);
            if (posA == -1)
            {
                return value;
            }
            return value.Substring(0, posA);
        }
    }

}
