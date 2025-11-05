using System.Windows;
using System.Windows.Controls;
using FinanceManager.UI.ViewModels;
using System.Linq;

namespace FinanceManager.UI.Views;

public partial class DashboardView : UserControl
{
    private readonly DashboardViewModel _vm;
    private readonly FinanceManager.BLL.Services.IAccountService _accountService;
    private readonly FinanceManager.BLL.Services.ICategoryService _categoryService;
    private readonly FinanceManager.BLL.Services.ITransactionService _transactionService;

    public DashboardView(DashboardViewModel vm, FinanceManager.BLL.Services.IAccountService accountService, FinanceManager.BLL.Services.ICategoryService categoryService, FinanceManager.BLL.Services.ITransactionService transactionService)
    {
        InitializeComponent();
        _vm = vm;
        _accountService = accountService;
            _categoryService = categoryService;
            _transactionService = transactionService;

        DataContext = _vm;
        Loaded += async (s, e) => await _vm.LoadAsync();
    }

    private async void AddIncome_Click(object sender, RoutedEventArgs e)
    {
        await _vm.LoadAsync();
        var accounts = await _accountService.GetAllAsync();
        var categories = (await _categoryService.GetAllAsync()).Where(c => c.Type.ToString().ToLower().Contains("income"));

        var dlg = new TransactionEditWindow(accounts, categories) { Owner = Window.GetWindow(this) };
        if (dlg.ShowDialog() == true)
        {
            var dto = dlg.Transaction;
            await _vm.CreateTransactionAsync(dto);
            await _vm.LoadAsync();
        }
    }

    private async void AddExpense_Click(object sender, RoutedEventArgs e)
    {
        await _vm.LoadAsync();
        var accounts = await _accountService.GetAllAsync();
    var categories = (await _categoryService.GetAllAsync()).Where(c => c.Type.ToString().ToLower().Contains("expense"));

        var dlg = new TransactionEditWindow(accounts, categories) { Owner = Window.GetWindow(this) };
        if (dlg.ShowDialog() == true)
        {
            var dto = dlg.Transaction;
            await _vm.CreateTransactionAsync(dto);
            await _vm.LoadAsync();
        }
    }

    private void ViewTransactions_Click(object sender, RoutedEventArgs e)
    {
        // Navigate: find parent window and set content to TransactionsView if available
        if (Window.GetWindow(this) is FinanceManager.UI.MainWindow mw)
        {
            // try to access the private _transactionsView field on MainWindow via reflection
            try
            {
                var fi = typeof(FinanceManager.UI.MainWindow).GetField("_transactionsView", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                if (fi != null)
                {
                    var tv = fi.GetValue(mw) as System.Windows.Controls.UserControl;
                    if (tv != null)
                    {
                        mw.ContentArea.Content = tv;
                        return;
                    }
                }

                // Fallback: invoke the private NavTransactions_Click method on MainWindow via reflection
                var mi = typeof(FinanceManager.UI.MainWindow).GetMethod("NavTransactions_Click", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                if (mi != null)
                {
                    mi.Invoke(mw, new object[] { mw, new System.Windows.RoutedEventArgs() });
                    return;
                }

                MessageBox.Show("Unable to navigate to Transactions page.");
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Unable to navigate to Transactions: " + ex.Message);
            }
        }
        else
        {
            MessageBox.Show("View Transactions - navigate in-app to Transactions page.");
        }
    }
}
