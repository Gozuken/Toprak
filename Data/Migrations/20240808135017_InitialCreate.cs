using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Verim.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Assets",
                columns: table => new
                {
                    AssetID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    AssetType = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    ProvinceName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    DistrictName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    NeighborhoodName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    BlockNumber = table.Column<string>(type: "TEXT", nullable: false),
                    ParcelNumber = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assets", x => x.AssetID);
                    table.ForeignKey(
                        name: "FK_Assets_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Password", "Username" },
                values: new object[,]
                {
                    { 1, "password", "admin" },
                    { 2, "C#asp.netdatabase123", "Ahmet" },
                    { 3, "ilovegaming321", "coolbob" }
                });

            migrationBuilder.InsertData(
                table: "Assets",
                columns: new[] { "AssetID", "AssetType", "BlockNumber", "DistrictName", "NeighborhoodName", "ParcelNumber", "ProvinceName", "UserId" },
                values: new object[,]
                {
                    { 1, "tarla", "6", "golbasi", "segmenler", "5", "ankara", 1 },
                    { 2, "bag", "4", "bozoyuk", "yildirim", "3", "bursa", 2 },
                    { 3, "bahce", "1", "cayyolu", "demiroren", "2", "kars", 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assets_UserId",
                table: "Assets",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Assets");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
