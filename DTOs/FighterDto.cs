namespace SportsStatsApi.DTOs;

/// <summary>
/// DTO for returning fighter data in API responses.
/// </summary>
public class FighterDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Nickname { get; set; }
    public string Record { get; set; } = string.Empty;
    public int Wins { get; set; }
    public int Losses { get; set; }
    public int Draws { get; set; }
    public string Division { get; set; } = string.Empty;
    public decimal Height { get; set; }
    public decimal Reach { get; set; }
    public decimal KOPercentage { get; set; }
    public decimal SubmissionPercentage { get; set; }
    public decimal WinPercentage { get; set; }
    public int TotalFights { get; set; }
    public string? Country { get; set; }
    public int? Age { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; }
    public int? Ranking { get; set; }
}

/// <summary>
/// DTO for comparing two fighters side by side.
/// </summary>
public class FighterComparisonDto
{
    public FighterDto Fighter1 { get; set; } = null!;
    public FighterDto Fighter2 { get; set; } = null!;
    public ComparisonResultDto Comparison { get; set; } = null!;
}

/// <summary>
/// Detailed comparison results highlighting advantages.
/// </summary>
public class ComparisonResultDto
{
    public string MoreWins { get; set; } = string.Empty;
    public string FewerLosses { get; set; } = string.Empty;
    public string BetterWinPercentage { get; set; } = string.Empty;
    public string Taller { get; set; } = string.Empty;
    public string LongerReach { get; set; } = string.Empty;
    public string HigherKORate { get; set; } = string.Empty;
    public string HigherSubRate { get; set; } = string.Empty;
    public string MoreExperience { get; set; } = string.Empty;
}

/// <summary>
/// DTO for division rankings response.
/// </summary>
public class DivisionRankingDto
{
    public string Division { get; set; } = string.Empty;
    public int TotalFighters { get; set; }
    public List<FighterDto> Fighters { get; set; } = new();
}

/// <summary>
/// Paginated response wrapper.
/// </summary>
public class PaginatedResponse<T>
{
    public List<T> Data { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasPreviousPage => Page > 1;
    public bool HasNextPage => Page < TotalPages;
}

/// <summary>
/// Standard API error response.
/// </summary>
public class ApiErrorResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Detail { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
