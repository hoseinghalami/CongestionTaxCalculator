using Domain.Entities;
using Domain.Interfaces;

using Infrastructure.DbContexts;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CityTaxRuleRepository(TaxDBContext dbContext) : ICityTaxRuleRepository
{
    public async Task<CityTaxRule?> GetCityByName(string cityName)
    {
        return await dbContext.CityTaxRules.Where(x => x.CityName.ToLower() == cityName.ToLower())
            .FirstOrDefaultAsync();
    }

    public async Task<CityTaxRule?> GetCityById(long cityId)
    {
        return await dbContext.CityTaxRules.FindAsync(cityId);
    }
}
