namespace Domain.Interfaces;

public interface ITaxAmountRepository
{
    Task<long> GetTaxAmount(long cityId, TimeOnly time);
}
