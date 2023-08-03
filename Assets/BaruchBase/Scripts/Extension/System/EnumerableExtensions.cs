using System.Linq;
using System.Collections.Generic;
using System;

namespace Baruch.Extension
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="enumerable"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T FindObjectOfType<T>(this IEnumerable<object> enumerable)
        {
            return (T)enumerable.First(entity => entity.GetType() == typeof(T));
        }
        public static T GetRandom<T>(this IEnumerable<T> list)
        {
            return Enumerable.ElementAt(list, UnityEngine.Random.Range(0, Enumerable.Count(list)));
        }
        public static int FirstNullIndex<T>(this IEnumerable<T> list) where T : class
        {

            var index = 0;
            foreach (var item in list)
            {
                if (item == null)
                {
                    return index;
                }
                index++;
            }
            return -1;
        }
        public static void Remove<T>(this T[] array,T target) where T : class
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == target)
                {
                    array[i] = null;                    
                    return;
                }
            }
           
        }
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T item in source)
            {
                action(item);
            }

            return source;
        }
    }
}