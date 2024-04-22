using System.Linq.Expressions;

namespace fw_shop_api.Models.Util
{
    public static class PaginationQuery
    {
        public static IQueryable<T> CustomQuery<T>(this IQueryable<T> query, Expression<Func<T, bool>> filter = null) where T: class
        {
            if (filter is not null) query = query.Where(filter);
            return query;
        }

        public static IQueryable<T> CustomPagination<T>(this IQueryable<T> query, int? page = 0, int? pageSize = null)
        {
            if (page is not null) query = query.Skip(((int)page - 1 ) * (int)pageSize);
            if (pageSize is not null) query = query.Take((int)pageSize);
            return query;
        } 
    }
}