using System;
using System.Collections.Generic;
using System.Linq;

namespace Nashet.UnitGame
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// Gives minimal element in collection. If there is more than 1 result - gives random
        /// </summary>        
        public static TSource MinByRandom<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
        {
            if (source.IsEmpty())
                return default(TSource);
            var res = source.Min(selector);
            return source.Where(x => selector(x).Equals(res)).Random();

        }

        public static bool IsEmpty<T>(this IEnumerable<T> source)
        {
            return !source.Any();
        }

        private static Random Rand = new Random();
        public static T Random<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable.IsEmpty())
                return default(T);
            else
            {
                var count = enumerable.Count();
                int index = Rand.Next(count);
                return enumerable.ElementAt(index);
            }
        }
    }
}