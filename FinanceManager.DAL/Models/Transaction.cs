using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceManager.Models;

public class Transaction
{
    [Key]
    public int TransactionId { get; set; }

    [Required]
    public int AccountId { get; set; }
    [ForeignKey("AccountId")]
    public Account Account { get; set; }

    public int? CategoryId { get; set; }
    [ForeignKey("CategoryId")]
    public Category Category { get; set; }

    [Required]
    public decimal Amount { get; set; }

    [Required]
    public DateTime TransactionDateTime { get; set; }

    public string Description { get; set; }

    public ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
