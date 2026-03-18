using Microsoft.EntityFrameworkCore;
using SportsStatsApi.Models;

namespace SportsStatsApi.Data;

/// <summary>
/// Entity Framework Core database context for the Sports Stats API.
/// Manages Fighter entities and seeds initial data.
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Fighter> Fighters => Set<Fighter>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Fighter entity
        modelBuilder.Entity<Fighter>(entity =>
        {
            entity.HasIndex(f => f.Name).IsUnique();
            entity.HasIndex(f => f.Division);
            entity.HasIndex(f => f.Ranking);
        });

        // Seed data with real UFC fighters
        modelBuilder.Entity<Fighter>().HasData(GetSeedData());
    }

    /// <summary>
    /// Returns seed data with real UFC fighter statistics.
    /// </summary>
    private static Fighter[] GetSeedData()
    {
        return new Fighter[]
        {
            // Heavyweight Division
            new()
            {
                Id = 1, Name = "Jon Jones", Nickname = "Bones",
                Wins = 27, Losses = 1, Draws = 0,
                Division = "Heavyweight", Height = 193.04m, Reach = 215.90m,
                KOPercentage = 33.33m, SubmissionPercentage = 25.93m,
                Country = "United States", Age = 37, IsActive = true, Ranking = 1
            },
            new()
            {
                Id = 2, Name = "Tom Aspinall", Nickname = "The Honey Badger",
                Wins = 15, Losses = 3, Draws = 0,
                Division = "Heavyweight", Height = 195.58m, Reach = 198.12m,
                KOPercentage = 73.33m, SubmissionPercentage = 13.33m,
                Country = "United Kingdom", Age = 31, IsActive = true, Ranking = 2
            },
            new()
            {
                Id = 3, Name = "Ciryl Gane", Nickname = "Bon Gamin",
                Wins = 12, Losses = 2, Draws = 0,
                Division = "Heavyweight", Height = 193.04m, Reach = 203.20m,
                KOPercentage = 50.00m, SubmissionPercentage = 8.33m,
                Country = "France", Age = 34, IsActive = true, Ranking = 3
            },
            new()
            {
                Id = 4, Name = "Curtis Blaydes", Nickname = "Razor",
                Wins = 18, Losses = 4, Draws = 0,
                Division = "Heavyweight", Height = 193.04m, Reach = 203.20m,
                KOPercentage = 38.89m, SubmissionPercentage = 5.56m,
                Country = "United States", Age = 33, IsActive = true, Ranking = 4
            },
            new()
            {
                Id = 5, Name = "Sergei Pavlovich", Nickname = null,
                Wins = 18, Losses = 3, Draws = 0,
                Division = "Heavyweight", Height = 193.04m, Reach = 200.66m,
                KOPercentage = 77.78m, SubmissionPercentage = 0.00m,
                Country = "Russia", Age = 32, IsActive = true, Ranking = 5
            },

            // Lightweight Division
            new()
            {
                Id = 6, Name = "Islam Makhachev", Nickname = null,
                Wins = 26, Losses = 1, Draws = 0,
                Division = "Lightweight", Height = 178.00m, Reach = 178.00m,
                KOPercentage = 19.23m, SubmissionPercentage = 34.62m,
                Country = "Russia", Age = 33, IsActive = true, Ranking = 1
            },
            new()
            {
                Id = 7, Name = "Charles Oliveira", Nickname = "Do Bronx",
                Wins = 34, Losses = 10, Draws = 0,
                Division = "Lightweight", Height = 178.00m, Reach = 188.00m,
                KOPercentage = 29.41m, SubmissionPercentage = 52.94m,
                Country = "Brazil", Age = 35, IsActive = true, Ranking = 2
            },
            new()
            {
                Id = 8, Name = "Dustin Poirier", Nickname = "The Diamond",
                Wins = 30, Losses = 8, Draws = 0,
                Division = "Lightweight", Height = 175.00m, Reach = 183.00m,
                KOPercentage = 43.33m, SubmissionPercentage = 23.33m,
                Country = "United States", Age = 36, IsActive = true, Ranking = 3
            },
            new()
            {
                Id = 9, Name = "Justin Gaethje", Nickname = "The Highlight",
                Wins = 25, Losses = 5, Draws = 0,
                Division = "Lightweight", Height = 180.00m, Reach = 178.00m,
                KOPercentage = 72.00m, SubmissionPercentage = 0.00m,
                Country = "United States", Age = 35, IsActive = true, Ranking = 4
            },
            new()
            {
                Id = 10, Name = "Arman Tsarukyan", Nickname = "Ahalkalakets",
                Wins = 22, Losses = 3, Draws = 0,
                Division = "Lightweight", Height = 178.00m, Reach = 183.00m,
                KOPercentage = 27.27m, SubmissionPercentage = 22.73m,
                Country = "Armenia", Age = 28, IsActive = true, Ranking = 5
            },

            // Welterweight Division
            new()
            {
                Id = 11, Name = "Leon Edwards", Nickname = "Rocky",
                Wins = 22, Losses = 3, Draws = 0,
                Division = "Welterweight", Height = 183.00m, Reach = 188.00m,
                KOPercentage = 31.82m, SubmissionPercentage = 18.18m,
                Country = "United Kingdom", Age = 33, IsActive = true, Ranking = 1
            },
            new()
            {
                Id = 12, Name = "Kamaru Usman", Nickname = "The Nigerian Nightmare",
                Wins = 20, Losses = 4, Draws = 0,
                Division = "Welterweight", Height = 183.00m, Reach = 193.00m,
                KOPercentage = 40.00m, SubmissionPercentage = 5.00m,
                Country = "Nigeria", Age = 37, IsActive = true, Ranking = 2
            },
            new()
            {
                Id = 13, Name = "Belal Muhammad", Nickname = "Remember the Name",
                Wins = 24, Losses = 3, Draws = 1,
                Division = "Welterweight", Height = 180.00m, Reach = 183.00m,
                KOPercentage = 12.50m, SubmissionPercentage = 16.67m,
                Country = "United States", Age = 36, IsActive = true, Ranking = 3
            },
            new()
            {
                Id = 14, Name = "Colby Covington", Nickname = "Chaos",
                Wins = 17, Losses = 4, Draws = 0,
                Division = "Welterweight", Height = 180.00m, Reach = 183.00m,
                KOPercentage = 11.76m, SubmissionPercentage = 17.65m,
                Country = "United States", Age = 37, IsActive = true, Ranking = 4
            },
            new()
            {
                Id = 15, Name = "Sean Brady", Nickname = null,
                Wins = 16, Losses = 2, Draws = 0,
                Division = "Welterweight", Height = 180.00m, Reach = 183.00m,
                KOPercentage = 18.75m, SubmissionPercentage = 43.75m,
                Country = "United States", Age = 32, IsActive = true, Ranking = 5
            },

            // Middleweight Division
            new()
            {
                Id = 16, Name = "Dricus Du Plessis", Nickname = "Stillknocks",
                Wins = 22, Losses = 2, Draws = 0,
                Division = "Middleweight", Height = 185.00m, Reach = 190.00m,
                KOPercentage = 54.55m, SubmissionPercentage = 22.73m,
                Country = "South Africa", Age = 30, IsActive = true, Ranking = 1
            },
            new()
            {
                Id = 17, Name = "Israel Adesanya", Nickname = "The Last Stylebender",
                Wins = 24, Losses = 4, Draws = 0,
                Division = "Middleweight", Height = 193.00m, Reach = 203.00m,
                KOPercentage = 58.33m, SubmissionPercentage = 0.00m,
                Country = "Nigeria", Age = 35, IsActive = true, Ranking = 2
            },
            new()
            {
                Id = 18, Name = "Robert Whittaker", Nickname = "The Reaper",
                Wins = 25, Losses = 7, Draws = 0,
                Division = "Middleweight", Height = 183.00m, Reach = 185.00m,
                KOPercentage = 36.00m, SubmissionPercentage = 12.00m,
                Country = "Australia", Age = 33, IsActive = true, Ranking = 3
            },
            new()
            {
                Id = 19, Name = "Sean Strickland", Nickname = "Tarzan",
                Wins = 28, Losses = 6, Draws = 0,
                Division = "Middleweight", Height = 185.00m, Reach = 193.00m,
                KOPercentage = 28.57m, SubmissionPercentage = 14.29m,
                Country = "United States", Age = 33, IsActive = true, Ranking = 4
            },

            // Featherweight Division
            new()
            {
                Id = 20, Name = "Alexander Volkanovski", Nickname = "The Great",
                Wins = 26, Losses = 4, Draws = 0,
                Division = "Featherweight", Height = 168.00m, Reach = 182.00m,
                KOPercentage = 23.08m, SubmissionPercentage = 11.54m,
                Country = "Australia", Age = 36, IsActive = true, Ranking = 1
            },
            new()
            {
                Id = 21, Name = "Ilia Topuria", Nickname = "El Matador",
                Wins = 16, Losses = 0, Draws = 0,
                Division = "Featherweight", Height = 170.00m, Reach = 170.00m,
                KOPercentage = 56.25m, SubmissionPercentage = 25.00m,
                Country = "Spain", Age = 28, IsActive = true, Ranking = 2
            },
            new()
            {
                Id = 22, Name = "Max Holloway", Nickname = "Blessed",
                Wins = 25, Losses = 7, Draws = 0,
                Division = "Featherweight", Height = 180.00m, Reach = 175.00m,
                KOPercentage = 40.00m, SubmissionPercentage = 4.00m,
                Country = "United States", Age = 33, IsActive = true, Ranking = 3
            },
            new()
            {
                Id = 23, Name = "Yair Rodriguez", Nickname = "El Pantera",
                Wins = 16, Losses = 4, Draws = 0,
                Division = "Featherweight", Height = 180.00m, Reach = 185.00m,
                KOPercentage = 37.50m, SubmissionPercentage = 12.50m,
                Country = "Mexico", Age = 32, IsActive = true, Ranking = 4
            },

            // Bantamweight Division
            new()
            {
                Id = 24, Name = "Sean O'Malley", Nickname = "Sugar",
                Wins = 18, Losses = 2, Draws = 0,
                Division = "Bantamweight", Height = 180.00m, Reach = 183.00m,
                KOPercentage = 61.11m, SubmissionPercentage = 0.00m,
                Country = "United States", Age = 30, IsActive = true, Ranking = 1
            },
            new()
            {
                Id = 25, Name = "Merab Dvalishvili", Nickname = "The Machine",
                Wins = 18, Losses = 4, Draws = 0,
                Division = "Bantamweight", Height = 170.00m, Reach = 175.00m,
                KOPercentage = 5.56m, SubmissionPercentage = 11.11m,
                Country = "Georgia", Age = 34, IsActive = true, Ranking = 2
            },
        };
    }
}
