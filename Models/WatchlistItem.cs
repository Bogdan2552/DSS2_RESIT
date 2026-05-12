namespace MovieWatchlist.Api.Models;

public class WatchlistItem
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser? User { get; set; }

    public string Title { get; set; } = string.Empty;
    public string? Director { get; set; }
    public int? ReleaseYear { get; set; }
    public string? Genre { get; set; }
    public string Status { get; set; } = WatchlistStatuses.PlanToWatch;
    public int? Rating { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public static class WatchlistStatuses
{
    public const string PlanToWatch = "PlanToWatch";
    public const string Watching = "Watching";
    public const string Watched = "Watched";
    public const string Dropped = "Dropped";

    public static readonly string[] All = { PlanToWatch, Watching, Watched, Dropped };

    public static bool IsValid(string? value) => All.Contains(value);
}
