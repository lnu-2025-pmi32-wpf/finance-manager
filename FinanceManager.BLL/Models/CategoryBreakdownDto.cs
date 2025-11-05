namespace FinanceManager.BLL.Models;

public class CategoryBreakdownDto
{
    public int CategoryId { get; set; }
    public string Name { get; set; }
    public decimal Amount { get; set; }
    public string Type { get; set; }
}
