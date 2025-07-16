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
    public class AddressLevel3Service : IAddressLevel3Service
    {
        private readonly IAddressLevel1Repository _level1Repository;
        private readonly IAddressLevel2Repository _level2Repository;
        private readonly IAddressLevel3Repository _level3Repository;
        private readonly IAddressLevelStructureRepository _addressLevelStructureRepository;
        private readonly IAddressLocationService _addressLocationService;
        private readonly ICountryRepository _countryRepository;
        private readonly ILogger<AddressLevel3Service> _logger;

        public AddressLevel3Service(
            IAddressLevel1Repository level1Repository,
            IAddressLevel2Repository level2Repository,
            IAddressLevel3Repository level3Repository,
            IAddressLocationService addressLocationService,
            IAddressLevelStructureRepository AddressLevelStructureRepository,
            ICountryRepository countryRepository,
            ILogger<AddressLevel3Service> logger)
        {
            _level1Repository = level1Repository;
            _level2Repository = level2Repository;
            _level3Repository = level3Repository;
            _countryRepository = countryRepository;
            _addressLocationService = addressLocationService;
            _addressLevelStructureRepository = AddressLevelStructureRepository;
            _logger = logger;
            _logger.LogTrace("Enter AddressLevelService");
        }

        public async Task AddLevel(AddressLevel source, ChangeContext? changeContext)
        {
            var item = source as AddressLevel3;
            _logger.LogDebug($"Add from AddressLevel3 : {JsonSerializer.Serialize(source)}");
            await CheckIntegrityForCREATE(item);
            await _level3Repository.AddAsync(item, changeContext);
        }

        private async Task CheckIntegrityForCREATE(AddressLevel3 source)
        {
            await Task.WhenAll(
                CheckCountryExist(source),
                CheckAddressLevelExistForPOST(source),
                CheckAddressLevelStructureIntegrity(source),
                _addressLocationService.ValidateAddressLocationsIdUnicity(source.Id)
                );
        }

        private async Task CheckCountryExist(AddressLevel3 source)
        {
            if (await _countryRepository.AnyAsync<Country>(x => x.Id == source.CountryId))
                return;
            _logger.LogError($"{DomainNotFoundKey.CountryNotFound.ToString()} - The country with id {source.CountryId} does not exist");
            throw new DomainNotFoundException(DomainNotFoundKey.CountryNotFound.ToString(), $"The country with id {source.CountryId} does not exist");
        }

        private async Task CheckParentLevelExist(string parentId, AddressLevelEnum parentLevel, AddressLevelEnum level)
        {
            switch (level) {
                case AddressLevelEnum.Level1:
                    if (await _countryRepository.AnyAsync<Country>(x => x.Id == parentId))
                        return;
                    _logger.LogError($"{DomainViolationKey.AddressLevelParentIdDoesNotExist.ToString()} - The parent with id {parentId} does not exist");
                    throw new DomainViolationException(DomainViolationKey.AddressLevelParentIdDoesNotExist.ToString(), $"The parent with id {parentId} does not exist");
                case AddressLevelEnum.Level2:
                    if (parentLevel == AddressLevelEnum.Level1 && await _level1Repository.AnyAsync<AddressLevel1>(x => x.Id == parentId)) {
                        return;
                    }
                    else if (await _countryRepository.AnyAsync<Country>(x => x.Id == parentId)) {
                        _logger.LogDebug($"The addressStructure doesn't provide an address level 1, the address level 2 provided is now linked to Country : {parentId}");
                        return;
                    }
                    _logger.LogError($"{DomainViolationKey.AddressLevelParentIdDoesNotExist.ToString()} - The parent with id {parentId} does not exist");
                    throw new DomainViolationException(DomainViolationKey.AddressLevelParentIdDoesNotExist.ToString(), $"The parent with id {parentId} does not exist");
                case AddressLevelEnum.Level3:
                    if (parentLevel == AddressLevelEnum.Level2 && await _level2Repository.AnyAsync<AddressLevel2>(x => x.Id == parentId)) {
                        return;
                    }
                    else if (await _countryRepository.AnyAsync<Country>(x => x.Id == parentId)) {
                            _logger.LogDebug($"The addressStructure doesn't provide an address level 2, the address level 3 provided is now linked to Country : {parentId}");
                        return;
                    }  
                    _logger.LogError($"{DomainViolationKey.AddressLevelParentIdDoesNotExist.ToString()} - The parent with id {parentId} does not exist");
                    throw new DomainViolationException(DomainViolationKey.AddressLevelParentIdDoesNotExist.ToString(), $"The parent with id {parentId} does not exist");

                default:
                    _logger.LogError($"{DomainViolationKey.AddressLevelIsUnknown.ToString()} - The AddressLevel provided is not supported : {level}");
                    throw new DomainViolationException(DomainViolationKey.AddressLevelIsUnknown.ToString(), $"The AddressLevel provided is not supported : {level}");
            }
        }

        private async Task CheckAddressLevelStructureIntegrity(AddressLevel3 addressLevel)
        {
            var addressLevelStructure = await _addressLevelStructureRepository.SingleOrDefaultAsync<AddressLevelStructure>(x => x.CountryId == addressLevel.CountryId);
            if (addressLevelStructure == null) {
                _logger.LogError($"{DomainViolationKey.AddressLevelStructureDoNotExist.ToString()} - The AddressLevelStructure with countryId {addressLevel.CountryId} does not exist");
                throw new DomainViolationException(DomainViolationKey.AddressLevelStructureDoNotExist.ToString(), $"The AddressLevelStructure with countryId {addressLevel.CountryId} does not exist");
            }

            if (addressLevelStructure.AddressLevel3Description.IsAbbreviationUsed == false && addressLevelStructure.AddressLevel3Description.IsDescriptionUsed == false)
            {
                _logger.LogError($"{DomainViolationKey.AddressLevelStructureDoNotAllowLevel3.ToString()} - The AddressLevelStructure with countryId {addressLevel.ParentId} doesn't allow level 3");
                throw new DomainViolationException(DomainViolationKey.AddressLevelStructureDoNotAllowLevel3.ToString(), $"The AddressLevelStructure with countryId {addressLevel.ParentId} doesn't allow level 3");
            }

            if (addressLevelStructure.AddressLevel3Description.IsAbbreviationUsed && string.IsNullOrEmpty(addressLevel.Abbreviation) ||
                addressLevelStructure.AddressLevel3Description.IsDescriptionUsed && string.IsNullOrEmpty(addressLevel.LongLabel))
            {
                _logger.LogError($"{DomainViolationKey.AddressLevelStructureDoNotAllowThisLevel3Configuration.ToString()} - The AddressLevelStructure with countryId {addressLevel.ParentId} doesn't allow this configuration for level 3");
                throw new DomainViolationException(DomainViolationKey.AddressLevelStructureDoNotAllowThisLevel3Configuration.ToString(), $"The AddressLevelStructure with countryId {addressLevel.ParentId} doesn't allow this configuration for level 3");
            }

            if (addressLevelStructure.AddressLevel2Description.IsAbbreviationUsed == false && addressLevelStructure.AddressLevel2Description.IsDescriptionUsed == false)
            {
                await CheckParentLevelExist(addressLevel.ParentId, AddressLevelEnum.Country, AddressLevelEnum.Level3);
                addressLevel.ParentLevel = AddressLevelEnum.Country;
            }
            else
            {
                await CheckParentLevelExist(addressLevel.ParentId, AddressLevelEnum.Level2, AddressLevelEnum.Level3);
                addressLevel.ParentLevel = AddressLevelEnum.Level2;
            }
        }

        private async Task CheckAddressLevelExistForPOST(AddressLevel3 source)
        {
            if (source.Id == null)
                return;
            if (!await _level3Repository.AnyAsync<AddressLevel3>(x => x.Id == source.Id))
                return;

            _logger.LogError($"{DomainViolationKey.AddressLevelAlreadyExist.ToString()} - The AddressLevel {source.Level} with id {source.Id} already exist");
            throw new DomainViolationException(DomainViolationKey.AddressLevelAlreadyExist.ToString(), $"The AddressLevel {source.Level} with id {source.Id} already exist");
        }

        private async Task CheckAddressLevelExistForUpdate(AddressLevel addressLevel)
        {
            if (addressLevel.Id == null)
                return;

            if (await _level3Repository.SingleOrDefaultFromCountry(addressLevel.CountryId, addressLevel.Id) != null)
                return;
            _logger.LogError($"{DomainViolationKey.AddressLevel3DoNotExist.ToString()} - The AddressLevel 3 with id {addressLevel.Id} does not exist for CountryId {addressLevel.CountryId}");
            throw new DomainViolationException(DomainViolationKey.AddressLevel3DoNotExist.ToString(), $"The AddressLevel 3 with id {addressLevel.Id} does not exist for CountryId {addressLevel.CountryId}");
        }

        private async Task CheckIntegrityForUPDATE(AddressLevel3 source)
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
                var item = await _level3Repository.SingleOrDefaultFromCountry(countryId, id);
                if (item != null)
                    return item;
            }
            _logger.LogError($"{DomainNotFoundKey.AddressLevel3NotFound} - The AddressLevel 3 with id {id} does not exist");
            throw new DomainNotFoundException(DomainNotFoundKey.AddressLevel3NotFound.ToString(), $"The AddressLevel 3 with id {id} does not exist");
        }

        public async Task UpdateLevel(AddressLevel source, ChangeContext? changeContext)
        {
            var item = source as AddressLevel3;
            _logger.LogDebug($"Update from AddressLevel3 : {JsonSerializer.Serialize(item)}");
            await CheckIntegrityForUPDATE(item);
            await _level3Repository.UpdateAsync(item, changeContext);
        }

        private async Task CheckNoChildrenLinked(string id)
        {
            if (await _level3Repository.IsLinkedToChildren(id))
            {
                _logger.LogError($"{DomainViolationKey.AddressLevel3DoNotExist.ToString()} - The AddressLevel 3 with id {id} does not exist");
                throw new DomainViolationException(DomainViolationKey.AddressLevel3DoNotExist.ToString(), $"The AddressLevel 3 with id {id} does not exist");
            }
        }
        public async Task DeleteLevel(AddressLevel source, ChangeContext? changeContext)
        {
            var item = source as AddressLevel3;
            _logger.LogDebug($"Delete from AddressLevel3 : {JsonSerializer.Serialize(item)}");
            await CheckIntegrityForDELETE(item.Id);
            await _level3Repository.RemoveAsync(item, changeContext);
        }

        public async Task SaveChangesAsync()
        {
            await _level3Repository.ApplyAsync();
        }
    }
}
