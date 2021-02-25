using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Repositories;
using Core.Dto;
using Core.Models;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	[Authorize]
	[ApiController]
	[Route("[controller]")]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[Produces("application/json")]
	public class StudentController : ControllerBase
	{
		/// <summary>
		///     Returns all the students, can be paged using the page and pageSize query
		/// </summary>
		/// <response code="200">Returns all students</response>
		/// <response code="400">If there is no students in the database</response>
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Student>))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Get([FromServices] IStudentRepository repository, [FromQuery] int page = 0,
			[FromQuery] int pageSize = 0)
		{
			if (page <= 0 || pageSize <= 0)
			{
				var result = await repository.GetAll();
				if (result?.Count == 0)
					return BadRequest("No students were found");
				
				return Ok(result);
			}

			var pagedResult = await repository.GetPaged(page, pageSize);
			if (pagedResult.TotalItems == 0)
				return BadRequest("No students were found");
				
			return Ok(pagedResult);
		}
		
		/// <summary>
		///     Returns a single student by their id
		/// </summary>
		/// <response code="200">Returns the student</response>
		/// <response code="400">If there is no student with this ID</response>
		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Student))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetFromId([FromServices] IStudentRepository repository, [FromRoute] int id)
		{
			var result = await repository.GetFromId(id);

			if (result == null)
				return BadRequest($"The Student {id} was not found.");

			return Ok(result);
		}

		/// <summary>
		///     Returns all the courses assigned to the student
		/// </summary>
		/// <response code="200">Returns all the courses assigned to the student</response>
		/// <response code="400">If there is no course associated with the student</response>
		[HttpGet("{id}/courses")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Course>))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetCoursesFromId([FromServices] IStudentRepository repository,
			[FromRoute] int id)
		{
			var result = await repository.GetCoursesFromId(id);

			if (result == null)
				return BadRequest($"There are no courses assigned to this teacher.");

			return Ok(result);
		}

		/// <summary>
		///     Creates a new student
		/// </summary>
		/// <response code="200">Returns the created student</response>
		/// <response code="400">If there is an error</response>
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Student))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Create([FromServices] IStudentRepository repository,
			[FromBody] NameData student)
		{
			try
			{
				var result = await repository.Create(student);
				return Ok(result);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		/// <summary>
		///     Updates a student
		/// </summary>
		/// <response code="200">Returns the updated student</response>
		/// <response code="400">If there is an error</response>
		[HttpPut("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Student))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Update([FromServices] IStudentRepository repository, [FromRoute] int id,
			[FromBody] NameData student)
		{
			try
			{
				var result = await repository.Update(id, student.Name);
				if (result == null)
					return BadRequest($"The id {id} was not found.");
				
				return Ok(result);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		/// <summary>
		///     Deletes a student
		/// </summary>
		/// <response code="200">If the student was deleted</response>
		/// <response code="400">If there is an error</response>
		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Delete([FromServices] IStudentRepository repository, [FromRoute] int id)
		{
			try
			{
				await repository.Delete(id);
				return Ok();
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}
	}
}