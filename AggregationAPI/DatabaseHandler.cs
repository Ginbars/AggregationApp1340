﻿using Microsoft.EntityFrameworkCore;

namespace AggregationApp
{
    public class DatabaseHandler
    {
        static readonly ILogger _logger = ApiLogger.CreateLogger<DatabaseHandler>();

        public static async Task AddEntries(List<AggregatedData> list, AggregatedDataContext db)
        {
            foreach (var data in list)
            {
                if (db.AData.Find(data.Region) is AggregatedData ad)
                {
                    _logger.LogWarning("Existing entry for {region} found, updating values.", data.Region);
                    ad.PPlusSum = data.PPlusSum;
                    ad.PMinusSum = data.PMinusSum;
                }
                else
                {
                    db.Add(data);
                }
            }

            try
            {
                await db.SaveChangesAsync();
                _logger.LogInformation("Added entries to the database.");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Encountered error while trying to add entries to database.");
            }
        }

        public static Task<List<AggregatedData>> GetEntries(AggregatedDataContext db)
        {
            return db.AData.ToListAsync();
        }

        public static void CheckMigration(AggregatedDataContext db)
        {
            var applied = db.Database.GetAppliedMigrations().ToArray();
            var defined = db.Database.GetMigrations().ToArray();
            if (applied.Length != defined.Length)
            {
                _logger.LogWarning("Database migration inconsitencies found, migrating.");
                db.Database.Migrate();
                return;
            }
            for (int i = 0; i < defined.Length; i++)
            {
                if (defined[i] != applied[i])
                {
                    _logger.LogWarning("Database migration inconsitencies found, migrating.");
                    db.Database.Migrate();
                    return;
                }
            }
            _logger.LogInformation("Database migration is consistent.");
        }
    }
}
