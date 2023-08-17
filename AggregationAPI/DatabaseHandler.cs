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
        public static void AddEntries(List<AggregatedData> list)
        {
            using AggregatedDataContext db = new();

            foreach (var data in list)
            {
                if (db.AData.Find(data.Region) is AggregatedData ad)
                {
                    ad.PPlusSum = data.PPlusSum;
                    ad.PMinusSum = data.PMinusSum;
                }
                else
                {
                    db.Add(data);
                }
            }
            db.SaveChanges();
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
            if (applied.Length == 0)
            {
                db.Database.Migrate();
                return;
            }
            var defined = db.Database.GetMigrations().ToArray();
            if (applied.Length != defined.Length)
            {
                db.Database.Migrate();
                return;
            }
            for (int i = 0; i < defined.Length; i++)
            {
                if (defined[i] != applied[i])
                {
                    db.Database.Migrate();
                    return;
                }
            }
        }
    }
}
