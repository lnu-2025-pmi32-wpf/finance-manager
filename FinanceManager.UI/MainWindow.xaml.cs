using System;
using System.Linq;
using System.Windows;
using FinanceManager.BLL.Services;
using FinanceManager.Models;

namespace FinanceManager.UI;

public partial class MainWindow : Window
{
    private readonly IFinancialProfileService _profileService;

    public MainWindow(IFinancialProfileService profileService)
    {
        InitializeComponent();
        _profileService = profileService;

        LoadProfiles();
    }

    private void LoadProfiles()
    {
        var profiles = _profileService.GetAllProfiles().Select(p => p.Name).ToList();
        ProfilesListBox.ItemsSource = profiles;
    }

    private void MenuExit_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    private void MenuAbout_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Finance Manager - WPF UI", "About");
    }

    private void AddProfile_Click(object sender, RoutedEventArgs e)
    {
        var name = ProfileNameTextBox.Text?.Trim();
        if (string.IsNullOrWhiteSpace(name))
        {
            MessageBox.Show("Enter a profile name.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        MessageBox.Show($"(Demo) Would add profile: {name}");
    }

    private void EditProfile_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("(Demo) Edit profile");
    }

    private void DeleteProfile_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("(Demo) Delete profile");
    }
}
