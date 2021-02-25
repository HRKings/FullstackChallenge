using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Utils
{
	public static class Extensions
	{
		public static async Task<List<T>> GetPaged<T>(this IQueryable<T> query,
			int page, int pageSize, Expression<Func<T, int>> keySelector) where T : class
		{
			int skip = (page - 1) * pageSize;
			return await query.OrderBy(keySelector).Skip(skip).Take(pageSize).ToListAsync();
		}
	}
}