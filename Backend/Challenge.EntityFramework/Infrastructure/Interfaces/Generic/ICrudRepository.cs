using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Dto;
using Infrastructure.Database.Context;

namespace Infrastructure.Interfaces.Generic
{
	public interface ICrudRepository<T>
	{
		public Task<List<T>> GetAll();

		public Task<PagedData<List<T>>> GetPaged(int page, int pageSize);

		public ValueTask<T> GetFromId(int id);
		
		public Task<T> Create( NameData data);

		public Task<T> Update(int id, string name);

		public Task Delete(int id);
	}
}