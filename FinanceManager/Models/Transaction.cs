using System;
using System.ComponentModel.DataAnnotations;
using FinanceManager.Enums;

namespace FinanceManager.Models;

public class Transaction
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Description { get; set; }

    [Required]
    public decimal Amount { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [Required]
    public TransactionType Type { get; set; }
}
