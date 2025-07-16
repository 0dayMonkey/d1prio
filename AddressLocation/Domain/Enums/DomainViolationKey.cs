namespace AddressLocation.Domain.Enums
{
    public enum DomainViolationKey
    {
        AddressLevelIsUnknown,
        AddressLevelStructureAlreadyExist,
        AddressLevelStructureDoNotExist,
        AddressLevel1DoNotExist,
        AddressLevel1LinkToChildren,
        AddressLevel2DoNotExist,
        AddressLevel2LinkToChildren,
        AddressLevel3DoNotExist,
        AddressLevel3LinkToChildren,
        AddressLevelStructureDoNotAllowLevel1,
        AddressLevelStructureDoNotAllowLevel2,
        AddressLevelStructureDoNotAllowLevel3,
        AddressLevelStructureRequireLevel3,
        AddressLevelStructureDoNotAllowThisLevel1Configuration,
        AddressLevelStructureDoNotAllowThisLevel2Configuration,
        AddressLevelStructureDoNotAllowThisLevel3Configuration,
        AddressLevelAlreadyExist,
        CityAlreadyExist,
        AddressLevelStructureDataLogicViolation,
        AddressLevelParentIdDoesNotExist,
        LocationIdAlreadyUsed,
        CountryLinkToChildren
    }
}
