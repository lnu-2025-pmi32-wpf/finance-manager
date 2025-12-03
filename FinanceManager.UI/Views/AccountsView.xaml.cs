namespace FinanceManager.UI.Views
{
    // <copyright file="AccountsView.xaml.cs" company="LNU">
    // Copyright (c) LNU. All rights reserved.
    // </copyright>

    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Animation;
    using FinanceManager.UI.ViewModels;
    using FinanceManager.UI.Services;

    public partial class AccountsView : UserControl
    {
        private readonly AccountsViewModel vm;
        private readonly IAppLogger _logger;

        public AccountsView(AccountsViewModel vm, IAppLogger logger)
        {
            InitializeComponent();
            this.vm = vm;
            this._logger = logger;
            this.DataContext = this.vm;

            this.Loaded += AccountsView_Loaded;

            this.BtnAdd.Click += BtnAdd_Click;
            this.BtnEdit.Click += BtnEdit_Click;
            this.BtnDelete.Click += async (_, __) => await this.vm.DeleteSelected();
        }

        private async void AccountsView_Loaded(object? sender, RoutedEventArgs e)
        {
            try
            {
                _logger?.Info("AccountsView loaded");
                // start enter animation if present
                if (FindResource("EnterAnimation") is Storyboard sb)
                {
                    sb.Begin(this);
                    _logger?.Trace("AccountsView enter animation started");
                }

                await this.vm.LoadAsync();
            }
            catch (System.Exception ex)
            {
                _logger?.Error($"AccountsView load error: {ex}");
            }
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
