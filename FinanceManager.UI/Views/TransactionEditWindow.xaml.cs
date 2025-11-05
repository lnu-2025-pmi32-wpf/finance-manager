using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using FinanceManager.BLL.Models;

namespace FinanceManager.UI.Views;

public partial class TransactionEditWindow : Window
{
    public TransactionDto Transaction { get; private set; }

    public TransactionEditWindow(IEnumerable<AccountDto> accounts, IEnumerable<CategoryDto> categories)
    {
        InitializeComponent();
        CmbAccount.ItemsSource = accounts;
        CmbCategory.ItemsSource = categories;
        Transaction = new TransactionDto { TransactionDateTime = DateTime.UtcNow };
    DatePicker.SelectedDate = DateTime.UtcNow.Date;
    TxtHour.Text = DateTime.UtcNow.Hour.ToString("D2");
    TxtMinute.Text = DateTime.UtcNow.Minute.ToString("D2");
        BtnOk.Click += BtnOk_Click;
        BtnCancel.Click += (_, __) => DialogResult = false;
    }

    public TransactionEditWindow(TransactionDto existing, IEnumerable<AccountDto> accounts, IEnumerable<CategoryDto> categories) : this(accounts, categories)
    {
        Transaction = new TransactionDto
        {
            TransactionId = existing.TransactionId,
            AccountId = existing.AccountId,
            CategoryId = existing.CategoryId,
            Amount = existing.Amount,
            TransactionDateTime = existing.TransactionDateTime,
            Description = existing.Description
        };

        CmbAccount.SelectedItem = accounts.FirstOrDefault(a => a.AccountId == existing.AccountId);
        CmbCategory.SelectedItem = categories.FirstOrDefault(c => c.CategoryId == existing.CategoryId);
    TxtAmount.Text = existing.Amount.ToString(CultureInfo.InvariantCulture);
    DatePicker.SelectedDate = existing.TransactionDateTime.Date;
    TxtHour.Text = existing.TransactionDateTime.Hour.ToString("D2");
    TxtMinute.Text = existing.TransactionDateTime.Minute.ToString("D2");
    TxtDescription.Text = existing.Description;
    }

    private void BtnOk_Click(object sender, RoutedEventArgs e)
    {
        if (CmbAccount.SelectedItem is AccountDto acc)
            Transaction.AccountId = acc.AccountId;

        if (CmbCategory.SelectedItem is CategoryDto cat)
            Transaction.CategoryId = cat.CategoryId;

        if (decimal.TryParse(TxtAmount.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out var amount))
            Transaction.Amount = amount;

        // Combine date + time inputs. Time format expected HH:mm (24h). If time missing or invalid, use current UTC time's hour/min.
        if (DatePicker.SelectedDate.HasValue)
        {
            var date = DatePicker.SelectedDate.Value.Date;
            // Read hours/minutes from inputs and validate ranges
            if (!int.TryParse((TxtHour.Text ?? string.Empty).Trim(), out var hour) || hour < 0 || hour > 23)
            {
                MessageBox.Show("Please enter a valid hour (0-23).", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!int.TryParse((TxtMinute.Text ?? string.Empty).Trim(), out var minute) || minute < 0 || minute > 59)
            {
                MessageBox.Show("Please enter a valid minute (0-59).", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var combined = new DateTime(date.Year, date.Month, date.Day, hour, minute, 0);
            Transaction.TransactionDateTime = DateTime.SpecifyKind(combined, DateTimeKind.Utc);
        }

        Transaction.Description = TxtDescription.Text?.Trim();

        if (!Transaction.AccountId.HasValue)
        {
            MessageBox.Show("Account is required", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        DialogResult = true;
    }
}
