namespace FinanceManager.UI.Views
{
    // <copyright file="CategoryEditWindow.xaml.cs" company="LNU">
    // Copyright (c) LNU. All rights reserved.
    // </copyright>

    using System;
    using System.Linq;
    using System.Windows;
    using FinanceManager.BLL.Models;
    using FinanceManager.Enums;

    public partial class CategoryEditWindow : Window
    {
        public CategoryDto Category { get; private set; }

        public CategoryEditWindow()
        {
            InitializeComponent();
            this.Category = new CategoryDto { ProfileId = 1, Type = CategoryType.Expense };
            this.CmbType.ItemsSource = Enum.GetValues(typeof(CategoryType)).Cast<CategoryType>();
            this.CmbType.SelectedItem = this.Category.Type;

            this.BtnOk.Click += BtnOk_Click;
            this.BtnCancel.Click += (_, __) => DialogResult = false;
        }

        public CategoryEditWindow(CategoryDto existing) : this()
        {
            this.Category = new CategoryDto
            {
                CategoryId = existing.CategoryId,
                ProfileId = existing.ProfileId,
                Name = existing.Name,
                Type = existing.Type,
                Icon = existing.Icon,
                ColorHex = existing.ColorHex
            };
            this.TxtName.Text = this.Category.Name;
            this.TxtColor.Text = this.Category.ColorHex;
            this.CmbType.SelectedItem = this.Category.Type;
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            this.Category.Name = this.TxtName.Text?.Trim() ?? string.Empty;
            this.Category.ColorHex = this.TxtColor.Text?.Trim();
            if (this.CmbType.SelectedItem is CategoryType ct)
            {
                this.Category.Type = ct;
            }

            if (string.IsNullOrWhiteSpace(this.Category.Name))
            {
                MessageBox.Show("Name is required", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DialogResult = true;
        }
    }
}
