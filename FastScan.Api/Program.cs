using FastScan.Services;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("FastScanDatabase") ?? Environment.GetEnvironmentVariable("MYSQL_URL");
if (string.IsNullOrWhiteSpace(connectionString)) throw new InvalidOperationException("Configurá ConnectionStrings__FastScanDatabase o MYSQL_URL.");
builder.Services.AddDbContext<FastScanDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
builder.Services.AddControllers(); builder.Services.AddOpenApi();
var app = builder.Build(); app.MapOpenApi(); app.UseAuthorization(); app.MapControllers(); app.Run();
