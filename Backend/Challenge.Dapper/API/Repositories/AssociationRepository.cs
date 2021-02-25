using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Core.Dto;
using Core.Models;
using Dapper;
using Infrastructure.Interfaces;
using Npgsql;

namespace API.Repositories
{
	public class AssociationRepository : IAssociationRepository
	{
		public async Task<List<Teach>> GetAllTeaches()
		{
			await using var connection =
				new NpgsqlConnection(Environment.GetEnvironmentVariable("DATABASE"));
			await connection.OpenAsync();
			var result =
				await connection.QueryAsync<Teach>("SELECT id, teacher_Id teacherId, course_Id courseId FROM _teaches");
			await connection.CloseAsync();
			return (List<Teach>) result;
		}
		
		public async Task<PagedData<Teach>> GetTeachesPaged(int page, int pageSize)
		{
			await using var connection =
				new NpgsqlConnection(Environment.GetEnvironmentVariable("DATABASE"));
			
			await connection.OpenAsync();
			
			int skip = (page - 1) * pageSize;
			var items =
				await connection.QueryAsync<Teach>("SELECT id, teacher_Id teacherId, course_Id courseId FROM _teaches OFFSET @skip LIMIT @pageSize", 
					new {skip, pageSize});
			var total = await connection.QueryFirstAsync<int>("SELECT COUNT(*) FROM teaches");
			
			await connection.CloseAsync();

			var result = new PagedData<Teach>
			{
				TotalItems = total,
				PageSize = pageSize,
				Value = (List<Teach>) items,
			};
			
			return result;
		}

		public async Task<Teach> AddTeacherToCourse(TeacherCourseAssociation association)
		{
			await using var connection =
				new NpgsqlConnection(Environment.GetEnvironmentVariable("DATABASE"));

			await connection.OpenAsync();
			
			var result = await connection.QueryFirstAsync<Teach>(
				"INSERT INTO _teaches(teacher_id, course_id) VALUES (@teacher, @course)", 
				new {teacher = association.TeacherId, course = association.CourseId});
			
			await connection.CloseAsync();
			return result;
		}

		public async Task<List<Attend>> GetAllAttends()
		{
			await using var connection =
				new NpgsqlConnection(Environment.GetEnvironmentVariable("DATABASE"));
			await connection.OpenAsync();
			var result =
				await connection.QueryAsync<Attend>(
					"SELECT id, student_Id studentId, course_Id courseId FROM _attends");
			await connection.CloseAsync();
			
			return (List<Attend>) result;
		}
		
		public async Task<PagedData<Attend>> GetAttendsPaged(int page, int pageSize)
		{
			await using var connection =
				new NpgsqlConnection(Environment.GetEnvironmentVariable("DATABASE"));
			
			await connection.OpenAsync();
			
			int skip = (page - 1) * pageSize;
			var items =
				await connection.QueryAsync<Attend>("SELECT id, student_Id studentId, course_Id courseId FROM _attends OFFSET @skip LIMIT @pageSize", 
					new {skip, pageSize});
			var total = await connection.QueryFirstAsync<int>("SELECT COUNT(*) FROM teaches");
			
			await connection.CloseAsync();

			var result = new PagedData<Attend>
			{
				TotalItems = total,
				PageSize = pageSize,
				Value = (List<Attend>) items,
			};
			
			return result;
		}

		public async Task<Attend> AddStudentToCourse(StudentCourseAssociation association)
		{
			await using var connection =
				new NpgsqlConnection(Environment.GetEnvironmentVariable("DATABASE"));

			await connection.OpenAsync();
			
			var result = await connection.QueryFirstAsync<Attend>(
				"INSERT INTO _attends(student_id, course_id) VALUES (@student, @course)", 
				new {student = association.StudentId, course = association.CourseId});
			
			await connection.CloseAsync();
			return result;
		}

		public async Task RemoveTeacherFromCourse(TeacherCourseAssociation association)
		{
			await using var connection =
				new NpgsqlConnection(Environment.GetEnvironmentVariable("DATABASE"));

			await connection.OpenAsync();
			
			var result = await connection.QueryFirstAsync<Teach>(
				"DELETE FROM _teaches WHERE teacher_id = @teacher AND course_id = @course", 
				new {teacher = association.TeacherId, course = association.CourseId});
			
			await connection.CloseAsync();
		}

		public async Task RemoveStudentFromCourse(StudentCourseAssociation association)
		{
			await using var connection =
				new NpgsqlConnection(Environment.GetEnvironmentVariable("DATABASE"));

			await connection.OpenAsync();
			var result = await connection.QueryFirstAsync<Attend>(
				"DELETE FROM _attends WHERE student_id = @student AND course_id = @course", 
				new {student = association.StudentId, course = association.CourseId});
			await connection.CloseAsync();
		}
	}
}