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
	public class StudentRepository : IStudentRepository
	{
		private ChallengeDbContext _dbContext;
		public StudentRepository(ChallengeDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		
		public Task<List<Student>> GetAll()
		{
			return _dbContext.Students.ToListAsync();
		}

		public async Task<PagedData<List<Student>>> GetPaged(int page, int pageSize)
		{
			var items = await _dbContext.Students.GetPaged(page, pageSize, student => student.Id);
			int quantity = await _dbContext.Students.CountAsync();

			return new PagedData<List<Student>>()
			{
				TotalItems = quantity,
				PageSize = pageSize,
				Value = items,
			};
		}

		public ValueTask<Student> GetFromId(int id)
		{
			return _dbContext.Students.FindAsync(id);
		}

		public async Task<List<Course>> GetCoursesFromId(int id)
		{
			return await _dbContext.Attends.OrderBy(attend => attend.CourseId).Where(attend => attend.StudentId == id)
				.Select(teach => teach.Course).ToListAsync();
		}

		public async Task<Student> Create(NameData data)
		{
			var result = await _dbContext.Students.AddAsync(new Student
			{
				Name = data.Name
			});

			await _dbContext.SaveChangesAsync();

			return result.Entity;
		}

		public async Task<Student> Update(int id, string name)
		{
			var result = await _dbContext.Students.FindAsync(id);

			if (result == null)
				return null;
			
			result.Name = name;

			_dbContext.Students.Update(result);
			await _dbContext.SaveChangesAsync();

			return result;
		}

		public Task Delete(int id)
		{
			var result = new Student {Id = id};
			_dbContext.Entry(result).State = EntityState.Deleted;
			return _dbContext.SaveChangesAsync();
		}
	}
}