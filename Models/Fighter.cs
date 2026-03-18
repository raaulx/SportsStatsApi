using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsStatsApi.Models;

/// <summary>
/// Represents a UFC fighter with their career statistics.
/// </summary>
public class Fighter
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(80)]
    public string? Nickname { get; set; }

    [Required]
    public int Wins { get; set; }

    [Required]
    public int Losses { get; set; }

    public int Draws { get; set; }

    [Required]
    [MaxLength(50)]
    public string Division { get; set; } = string.Empty;

    /// <summary>Height in centimeters</summary>
    [Column(TypeName = "decimal(5,2)")]
    public decimal Height { get; set; }

    /// <summary>Reach in centimeters</summary>
    [Column(TypeName = "decimal(5,2)")]
    public decimal Reach { get; set; }

    /// <summary>KO percentage (0-100)</summary>
    [Column(TypeName = "decimal(5,2)")]
    [Range(0, 100)]
    public decimal KOPercentage { get; set; }

    /// <summary>Submission percentage (0-100)</summary>
    [Column(TypeName = "decimal(5,2)")]
    [Range(0, 100)]
    public decimal SubmissionPercentage { get; set; }

    [MaxLength(50)]
    public string? Country { get; set; }

    /// <summary>Fighter's age</summary>
    public int? Age { get; set; }

    /// <summary>URL to fighter's image</summary>
    [MaxLength(500)]
    public string? ImageUrl { get; set; }

    /// <summary>Whether the fighter is currently active</summary>
    public bool IsActive { get; set; } = true;

    /// <summary>Current ranking position in division (null if unranked)</summary>
    public int? Ranking { get; set; }

    // Computed properties
    [NotMapped]
    public int TotalFights => Wins + Losses + Draws;

    [NotMapped]
    public decimal WinPercentage => TotalFights > 0
        ? Math.Round((decimal)Wins / TotalFights * 100, 2)
        : 0;

    [NotMapped]
    public string Record => $"{Wins}-{Losses}-{Draws}";
}
