using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Dto;
using Core.Models;
using Infrastructure.Database.Context;

namespace Infrastructure.Interfaces
{
	public interface IAssociationRepository
	{
		public Task<List<Teach>> GetAllTeaches();
		public Task<PagedData<List<Teach>>> GetTeachesPaged(int page, int pageSize);

		public Task<Teach> AddTeacherToCourse(TeacherCourseAssociation association);
		
		public Task RemoveTeacherFromCourse(TeacherCourseAssociation association);

		public Task<List<Attend>> GetAllAttends();
		public Task<PagedData<List<Attend>>> GetAttendsPaged(int page, int pageSize);

		public Task<Attend> AddStudentToCourse(StudentCourseAssociation association);
		
		public Task RemoveStudentFromCourse(StudentCourseAssociation association);
	}
}