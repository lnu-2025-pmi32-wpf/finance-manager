using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using FinanceManager.Data;
using FinanceManager.BLL.Services;
using FinanceManager.BLL.Models;
using Xunit;

namespace FinanceManager.BLL.Tests;

public class TransactionServiceTests
{
    private AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task CreateAsync_WithoutAccountId_Throws()
    {
        using var ctx = CreateContext();
        var svc = new TransactionService(ctx);

        var dto = new TransactionDto { Amount = 10, TransactionDateTime = DateTime.UtcNow };
        await FluentActions.Invoking(() => svc.CreateAsync(dto)).Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task GetTotalExpensesCurrentMonthAsync_ComputesCorrectly()
    {
        using var ctx = CreateContext();
        // prepare account and transactions
        var acc = new FinanceManager.Models.Account { ProfileId = 1, Name = "A", Balance = 0m, CurrencyCode = "USD" };
        ctx.Accounts.Add(acc);
        await ctx.SaveChangesAsync();

        // expense this month
        ctx.Transactions.Add(new FinanceManager.Models.Transaction { Account = acc, Amount = -50m, TransactionDateTime = DateTime.UtcNow.AddDays(-1), Description = "expense" });
        // income this month
        ctx.Transactions.Add(new FinanceManager.Models.Transaction { Account = acc, Amount = 200m, TransactionDateTime = DateTime.UtcNow.AddDays(-2), Description = "income" });
        // expense previous month
        ctx.Transactions.Add(new FinanceManager.Models.Transaction { Account = acc, Amount = -10m, TransactionDateTime = DateTime.UtcNow.AddMonths(-1), Description = "old" });
        await ctx.SaveChangesAsync();

        var svc = new TransactionService(ctx);
        var expenses = await svc.GetTotalExpensesCurrentMonthAsync();
        expenses.Should().Be(50m);
        var income = await svc.GetTotalIncomeCurrentMonthAsync();
        income.Should().Be(200m);
    }
}
