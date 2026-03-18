using Microsoft.EntityFrameworkCore;
using SportsStatsApi.Data;
using SportsStatsApi.DTOs;
using SportsStatsApi.Models;

namespace SportsStatsApi.Services;

/// <summary>
/// Implementation of fighter business logic using Entity Framework Core.
/// </summary>
public class FighterService : IFighterService
{
    private readonly AppDbContext _context;
    private readonly ILogger<FighterService> _logger;

    public FighterService(AppDbContext context, ILogger<FighterService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<PaginatedResponse<FighterDto>> GetAllFightersAsync(
        int page = 1,
        int pageSize = 10,
        string? division = null,
        string? country = null,
        bool? isActive = null,
        string? sortBy = null,
        bool descending = true)
    {
        _logger.LogInformation("Fetching fighters - Page: {Page}, Size: {PageSize}, Division: {Division}",
            page, pageSize, division);

        var query = _context.Fighters.AsQueryable();

        // Apply filters
        if (!string.IsNullOrWhiteSpace(division))
            query = query.Where(f => f.Division.ToLower() == division.ToLower());

        if (!string.IsNullOrWhiteSpace(country))
            query = query.Where(f => f.Country != null && f.Country.ToLower() == country.ToLower());

        if (isActive.HasValue)
            query = query.Where(f => f.IsActive == isActive.Value);

        // Apply sorting
        query = sortBy?.ToLower() switch
        {
            "name" => descending ? query.OrderByDescending(f => f.Name) : query.OrderBy(f => f.Name),
            "wins" => descending ? query.OrderByDescending(f => f.Wins) : query.OrderBy(f => f.Wins),
            "losses" => descending ? query.OrderByDescending(f => f.Losses) : query.OrderBy(f => f.Losses),
            "kopercentage" => descending ? query.OrderByDescending(f => f.KOPercentage) : query.OrderBy(f => f.KOPercentage),
            "height" => descending ? query.OrderByDescending(f => f.Height) : query.OrderBy(f => f.Height),
            "reach" => descending ? query.OrderByDescending(f => f.Reach) : query.OrderBy(f => f.Reach),
            "ranking" => query.OrderBy(f => f.Ranking ?? int.MaxValue),
            _ => query.OrderBy(f => f.Division).ThenBy(f => f.Ranking ?? int.MaxValue)
        };

        var totalCount = await query.CountAsync();

        var fighters = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedResponse<FighterDto>
        {
            Data = fighters.Select(MapToDto).ToList(),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    /// <inheritdoc />
    public async Task<FighterDto?> GetFighterByNameAsync(string name)
    {
        _logger.LogInformation("Searching fighter by name: {Name}", name);

        var fighter = await _context.Fighters
            .FirstOrDefaultAsync(f => f.Name.ToLower() == name.ToLower());

        // If exact match not found, try partial match
        if (fighter == null)
        {
            fighter = await _context.Fighters
                .FirstOrDefaultAsync(f => f.Name.ToLower().Contains(name.ToLower()));
        }

        return fighter != null ? MapToDto(fighter) : null;
    }

    /// <inheritdoc />
    public async Task<FighterDto?> GetFighterByIdAsync(int id)
    {
        var fighter = await _context.Fighters.FindAsync(id);
        return fighter != null ? MapToDto(fighter) : null;
    }

    /// <inheritdoc />
    public async Task<FighterComparisonDto?> CompareFightersAsync(string fighter1Name, string fighter2Name)
    {
        _logger.LogInformation("Comparing fighters: {Fighter1} vs {Fighter2}", fighter1Name, fighter2Name);

        var f1 = await _context.Fighters
            .FirstOrDefaultAsync(f => f.Name.ToLower() == fighter1Name.ToLower());
        var f2 = await _context.Fighters
            .FirstOrDefaultAsync(f => f.Name.ToLower() == fighter2Name.ToLower());

        // Try partial match if exact not found
        if (f1 == null)
            f1 = await _context.Fighters
                .FirstOrDefaultAsync(f => f.Name.ToLower().Contains(fighter1Name.ToLower()));
        if (f2 == null)
            f2 = await _context.Fighters
                .FirstOrDefaultAsync(f => f.Name.ToLower().Contains(fighter2Name.ToLower()));

        if (f1 == null || f2 == null) return null;

        var dto1 = MapToDto(f1);
        var dto2 = MapToDto(f2);

        return new FighterComparisonDto
        {
            Fighter1 = dto1,
            Fighter2 = dto2,
            Comparison = new ComparisonResultDto
            {
                MoreWins = CompareValues(dto1.Wins, dto2.Wins, dto1.Name, dto2.Name),
                FewerLosses = CompareValues(dto2.Losses, dto1.Losses, dto1.Name, dto2.Name),
                BetterWinPercentage = CompareValues(dto1.WinPercentage, dto2.WinPercentage, dto1.Name, dto2.Name),
                Taller = CompareValues(dto1.Height, dto2.Height, dto1.Name, dto2.Name),
                LongerReach = CompareValues(dto1.Reach, dto2.Reach, dto1.Name, dto2.Name),
                HigherKORate = CompareValues(dto1.KOPercentage, dto2.KOPercentage, dto1.Name, dto2.Name),
                HigherSubRate = CompareValues(dto1.SubmissionPercentage, dto2.SubmissionPercentage, dto1.Name, dto2.Name),
                MoreExperience = CompareValues(dto1.TotalFights, dto2.TotalFights, dto1.Name, dto2.Name)
            }
        };
    }

    /// <inheritdoc />
    public async Task<DivisionRankingDto?> GetRankingsByDivisionAsync(string division)
    {
        _logger.LogInformation("Fetching rankings for division: {Division}", division);

        var fighters = await _context.Fighters
            .Where(f => f.Division.ToLower() == division.ToLower() && f.IsActive)
            .OrderBy(f => f.Ranking ?? int.MaxValue)
            .ThenByDescending(f => f.Wins)
            .ToListAsync();

        if (!fighters.Any()) return null;

        return new DivisionRankingDto
        {
            Division = fighters.First().Division,
            TotalFighters = fighters.Count,
            Fighters = fighters.Select(MapToDto).ToList()
        };
    }

    /// <inheritdoc />
    public async Task<List<string>> GetAllDivisionsAsync()
    {
        return await _context.Fighters
            .Select(f => f.Division)
            .Distinct()
            .OrderBy(d => d)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<List<FighterDto>> SearchFightersAsync(string query)
    {
        _logger.LogInformation("Searching fighters with query: {Query}", query);

        var fighters = await _context.Fighters
            .Where(f => f.Name.ToLower().Contains(query.ToLower()) ||
                        (f.Nickname != null && f.Nickname.ToLower().Contains(query.ToLower())) ||
                        (f.Country != null && f.Country.ToLower().Contains(query.ToLower())))
            .OrderBy(f => f.Name)
            .Take(20)
            .ToListAsync();

        return fighters.Select(MapToDto).ToList();
    }

    // --- Private Helper Methods ---

    /// <summary>Maps a Fighter entity to a FighterDto.</summary>
    private static FighterDto MapToDto(Fighter fighter)
    {
        return new FighterDto
        {
            Id = fighter.Id,
            Name = fighter.Name,
            Nickname = fighter.Nickname,
            Record = fighter.Record,
            Wins = fighter.Wins,
            Losses = fighter.Losses,
            Draws = fighter.Draws,
            Division = fighter.Division,
            Height = fighter.Height,
            Reach = fighter.Reach,
            KOPercentage = fighter.KOPercentage,
            SubmissionPercentage = fighter.SubmissionPercentage,
            WinPercentage = fighter.WinPercentage,
            TotalFights = fighter.TotalFights,
            Country = fighter.Country,
            Age = fighter.Age,
            ImageUrl = fighter.ImageUrl,
            IsActive = fighter.IsActive,
            Ranking = fighter.Ranking
        };
    }

    /// <summary>Compares two decimal values and returns which fighter has the advantage.</summary>
    private static string CompareValues(decimal value1, decimal value2, string name1, string name2)
    {
        if (value1 > value2) return $"{name1} ({value1} vs {value2})";
        if (value2 > value1) return $"{name2} ({value2} vs {value1})";
        return $"Tied ({value1})";
    }

    /// <summary>Compares two int values and returns which fighter has the advantage.</summary>
    private static string CompareValues(int value1, int value2, string name1, string name2)
    {
        if (value1 > value2) return $"{name1} ({value1} vs {value2})";
        if (value2 > value1) return $"{name2} ({value2} vs {value1})";
        return $"Tied ({value1})";
    }
}
