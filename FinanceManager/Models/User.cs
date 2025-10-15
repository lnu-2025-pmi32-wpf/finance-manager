
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinanceManager.Models;

public class User
{
    [Key]
    public int UserId { get; set; }

    [Required]
    [StringLength(255)]
    public string Email { get; set; }

    [Required]
    [StringLength(255)]
    public string PasswordHash { get; set; }

    public ICollection<FinancialProfile> FinancialProfiles { get; set; } = new List<FinancialProfile>();
}
