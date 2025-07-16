using AddressLocation.Domain.Enums;
using AddressLocation.Domain.Models;
using AddressLocation.Domain.Repositories;
using AddressLocation.Domain.Services.Interfaces;
using Framework.Exceptions;
using Microsoft.Extensions.Logging;

namespace AddressLocation.Domain.Services
{
    public class AddressLocationService : IAddressLocationService
    {
        private readonly ICityRepository _cityRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IAddressLevel1Repository _addressLevel1Repository;
        private readonly IAddressLevel2Repository _addressLevel2Repository;
        private readonly IAddressLevel3Repository _addressLevel3Repository;
        private readonly ILogger<AddressLocationService> _logger;
        public AddressLocationService(
            ICityRepository cityRepository,
            ICountryRepository countryRepository,
            IAddressLevel1Repository addressLevel1Repository,
            IAddressLevel2Repository addressLevel2Repository,
            IAddressLevel3Repository addressLevel3Repository,
            ILogger<AddressLocationService> logger) {
        
            _addressLevel1Repository = addressLevel1Repository;
            _addressLevel2Repository = addressLevel2Repository;
            _addressLevel3Repository = addressLevel3Repository;
            _countryRepository = countryRepository;
            _cityRepository = cityRepository;
            _logger = logger;
        }

        public async Task ValidateAddressLocationsIdUnicity(string? id)
        {
            if (string.IsNullOrEmpty(id))
                return;
            await Task.WhenAll(
                ValidateAddressLevelIds(id),
                ValidateCountryId(id),
                ValidateCityId(id));
        }

        private async Task ValidateAddressLevelIds(string id) {
            if (!await _addressLevel1Repository.AnyAsync<AddressLevel1>(x => x.Id == id))
                return;
            if (!await _addressLevel2Repository.AnyAsync<AddressLevel2>(x => x.Id == id))
                return;
            if (!await _addressLevel3Repository.AnyAsync<AddressLevel3>(x => x.Id == id))
                return;
            _logger.LogError($"{DomainViolationKey.LocationIdAlreadyUsed.ToString()} - The location Id : {id} is Already Used");
            throw new DomainViolationException(DomainViolationKey.LocationIdAlreadyUsed.ToString(), $"The location Id : {id} is Already Used");
        }

        private async Task ValidateCountryId(string id)
        {
            if (!await _countryRepository.AnyAsync<Country>(x => x.Id == id))
                return;
            _logger.LogError($"{DomainViolationKey.LocationIdAlreadyUsed.ToString()} - The location Id : {id} is Already Used by country");
            throw new DomainViolationException(DomainViolationKey.LocationIdAlreadyUsed.ToString(), $"The location Id : {id} is Already Used by country");
        }

        private async Task ValidateCityId(string id)
        {
            if (!await _cityRepository.AnyAsync<City>(x => x.Id == id))
                return;
            _logger.LogError($"{DomainViolationKey.LocationIdAlreadyUsed.ToString()} - The location Id : {id} is Already Used by city");
            throw new DomainViolationException(DomainViolationKey.LocationIdAlreadyUsed.ToString(), $"The location Id : {id} is Already Used by city");
        }
    }
}
