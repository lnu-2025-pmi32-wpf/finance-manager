namespace FinanceManager.BLL.Models;

public class GoalProgressDto
{
    public int GoalId { get; set; }
    public string Name { get; set; }
    public decimal CurrentAmount { get; set; }
    public decimal TargetAmount { get; set; }
    public int Percent => TargetAmount <= 0 ? 0 : (int)System.Math.Min(100, System.Convert.ToInt32((CurrentAmount / TargetAmount) * 100));
}
