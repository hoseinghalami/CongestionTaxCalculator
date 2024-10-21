using Domain.Interfaces;

using Infrastructure.DbContexts;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TaxExemptVehicleRepository(TaxDBContext dbContext) : ITaxExemptVehicleRepository
{
    public async Task<bool> IsTaxExemptVehicle(long cityId, string vehicleType)
    {
        return await dbContext.TaxExemptVehicles.AnyAsync
            (x => x.CityTaxRuleId == cityId & x.VehicleType.ToLower() == vehicleType.ToLower());
    }
}
