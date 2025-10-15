
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FinanceManager.Enums;

namespace FinanceManager.Models;

public class Category
{
    [Key]
    public int CategoryId { get; set; }

    [Required]
    public int ProfileId { get; set; }
    [ForeignKey("ProfileId")]
    public FinancialProfile FinancialProfile { get; set; }

    [Required]
    [StringLength(255)]
    public string Name { get; set; }

    [Required]
    public CategoryType Type { get; set; }

    [StringLength(255)]
    public string Icon { get; set; }

    [StringLength(7)]
    public string ColorHex { get; set; }
}
