using System.Linq.Expressions;

namespace fw_shop_api.Models.Util
{
    public static class CustomExpressionFilter<T> where T : class
    {
        public class ExpressionFilter
        {
            public string ColumnName { get; set; } = string.Empty;
            public string Value { get; set; } = string.Empty;
        }

        public static Expression<Func<T, bool>> CustomFilter(List<ColumnFilter> columnFilters, string className)
        {
            Expression<Func<T, bool>> filters = null;
            try
            {
                var expressionFilters = new List<ExpressionFilter>();
                foreach (var item in columnFilters)
                {
                    expressionFilters.Add(new ExpressionFilter() {ColumnName = item.Id, Value = item.Value});
                }

                var parameter = Expression.Parameter(typeof(T), className);
                Expression filterExpression = null;
                foreach (var filter in expressionFilters)
                {
                    var property = Expression.Property(parameter, filter.ColumnName);
                    Expression comparison;
                    if (property.Type == typeof(string))
                    {
                        var constant = Expression.Constant(filter.Value);
                        comparison = Expression.Call(property, "Contains", Type.EmptyTypes, constant);
                    }
                    else if (property.Type == typeof(double))
                    {
                        var constant = Expression.Constant(Convert.ToDouble(filter.Value));
                        comparison = Expression.Equal(property, constant);
                    }
                    else if (property.Type == typeof(Guid))
                    {
                        var constant = Expression.Constant(Guid.Parse(filter.Value));
                        comparison = Expression.Equal(property, constant);
                    }
                    else
                    {
                        var constant = Expression.Constant(Convert.ToInt32(filter.Value));
                        comparison = Expression.Equal(property, constant);
                    }

                    filterExpression = filterExpression == null ? comparison : Expression.And(filterExpression, comparison);
                }

                filters = Expression.Lambda<Func<T, bool>>(filterExpression, parameter);
            }
            catch (Exception) {filters = null;}

            return filters;
        }
    }
}