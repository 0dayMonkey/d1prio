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
    public class AddressLevel2Service : IAddressLevel2Service
    {
        private readonly IAddressLevel1Repository _level1Repository;
        private readonly IAddressLevel2Repository _level2Repository;
        private readonly IAddressLevelStructureRepository _addressLevelStructureRepository;
        private readonly IAddressLocationService _addressLocationService;
        private readonly ICountryRepository _countryRepository;
        private readonly ILogger<AddressLevel2Service> _logger;

        public AddressLevel2Service(
            IAddressLevel1Repository level1Repository,
            IAddressLevel2Repository level2Repository,
            IAddressLevelStructureRepository AddressLevelStructureRepository,
            IAddressLocationService addressLocationService,
            ICountryRepository countryRepository,
            ILogger<AddressLevel2Service> logger)
        {
            _level1Repository = level1Repository;
            _level2Repository = level2Repository;
            _countryRepository = countryRepository;
            _addressLocationService = addressLocationService;
            _addressLevelStructureRepository = AddressLevelStructureRepository;
            _logger = logger;
            _logger.LogTrace("Enter AddressLevelService");
        }

        public async Task AddLevel(AddressLevel source, ChangeContext? changeContext)
        {
            var item = source as AddressLevel2;
            _logger.LogDebug($"Add from AddressLevel2 : {JsonSerializer.Serialize(item)}");
            await CheckIntegrityForCREATE(item);
            await _level2Repository.AddAsync(item, changeContext);
        }

        private async Task CheckIntegrityForCREATE(AddressLevel2 source)
        {
            await Task.WhenAll(
                CheckCountryExist(source),
                CheckAddressLevelExistForPOST(source),
                CheckAddressLevelStructureIntegrity(source),
                _addressLocationService.ValidateAddressLocationsIdUnicity(source.Id)
                );
        }

        private async Task CheckCountryExist(AddressLevel2 source)
        {
            if (await _countryRepository.AnyAsync<Country>(x => x.Id == source.CountryId))
                return;
            _logger.LogError($"{DomainNotFoundKey.CountryNotFound.ToString()} - The country with id {source.CountryId} does not exist");
            throw new DomainNotFoundException(DomainNotFoundKey.CountryNotFound.ToString(), $"The country with id {source.CountryId} does not exist");
        }

        private async Task CheckParentLevelExist(string parentId, AddressLevelEnum parentLevel, AddressLevelEnum level)
        {
            if (parentLevel == AddressLevelEnum.Level1 && await _level1Repository.AnyAsync<AddressLevel1>(x => x.Id == parentId)) {
                return;
            }
            else if (await _countryRepository.AnyAsync<Country>(x => x.Id == parentId)) {
                _logger.LogDebug($"The addressStructure doesn't provide an address level 1, the address level 2 provided is now linked to Country : {parentId}");
                return;
            }
            _logger.LogError($"{DomainViolationKey.AddressLevelParentIdDoesNotExist.ToString()} - The parent with id {parentId} does not exist");
            throw new DomainViolationException(DomainViolationKey.AddressLevelParentIdDoesNotExist.ToString(), $"The parent with id {parentId} does not exist");
        }

        private async Task CheckAddressLevelStructureIntegrity(AddressLevel2 source)
        {
            var addressLevelStructure = await _addressLevelStructureRepository.SingleOrDefaultAsync<AddressLevelStructure>(x => x.CountryId == source.CountryId);
            if (addressLevelStructure == null) {
                _logger.LogError($"{DomainViolationKey.AddressLevelStructureDoNotExist.ToString()} - The AddressLevelStructure with countryId {source.CountryId} does not exist");
                throw new DomainViolationException(DomainViolationKey.AddressLevelStructureDoNotExist.ToString(), $"The AddressLevelStructure with countryId {source.CountryId} does not exist");
            }

            if (addressLevelStructure.AddressLevel2Description.IsAbbreviationUsed == false && addressLevelStructure.AddressLevel2Description.IsDescriptionUsed == false)
            {
                _logger.LogError($"{DomainViolationKey.AddressLevelStructureDoNotAllowLevel2.ToString()} - The AddressLevelStructure with countryId {source.ParentId} doesn't allow level 2");
                throw new DomainViolationException(DomainViolationKey.AddressLevelStructureDoNotAllowLevel2.ToString(), $"The AddressLevelStructure with countryId {source.ParentId} doesn't allow level 2");
            }

            if (addressLevelStructure.AddressLevel2Description.IsAbbreviationUsed && string.IsNullOrEmpty(source.Abbreviation) ||
                addressLevelStructure.AddressLevel2Description.IsDescriptionUsed && string.IsNullOrEmpty(source.LongLabel))
            {
                _logger.LogError($"{DomainViolationKey.AddressLevelStructureDoNotAllowThisLevel2Configuration.ToString()} - The AddressLevelStructure with countryId {source.ParentId} doesn't allow this configuration for level 2");
                throw new DomainViolationException(DomainViolationKey.AddressLevelStructureDoNotAllowThisLevel2Configuration.ToString(), $"The AddressLevelStructure with countryId {source.ParentId} doesn't allow this configuration for level 2");
            }

            if (addressLevelStructure.AddressLevel1Description.IsAbbreviationUsed == false && addressLevelStructure.AddressLevel1Description.IsDescriptionUsed == false)
            {
                await CheckParentLevelExist(source.ParentId, AddressLevelEnum.Country, AddressLevelEnum.Level2);
                source.ParentLevel = AddressLevelEnum.Country;
            }
            else {
                await CheckParentLevelExist(source.ParentId, AddressLevelEnum.Level1, AddressLevelEnum.Level2);
                source.ParentLevel = AddressLevelEnum.Level1;
            }
        }

        private async Task CheckAddressLevelExistForPOST(AddressLevel2 addressLevel)
        {
            if (addressLevel.Id == null)
                return;

            if (!await _level2Repository.AnyAsync<AddressLevel2>(x => x.Id == addressLevel.Id))
                return;
            _logger.LogError($"{DomainViolationKey.AddressLevelAlreadyExist.ToString()} - The AddressLevel {addressLevel.Level} with id {addressLevel.Id} already exist");
            throw new DomainViolationException(DomainViolationKey.AddressLevelAlreadyExist.ToString(), $"The AddressLevel {addressLevel.Level} with id {addressLevel.Id} already exist");
        }

        private async Task CheckAddressLevelExistForUpdate(AddressLevel addressLevel)
        {
            if (addressLevel.Id == null)
                return;

            if (await _level2Repository.SingleOrDefaultFromCountry(addressLevel.CountryId, addressLevel.Id) != null)
                return;
            _logger.LogError($"{DomainViolationKey.AddressLevel2DoNotExist.ToString()} - The AddressLevel 2 with id {addressLevel.Id} does not exist for CountryId {addressLevel.CountryId}");
            throw new DomainViolationException(DomainViolationKey.AddressLevel2DoNotExist.ToString(), $"The AddressLevel 2 with id {addressLevel.Id} does not exist for CountryId {addressLevel.CountryId}");
        }

        private async Task CheckIntegrityForUPDATE(AddressLevel2 source)
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

        public async Task<AddressLevel> CheckAddressLevelExistForDelete(string id, string countryId)
        {
            if (id != null)
            {
                var item = await _level2Repository.SingleOrDefaultFromCountry(countryId, id);
                if (item != null)
                    return item;
            }
            _logger.LogError($"{DomainNotFoundKey.AddressLevel2NotFound} - The AddressLevel 2 with id {id} does not exist");
            throw new DomainNotFoundException(DomainNotFoundKey.AddressLevel2NotFound.ToString(), $"The AddressLevel 2 with id {id} does not exist");
        }

        public async Task UpdateLevel(AddressLevel source, ChangeContext? changeContext)
        {
            var item = source as AddressLevel2;
            _logger.LogDebug($"Update from AddressLevel2 : {JsonSerializer.Serialize(item)}");
            await CheckIntegrityForUPDATE(item);
            await _level2Repository.UpdateAsync(item, changeContext);
        }

        private async Task CheckNoChildrenLinked(string id)
        {
            if (await _level2Repository.IsLinkedToChildren(id))
            {
                _logger.LogError($"{DomainViolationKey.AddressLevel2DoNotExist.ToString()} - The AddressLevel 2 with id {id} does not exist");
                throw new DomainViolationException(DomainViolationKey.AddressLevel2DoNotExist.ToString(), $"The AddressLevel 2 with id {id} does not exist");
            }
        }
        public async Task DeleteLevel(AddressLevel source, ChangeContext? changeContext)
        {
            var item = source as AddressLevel2;
            _logger.LogDebug($"Delete from AddressLevel2 : {JsonSerializer.Serialize(item)}");
            await CheckIntegrityForDELETE(item.Id);
            await _level2Repository.RemoveAsync(item, changeContext);
        }

        public async Task SaveChangesAsync()
        {
            await _level2Repository.ApplyAsync();
        }
    }
}
