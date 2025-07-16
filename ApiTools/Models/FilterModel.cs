using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace ApiTools.Models
{
    public class FilterModel
    {
        public string Field { get; set; }

        public List<FilterModel>? ListFilters { get; set; }

        public List<FilterDetail> Details { get; set; }

        public FilterOperator? Operator { get; set; }

        public FilterModel(string field, List<FilterDetail> details)
        {
            Field = field;
            Details = details;
        }

        public string CreateFilterClause(string fieldName) =>
            ListFilters != null
            ? $"{fieldName}.Any(prop => {GetPredicateString(ListFilters, "prop")})"
            : Details
                .Where(detail => detail.Value != null)
                .Select(detail => detail.CreateClauses(fieldName))
                .Where(detail => !string.IsNullOrEmpty(detail.condition))
                .Aggregate(
                    (orOperator: true, condition: string.Empty),
                    (clause, next) => (
                        next.orOperator,
                        $"{(clause.condition == string.Empty ? string.Empty : $"{clause.condition} {(next.orOperator ? "OR" : "AND")} ")}{next.condition}"
                    ),
                    clause => clause.condition == string.Empty ? string.Empty : $"({clause.condition})"
                );

        public (bool orOperator, string condition) CreateClauses(string? param = null) => (
            Operator != null && Operator == FilterOperator.Or,
            CreateFilterClause(param == null ? Field : $"{param}.{Field}")
        );

        public static string GetPredicateString(List<FilterModel>? filters, string? param = null)
        {
            if (filters == null)
                return "true";

            var predicate = filters
                .Select(filter => filter.CreateClauses(param))
                .Where(clause => !string.IsNullOrEmpty(clause.condition))
                .Aggregate(
                    (orOperator: true, condition: string.Empty),
                    (clause, next) => (
                        next.orOperator,
                        $"{(clause.condition == string.Empty ? string.Empty : $"{clause.condition} {(next.orOperator ? "OR" : "AND")} ")}{next.condition}"
                    ),
                    clause => clause.condition == string.Empty ? string.Empty : $"({clause.condition})"
            );

            if (string.IsNullOrEmpty(predicate))
                return "true";

            return predicate;
        }
    }
}
