using System.Text.Json.Serialization;
using ApiTools.Serialization;

namespace ApiTools.Models
{
    [JsonConverter(typeof(EnumModeConverter<FilterMatchMode>))]
    public enum FilterMatchMode
    {
        StartsWith,
        Contains,
        NotContains,
        EndsWith,
        Equals,
        NotEquals,
        In,
        Lt,  //less than
        Lte, //less than or equal to
        Gt,  //greater than
        Gte, //greater than or equal to
        Between,
        Is,
        IsNot,
        Before,
        After,
        DateIs,
        DateIsNot,
        DateBefore,
        DateAfter,
    }
}
