using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AggregationApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AData",
                columns: table => new
                {
                    Region = table.Column<string>(type: "TEXT", nullable: false),
                    PPlusSum = table.Column<float>(type: "REAL", nullable: false),
                    PMinusSum = table.Column<float>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AData", x => x.Region);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AData");
        }
    }
}
