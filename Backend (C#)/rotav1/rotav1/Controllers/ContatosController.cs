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

public class ContatosController: ControllerBase
{
    private readonly AppDbContext _context;
    private readonly TokenService _tokenService;

    public ContatosController(AppDbContext context, TokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetContatos()
    {
        var username = User.Identity?.Name;
        var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nome == username);
        if (user == null) return NotFound();
        var contatos = await _context.UsuarioContato
            .Where(uc => uc.UsuarioId == user.UsuarioId)
            .Select(uc => uc.Contato)
            .ToListAsync();
        return Ok(contatos);
    }

    [Authorize]
    [HttpPost("{id}")]
    public async Task<IActionResult> AddContato(int id)
    {
        var username = User.Identity?.Name;
        var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nome == username);
        if (user == null) return NotFound();
        var contato = await _context.Usuarios.FindAsync(id);
        if (contato == null) return NotFound();
        var usuarioContato = new UsuarioContato
        {
            UsuarioId = user.UsuarioId,
            ContatoId = contato.UsuarioId
        };
        _context.UsuarioContatos.Add(usuarioContato);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveContato(int id)
    {
        var username = User.Identity?.Name;
        var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nome == username);
        if (user == null) return NotFound();
        var usuarioContato = await _context.UsuarioContatos
            .FirstOrDefaultAsync(uc => uc.UsuarioId == user.UsuarioId && uc.ContatoId == id);
        if (usuarioContato == null) return NotFound();
        _context.UsuarioContatos.Remove(usuarioContato);
        await _context.SaveChangesAsync();
        return Ok();
    }
}