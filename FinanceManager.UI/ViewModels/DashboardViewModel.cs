using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FinanceManager.BLL.Models;
using FinanceManager.BLL.Services;

namespace FinanceManager.UI.ViewModels;

public class DashboardViewModel : BaseViewModel
{
    private readonly IAccountService _accountService;
    private readonly ITransactionService _transactionService;
    private readonly ICategoryService _categoryService;

    private decimal _totalBalance;
    public decimal TotalBalance { get => _totalBalance; set { _totalBalance = value; Raise(); } }

    private decimal _totalExpenses;
    public decimal TotalExpenses { get => _totalExpenses; set { _totalExpenses = value; Raise(); } }

    private decimal _totalIncome;
    public decimal TotalIncome { get => _totalIncome; set { _totalIncome = value; Raise(); } }

    public ObservableCollection<CategoryBreakdownDto> Categories { get; } = new ObservableCollection<CategoryBreakdownDto>();

    public DashboardViewModel(IAccountService accountService, ITransactionService transactionService, ICategoryService categoryService)
    {
        _accountService = accountService;
        _transactionService = transactionService;
        _categoryService = categoryService;
    }

    public async Task LoadAsync()
    {
        TotalBalance = await _accountService.GetTotalBalanceAsync();
        TotalExpenses = await _transactionService.GetTotalExpensesCurrentMonthAsync();
        TotalIncome = await _transactionService.GetTotalIncomeCurrentMonthAsync();

        Categories.Clear();
        var list = await _categoryService.GetCategoryBreakdownAsync();
        foreach (var c in list)
            Categories.Add(c);
    }
}
