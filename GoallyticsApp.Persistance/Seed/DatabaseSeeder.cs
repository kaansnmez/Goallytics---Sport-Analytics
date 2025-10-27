using CsvHelper;
using CsvHelper.TypeConversion;
using GoallyticsApp.Domain.Entities.MatchEntities;
using GoallyticsApp.Persistance.Context;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoallyticsApp.Persistance.Seed
{
    public static class PersistenceSeedingExtensions
    {
        public static async Task SeedIfNeededAsync(this IServiceCollection services, IConfiguration configuration, string csvRoot)
        {

            // DI kablolama (DbContext vs.) hazır olmalı
            using var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();

            var ctx = scope.ServiceProvider.GetRequiredService<MatchContext>();
            // Commit işlemi
            await ctx.Database.MigrateAsync();
            bool needsSeed =
                !await ctx.Set<Leagues>().AnyAsync() ||
                !await ctx.Set<Season>().AnyAsync() ||
                !await ctx.Set<Team>().AnyAsync() ||
                !await ctx.Set<Round>().AnyAsync() ||
                !await ctx.Set<Fixtures>().AnyAsync();

            var fixture = await ctx.Fixtures
                .Include(f => f.HomeTeam)
                .Include(f => f.AwayTeam)
                .Include(f => f.League)
                .Include(f => f.Season)
                .Include(f => f.Round)
                .FirstOrDefaultAsync();
            Console.WriteLine($"Fixture: {fixture?.HomeTeamId} vs {fixture?.AwayTeamId}, Date: {fixture?.DateUtc}");
            if (!needsSeed)
                return;
            var solutionDir = Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.Parent.FullName;

            var root = Path.IsPathRooted(csvRoot)
            ? csvRoot
            : Path.Combine(solutionDir, csvRoot);
        
            // (İsteğe bağlı) doğrulama
            if (!Directory.Exists(root))
                throw new DirectoryNotFoundException(root);

            await BulkLoadAsync(ctx, Path.Combine(root, "leagues.csv"), "Leagues");
            await BulkLoadAsync(ctx, Path.Combine(root, "seasons.csv"), "Seasons");
            await BulkLoadAsync(ctx, Path.Combine(root, "teams.csv"), "Teams");
            await BulkLoadAsync(ctx, Path.Combine(root, "rounds.csv"), "Rounds");
            await BulkLoadAsync(ctx, Path.Combine(root, "fixtures_1.csv"), "Fixtures");
            await BulkLoadAsync(ctx, Path.Combine(root, "Fstats.csv"), "FixtureStat");


            await ctx.SaveChangesAsync();
            /*
            foreach (var task in process)
            {
                try
                { await task; }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
            }*/

        }

        static async Task BulkLoadAsync(DbContext ctx, string csvPath, string table)
        {
            var config = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture)
            {
                MissingFieldFound = null,
                PrepareHeaderForMatch = args => args.Header.Trim(),
            };


            using var reader = new StreamReader(csvPath, Encoding.UTF8);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            using var dr = new CsvDataReader(csv);
            
           


            var conn = (SqlConnection)ctx.Database.GetDbConnection();
            var opened = conn.State != ConnectionState.Open;
            if (opened)
                await conn.OpenAsync();

            using var bulk = new SqlBulkCopy(conn,SqlBulkCopyOptions.KeepIdentity, null) { DestinationTableName = $"dbo.{table}", BulkCopyTimeout = 0 };
            await bulk.WriteToServerAsync(dr);
           
            if (opened)
                await conn.CloseAsync();
        }
        
    }
}
