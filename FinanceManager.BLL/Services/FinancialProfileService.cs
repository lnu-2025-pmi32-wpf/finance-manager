using System.Collections.Generic;
using System.Linq;
using FinanceManager.Data;
using FinanceManager.Models;

namespace FinanceManager.BLL.Services;

public class FinancialProfileService : IFinancialProfileService
{
    private readonly AppDbContext _db;

    public FinancialProfileService(AppDbContext db)
    {
        _db = db;
    }

    public IEnumerable<FinancialProfile> GetAllProfiles()
    {
        return _db.FinancialProfiles.ToList();
    }
}
