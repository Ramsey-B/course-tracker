using System;
using System.Collections.Generic;
using System.Linq;

namespace course_tracker.Models.extensions
{
    public static class CollectionExtensions
    {
        public static bool Remove<T>(this ICollection<T> collection, Func<T, bool> predicate)
        {
            var item = collection.First(i => predicate(i));
            return collection.Remove(item);
        }

        public static ICollection<T> Replace<T>(this ICollection<T> collection, Func<T, bool> predicate, T newItem)
        {
            collection.Remove(predicate);
            collection.Add(newItem);
            return collection;
        }
    }
}
