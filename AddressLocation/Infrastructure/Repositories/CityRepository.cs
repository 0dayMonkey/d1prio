using AddressLocation.Domain.Enums;
using AddressLocation.Domain.Models;
using AddressLocation.Domain.Repositories;
using AddressLocation.Infrastructure.Database;
using AddressLocation.Infrastructure.Database.Enums;
using AddressLocation.Infrastructure.Database.Models;
using AddressLocation.Infrastructure.Keys;
using AutoMapper;
using Framework.Extensions;
using Framework.Messages;
using Framework.Models;
using Framework.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text.Json;

namespace AddressLocation.Infrastructure.Repositories
{
    public class CityRepository: ICityRepository
    {
        private readonly AddressLocationContext _context;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly IMapper _mapper;

        public CityRepository(AddressLocationContext context, IMapper mapper)
        {
            _context = context;
            _jsonOptions = MessagesJsonSerializationOptions.Create();
            _mapper = mapper;
        }


        public void Dispose()
        {
            _context.Dispose();
        }        

        public bool IsUsed(string id)
        {
            return true;
        }

        public FoundItems<City> Search<U>(Search<City> search) where U : City
        {
            Search<DbCity> searchCities = _mapper.Map<Search<City>, Search<DbCity>>(search);
            if (searchCities.Filter != null)
                searchCities.Filter.Expression = UpdateExpressionToSupportTranslate(searchCities.Filter.Expression);
            var result = _context.Cities
                .Include(x => x.PostalCodes)
                .Include(x => x.AddressPath).ThenInclude(x => x.ParentAddressLevel3)
                .Include(x => x.AddressPath).ThenInclude(x => x.ParentCountry)

                .Include(x => x.AddressPath).ThenInclude(x => x.Parent).ThenInclude(x => x.ParentAddressLevel2)
                .Include(x => x.AddressPath).ThenInclude(x => x.Parent).ThenInclude(x => x.ParentCountry)

                .Include(x => x.AddressPath).ThenInclude(x => x.Parent).ThenInclude(x => x.Parent).ThenInclude(x => x.ParentAddressLevel1)
                .Include(x => x.AddressPath).ThenInclude(x => x.Parent).ThenInclude(x => x.Parent).ThenInclude(x => x.ParentCountry)

                .Include(x => x.AddressPath).ThenInclude(x => x.Parent).ThenInclude(x => x.Parent).ThenInclude(x => x.Parent).ThenInclude(x => x.ParentCountry)

                .OrderBy(x => x.LongLabel)
                .AsNoTracking()
                .SearchBy(searchCities);

            return new FoundItems<City>
            {
                First = result.First,
                TotalItems = result.TotalItems,
                Items = result.Items.Select(x => _mapper.Map<DbCity, City>(x))
            };
        }

        private Expression<Func<DbCity, bool>> UpdateExpressionToSupportTranslate(Expression<Func<DbCity, bool>> expression)
        {
            var visitor = new TranslateVisitor();
            var modifiedBody = visitor.VisitAndConvert(expression.Body, "UpdateExpressionToSupportTranslate");
            return Expression.Lambda<Func<DbCity, bool>>(modifiedBody, expression.Parameters);
        }

        private class TranslateVisitor : ExpressionVisitor {
            protected override Expression VisitMember(MemberExpression node)
            {
                    if (node.Member.Name == "LongLabel")
                    {
                        return Expression.Call(
                                   typeof(ContextDbFunctions),
                                   nameof(ContextDbFunctions.Translate),
                                   null,
                                    Expression.Call(node,
                                      typeof(string).GetMethod("ToUpper", Type.EmptyTypes)),
                                   Expression.Constant(ContextDbFunctions.From_String),
                                   Expression.Constant(ContextDbFunctions.To_String)
                               );
                    }
                    return base.VisitMember(node);
            }

