using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FinanceManager.Data;
using FinanceManager.BLL.Models;
using System.Collections.Generic;

using FinanceManager.Models;

namespace FinanceManager.BLL.Services;

public class TransactionService : ITransactionService
{
    private readonly AppDbContext _db;
    public TransactionService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<decimal> GetTotalExpensesCurrentMonthAsync()
    {
    var now = DateTime.UtcNow;
    // Ensure start and end are UTC DateTime (Npgsql requires UTC for timestamptz)
    var start = DateTime.SpecifyKind(new DateTime(now.Year, now.Month, 1, 0, 0, 0), DateTimeKind.Utc);
    var end = DateTime.SpecifyKind(start.AddMonths(1), DateTimeKind.Utc);
        var sum = await _db.Transactions
            .Where(t => t.TransactionDateTime >= start && t.TransactionDateTime < end && t.Amount < 0)
            .SumAsync(t => (decimal?)t.Amount) ?? 0m;
        return Math.Abs(sum);
    }

    public async Task<decimal> GetTotalIncomeCurrentMonthAsync()
    {
    var now = DateTime.UtcNow;
    var start = DateTime.SpecifyKind(new DateTime(now.Year, now.Month, 1, 0, 0, 0), DateTimeKind.Utc);
    var end = DateTime.SpecifyKind(start.AddMonths(1), DateTimeKind.Utc);
        var sum = await _db.Transactions
            .Where(t => t.TransactionDateTime >= start && t.TransactionDateTime < end && t.Amount > 0)
            .SumAsync(t => (decimal?)t.Amount) ?? 0m;
        return sum;
    }

    public async Task<IEnumerable<TransactionDto>> GetAllAsync(TransactionQuery? query = null)
    {
        var q = _db.Transactions.AsQueryable();

        if (query != null)
        {
            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                var s = query.Search.Trim();
                q = q.Where(t => t.Description != null && t.Description.Contains(s));
            }

            if (!string.IsNullOrWhiteSpace(query.Type))
            {
                if (query.Type.Equals("income", StringComparison.OrdinalIgnoreCase))
                    q = q.Where(t => t.Amount > 0);
                else if (query.Type.Equals("expense", StringComparison.OrdinalIgnoreCase))
                    q = q.Where(t => t.Amount < 0);
            }

            if (query.From.HasValue)
            {
                var fromUtc = DateTime.SpecifyKind(query.From.Value.Date, DateTimeKind.Utc);
                q = q.Where(t => t.TransactionDateTime >= fromUtc);
            }
            if (query.To.HasValue)
            {
                var toUtc = DateTime.SpecifyKind(query.To.Value.Date.AddDays(1), DateTimeKind.Utc);
                q = q.Where(t => t.TransactionDateTime < toUtc);
            }
        }

        var list = await q.OrderByDescending(t => t.TransactionDateTime)
            .Select(t => new TransactionDto
            {
                TransactionId = t.TransactionId,
                AccountId = t.AccountId,
                CategoryId = t.CategoryId,
                Amount = t.Amount,
                TransactionDateTime = t.TransactionDateTime,
                Description = t.Description
            }).ToListAsync();

        return list;
    }

    public async Task<TransactionDto?> GetByIdAsync(int id)
    {
        var t = await _db.Transactions.FindAsync(id);
        if (t == null) return null;
        return new TransactionDto
        {
            TransactionId = t.TransactionId,
            AccountId = t.AccountId,
            CategoryId = t.CategoryId,
            Amount = t.Amount,
            TransactionDateTime = t.TransactionDateTime,
            Description = t.Description
        };
    }

    public async Task<TransactionDto> CreateAsync(TransactionDto dto)
    {
        if (!dto.AccountId.HasValue)
            throw new ArgumentException("AccountId is required for a transaction");

        var entity = new FinanceManager.Models.Transaction
        {
            AccountId = dto.AccountId.Value,
            CategoryId = dto.CategoryId,
            Amount = dto.Amount,
            TransactionDateTime = DateTime.SpecifyKind(dto.TransactionDateTime, DateTimeKind.Utc),
            Description = dto.Description
        };
        _db.Transactions.Add(entity);
        await _db.SaveChangesAsync();
        dto.TransactionId = entity.TransactionId;
        return dto;
    }

    public async Task<bool> UpdateAsync(TransactionDto dto)
    {
    var e = await _db.Transactions.FindAsync(dto.TransactionId);
        if (e == null) return false;
    e.AccountId = dto.AccountId ?? e.AccountId;
    e.CategoryId = dto.CategoryId;
        e.Amount = dto.Amount;
        e.TransactionDateTime = DateTime.SpecifyKind(dto.TransactionDateTime, DateTimeKind.Utc);
    e.Description = dto.Description;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var e = await _db.Transactions.FindAsync(id);
        if (e == null) return false;
        _db.Transactions.Remove(e);
        await _db.SaveChangesAsync();
        return true;
    }
}
