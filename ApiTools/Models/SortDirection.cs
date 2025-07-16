using System.Text.Json.Serialization;
using ApiTools.Serialization;

namespace ApiTools.Models
{
    [JsonConverter(typeof(EnumModeConverter<SortDirection>))]
    public enum SortDirection { asc, desc };
}
