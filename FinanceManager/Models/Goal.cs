
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceManager.Models;

public class Goal
{
    [Key]
    public int GoalId { get; set; }

    [Required]
    public int ProfileId { get; set; }
    [ForeignKey("ProfileId")]
    public FinancialProfile FinancialProfile { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Required]
    public decimal TargetAmount { get; set; }

    [Required]
    public decimal CurrentAmount { get; set; }

    public DateTime? TargetDate { get; set; }
}
