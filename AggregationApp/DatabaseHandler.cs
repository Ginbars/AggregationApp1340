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
            db.Database.ExecuteSqlRaw("DELETE FROM AData");
            foreach (var data in list)
            {
                db.Add(data);
            }
            db.SaveChanges();
        }

        public void AddNewEntry(AggregatedData data)
        {
            using AggregatedDataContext db = new();
            db.Add(data);
            db.SaveChanges();
        }

        public static List<AggregatedData> GetEntries()
        {
            using AggregatedDataContext db = new();
            return db.AData.ToList();
        }
    }
}
