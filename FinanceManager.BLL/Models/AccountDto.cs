namespace FinanceManager.BLL.Models;

public class AccountDto
{
    public int AccountId { get; set; }
    public int ProfileId { get; set; }
    public string Name { get; set; }
    public decimal Balance { get; set; }
    public string CurrencyCode { get; set; }
}
