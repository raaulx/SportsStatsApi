using SportsStatsApi.DTOs;

namespace SportsStatsApi.Services;

/// <summary>
/// Interface for fighter-related business logic operations.
/// </summary>
public interface IFighterService
{
    /// <summary>Gets all fighters with optional filtering and pagination.</summary>
    Task<PaginatedResponse<FighterDto>> GetAllFightersAsync(
        int page = 1,
        int pageSize = 10,
        string? division = null,
        string? country = null,
        bool? isActive = null,
        string? sortBy = null,
        bool descending = true);

    /// <summary>Gets a fighter by their exact or partial name.</summary>
    Task<FighterDto?> GetFighterByNameAsync(string name);

    /// <summary>Gets a fighter by their ID.</summary>
    Task<FighterDto?> GetFighterByIdAsync(int id);

    /// <summary>Compares two fighters by name.</summary>
    Task<FighterComparisonDto?> CompareFightersAsync(string fighter1Name, string fighter2Name);

    /// <summary>Gets fighters ranked in a specific division.</summary>
    Task<DivisionRankingDto?> GetRankingsByDivisionAsync(string division);

    /// <summary>Gets all available divisions.</summary>
    Task<List<string>> GetAllDivisionsAsync();

    /// <summary>Searches fighters by partial name match.</summary>
    Task<List<FighterDto>> SearchFightersAsync(string query);
}
