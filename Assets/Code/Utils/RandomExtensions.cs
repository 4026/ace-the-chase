using System;
using System.Collections.Generic;

namespace AceTheChase.Utils
{
    /// <summary>
    /// Extensions to System.Random.
    /// </summary>
    public static class RandomExtensions
    {
        /// <summary>
        /// Generate N random positive (optionally including 0) integers that sum to the specified
        /// total.
        /// </summary>
        public static IList<int> SumTo(
            this System.Random rng,
            int n,
            int sum,
            bool allowZero = false
        )
        {
            if (n <= 0 || sum <= 0)
            {
                throw new ArgumentException($"Can't generate {n} numbers that sum to {sum}.");
            }

            if (!allowZero && n > sum)
            {
                throw new ArgumentException($"Can't generate {n} non-zero numbers that sum to {sum}.");
            }

            // Generate a list of N-1 random numbers between 0 and sum, which will act as
            // partition points between the output random numbers.
            IList<int> divisions = allowZero 
                ? rng.Many(n - 1, 0, sum, true)
                : rng.Distinct(n - 1, 1, sum, true);

            // Add each partition width to the list of output numbers.
            List<int> output = new List<int>();
            int lastDivision = 0;
            for (int i = 0; i < divisions.Count; ++i)
            {
                output.Add(divisions[i] - lastDivision);
                lastDivision = divisions[i];
            }
            output.Add(sum - lastDivision);

            return output;
        }
    
        /// <summary>
        /// Generate N distinct random integers between min (inclusive) and max (exclusive),
        /// optionally sorted in ascending order.
        /// </summary>
        public static IList<int> Distinct(
            this System.Random rng,
            int n,
            int min,
            int max,
            bool sorted = false
        )
        {
            if (n == 0)
            {
                return new int[0];
            }
            if (max <= min || n > (max - min))
            {
                throw new ArgumentException($"Can't generate {n} distinct integers between {min} and {max}.");
            }

            /* We use a HashSet to track previously-generated numbers and reject any that recur to 
             * ensure uniqueness (which is required to output a list of non-zero integers). This
             * method can result in some long worst-case execution times as n approaches sum for
             * large values of sum.
             */
            HashSet<int> previouslyGenerated = new HashSet<int>();
            for (int i = 0; i < n; ++i)
            {
                int newNumber;
                do 
                {
                    newNumber = rng.Next(min, max);
                } while (previouslyGenerated.Contains(newNumber));
                previouslyGenerated.Add(newNumber);
            }

            List<int> output = new List<int>(previouslyGenerated);
            if (sorted)
            {
                output.Sort();
            }

            return output;
        }

        /// <summary>
        /// Generate a list of (not necessarily unique) random integers between min (inclusive) and
        /// max (exclusive), optionally sorted in ascending order.
        /// </summary>
        public static IList<int> Many(
            this System.Random rng,
            int n,
            int min,
            int max,
            bool sorted = false
        )
        {
            if (n == 0)
            {
                return new int[0];
            }
            if (max <= min)
            {
                throw new ArgumentException($"Can't generate {n} integers between {min} and {max}.");
            }

            List<int> output = new List<int>(n);
            for (int i = 0; i < n; ++i)
            {
                output.Add(rng.Next(min, max));
            }

            if (sorted)
            {
                output.Sort();
            }

            return output;
        }
    }
    
}