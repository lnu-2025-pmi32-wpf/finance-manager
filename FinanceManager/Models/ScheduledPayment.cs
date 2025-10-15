
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FinanceManager.Enums;

namespace FinanceManager.Models;

public class ScheduledPayment
{
    [Key]
    public int PaymentId { get; set; }

    [Required]
    public int ProfileId { get; set; }
    [ForeignKey("ProfileId")]
    public FinancialProfile FinancialProfile { get; set; }

    [Required]
    public int AccountId { get; set; }
    [ForeignKey("AccountId")]
    public Account Account { get; set; }

    public int? CategoryId { get; set; }
    [ForeignKey("CategoryId")]
    public Category Category { get; set; }

    [Required]
    public decimal Amount { get; set; }

    [StringLength(255)]
    public string Description { get; set; }

    [Required]
    public ScheduledPaymentFrequency Frequency { get; set; }

    [Required]
    public DateTime NextExecutionDate { get; set; }
}
