using System;
using System.Collections.Generic;
using UnityEngine;

namespace Baruch.Extension
{
    public static class Formatter
    {
        public enum Variance
        {
            A,
            B
        }
        private static readonly string[] _formatterVarianceA = new string[] { "0.", "0.", "0.", "#,.00K", "#,.0K", "#,.K", "#,,.00M", "#,,.0M", "#,,.M", "#,,,.00B", "#,,,.0B", "#,,,.B", "#,,,,.0T", "#,,,,.00T", "#,,,,.T", "#,,,,,.0AA", "#,,,,,.0AA", "#,,,,,.AA", "#,,,,,,.0BB", "#,,,,,,.0BB", "#,,,,,,.BB"
            ,"#,,,,,,,.0CC", "#,,,,,,,.0CC", "#,,,,,,,.CC" ,"#,,,,,,,,.0DD", "#,,,,,,,,.0DD", "#,,,,,,,,.DD","#,,,,,,,,,.0EE", "#,,,,,,,,,.0EE", "#,,,,,,,,,.EE"};

        private static readonly string[] _formatterVarianceB = new string[] { "0.00", "0.0", "0.", "#,.00K", "#,.0K", "#,.K", "#,,.00M", "#,,.0M", "#,,.M", "#,,,.00B", "#,,,.0B", "#,,,.B", "#,,,,.00T", "#,,,,.0T", "#,,,,.T", "#,,,,,.00AA", "#,,,,,.0AA", "#,,,,,.AA", "#,,,,,,.00BB", "#,,,,,,.0BB", "#,,,,,,.BB", "#,,,,,,,.00CC", "#,,,,,,,.0CC", "#,,,,,,,.CC", "#,,,,,,,,.00DD", "#,,,,,,,,.0DD", "#,,,,,,,,.DD", "#,,,,,,,,,.00EE", "#,,,,,,,,,.0EE", "#,,,,,,,,,.EE" };

        static readonly Dictionary<Variance, string[]> _varianceToFormatter = new() { {Variance.A, _formatterVarianceA }, { Variance.B, _formatterVarianceB } };

       
        static int Significance(double d)
        {
            return (int)Math.Max(Math.Floor(Math.Log10(d)),0);
        }
     

        public static string Format(this float coin, Variance variance = Variance.A, string prefix = "", string suffix = "")
        {
            return $"{prefix}{coin.ToString(_varianceToFormatter[variance][Significance(coin)])}{suffix}";
        }
    
        public static string Format(this int coin, Variance variance = Variance.A, string prefix = "", string suffix = "")
        {
            return $"{prefix}{coin.ToString(_varianceToFormatter[variance][Significance(coin)])}{suffix}";
        }

        public static string Format(this double coin, Variance variance = Variance.A, string prefix = "", string suffix = "")
        {
            return $"{prefix}{coin.ToString(_varianceToFormatter[variance][Significance(coin)])}{suffix}";
        }

    }
}