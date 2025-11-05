using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using FinanceManager.UI.ViewModels;

namespace FinanceManager.UI.Views;

public partial class TransactionsView : UserControl
{
    private readonly TransactionsViewModel _vm;

    public TransactionsView(TransactionsViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        DataContext = _vm;

        Loaded += async (_, __) =>
        {
            await _vm.LoadAccountsAndCategoriesAsync();
            await _vm.LoadAsync();
        };

        BtnRefresh.Click += async (_, __) =>
        {
            _vm.SearchText = TxtSearch.Text;
            _vm.FilterType = (CmbType.SelectedItem as ComboBoxItem)?.Content?.ToString()?.ToLower();
            await _vm.LoadAsync();
        };

        BtnExport.Click += (_, __) => ExportCsv();
    BtnAdd.Click += BtnAdd_Click;
    }

    private async void BtnAdd_Click(object sender, RoutedEventArgs e)
    {
        await _vm.LoadAccountsAndCategoriesAsync();
        var dlg = new TransactionEditWindow(_vm.Accounts, _vm.Categories) { Owner = Window.GetWindow(this) };
        if (dlg.ShowDialog() == true)
        {
            var dto = dlg.Transaction;
            await _vm.CreateAsync(dto);
            await _vm.LoadAsync();
        }
    }

    private async void BtnEdit_Click(object sender, RoutedEventArgs e)
    {
        if (_vm.Selected == null) return;
        await _vm.LoadAccountsAndCategoriesAsync();
        var dlg = new TransactionEditWindow(_vm.Selected, _vm.Accounts, _vm.Categories) { Owner = Window.GetWindow(this) };
        if (dlg.ShowDialog() == true)
        {
            var dto = dlg.Transaction;
            await _vm.UpdateAsync(dto);
            await _vm.LoadAsync();
        }
    }

    private void ExportCsv()
    {
        var list = _vm.Transactions.ToList();
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

    private async void OnDeleteItem(object sender, RoutedEventArgs e)
    {
        if (sender is FrameworkElement fe && fe.DataContext is ViewModels.TransactionListItem item)
        {
            // find underlying TransactionDto
            var dto = _vm.Transactions.FirstOrDefault(t => t.TransactionId == item.TransactionId);
            if (dto == null) return;
            var ok = MessageBox.Show($"Delete transaction #{dto.TransactionId}?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (ok == MessageBoxResult.Yes)
            {
                await _vm.DeleteByIdAsync(dto.TransactionId);
            }
        }
    }

    private async void OnEditItem(object sender, RoutedEventArgs e)
    {
        if (sender is FrameworkElement fe && fe.DataContext is ViewModels.TransactionListItem item)
        {
            var dto = _vm.Transactions.FirstOrDefault(t => t.TransactionId == item.TransactionId);
            if (dto == null) return;
            await _vm.LoadAccountsAndCategoriesAsync();
            var dlg = new TransactionEditWindow(dto, _vm.Accounts, _vm.Categories) { Owner = Window.GetWindow(this) };
            if (dlg.ShowDialog() == true)
            {
                var updated = dlg.Transaction;
                await _vm.UpdateAsync(updated);
                await _vm.LoadAsync();
            }
        }
    }
}
