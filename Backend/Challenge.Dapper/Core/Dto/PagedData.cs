using System.Collections.Generic;

namespace Core.Dto
{
	public struct PagedData<T>
	{
		public int TotalItems { get; set; }
		public int PageSize { get; set; }
		public List<T> Value { get; set; }
	}
}