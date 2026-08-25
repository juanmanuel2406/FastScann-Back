using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FastScan.Services;

public class FastScanDbContextFactory : IDesignTimeDbContextFactory<FastScanDbContext>
{
    public FastScanDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<FastScanDbContext>();
        options.UseMySql(
            "Server=localhost;Database=fastscan;User=root;Password=placeholder;",
            new MySqlServerVersion(new Version(8, 0, 0)));
        return new FastScanDbContext(options.Options);
    }
}
