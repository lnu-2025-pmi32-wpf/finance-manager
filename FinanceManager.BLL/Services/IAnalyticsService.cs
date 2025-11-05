using System.Collections.Generic;
using System.Threading.Tasks;
using FinanceManager.BLL.Models;

namespace FinanceManager.BLL.Services;

public interface IAnalyticsService
{
    Task<AnalyticsSummaryDto> GetSummaryAsync(int profileId = 0);

    Task<IEnumerable<CategoryBreakdownDto>> GetIncomeByCategoryAsync(int profileId = 0);
    Task<IEnumerable<CategoryBreakdownDto>> GetExpensesByCategoryAsync(int profileId = 0);

    Task<IEnumerable<GoalProgressDto>> GetGoalsProgressAsync(int profileId = 0);
}
