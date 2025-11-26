namespace FinanceManager.UI.Views
{
    // <copyright file="TransactionEditWindow.xaml.cs" company="LNU">
    // Copyright (c) LNU. All rights reserved.
    // </copyright>

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Windows;
    using FinanceManager.BLL.Models;

    public partial class TransactionEditWindow : Window
    {
        public TransactionDto Transaction { get; private set; }

        public TransactionEditWindow(IEnumerable<AccountDto> accounts, IEnumerable<CategoryDto> categories)
        {
            InitializeComponent();
            this.CmbAccount.ItemsSource = accounts;
            this.CmbCategory.ItemsSource = categories;
            this.Transaction = new TransactionDto { TransactionDateTime = DateTime.UtcNow };
            this.DatePicker.SelectedDate = DateTime.UtcNow.Date;
            this.TxtHour.Text = DateTime.UtcNow.Hour.ToString("D2");
            this.TxtMinute.Text = DateTime.UtcNow.Minute.ToString("D2");
            this.BtnOk.Click += BtnOk_Click;
            this.BtnCancel.Click += (_, __) => DialogResult = false;
        }

        public TransactionEditWindow(TransactionDto existing, IEnumerable<AccountDto> accounts, IEnumerable<CategoryDto> categories) : this(accounts, categories)
        {
            this.Transaction = new TransactionDto
            {
                TransactionId = existing.TransactionId,
                AccountId = existing.AccountId,
                CategoryId = existing.CategoryId,
                Amount = existing.Amount,
                TransactionDateTime = existing.TransactionDateTime,
                Description = existing.Description
            };

            this.CmbAccount.SelectedItem = accounts.FirstOrDefault(a => a.AccountId == existing.AccountId);
            this.CmbCategory.SelectedItem = categories.FirstOrDefault(c => c.CategoryId == existing.CategoryId);
            this.TxtAmount.Text = existing.Amount.ToString(CultureInfo.InvariantCulture);
            this.DatePicker.SelectedDate = existing.TransactionDateTime.Date;
            this.TxtHour.Text = existing.TransactionDateTime.Hour.ToString("D2");
            this.TxtMinute.Text = existing.TransactionDateTime.Minute.ToString("D2");
            this.TxtDescription.Text = existing.Description;
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            if (this.CmbAccount.SelectedItem is AccountDto acc)
            {
                this.Transaction.AccountId = acc.AccountId;
            }

            if (this.CmbCategory.SelectedItem is CategoryDto cat)
            {
                this.Transaction.CategoryId = cat.CategoryId;
            }

            if (decimal.TryParse(this.TxtAmount.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out var amount))
            {
                this.Transaction.Amount = amount;
            }

            // Combine date + time inputs. Time format expected HH:mm (24h). If time missing or invalid, use current UTC time's hour/min.
            if (this.DatePicker.SelectedDate.HasValue)
            {
                var date = this.DatePicker.SelectedDate.Value.Date;
                // Read hours/minutes from inputs and validate ranges
                if (!int.TryParse((this.TxtHour.Text ?? string.Empty).Trim(), out var hour) || hour < 0 || hour > 23)
                {
                    MessageBox.Show("Please enter a valid hour (0-23).", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!int.TryParse((this.TxtMinute.Text ?? string.Empty).Trim(), out var minute) || minute < 0 || minute > 59)
                {
                    MessageBox.Show("Please enter a valid minute (0-59).", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var combined = new DateTime(date.Year, date.Month, date.Day, hour, minute, 0);
                this.Transaction.TransactionDateTime = DateTime.SpecifyKind(combined, DateTimeKind.Utc);
            }

            this.Transaction.Description = this.TxtDescription.Text?.Trim();

            if (!this.Transaction.AccountId.HasValue)
            {
                MessageBox.Show("Account is required", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DialogResult = true;
        }
    }
}
