using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Challenge_Dapper.Models;
using Challenge_Dapper.Repositories;
using Challenge_EF.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Challenge_Dapper.Controllers
{
	/// <response code="401">If you don't have a valid JWT token</response>
	[Authorize]
	[ApiController]
	[Route("[controller]")]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[Produces("application/json")]
	public class AssociationController : ControllerBase
	{
		/// <summary>
		///     Returns all the teachers associated with their courses
		/// </summary>
		/// <response code="200">Returns all the teachers associated with their courses</response>
		/// <response code="400">If there are no teachers associated with any course</response>
		[HttpGet("teacher")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Teach>))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetTeachers([FromQuery] int page = 0, [FromQuery] int pageSize = 10)
		{
			var result = (List<Teach>) await AssociationRepository.GetAllTeachers();

			if (result.Count == 0)
				return BadRequest("No teachers were associated with any courses.");

			return Ok(result);
		}
		
		/// <summary>
		///     Associates a teacher to a course
		/// </summary>
		/// <response code="200">Returns the association between the course and teacher</response>
		/// <response code="400">If there is an error</response>
		[HttpPost("teacher")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Teach))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> AddTeacherToCourse([FromBody] TeacherCourseAssociation association)
		{
			if (await TeacherRepository.Get(association.TeacherId) == null)
				return BadRequest($"The teacher {association.TeacherId} was not found.");

			if (await CourseRepository.Get(association.CourseId) == null)
				return BadRequest($"The course {association.CourseId} was not found.");

			try
			{
				var result = await AssociationRepository.InsertTeacher(association.TeacherId, association.CourseId);
				return Ok(result);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}
		
		/// <summary>
		///     Returns all the students associated with their courses
		/// </summary>
		/// <response code="200">Returns all the students associated with their courses</response>
		/// <response code="400">If there are no students associated with any course</response>
		[HttpGet("student")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Attend>))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetStudents([FromQuery] int page = 0, [FromQuery] int pageSize = 10)
		{
			var result = (List<Attend>) await AssociationRepository.GetAllStudents();

			if (result.Count == 0)
				return BadRequest("No students were associated with any courses.");

			return Ok(result);
		}

		/// <summary>
		///     Associates a student to a course
		/// </summary>
		/// <response code="200">Returns the association between the course and student</response>
		/// <response code="400">If there is an error</response>
		[HttpPost("student")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Attend))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> AddStudentToCourse([FromBody] StudentCourseAssociation association)
		{
			if (await StudentRepository.Get(association.StudentId) == null)
				return BadRequest($"The student {association.StudentId} was not found.");

			if (await CourseRepository.Get(association.CourseId) == null)
				return BadRequest($"The course {association.CourseId} was not found.");

			try
			{
				var result = await AssociationRepository.InsertStudent(association.StudentId, association.CourseId);
				return Ok(result);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		/// <summary>
		///     Removes a teacher from a course
		/// </summary>
		/// <response code="200">If the teacher was removed from the course</response>
		/// <response code="400">If there is an error</response>
		[HttpDelete("teacher")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> RemoveTeacherFromCourse([FromBody] TeacherCourseAssociation association)
		{
			if (await TeacherRepository.Get(association.TeacherId) == null)
				return BadRequest($"The teacher {association.TeacherId} was not found.");

			if (await CourseRepository.Get(association.CourseId) == null)
				return BadRequest($"The course {association.CourseId} was not found.");

			try
			{
				await AssociationRepository.DeleteTeacher(association.TeacherId, association.CourseId);
				return Ok();
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		/// <summary>
		///     Removes a student from a course
		/// </summary>
		/// <response code="200">If the student was remove from the course</response>
		/// <response code="400">If there is an error</response>
		[HttpDelete("student")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> RemoveStudentFromCourse([FromBody] StudentCourseAssociation association)
		{
			if (await StudentRepository.Get(association.StudentId) == null)
				return BadRequest($"The student {association.StudentId} was not found.");

			if (await CourseRepository.Get(association.CourseId) == null)
				return BadRequest($"The course {association.CourseId} was not found.");

			try
			{
				await AssociationRepository.DeleteStudent(association.StudentId, association.CourseId);
				return Ok();
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}
	}
}