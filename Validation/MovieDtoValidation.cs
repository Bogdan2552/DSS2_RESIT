using System.ComponentModel.DataAnnotations;
using MovieWatchlist.Api.Models;

namespace MovieWatchlist.Api.Validation;

public static class MovieDtoValidation
{
    public static IEnumerable<ValidationResult> ValidateReleaseYear(int? releaseYear)
    {
        if (releaseYear is null)
        {
            yield break;
        }

        var maxYear = DateTime.UtcNow.Year + 2;
        if (releaseYear < 1888 || releaseYear > maxYear)
        {
            yield return new ValidationResult(
                $"Release year must be between 1888 and {maxYear}.",
                new[] { "ReleaseYear" });
        }
    }

    public static IEnumerable<ValidationResult> ValidateStatus(string? status)
    {
        if (!WatchlistStatuses.IsValid(status))
        {
            yield return new ValidationResult(
                $"Status must be one of: {string.Join(", ", WatchlistStatuses.All)}.",
                new[] { "Status" });
        }
    }
}
