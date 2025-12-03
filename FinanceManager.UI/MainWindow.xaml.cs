namespace FinanceManager.UI
{
    // <copyright file="MainWindow.xaml.cs" company="LNU">
    // Copyright (c) LNU. All rights reserved.
    // </copyright>

    using System;
    using System.Windows;
    using FinanceManager.UI.Views;

    public partial class MainWindow : Window
    {
        private readonly DashboardView dashboardView;
        private readonly Views.CategoriesView categoriesView;
        private readonly Views.AccountsView accountsView;
        private readonly Views.TransactionsView transactionsView;
        private readonly Views.AnalyticsView analyticsView;

        public MainWindow(DashboardView dashboardView, Views.CategoriesView categoriesView, Views.AccountsView accountsView, Views.TransactionsView transactionsView, Views.AnalyticsView analyticsView)
        {
            InitializeComponent();
            this.dashboardView = dashboardView;
            this.categoriesView = categoriesView;
            this.accountsView = accountsView;
            this.transactionsView = transactionsView;
            this.analyticsView = analyticsView;

            // show dashboard by default
            ContentArea.Content = this.dashboardView;
        }

        private void NavDashboard_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = this.dashboardView;
        }

        private void NavAccounts_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = this.accountsView;
        }

        private void NavCategories_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = this.categoriesView;
        }

        private void NavTransactions_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = this.transactionsView;
        }

        private void NavProfiles_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Profiles view not implemented yet");
        }

        private void NavAnalytics_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = this.analyticsView;
        }
    }
}
