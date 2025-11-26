namespace FinanceManager.UI.Views
{
    // <copyright file="TransactionsView.xaml.cs" company="LNU">
    // Copyright (c) LNU. All rights reserved.
    // </copyright>

    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using FinanceManager.UI.ViewModels;

    public partial class TransactionsView : UserControl
    {
        private readonly TransactionsViewModel vm;

        public TransactionsView(TransactionsViewModel vm)
        {
            InitializeComponent();
            this.vm = vm;
            this.DataContext = this.vm;

            this.Loaded += async (_, __) =>
            {
                await this.vm.LoadAccountsAndCategoriesAsync();
                await this.vm.LoadAsync();
            };

            this.BtnRefresh.Click += async (_, __) =>
            {
                this.vm.SearchText = this.TxtSearch.Text;
                this.vm.FilterType = (this.CmbType.SelectedItem as ComboBoxItem)?.Content?.ToString()?.ToLower();
                await this.vm.LoadAsync();
            };

            this.BtnExport.Click += (_, __) => ExportCsv();
            this.BtnAdd.Click += BtnAdd_Click;
            this.BtnEdit.Click += BtnEdit_Click;
        }

        private async void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            await this.vm.LoadAccountsAndCategoriesAsync();
            var dlg = new TransactionEditWindow(this.vm.Accounts, this.vm.Categories) { Owner = Window.GetWindow(this) };
            if (dlg.ShowDialog() == true)
            {
                var dto = dlg.Transaction;
                await this.vm.CreateAsync(dto);
                await this.vm.LoadAsync();
            }
        }

        private async void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (this.vm.Selected == null) return;
            await this.vm.LoadAccountsAndCategoriesAsync();
            var dlg = new TransactionEditWindow(this.vm.Selected, this.vm.Accounts, this.vm.Categories) { Owner = Window.GetWindow(this) };
            if (dlg.ShowDialog() == true)
            {
                var dto = dlg.Transaction;
                await this.vm.UpdateAsync(dto);
                await this.vm.LoadAsync();
            }
        }

        private void ExportCsv()
        {
            var list = this.vm.Transactions.ToList();
            if (!list.Any())
            {
                MessageBox.Show("No transactions to export.", "Export", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var sb = new StringBuilder();
            sb.AppendLine("Id,Date,Description,Amount");
            foreach (var t in list)
            {
                sb.AppendLine($"{t.TransactionId},\"{t.TransactionDateTime:O}\",\"{(t.Description ?? string.Empty).Replace("\"","\"\"")}\",{t.Amount}");
            }

            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "transactions_export.csv");
            File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
            MessageBox.Show($"Exported to: {path}", "Export", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
