// See https://aka.ms/new-console-template for more information
using AggregationApp;

Console.WriteLine("Hello, World!");

var data = await DataHandler.CollectData();
var aggregated = DataHandler.AggregateData(data);

DatabaseHandler.AddNewEntries(aggregated);

var stored = DatabaseHandler.GetEntries();

foreach (var item in stored)
{
    Console.WriteLine(item);
}
