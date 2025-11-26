using System.Linq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using FinanceManager.Data;
using FinanceManager.BLL.Services;
using Xunit;

namespace FinanceManager.BLL.Tests;

public class FinancialProfileServiceTests
{
    private AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public void GetAllProfiles_ReturnsProfiles()
    {
        using var ctx = CreateContext();
        ctx.FinancialProfiles.Add(new FinanceManager.Models.FinancialProfile { Name = "p1", MainCurrencyCode = "USD" });
        ctx.FinancialProfiles.Add(new FinanceManager.Models.FinancialProfile { Name = "p2", MainCurrencyCode = "EUR" });
        ctx.SaveChanges();

        var svc = new FinancialProfileService(ctx);
        var all = svc.GetAllProfiles().ToList();
        all.Count.Should().Be(2);
    }
}
