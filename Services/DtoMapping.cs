using MovieWatchlist.Api.Dtos;
using MovieWatchlist.Api.Models;

namespace MovieWatchlist.Api.Services;

public static class DtoMapping
{
    public static UserResponseDto ToUserResponse(this ApplicationUser user) => new()
    {
        Id = user.Id,
        Email = user.Email,
        DisplayName = user.DisplayName
    };

    public static WatchlistItemResponseDto ToWatchlistResponse(this WatchlistItem item) => new()
    {
        Id = item.Id,
        Title = item.Title,
        Director = item.Director,
        ReleaseYear = item.ReleaseYear,
        Genre = item.Genre,
        Status = item.Status,
        Rating = item.Rating,
        Notes = item.Notes,
        CreatedAt = item.CreatedAt,
        UpdatedAt = item.UpdatedAt
    };

    public static CommunityMovieResponseDto ToCommunityMovieResponse(this CommunityMovie movie) => new()
    {
        Id = movie.Id,
        Title = movie.Title,
        Director = movie.Director,
        ReleaseYear = movie.ReleaseYear,
        Genre = movie.Genre,
        Description = movie.Description,
        CreatedByUserId = movie.CreatedByUserId,
        CreatedByDisplayName = movie.CreatedByDisplayName,
        CreatedAt = movie.CreatedAt,
        UpdatedAt = movie.UpdatedAt
    };

    public static string? CleanOptional(string? value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
