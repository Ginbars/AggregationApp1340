using AggregationAPI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AggregationApp
{
    public class DatabaseHandler
    {
        static readonly ILogger _logger = ApiLogger.CreateLogger<DatabaseHandler>();

        public static async Task AddEntries(List<AggregatedData> list)
        {
            using AggregatedDataContext db = new();

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
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Encountered error while trying to add entries to database.");
            }
        }

        public static List<AggregatedData> GetEntries()
        {
            using AggregatedDataContext db = new();
            return db.AData.ToList();
        }

        public static void CheckMigration()
        {
            using AggregatedDataContext db = new();
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
        }
    }
}
