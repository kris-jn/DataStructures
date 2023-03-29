// See https://aka.ms/new-console-template for more information
using DataStructures;
using static DataStructures.BoyerSearch;
/*
Console.WriteLine("Hello, World!");
var items = new List<int>();
var random = new Random();    
for (int i = 0; i < 5; i++)
{
items.Add(random.Next(0, 1000));
}
var mergeSort = new MergeSort<int>();
var quickSort = new QuickSort<int>();
Console.WriteLine(String.Join(",", items.ToArray()));
items = quickSort.MetricSort(items.ToArray()).ToList();
Console.WriteLine(String.Join(",", items.ToArray()));
*/

var val = Console.ReadLine();
var arr = val.Split('-');
Console.WriteLine(StringReplace.Replace(new BoyerSearch(), arr[0], arr[1], arr[2]));