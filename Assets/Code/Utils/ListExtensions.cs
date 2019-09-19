using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AceTheChase.Utils 
{
    /// <summary>
    /// Extension methods to the built-in generic List.
    /// </summary>
    static class ListExtensions
    {
        private static System.Random _rng = new System.Random();

        /// <summary>
        /// Randomize the order of a List.
        /// Fisher-Yates shuffle implementation stolen from http://stackoverflow.com/a/1262619.
        /// </summary>
        public static void Shuffle<T>(this IList<T> list, System.Random rng = null)
        {
            if (rng == null) { rng = _rng; }

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

        /// <summary>
        /// Get a random element from a List.
        /// </summary>
        public static T ChooseRandom<T>(this IList<T> list, System.Random rng = null)
        {
			if (list.Count == 0)
			{
				throw new ArgumentOutOfRangeException("Can't select a random element from an empty list.");
			}

            if (rng == null) { rng = _rng;  }
            return list[rng.Next(list.Count)];
        }

        /// <summary>
        /// Get a random subset of the specified size from the provided List.
        /// </summary>
        public static IList<T> RandomSubset<T>(this IList<T> list, int n, System.Random rng = null)
        {
			if (list.Count < n)
			{
				throw new ArgumentOutOfRangeException($"Can't select a random subset of {n} items from a list of {list.Count} items.");
			}

            if (rng == null) { rng = _rng;  }
            IList<int> indices = rng.Distinct(n, 0, list.Count);

            List<T> output = new List<T>(n);
            foreach (int index in indices)
            {
                output.Add(list[index]);
            }

            return output;
        }
    }
}
