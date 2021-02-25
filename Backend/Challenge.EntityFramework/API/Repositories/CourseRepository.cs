using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Dto;
using Core.Models;
using Infrastructure.Database.Context;
using Infrastructure.Interfaces;
using Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
	public class CourseRepository : ICourseRepository
	{
		private ChallengeDbContext _dbContext;
		
		public CourseRepository(ChallengeDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		
		public Task<List<Course>> GetAll()
		{
			return _dbContext.Courses.ToListAsync();
		}

		public async Task<PagedData<List<Course>>> GetPaged(int page, int pageSize)
		{
			var items = await _dbContext.Courses.GetPaged(page, pageSize, teacher => teacher.Id);
			int quantity = await _dbContext.Courses.CountAsync();

			return new PagedData<List<Course>>()
			{
				TotalItems = quantity,
				PageSize = pageSize,
				Value = items,
			};
		}

		public ValueTask<Course> GetFromId(int id)
		{
			return _dbContext.Courses.FindAsync(id);
		}

		public async Task<Course> Create(NameData data)
		{
			var result = await _dbContext.Courses.AddAsync(new Course
			{
				Name = data.Name
			});

			await _dbContext.SaveChangesAsync();

			return result.Entity;
		}

		public async Task<Course> Update(int id, string name)
		{
			var result = await _dbContext.Courses.FindAsync(id);

			if (result == null)
				return null;
			
			result.Name = name;

			_dbContext.Courses.Update(result);
			await _dbContext.SaveChangesAsync();

			return result;
		}

		public Task Delete(int id)
		{
			var result = new Course {Id = id};
			_dbContext.Entry(result).State = EntityState.Deleted;
			return _dbContext.SaveChangesAsync();
		}
	}
}