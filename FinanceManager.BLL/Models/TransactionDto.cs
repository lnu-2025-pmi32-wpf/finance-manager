using System;

namespace FinanceManager.BLL.Models;

public class TransactionDto
{
    public int TransactionId { get; set; }
    public int? AccountId { get; set; }
    public int? CategoryId { get; set; }
    public decimal Amount { get; set; }
    public DateTime TransactionDateTime { get; set; }
    public string? Description { get; set; }
}

public class TransactionQuery
{
    public string? Search { get; set; }
    public string? Type { get; set; } // "income" or "expense"
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
}
