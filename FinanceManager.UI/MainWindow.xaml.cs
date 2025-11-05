using System;
using System.Windows;
using FinanceManager.UI.Views;

namespace FinanceManager.UI;

public partial class MainWindow : Window
{
    private readonly DashboardView _dashboardView;
    private readonly Views.CategoriesView _categoriesView;
    private readonly Views.AccountsView _accountsView;
    private readonly Views.TransactionsView _transactionsView;
    private readonly Views.AnalyticsView _analyticsView;

    public MainWindow(DashboardView dashboardView, Views.CategoriesView categoriesView, Views.AccountsView accountsView, Views.TransactionsView transactionsView, Views.AnalyticsView analyticsView)
    {
        InitializeComponent();
        _dashboardView = dashboardView;
        _categoriesView = categoriesView;
        _accountsView = accountsView;
        _transactionsView = transactionsView;
        _analyticsView = analyticsView;

        // show dashboard by default
        ContentArea.Content = _dashboardView;
    }

    private void NavDashboard_Click(object sender, RoutedEventArgs e)
    {
        ContentArea.Content = _dashboardView;
    }

    private void NavAccounts_Click(object sender, RoutedEventArgs e)
    {
        ContentArea.Content = _accountsView;
    }

    private void NavCategories_Click(object sender, RoutedEventArgs e)
    {
        ContentArea.Content = _categoriesView;
    }

    private void NavTransactions_Click(object sender, RoutedEventArgs e)
    {
        ContentArea.Content = _transactionsView;
    }

    private void NavProfiles_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Profiles view not implemented yet");
    }

    private void NavAnalytics_Click(object sender, RoutedEventArgs e)
    {
        ContentArea.Content = _analyticsView;
    }
}
