using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using rotav1.Data;
using rotav1.Models;
using rotav1.Services;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace rotav1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly TokenService _tokenService;

    public UsersController(AppDbContext context, TokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(User user)
    {
        user.PasswordHash = HashPassword(user.PasswordHash);
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return Ok(new { Message = "User registered successfully" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] User credentials)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == credentials.Username);
        Console.WriteLine(user);
        if (user == null || !VerifyPassword(credentials.PasswordHash, user.PasswordHash))
            return Unauthorized();

        var token = _tokenService.GenerateToken(user.Username, user.Role);
        return Ok(new { Token = token });
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        var username = User.Identity?.Name;
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null) return NotFound();
        return Ok(user);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("all")]
    public async Task<IActionResult> GetAllUsers()
    {
        var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value;
        Console.WriteLine($"Role Claim: {roleClaim}");

        var users = await _context.Users.ToListAsync();
        return Ok(users);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var username = User.Identity?.Name;
        var user = await _context.Users.FindAsync(id);
        Console.WriteLine(user);

        if (user == null) return NotFound();

        if (user.Username != username && !User.IsInRole("Admin"))
            return Forbid();

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        return Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
    }

    private static bool VerifyPassword(string inputPassword, string storedHash)
    {
        return HashPassword(inputPassword) == storedHash;
    }
}