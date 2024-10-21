﻿using Domain.Entities;

namespace Domain.Interfaces;

public interface ICityTaxRuleRepository
{
    Task<CityTaxRule?> GetCityByName(string cityName);

    Task<CityTaxRule?> GetCityById(long cityId);
}
