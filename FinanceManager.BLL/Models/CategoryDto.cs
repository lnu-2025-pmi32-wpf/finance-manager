using FinanceManager.Enums;

namespace FinanceManager.BLL.Models;

public class CategoryDto
{
    public int CategoryId { get; set; }
    public int ProfileId { get; set; }
    public string Name { get; set; }
    public CategoryType Type { get; set; }
    public string? Icon { get; set; }
    public string? ColorHex { get; set; }
}
