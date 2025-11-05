using System;
using System.Globalization;
using System.Windows;
using FinanceManager.BLL.Models;

namespace FinanceManager.UI.Views;

public partial class AccountEditWindow : Window
{
    public AccountDto Account { get; private set; }

    public AccountEditWindow()
    {
        InitializeComponent();
        Account = new AccountDto { ProfileId = 1, Balance = 0m, CurrencyCode = "USD" };
        BtnOk.Click += BtnOk_Click;
        BtnCancel.Click += (_, __) => DialogResult = false;
    }

    public AccountEditWindow(AccountDto existing) : this()
    {
        Account = new AccountDto
        {
            AccountId = existing.AccountId,
            ProfileId = existing.ProfileId,
            Name = existing.Name,
            Balance = existing.Balance,
            CurrencyCode = existing.CurrencyCode
        };
        TxtName.Text = Account.Name;
        TxtBalance.Text = Account.Balance.ToString(CultureInfo.InvariantCulture);
        TxtCurrency.Text = Account.CurrencyCode;
    }

    private void BtnOk_Click(object sender, RoutedEventArgs e)
    {
        Account.Name = TxtName.Text?.Trim() ?? string.Empty;
        Account.CurrencyCode = TxtCurrency.Text?.Trim() ?? "";

        if (decimal.TryParse(TxtBalance.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out var bal))
            Account.Balance = bal;

        if (string.IsNullOrWhiteSpace(Account.Name))
        {
            MessageBox.Show("Name is required", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        DialogResult = true;
    }
}
