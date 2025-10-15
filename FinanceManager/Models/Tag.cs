
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceManager.Models;

public class Tag
{
    [Key]
    public int TagId { get; set; }

    [Required]
    public int ProfileId { get; set; }
    [ForeignKey("ProfileId")]
    public FinancialProfile FinancialProfile { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