            protected override Expression VisitConstant(ConstantExpression node)
            {
                if (node.Value != null && node.Type == typeof(string))
                {
                    return Expression.Constant(ContextDbFunctions.Translate(((string)node.Value).ToUpper(), ContextDbFunctions.From_String, ContextDbFunctions.To_String));
                }
                return base.VisitConstant(node);
            }
        }

        public async Task<List<City>> ListAsync<U>(Expression<Func<City, bool>>? filter = null, bool tracking = false) where U : City
        {
            Expression<Func<DbCity, bool>> predicate = filter == null ? t => true : _mapper.Map<Expression<Func<DbCity, bool>>>(filter);
            var query = _context.Cities
                .Include(x => x.PostalCodes)
                .Include(x => x.AddressPath).ThenInclude(x => x.ParentAddressLevel3)
                .Include(x => x.AddressPath).ThenInclude(x => x.ParentCountry)

                .Include(x => x.AddressPath).ThenInclude(x => x.Parent).ThenInclude(x => x.ParentAddressLevel2)
                .Include(x => x.AddressPath).ThenInclude(x => x.Parent).ThenInclude(x => x.ParentCountry)

                .Include(x => x.AddressPath).ThenInclude(x => x.Parent).ThenInclude(x => x.Parent).ThenInclude(x => x.ParentAddressLevel1)
                .Include(x => x.AddressPath).ThenInclude(x => x.Parent).ThenInclude(x => x.Parent).ThenInclude(x => x.ParentCountry)

                .Include(x => x.AddressPath).ThenInclude(x => x.Parent).ThenInclude(x => x.Parent).ThenInclude(x => x.Parent).ThenInclude(x => x.ParentCountry)

                .Where(predicate)

                .OrderBy(x => x.LongLabel)

                .Select(c => _mapper.Map<DbCity, City>(c));
            return tracking ? await query.ToListAsync() : await query.AsNoTracking().ToListAsync();
        }

        public async Task<City?> SingleOrDefaultAsync<U>(Expression<Func<City, bool>> filter, bool tracking = false) where U : City
        {
            var query = _context.Cities
                .Include(x => x.PostalCodes)
                .Include(x => x.AddressPath).ThenInclude(x => x.ParentAddressLevel3)
                .Include(x => x.AddressPath).ThenInclude(x => x.ParentCountry)

                .Include(x => x.AddressPath).ThenInclude(x => x.Parent).ThenInclude(x => x.ParentAddressLevel2)
                .Include(x => x.AddressPath).ThenInclude(x => x.Parent).ThenInclude(x => x.ParentCountry)

                .Include(x => x.AddressPath).ThenInclude(x => x.Parent).ThenInclude(x => x.Parent).ThenInclude(x => x.ParentAddressLevel1)
                .Include(x => x.AddressPath).ThenInclude(x => x.Parent).ThenInclude(x => x.Parent).ThenInclude(x => x.ParentCountry)

                .Include(x => x.AddressPath).ThenInclude(x => x.Parent).ThenInclude(x => x.Parent).ThenInclude(x => x.Parent).ThenInclude(x => x.ParentCountry)

                .Where(_mapper.Map<Expression<Func<DbCity, bool>>>(filter))

                .Select(c => _mapper.Map<DbCity, City>(c)); ;
            return tracking ? await query.SingleOrDefaultAsync() : await query.AsNoTracking().SingleOrDefaultAsync();
        }

        public async Task<bool> AnyAsync<U>(Expression<Func<City, bool>> filter) where U : City
        {
            return await _context.Cities.AnyAsync(_mapper.Map<Expression<Func<DbCity, bool>>>(filter));
        }

