using System.Collections.Generic;

namespace JRPG.Utils
{
    public static class KeyValuePairExtensions
    {
        public static void Deconstruct<T, U>(this KeyValuePair<T, U> k, out T t, out U u) {
            t = k.Key; u = k.Value;
        }
    }
}