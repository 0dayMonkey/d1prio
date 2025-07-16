using System.Text.Json.Serialization;
using ApiTools.Serialization;

namespace ApiTools.Models
{
    [JsonConverter(typeof(EnumModeConverter<FilterOperator>))]
    public enum FilterOperator
    {
        And,
        Or
    }
}
