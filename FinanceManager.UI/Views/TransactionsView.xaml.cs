using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using FinanceManager.UI.ViewModels;
using FinanceManager.UI.Services;

namespace FinanceManager.UI.Views;

public partial class TransactionsView : UserControl
{
    private readonly TransactionsViewModel _vm;
    private readonly IAppLogger _logger;

    public TransactionsView(TransactionsViewModel vm, IAppLogger logger)
    {
        InitializeComponent();
        _vm = vm;
        _logger = logger;
        DataContext = _vm;

        Loaded += async (_, __) =>
        {
            try
            {
                _logger?.Info("TransactionsView loaded");
                if (FindResource("EnterAnimation") is Storyboard sb)
                {
                    sb.Begin(this);
                    _logger?.Trace("TransactionsView enter animation started");
                }

                await _vm.LoadAccountsAndCategoriesAsync();
                await _vm.LoadAsync();
            }
            catch (Exception ex)
            {
                _logger?.Error($"TransactionsView load error: {ex}");
            }
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
        try
        {
            await _vm.LoadAccountsAndCategoriesAsync();
            var dlg = new TransactionEditWindow(_vm.Accounts, _vm.Categories) { Owner = Window.GetWindow(this) };
            if (dlg.ShowDialog() == true)
            {
                var dto = dlg.Transaction;
                _logger?.Info($"Creating transaction: {dto.Description} {dto.Amount}");
                await _vm.CreateAsync(dto);
                _logger?.Info($"Transaction created: {dto.TransactionId}");
                await _vm.LoadAsync();
            }
        }
        catch (Exception ex)
        {
            _logger?.Error($"Error creating transaction: {ex}");
        }
    }

    private async void BtnEdit_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (_vm.Selected == null) return;
            await _vm.LoadAccountsAndCategoriesAsync();
            var dlg = new TransactionEditWindow(_vm.Selected, _vm.Accounts, _vm.Categories) { Owner = Window.GetWindow(this) };
            if (dlg.ShowDialog() == true)
            {
                var dto = dlg.Transaction;
                _logger?.Info($"Updating transaction: {dto.TransactionId}");
                await _vm.UpdateAsync(dto);
                _logger?.Info($"Transaction updated: {dto.TransactionId}");
                await _vm.LoadAsync();
            }
        }
        catch (Exception ex)
        {
            _logger?.Error($"Error updating transaction: {ex}");
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
        try
        {
            if (sender is FrameworkElement fe && fe.DataContext is ViewModels.TransactionListItem item)
            {
                // find underlying TransactionDto
                var dto = _vm.Transactions.FirstOrDefault(t => t.TransactionId == item.TransactionId);
                if (dto == null) return;
                var ok = MessageBox.Show($"Delete transaction #{dto.TransactionId}?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (ok == MessageBoxResult.Yes)
                {
                    _logger?.Info($"Deleting transaction: {dto.TransactionId}");
                    await _vm.DeleteByIdAsync(dto.TransactionId);
                    _logger?.Info($"Transaction deleted: {dto.TransactionId}");
                }
            }
        }
        catch (Exception ex)
        {
            _logger?.Error($"Error deleting transaction: {ex}");
        }
    }

    private async void OnEditItem(object sender, RoutedEventArgs e)
    {
        try
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
                    _logger?.Info($"Updating transaction from item: {updated.TransactionId}");
                    await _vm.UpdateAsync(updated);
                    _logger?.Info($"Transaction updated: {updated.TransactionId}");
                    await _vm.LoadAsync();
                }
            }
        }
        catch (Exception ex)
        {
            _logger?.Error($"Error editing transaction: {ex}");
        }
    }
}