// <copyright file="TransactionsViewModel.cs" company="LNU">
// Copyright (c) LNU. All rights reserved.
// </copyright>

namespace FinanceManager.UI.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using FinanceManager.BLL.Models;
    using FinanceManager.BLL.Services;
    using System;

    public class TransactionsViewModel : BaseViewModel
    {
        private readonly ITransactionService _transactionService;
        private readonly IAccountService _accountService;
        private readonly ICategoryService _categoryService;

        public ObservableCollection<TransactionDto> Transactions { get; } = new ObservableCollection<TransactionDto>();
        public ObservableCollection<AccountDto> Accounts { get; } = new ObservableCollection<AccountDto>();
        public ObservableCollection<CategoryDto> Categories { get; } = new ObservableCollection<CategoryDto>();

        private TransactionDto? _selected;
        public TransactionDto? Selected
        {
            get => _selected;
            set { _selected = value; this.Raise(); }
        }

        public RelayCommand RefreshCommand { get; }
        public RelayCommand DeleteCommand { get; }

        public string FilterType { get; set; }
        public string SearchText { get; set; }

        public TransactionsViewModel(ITransactionService transactionService, IAccountService accountService, ICategoryService categoryService)
        {
            _transactionService = transactionService;
            _accountService = accountService;
                _categoryService = categoryService;
            this.RefreshCommand = new RelayCommand(async _ => await this.LoadAsync());
            this.DeleteCommand = new RelayCommand(async _ => await this.DeleteSelected(), _ => this.Selected != null);
        }

        public async Task LoadAsync()
        {
            this.Transactions.Clear();
            var q = new TransactionQuery
            {
                Search = string.IsNullOrWhiteSpace(this.SearchText) ? null : this.SearchText,
                Type = string.IsNullOrWhiteSpace(this.FilterType) ? null : this.FilterType
            };
            var list = await _transactionService.GetAllAsync(q);
            foreach (var t in list)
                this.Transactions.Add(t);
            this.Raise(nameof(Transactions));
        }

        public async Task LoadAccountsAndCategoriesAsync()
        {
            this.Accounts.Clear();
            this.Categories.Clear();
            var a = await _accountService.GetAllAsync();
            var c = await _categoryService.GetAllAsync();
            foreach (var x in a) this.Accounts.Add(x);
            foreach (var x in c) this.Categories.Add(x);
            this.Raise(nameof(Accounts));
            this.Raise(nameof(Categories));
        }

        public async Task<TransactionDto> CreateAsync(TransactionDto dto)
        {
            return await _transactionService.CreateAsync(dto);
        }

        public async Task<bool> UpdateAsync(TransactionDto dto)
        {
            return await _transactionService.UpdateAsync(dto);
        }

        public async Task DeleteSelected()
        {
            if (this.Selected == null) return;
            await _transactionService.DeleteAsync(this.Selected.TransactionId);
            await this.LoadAsync();
        }
    }
}
