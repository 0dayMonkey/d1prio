using AddressLocation.Domain.Enums;
using AddressLocation.Domain.Models;
using AddressLocation.Domain.Repositories;
using AddressLocation.Domain.Services.Interfaces;
using Framework.Exceptions;
using Framework.Repositories;
using Microsoft.Extensions.Logging;

namespace AddressLocation.Domain.Services
{
    public class CityService : ICityService
    {

        private readonly ICityRepository _cityRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IAddressLevel1Repository _level1Repository;
        private readonly IAddressLevel2Repository _level2Repository;
        private readonly IAddressLevel3Repository _level3Repository;
        private readonly IAddressLocationService _addressLocationService;
        private readonly IAddressLevelStructureRepository _addressLevelStructureRepository;

        private readonly ILogger<CityService> _logger;

        public CityService(
            ICityRepository cityRepository,
            ICountryRepository countryRepository,
            IAddressLevel1Repository level1Repository,
            IAddressLevel2Repository level2Repository,
            IAddressLevel3Repository addressLevel3Repository,
            IAddressLocationService addressLocationService,
            IAddressLevelStructureRepository levelStructureRepository,
            ILogger<CityService> logger)
        {
            _cityRepository = cityRepository;
            _countryRepository = countryRepository;
            _level1Repository = level1Repository;
            _level2Repository = level2Repository;
            _level3Repository = addressLevel3Repository;
            _addressLocationService = addressLocationService;
            _addressLevelStructureRepository = levelStructureRepository;
            _logger = logger;
            _logger.LogTrace("Enter CityService");
        }
        public async Task Add(City source, ChangeContext? changeContext)
        {
            await CheckIntegrityForCREATE(source);
            await _cityRepository.AddAsync(source, changeContext);
        }

        public async Task Update(City source, ChangeContext? changeContext)
        {
            await CheckIntegrityForUpdate(source);
            await _cityRepository.UpdateAsync(source, changeContext);
        }

        private async Task CheckIntegrityForCREATE(City source)
        {
            await Task.WhenAll(
                CheckCountryExist(source.CountryId),
                CheckCityExistForPOST(source.Id),
                CheckAddressLevelExistForPOSTPUTAndEnrich(source),
                CheckAddressLevelStructureIntegrity(source),
                _addressLocationService.ValidateAddressLocationsIdUnicity(source.Id)
            );
        }

        private async Task CheckIntegrityForUpdate(City source)
        {
            await Task.WhenAll(
                CheckCountryExist(source.CountryId),
                CheckCityExistForPUT(source.Id),
                CheckAddressLevelExistForPOSTPUTAndEnrich(source),
                CheckAddressLevelStructureIntegrity(source));
        }

        private async Task CheckAddressLevelExistForPOSTPUTAndEnrich(City city)
        {
            if (city.AddressLevel3 == null)
                return;
            var addressLevel3 = await _level3Repository.SingleOrDefaultAsync<AddressLevel3>(x => x.Id == city.AddressLevel3.Id);
            if (addressLevel3 == null)
            {
                _logger.LogError($"{DomainViolationKey.AddressLevel3DoNotExist} - The AddressLevel3 id : {city.AddressLevel3} provided does not exist");
                throw new DomainViolationException(DomainViolationKey.AddressLevel3DoNotExist.ToString(), $"The AddressLevel3 id: {city.AddressLevel3} provided does not exist");
            }
            if (addressLevel3.ParentLevel != AddressLevelEnum.Country) {
                city.AddressLevel2 = await _level2Repository.SingleOrDefaultAsync<AddressLevel2>(x => x.Id == addressLevel3.ParentId);
            }
            if (city.AddressLevel2 != null && addressLevel3.ParentLevel != AddressLevelEnum.Country) {
                city.AddressLevel1 = await _level1Repository.SingleOrDefaultAsync<AddressLevel1>(x => x.Id == city.AddressLevel2.ParentId);
            }

            city.AddressLevel3 = addressLevel3;
        }

