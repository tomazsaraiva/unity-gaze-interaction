#region Includes
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
#endregion

namespace TS.Extensions
{
    public static class CollectionExtensions
    {
        public static string Print<T>(this IList<T> list, bool pretty = false)
        {
            if (list == null) { return "NULL"; }
            if (list.Count == 0) { return "EMPTY"; }

            var builder = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                builder.AppendFormat("{0}: {1}", i, list[i].ToString());
                if (i < list.Count - 1)
                {
                    builder.Append(pretty ? "\n" : ", ");
                }
            }
            return builder.ToString();
        }
        public static string Print<TKey, TElement>(this IDictionary<TKey, TElement> dic, bool pretty = false)
        {
            if (dic == null) { return "NULL"; }
            if (dic.Count == 0) { return "EMPTY"; }

            var builder = new StringBuilder();
            var count = 0;
            foreach (TKey key in dic.Keys)
            {
                builder.AppendFormat("{0}: {1}", key, dic[key].ToString());
                if (count < dic.Count - 1)
                {
                    builder.Append(pretty ? "\n" : ", ");
                }
                count++;
            }
            return builder.ToString();
        }

        public static int RandomIndex<T>(this T[] array)
        {
            return Random.Range(0, array.Length);
        }
        public static T RandomElement<T>(this T[] array)
        {
            return array.IsNullOrEmpty() ? default : array[RandomIndex(array)];
        }
        public static int RandomIndex(this IList list)
        {
            return Random.Range(0, list.Count);
        }
        public static T RandomElement<T>(this List<T> list)
        {
            return list.IsNullOrEmpty() ? default : list[RandomIndex(list)];
        }
        public static bool IsNullOrEmpty<T>(this IList<T> list)
        {
            return list == null || list.Count == 0;
        }
        public static bool IsNullOrEmpty<T>(this T[] array)
        {
            return array == null || array.Length == 0;
        }
        public static IOrderedEnumerable<T> Shuffle<T>(this IOrderedEnumerable<T> target, System.Random random)
        {
            return target.OrderBy(x => random.Next());
        }
    }
}