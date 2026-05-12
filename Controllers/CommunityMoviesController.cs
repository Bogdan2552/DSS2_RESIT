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
[Route("api/community-movies")]
public class CommunityMoviesController(ApplicationDbContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CommunityMovieResponseDto>>> GetAll()
    {
        var movies = await dbContext.CommunityMovies
            .OrderByDescending(movie => movie.CreatedAt)
            .Select(movie => movie.ToCommunityMovieResponse())
            .ToListAsync();

        return Ok(movies);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CommunityMovieResponseDto>> GetById(int id)
    {
        var movie = await dbContext.CommunityMovies.FindAsync(id);
        if (movie is null)
        {
            return NotFound(new { message = "Community movie was not found." });
        }

        return Ok(movie.ToCommunityMovieResponse());
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<CommunityMovieResponseDto>> Create(CreateCommunityMovieDto dto)
    {
        var user = await GetCurrentUserAsync();
        if (user is null)
        {
            return Unauthorized(new { message = "Invalid token." });
        }

        var now = DateTime.UtcNow;
        var movie = new CommunityMovie
        {
            CreatedByUserId = user.Id,
            CreatedByDisplayName = user.DisplayName,
            Title = dto.Title.Trim(),
            Director = DtoMapping.CleanOptional(dto.Director),
            ReleaseYear = dto.ReleaseYear,
            Genre = DtoMapping.CleanOptional(dto.Genre),
            Description = DtoMapping.CleanOptional(dto.Description),
            CreatedAt = now,
            UpdatedAt = now
        };

        dbContext.CommunityMovies.Add(movie);
        await dbContext.SaveChangesAsync();

        var response = movie.ToCommunityMovieResponse();
        return CreatedAtAction(nameof(GetById), new { id = movie.Id }, response);
    }

    [Authorize]
    [HttpPut("{id:int}")]
    public async Task<ActionResult<CommunityMovieResponseDto>> Update(int id, UpdateCommunityMovieDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var movie = await dbContext.CommunityMovies.FindAsync(id);
        if (movie is null)
        {
            return NotFound(new { message = "Community movie was not found." });
        }

        if (movie.CreatedByUserId != userId)
        {
            return Forbid();
        }

        movie.Title = dto.Title.Trim();
        movie.Director = DtoMapping.CleanOptional(dto.Director);
        movie.ReleaseYear = dto.ReleaseYear;
        movie.Genre = DtoMapping.CleanOptional(dto.Genre);
        movie.Description = DtoMapping.CleanOptional(dto.Description);
        movie.UpdatedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync();
        return Ok(movie.ToCommunityMovieResponse());
    }

    [Authorize]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var movie = await dbContext.CommunityMovies.FindAsync(id);
        if (movie is null)
        {
            return NotFound(new { message = "Community movie was not found." });
        }

        if (movie.CreatedByUserId != userId)
        {
            return Forbid();
        }

        dbContext.CommunityMovies.Remove(movie);
        await dbContext.SaveChangesAsync();
        return NoContent();
    }

    private async Task<ApplicationUser?> GetCurrentUserAsync()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return userId is null ? null : await dbContext.Users.FindAsync(userId);
    }
}
