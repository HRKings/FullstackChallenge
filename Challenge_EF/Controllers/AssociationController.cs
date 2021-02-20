using System;
using System.Linq;
using System.Threading.Tasks;
using Challenge_EF.Context;
using Challenge_EF.Data;
using Challenge_EF.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Challenge_EF.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class AssociationController : ControllerBase
	{
		private readonly ILogger<AssociationController> _logger;

		public AssociationController(ILogger<AssociationController> logger)
		{
			_logger = logger;
		}
		
		/// <summary>
		/// Associates a teacher to a course
		/// </summary>
		[HttpPost("teacher")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Teach))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> AddTeacherToCourse([FromServices] ChallengeDbContext dbContext, [FromBody] TeacherCourseAssociation association)
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
		/// Associates a teacher to a course
		/// </summary>
		[HttpPost("student")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Attend))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> AddStudentToCourse([FromServices] ChallengeDbContext dbContext, [FromBody] StudentCourseAssociation association)
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
		/// Removes a teacher from a course
		/// </summary>
		[HttpDelete("teacher")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> RemoveTeacherFromCourse([FromServices] ChallengeDbContext dbContext, [FromBody] TeacherCourseAssociation association)
		{
			if (await dbContext.Teachers.FindAsync(association.TeacherId) == null)
				return BadRequest($"The teacher {association.TeacherId} was not found.");
			
			if (await dbContext.Courses.FindAsync(association.CourseId) == null)
				return BadRequest($"The course {association.CourseId} was not found.");
				
			try
			{
				var result = await dbContext.Teaches.FirstAsync(teach => teach.TeacherId == association.TeacherId && teach.CourseId == association.CourseId);
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
		/// Removes a student from a course
		/// </summary>
		[HttpDelete("student")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> RemoveStudentFromCourse([FromServices] ChallengeDbContext dbContext, [FromBody] StudentCourseAssociation association)
		{
			if (await dbContext.Teachers.FindAsync(association.StudentId) == null)
				return BadRequest($"The student {association.StudentId} was not found.");
			
			if (await dbContext.Courses.FindAsync(association.CourseId) == null)
				return BadRequest($"The course {association.CourseId} was not found.");
				
			try
			{
				var result = await dbContext.Attends.FirstAsync(attend => attend.StudentId == association.StudentId && attend.CourseId == association.CourseId);
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