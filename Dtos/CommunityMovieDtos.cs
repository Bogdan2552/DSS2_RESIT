using System.ComponentModel.DataAnnotations;
using MovieWatchlist.Api.Validation;

namespace MovieWatchlist.Api.Dtos;

public class CreateCommunityMovieDto : IValidatableObject
{
    [Required]
    [StringLength(120, MinimumLength = 1)]
    public string Title { get; set; } = string.Empty;

    [StringLength(80)]
    public string? Director { get; set; }

    public int? ReleaseYear { get; set; }

    [StringLength(40)]
    public string? Genre { get; set; }

    [StringLength(1500)]
    public string? Description { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var validationResult in MovieDtoValidation.ValidateReleaseYear(ReleaseYear))
        {
            yield return validationResult;
        }
    }
}

public class UpdateCommunityMovieDto : CreateCommunityMovieDto
{
}

public class CommunityMovieResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Director { get; set; }
    public int? ReleaseYear { get; set; }
    public string? Genre { get; set; }
    public string? Description { get; set; }
    public string CreatedByUserId { get; set; } = string.Empty;
    public string CreatedByDisplayName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
