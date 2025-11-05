using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinanceManager.BLL.Models;

namespace FinanceManager.BLL.Services;

public interface ITransactionService
{
    Task<decimal> GetTotalExpensesCurrentMonthAsync();
    Task<decimal> GetTotalIncomeCurrentMonthAsync();

    // CRUD & filtering
    Task<IEnumerable<TransactionDto>> GetAllAsync(TransactionQuery? query = null);
    Task<TransactionDto?> GetByIdAsync(int id);
    Task<TransactionDto> CreateAsync(TransactionDto dto);
    Task<bool> UpdateAsync(TransactionDto dto);
    Task<bool> DeleteAsync(int id);
}
