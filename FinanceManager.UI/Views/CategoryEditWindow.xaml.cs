using System;
using System.Linq;
using System.Windows;
using FinanceManager.BLL.Models;
using FinanceManager.Enums;

namespace FinanceManager.UI.Views;

public partial class CategoryEditWindow : Window
{
    public CategoryDto Category { get; private set; }

    public CategoryEditWindow()
    {
        InitializeComponent();
        Category = new CategoryDto { ProfileId = 1, Type = CategoryType.Expense };
        CmbType.ItemsSource = Enum.GetValues(typeof(CategoryType)).Cast<CategoryType>();
        CmbType.SelectedItem = Category.Type;

        BtnOk.Click += BtnOk_Click;
        BtnCancel.Click += (_, __) => DialogResult = false;
    }

    public CategoryEditWindow(CategoryDto existing) : this()
    {
        Category = new CategoryDto
        {
            CategoryId = existing.CategoryId,
            ProfileId = existing.ProfileId,
            Name = existing.Name,
            Type = existing.Type,
            Icon = existing.Icon,
            ColorHex = existing.ColorHex
        };
        TxtName.Text = Category.Name;
        TxtColor.Text = Category.ColorHex;
        CmbType.SelectedItem = Category.Type;
    }

    private void BtnOk_Click(object sender, RoutedEventArgs e)
    {
        Category.Name = TxtName.Text?.Trim() ?? string.Empty;
        Category.ColorHex = TxtColor.Text?.Trim();
        if (CmbType.SelectedItem is CategoryType ct)
            Category.Type = ct;

        if (string.IsNullOrWhiteSpace(Category.Name))
        {
            MessageBox.Show("Name is required", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        DialogResult = true;
    }
}
