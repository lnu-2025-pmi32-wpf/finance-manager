using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FinanceManager.Data;
using FinanceManager.BLL.Models;

namespace FinanceManager.BLL.Services;

public class AnalyticsService : IAnalyticsService
{
    private readonly AppDbContext _db;
    public AnalyticsService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<AnalyticsSummaryDto> GetSummaryAsync(int profileId = 0)
    {
        var net = await _db.Accounts.SumAsync(a => (decimal?)a.Balance) ?? 0m;

        var now = DateTime.UtcNow;
        var start = DateTime.SpecifyKind(new DateTime(now.Year, now.Month, 1), DateTimeKind.Utc);
        var end = start.AddMonths(1);

        var totalIncome = await _db.Transactions
            .Where(t => t.TransactionDateTime >= start && t.TransactionDateTime < end && t.Amount > 0)
            .SumAsync(t => (decimal?)t.Amount) ?? 0m;

        var totalExpense = await _db.Transactions
            .Where(t => t.TransactionDateTime >= start && t.TransactionDateTime < end && t.Amount < 0)
            .SumAsync(t => (decimal?)t.Amount) ?? 0m;

        var savingRate = 0;
        if (totalIncome > 0)
            savingRate = (int)System.Math.Round((double)((totalIncome + totalExpense) / totalIncome * 100)); // totalExpense is negative

        var topIncome = await _db.Transactions
            .Where(t => t.Amount > 0)
            .GroupBy(t => t.Category != null ? t.Category.Name : "Other")
            .Select(g => new { Name = g.Key, Sum = g.Sum(x => x.Amount) })
            .OrderByDescending(x => x.Sum)
            .FirstOrDefaultAsync();

        var topExpense = await _db.Transactions
            .Where(t => t.Amount < 0)
            .GroupBy(t => t.Category != null ? t.Category.Name : "Other")
            .Select(g => new { Name = g.Key, Sum = g.Sum(x => x.Amount) })
            .OrderBy(x => x.Sum) // expenses negative, smallest (most negative) is largest expense
            .FirstOrDefaultAsync();

        return new AnalyticsSummaryDto
        {
            NetBalance = net,
            TotalIncome = totalIncome,
            TotalExpense = Math.Abs(totalExpense),
            SavingRatePercent = savingRate,
            TopIncomeSource = topIncome?.Name ?? "-",
            TopIncomeAmount = topIncome?.Sum ?? 0m,
            LargestExpenseCategory = topExpense?.Name ?? "-",
            LargestExpenseAmount = Math.Abs(topExpense?.Sum ?? 0m)
        };
    }

    public async Task<IEnumerable<CategoryBreakdownDto>> GetIncomeByCategoryAsync(int profileId = 0)
    {
        var list = await _db.Transactions
            .Where(t => t.Amount > 0)
            .GroupBy(t => t.Category != null ? t.Category.Name : "Other")
            .Select(g => new CategoryBreakdownDto
            {
                CategoryId = 0,
                Name = g.Key,
                Amount = g.Sum(x => x.Amount),
                Type = "income"
            })
            .OrderByDescending(x => x.Amount)
            .ToListAsync();

        return list;
    }

    public async Task<IEnumerable<CategoryBreakdownDto>> GetExpensesByCategoryAsync(int profileId = 0)
    {
        var list = await _db.Transactions
            .Where(t => t.Amount < 0)
            .GroupBy(t => t.Category != null ? t.Category.Name : "Other")
            .Select(g => new CategoryBreakdownDto
            {
                CategoryId = 0,
                Name = g.Key,
                Amount = Math.Abs(g.Sum(x => x.Amount)),
                Type = "expense"
            })
            .OrderByDescending(x => x.Amount)
            .ToListAsync();

        return list;
    }

    public async Task<IEnumerable<GoalProgressDto>> GetGoalsProgressAsync(int profileId = 0)
    {
        var q = _db.Goals.AsQueryable();
        if (profileId > 0)
            q = q.Where(g => g.ProfileId == profileId);

        var list = await q.Select(g => new GoalProgressDto
        {
            GoalId = g.GoalId,
            Name = g.Name,
            CurrentAmount = g.CurrentAmount,
            TargetAmount = g.TargetAmount
        }).ToListAsync();

        return list;
    }
}
