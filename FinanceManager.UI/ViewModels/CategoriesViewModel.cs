using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FinanceManager.BLL.Models;
using FinanceManager.BLL.Services;

namespace FinanceManager.UI.ViewModels;

public class CategoriesViewModel : BaseViewModel
{
    private readonly ICategoryService _categoryService;

    public ObservableCollection<CategoryDto> Categories { get; } = new ObservableCollection<CategoryDto>();

    private CategoryDto? _selected;
    public CategoryDto? Selected
    {
        get => _selected;
        set { _selected = value; Raise(); }
    }

    public RelayCommand RefreshCommand { get; }
    public RelayCommand DeleteCommand { get; }

    public CategoriesViewModel(ICategoryService categoryService)
    {
        _categoryService = categoryService;
        RefreshCommand = new RelayCommand(async _ => await LoadAsync());
        DeleteCommand = new RelayCommand(async _ => await DeleteSelected(), _ => Selected != null);
    }

    public async Task LoadAsync()
    {
        Categories.Clear();
        var list = await _categoryService.GetAllAsync();
        foreach (var c in list)
            Categories.Add(c);
        Raise(nameof(Categories));
    }

    public async Task<CategoryDto> CreateAsync(CategoryDto dto)
    {
        return await _categoryService.CreateAsync(dto);
    }

    public async Task<bool> UpdateAsync(CategoryDto dto)
    {
        return await _categoryService.UpdateAsync(dto);
    }

    public async Task DeleteSelected()
    {
        if (Selected == null) return;
        await _categoryService.DeleteAsync(Selected.CategoryId);
        await LoadAsync();
    }
}
