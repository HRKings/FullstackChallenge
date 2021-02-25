using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Dto;
using Core.Models;
using Infrastructure.Database.Context;
using Infrastructure.Interfaces;
using Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
	public class TeacherRepository : ITeacherRepository
	{
		private ChallengeDbContext _dbContext;
		
		public TeacherRepository(ChallengeDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		
		public Task<List<Teacher>> GetAll()
		{
			return _dbContext.Teachers.ToListAsync();
		}

		public async Task<PagedData<List<Teacher>>> GetPaged(int page, int pageSize)
		{
			var items = await _dbContext.Teachers.GetPaged(page, pageSize, teacher => teacher.Id);
			int quantity = await _dbContext.Teachers.CountAsync();

			return new PagedData<List<Teacher>>()
			{
				TotalItems = quantity,
				PageSize = pageSize,
				Value = items,
			};
		}

		public ValueTask<Teacher> GetFromId(int id)
		{
			return _dbContext.Teachers.FindAsync(id);
		}

		public async Task<List<Course>> GetCoursesFromId(int id)
		{
			return await _dbContext.Teaches.OrderBy(teach => teach.CourseId).Where(teach => teach.TeacherId == id)
				.Select(teach => teach.Course).ToListAsync();
		}

		public async Task<Teacher> Create(NameData data)
		{
			var result = await _dbContext.Teachers.AddAsync(new Teacher
			{
				Name = data.Name
			});

			await _dbContext.SaveChangesAsync();

			return result.Entity;
		}

		public async Task<Teacher> Update(int id, string name)
		{
			var result = await _dbContext.Teachers.FindAsync(id);

			if (result == null)
				return null;
			
			result.Name = name;

			_dbContext.Teachers.Update(result);
			await _dbContext.SaveChangesAsync();

			return result;
		}

		public Task Delete(int id)
		{
			var result = new Teacher {Id = id};
			_dbContext.Entry(result).State = EntityState.Deleted;
			return _dbContext.SaveChangesAsync();
		}
	}
}