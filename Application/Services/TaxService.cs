using Application.Dtos;
using Application.Services.Interfaces;

using Domain.Interfaces;

namespace Application.Services;

public class TaxService(ICityTaxRuleRepository cityTaxRuleRepository,
    IHolidayRepository holidayRepository,
    ITaxAmountRepository taxAmountRepository,
    ITaxExemptVehicleRepository taxExemptVehicleRepository) : ITaxService
{
    public async Task<long> CalculateTax(TaxCalculationDto dto)
    {

        var city = await cityTaxRuleRepository.GetCityByName(dto.CityName);

        if (city is null)
            return -1;

        if (await taxExemptVehicleRepository.IsTaxExemptVehicle(city.Id, dto.VehicleType))
            return 0;

        dto.PassesDates = dto.PassesDates.OrderBy(date => date).ToArray();

        DateOnly passDate = DateOnly.FromDateTime(dto.PassesDates[0]);
        if (await IsTaxExemptDate(city.Id, passDate))
            return 0;

        long totalFee = 0;

        if (city.SingleChargeDurationMinutes is 0)
        {
            foreach (DateTime date in dto.PassesDates)
            {
                TimeOnly time = TimeOnly.FromDateTime(date);
                totalFee += await taxAmountRepository.GetTaxAmount(city.Id, time);
            }
        }
        else
        {
            long currentMaxFee = 0;
            DateTime intervalStart = dto.PassesDates[0];

            foreach (DateTime date in dto.PassesDates)
            {
                TimeOnly time = TimeOnly.FromDateTime(date);
                long nextFee = await taxAmountRepository.GetTaxAmount(city.Id, time);

                TimeSpan span = date.Subtract(intervalStart);
                double minutesDiff = span.TotalMinutes;

                if (minutesDiff <= city.SingleChargeDurationMinutes)
                {

                    if (nextFee > currentMaxFee)
                    {
                        currentMaxFee = nextFee;
                    }
                }
                else
                {
                    totalFee += currentMaxFee;

                    intervalStart = date;
                    currentMaxFee = nextFee;
                }
            }

            totalFee += currentMaxFee;
        }

        if (city.MaximumTaxPerDay != 0 && totalFee > city.MaximumTaxPerDay)
            return city.MaximumTaxPerDay;

        return totalFee;
    }

    public async Task<bool> IsTaxExemptDate(long cityId, DateOnly date)
    {
        var city = await cityTaxRuleRepository.GetCityById(cityId);

        if (city.IsHolidayTaxExempt)
        {
            if (await holidayRepository.IsHoliday(cityId, date))
                return true;
        }

        if (city.IsDayBeforeHolidayTaxExempt)
        {
            DateOnly tomorrow = date.AddDays(1);
            if (await holidayRepository.IsHoliday(cityId, tomorrow))
                return true;
        }

        if (city.IsJulyTaxExempt)
        {
            if (date.Month == 7)
                return true;
        }

        if (city.IsWeekendTaxExempt)
        {
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            {
                return true;
            }
        }

        return false;
    }
}