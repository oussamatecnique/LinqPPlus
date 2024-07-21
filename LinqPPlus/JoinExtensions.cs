using System.Reflection;

namespace LinqPPlus;

public static class JoinExtensions
{
    public static IEnumerable<(T LeftItem, TK rightItem)> LeftJoin<T, TK>(
        this IEnumerable<T> left, 
        IEnumerable<TK> right, 
        string leftColumn, 
        string rightColumn)
    {
        var leftType = typeof(T);
        var rightType = typeof(TK);

        var leftProperty = leftType.GetProperty(leftColumn, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        if (leftProperty == null)
            throw new InvalidOperationException($"Property '{leftColumn}' not found in type '{leftType}'");

        var rightProperty = rightType.GetProperty(rightColumn, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        if (rightProperty == null)
            throw new InvalidOperationException($"Property '{rightColumn}' not found in type '{rightType}'");

        var result= left.GroupJoin(
                right, 
                leftItem => leftProperty.GetValue(leftItem, null), 
                rightItem => rightProperty.GetValue(rightItem, null),
                (leftItem, rightItems) => new { LeftItem = leftItem, RightItems = rightItems })
            .SelectMany(
                joined => joined.RightItems.DefaultIfEmpty(), 
                (joined, rightItem) => (joined.LeftItem, rightItem));

        return result;
    }
    
    public static IEnumerable<(T LeftItem, TK rightItem)> LeftJoinArrayIncElement<T, TK>(
        this IEnumerable<T> left, 
        IEnumerable<TK> right, 
        string leftColumn, 
        string rightColumn)
    {
        var leftType = typeof(T);
        var rightType = typeof(TK);
        var leftProperty = leftType.GetProperty(leftColumn, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        if (leftProperty == null)
            throw new InvalidOperationException($"Property '{leftColumn}' not found in type '{leftType}'");

        if(!leftProperty.PropertyType.IsArray || leftProperty.PropertyType.IsAssignableTo(typeof(IEnumerable<object>)))
            throw new InvalidOperationException($"Property '{leftColumn}' must be an array'");

        var rightProperty = rightType.GetProperty(rightColumn, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        if (rightProperty == null)
            throw new InvalidOperationException($"Property '{rightColumn}' not found in type '{rightType}'");
        

        Func<T, IEnumerable<object>> leftKeySelector = leftItem =>
        {
            var value = leftProperty.GetValue(leftItem);
            if (value is IEnumerable<object> enumerableValue)
            {
                return enumerableValue;
            }

            if (value is Array arrayValue)
            {
                return arrayValue.Cast<object>();
            }
            throw new InvalidOperationException($"Property '{leftColumn}' must be an array or IEnumerable");
        };
        Func<TK, object> rightKeySelector = rightItem => rightProperty.GetValue(rightItem);
        
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
                yield return (leftItem, default(TK));
            }
        }
    }
    
        public static IEnumerable<(T LeftItem, TK? rightItem)> LeftJoinElementInArray<T, TK>(
        this IEnumerable<T> left, 
        IEnumerable<TK> right, 
        string leftColumn, 
        string rightColumn)
    {
        var leftType = typeof(T);
        var rightType = typeof(TK);
        var leftProperty = leftType.GetProperty(leftColumn, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        if (leftProperty == null)
            throw new InvalidOperationException($"Property '{leftColumn}' not found in type '{leftType}'");



        var rightProperty = rightType.GetProperty(rightColumn, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        if (rightProperty == null)
            throw new InvalidOperationException($"Property '{rightColumn}' not found in type '{rightType}'");
        
        if(!rightProperty.PropertyType.IsArray || rightProperty.PropertyType.IsAssignableTo(typeof(IEnumerable<object>)))
            throw new InvalidOperationException($"Property '{rightProperty}' must be an array'");


        Func<T, object> leftKeySelector = leftItem => leftProperty.GetValue(leftItem);
        Func<TK, IEnumerable<object>> rightKeySelector = rightItem =>
        {
            var objectValue = rightProperty.GetValue(rightItem);

            if (objectValue is IEnumerable<object> enumerableObject)
                return enumerableObject;

            if (objectValue is Array arrayObject)
                return arrayObject.Cast<object>();

            throw new InvalidOperationException();
        };
        
        var rightLookup = right.ToLookup(rightKeySelector);

        foreach (var leftItem in left)
        {
            var leftKey = leftKeySelector(leftItem);
            
            var anyMatch = false;

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
