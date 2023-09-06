using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AggregationAPI.Migrations
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
                    Region = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PPlusSum = table.Column<float>(type: "real", nullable: false),
                    PMinusSum = table.Column<float>(type: "real", nullable: false)
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
