using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Dto;
using Core.Models;

namespace Infrastructure.Interfaces
{
	public interface IAssociationRepository
	{
		public Task<List<Teach>> GetAllTeaches();
		public Task<PagedData<Teach>> GetTeachesPaged(int page, int pageSize);

		public Task<Teach> AddTeacherToCourse(TeacherCourseAssociation association);
		
		public Task RemoveTeacherFromCourse(TeacherCourseAssociation association);

		public Task<List<Attend>> GetAllAttends();
		public Task<PagedData<Attend>> GetAttendsPaged(int page, int pageSize);

		public Task<Attend> AddStudentToCourse(StudentCourseAssociation association);
		
		public Task RemoveStudentFromCourse(StudentCourseAssociation association);
	}
}