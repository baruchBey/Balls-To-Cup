using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Baruch.Extension
{
    public static class StringExtensions
    {
      

        public static string ToPascalCase(this string input)
        {
            // Remove leading underscore and split the string by underscore
            string[] parts = input.TrimStart('_').Split('_');

            // Convert each part to PascalCase
            for (int i = 0; i < parts.Length; i++)
            {
                parts[i] = PascalCase(parts[i]);
            }

            // Join the parts and return the result
            return string.Join("", parts);
        }

        private static string PascalCase(this string input)
        {
            // Remove non-word characters and convert to PascalCase
            string pascalCase = Regex.Replace(input, @"\W+", "");
            if (string.IsNullOrEmpty(pascalCase))
            {
                return input; // Return input as is if it contains only non-word characters
            }
            return char.ToUpper(pascalCase[0]) + pascalCase.Substring(1);
        }
    }
}