using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConsoleApp1
{
    public static class StringExtensions
    {
        public static bool IsPrimaryColor(this string inString)
        {
            string[] primaryColors = { "Red", "Yellow", "Blue" };
            foreach (var color in primaryColors)
            {
                if (inString.Equals(color, StringComparison.CurrentCultureIgnoreCase))
                    return true;
            }
            return false;
        }
    }
}
