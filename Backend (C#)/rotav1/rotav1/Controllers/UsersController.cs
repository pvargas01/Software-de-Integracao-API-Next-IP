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
    public async Task<IActionResult> Register(Usuario user)
    {
        user.Senhahash = HashPassword(user.Senhahash);
        _context.Usuarios.Add(user);
        await _context.SaveChangesAsync();
        var token = _tokenService.GenerateToken(user.Nome, user.Role);
        return Ok(new { Token = token });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] Usuario credentials)
    {
        var user = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Nome == credentials.Nome);
        if (user == null || !VerifyPassword(credentials.Senhahash, user.Senhahash))
            return Unauthorized();

        var token = _tokenService.GenerateToken(user.Nome, user.Role);
        return Ok(new { Token = token });
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        var username = User.Identity?.Name;
        var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nome == username);
        if (user == null) return NotFound();
        return Ok(user);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("all")]
    public async Task<IActionResult> GetAllUsers()
    {
        var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value;
        Console.WriteLine($"Role Claim: {roleClaim}");

        var users = await _context.Usuarios.ToListAsync();
        return Ok(users);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var username = User.Identity?.Name;
        var user = await _context.Usuarios.FindAsync(id);
        Console.WriteLine(user);

        if (user == null) return NotFound();

        if (user.Nome != username && !User.IsInRole("Admin"))
            return Forbid();

        _context.Usuarios.Remove(user);
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