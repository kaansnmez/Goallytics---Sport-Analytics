using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GoallyticsApp.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class InitialDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Definition = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Definition = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Leagues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeagueApiId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leagues", x => x.Id);
                    table.UniqueConstraint("AK_Leagues_LeagueApiId", x => x.LeagueApiId);
                });

            migrationBuilder.CreateTable(
                name: "Seasons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeasonApiId = table.Column<int>(type: "int", nullable: false),
                    SeasonYear = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seasons", x => x.Id);
                    table.UniqueConstraint("AK_Seasons_SeasonApiId", x => x.SeasonApiId);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeamApiId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Founded = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    National = table.Column<bool>(type: "bit", nullable: true),
                    Logo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                    table.UniqueConstraint("AK_Teams_TeamApiId", x => x.TeamApiId);
                });

            migrationBuilder.CreateTable(
                name: "AppUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AppRoleId = table.Column<int>(type: "int", nullable: false),
                    GenderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppUsers_Genders_GenderId",
                        column: x => x.GenderId,
                        principalTable: "Genders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rounds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeagueApiId = table.Column<int>(type: "int", nullable: false),
                    SeasonApiId = table.Column<int>(type: "int", nullable: false),
                    RoundApiId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rounds", x => x.Id);
                    table.UniqueConstraint("AK_Rounds_RoundApiId", x => x.RoundApiId);
                    table.ForeignKey(
                        name: "FK_Rounds_Leagues_LeagueApiId",
                        column: x => x.LeagueApiId,
                        principalTable: "Leagues",
                        principalColumn: "LeagueApiId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rounds_Seasons_SeasonApiId",
                        column: x => x.SeasonApiId,
                        principalTable: "Seasons",
                        principalColumn: "SeasonApiId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppUserRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppUserId = table.Column<int>(type: "int", nullable: false),
                    AppRoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppUserRoles_AppRoles_AppRoleId",
                        column: x => x.AppRoleId,
                        principalTable: "AppRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppUserRoles_AppUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Fixtures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FixturesId = table.Column<int>(type: "int", nullable: false),
                    LeagueApiId = table.Column<int>(type: "int", nullable: false),
                    SeasonApiId = table.Column<int>(type: "int", nullable: false),
                    RoundApiId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    HomeTeamId = table.Column<int>(type: "int", nullable: false),
                    AwayTeamId = table.Column<int>(type: "int", nullable: false),
                    DateUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StatusShort = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HomeScore = table.Column<int>(type: "int", nullable: true),
                    AwayScore = table.Column<int>(type: "int", nullable: true),
                    Day = table.Column<int>(type: "int", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fixtures", x => x.Id);
                    table.UniqueConstraint("AK_Fixtures_FixturesId_HomeTeamId", x => new { x.FixturesId, x.HomeTeamId });
                    table.ForeignKey(
                        name: "FK_Fixtures_Leagues_LeagueApiId",
                        column: x => x.LeagueApiId,
                        principalTable: "Leagues",
                        principalColumn: "LeagueApiId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Fixtures_Rounds_RoundApiId",
                        column: x => x.RoundApiId,
                        principalTable: "Rounds",
                        principalColumn: "RoundApiId");
                    table.ForeignKey(
                        name: "FK_Fixtures_Seasons_SeasonApiId",
                        column: x => x.SeasonApiId,
                        principalTable: "Seasons",
                        principalColumn: "SeasonApiId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Fixtures_Teams_AwayTeamId",
                        column: x => x.AwayTeamId,
                        principalTable: "Teams",
                        principalColumn: "TeamApiId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Fixtures_Teams_HomeTeamId",
                        column: x => x.HomeTeamId,
                        principalTable: "Teams",
                        principalColumn: "TeamApiId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FixtureStat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FixtureId = table.Column<int>(type: "int", nullable: false),
                    TeamId = table.Column<int>(type: "int", nullable: false),
                    TeamName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HomeTeamId = table.Column<int>(type: "int", nullable: false),
                    AwayTeamId = table.Column<int>(type: "int", nullable: false),
                    HomeScore = table.Column<int>(type: "int", nullable: true),
                    AwayScore = table.Column<int>(type: "int", nullable: true),
                    LeagueApiId = table.Column<int>(type: "int", nullable: false),
                    SeasonApiId = table.Column<int>(type: "int", nullable: false),
                    Shots_on_Goal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Shots_off_Goal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Total_Shots = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Blocked_Shots = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Shots_insidebox = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Shots_outsidebox = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fouls = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Corner_Kicks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Offsides = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ball_Possession = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Yellow_Cards = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Red_Cards = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Goalkeeper_Saves = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Total_passes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Passes_accurate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Passes_Percent = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FixtureStat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FixtureStat_Fixtures_FixtureId_HomeTeamId",
                        columns: x => new { x.FixtureId, x.HomeTeamId },
                        principalTable: "Fixtures",
                        principalColumns: new[] { "FixturesId", "HomeTeamId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AppRoles",
                columns: new[] { "Id", "Definition" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "Member" }
                });

            migrationBuilder.InsertData(
                table: "Genders",
                columns: new[] { "Id", "Definition" },
                values: new object[,]
                {
                    { 1, "Male" },
                    { 2, "Female" },
                    { 3, "Prefer not to say" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUserRoles_AppRoleId",
                table: "AppUserRoles",
                column: "AppRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserRoles_AppUserId_AppRoleId",
                table: "AppUserRoles",
                columns: new[] { "AppUserId", "AppRoleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_Email",
                table: "AppUsers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_GenderId",
                table: "AppUsers",
                column: "GenderId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_UserName",
                table: "AppUsers",
                column: "UserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Fixtures_AwayTeamId",
                table: "Fixtures",
                column: "AwayTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Fixtures_HomeTeamId",
                table: "Fixtures",
                column: "HomeTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Fixtures_LeagueApiId",
                table: "Fixtures",
                column: "LeagueApiId");

            migrationBuilder.CreateIndex(
                name: "IX_Fixtures_RoundApiId",
                table: "Fixtures",
                column: "RoundApiId");

            migrationBuilder.CreateIndex(
                name: "IX_Fixtures_SeasonApiId",
                table: "Fixtures",
                column: "SeasonApiId");

            migrationBuilder.CreateIndex(
                name: "IX_FixtureStat_FixtureId_HomeTeamId",
                table: "FixtureStat",
                columns: new[] { "FixtureId", "HomeTeamId" });

            migrationBuilder.CreateIndex(
                name: "IX_FixtureStat_FixtureId_TeamId",
                table: "FixtureStat",
                columns: new[] { "FixtureId", "TeamId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rounds_LeagueApiId",
                table: "Rounds",
                column: "LeagueApiId");

            migrationBuilder.CreateIndex(
                name: "IX_Rounds_SeasonApiId",
                table: "Rounds",
                column: "SeasonApiId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUserRoles");

            migrationBuilder.DropTable(
                name: "FixtureStat");

            migrationBuilder.DropTable(
                name: "AppRoles");

            migrationBuilder.DropTable(
                name: "AppUsers");

            migrationBuilder.DropTable(
                name: "Fixtures");

            migrationBuilder.DropTable(
                name: "Genders");

            migrationBuilder.DropTable(
                name: "Rounds");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Leagues");

            migrationBuilder.DropTable(
                name: "Seasons");
        }
    }
}
