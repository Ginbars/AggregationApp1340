using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace AggregationApp
{
    public class AggregatedDataContext : DbContext
    {
        public DbSet<AggregatedData> AData { get; set; }

        public string DbPath { get; }

        public AggregatedDataContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "edata.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }

    public class AggregatedData
    {
        [Key]
        public required string Region { get; set; }
        public float PPlusSum { get; set; }
        public float PMinusSum { get; set; }

        public AggregatedData AddAggregation(ElectricityData data)
        {
            PPlusSum += data.PPlus;
            PMinusSum += data.PMinus;

            return this;
        }
    }
}
