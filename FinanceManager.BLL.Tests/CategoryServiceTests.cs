using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using FinanceManager.Data;
using FinanceManager.BLL.Services;
using FinanceManager.BLL.Models;
using FinanceManager.Enums;
using Xunit;

namespace FinanceManager.BLL.Tests;

public class CategoryServiceTests
{
    private AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task CreateAsync_AddsCategory_And_ReturnsDto()
    {
        using var ctx = CreateContext();
        var svc = new CategoryService(ctx);

        var dto = new CategoryDto
        {
            ProfileId = 1,
            Name = "Food",
            Type = CategoryType.Expense,
            Icon = "food",
            ColorHex = "#fff"
        };

        var created = await svc.CreateAsync(dto);
        created.CategoryId.Should().BeGreaterThan(0);
        (await ctx.Categories.CountAsync()).Should().Be(1);
    }

    [Fact]
    public async Task GetCategoryBreakdownAsync_ReturnsSums()
    {
        using var ctx = CreateContext();
        var profile = new FinanceManager.Models.FinancialProfile { Name = "p", MainCurrencyCode = "USD", UserId = 0 };
        ctx.FinancialProfiles.Add(profile);
        await ctx.SaveChangesAsync();

        var cat = new FinanceManager.Models.Category { ProfileId = profile.ProfileId, Name = "Food", Type = CategoryType.Expense, Icon = "food", ColorHex = "#fff" };
        ctx.Categories.Add(cat);
        await ctx.SaveChangesAsync();

        var acc = new FinanceManager.Models.Account { ProfileId = profile.ProfileId, Name = "A", Balance = 0m, CurrencyCode = "USD" };
        ctx.Accounts.Add(acc);
        await ctx.SaveChangesAsync();

        ctx.Transactions.Add(new FinanceManager.Models.Transaction { Account = acc, Category = cat, Amount = -20m, TransactionDateTime = System.DateTime.UtcNow, Description = "test" });
        await ctx.SaveChangesAsync();

        var svc = new CategoryService(ctx);
        var list = await svc.GetCategoryBreakdownAsync();
        list.Should().ContainSingle(x => x.CategoryId == cat.CategoryId && x.Amount == -20m);
    }
}
