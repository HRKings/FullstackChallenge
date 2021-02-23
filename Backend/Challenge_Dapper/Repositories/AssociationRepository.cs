using System.Collections.Generic;
using System.Threading.Tasks;
using Challenge_Dapper.Models;
using Dapper;
using Npgsql;

namespace Challenge_Dapper.Repositories
{
	public class AssociationRepository
	{
		public static async Task<IEnumerable<Teach>> GetAllTeachers()
		{
			await using var connection =
				new NpgsqlConnection("Host=localhost;Database=challenge;Username=postgres;Password=");
			await connection.OpenAsync();
			var result = await connection.QueryAsync<Teach>("SELECT * FROM _teaches");
			await connection.CloseAsync();
			return result;
		}
		
		public static async Task<Teach> InsertTeacher(int teacher, int course)
		{
			await using var connection =
				new NpgsqlConnection("Host=localhost;Database=challenge;Username=postgres;Password=");

			await connection.OpenAsync();
			var result = await connection.QueryFirstAsync<Teach>("INSERT INTO _teaches(teacher_id, course_id) VALUES (@teacher, @course)", new {teacher, course});
			await connection.CloseAsync();
			return result;
		}
		
		public static async Task<IEnumerable<Attend>> GetAllStudents()
		{
			await using var connection =
				new NpgsqlConnection("Host=localhost;Database=challenge;Username=postgres;Password=");
			await connection.OpenAsync();
			var result = await connection.QueryAsync<Attend>("SELECT * FROM __attends");
			await connection.CloseAsync();
			return result;
		}
		
		public static async Task<Attend> InsertStudent(int student, int course)
		{
			await using var connection =
				new NpgsqlConnection("Host=localhost;Database=challenge;Username=postgres;Password=");

			await connection.OpenAsync();
			var result = await connection.QueryFirstAsync<Attend>("INSERT INTO _attends(student_id, course_id) VALUES (@student, @course)", new {student, course});
			await connection.CloseAsync();
			return result;
		}
		
		public static async Task<Teach> DeleteTeacher(int teacher, int course)
		{
			await using var connection =
				new NpgsqlConnection("Host=localhost;Database=challenge;Username=postgres;Password=");

			await connection.OpenAsync();
			var result = await connection.QueryFirstAsync<Teach>("DELETE FROM _teaches WHERE teacher_id = @teacher AND course_id = @course", new {teacher, course});
			await connection.CloseAsync();
			return result;
		}
		
		public static async Task<Attend> DeleteStudent(int student, int course)
		{
			await using var connection =
				new NpgsqlConnection("Host=localhost;Database=challenge;Username=postgres;Password=");

			await connection.OpenAsync();
			var result = await connection.QueryFirstAsync<Attend>("DELETE FROM _attends WHERE student_id = @student AND course_id = @course", new {student, course});
			await connection.CloseAsync();
			return result;
		}
	}
}