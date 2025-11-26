// <copyright file="DashboardViewModel.cs" company="LNU">
// Copyright (c) LNU. All rights reserved.
// </copyright>

namespace FinanceManager.UI.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using FinanceManager.BLL.Models;
    using FinanceManager.BLL.Services;

    public class DashboardViewModel : BaseViewModel
    {
        private readonly IAccountService _accountService;
        private readonly ITransactionService _transactionService;
        private readonly ICategoryService _categoryService;

        private decimal _totalBalance;
        public decimal TotalBalance { get => _totalBalance; set { _totalBalance = value; this.Raise(); } }

        private decimal _totalExpenses;
        public decimal TotalExpenses { get => _totalExpenses; set { _totalExpenses = value; this.Raise(); } }

        private decimal _totalIncome;
        public decimal TotalIncome { get => _totalIncome; set { _totalIncome = value; this.Raise(); } }

        public ObservableCollection<CategoryBreakdownDto> Categories { get; } = new ObservableCollection<CategoryBreakdownDto>();

        public DashboardViewModel(IAccountService accountService, ITransactionService transactionService, ICategoryService categoryService)
        {
            _accountService = accountService;
            _transactionService = transactionService;
            _categoryService = categoryService;
        }

        public async Task LoadAsync()
        {
            this.TotalBalance = await _accountService.GetTotalBalanceAsync();
            this.TotalExpenses = await _transactionService.GetTotalExpensesCurrentMonthAsync();
            this.TotalIncome = await _transactionService.GetTotalIncomeCurrentMonthAsync();

            this.Categories.Clear();
            var list = await _categoryService.GetCategoryBreakdownAsync();
            foreach (var c in list)
                this.Categories.Add(c);
        }
    }
}
