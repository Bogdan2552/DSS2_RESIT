using System.ComponentModel.DataAnnotations;
using MovieWatchlist.Api.Validation;

namespace MovieWatchlist.Api.Dtos;

public class CreateWatchlistItemDto : IValidatableObject
{
    [Required]
    [StringLength(120, MinimumLength = 1)]
    public string Title { get; set; } = string.Empty;

    [StringLength(80)]
    public string? Director { get; set; }

    public int? ReleaseYear { get; set; }

    [StringLength(40)]
    public string? Genre { get; set; }

    [Required]
    public string Status { get; set; } = string.Empty;

    [Range(1, 10)]
    public int? Rating { get; set; }

    [StringLength(1000)]
    public string? Notes { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var validationResult in MovieDtoValidation.ValidateReleaseYear(ReleaseYear))
        {
            yield return validationResult;
        }

        foreach (var validationResult in MovieDtoValidation.ValidateStatus(Status))
        {
            yield return validationResult;
        }
    }
}

public class UpdateWatchlistItemDto : CreateWatchlistItemDto
{
}

public class WatchlistItemResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Director { get; set; }
    public int? ReleaseYear { get; set; }
    public string? Genre { get; set; }
    public string Status { get; set; } = string.Empty;
    public int? Rating { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
