using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Challenge_EF.Utils
{
	public static class Extensions
	{
		public static async Task<List<T>> GetPaged<T>(this IQueryable<T> query, 
			int page, int pageSize) where T : class
		{
			int skip = (page - 1) * pageSize;     
			return await query.Skip(skip).Take(pageSize).ToListAsync();
		}
	}
}