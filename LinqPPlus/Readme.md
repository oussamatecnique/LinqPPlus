# LinqPPlus

This package provides additional LINQ extensions to enhance the functionality of LINQ queries. Below are the descriptions and examples for each method.

## Installation

```bash
dotnet add package LinqPPlus
```

## Methods

### 1. `ExceptByOccurrences`

**Description:**
Returns the elements of the first sequence that do not appear in the second sequence, taking into account the number of occurrences of each element.

**Type Parameters:**
- `T`: The type of the elements of the input sequences.
- `TKey`: The type of the key returned by the key selector.

**Parameters:**
- `list1`: The first sequence.
- `list2`: The second sequence, used to exclude elements from the first sequence.
- `keySelector`: A function to extract the key for each element in the first sequence.

**Returns:**
An `IEnumerable<T>` that contains the elements from the input sequence `list1` that do not appear in the second sequence `list2`, taking into account the number of occurrences of each element.

**Example:**

```csharp
var list1 = new List<int> { 1, 2, 2, 3 };
var list2 = new List<int> { 2, 3 };

var result = list1.ExceptByOccurrences(list2, x => x);

// result: [1, 2]
```
### 2. `LeftJoin`

**Description:**
Performs a left join on two sequences based on matching keys, producing a sequence of tuples where each tuple contains an element from the first sequence and a matching element from the second sequence, or a default value if no match is found.

**Type Parameters:**
- `T`: The type of the elements in the first sequence.
- `TK`: The type of the elements in the second sequence

**Parameters:**

- `left`: The first sequence to join.
- `right`: The second sequence to join.
- `leftKeySelector`: A function to extract the join key from each element of the first sequence.
- `rightKeySelector`: A function to extract the join key from each element of the second sequence.

**Returns:**
An IEnumerable<(T LeftItem, TK RightItem)> of tuples where each tuple contains an element from the first sequence and a matching element from the second sequence, or a default value if no match is found in the second sequence.

**Example:**

```csharp
Copier le code
var left = new List<string> { "a", "b", "c" };
var right = new List<int> { 1, 2, 3 };

var result = left.LeftJoin(right, x => x, y => y.ToString());

// result: [("a", default), ("b", default), ("c", default)]
```

### 3. `LeftJoinArrayIncElement`

**Description:**
Performs a left join on two sequences based on matching keys, where the keys from the first sequence are arrays. This method produces a sequence of tuples where each tuple contains an element from the first sequence and a matching element from the second sequence. If no match is found, a default value is returned for the second sequence element.

**Type Parameters:**
- `T`: The type of the elements in the first sequence.
- `TK`: The type of the elements in the second sequence.
- `TKey`: The type of the keys used for joining.

**Parameters:**
- `left`: The first sequence to join. This sequence provides elements that will be joined with elements from the second sequence based on matching keys.
- `right`: The second sequence to join. This sequence is used to find matching elements based on the keys extracted from the first sequence.
- `leftKeySelector`: A function to extract the join keys from each element of the first sequence. This function returns an `IEnumerable<TKey>` of keys for each element.
- `rightKeySelector`: A function to extract the join key from each element of the second sequence.

**Returns:**
An `IEnumerable<(T LeftItem, TK RightItem)>` of tuples where each tuple contains:
- An element from the first sequence.
- A matching element from the second sequence, or a default value if no match is found in the second sequence.

**Exceptions:**
- `ArgumentNullException`: Thrown when `left`, `right`, `leftKeySelector`, or `rightKeySelector` is `null`.

**Example:**

```csharp
var students = new List<Student2>
{
    new () {Name = "John", Age = 30, ProfIds = [1, 2]},
    new () {Name = "Mark", Age = 30, ProfIds = [1, 8]},
    new () {Name = "Paul", Age = 30, ProfIds = [2]},
    new () {Name = "George", Age = 30, ProfIds = [4, 9]}
};

var profs = new List<Professor>
{
    new() {Id = 1, Name = "Philip"},
    new() {Id = 2, Name = "Max"},
    new () {Id = 12, Name = "Arnold"}
};


var resultOfJoin1 = students.LeftJoinArrayIncElement(profs, x => x.ProfIds.AsEnumerable(), x => x.Id).Select(x => new
{
    Student = x.LeftItem.Name,
    Professor = x.rightItem?.Name
});

// resultOfJoin1: [{"Student":"John","Professor":"Philip"},{"Student":"John","Professor":"Max"},{"Student":"Mark","Professor":"Philip"},{"Student":"Paul","Professor":"Max"},{"Student":"George","Professor":null}]

```

### 4. `LeftJoinElementInArray`

**Description:**
Performs a left join on two sequences based on matching keys, where the keys from the second sequence are arrays. This method produces a sequence of tuples where each tuple contains an element from the first sequence and a matching element from the second sequence. If no match is found, a default value is returned for the second sequence element.

**Type Parameters:**
- `T`: The type of the elements in the first sequence.
- `TK`: The type of the elements in the second sequence.
- `TKey`: The type of the keys used for joining.

**Parameters:**
- `left`: The first sequence to join. This sequence provides elements that will be joined with elements from the second sequence based on matching keys.
- `right`: The second sequence to join. This sequence is used to find matching elements based on the keys extracted from the first sequence.
- `leftKeySelector`: A function to extract the join key from each element of the first sequence. This function returns a single key for each element.
- `rightKeySelector`: A function to extract an array of keys from each element of the second sequence.

**Returns:**
An `IEnumerable<(T LeftItem, TK RightItem)>` of tuples where each tuple contains:
- An element from the first sequence.
- A matching element from the second sequence, or a default value if no match is found in the second sequence.

**Exceptions:**
- `ArgumentNullException`: Thrown when `left`, `right`, `leftKeySelector`, or `rightKeySelector` is `null`.

**Example:**

```csharp
var left = new List<string>
{
    "John",
    "Paul",
    "George",
    "Ringo"
};

var right = new List<(int Id, string Name)>
{
    (1, "John"),
    (2, "Paul"),
    (3, "George")
};

// Join left sequence based on string names with the right sequence names
var result = left.LeftJoinElementInArray(
    right,
    x => x, // Use the string as the key
    y => new[] { y.Name } // Extract keys from the right sequence
);

// Result: [("John", (1, "John")), ("Paul", (2, "Paul")), ("George", (3, "George")), ("Ringo", default)]
```

## Licence
This project is licensed under the MIT License - see the LICENSE file for details.
