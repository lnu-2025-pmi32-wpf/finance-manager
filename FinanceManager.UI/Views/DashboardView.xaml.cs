namespace FinanceManager.UI.Views
{
    // <copyright file="DashboardView.xaml.cs" company="LNU">
    // Copyright (c) LNU. All rights reserved.
    // </copyright>

    using System.Windows;
    using System.Windows.Controls;
    using FinanceManager.UI.ViewModels;

    public partial class DashboardView : UserControl
    {
        public DashboardView(DashboardViewModel vm)
        {
            InitializeComponent();
            this.DataContext = vm;
            this.Loaded += async (s, e) => await vm.LoadAsync();
        }

        private void AddIncome_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Add Income - not implemented yet");
        }

        private void AddExpense_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Add Expense - not implemented yet");
        }

        private void ViewTransactions_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("View Transactions - not implemented yet");
        }
    }
}
