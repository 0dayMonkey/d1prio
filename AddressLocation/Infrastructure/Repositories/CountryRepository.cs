using AddressLocation.Domain.Models;
using AddressLocation.Domain.Repositories;
using AddressLocation.Infrastructure.Database;
using System.Linq.Dynamic.Core;
using System.Text.Json;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Framework.Messages;
using AutoMapper;
using Framework.Repositories;
using AddressLocation.Infrastructure.Database.Models;
using Framework.Models;
using Framework.Extensions;
using AddressLocation.Infrastructure.Keys;
using AddressLocation.Domain.Enums;

namespace AddressLocation.Infrastructure.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly AddressLocationContext _context;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly IMapper _mapper;

        public CountryRepository(AddressLocationContext context, IMapper mapper)
        {
            _context = context;
            _jsonOptions = MessagesJsonSerializationOptions.Create();
            _mapper = mapper;
        }

        public async Task<bool> LanguageExistsAsync(string languageId)
        {
            return await _context.Languages.AnyAsync(x => x.Id == languageId);
        }

        public async Task AddAsync(Country item, ChangeContext changeContext)
        {
            if (string.IsNullOrEmpty(item.Id))
                item.Id = _context.GetGalaxisId();
            if (item.LastUpdatedTimestamp == null)
                item.LastUpdatedTimestamp = DateTime.Now;

            await _context.Countries.AddAsync(_mapper.Map<Country, DbCountry>(item));

            var message = new DbOutboxMessage
            {
                CorrelationId = changeContext.CorrelationId,
                SiteOriginId = changeContext.SiteOriginId,
                Exchange = OutboxMessageKeys.Exchange,
                Context = OutboxMessageKeys.Context,
                IsReliable = true,
                RoutingKey = "ref.country.created",
                Data = JsonSerializer.Serialize(new RessourceCreated { Data = item }, _jsonOptions),
            };
            await _context.OutboxMessages.AddAsync(message);
        }

        public async Task UpdateAsync(Country item, ChangeContext changeContext)
        {
            var country = await _context.Countries.FindAsync(item.Id);
            var oldItem = _mapper.Map<DbCountry, Country>(country);

            if (item.LastUpdatedTimestamp == null)
                item.LastUpdatedTimestamp = DateTime.Now;
            _mapper.Map(item, country);

            var message = new DbOutboxMessage
            {
                CorrelationId = changeContext.CorrelationId,
                SiteOriginId = changeContext.SiteOriginId,
                Exchange = OutboxMessageKeys.Exchange,
                Context = OutboxMessageKeys.Context,
                IsReliable = true,
                RoutingKey = "ref.country.updated",
                Data = JsonSerializer.Serialize(new RessourceUpdated { OldData = oldItem, NewData = item }, _jsonOptions),
            };
            await _context.OutboxMessages.AddAsync(message);
        }

        public async Task RemoveAsync(Country item, ChangeContext changeContext)
        {
            _context.Countries.Remove(_mapper.Map<Country, DbCountry>(item));

            var message = new DbOutboxMessage
            {
                CorrelationId = changeContext.CorrelationId,
                SiteOriginId = changeContext.SiteOriginId,
                Exchange = OutboxMessageKeys.Exchange,
                Context = OutboxMessageKeys.Context,
                IsReliable = true,
                RoutingKey = "ref.country.deleted",
                Data = JsonSerializer.Serialize(new RessourceDeleted { Data = item }, _jsonOptions),
            };
            await _context.OutboxMessages.AddAsync(message);
        }

        public async Task ApplyAsync()
        {
            await _context.SaveChangesAsync();
        }

        public FoundItems<Country> Search<U>(Search<Country> search) where U : Country
        {
            if (_context.Countries == null)
                return new();

            Search<DbCountry> searchCountries = _mapper.Map<Search<Country>, Search<DbCountry>>(search);
            var result = _context.Countries.SearchBy(searchCountries);

            return new FoundItems<Country>
            {
                First = result.First,
                TotalItems = result.TotalItems,
                Items = result.Items.Select(x => _mapper.Map<DbCountry, Country>(x))
            };
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public bool IsUsed(string id)
        {
            return true; // To analyze 
        }

        public async Task<List<Country>> ListAsync<U>(Expression<Func<Country, bool>>? filter = null, bool tracking = false) where U : Country
        {
            Expression<Func<DbCountry, bool>> predicate = filter == null ? t => true : _mapper.Map<Expression<Func<DbCountry, bool>>>(filter);
            var query = _context.Countries
                .Where(predicate)
                .OrderBy(t => t.LongLabel)
                .Select(c => _mapper.Map<DbCountry, Country>(c));
            return tracking ? await query.ToListAsync() : await query.AsNoTracking().ToListAsync();
        }

        public async Task<Country?> SingleOrDefaultAsync<U>(Expression<Func<Country, bool>> filter, bool tracking = false) where U : Country
        {
            var query = _context.Countries
                .Where(_mapper.Map<Expression<Func<DbCountry, bool>>>(filter))
                .Select(c => _mapper.Map<DbCountry, Country>(c));
            return tracking ? await query.SingleOrDefaultAsync() : await query.AsNoTracking().SingleOrDefaultAsync();
        }

        public async Task<bool> HasAddressLevelOrCityLinked(string countryId) {
            return await _context.AddressPath.AnyAsync(x => x.ParentId == countryId && x.ParentLevel == (decimal)AddressLevelEnum.Country);
        }

        public async Task<bool> AnyAsync<U>(Expression<Func<Country, bool>> filter) where U : Country
        {
            return await _context.Countries.AnyAsync(_mapper.Map<Expression<Func<DbCountry, bool>>>(filter));
        }
		
        public async Task<List<Country>> GetCountriesforNationalities()
        {
            return await _context.Countries
                .Where(x => x.NationalityLabel != null)
                .OrderBy(x => x.NationalityLabel).ThenBy(x => x.Id)
                .Select(x => _mapper.Map<DbCountry, Country>(x))
                .ToListAsync();
        }
    }
}
