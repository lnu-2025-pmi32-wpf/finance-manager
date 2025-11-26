namespace FinanceManager.UI.Views
{
    // <copyright file="AccountsView.xaml.cs" company="LNU">
    // Copyright (c) LNU. All rights reserved.
    // </copyright>

    using System.Windows;
    using System.Windows.Controls;
    using FinanceManager.UI.ViewModels;

    public partial class AccountsView : UserControl
    {
        private readonly AccountsViewModel vm;

        public AccountsView(AccountsViewModel vm)
        {
            InitializeComponent();
            this.vm = vm;
            this.DataContext = this.vm;

            this.Loaded += async (_, __) => await this.vm.LoadAsync();

            this.BtnAdd.Click += BtnAdd_Click;
            this.BtnEdit.Click += BtnEdit_Click;
            this.BtnDelete.Click += async (_, __) => await this.vm.DeleteSelected();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new AccountEditWindow();
            if (dlg.ShowDialog() == true)
            {
                var dto = dlg.Account;
                _ = this.vm.CreateAsync(dto).ContinueWith(async t => await this.vm.LoadAsync());
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (this.vm.Selected == null) return;
            var dlg = new AccountEditWindow(this.vm.Selected);
            if (dlg.ShowDialog() == true)
            {
                var dto = dlg.Account;
                _ = this.vm.UpdateAsync(dto).ContinueWith(async t => await this.vm.LoadAsync());
            }
        }
    }
}
