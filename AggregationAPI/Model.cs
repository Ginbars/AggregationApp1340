using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

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

        public override bool Equals(object? obj)
        {
            return obj is AggregatedData ad && (ad.Region, ad.PPlusSum, ad.PMinusSum).Equals((Region, PPlusSum, PMinusSum));
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Region, PPlusSum, PMinusSum);
        }
    }
}
