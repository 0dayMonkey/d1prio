using AddressLocation.Domain.Models;
using AddressLocation.Domain.Repositories;
using AddressLocation.Infrastructure.Database;
using AddressLocation.Infrastructure.Database.Models;
using AddressLocation.Infrastructure.Keys;
using AutoMapper;
using Framework.Messages;
using Framework.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text.Json;

namespace AddressLocation.Infrastructure.Repositories
{
    public class AddressLevelStructureRepository : IAddressLevelStructureRepository
    {
        private readonly AddressLocationContext _context;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly IMapper _mapper;

        public AddressLevelStructureRepository(AddressLocationContext context, IMapper mapper)
        {
            _context = context;
            _jsonOptions = MessagesJsonSerializationOptions.Create();
            _mapper = mapper;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<List<AddressLevelStructure>> ListAsync<U>(Expression<Func<AddressLevelStructure, bool>>? filter = null, bool tracking = false) where U : AddressLevelStructure
        {
            Expression<Func<DbAddressLevelStructure, bool>> predicate = filter == null ? t => true : _mapper.Map<Expression<Func<DbAddressLevelStructure, bool>>>(filter);
            var query = _context.AddressLevelStructure
                .Where(predicate)
                .Select(c => _mapper.Map<DbAddressLevelStructure, AddressLevelStructure>(c));
            return tracking ? await query.ToListAsync() : await query.AsNoTracking().ToListAsync();
        }

        public async Task<AddressLevelStructure?> SingleOrDefaultAsync<U>(Expression<Func<AddressLevelStructure, bool>> filter, bool tracking = false) where U : AddressLevelStructure
        {
            var query = _context.AddressLevelStructure
                .Include(x => x.AddressLevel1Abbreviation)
                .Include(x => x.AddressLevel2Abbreviation)
                .Include(x => x.AddressLevel3Abbreviation)
                .Include(x => x.AddressLevel1Description)
                .Include(x => x.AddressLevel2Description)
                .Include(x => x.AddressLevel3Description)
                .Include(x => x.Country)
                .Where(_mapper.Map<Expression<Func<DbAddressLevelStructure, bool>>>(filter))
                .Select(c => _mapper.Map<DbAddressLevelStructure, AddressLevelStructure>(c)); ;
            return tracking ? await query.SingleOrDefaultAsync() : await query.AsNoTracking().SingleOrDefaultAsync();
        }

        public async Task<bool> AnyAsync<U>(Expression<Func<AddressLevelStructure, bool>> filter) where U : AddressLevelStructure
        {
            return await _context.AddressLevelStructure.AnyAsync(_mapper.Map<Expression<Func<DbAddressLevelStructure, bool>>>(filter));
        }

        public async Task AddAsync(AddressLevelStructure item, ChangeContext changeContext)
        {
            if (item.LastUpdatedTimestamp == null)
                item.LastUpdatedTimestamp = DateTime.Now;

            var addressLevelStructure = _mapper.Map<AddressLevelStructure, DbAddressLevelStructure>(item);

            AddAddressLevelDescription(item, addressLevelStructure);

            await _context.AddressLevelStructure.AddAsync(addressLevelStructure);

            var message = new DbOutboxMessage
            {
                CorrelationId = changeContext.CorrelationId,
                SiteOriginId = changeContext.SiteOriginId,
                Exchange = OutboxMessageKeys.Exchange,
                Context = OutboxMessageKeys.Context,
                IsReliable = true,
                RoutingKey = "ref.address-level-structure.created",
                Data = JsonSerializer.Serialize(new RessourceCreated { Data = item }, _jsonOptions),
            };
            await _context.OutboxMessages.AddAsync(message);
        }

        private void AddAddressLevelDescription(AddressLevelStructure addressLevelStructure, DbAddressLevelStructure dbAddressLevelStructure)
        {
            var nextId = _context.AddressLevelDescription.Max(x => x.Id).GetValueOrDefault();
            nextId++;
            List<DbAddressLevelLabel> addressLevels = new();
            if (addressLevelStructure.AddressLevel1Description != null)
            {
                dbAddressLevelStructure.AddressLevel1AbbreviationId = AddDescription(addressLevelStructure.AddressLevel1Description.Label, addressLevelStructure.AddressLevel1Description.IsAbbreviationUsed, ref nextId, addressLevelStructure.LastUpdatedTimestamp, addressLevels);
                if (addressLevelStructure.AddressLevel1Description.IsAbbreviationUsed && addressLevelStructure.AddressLevel1Description.IsDescriptionUsed)
                    dbAddressLevelStructure.AddressLevel1DescriptionId = dbAddressLevelStructure.AddressLevel1AbbreviationId;
                else
                    dbAddressLevelStructure.AddressLevel1DescriptionId = AddDescription(addressLevelStructure.AddressLevel1Description.Label, addressLevelStructure.AddressLevel1Description.IsDescriptionUsed, ref nextId, addressLevelStructure.LastUpdatedTimestamp, addressLevels);
            }
            if (addressLevelStructure.AddressLevel2Description != null)
            {
                dbAddressLevelStructure.AddressLevel2AbbreviationId = AddDescription(addressLevelStructure.AddressLevel2Description.Label, addressLevelStructure.AddressLevel2Description.IsAbbreviationUsed, ref nextId, addressLevelStructure.LastUpdatedTimestamp, addressLevels);
                if (addressLevelStructure.AddressLevel2Description.IsAbbreviationUsed && addressLevelStructure.AddressLevel2Description.IsDescriptionUsed)
                    dbAddressLevelStructure.AddressLevel2DescriptionId = dbAddressLevelStructure.AddressLevel2AbbreviationId;
                else
                    dbAddressLevelStructure.AddressLevel2DescriptionId = AddDescription(addressLevelStructure.AddressLevel2Description.Label, addressLevelStructure.AddressLevel2Description.IsDescriptionUsed, ref nextId, addressLevelStructure.LastUpdatedTimestamp, addressLevels);
            }
            if (addressLevelStructure.AddressLevel3Description != null)
            {
                dbAddressLevelStructure.AddressLevel3AbbreviationId = AddDescription(addressLevelStructure.AddressLevel3Description.Label, addressLevelStructure.AddressLevel3Description.IsAbbreviationUsed, ref nextId, addressLevelStructure.LastUpdatedTimestamp, addressLevels);
                if (addressLevelStructure.AddressLevel3Description.IsAbbreviationUsed && addressLevelStructure.AddressLevel3Description.IsDescriptionUsed)
                    dbAddressLevelStructure.AddressLevel3DescriptionId = dbAddressLevelStructure.AddressLevel3AbbreviationId;
                else
                    dbAddressLevelStructure.AddressLevel3DescriptionId = AddDescription(addressLevelStructure.AddressLevel3Description.Label, addressLevelStructure.AddressLevel3Description.IsDescriptionUsed, ref nextId, addressLevelStructure.LastUpdatedTimestamp, addressLevels);
            }
            if (addressLevelStructure.AddressLevel1Description != null)
                addressLevelStructure.AddressLevel1Description.Id = dbAddressLevelStructure.AddressLevel1DescriptionId > dbAddressLevelStructure.AddressLevel1AbbreviationId ? dbAddressLevelStructure.AddressLevel1DescriptionId : dbAddressLevelStructure.AddressLevel1AbbreviationId;
            if (addressLevelStructure.AddressLevel2Description != null)
                addressLevelStructure.AddressLevel2Description.Id = dbAddressLevelStructure.AddressLevel2DescriptionId > dbAddressLevelStructure.AddressLevel2AbbreviationId ? dbAddressLevelStructure.AddressLevel2DescriptionId : dbAddressLevelStructure.AddressLevel2AbbreviationId;
            if (addressLevelStructure.AddressLevel3Description != null)
                addressLevelStructure.AddressLevel3Description.Id = dbAddressLevelStructure.AddressLevel3DescriptionId > dbAddressLevelStructure.AddressLevel3AbbreviationId ? dbAddressLevelStructure.AddressLevel3DescriptionId : dbAddressLevelStructure.AddressLevel3AbbreviationId;
        }

        private decimal AddDescription(string? label, bool isUsed, ref decimal nextId, DateTime? timestamp, List<DbAddressLevelLabel> addressLevels)
        {
            if (!string.IsNullOrEmpty(label) && isUsed)
            {
                var desc = new DbAddressLevelLabel()
                {
                    Id = nextId,
                    Label = label,
                    LastUpdatedTimestamp = timestamp ?? DateTime.Now
                };
                var cachedItem = addressLevels.FirstOrDefault(x => x.Label.ToLower().Trim() == label?.ToLower().Trim() && x.Id != -1);
                if (cachedItem != null) {
                    return (decimal)cachedItem.Id;
                }
                var item = _context.AddressLevelDescription.FirstOrDefault(x => x.Label.ToLower().Trim() == label.ToLower().Trim() && x.Id != -1);
                if (item == null)
                {
                    _context.AddressLevelDescription.Add(desc);
                    addressLevels.Add(desc);  
                    return nextId++;
                }
                return (decimal)item.Id;
            }
            return -1;
        }

        public async Task UpdateAsync(AddressLevelStructure item, ChangeContext changeContext)
        {
            var addressLevelStructure = await _context.AddressLevelStructure.FindAsync(item.CountryId);
            var oldItem = _mapper.Map<DbAddressLevelStructure, AddressLevelStructure>(addressLevelStructure);

            if (item.LastUpdatedTimestamp == null)
                item.LastUpdatedTimestamp = DateTime.Now;

            _mapper.Map(item, addressLevelStructure);
            
            AddAddressLevelDescription(item, addressLevelStructure);

            var message = new DbOutboxMessage
            {
                CorrelationId = changeContext.CorrelationId,
                SiteOriginId = changeContext.SiteOriginId,
                Exchange = OutboxMessageKeys.Exchange,
                Context = OutboxMessageKeys.Context,
                IsReliable = true,
                RoutingKey = "ref.address-level-structure.updated",
                Data = JsonSerializer.Serialize(new RessourceUpdated { OldData = oldItem, NewData = item }, _jsonOptions),
            };
            await _context.OutboxMessages.AddAsync(message);
        }

        public async Task RemoveAsync(AddressLevelStructure item, ChangeContext changeContext)
        {
            var addressLevelStructure = await _context.AddressLevelStructure.FindAsync(item.CountryId);
            _context.AddressLevelStructure.Remove(addressLevelStructure);

            var message = new DbOutboxMessage
            {
                CorrelationId = changeContext.CorrelationId,
                SiteOriginId = changeContext.SiteOriginId,
                Exchange = OutboxMessageKeys.Exchange,
                Context = OutboxMessageKeys.Context,
                IsReliable = true,
                RoutingKey = "ref.address-level-structure.deleted",
                Data = JsonSerializer.Serialize(new RessourceDeleted { Data = item }, _jsonOptions),
            };
            await _context.OutboxMessages.AddAsync(message);
        }

        public async Task ApplyAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
