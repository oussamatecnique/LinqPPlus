# LinqPPlus

this library contains additional linq extensions that doesn't exist by default, and help you with your data
handling and operations.

## Installation

```bash
dotnet add package LinqPPlus
```

## Usage
```bash
var profs = new List<Professor>
{
    new() {Id = 1, Name = "Mustapha"},
    new() {Id = 2, Name = "Mohammed"},
    new () {Id = 12, Name = "Simo"}
};


var resultOfJoin1 = students.LeftJoinArrayIncElement(profs, x => x.ProfIds.AsEnumerable(), x => x.Id).Select(x => new
{
    Student = x.LeftItem.Name,
    Professor = x.rightItem?.Name
});
var resultOfJoin2 = profs.LeftJoinElementInArray(students, x => x.Id, x => x.ProfIds.Select(x => float.Parse(x.ToString()))).Select(x => new
{
    Professor= x.LeftItem.Name,
    Student = x.rightItem?.Name
});
```
in that example we can operate left joins on already enumerable data (not on database tables),
so if you have a database query consider converting with .AsEnumerable() before applying these extensions.
Left join can be matched between single element with an array and vice versa.

## Licence
This project is licensed under the MIT License - see the LICENSE file for details.
