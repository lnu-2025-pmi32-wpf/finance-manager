using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using FinanceManager.Data;
using FinanceManager.BLL.Services;
using Xunit;

namespace FinanceManager.BLL.Tests;

public class AccountServiceTests
{
    private AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task CreateAsync_AddsAccount_And_ReturnsDto()
    {
        using var ctx = CreateContext();
        var svc = new AccountService(ctx);

        var dto = new FinanceManager.BLL.Models.AccountDto
        {
            ProfileId = 1,
            Name = "Test Account",
            Balance = 100m,
            CurrencyCode = "USD"
        };

        var created = await svc.CreateAsync(dto);

        created.AccountId.Should().BeGreaterThan(0);
        (await ctx.Accounts.CountAsync()).Should().Be(1);
    }

    [Fact]
    public async Task GetTotalBalanceAsync_ReturnsSum()
    {
        using var ctx = CreateContext();
        ctx.Accounts.Add(new FinanceManager.Models.Account { ProfileId = 1, Name = "A", Balance = 10m, CurrencyCode = "USD" });
        ctx.Accounts.Add(new FinanceManager.Models.Account { ProfileId = 1, Name = "B", Balance = 5m, CurrencyCode = "USD" });
        await ctx.SaveChangesAsync();

        var svc = new AccountService(ctx);
        var sum = await svc.GetTotalBalanceAsync();
        sum.Should().Be(15m);
    }
}
