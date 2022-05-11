using System;
using System.Linq;
using System.Linq.Expressions;

namespace BikeScanner.Data.Postgre.Extensions
{
	internal static class QueryableExtensions
	{
		public static IQueryable<T> WhereIf<T>(this IQueryable<T> queryable, Expression<Func<T, bool>> expr, bool condition) =>
			condition ? queryable.Where(expr) : queryable;

	}
}

