using System;
using System.Collections.Generic;
using System.Linq;
namespace Util {
    public static class EnumeratorUtil
    {
        public static void ForEachIndex<T>(this IEnumerable<T> enumeration, Action<int, T> action) {
            for (int i = 0; i < enumeration.Count(); i++) {
                action(i, enumeration.ElementAt(i));
            }
        }
    
        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (T obj in enumeration)
                action(obj);
        }

        public static void ForEach<T, U>(this IDictionary<T, U> dictionary, Action<T, U> act) {
            foreach (var kvp in dictionary) {
                act(kvp.Key, kvp.Value);
            }
        }

        public static IEnumerable<U> Select<T, U>(this IEnumerable<T> enumeration, Func<T, U> function) {
            List<U> l = new List<U>();
            foreach (var a in enumeration) {
                l.Add(function(a));
            }
            return l;
        }

        public static T Find<T>(this IEnumerable<T> enumeration, Func<T, bool> c) {
            foreach (var a in enumeration) {
                if (c(a))
                    return a;
            }
            return default;
        }
    }
}
