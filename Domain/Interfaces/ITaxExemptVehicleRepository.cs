namespace Domain.Interfaces;

public interface ITaxExemptVehicleRepository
{
    Task<bool> IsTaxExemptVehicle(long cityId, string vehicleType);
}
