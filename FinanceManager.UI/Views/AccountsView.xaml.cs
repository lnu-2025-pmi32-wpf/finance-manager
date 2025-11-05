using System.Windows;
using System.Windows.Controls;
using FinanceManager.UI.ViewModels;

namespace FinanceManager.UI.Views;

public partial class AccountsView : UserControl
{
    private readonly AccountsViewModel _vm;

    public AccountsView(AccountsViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        DataContext = _vm;

        Loaded += async (_, __) => await _vm.LoadAsync();

    BtnAdd.Click += BtnAdd_Click;
    BtnEdit.Click += BtnEdit_Click;
    BtnDelete.Click += async (_, __) => await _vm.DeleteSelected();
    }

    private void BtnAdd_Click(object sender, RoutedEventArgs e)
    {
        var dlg = new AccountEditWindow();
        if (dlg.ShowDialog() == true)
        {
            var dto = dlg.Account;
            _ = _vm.CreateAsync(dto).ContinueWith(async t => await _vm.LoadAsync());
        }
    }

    private void BtnEdit_Click(object sender, RoutedEventArgs e)
    {
        if (_vm.Selected == null) return;
        var dlg = new AccountEditWindow(_vm.Selected);
        if (dlg.ShowDialog() == true)
        {
            var dto = dlg.Account;
            _ = _vm.UpdateAsync(dto).ContinueWith(async t => await _vm.LoadAsync());
        }
    }
}
