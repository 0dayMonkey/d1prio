using AddressLocation.Domain.Enums;
using AddressLocation.Domain.Models;
using AddressLocation.Infrastructure.Database.Models;
using AutoMapper;
using Framework.Models;

namespace AddressLocation.Infrastructure
{
    public class AddressLocationMappingProfile: Profile
    {
        public AddressLocationMappingProfile()
        {
            CreateMap<City, DbCity>()
                .AfterMap((src, dest) => dest.PostalCodes?.ToList().ForEach(x => x.CityId = src.Id));

            CreateMap<DbCity, City>()
                .ForMember(dest => dest.CountryId, opt => opt.MapFrom(src => 
                    src.AddressPath == null 
                        ? null 
                        : (src.AddressPath.ParentLevel == (int)AddressLevelEnum.Country ? src.AddressPath.ParentId
                            : (src.AddressPath.Parent == null ? null 
                                : (src.AddressPath.Parent.ParentLevel == (int)AddressLevelEnum.Country ? src.AddressPath.Parent.ParentId
                                    : (src.AddressPath.Parent.Parent == null ? null 
                                        : (src.AddressPath.Parent.Parent.ParentLevel == (int)AddressLevelEnum.Country ? src.AddressPath.Parent.Parent.ParentId
                                            : (src.AddressPath.Parent.Parent.Parent == null ? null 
                                                : (src.AddressPath.Parent.Parent.Parent.ParentLevel == (int)AddressLevelEnum.Country ? src.AddressPath.Parent.Parent.Parent.ParentId
                                                    : (src.AddressPath.Parent.Parent.Parent.Parent == null ? null 
                                                        : (src.AddressPath.Parent.Parent.Parent.Parent.ParentLevel == (int)AddressLevelEnum.Country ? src.AddressPath.Parent.Parent.Parent.Parent.ParentId
                                                            : null)))))))))))
                .ForMember(dest => dest.AddressLevel3, opt => opt.MapFrom((src, dest) => src.AddressPath?.ParentLevel == (int)AddressLevelEnum.Level3 ? src.AddressPath.ParentAddressLevel3 : null))
                .ForMember(dest => dest.AddressLevel2, opt => opt.MapFrom((src, dest) => src.AddressPath?.ParentLevel == (int)AddressLevelEnum.Level2 ? src.AddressPath.ParentAddressLevel2 : (src.AddressPath?.Parent?.ParentLevel == (int)AddressLevelEnum.Level2 ? src.AddressPath.Parent.ParentAddressLevel2 : null)))
                .ForMember(dest => dest.AddressLevel1, opt => opt.MapFrom((src, dest) => src.AddressPath?.ParentLevel == (int)AddressLevelEnum.Level1 ? src.AddressPath.ParentAddressLevel1 : (src.AddressPath?.Parent?.ParentLevel == (int)AddressLevelEnum.Level1 ? src.AddressPath.ParentAddressLevel1 : src.AddressPath?.Parent?.Parent?.ParentLevel == (int)AddressLevelEnum.Level1 ? src.AddressPath.Parent.Parent.ParentAddressLevel1 : null)));
            CreateMap<DbAddressLevelStructure, AddressLevelStructure>()
                .ForMember(dest => dest.AddressLevel1Description, opt => opt.MapFrom((src, dst) =>
                {
                    var item = new AddressLevelDescription()
                    {
                        IsAbbreviationUsed = src.AddressLevel1AbbreviationId != -1,
                        IsDescriptionUsed = src.AddressLevel1DescriptionId != -1,
                    };
                    if (src.AddressLevel1AbbreviationId != -1 || src.AddressLevel1DescriptionId != -1)
                    {
                        item.Label = (src.AddressLevel1DescriptionId == -1 ? src.AddressLevel1Abbreviation : src.AddressLevel1Description)?.Label;
                        item.Id = (src.AddressLevel1DescriptionId == -1 ? src.AddressLevel1Abbreviation : src.AddressLevel1Description)?.Id;
                    }
                    return item;
                }))
                .ForMember(dest => dest.AddressLevel2Description, opt => opt.MapFrom((src, dst) =>
                {
                    var item = new AddressLevelDescription()
                    {
                        IsAbbreviationUsed = src.AddressLevel2AbbreviationId != -1,
                        IsDescriptionUsed = src.AddressLevel2DescriptionId != -1,
                    };
                    if (src.AddressLevel2AbbreviationId != -1 || src.AddressLevel2DescriptionId != -1)
                    {
                        item.Label = (src.AddressLevel2DescriptionId == -1 ? src.AddressLevel2Abbreviation : src.AddressLevel2Description)?.Label;
                        item.Id = (src.AddressLevel2DescriptionId == -1 ? src.AddressLevel2Abbreviation : src.AddressLevel2Description)?.Id;
                    }
                    return item;
                }))
                .ForMember(dest => dest.AddressLevel3Description, opt => opt.MapFrom((src, dst) =>
                {
                    var item = new AddressLevelDescription()
                    {
                        IsAbbreviationUsed = src.AddressLevel3AbbreviationId != -1,
                        IsDescriptionUsed = src.AddressLevel3DescriptionId != -1,
                    };
                    if (src.AddressLevel3AbbreviationId != -1 || src.AddressLevel3DescriptionId != -1)
                    {
                        item.Label = (src.AddressLevel3DescriptionId == -1 ? src.AddressLevel3Abbreviation : src.AddressLevel3Description)?.Label;
                        item.Id = (src.AddressLevel3DescriptionId == -1 ? src.AddressLevel3Abbreviation : src.AddressLevel3Description)?.Id;
                    }
                    return item;
                }));
            CreateMap<AddressLevelStructure, DbAddressLevelStructure>()
                .ForMember(dest => dest.AddressLevel1AbbreviationId, opt => opt.MapFrom((src, dst) =>
                    {
                        if (src.AddressLevel1Description?.IsAbbreviationUsed == true)
                            return src.AddressLevel1Description?.Id;
                        return -1;
                    }))
                .ForMember(dest => dest.AddressLevel2AbbreviationId, opt => opt.MapFrom((src, dst) =>
                {
                    if (src.AddressLevel2Description?.IsAbbreviationUsed == true)
                        return src.AddressLevel2Description?.Id;
                    return -1;
                }))
                .ForMember(dest => dest.AddressLevel3AbbreviationId, opt => opt.MapFrom((src, dst) =>
                {
                    if (src.AddressLevel3Description?.IsAbbreviationUsed == true)
                        return src.AddressLevel3Description?.Id;
                    return -1;
                }))
                .ForMember(dest => dest.AddressLevel1DescriptionId, opt => opt.MapFrom((src, dst) =>
                {
                    if (src.AddressLevel1Description?.IsDescriptionUsed == true)
                        return src.AddressLevel1Description?.Id;
                    return -1;
                }))
                .ForMember(dest => dest.AddressLevel2DescriptionId, opt => opt.MapFrom((src, dst) =>
                {
                    if (src.AddressLevel2Description?.IsAbbreviationUsed == true)
                        return src.AddressLevel2Description?.Id;
                    return -1;
                }))
                .ForMember(dest => dest.AddressLevel3DescriptionId, opt => opt.MapFrom((src, dst) =>
                {
                    if (src.AddressLevel3Description?.IsAbbreviationUsed == true)
                        return src.AddressLevel1Description?.Id;
                    return -1;
                }))
                .ForPath(dest => dest.AddressLevel1Abbreviation, opt => opt.Ignore())
                .ForPath(dest => dest.AddressLevel2Abbreviation, opt => opt.Ignore())
                .ForPath(dest => dest.AddressLevel3Abbreviation, opt => opt.Ignore())
                .ForPath(dest => dest.AddressLevel1Description, opt => opt.Ignore())
                .ForPath(dest => dest.AddressLevel2Description, opt => opt.Ignore())
                .ForPath(dest => dest.AddressLevel3Description, opt => opt.Ignore())
                .ForPath(dest => dest.Country, opt => opt.Ignore());

            CreateMap<DbAddressLevel1, AddressLevel1>()
                .ForMember(dest => dest.ParentId, opt => {
                    opt.PreCondition(src => src != null && src.AsChildAddressPath != null);
                    opt.MapFrom(src => src.AsChildAddressPath.ParentId); })
                .ForMember(dest => dest.ParentLevel, opt =>
                {
                    opt.PreCondition(src => src != null && src.AsChildAddressPath != null);
                    opt.MapFrom(src => (AddressLevelEnum)src.AsChildAddressPath.ParentLevel);
                });
            CreateMap<AddressLevel1, DbAddressLevel1>()
                .ForMember(dest => dest.AsChildAddressPath, opt =>
                {
                    opt.PreCondition((src, dst) => { return src != null; });
                    opt.MapFrom(src =>
                        new DbAddressPath()
                        {
                            ParentId = src.ParentId,
                            ChildId = src.Id,
                            ParentLevel = (int)src.ParentLevel,
                            ChildLevel = (int)src.Level
                        }
                    );
                });

            CreateMap<DbAddressLevel2, AddressLevel2>()
                .ForMember(dest => dest.ParentId, opt => {
                    opt.PreCondition(src => src != null && src.AsChildAddressPath != null);
                    opt.MapFrom(src => src.AsChildAddressPath.ParentId);
                })
                .ForMember(dest => dest.ParentLevel, opt =>
                {
                    opt.PreCondition(src => src != null && src.AsChildAddressPath != null);
                    opt.MapFrom(src => (AddressLevelEnum)src.AsChildAddressPath.ParentLevel);
                });
            CreateMap<AddressLevel2, DbAddressLevel2>()
                .ForMember(dest => dest.AsChildAddressPath, opt =>
                {
                    opt.PreCondition((src, dst) => { return src != null; });
                    opt.MapFrom(src =>
                        new DbAddressPath()
                        {
                            ParentId = src.ParentId,
                            ChildId = src.Id,
                            ParentLevel = (int)src.ParentLevel,
                            ChildLevel = (int)src.Level
                        }
                    );
                });
            CreateMap<DbAddressLevel3, AddressLevel3>()
                .ForMember(dest => dest.ParentId, opt =>
                {
                    opt.PreCondition(src => src != null && src.AsChildAddressPath != null);
                    opt.MapFrom(src => src.AsChildAddressPath.ParentId);
                })
                .ForMember(dest => dest.ParentLevel, opt =>
                {
                    opt.PreCondition(src => src != null && src.AsChildAddressPath != null);
                    opt.MapFrom(src => (AddressLevelEnum)src.AsChildAddressPath.ParentLevel);
                });
            CreateMap<AddressLevel3, DbAddressLevel3>()
                .ForMember(dest => dest.AsChildAddressPath, opt =>
                {
                    opt.PreCondition((src, dst) => { return src != null; });
                    opt.MapFrom(src =>
                        new DbAddressPath()
                        {
                            ParentId = src.ParentId,
                            ChildId = src.Id,
                            ParentLevel = (int)src.ParentLevel,
                            ChildLevel = (int)src.Level
                        }
                    );
                });

            CreateMap<PostalCode, DbPostalCode>().ReverseMap();
            CreateMap<Country, DbCountry>().ReverseMap();

            CreateMap<Filter<City>, Filter<DbCity>>();
            CreateMap<Sort<City>, Sort<DbCity>>();
            CreateMap<Search<City>, Search<DbCity>>();

            CreateMap<Filter<Country>, Filter<DbCountry>>();
            CreateMap<Sort<Country>, Sort<DbCountry>>();
            CreateMap<Search<Country>, Search<DbCountry>>();

        }
    }
}
