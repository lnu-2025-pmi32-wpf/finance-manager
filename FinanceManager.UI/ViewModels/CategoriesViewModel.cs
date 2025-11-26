// <copyright file="CategoriesViewModel.cs" company="LNU">
// Copyright (c) LNU. All rights reserved.
// </copyright>

namespace FinanceManager.UI.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using FinanceManager.BLL.Models;
    using FinanceManager.BLL.Services;

    public class CategoriesViewModel : BaseViewModel
    {
        private readonly ICategoryService _categoryService;

        public ObservableCollection<CategoryDto> Categories { get; } = new ObservableCollection<CategoryDto>();

        private CategoryDto? _selected;
        public CategoryDto? Selected
        {
            get => _selected;
            set { _selected = value; this.Raise(); }
        }

        public RelayCommand RefreshCommand { get; }
        public RelayCommand DeleteCommand { get; }

        public CategoriesViewModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
            this.RefreshCommand = new RelayCommand(async _ => await this.LoadAsync());
            this.DeleteCommand = new RelayCommand(async _ => await this.DeleteSelected(), _ => this.Selected != null);
        }

        public async Task LoadAsync()
        {
            this.Categories.Clear();
            var list = await _categoryService.GetAllAsync();
            foreach (var c in list)
                this.Categories.Add(c);
            this.Raise(nameof(Categories));
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
            if (this.Selected == null) return;
            await _categoryService.DeleteAsync(this.Selected.CategoryId);
            await this.LoadAsync();
        }
    }
}
