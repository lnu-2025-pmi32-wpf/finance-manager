using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FinanceManager.Data;

namespace FinanceManager.BLL.Services;

public class AccountService : IAccountService
{
    private readonly AppDbContext _db;
    public AccountService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<decimal> GetTotalBalanceAsync()
    {
        var sum = await _db.Accounts.SumAsync(a => (decimal?)a.Balance) ?? 0m;
        return sum;
    }

    public async Task<IEnumerable<FinanceManager.BLL.Models.AccountDto>> GetAllAsync()
    {
        return await _db.Accounts
            .Select(a => new FinanceManager.BLL.Models.AccountDto
            {
                AccountId = a.AccountId,
                ProfileId = a.ProfileId,
                Name = a.Name,
                Balance = a.Balance,
                CurrencyCode = a.CurrencyCode
            })
            .ToListAsync();
    }

    public async Task<FinanceManager.BLL.Models.AccountDto?> GetByIdAsync(int id)
    {
        var a = await _db.Accounts.FindAsync(id);
        if (a == null) return null;
        return new FinanceManager.BLL.Models.AccountDto
        {
            AccountId = a.AccountId,
            ProfileId = a.ProfileId,
            Name = a.Name,
            Balance = a.Balance,
            CurrencyCode = a.CurrencyCode
        };
    }

    public async Task<FinanceManager.BLL.Models.AccountDto> CreateAsync(FinanceManager.BLL.Models.AccountDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new System.ArgumentException("Account name is required");

        var entity = new FinanceManager.Models.Account
        {
            ProfileId = dto.ProfileId,
            Name = dto.Name,
            Balance = dto.Balance,
            CurrencyCode = dto.CurrencyCode
        };

        _db.Accounts.Add(entity);
        await _db.SaveChangesAsync();

        dto.AccountId = entity.AccountId;
        return dto;
    }

    public async Task<bool> UpdateAsync(FinanceManager.BLL.Models.AccountDto dto)
    {
        var e = await _db.Accounts.FindAsync(dto.AccountId);
        if (e == null) return false;
        e.Name = dto.Name;
        e.Balance = dto.Balance;
        e.CurrencyCode = dto.CurrencyCode;
        e.ProfileId = dto.ProfileId;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var e = await _db.Accounts.FindAsync(id);
        if (e == null) return false;
        _db.Accounts.Remove(e);
        await _db.SaveChangesAsync();
        return true;
    }
}
