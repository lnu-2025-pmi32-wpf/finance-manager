namespace FinanceManager.BLL.Models;

public class AnalyticsSummaryDto
{
    public decimal NetBalance { get; set; }
    public decimal TotalIncome { get; set; }
    public decimal TotalExpense { get; set; }
    public int SavingRatePercent { get; set; }
    public string TopIncomeSource { get; set; }
    public decimal TopIncomeAmount { get; set; }
    public string LargestExpenseCategory { get; set; }
    public decimal LargestExpenseAmount { get; set; }
}
