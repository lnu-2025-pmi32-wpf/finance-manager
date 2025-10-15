
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceManager.Models;

public class FinancialProfile
{
    [Key]
    public int ProfileId { get; set; }

    [Required]
    public int UserId { get; set; }
    [ForeignKey("UserId")]
    public User User { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Required]
    [StringLength(3)]
    public string MainCurrencyCode { get; set; }
    [ForeignKey("MainCurrencyCode")]
    public Currency MainCurrency { get; set; }

    public ICollection<Account> Accounts { get; set; } = new List<Account>();
    public ICollection<Category> Categories { get; set; } = new List<Category>();
    public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    public ICollection<Goal> Goals { get; set; } = new List<Goal>();
    public ICollection<ScheduledPayment> ScheduledPayments { get; set; } = new List<ScheduledPayment>();
}
