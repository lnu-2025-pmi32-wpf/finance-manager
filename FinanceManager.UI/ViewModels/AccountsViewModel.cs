// <copyright file="AccountsViewModel.cs" company="LNU">
// Copyright (c) LNU. All rights reserved.
// </copyright>

namespace FinanceManager.UI.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using FinanceManager.BLL.Models;
    using FinanceManager.BLL.Services;

    public class AccountsViewModel : BaseViewModel
    {
        private readonly IAccountService _accountService;

        public ObservableCollection<AccountDto> Accounts { get; } = new ObservableCollection<AccountDto>();

        private AccountDto? _selected;
        public AccountDto? Selected
        {
            get => _selected;
            set { _selected = value; this.Raise(); }
        }

        public RelayCommand RefreshCommand { get; }
        public RelayCommand DeleteCommand { get; }

        public AccountsViewModel(IAccountService accountService)
        {
            _accountService = accountService;
            this.RefreshCommand = new RelayCommand(async _ => await this.LoadAsync());
            this.DeleteCommand = new RelayCommand(async _ => await this.DeleteSelected(), _ => this.Selected != null);
        }

        public async Task LoadAsync()
        {
            this.Accounts.Clear();
            var list = await _accountService.GetAllAsync();
            foreach (var a in list)
                this.Accounts.Add(a);
            this.Raise(nameof(Accounts));
        }

        public async Task DeleteSelected()
        {
            if (this.Selected == null) return;
            await _accountService.DeleteAsync(this.Selected.AccountId);
            await this.LoadAsync();
        }

        public async Task<AccountDto> CreateAsync(AccountDto dto)
        {
            return await _accountService.CreateAsync(dto);
        }

        public async Task<bool> UpdateAsync(AccountDto dto)
        {
            return await _accountService.UpdateAsync(dto);
        }
    }
}
