namespace OpenMarketingApi.Interfaces.Common
{
    public interface IServiceAction
    {
        [DataMember(Name = "lastUpdatedTimestamp")]
        DateTime? LastUpdatedTimestamp { get; set; }
    }
}
