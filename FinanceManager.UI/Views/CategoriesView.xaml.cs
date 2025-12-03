namespace FinanceManager.UI.Views
{
    // <copyright file="CategoriesView.xaml.cs" company="LNU">
    // Copyright (c) LNU. All rights reserved.
    // </copyright>

    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Animation;
    using FinanceManager.UI.ViewModels;
    using FinanceManager.UI.Services;

    public partial class CategoriesView : UserControl
    {
        private readonly CategoriesViewModel vm;
        private readonly IAppLogger _logger;

        public CategoriesView(CategoriesViewModel vm, IAppLogger logger)
        {
            InitializeComponent();
            this.vm = vm;
            this._logger = logger;
            this.DataContext = this.vm;

            this.Loaded += async (_, __) =>
            {
                try
                {
                    _logger?.Info("CategoriesView loaded");
                    if (FindResource("EnterAnimation") is Storyboard sb)
                    {
                        sb.Begin(this);
                        _logger?.Trace("CategoriesView enter animation started");
                    }

                    await this.vm.LoadAsync();
                }
                catch (System.Exception ex)
                {
                    _logger?.Error($"CategoriesView load error: {ex}");
                }
            };

            this.BtnAdd.Click += BtnAdd_Click;
            this.BtnEdit.Click += BtnEdit_Click;
            this.BtnDelete.Click += async (_, __) =>
            {
                _logger?.Info("Deleting selected category");
                await this.vm.DeleteSelected();
                _logger?.Info("Category delete requested");
            };
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new CategoryEditWindow();
            if (dlg.ShowDialog() == true)
            {
                var dto = dlg.Category;
                _logger?.Info($"Creating category: {dto.Name}");
                _ = this.vm.CreateAsync(dto).ContinueWith(async t => await this.vm.LoadAsync());
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (this.vm.Selected == null) return;
            var dlg = new CategoryEditWindow(this.vm.Selected);
            if (dlg.ShowDialog() == true)
            {
                var dto = dlg.Category;
                _logger?.Info($"Updating category: {dto.Name}");
                _ = this.vm.UpdateAsync(dto).ContinueWith(async t => await this.vm.LoadAsync());
            }
        }
    }
}
