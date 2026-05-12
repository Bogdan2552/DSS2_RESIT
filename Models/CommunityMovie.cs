namespace MovieWatchlist.Api.Models;

public class CommunityMovie
{
    public int Id { get; set; }
    public string CreatedByUserId { get; set; } = string.Empty;
    public ApplicationUser? CreatedByUser { get; set; }
    public string CreatedByDisplayName { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;
    public string? Director { get; set; }
    public int? ReleaseYear { get; set; }
    public string? Genre { get; set; }
    public string? Description { get; set; }
    public decimal? AverageCommunityRating { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