        public async Task AddAsync(City item, ChangeContext changeContext)
        {
            if (string.IsNullOrEmpty(item.Id))
                item.Id = _context.GetGalaxisId();
            if (item.LastUpdatedTimestamp == null)
                item.LastUpdatedTimestamp = DateTime.Now;
            item.PostalCodes?.ToList().ForEach(x => x.LastUpdatedTimestamp = DateTime.Now);

            var city = _mapper.Map<City, DbCity>(item);

            await _context.Cities.AddAsync(city);

            city.AddressPath = new DbAddressPath
            {
                ChildId = item.Id,
                ChildLevel = (int)LevelDescriptionType.City,
                ParentId = item.AddressLevel3?.Id ?? item.CountryId,
                ParentLevel = GetCityCurrentParentLevelDescriptionType(item),
                LastUpdatedTimestamp = item.LastUpdatedTimestamp
            };

            var message = new DbOutboxMessage
            {
                CorrelationId = changeContext.CorrelationId,
                SiteOriginId = changeContext.SiteOriginId,
                Exchange = OutboxMessageKeys.Exchange,
                Context = OutboxMessageKeys.Context,
                IsReliable = true,
                RoutingKey = "ref.city.created",
                Data = JsonSerializer.Serialize(new RessourceCreated { Data = item }, _jsonOptions),
            };
            await _context.OutboxMessages.AddAsync(message);
        }

        private int GetCityCurrentParentLevelDescriptionType(City city) {
            if (city.AddressLevel3 != null)
                return (int)LevelDescriptionType.Level3;
            return (int)LevelDescriptionType.Country;
        }

        public async Task UpdateAsync(City item, ChangeContext changeContext)
        {
            var city = await _context.Cities.FindAsync(item.Id);
            var oldItem = _mapper.Map<DbCity, City>(city);

            if (item.LastUpdatedTimestamp == null)
                item.LastUpdatedTimestamp = DateTime.Now;
            item.PostalCodes?.ToList().ForEach(x => x.LastUpdatedTimestamp ??= DateTime.Now);

            var oldParentId = oldItem.AddressLevel3 != null ? oldItem.AddressLevel3.Id : oldItem.CountryId;
            var newParentId = item.AddressLevel3 != null ? item.AddressLevel3.Id : item.CountryId;
            _mapper.Map(item, city);

            var addressPath = city.AddressPath;

            // We must clear the link between addressPath and city/country
            // so ef core recognize the ressource as being disposable
            addressPath?.ClearDbLink();

            _context.AddressPath.Remove(addressPath);
            // we finally remove the addresspath
            await ApplyAsync();

            // And create the new one
            city.AddressPath = new DbAddressPath()
            {
                ChildLevel = (decimal)AddressLevelEnum.City,
                ChildId = city.Id,
                ParentId = newParentId,
                ParentLevel = item.AddressLevel3 != null ? (decimal)AddressLevelEnum.Level3 : (decimal)AddressLevelEnum.Country,
                LastUpdatedTimestamp = item.LastUpdatedTimestamp
            };

            var message = new DbOutboxMessage
            {
                CorrelationId = changeContext.CorrelationId,
                SiteOriginId = changeContext.SiteOriginId,
                Exchange = OutboxMessageKeys.Exchange,
                Context = OutboxMessageKeys.Context,
                IsReliable = true,
                RoutingKey = "ref.city.updated",
                Data = JsonSerializer.Serialize(new RessourceUpdated { OldData = oldItem, NewData = item }, _jsonOptions),
            };
            await _context.OutboxMessages.AddAsync(message);
        }

        public async Task RemoveAsync(City item, ChangeContext changeContext)
        {
            var city = await _context.Cities.FindAsync(item.Id);
            if (city.PostalCodes != null)
            {
                _context.PostalCodes.RemoveRange(city.PostalCodes);
            }
            _context.Cities.Remove(city);
            if (city.AddressPath != null)
            {
                _context.AddressPath.Remove(city.AddressPath);
            }

            var message = new DbOutboxMessage
            {
                CorrelationId = changeContext.CorrelationId,
                SiteOriginId = changeContext.SiteOriginId,
                Exchange = OutboxMessageKeys.Exchange,
                Context = OutboxMessageKeys.Context,
                IsReliable = true,
                RoutingKey = "ref.city.deleted",
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
