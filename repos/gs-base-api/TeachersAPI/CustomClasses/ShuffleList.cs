using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.CustomClasses
{
    /// <summary>
    /// To shuffle a list.
    /// </summary>
    public static class ShuffleList
    {
        /// <summary>
        /// To shuffle a list.
        /// </summary>
        /// <typeparam name="T">Any type of list.</typeparam>
        /// <param name="list">The list to shuffle.</param>
        /// <param name="rnd">Reandom object.</param>
        public static void Shuffle<T>(this IList<T> list, Random rnd)
        {
            for (var i = list.Count; i > 0; i--)
                list.Swap(0, rnd.Next(0, i));
        }

        /// <summary>
        /// To Swap.
        /// </summary>
        /// <typeparam name="T">Any type.</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="i">The item to swap.</param>
        /// <param name="j">The item to swap.</param>
        public static void Swap<T>(this IList<T> list, int i, int j)
        {
            var temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
}