
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceManager.Models;

public class Account
{
    [Key]
    public int AccountId { get; set; }

    [Required]
    public int ProfileId { get; set; }
    [ForeignKey("ProfileId")]
    public FinancialProfile FinancialProfile { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Required]
    public decimal Balance { get; set; }

    [Required]
    [StringLength(3)]
    public string CurrencyCode { get; set; }
    [ForeignKey("CurrencyCode")]
    public Currency Currency { get; set; }

    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
