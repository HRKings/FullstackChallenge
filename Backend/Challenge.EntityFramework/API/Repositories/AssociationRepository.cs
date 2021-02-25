using System;
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
	public class AssociationRepository : IAssociationRepository
	{
		private ChallengeDbContext _dbContext;
		
		public AssociationRepository(ChallengeDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		
		public Task<List<Teach>> GetAllTeaches()
		{
			return _dbContext.Teaches.ToListAsync();
		}

		public async Task<PagedData<List<Teach>>> GetTeachesPaged(int page, int pageSize)
		{
			var items = await _dbContext.Teaches.GetPaged(page, pageSize, teacher => teacher.Id);
			int quantity = await _dbContext.Teaches.CountAsync();

			return new PagedData<List<Teach>>
			{
				TotalItems = quantity,
				PageSize = pageSize,
				Value = items
			};
		}

		public async Task<Teach> AddTeacherToCourse(TeacherCourseAssociation association)
		{
			if (await _dbContext.Teachers.FindAsync(association.TeacherId) == null)
				return null;

			if (await _dbContext.Courses.FindAsync(association.CourseId) == null)
				return null;


			var result = await _dbContext.Teaches.AddAsync(new Teach
			{
				TeacherId = association.TeacherId,
				CourseId = association.CourseId
			});

			await _dbContext.SaveChangesAsync();

			return result.Entity;
		}

		public async Task RemoveTeacherFromCourse(TeacherCourseAssociation association)
		{
			if (await _dbContext.Teachers.FindAsync(association.TeacherId) == null)
				return;

			if (await _dbContext.Courses.FindAsync(association.CourseId) == null)
				return;
			
			var result = await _dbContext.Teaches.FirstAsync(teach =>
					teach.TeacherId == association.TeacherId && teach.CourseId == association.CourseId);
			_dbContext.Entry(result).State = EntityState.Deleted;

			await _dbContext.SaveChangesAsync();
		}

		public Task<List<Attend>> GetAllAttends()
		{
			return _dbContext.Attends.ToListAsync();
		}

		public async Task<PagedData<List<Attend>>> GetAttendsPaged(int page, int pageSize)
		{
			var items = await _dbContext.Attends.GetPaged(page, pageSize, teacher => teacher.Id);
			int quantity = await _dbContext.Attends.CountAsync();

			return new PagedData<List<Attend>>
			{
				TotalItems = quantity,
				PageSize = pageSize,
				Value = items
			};
		}

		public async Task<Attend> AddStudentToCourse(StudentCourseAssociation association)
		{
			if (await _dbContext.Students.FindAsync(association.StudentId) == null)
				return null;

			if (await _dbContext.Courses.FindAsync(association.CourseId) == null)
				return null;


			var result = await _dbContext.Attends.AddAsync(new Attend
			{
				StudentId = association.StudentId,
				CourseId = association.CourseId
			});

			await _dbContext.SaveChangesAsync();

			return result.Entity;
		}

		public async Task RemoveStudentFromCourse(StudentCourseAssociation association)
		{
			if (await _dbContext.Students.FindAsync(association.StudentId) == null)
				return;

			if (await _dbContext.Courses.FindAsync(association.CourseId) == null)
				return;
			
			var result = await _dbContext.Attends.FirstAsync(attend =>
				attend.StudentId == association.StudentId && attend.CourseId == association.CourseId);
			_dbContext.Entry(result).State = EntityState.Deleted;
			
			await _dbContext.SaveChangesAsync();
		}
	}
}