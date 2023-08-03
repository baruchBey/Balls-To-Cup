using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Baruch.Extension
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="current"></param>
        /// <param name="compared"></param>
        /// <returns></returns>
        public static bool NotEquals(this object current, object compared)
        {
            return !current.Equals(compared);
        }

      
    }
}