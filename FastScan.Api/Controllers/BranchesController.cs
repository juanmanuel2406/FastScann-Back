using FastScan.Models; using FastScan.Services; using Microsoft.AspNetCore.Mvc; using Microsoft.EntityFrameworkCore;
namespace FastScan.Api.Controllers;
public record CreateBranchRequest(string Code, string Name, string? Address);
[ApiController, Route("api/branches")]
public class BranchesController(FastScanDbContext db) : ControllerBase
{ [HttpGet] public async Task<IActionResult> GetAll() => Ok(await db.Branches.OrderBy(x => x.Name).ToListAsync()); [HttpPost] public async Task<IActionResult> Create(CreateBranchRequest request) { if (await db.Branches.AnyAsync(x => x.Code == request.Code)) return Conflict(new { message = "Ya existe una sucursal con ese código." }); var branch = new Branch { Code = request.Code.Trim(), Name = request.Name.Trim(), Address = request.Address?.Trim() }; db.Branches.Add(branch); await db.SaveChangesAsync(); return Created($"api/branches/{branch.Id}", branch); } }
