using Domain.Interfaces;

using Infrastructure.DbContexts;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TaxAmountRepository(TaxDBContext dbContext) : ITaxAmountRepository
{
    public async Task<long> GetTaxAmount(long cityId, TimeOnly time)
    {
        var taxAmount = await dbContext.TaxAmounts.Where
            (x => x.CityTaxRuleId == cityId & x.StartTime <= time && x.EndTime >= time)
            .FirstOrDefaultAsync();

        return taxAmount.Amount;
    }
}