        private async Task CheckAddressLevelStructureIntegrity(City source)
        {
            var addressLevelStructure = await _addressLevelStructureRepository.SingleOrDefaultAsync<AddressLevelStructure>(x => x.CountryId == source.CountryId);
            if (addressLevelStructure == null)
            {
                _logger.LogError($"{DomainViolationKey.AddressLevelStructureDoNotExist.ToString()} - The AddressLevelStructure prerequisite with countryId {source.CountryId} does not exist");
                throw new DomainViolationException(DomainViolationKey.AddressLevelStructureDoNotExist.ToString(), $"The AddressLevelStructure prerequisite with countryId {source.CountryId} does not exist");
            }

            if ((source.AddressLevel3 != null && addressLevelStructure.AddressLevel3Description.IsAbbreviationUsed == false && addressLevelStructure.AddressLevel3Description.IsDescriptionUsed == false) ) {
                _logger.LogError($"{DomainViolationKey.AddressLevelStructureDoNotAllowLevel3.ToString()} - AddressLevel3 not allowed on addressLevelStructure : {source.CountryId}");
                throw new DomainViolationException(DomainViolationKey.AddressLevelStructureDoNotAllowLevel3.ToString(), $"AddressLevel3 not allowed on addressLevelStructure : {source.CountryId}");
            }

            if ((source.AddressLevel3 == null && (addressLevelStructure.AddressLevel3Description.IsAbbreviationUsed == true || addressLevelStructure.AddressLevel3Description.IsDescriptionUsed == true))) {
                _logger.LogError($"{DomainViolationKey.AddressLevelStructureRequireLevel3.ToString()} - AddressLevel3 is required on addressLevelStructure : {source.CountryId}");
                throw new DomainViolationException(DomainViolationKey.AddressLevelStructureRequireLevel3.ToString(), $"AddressLevel3 is required on addressLevelStructure : {source.CountryId}");
            }
        }

        private async Task CheckCityExistForPOST(string id)
        {
            if (string.IsNullOrEmpty(id))
                return;
            if (!await _cityRepository.AnyAsync<City>(c => c.Id.ToLower().Equals(id.ToLower())))
                return;
            _logger.LogError($"{DomainViolationKey.CityAlreadyExist.ToString()} - The City with id {id} already exist");
            throw new DomainViolationException(DomainViolationKey.CityAlreadyExist.ToString(), $"The City with id {id} already exist");
        }

        private async Task CheckCityExistForPUT(string id)
        {
            if (string.IsNullOrEmpty(id))
                return;
            if (await _cityRepository.AnyAsync<City>(c => c.Id.ToLower().Equals(id.ToLower())))
                return;
            _logger.LogError($"{DomainViolationKey.CityAlreadyExist.ToString()} - The City with id {id} already exist");
            throw new DomainViolationException(DomainViolationKey.CityAlreadyExist.ToString(), $"The City with id {id} already exist");
        }

        private async Task CheckCountryExist(string countryId)
        {
            if (await _countryRepository.AnyAsync<Country>(c => c.Id.ToLower().Equals(countryId.ToLower())))
                return;
            _logger.LogError($"{DomainNotFoundKey.CountryNotFound.ToString()} - The country with id {countryId} does not exist");
            throw new DomainNotFoundException(DomainNotFoundKey.CountryNotFound.ToString(), $"The country with id {countryId} does not exist");
        }

        public async Task EnrichAddressLevels(City newCity)
        {
            if (newCity.AddressLevel1 != null && !string.IsNullOrEmpty(newCity.AddressLevel1.Id))
                newCity.AddressLevel1 = await _level1Repository.SingleOrDefaultAsync<AddressLevel1>(x => x.Id == newCity.AddressLevel1.Id);
            if (newCity.AddressLevel2 != null && !string.IsNullOrEmpty(newCity.AddressLevel2.Id))
                newCity.AddressLevel2 = await _level2Repository.SingleOrDefaultAsync<AddressLevel2>(x => x.Id == newCity.AddressLevel2.Id);
            if (newCity.AddressLevel3 != null && !string.IsNullOrEmpty(newCity.AddressLevel3.Id))
                newCity.AddressLevel3 = await _level3Repository.SingleOrDefaultAsync<AddressLevel3>(x => x.Id == newCity.AddressLevel3.Id);
        }

        public async Task EnrichAddressLevelsForPUT(City newCity)
        {
            var city = await _cityRepository.SingleOrDefaultAsync<City>(x => x.Id == newCity.Id, true);
            if (city == null) return;
            newCity.AddressLevel1 = city.AddressLevel1;
            newCity.AddressLevel2 = city.AddressLevel2;
        }
    }
}
