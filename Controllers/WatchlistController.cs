using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieWatchlist.Api.Data;
using MovieWatchlist.Api.Dtos;
using MovieWatchlist.Api.Models;
using MovieWatchlist.Api.Services;

namespace MovieWatchlist.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/watchlist")]
public class WatchlistController(ApplicationDbContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<WatchlistItemResponseDto>>> GetAll()
    {
        var userId = GetCurrentUserId();
        var items = await dbContext.WatchlistItems
            .Where(item => item.UserId == userId)
            .OrderByDescending(item => item.CreatedAt)
            .Select(item => item.ToWatchlistResponse())
            .ToListAsync();

        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<WatchlistItemResponseDto>> GetById(int id)
    {
        var userId = GetCurrentUserId();
        var item = await dbContext.WatchlistItems.SingleOrDefaultAsync(candidate => candidate.Id == id && candidate.UserId == userId);
        if (item is null)
        {
            return NotFound(new { message = "Watchlist item was not found." });
        }

        return Ok(item.ToWatchlistResponse());
    }

    [HttpPost]
    public async Task<ActionResult<WatchlistItemResponseDto>> Create(CreateWatchlistItemDto dto)
    {
        var now = DateTime.UtcNow;
        var item = new WatchlistItem
        {
            UserId = GetCurrentUserId(),
            Title = dto.Title.Trim(),
            Director = DtoMapping.CleanOptional(dto.Director),
            ReleaseYear = dto.ReleaseYear,
            Genre = DtoMapping.CleanOptional(dto.Genre),
            Status = dto.Status,
            Rating = dto.Rating,
            Notes = DtoMapping.CleanOptional(dto.Notes),
            CreatedAt = now,
            UpdatedAt = now
        };

        dbContext.WatchlistItems.Add(item);
        await dbContext.SaveChangesAsync();

        var response = item.ToWatchlistResponse();
        return CreatedAtAction(nameof(GetById), new { id = item.Id }, response);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<WatchlistItemResponseDto>> Update(int id, UpdateWatchlistItemDto dto)
    {
        var userId = GetCurrentUserId();
        var item = await dbContext.WatchlistItems.SingleOrDefaultAsync(candidate => candidate.Id == id && candidate.UserId == userId);
        if (item is null)
        {
            return NotFound(new { message = "Watchlist item was not found." });
        }

        item.Title = dto.Title.Trim();
        item.Director = DtoMapping.CleanOptional(dto.Director);
        item.ReleaseYear = dto.ReleaseYear;
        item.Genre = DtoMapping.CleanOptional(dto.Genre);
        item.Status = dto.Status;
        item.Rating = dto.Rating;
        item.Notes = DtoMapping.CleanOptional(dto.Notes);
        item.UpdatedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync();
        return Ok(item.ToWatchlistResponse());
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = GetCurrentUserId();
        var item = await dbContext.WatchlistItems.SingleOrDefaultAsync(candidate => candidate.Id == id && candidate.UserId == userId);
        if (item is null)
        {
            return NotFound(new { message = "Watchlist item was not found." });
        }

        dbContext.WatchlistItems.Remove(item);
        await dbContext.SaveChangesAsync();
        return NoContent();
    }

    private string GetCurrentUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)!;
}
