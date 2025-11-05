using System;

namespace FinanceManager.UI.ViewModels;

public class TransactionListItem
{
    public int TransactionId { get; set; }
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public DateTime TransactionDateTime { get; set; }
    public string? CategoryName { get; set; }
    public string? CategoryType { get; set; }
    public string FormattedAmount => Amount >= 0 ? $"+{Amount:C}" : $"{Amount:C}";
    public string AmountColor => Amount >= 0 ? "AccentGreen" : "AccentRed";
}
