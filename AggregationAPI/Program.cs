using AggregationApp;

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

List<ElectricityData> data = await DataHandler.CollectData();
List<AggregatedData> aggregated = DataHandler.AggregateData(data);
DatabaseHandler.AddEntries(aggregated);

app.Run();

