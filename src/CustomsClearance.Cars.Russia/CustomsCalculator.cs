using CustomsClearance.Cars.Russia.Enums;
using CustomsClearance.Cars.Russia.Models;

namespace CustomsClearance.Cars.Russia
{
    /// <summary>
    /// Car customs clearance calculator
    /// </summary>
    /// <param name="importerType">Importer Type <see cref="ImporterType"/></param>
    /// <param name="carAge">Car age</param>
    /// <param name="engineType">Car engine type <see cref="EngineType"/></param>
    /// <param name="enginePower">Engine power (horsepower)</param>
    /// <param name="engineVolume">Engine volume (centimeter cubed)</param>
    /// <param name="carPrice">Car price (rubles)</param>
    /// <param name="euroExchangeRate">Euro exchange rate (euro to ruble)</param>
    public class CarCustomsClearance(
        ImporterType importerType,
        int carAge,
        EngineType engineType,
        int enginePower,
        int engineVolume,
        decimal carPrice,
        decimal euroExchangeRate)
    {
        private readonly CarCustomsClearanceCost _carCustomClearanceCost = new();

        /// <summary>
        /// CarCustomsClearance create method
        /// </summary>
        /// <param name="importerType">Importer Type <see cref="ImporterType"/></param>
        /// <param name="carAge">Car age</param>
        /// <param name="engineType">Car engine type <see cref="EngineType"/></param>
        /// <param name="enginePower">Engine power (horsepower)</param>
        /// <param name="engineVolume">Engine volume (centimeter cubed)</param>
        /// <param name="carPrice">Car price (rubles)</param>
        /// <param name="euroExchangeRate">Euro exchange rate (euro to ruble)</param>
        public static CarCustomsClearance Create(
            ImporterType importerType,
            int carAge,
            EngineType engineType,
            int enginePower,
            int engineVolume,
            decimal carPrice,
            decimal euroExchangeRate)
        {
            return new CarCustomsClearance(
                importerType,
                carAge,
                engineType,
                enginePower,
                engineVolume,
                carPrice,
                euroExchangeRate);
        }

        private void CalculateCustomsFee()
        {
            _carCustomClearanceCost.CustomsFee = carPrice switch
            {
                <= 20000 => 1067,
                <= 45000 => 2134,
                <= 1200000 => 4269,
                <= 2700000 => 11746,
                <= 4200000 => 16524,
                <= 5500000 => 21344,
                <= 7000000 => 27540,
                _ => 30000
            };
        }

        private void CalculateCustomsDuty()
        {
            decimal convertedCarPrice = carPrice / euroExchangeRate;

            if (engineType == EngineType.Electric)
            {
                _carCustomClearanceCost.CustomsDuty = carPrice * 0.15M;
            }
            else if (engineType == EngineType.Hybrid)
            {
                //TODO Clarification needed
                _carCustomClearanceCost.CustomsDuty = euroExchangeRate * carAge switch
                {
                    <= 3 => Math.Max(convertedCarPrice * 0.48M, engineVolume * 5.5M),
                    <= 5 => engineVolume * 2.5M,
                    <= 7 => engineVolume * 3.5M,
                    _ => engineVolume * 3.5M
                };
            }
            else
            {
                if (importerType is ImporterType.Individual or ImporterType.PhysicalPersonWithResell)
                {
                    _carCustomClearanceCost.CustomsDuty = carAge <= 3
                        ? euroExchangeRate * convertedCarPrice switch
                        {
                            <= 8500 => Math.Max(convertedCarPrice * 0.54M, engineVolume * 2.5M),
                            <= 16700 => Math.Max(convertedCarPrice * 0.48M, engineVolume * 3.5M),
                            <= 42300 => Math.Max(convertedCarPrice * 0.48M, engineVolume * 5.5M),
                            <= 84500 => Math.Max(convertedCarPrice * 0.48M, engineVolume * 7.5M),
                            <= 169000 => Math.Max(convertedCarPrice * 0.48M, engineVolume * 15M),
                            _ => Math.Max(convertedCarPrice * 0.48M, engineVolume * 20M),
                        }
                        : euroExchangeRate * engineVolume * engineVolume switch
                        {
                            <= 1000 => carAge is > 3 and <= 5 ? 1.5M : 3M,
                            <= 1500 => carAge is > 3 and <= 5 ? 1.7M : 3.2M,
                            <= 1800 => carAge is > 3 and <= 5 ? 2.5M : 3.5M,
                            <= 2300 => carAge is > 3 and <= 5 ? 2.7M : 4.8M,
                            <= 3000 => carAge is > 3 and <= 5 ? 3M : 5M,
                            _ => carAge is > 3 and <= 5 ? 3.6M : 5.7M,
                        };
                }
                else
                {
                    if (engineType == EngineType.Gasoline)
                    {
                        _carCustomClearanceCost.CustomsDuty = euroExchangeRate * engineVolume switch
                        {
                            <= 1000 => carAge switch
                            {
                                <= 3 => convertedCarPrice * 0.15M,
                                <= 5 => Math.Max(convertedCarPrice * 0.2M, engineVolume * 0.36M),
                                <= 7 => Math.Max(convertedCarPrice * 0.2M, engineVolume * 0.36M),
                                _ => engineVolume * 1.4M
                            },
                            <= 1500 => carAge switch
                            {
                                <= 3 => convertedCarPrice * 0.15M,
                                <= 5 => Math.Max(convertedCarPrice * 0.2M, engineVolume * 0.4M),
                                <= 7 => Math.Max(convertedCarPrice * 0.2M, engineVolume * 0.4M),
                                _ => engineVolume * 1.5M
                            },
                            <= 1800 => carAge switch
                            {
                                <= 3 => convertedCarPrice * 0.15M,
                                <= 5 => Math.Max(convertedCarPrice * 0.2M, engineVolume * 0.36M),
                                <= 7 => Math.Max(convertedCarPrice * 0.2M, engineVolume * 0.36M),
                                _ => engineVolume * 1.6M
                            },
                            <= 2300 => carAge switch
                            {
                                <= 3 => convertedCarPrice * 0.15M,
                                <= 5 => Math.Max(convertedCarPrice * 0.2M, engineVolume * 0.44M),
                                <= 7 => Math.Max(convertedCarPrice * 0.2M, engineVolume * 0.44M),
                                _ => engineVolume * 2.2M
                            },
                            <= 2800 => carAge switch
                            {
                                <= 3 => convertedCarPrice * 0.15M,
                                <= 5 => Math.Max(convertedCarPrice * 0.2M, engineVolume * 0.44M),
                                <= 7 => Math.Max(convertedCarPrice * 0.2M, engineVolume * 0.44M),
                                _ => engineVolume * 2.2M
                            },
                            <= 3000 => carAge switch
                            {
                                <= 3 => convertedCarPrice * 0.125M,
                                <= 5 => Math.Max(convertedCarPrice * 0.2M, engineVolume * 0.44M),
                                <= 7 => Math.Max(convertedCarPrice * 0.2M, engineVolume * 0.44M),
                                _ => engineVolume * 2.2M
                            },
                            _ => carAge switch
                            {
                                <= 3 => convertedCarPrice * 0.125M,
                                <= 5 => Math.Max(convertedCarPrice * 0.2M, engineVolume * 0.8M),
                                <= 7 => Math.Max(convertedCarPrice * 0.2M, engineVolume * 0.8M),
                                _ => engineVolume * 3.2M
                            }
                        };
                    }
                    else if (engineType == EngineType.Diesel)
                    {
                        _carCustomClearanceCost.CustomsDuty = euroExchangeRate * engineVolume switch
                        {
                            <= 1500 => carAge switch
                            {
                                <= 3 => convertedCarPrice * 0.15M,
                                <= 5 => Math.Max(convertedCarPrice * 0.2M, engineVolume * 0.32M),
                                <= 7 => Math.Max(convertedCarPrice * 0.2M, engineVolume * 0.32M),
                                _ => engineVolume * 1.5M
                            },
                            <= 2500 => carAge switch
                            {
                                <= 3 => convertedCarPrice * 0.15M,
                                <= 5 => Math.Max(convertedCarPrice * 0.2M, engineVolume * 0.4M),
                                <= 7 => Math.Max(convertedCarPrice * 0.2M, engineVolume * 0.4M),
                                _ => engineVolume * 2.2M
                            },
                            _ => carAge switch
                            {
                                <= 3 => convertedCarPrice * 0.15M,
                                <= 5 => Math.Max(convertedCarPrice * 0.2M, engineVolume * 0.8M),
                                <= 7 => Math.Max(convertedCarPrice * 0.2M, engineVolume * 0.8M),
                                _ => engineVolume * 3.2M
                            }
                        };
                    }
                }
            }
        }

        private void CalculateExciseTax()
        {
            _carCustomClearanceCost.ExciseTax = enginePower * enginePower switch
            {
                <= 90 => 0,
                <= 150 => 61,
                <= 200 => 583,
                <= 300 => 955,
                <= 400 => 1628,
                <= 500 => 1685,
                _ => 1740
            };
        }

        private void CalculateVat()
        {
            _carCustomClearanceCost.Vat = (carPrice + _carCustomClearanceCost.CustomsDuty + _carCustomClearanceCost.ExciseTax) * 0.2M;
        }

        private void CalculateRecyclingFee()
        {
            //TODO For commercial cars
            int baseRate = 20000;

            decimal coefficient = importerType == ImporterType.Individual
                ? engineType == EngineType.Electric
                    ? carAge <= 3 ? 0.17M : 0.26M
                    : engineVolume switch
                    {
                        <= 1000 => carAge <= 3 ? 0.17M : 0.26M,
                        <= 2000 => carAge <= 3 ? 0.17M : 0.26M,
                        <= 3000 => carAge <= 3 ? 0.17M : 0.26M,
                        <= 3500 => carAge <= 3 ? 107.67M : 165.84M,
                        _ => carAge <= 3 ? 137.11M : 180.24M
                    }
                : engineType == EngineType.Electric
                    ? carAge <= 3 ? 33.37M : 58.7M
                    : engineVolume switch
                    {
                        <= 1000 => carAge <= 3 ? 9.01M : 23M,
                        <= 2000 => carAge <= 3 ? 33.37M : 58.7M,
                        <= 3000 => carAge <= 3 ? 93.77M : 141.97M,
                        <= 3500 => carAge <= 3 ? 107.67M : 165.84M,
                        _ => carAge <= 3 ? 137.11M : 80.24M
                    };
            _carCustomClearanceCost.RecyclingFee = baseRate * coefficient;
        }

        private void RoundCost()
        {
            _carCustomClearanceCost.CustomsFee = Math.Round(_carCustomClearanceCost.CustomsFee, 2);
            _carCustomClearanceCost.CustomsDuty = Math.Round(_carCustomClearanceCost.CustomsDuty, 2);
            _carCustomClearanceCost.ExciseTax = Math.Round(_carCustomClearanceCost.ExciseTax, 2);
            _carCustomClearanceCost.Vat = Math.Round(_carCustomClearanceCost.Vat, 2);
            _carCustomClearanceCost.RecyclingFee = Math.Round(_carCustomClearanceCost.RecyclingFee, 2);
        }

        /// <summary>
        /// Calculate car customs clearance cost
        /// </summary>
        /// <param name="roundCost">Round cost</param>
        /// <returns></returns>
        public CarCustomsClearanceCost Calculate(bool roundCost = true)
        {
            CalculateCustomsFee();
            CalculateCustomsDuty();
            if (importerType == ImporterType.LegalEntity)
            {
                CalculateExciseTax();
                CalculateVat();
            }
            CalculateRecyclingFee();

            if (roundCost)
            {
                RoundCost();
            }

            return _carCustomClearanceCost;
        }
    }
}