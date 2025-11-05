using System.Collections.Generic;
using FinanceManager.Models;

namespace FinanceManager.BLL.Services;

public interface IFinancialProfileService
{
    IEnumerable<FinancialProfile> GetAllProfiles();
}
