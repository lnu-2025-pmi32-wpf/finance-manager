using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FinanceManager.BLL.Models;
using FinanceManager.BLL.Services;

namespace FinanceManager.UI.ViewModels;

public class AnalyticsViewModel : BaseViewModel
{
    private readonly IAnalyticsService _analyticsService;

    public AnalyticsSummaryDto Summary { get; private set; } = new AnalyticsSummaryDto();

    public ObservableCollection<CategoryBreakdownDto> Incomes { get; } = new ObservableCollection<CategoryBreakdownDto>();
    public ObservableCollection<CategoryBreakdownDto> Expenses { get; } = new ObservableCollection<CategoryBreakdownDto>();
    public ObservableCollection<GoalProgressDto> Goals { get; } = new ObservableCollection<GoalProgressDto>();

    public AnalyticsViewModel(IAnalyticsService analyticsService)
    {
        _analyticsService = analyticsService;
    }

    public async Task LoadAsync()
    {
        Summary = await _analyticsService.GetSummaryAsync();
        Raise(nameof(Summary));

        Incomes.Clear();
        foreach (var i in await _analyticsService.GetIncomeByCategoryAsync())
            Incomes.Add(i);

        Expenses.Clear();
        foreach (var e in await _analyticsService.GetExpensesByCategoryAsync())
            Expenses.Add(e);

        Goals.Clear();
        foreach (var g in await _analyticsService.GetGoalsProgressAsync())
            Goals.Add(g);
    }
}
