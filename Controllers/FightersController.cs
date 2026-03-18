using Microsoft.AspNetCore.Mvc;
using SportsStatsApi.DTOs;
using SportsStatsApi.Services;

namespace SportsStatsApi.Controllers;

/// <summary>
/// API controller for managing UFC fighter statistics.
/// Provides endpoints for querying, searching, comparing fighters and viewing division rankings.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class FightersController : ControllerBase
{
    private readonly IFighterService _fighterService;
    private readonly ILogger<FightersController> _logger;

    public FightersController(IFighterService fighterService, ILogger<FightersController> logger)
    {
        _fighterService = fighterService;
        _logger = logger;
    }

    /// <summary>
    /// Gets all fighters with optional filtering, sorting, and pagination.
    /// </summary>
    /// <param name="page">Page number (default: 1)</param>
    /// <param name="pageSize">Items per page (default: 10, max: 50)</param>
    /// <param name="division">Filter by division name</param>
    /// <param name="country">Filter by country</param>
    /// <param name="isActive">Filter by active status</param>
    /// <param name="sortBy">Sort by field: name, wins, losses, kopercentage, height, reach, ranking</param>
    /// <param name="descending">Sort descending (default: true)</param>
    /// <returns>Paginated list of fighters</returns>
    /// <response code="200">Returns the paginated list of fighters</response>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponse<FighterDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedResponse<FighterDto>>> GetAllFighters(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? division = null,
        [FromQuery] string? country = null,
        [FromQuery] bool? isActive = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] bool descending = true)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 1;
        if (pageSize > 50) pageSize = 50;

        var result = await _fighterService.GetAllFightersAsync(
            page, pageSize, division, country, isActive, sortBy, descending);

        return Ok(result);
    }

    /// <summary>
    /// Gets a fighter by their name (supports partial matching).
    /// </summary>
    /// <param name="name">Fighter name or partial name</param>
    /// <returns>Fighter details</returns>
    /// <response code="200">Returns the fighter</response>
    /// <response code="404">Fighter not found</response>
    [HttpGet("{name}")]
    [ProducesResponseType(typeof(FighterDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FighterDto>> GetFighterByName(string name)
    {
        var fighter = await _fighterService.GetFighterByNameAsync(name);

        if (fighter == null)
        {
            return NotFound(new ApiErrorResponse
            {
                StatusCode = 404,
                Message = $"Fighter '{name}' not found.",
                Detail = "Try using the search endpoint: GET /api/fighters/search?q=partial_name"
            });
        }

        return Ok(fighter);
    }

    /// <summary>
    /// Gets a fighter by their ID.
    /// </summary>
    /// <param name="id">Fighter ID</param>
    /// <returns>Fighter details</returns>
    /// <response code="200">Returns the fighter</response>
    /// <response code="404">Fighter not found</response>
    [HttpGet("id/{id:int}")]
    [ProducesResponseType(typeof(FighterDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FighterDto>> GetFighterById(int id)
    {
        var fighter = await _fighterService.GetFighterByIdAsync(id);

        if (fighter == null)
        {
            return NotFound(new ApiErrorResponse
            {
                StatusCode = 404,
                Message = $"Fighter with ID {id} not found."
            });
        }

        return Ok(fighter);
    }

    /// <summary>
    /// Compares two fighters side by side with advantage analysis.
    /// </summary>
    /// <param name="fighter1">Name of the first fighter</param>
    /// <param name="fighter2">Name of the second fighter</param>
    /// <returns>Detailed comparison of both fighters</returns>
    /// <response code="200">Returns the comparison result</response>
    /// <response code="400">Missing fighter names</response>
    /// <response code="404">One or both fighters not found</response>
    [HttpGet("compare")]
    [ProducesResponseType(typeof(FighterComparisonDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FighterComparisonDto>> CompareFighters(
        [FromQuery] string fighter1,
        [FromQuery] string fighter2)
    {
        if (string.IsNullOrWhiteSpace(fighter1) || string.IsNullOrWhiteSpace(fighter2))
        {
            return BadRequest(new ApiErrorResponse
            {
                StatusCode = 400,
                Message = "Both fighter1 and fighter2 query parameters are required.",
                Detail = "Example: /api/fighters/compare?fighter1=Jon Jones&fighter2=Tom Aspinall"
            });
        }

        var comparison = await _fighterService.CompareFightersAsync(fighter1, fighter2);

        if (comparison == null)
        {
            return NotFound(new ApiErrorResponse
            {
                StatusCode = 404,
                Message = $"One or both fighters not found: '{fighter1}', '{fighter2}'.",
                Detail = "Use the search endpoint to find available fighters: GET /api/fighters/search?q=name"
            });
        }

        return Ok(comparison);
    }

    /// <summary>
    /// Gets fighter rankings for a specific division.
    /// </summary>
    /// <param name="division">Division name (e.g., Heavyweight, Lightweight)</param>
    /// <returns>Ranked list of fighters in the division</returns>
    /// <response code="200">Returns division rankings</response>
    /// <response code="404">Division not found</response>
    [HttpGet("rankings/{division}")]
    [ProducesResponseType(typeof(DivisionRankingDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DivisionRankingDto>> GetRankings(string division)
    {
        var rankings = await _fighterService.GetRankingsByDivisionAsync(division);

        if (rankings == null)
        {
            var divisions = await _fighterService.GetAllDivisionsAsync();
            return NotFound(new ApiErrorResponse
            {
                StatusCode = 404,
                Message = $"Division '{division}' not found or has no active fighters.",
                Detail = $"Available divisions: {string.Join(", ", divisions)}"
            });
        }

        return Ok(rankings);
    }

    /// <summary>
    /// Gets a list of all available divisions.
    /// </summary>
    /// <returns>List of division names</returns>
    /// <response code="200">Returns list of divisions</response>
    [HttpGet("divisions")]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<string>>> GetDivisions()
    {
        var divisions = await _fighterService.GetAllDivisionsAsync();
        return Ok(divisions);
    }

    /// <summary>
    /// Searches fighters by name, nickname, or country.
    /// </summary>
    /// <param name="q">Search query</param>
    /// <returns>List of matching fighters</returns>
    /// <response code="200">Returns matching fighters</response>
    /// <response code="400">Missing search query</response>
    [HttpGet("search")]
    [ProducesResponseType(typeof(List<FighterDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<List<FighterDto>>> SearchFighters([FromQuery] string q)
    {
        if (string.IsNullOrWhiteSpace(q))
        {
            return BadRequest(new ApiErrorResponse
            {
                StatusCode = 400,
                Message = "Search query parameter 'q' is required.",
                Detail = "Example: /api/fighters/search?q=jones"
            });
        }

        var fighters = await _fighterService.SearchFightersAsync(q);
        return Ok(fighters);
    }
}
