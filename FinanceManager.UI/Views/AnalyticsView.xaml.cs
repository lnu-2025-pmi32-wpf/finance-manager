using System.Windows.Controls;
using System.Threading.Tasks;
using FinanceManager.UI.ViewModels;

namespace FinanceManager.UI.Views;

public partial class AnalyticsView : UserControl
{
    private readonly AnalyticsViewModel _vm;

    public AnalyticsView(AnalyticsViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        DataContext = _vm;

        // load data async after control created
        Loaded += async (s, e) => await EnsureLoaded();
    }

    private async Task EnsureLoaded()
    {
        await _vm.LoadAsync();
    }
}
