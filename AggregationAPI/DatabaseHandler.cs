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
        public static void AddNewEntries(List<AggregatedData> list)
        {
            using AggregatedDataContext db = new();

            if (CheckMigration())
            {
                Console.WriteLine("Up to date");
                db.Database.ExecuteSqlRaw("DELETE FROM AData");
            }
            else
            {
                db.Database.Migrate();
            }

            foreach (var data in list)
            {
                db.Add(data);
            }
            db.SaveChanges();
        }

        public static List<AggregatedData> GetEntries()
        {
            using AggregatedDataContext db = new();
            return db.AData.ToList();
        }

        private static bool CheckMigration()
        {
            using AggregatedDataContext db = new();
            var applied = db.Database.GetAppliedMigrations().ToArray();
            if (applied.Length == 0)
            {
                return false;
            }
            var defined = db.Database.GetMigrations().ToArray();
            if (applied.Length != defined.Length)
            {
                return false;
            }
            for (int i = 0; i < defined.Length; i++)
            {
                if (defined[i] != applied[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
