namespace FinanceManager.UI.Views
{
    // <copyright file="DashboardView.xaml.cs" company="LNU">
    // Copyright (c) LNU. All rights reserved.
    // </copyright>

    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Animation;
    using FinanceManager.UI.ViewModels;
    using FinanceManager.UI.Services;

    public partial class DashboardView : UserControl
    {
        private readonly IAppLogger _logger;

        public DashboardView(DashboardViewModel vm, IAppLogger logger)
        {
            InitializeComponent();
            this.DataContext = vm;
            this._logger = logger;

            this.Loaded += async (s, e) =>
            {
                try
                {
                    _logger?.Info("DashboardView loaded");
                    if (FindResource("EnterAnimation") is Storyboard sb)
                    {
                        sb.Begin(this);
                        _logger?.Trace("DashboardView enter animation started");
                    }

                    await vm.LoadAsync();
                }
                catch (System.Exception ex)
                {
                    _logger?.Error($"DashboardView load error: {ex}");
                }
            };
        }

        private void AddIncome_Click(object sender, RoutedEventArgs e)
        {
            _logger?.Info("Quick action: Add Income clicked");
            MessageBox.Show("Add Income - not implemented yet");
        }

        private void AddExpense_Click(object sender, RoutedEventArgs e)
        {
            _logger?.Info("Quick action: Add Expense clicked");
            MessageBox.Show("Add Expense - not implemented yet");
        }

        private void ViewTransactions_Click(object sender, RoutedEventArgs e)
        {
            _logger?.Info("Quick action: View Transactions clicked");
            MessageBox.Show("View Transactions - not implemented yet");
        }
    }
}
