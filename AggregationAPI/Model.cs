using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AggregationApp
{
    public class AggregatedDataContext : DbContext
    {
        public DbSet<AggregatedData> AData => Set<AggregatedData>();

        public AggregatedDataContext(DbContextOptions<AggregatedDataContext> options) : base(options)
        { }
    }

    public class AggregatedData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
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
