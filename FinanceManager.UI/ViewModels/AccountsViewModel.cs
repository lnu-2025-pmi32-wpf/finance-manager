using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FinanceManager.BLL.Models;
using FinanceManager.BLL.Services;

namespace FinanceManager.UI.ViewModels;

public class AccountsViewModel : BaseViewModel
{
    private readonly IAccountService _accountService;

    public ObservableCollection<AccountDto> Accounts { get; } = new ObservableCollection<AccountDto>();

    private AccountDto? _selected;
    public AccountDto? Selected
    {
        get => _selected;
        set { _selected = value; Raise(); }
    }

    public RelayCommand RefreshCommand { get; }
    public RelayCommand DeleteCommand { get; }

    public AccountsViewModel(IAccountService accountService)
    {
        _accountService = accountService;
        RefreshCommand = new RelayCommand(async _ => await LoadAsync());
        DeleteCommand = new RelayCommand(async _ => await DeleteSelected(), _ => Selected != null);
    }

    public async Task LoadAsync()
    {
        Accounts.Clear();
        var list = await _accountService.GetAllAsync();
        foreach (var a in list)
            Accounts.Add(a);
        Raise(nameof(Accounts));
    }

    public async Task DeleteSelected()
    {
        if (Selected == null) return;
        await _accountService.DeleteAsync(Selected.AccountId);
        await LoadAsync();
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
