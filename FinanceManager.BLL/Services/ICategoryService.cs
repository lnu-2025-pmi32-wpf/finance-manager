using System.Collections.Generic;
using System.Threading.Tasks;
using FinanceManager.BLL.Models;

namespace FinanceManager.BLL.Services;

public interface ICategoryService
{
    // Get breakdown used by dashboard
    Task<IEnumerable<CategoryBreakdownDto>> GetCategoryBreakdownAsync();

    // CRUD
    Task<IEnumerable<CategoryDto>> GetAllAsync();
    Task<CategoryDto?> GetByIdAsync(int id);
    Task<CategoryDto> CreateAsync(CategoryDto dto);
    Task<bool> UpdateAsync(CategoryDto dto);
    Task<bool> DeleteAsync(int id);
}
