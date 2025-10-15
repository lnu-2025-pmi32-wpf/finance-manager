
using System.ComponentModel.DataAnnotations;

namespace FinanceManager.Models;

public class Currency
{
    [Key]
    [StringLength(3)]
    public string CurrencyCode { get; set; }

    [Required]
    public decimal UsdExchangeRate { get; set; }
}
