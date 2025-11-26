namespace FinanceManager.UI.Views
{
    // <copyright file="AccountEditWindow.xaml.cs" company="LNU">
    // Copyright (c) LNU. All rights reserved.
    // </copyright>

    using System;
    using System.Globalization;
    using System.Windows;
    using FinanceManager.BLL.Models;

    public partial class AccountEditWindow : Window
    {
        public AccountDto Account { get; private set; }

        public AccountEditWindow()
        {
            InitializeComponent();
            this.Account = new AccountDto { ProfileId = 1, Balance = 0m, CurrencyCode = "USD" };
            this.BtnOk.Click += BtnOk_Click;
            this.BtnCancel.Click += (_, __) => DialogResult = false;
        }

        public AccountEditWindow(AccountDto existing) : this()
        {
            this.Account = new AccountDto
            {
                AccountId = existing.AccountId,
                ProfileId = existing.ProfileId,
                Name = existing.Name,
                Balance = existing.Balance,
                CurrencyCode = existing.CurrencyCode
            };
            this.TxtName.Text = this.Account.Name;
            this.TxtBalance.Text = this.Account.Balance.ToString(CultureInfo.InvariantCulture);
            this.TxtCurrency.Text = this.Account.CurrencyCode;
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            this.Account.Name = this.TxtName.Text?.Trim() ?? string.Empty;
            this.Account.CurrencyCode = this.TxtCurrency.Text?.Trim() ?? string.Empty;

            if (decimal.TryParse(this.TxtBalance.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out var bal))
            {
                this.Account.Balance = bal;
            }

            if (string.IsNullOrWhiteSpace(this.Account.Name))
            {
                MessageBox.Show("Name is required", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DialogResult = true;
        }
    }
}
