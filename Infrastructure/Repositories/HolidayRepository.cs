using Domain.Interfaces;

using Infrastructure.DbContexts;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class HolidayRepository(TaxDBContext dbContext) : IHolidayRepository
{
    public async Task<bool> IsHoliday(long cityId, DateOnly passDate)
    {
        return await dbContext.Holidays.AnyAsync
            (x => x.CityTaxRuleId == cityId & x.Date == passDate);
    }
}
