using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Dto;
using Core.Models;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
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
		public async Task<IActionResult> GetTeachers([FromServices] IAssociationRepository repository,
			[FromQuery] int page = 0, [FromQuery] int pageSize = 0)
		{
			if (page == 0 || pageSize == 0)
			{
				var result = await repository.GetAllTeaches();
				if (result?.Count == 0)
					return BadRequest("No students were associated");
				
				return Ok(result);
			}

			var pagedResult = await repository.GetTeachesPaged(page, pageSize);
			if (pagedResult.TotalItems == 0)
				return BadRequest("No students were associated");
				
			return Ok(pagedResult);
		}

		/// <summary>
		///     Associates a teacher to a course
		/// </summary>
		/// <response code="200">Returns the association between the course and teacher</response>
		/// <response code="400">If there is an error</response>
		[HttpPost("teacher")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Teach))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> AddTeacherToCourse([FromServices] IAssociationRepository repository,
			[FromBody] TeacherCourseAssociation association)
		{
			try
			{
				var result = await repository.AddTeacherToCourse(association);
				return Ok(result);
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
		public async Task<IActionResult> GetStudents([FromServices] IAssociationRepository repository,
			[FromQuery] int page = 0, [FromQuery] int pageSize = 0)
		{
			if (page == 0 || pageSize == 0)
			{
				var result = await repository.GetAllAttends();
				if (result?.Count == 0)
					return BadRequest("No students were associated");
				
				return Ok(result);
			}

			var pagedResult = await repository.GetAttendsPaged(page, pageSize);
			if (pagedResult.TotalItems == 0)
				return BadRequest("No students were associated");
				
			return Ok(pagedResult);
		}

		/// <summary>
		///     Associates a student to a course
		/// </summary>
		/// <response code="200">Returns the association between the course and student</response>
		/// <response code="400">If there is an error</response>
		[HttpPost("student")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Attend))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> AddStudentToCourse([FromServices] IAssociationRepository repository,
			[FromBody] StudentCourseAssociation association)
		{
			try
			{
				var result = await repository.AddStudentToCourse(association);
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
		public async Task<IActionResult> RemoveTeacherFromCourse([FromServices]IAssociationRepository repository,
			[FromBody] TeacherCourseAssociation association)
		{
			try
			{
				await repository.RemoveTeacherFromCourse(association);
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
		public async Task<IActionResult> RemoveStudentFromCourse([FromServices] IAssociationRepository repository,
			[FromBody] StudentCourseAssociation association)
		{
			try
			{
				await repository.RemoveStudentFromCourse(association);
				return Ok();
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}
	}
}