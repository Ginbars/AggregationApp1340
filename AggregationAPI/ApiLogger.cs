namespace AggregationAPI
{
    public static class ApiLogger
    {
        public static ILoggerFactory StaticLoggerFactory { get; set; } = LoggerFactory.Create(builder =>
        {
            builder.AddConsole(configure: config => { builder.SetMinimumLevel(LogLevel.Warning); });
        });

        public static ILogger CreateLogger<T>() => StaticLoggerFactory.CreateLogger<T>();
        public static ILogger CreateLogger(string categoryName) => StaticLoggerFactory.CreateLogger(categoryName);
    }
}
