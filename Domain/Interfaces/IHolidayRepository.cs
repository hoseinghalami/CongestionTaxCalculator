namespace Domain.Interfaces;

public interface IHolidayRepository
{
    Task<bool> IsHoliday(long cityId, DateOnly passDate);
}
