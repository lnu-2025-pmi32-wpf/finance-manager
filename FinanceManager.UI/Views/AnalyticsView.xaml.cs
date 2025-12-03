using System.Windows.Controls;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using FinanceManager.UI.ViewModels;
using FinanceManager.UI.Services;

namespace FinanceManager.UI.Views;

public partial class AnalyticsView : UserControl
{
    private readonly AnalyticsViewModel _vm;
    private readonly IAppLogger _logger;

    public AnalyticsView(AnalyticsViewModel vm, IAppLogger logger)
    {
        InitializeComponent();
        _vm = vm;
        _logger = logger;
        DataContext = _vm;

        // load data async after control created
        Loaded += async (s, e) => await EnsureLoaded();
    }

    private async Task EnsureLoaded()
    {
        try
        {
            _logger?.Info("AnalyticsView loaded");
            if (FindResource("EnterAnimation") is Storyboard sb)
            {
                sb.Begin(this);
                _logger?.Trace("AnalyticsView enter animation started");
            }

            await _vm.LoadAsync();
        }
        catch (System.Exception ex)
        {
            _logger?.Error($"AnalyticsView load error: {ex}");
        }
    }
}
