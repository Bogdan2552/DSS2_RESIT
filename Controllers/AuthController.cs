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
[Route("api/auth")]
public class AuthController(
    ApplicationDbContext dbContext,
    IPasswordHasher passwordHasher,
    IJwtTokenService jwtTokenService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register(RegisterRequestDto dto)
    {
        var normalizedEmail = dto.Email.Trim().ToUpperInvariant();
        var emailExists = await dbContext.Users.AnyAsync(user => user.NormalizedEmail == normalizedEmail);
        if (emailExists)
        {
            return Conflict(new { message = "Email is already registered." });
        }

        var user = new ApplicationUser
        {
            Email = dto.Email.Trim(),
            NormalizedEmail = normalizedEmail,
            DisplayName = dto.DisplayName.Trim(),
            PasswordHash = passwordHasher.Hash(dto.Password),
            CreatedAt = DateTime.UtcNow
        };

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();

        return Ok(CreateAuthResponse(user));
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginRequestDto dto)
    {
        var normalizedEmail = dto.Email.Trim().ToUpperInvariant();
        var user = await dbContext.Users.SingleOrDefaultAsync(candidate => candidate.NormalizedEmail == normalizedEmail);

        if (user is null || !passwordHasher.Verify(dto.Password, user.PasswordHash))
        {
            return Unauthorized(new { message = "Invalid email or password." });
        }

        return Ok(CreateAuthResponse(user));
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<UserResponseDto>> Me()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
        {
            return Unauthorized(new { message = "Invalid token." });
        }

        var user = await dbContext.Users.FindAsync(userId);
        if (user is null)
        {
            return Unauthorized(new { message = "User no longer exists." });
        }

        return Ok(user.ToUserResponse());
    }

    private AuthResponseDto CreateAuthResponse(ApplicationUser user) => new()
    {
        Token = jwtTokenService.CreateToken(user),
        User = user.ToUserResponse()
    };
}
