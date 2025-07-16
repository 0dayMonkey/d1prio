using AddressLocation.Domain.Models;
using AddressLocation.Domain.Repositories;
using AddressLocation.Infrastructure.Database;
using AddressLocation.Infrastructure.Database.Enums;
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
    public class AddressLevel1Repository : IAddressLevel1Repository
    {
        private readonly AddressLocationContext _context;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly IMapper _mapper;

        public AddressLevel1Repository(AddressLocationContext context, IMapper mapper)
        {
            _context = context;
            _jsonOptions = MessagesJsonSerializationOptions.Create();
            _mapper = mapper;
        }

        public void Dispose()
        {
            _context.Dispose();
        }


        public async Task<List<AddressLevel>> ListAllFromCountry(string countryId, bool tracking = false)
        {
            var query = _context.AddressLevel1
                .Include(x => x.AsChildAddressPath)
                .Where(x => x.AsChildAddressPath.ParentId == countryId)
                .Select(c => _mapper.Map<DbAddressLevel1, AddressLevel1>(c)).Cast<AddressLevel>();
            return tracking ? await query.ToListAsync() : await query.AsNoTracking().ToListAsync();
        }

        public async Task<AddressLevel?> SingleOrDefaultFromCountry(string countryId, string id, bool tracking = false)
        {
            var query = _context.AddressLevel1
                .Include(x => x.AsChildAddressPath) 
                .Where(x =>
                    (x.AsChildAddressPath.ParentId == countryId) && x.Id == id)
                .Select(c => _mapper.Map<DbAddressLevel1, AddressLevel1>(c)).Cast<AddressLevel>();
            return tracking ? await query.SingleOrDefaultAsync() : await query.AsNoTracking().SingleOrDefaultAsync();
        }

        public async Task<List<AddressLevel1>> ListAsync<U>(Expression<Func<AddressLevel1, bool>>? filter = null, bool tracking = false) where U : AddressLevel1
        {
            Expression<Func<DbAddressLevel1, bool>> predicate = filter == null ? t => true : _mapper.Map<Expression<Func<DbAddressLevel1, bool>>>(filter);
            var query = _context.AddressLevel1
                .Where(predicate)
                .Select(c => _mapper.Map<DbAddressLevel1, AddressLevel1>(c)); 
            return tracking ? await query.ToListAsync() : await query.AsNoTracking().ToListAsync();
        }

        public async Task<AddressLevel1?> SingleOrDefaultAsync<U>(Expression<Func<AddressLevel1, bool>> filter, bool tracking = false) where U : AddressLevel1
        {
            var query = _context.AddressLevel1
                .Include(x => x.AsChildAddressPath)
                .Where(_mapper.Map<Expression<Func<DbAddressLevel1, bool>>>(filter))
                .Select(c => _mapper.Map<DbAddressLevel1, AddressLevel1>(c));
            return tracking ? await query.SingleOrDefaultAsync() : await query.AsNoTracking().SingleOrDefaultAsync();
        }

        public async Task<bool> AnyAsync<U>(Expression<Func<AddressLevel1, bool>> filter) where U : AddressLevel1
        {
            return await _context.AddressLevel1.AnyAsync(_mapper.Map<Expression<Func<DbAddressLevel1, bool>>>(filter));
        }

        public async Task AddAsync(AddressLevel1 item, ChangeContext changeContext)
        {
            if (string.IsNullOrEmpty(item.Id))
                item.Id = _context.GetGalaxisId();

            if (item.LastUpdatedTimestamp == null)
                item.LastUpdatedTimestamp = DateTime.Now;

            var addressLevel = _mapper.Map<AddressLevel1, DbAddressLevel1>(item);
            addressLevel.AsChildAddressPath.ChildLevel = (int)LevelDescriptionType.Level1;
            addressLevel.AsChildAddressPath.ParentLevel = (int)item.ParentLevel;
            addressLevel.AsChildAddressPath.LastUpdatedTimestamp = item.LastUpdatedTimestamp;

            await _context.AddressLevel1.AddAsync(addressLevel);

            var message = new DbOutboxMessage
            {
                CorrelationId = changeContext.CorrelationId,
                SiteOriginId = changeContext.SiteOriginId,
                Exchange = OutboxMessageKeys.Exchange,
                Context = OutboxMessageKeys.Context,
                IsReliable = true,
                RoutingKey = "ref.address-level-1.created",
                Data = JsonSerializer.Serialize(new RessourceCreated { Data = item }, _jsonOptions),
            };
            await _context.OutboxMessages.AddAsync(message);
        }


        public async Task UpdateAsync(AddressLevel1 item, ChangeContext changeContext)
        {
            var addressLevel = await _context.AddressLevel1.FindAsync(item.Id);
            var oldItem = _mapper.Map<DbAddressLevel1, AddressLevel1>(addressLevel);

            if (item.LastUpdatedTimestamp == null)
                item.LastUpdatedTimestamp = DateTime.Now;

            _mapper.Map(item, addressLevel);

            var addressPath = _context.AddressPath.SingleOrDefault(x => x.ChildId == oldItem.Id);
            if (addressPath != null)
            {
                addressPath.ParentId = item.ParentId;
                addressPath.ChildId = item.Id;
            }

            var message = new DbOutboxMessage
            {
                CorrelationId = changeContext.CorrelationId,
                SiteOriginId = changeContext.SiteOriginId,
                Exchange = OutboxMessageKeys.Exchange,
                Context = OutboxMessageKeys.Context,
                IsReliable = true,
                RoutingKey = "ref.address-level-1.updated",
                Data = JsonSerializer.Serialize(new RessourceUpdated { OldData = oldItem, NewData = item }, _jsonOptions),
            };
            await _context.OutboxMessages.AddAsync(message);
        }

        public async Task RemoveAsync(AddressLevel1 item, ChangeContext changeContext)
        {
            var addressLevel = await _context.AddressLevel1.FindAsync(item.Id);
            _context.AddressLevel1.Remove(addressLevel);

            var addressPaths = await _context.AddressPath.Where(x => x.ParentId == item.Id || x.ChildId == item.Id).ToListAsync();

            if(addressPaths.Any())
                _context.AddressPath.RemoveRange(addressPaths);

            var message = new DbOutboxMessage
            {
                CorrelationId = changeContext.CorrelationId,
                SiteOriginId = changeContext.SiteOriginId,
                Exchange = OutboxMessageKeys.Exchange,
                Context = OutboxMessageKeys.Context,
                IsReliable = true,
                RoutingKey = "ref.address-level-1.deleted",
                Data = JsonSerializer.Serialize(new RessourceDeleted { Data = item }, _jsonOptions),
            };
            await _context.OutboxMessages.AddAsync(message);
        }

        public async Task ApplyAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsLinkedToChildren(string id)
        {
            return await _context.AddressPath.AnyAsync(x => x.ParentId == id);
        }
    }
}
