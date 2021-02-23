using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Challenge_EF.Context;
using Challenge_EF.Data;
using Challenge_EF.Models;
using Challenge_EF.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Challenge_EF.Controllers
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
		///     Returns all the teachers associated with their courses, can be paged using the page and pageSize query
		/// </summary>
		/// <response code="200">Returns all the teachers associated with their courses</response>
		/// <response code="400">If there are no teachers associated with any course</response>
		[HttpGet("teacher")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Teach>))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetTeachers([FromServices] ChallengeDbContext dbContext, [FromQuery] int page = 0, [FromQuery] int pageSize = 10)
		{
			List<Teach> result;
			if (page == 0)
				result = await dbContext.Teaches.ToListAsync();
			else
				result = await dbContext.Teaches.GetPaged(page, pageSize, teach =>  teach.Id);

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
		public async Task<IActionResult> AddTeacherToCourse([FromServices] ChallengeDbContext dbContext,
			[FromBody] TeacherCourseAssociation association)
		{
			if (await dbContext.Teachers.FindAsync(association.TeacherId) == null)
				return BadRequest($"The teacher {association.TeacherId} was not found.");

			if (await dbContext.Courses.FindAsync(association.CourseId) == null)
				return BadRequest($"The course {association.CourseId} was not found.");

			try
			{
				var result = await dbContext.Teaches.AddAsync(new Teach
				{
					TeacherId = association.TeacherId,
					CourseId = association.CourseId
				});

				await dbContext.SaveChangesAsync();

				return Ok(result.Entity);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}
		
		/// <summary>
		///     Returns all the students associated with their courses, can be paged using the page and pageSize query
		/// </summary>
		/// <response code="200">Returns all the students associated with their courses</response>
		/// <response code="400">If there are no students associated with any course</response>
		[HttpGet("student")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Attend>))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetStudents([FromServices] ChallengeDbContext dbContext, [FromQuery] int page = 0, [FromQuery] int pageSize = 10)
		{
			List<Attend> result;
			if (page == 0)
				result = await dbContext.Attends.ToListAsync();
			else
				result = await dbContext.Attends.GetPaged(page, pageSize, attend => attend.Id);

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
		public async Task<IActionResult> AddStudentToCourse([FromServices] ChallengeDbContext dbContext,
			[FromBody] StudentCourseAssociation association)
		{
			if (await dbContext.Students.FindAsync(association.StudentId) == null)
				return BadRequest($"The student {association.StudentId} was not found.");

			if (await dbContext.Courses.FindAsync(association.CourseId) == null)
				return BadRequest($"The course {association.CourseId} was not found.");

			try
			{
				var result = await dbContext.Attends.AddAsync(new Attend
				{
					StudentId = association.StudentId,
					CourseId = association.CourseId
				});

				await dbContext.SaveChangesAsync();

				return Ok(result.Entity);
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
		public async Task<IActionResult> RemoveTeacherFromCourse([FromServices] ChallengeDbContext dbContext,
			[FromBody] TeacherCourseAssociation association)
		{
			if (await dbContext.Teachers.FindAsync(association.TeacherId) == null)
				return BadRequest($"The teacher {association.TeacherId} was not found.");

			if (await dbContext.Courses.FindAsync(association.CourseId) == null)
				return BadRequest($"The course {association.CourseId} was not found.");

			try
			{
				var result = await dbContext.Teaches.FirstAsync(teach =>
					teach.TeacherId == association.TeacherId && teach.CourseId == association.CourseId);
				dbContext.Entry(result).State = EntityState.Deleted;
				await dbContext.SaveChangesAsync();

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
		public async Task<IActionResult> RemoveStudentFromCourse([FromServices] ChallengeDbContext dbContext,
			[FromBody] StudentCourseAssociation association)
		{
			if (await dbContext.Students.FindAsync(association.StudentId) == null)
				return BadRequest($"The student {association.StudentId} was not found.");

			if (await dbContext.Courses.FindAsync(association.CourseId) == null)
				return BadRequest($"The course {association.CourseId} was not found.");

			try
			{
				var result = await dbContext.Attends.FirstAsync(attend =>
					attend.StudentId == association.StudentId && attend.CourseId == association.CourseId);
				dbContext.Entry(result).State = EntityState.Deleted;
				await dbContext.SaveChangesAsync();

				return Ok();
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}
	}
}