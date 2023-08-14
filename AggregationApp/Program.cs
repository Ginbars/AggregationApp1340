// See https://aka.ms/new-console-template for more information
using AggregationApp;

Console.WriteLine("Hello, World!");

var data = await DataHandler.CollectData();
var aggregated = DataHandler.AggregateData(data);

DatabaseHandler.AddNewEntries(aggregated);

Console.WriteLine(DatabaseHandler.GetEntries());
