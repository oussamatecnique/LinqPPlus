namespace LinqPPlus;

public static class FilterExtensions
{

    public static IEnumerable<T> DistinctByKey<T, TKey>(this IEnumerable<T> list, Func<T, TKey> keySelector)
    {
        ArgumentNullException.ThrowIfNull(list);

        ArgumentNullException.ThrowIfNull(keySelector);


        var distinctValues = new HashSet<TKey>();

        foreach (var element in list)
        {
            if (distinctValues.Add(keySelector(element)))
            {
                yield return element;
            }
        }
    }

    /// <summary>
    /// Returns the elements of the first sequence that do not appear in the second sequence,
    /// taking into account the number of occurrences of each element.
    /// </summary>
    /// <typeparam name="T">The type of the elements of the input sequences.</typeparam>
    /// <typeparam name="TKey">The type of the key returned by the key selector.</typeparam>
    /// <param name="list1">The first sequence.</param>
    /// <param name="list2">The second sequence, used to exclude elements from the first sequence.</param>
    /// <param name="keySelector">A function to extract the key for each element in the first sequence.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> that contains the elements from the input sequence 
    /// <paramref name="list1"/> that do not appear in the second sequence <paramref name="list2"/>, 
    /// taking into account the number of occurrences of each element.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="list1"/>, <paramref name="list2"/>, 
    /// or <paramref name="keySelector"/> is <c>null</c>.</exception>
    public static IEnumerable<T> ExceptByOccurrences<T, TKey>(this IEnumerable<T> list1, IEnumerable<TKey> list2,
        Func<T, TKey> keySelector)
    {
        ArgumentNullException.ThrowIfNull(list1);
        ArgumentNullException.ThrowIfNull(list2);
        if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));

        // Use a dictionary to count occurrences in list2
        var list2Counts = list2.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count());

        foreach (var element1 in list1)
        {
            var element1Key = keySelector(element1);

            if (list2Counts.TryGetValue(element1Key, out var value) && value > 0)
            {
                list2Counts[element1Key] = value - 1;
            }

            else
            {
                yield return element1;
            }
        }
    }
}
