using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FinanceManager.Data;
using FinanceManager.BLL.Models;
using FinanceManager.Enums;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.BLL.Services;

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _db;
    public CategoryService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<CategoryBreakdownDto>> GetCategoryBreakdownAsync()
    {
        var list = await _db.Categories
            .Select(c => new CategoryBreakdownDto
            {
                CategoryId = c.CategoryId,
                Name = c.Name,
                Type = c.Type.ToString(),
                Amount = 0m
            })
            .ToListAsync();

        // Sum transactions per category
        var sums = await _db.Transactions
            .GroupBy(t => t.CategoryId)
            .Select(g => new { CategoryId = g.Key, Sum = g.Sum(t => (decimal?)t.Amount) ?? 0m })
            .ToListAsync();

        foreach (var s in sums)
        {
            var item = list.FirstOrDefault(x => x.CategoryId == s.CategoryId);
            if (item != null)
                item.Amount = s.Sum;
        }

        return list;
    }

    public async Task<IEnumerable<CategoryDto>> GetAllAsync()
    {
        return await _db.Categories
            .Select(c => new CategoryDto
            {
                CategoryId = c.CategoryId,
                ProfileId = c.ProfileId,
                Name = c.Name,
                Type = c.Type,
                Icon = c.Icon,
                ColorHex = c.ColorHex
            })
            .ToListAsync();
    }

    public async Task<CategoryDto?> GetByIdAsync(int id)
    {
        var c = await _db.Categories.FindAsync(id);
        if (c == null) return null;
        return new CategoryDto
        {
            CategoryId = c.CategoryId,
            ProfileId = c.ProfileId,
            Name = c.Name,
            Type = c.Type,
            Icon = c.Icon,
            ColorHex = c.ColorHex
        };
    }

    public async Task<CategoryDto> CreateAsync(CategoryDto dto)
    {
        // basic validation
        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ArgumentException("Category name is required");

        var entity = new FinanceManager.Models.Category
        {
            ProfileId = dto.ProfileId,
            Name = dto.Name,
            Type = dto.Type,
            Icon = dto.Icon,
            ColorHex = dto.ColorHex
        };

        _db.Categories.Add(entity);
        await _db.SaveChangesAsync();

        dto.CategoryId = entity.CategoryId;
        return dto;
    }

    public async Task<bool> UpdateAsync(CategoryDto dto)
    {
        var e = await _db.Categories.FindAsync(dto.CategoryId);
        if (e == null) return false;
        e.Name = dto.Name;
        e.Type = dto.Type;
        e.Icon = dto.Icon;
        e.ColorHex = dto.ColorHex;
        e.ProfileId = dto.ProfileId;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var e = await _db.Categories.FindAsync(id);
        if (e == null) return false;
        _db.Categories.Remove(e);
        await _db.SaveChangesAsync();
        return true;
    }
}
