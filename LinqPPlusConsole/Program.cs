// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using LinqPPlus;
using LinqPPlusConsole;
using LinqPPlusConsole.Models;

/*var students = new List<Student>
{
    new () {Name = "Ayman", Age = 30, ProfId = 1},
    new () {Name = "Ahmed", Age = 30, ProfId = 1},
    new () {Name = "Omar", Age = 30, ProfId = 2},
    new () {Name = "Salim", Age = 30, ProfId = 4}
};

var profs = new List<Professor>
{
    new() {Id = 1, Name = "Mustapha"},
    new() {Id = 2, Name = "Mohammed"}
};

var resultOfJoin = students.LeftJoin(profs, "profId", "Id").Select(x => new
{
    Student = x.LeftItem.Name,
    Professor = x.rightItem?.Name
});
Console.WriteLine(JsonSerializer.Serialize(resultOfJoin));
Console.ReadLine();*/

var students = new List<Student2>
{
    new () {Name = "Ayman", Age = 30, ProfIds = [1, 2]},
    new () {Name = "Ahmed", Age = 30, ProfIds = [1, 8]},
    new () {Name = "Omar", Age = 30, ProfIds = [2]},
    new () {Name = "Salim", Age = 30, ProfIds = [4, 9]}
};

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
Console.WriteLine(JsonSerializer.Serialize(resultOfJoin1));
Console.WriteLine(JsonSerializer.Serialize(resultOfJoin2));
Console.ReadLine();

