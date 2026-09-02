using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FastScan.Models;
using FastScan.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace FastScan.Api.Controllers;

public record LoginRequest(string Email, string Password);
public record BootstrapAdminRequest(string FullName, string Email, string Password);

[ApiController, Route("api/auth")]
public class AuthController(FastScanDbContext db, IConfiguration config) : ControllerBase
{
    [AllowAnonymous, HttpPost("bootstrap")]
    public async Task<IActionResult> Bootstrap(BootstrapAdminRequest request, [FromHeader(Name = "X-Bootstrap-Key")] string? bootstrapKey)
    {
        if (bootstrapKey != config["Security:BootstrapKey"]) return Unauthorized();
        if (await db.Users.AnyAsync()) return Conflict(new { mensaje = "Ya existe un administrador." });
        if (request.Password.Length < 8) return BadRequest(new { mensaje = "La contraseña debe tener al menos 8 caracteres." });
        var user = new User { FullName = request.FullName.Trim(), Email = request.Email.Trim().ToLowerInvariant(), PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password), Role = UserRole.Administrator };
        db.Users.Add(user); await db.SaveChangesAsync(); return Ok(Session(user));
    }
    [AllowAnonymous, HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    { var user = await db.Users.SingleOrDefaultAsync(x => x.Email == request.Email.Trim().ToLowerInvariant()); if (user is null || !user.IsActive || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash)) return Unauthorized(new { mensaje = "Credenciales inválidas." }); return Ok(Session(user)); }
    private object Session(User user)
    { var jwtKey = config["Jwt:Key"]!; var token = new JwtSecurityToken(config["Jwt:Issuer"], config["Jwt:Audience"], new[] { new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), new Claim(ClaimTypes.Role, user.Role.ToString()) }, expires: DateTime.UtcNow.AddHours(8), signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)), SecurityAlgorithms.HmacSha256)); return new { token = new JwtSecurityTokenHandler().WriteToken(token), user = new { user.Id, user.FullName, user.Email, user.Role, user.BranchId } }; }
}
