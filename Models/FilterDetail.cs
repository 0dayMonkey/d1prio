namespace ApiTools.Models
{
    public class FilterDetail
    {
        public string? Value { get; set; }
        public FilterMatchMode? MatchMode { get; set; }
        public FilterOperator? Operator { get; set; }
        public FilterDetail(string? value, FilterMatchMode? matchMode, FilterOperator? @operator)
        {
            Value = value;
            MatchMode = matchMode;
            Operator = @operator;
        }

        public (bool orOperator, string condition) CreateClauses(string fieldName) => (
            Operator == FilterOperator.Or || MatchMode == FilterMatchMode.In,
            CreateDetailClause(fieldName)
        );

        public string CreateDetailClause(string fieldName) =>
            MatchMode switch
            {
                FilterMatchMode.StartsWith => $"{fieldName}.ToLower().StartsWith(\"{Value}\".ToLower())",
                FilterMatchMode.Contains => $"{fieldName}.ToLower().Contains(\"{Value}\".ToLower())",
                FilterMatchMode.NotContains => $"!{fieldName}.ToLower().Contains(\"{Value}\".ToLower())",
                FilterMatchMode.EndsWith => $"{fieldName}.ToLower().EndsWith(\"{Value}\".ToLower())",
                FilterMatchMode.Equals => $"{fieldName}.ToLower() = \"{Value}\".ToLower()",
                FilterMatchMode.NotEquals => $"{fieldName}.ToLower() != \"{Value}\".ToLower()",
                FilterMatchMode.In or FilterMatchMode.DateIs => $"{fieldName} = \"{Value}\"",
                FilterMatchMode.DateIsNot => $"{fieldName} != \"{Value}\"",
                FilterMatchMode.Lt or FilterMatchMode.DateBefore => $"{fieldName} < \"{Value}\"",
                FilterMatchMode.Gt or FilterMatchMode.DateAfter => $"{fieldName} > \"{Value}\"",
                FilterMatchMode.Lte => $"{fieldName} <= \"{Value}\"",
                FilterMatchMode.Gte => $"{fieldName} >= \"{Value}\"",
                _ => string.Empty,
            };
    }
}
