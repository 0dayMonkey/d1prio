using AddressLocation.Domain.Enums;
using AddressLocation.Domain.Models;
using AddressLocation.Domain.Repositories;
using AddressLocation.Domain.Services.Interfaces;
using Framework.Exceptions;
using Framework.Repositories;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace AddressLocation.Domain.Services
{
    public class AddressLevel1Service : IAddressLevel1Service
    {
        private readonly IAddressLevel1Repository _level1Repository;
        private readonly IAddressLevelStructureRepository _addressLevelStructureRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IAddressLocationService _addressLocationService;
        private readonly ILogger<AddressLevel1Service> _logger;

        public AddressLevel1Service(
            IAddressLevel1Repository level1Repository,
            IAddressLevelStructureRepository AddressLevelStructureRepository,
            ICountryRepository countryRepository,
            IAddressLocationService addressLocationService,
            ILogger<AddressLevel1Service> logger)
        {
            _level1Repository = level1Repository;
            _countryRepository = countryRepository;
            _addressLevelStructureRepository = AddressLevelStructureRepository;
            _addressLocationService = addressLocationService;
            _logger = logger;
            _logger.LogTrace("Enter AddressLevelService");
        }

        public async Task AddLevel(AddressLevel source, ChangeContext? changeContext)
        {
            var item = source as AddressLevel1;
            _logger.LogDebug($"Add from AddressLevel1 : {JsonSerializer.Serialize(item)}");
            await CheckIntegrityForCREATE(item);
            await _level1Repository.AddAsync(item, changeContext);
        }

        private async Task CheckIntegrityForCREATE(AddressLevel1 source)
        {
            await Task.WhenAll(
                CheckCountryExist(source),
                CheckAddressLevelExistForPOST(source),
                CheckAddressLevelStructureIntegrity(source),
                _addressLocationService.ValidateAddressLocationsIdUnicity(source.Id)
                );
        }

        private async Task CheckIntegrityForUPDATE(AddressLevel1 source)
        {
            await Task.WhenAll(
                CheckAddressLevelExistForUpdate(source),
                CheckCountryExist(source),
                CheckAddressLevelStructureIntegrity(source)
                );
        }

        private async Task CheckIntegrityForDELETE(string id)
        {
            await Task.WhenAll(
                    CheckNoChildrenLinked(id)
                );
        }

        private async Task CheckCountryExist(AddressLevel1 source)
        {
            if (await _countryRepository.AnyAsync<Country>(x => x.Id == source.CountryId))
                return;
            _logger.LogError($"{DomainNotFoundKey.CountryNotFound.ToString()} - The country with id {source.CountryId} does not exist");
            throw new DomainNotFoundException(DomainNotFoundKey.CountryNotFound.ToString(), $"The country with id {source.CountryId} does not exist");
        }

        private async Task CheckParentLevelExist(AddressLevel1 addressLevel)
        {
            if (await _countryRepository.AnyAsync<Country>(x => x.Id == addressLevel.ParentId))
                return;
            _logger.LogError($"{DomainViolationKey.AddressLevelParentIdDoesNotExist.ToString()} - The parent with id {addressLevel.ParentId} does not exist");
            throw new DomainViolationException(DomainViolationKey.AddressLevelParentIdDoesNotExist.ToString(), $"The parent with id {addressLevel.ParentId} does not exist");
        }

        private async Task CheckAddressLevelStructureIntegrity(AddressLevel1 addressLevel)
        {
            var addressLevelStructure = await _addressLevelStructureRepository.SingleOrDefaultAsync<AddressLevelStructure>(x => x.CountryId == addressLevel.CountryId);
            if (addressLevelStructure == null) {
                _logger.LogError($"{DomainViolationKey.AddressLevelStructureDoNotExist.ToString()} - The AddressLevelStructure with countryId {addressLevel.CountryId} does not exist");
                throw new DomainViolationException(DomainViolationKey.AddressLevelStructureDoNotExist.ToString(), $"The AddressLevelStructure with countryId {addressLevel.CountryId} does not exist");
            }

            if (addressLevelStructure.AddressLevel1Description.IsAbbreviationUsed && string.IsNullOrEmpty(addressLevel.Abbreviation) ||
                addressLevelStructure.AddressLevel1Description.IsDescriptionUsed && string.IsNullOrEmpty(addressLevel.LongLabel)) {
                _logger.LogError($"{DomainViolationKey.AddressLevelStructureDoNotAllowThisLevel1Configuration.ToString()} - The AddressLevelStructure with countryId {addressLevel.ParentId} doesn't allow this configuration for level 1");
                throw new DomainViolationException(DomainViolationKey.AddressLevelStructureDoNotAllowThisLevel1Configuration.ToString(), $"The AddressLevelStructure with countryId {addressLevel.ParentId} doesn't allow this configuration for level 1");
            }
            if (addressLevelStructure.AddressLevel1Description.IsAbbreviationUsed == false && addressLevelStructure.AddressLevel1Description.IsDescriptionUsed == false)
            {
                _logger.LogError($"{DomainViolationKey.AddressLevelStructureDoNotAllowLevel1.ToString()} - The AddressLevelStructure with countryId {addressLevel.ParentId} doesn't allow level 1");
                throw new DomainViolationException(DomainViolationKey.AddressLevelStructureDoNotAllowLevel1.ToString(), $"The AddressLevelStructure with countryId {addressLevel.ParentId} doesn't allow level 1");
            }
            await CheckParentLevelExist(addressLevel);
            addressLevel.ParentLevel = AddressLevelEnum.Country;
        }

        private async Task CheckAddressLevelExistForPOST(AddressLevel1 addressLevel)
        {
            if (addressLevel.Id == null)
                return;

            if (!await _level1Repository.AnyAsync<AddressLevel1>(x => x.Id == addressLevel.Id))
                return;
            _logger.LogError($"{DomainViolationKey.AddressLevelAlreadyExist.ToString()} - The AddressLevel {addressLevel.Level} with id {addressLevel.Id} already exist");
            throw new DomainViolationException(DomainViolationKey.AddressLevelAlreadyExist.ToString(), $"The AddressLevel {addressLevel.Level} with id {addressLevel.Id} already exist");
        }

        private async Task CheckAddressLevelExistForUpdate(AddressLevel addressLevel)
        {
            if (addressLevel.Id == null)
                return;

            if (await _level1Repository.SingleOrDefaultFromCountry(addressLevel.CountryId, addressLevel.Id) != null)
                return;
            _logger.LogError($"{DomainViolationKey.AddressLevel1DoNotExist.ToString()} - The AddressLevel 1 with id {addressLevel.Id} does not exist for CountryId {addressLevel.CountryId}");
            throw new DomainViolationException(DomainViolationKey.AddressLevel1DoNotExist.ToString(), $"The AddressLevel 1 with id {addressLevel.Id} does not exist for CountryId {addressLevel.CountryId}");
        }

        public async Task<AddressLevel> CheckAddressLevelExistForDelete(string id, string countryId)
        {
            if (id != null) {
                var item = await _level1Repository.SingleOrDefaultFromCountry(countryId, id);
                if (item != null)
                    return item;
            }
            _logger.LogError($"{DomainNotFoundKey.AddressLevel1NotFound} - The AddressLevel 1 with id {id} does not exist");
            throw new DomainNotFoundException(DomainNotFoundKey.AddressLevel1NotFound.ToString(), $"The AddressLevel 1 with id {id} does not exist");
        }

        public async Task UpdateLevel(AddressLevel source, ChangeContext? changeContext)
        {
            var item = source as AddressLevel1;
            _logger.LogDebug($"Update from AddressLevel1 : {JsonSerializer.Serialize(item)}");
            await CheckIntegrityForUPDATE(item);
            await _level1Repository.UpdateAsync(item, changeContext);
        }

        private async Task CheckNoChildrenLinked(string id)
        {
            if (await _level1Repository.IsLinkedToChildren(id)) {
                _logger.LogError($"{DomainViolationKey.AddressLevel1DoNotExist.ToString()} - The AddressLevel 1 with id {id} does not exist");
                throw new DomainViolationException(DomainViolationKey.AddressLevel1DoNotExist.ToString(), $"The AddressLevel 1 with id {id} does not exist");
            }
        }
        public async Task DeleteLevel(AddressLevel source, ChangeContext? changeContext)
        {
            var item = source as AddressLevel1;
            _logger.LogDebug($"Delete from AddressLevel1 : {JsonSerializer.Serialize(item)}");
            await CheckIntegrityForDELETE(item.Id);
            await _level1Repository.RemoveAsync(item, changeContext);
        }

        public async Task SaveChangesAsync()
        {
            await _level1Repository.ApplyAsync();
        }
    }
}
