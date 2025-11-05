using System.Collections.Generic;
using System.Threading.Tasks;
using FinanceManager.BLL.Models;

namespace FinanceManager.BLL.Services;

public interface IAccountService
{
    Task<decimal> GetTotalBalanceAsync();

    Task<IEnumerable<AccountDto>> GetAllAsync();
    Task<AccountDto?> GetByIdAsync(int id);
    Task<AccountDto> CreateAsync(AccountDto dto);
    Task<bool> UpdateAsync(AccountDto dto);
    Task<bool> DeleteAsync(int id);
}
