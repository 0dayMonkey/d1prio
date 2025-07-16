namespace OpenMarketingApi.Models
{

    public enum LocationTypeEnum
    {
        [EnumMember(Value = "Unknown")]
        Unknown, 
        [EnumMember(Value = "EGM")]
        EGM,
        [EnumMember(Value = "Reception")]
        Reception,
        [EnumMember(Value = "Table")]
        Table,
        [EnumMember(Value = "Cage")]
        Cage,
        [EnumMember(Value = "Shop")]
        Shop,
        [EnumMember(Value = "Kiosk")]
        Kiosk,
        [EnumMember(Value = "POS")]
        POS,
        [EnumMember(Value = "Service")]
        Service,
        [EnumMember(Value = "External")]
        External
    }

}
