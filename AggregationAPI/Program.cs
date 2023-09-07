using AggregationApp;
using Microsoft.EntityFrameworkCore;

string[] _dataPaths = new string[4]
{
    "https://data.gov.lt/dataset/1975/download/10763/2022-02.csv",
    "https://data.gov.lt/dataset/1975/download/10764/2022-03.csv",
    "https://data.gov.lt/dataset/1975/download/10765/2022-04.csv",
    "https://data.gov.lt/dataset/1975/download/10766/2022-05.csv"
};
ILogger _logger = ApiLogger.CreateLogger<Program>();


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AggregatedDataContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/aggregateddata", async (AggregatedDataContext db) =>
{
    return await DatabaseHandler.GetEntries(db);
})
.WithName("GetAggregatedData")
.WithOpenApi();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AggregatedDataContext>();

    DatabaseHandler.CheckMigration(db);
    var electricityData = new List<ElectricityData>();

    foreach (var url in _dataPaths)
    {
        try
        {
            var data = await DataHandler.DownloadData(url, new HttpClient());
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
    await DatabaseHandler.AddEntries(aggregated, db);
}

app.Run();

