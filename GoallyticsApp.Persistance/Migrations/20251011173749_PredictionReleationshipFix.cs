using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoallyticsApp.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class PredictionReleationshipFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Predictions_Fixtures_FixturesId",
                table: "Predictions");

            migrationBuilder.DropIndex(
                name: "IX_Predictions_FixturesId",
                table: "Predictions");

            migrationBuilder.DropColumn(
                name: "FixturesId",
                table: "Predictions");

            migrationBuilder.CreateIndex(
                name: "IX_Predictions_FixtureId",
                table: "Predictions",
                column: "FixtureId");

            migrationBuilder.AddForeignKey(
                name: "FK_Predictions_Fixtures_FixtureId",
                table: "Predictions",
                column: "FixtureId",
                principalTable: "Fixtures",
                principalColumn: "FixturesId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Predictions_Fixtures_FixtureId",
                table: "Predictions");

            migrationBuilder.DropIndex(
                name: "IX_Predictions_FixtureId",
                table: "Predictions");

            migrationBuilder.AddColumn<int>(
                name: "FixturesId",
                table: "Predictions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Predictions_FixturesId",
                table: "Predictions",
                column: "FixturesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Predictions_Fixtures_FixturesId",
                table: "Predictions",
                column: "FixturesId",
                principalTable: "Fixtures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
