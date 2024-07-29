namespace LinqPPlus;

public static class JoinExtensions
{
    /// <summary>
    /// Performs a left join on two sequences based on matching keys, producing a sequence of 
    /// tuples where each tuple contains an element from the first sequence and a matching 
    /// element from the second sequence, or a default value if no match is found.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the first sequence.</typeparam>
    /// <typeparam name="TK">The type of the elements in the second sequence.</typeparam>
    /// <param name="left">The first sequence to join.</param>
    /// <param name="right">The second sequence to join.</param>
    /// <param name="leftKeySelector">A function to extract the join key from each element of the first sequence.</param>
    /// <param name="rightKeySelector">A function to extract the join key from each element of the second sequence.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> of tuples where each tuple contains an element from 
    /// the first sequence and a matching element from the second sequence, or a default value 
    /// if no match is found in the second sequence.</returns>
    public static IEnumerable<(T LeftItem, TK rightItem)> LeftJoin<T, TK>(
        this IEnumerable<T> left,
        IEnumerable<TK> right,
        Func<T, object> leftKeySelector,
        Func<TK, object> rightKeySelector)
    {
        var rightLookp = right.ToLookup(rightKeySelector);

        foreach (var leftItem in left)
        {
            var leftKey = leftKeySelector(leftItem);
            var matches = rightLookp[leftKey];

            if (matches.Any())
            {
                foreach (var rightItem in matches)
                {
                    yield return (leftItem, rightItem);
                }
            }

            else
            {
                yield return (leftItem, default);
            }
        }
    }

    /// <summary>
    /// Performs a left join on two sequences based on matching keys, where the keys from the 
    /// first sequence are arrays. Produces a sequence of tuples where each tuple contains an 
    /// element from the first sequence and a matching element from the second sequence, or a 
    /// default value if no match is found.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the first sequence.</typeparam>
    /// <typeparam name="TK">The type of the elements in the second sequence.</typeparam>
    /// <typeparam name="TKey">The type of the keys used for joining.</typeparam>
    /// <param name="left">The first sequence to join.</param>
    /// <param name="right">The second sequence to join.</param>
    /// <param name="leftKeySelector">A function to extract the join keys from each element of the first sequence.</param>
    /// <param name="rightKeySelector">A function to extract the join key from each element of the second sequence.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> of tuples where each tuple contains an element from 
    /// the first sequence and a matching element from the second sequence, or a default value 
    /// if no match is found in the second sequence.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="left"/>, <paramref name="right"/>, 
    /// <paramref name="leftKeySelector"/>, or <paramref name="rightKeySelector"/> is <c>null</c>.</exception>
    public static IEnumerable<(T LeftItem, TK rightItem)> LeftJoinArrayIncElement<T, TK, TKey>(
        this IEnumerable<T> left,
        IEnumerable<TK> right,
        Func<T, IEnumerable<TKey>> leftKeySelector,
        Func<TK, object> rightKeySelector)
    {
        var rightLookup = right.ToLookup(rightKeySelector);

        foreach (var leftItem in left)
        {
            var leftKeys = leftKeySelector(leftItem);

            var anyMatch = false;
            foreach (var leftKey in leftKeys)
            {
                var matches = rightLookup[leftKey];
                if (matches.Any())
                {
                    anyMatch = true;
                    foreach (var rightItem in matches)
                    {
                        yield return (leftItem, rightItem);
                    }
                }
            }

            if (!anyMatch)
            {
                yield return (leftItem, default);
            }
        }
    }

    /// <summary>
    /// Performs a left join on two sequences based on matching keys, where the keys from the 
    /// second sequence are arrays. Produces a sequence of tuples where each tuple contains an 
    /// element from the first sequence and a matching element from the second sequence, or a 
    /// default value if no match is found.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the first sequence.</typeparam>
    /// <typeparam name="TK">The type of the elements in the second sequence.</typeparam>
    /// <typeparam name="TKey">The type of the keys used for joining.</typeparam>
    /// <param name="left">The first sequence to join.</param>
    /// <param name="right">The second sequence to join.</param>
    /// <param name="leftKeySelector">A function to extract the join keys from each element of the first sequence.</param>
    /// <param name="rightKeySelector">A function to extract the join key from each element of the second sequence.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> of tuples where each tuple contains an element from 
    /// the first sequence and a matching element from the second sequence, or a default value 
    /// if no match is found in the second sequence.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="left"/>, <paramref name="right"/>, 
    /// <paramref name="leftKeySelector"/>, or <paramref name="rightKeySelector"/> is <c>null</c>.</exception>
    public static IEnumerable<(T LeftItem, TK rightItem)> LeftJoinElementInArray<T, TK, TKey>(
        this IEnumerable<T> left,
        IEnumerable<TK> right,
        Func<T, TKey> leftKeySelector,
        Func<TK, IEnumerable<TKey>> rightKeySelector)
    {
        var rightLookup = right.ToLookup(rightKeySelector);

        foreach (var leftItem in left)
        {
            var leftKey = leftKeySelector(leftItem);

            var matches = rightLookup.Where(x => x.Key.Contains(leftKey)).SelectMany(x => x.Select(y => y));

            if (matches.Any())
            {
                foreach (var rightItem in matches)
                {
                    yield return (leftItem, rightItem);
                }
            }
            else
            {
                yield return (leftItem, default);
            }
        }
    }
}
