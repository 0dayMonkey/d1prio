using AddressLocation.Domain.Enums;
using AddressLocation.Domain.Models;
using AddressLocation.Domain.Repositories;
using AddressLocation.Domain.Services.Interfaces;
using Framework.Exceptions;
using Framework.Repositories;
using Microsoft.Extensions.Logging;

namespace AddressLocation.Domain.Services
{
    public class AddressLevelStructureService : IAddressLevelStructureService
    {
        private readonly IAddressLevelStructureRepository _repository;
        private readonly ICountryRepository _countryRepository;
        private readonly ILogger<AddressLevelStructureService> _logger;

        public AddressLevelStructureService(
            IAddressLevelStructureRepository repository,
            ICountryRepository countryRepository,
            ILogger<AddressLevelStructureService> logger)
        {
            _repository = repository;
            _countryRepository = countryRepository;
            _logger = logger;
            _logger.LogTrace("Enter AddressLevelStructuresService");
        }

        public async Task Add(AddressLevelStructure source, ChangeContext? changeContext)
        {
            _logger.LogDebug($"Add from AddressLevelStructure");
            await CheckIntegrityForCREATE(source);
            await _repository.AddAsync(source, changeContext);
        }

        private async Task CheckIntegrityForCREATE(AddressLevelStructure source)
        {
            await Task.WhenAll(
                CheckDataCoherence(source),
                CheckCountryExist(source.CountryId),
                CheckAddressLevelStructureExistForPOST(source.CountryId)
                );
        }

        private async Task CheckIntegrityForUPDATE(AddressLevelStructure source)
        {
            await Task.WhenAll(
                CheckUpdateValidity(source),
                CheckDataCoherence(source),
                CheckAddressLevelStructureExistForUPDATE(source.CountryId)
                );
        }

        private async Task CheckCountryExist(string countryId)
        {
            if (await _countryRepository.AnyAsync<Country>(x => x.Id == countryId))
                return;
            _logger.LogError($"{DomainNotFoundKey.CountryNotFound.ToString()} - The country with id {countryId} does not exist");
            throw new DomainNotFoundException(DomainNotFoundKey.CountryNotFound.ToString(), $"The country with id {countryId} does not exist");
        }

        private async Task CheckUpdateValidity(AddressLevelStructure source) {
            var country = await _countryRepository.SingleOrDefaultAsync<Country>(x => x.Id == source.CountryId);
            if (country == null) {
                _logger.LogError($"{DomainNotFoundKey.CountryNotFound.ToString()} - The country with id {source.CountryId} does not exist");
                throw new DomainNotFoundException(DomainNotFoundKey.CountryNotFound.ToString(), $"The country with id {source.CountryId} does not exist");
            }
            if (await _countryRepository.HasAddressLevelOrCityLinked(source.CountryId)) {
                _logger.LogError($"{DomainViolationKey.CountryLinkToChildren} - The country with id {source.CountryId} already have child entity and the address level structure can't be updated");
                throw new DomainViolationException(DomainViolationKey.CountryLinkToChildren.ToString(), $"The country with id {source.CountryId} already have child entity and the address level structure can't be updated");
            }
        }

        private async Task CheckDataCoherence(AddressLevelStructure source)
        {
            if (source.AddressLevel1Description != null && (source.AddressLevel1Description.IsAbbreviationUsed == true || source.AddressLevel1Description.IsDescriptionUsed == true) && string.IsNullOrEmpty(source.AddressLevel1Description.Label)) {
                _logger.LogError($"{DomainViolationKey.AddressLevelStructureDataLogicViolation.ToString()} - The addressLevel1Description is null or empty while being specified as set under a description or an abbreviation");
                throw new DomainViolationException(DomainViolationKey.AddressLevelStructureDataLogicViolation.ToString(), $"The addressLevel1Description is null or empty while being specified as set under a description or an abbreviation");
            }

            if (source.AddressLevel2Description != null && (source.AddressLevel2Description.IsAbbreviationUsed == true || source.AddressLevel2Description.IsDescriptionUsed == true) && string.IsNullOrEmpty(source.AddressLevel2Description.Label))
            {
                _logger.LogError($"{DomainViolationKey.AddressLevelStructureDataLogicViolation.ToString()} - The addressLevel2Description is null or empty while being specified as set under a description or an abbreviation");
                throw new DomainViolationException(DomainViolationKey.AddressLevelStructureDataLogicViolation.ToString(), $"The addressLevel2Description is null or empty while being specified as set under a description or an abbreviation");
            }

            if (source.AddressLevel3Description != null && (source.AddressLevel3Description.IsAbbreviationUsed == true || source.AddressLevel3Description.IsDescriptionUsed == true) && string.IsNullOrEmpty(source.AddressLevel3Description.Label))
            {
                _logger.LogError($"{DomainViolationKey.AddressLevelStructureDataLogicViolation.ToString()} - The addressLevel3Description is null or empty while being specified as set under a description or an abbreviation");
                throw new DomainViolationException(DomainViolationKey.AddressLevelStructureDataLogicViolation.ToString(), $"The addressLevel3Description is null or empty while being specified as set under a description or an abbreviation");
            }
        }

        private async Task CheckAddressLevelStructureExistForPOST(string countryId)
        {
            if (!await _repository.AnyAsync<AddressLevelStructure>(x => x.CountryId == countryId))
                return;
            _logger.LogError($"{DomainViolationKey.AddressLevelStructureAlreadyExist.ToString()} - The AddressLevelStructure for countryId {countryId} already exist");
            throw new DomainViolationException(DomainViolationKey.AddressLevelStructureAlreadyExist.ToString(), $"The AddressLevelStructure for countryId {countryId} already exist");
        }

        private async Task CheckAddressLevelStructureExistForUPDATE(string countryId)
        {
            if (await _repository.AnyAsync<AddressLevelStructure>(x => x.CountryId == countryId))
                return;
            _logger.LogError($"{DomainNotFoundKey.AddressLevelStructureNotFound} - The AddressLevelStructure for countryId {countryId} not found");
            throw new DomainNotFoundException(DomainNotFoundKey.AddressLevelStructureNotFound.ToString(), $"The AddressLevelStructure for countryId {countryId} not found");
        }

        public async Task Update(AddressLevelStructure source, ChangeContext changeContext)
        {
            _logger.LogDebug($"Add from AddressLevelStructure");
            await CheckIntegrityForUPDATE(source);
            await _repository.UpdateAsync(source, changeContext);
        }
    }
}
