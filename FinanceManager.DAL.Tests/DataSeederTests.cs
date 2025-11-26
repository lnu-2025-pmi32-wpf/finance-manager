using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using FinanceManager.Data;
using Xunit;

namespace FinanceManager.DAL.Tests;

public class DataSeederTests
{
    private AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public void Seed_PopulatesDatabase()
    {
        using var ctx = CreateContext();
        DataSeeder.Seed(ctx);

        // After seeding we expect some data to exist
        ctx.Users.CountAsync().Result.Should().BeGreaterThan(0);
        ctx.Accounts.CountAsync().Result.Should().BeGreaterThan(0);
        ctx.Categories.CountAsync().Result.Should().BeGreaterThan(0);
    }
}
