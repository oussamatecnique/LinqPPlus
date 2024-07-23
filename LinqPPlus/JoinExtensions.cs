namespace LinqPPlus;

public static class JoinExtensions
{
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
