using System;
using System.Collections.Generic;

namespace GSWebAPI.GSWebAPI.Services.GameQuizCompetitions.CustomClasses
{
    /// <summary>
    /// To shuffle a list.
    /// </summary>
    public static class ListShuffler
    {
        private static Random rng = new Random();

        /// <summary>
        /// Shuffle a list.
        /// </summary>
        /// <typeparam name="T">Any type.</typeparam>
        /// <param name="list">The list to shuffle.</param>
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
