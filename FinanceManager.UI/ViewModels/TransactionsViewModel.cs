using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FinanceManager.BLL.Models;
using FinanceManager.BLL.Services;
using System;

namespace FinanceManager.UI.ViewModels;

public class TransactionsViewModel : BaseViewModel
{
    private readonly ITransactionService _transactionService;
    private readonly IAccountService _accountService;
    private readonly ICategoryService _categoryService;

    public ObservableCollection<TransactionDto> Transactions { get; } = new ObservableCollection<TransactionDto>();
    public System.Collections.ObjectModel.ObservableCollection<AccountDto> Accounts { get; } = new System.Collections.ObjectModel.ObservableCollection<AccountDto>();
    public System.Collections.ObjectModel.ObservableCollection<CategoryDto> Categories { get; } = new System.Collections.ObjectModel.ObservableCollection<CategoryDto>();

    private TransactionDto? _selected;
    public TransactionDto? Selected
    {
        get => _selected;
        set { _selected = value; Raise(); }
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
        RefreshCommand = new RelayCommand(async _ => await LoadAsync());
        DeleteCommand = new RelayCommand(async _ => await DeleteSelected(), _ => Selected != null);
    }

    public async Task LoadAsync()
    {
        Transactions.Clear();
        var q = new TransactionQuery
        {
            Search = string.IsNullOrWhiteSpace(SearchText) ? null : SearchText,
            Type = string.IsNullOrWhiteSpace(FilterType) ? null : FilterType
        };
        var list = await _transactionService.GetAllAsync(q);
        foreach (var t in list)
            Transactions.Add(t);
        Raise(nameof(Transactions));
    }

    public async Task LoadAccountsAndCategoriesAsync()
    {
        Accounts.Clear();
        Categories.Clear();
        var a = await _accountService.GetAllAsync();
        var c = await _categoryService.GetAllAsync();
        foreach (var x in a) Accounts.Add(x);
        foreach (var x in c) Categories.Add(x);
        Raise(nameof(Accounts));
        Raise(nameof(Categories));
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
        if (Selected == null) return;
        await _transactionService.DeleteAsync(Selected.TransactionId);
        await LoadAsync();
    }
}
