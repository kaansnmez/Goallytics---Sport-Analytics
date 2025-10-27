using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoallyticsApp.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class InitialPrediction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Predictions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FixtureId = table.Column<int>(type: "int", nullable: false),
                    LeaugeId = table.Column<int>(type: "int", nullable: false),
                    DateUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HomeTeamId = table.Column<int>(type: "int", nullable: false),
                    AwayTeamId = table.Column<int>(type: "int", nullable: false),
                    Market = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Forecast = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Confidence = table.Column<double>(type: "float", nullable: false),
                    ExpectedGoals = table.Column<double>(type: "float", nullable: false),
                    HomeExpected = table.Column<double>(type: "float", nullable: false),
                    AwayExpected = table.Column<double>(type: "float", nullable: false),
                    AnalysisJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Result = table.Column<bool>(type: "bit", nullable: false),
                    FixturesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Predictions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Predictions_Fixtures_FixturesId",
                        column: x => x.FixturesId,
                        principalTable: "Fixtures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Predictions_FixturesId",
                table: "Predictions",
                column: "FixturesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Predictions");
        }
    }
}
