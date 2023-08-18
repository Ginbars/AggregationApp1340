using AggregationApp;

ILogger _logger = ApiLogger.CreateLogger<Program>();
string[] _dataPaths = new string[4]
{
    "https://data.gov.lt/dataset/1975/download/10763/2022-02.csv",
    "https://data.gov.lt/dataset/1975/download/10764/2022-03.csv",
    "https://data.gov.lt/dataset/1975/download/10765/2022-04.csv",
    "https://data.gov.lt/dataset/1975/download/10766/2022-05.csv"
};


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/aggregateddata", () =>
{
    return DatabaseHandler.GetEntries();
})
.WithName("GetAggregatedData")
.WithOpenApi();

DatabaseHandler.CheckMigration();
var electricityData = new List<ElectricityData>();

foreach (var url in _dataPaths)
{
    try
    {
        var data = await DataFetcher.DownloadData(url);
        var processed = DataHandler.ProcessData(data);
        DataHandler.FilterByObjName(processed, "Butas");
        electricityData.AddRange(processed);
    }
    catch (Exception e)
    {
        _logger.LogError(e, "Exception caught while trying to download data from {url}", url);
        continue;
    }
}

_logger.LogInformation("Finished collecting and processing data");
var aggregated = DataHandler.AggregateData(electricityData);
await DatabaseHandler.AddEntries(aggregated);


app.Run();

