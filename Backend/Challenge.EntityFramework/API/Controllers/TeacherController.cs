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
	public class TeacherController : ControllerBase
	{
		/// <summary>
		///     Returns all the teachers, can be paged using the page and pageSize query
		/// </summary>
		/// <response code="200">Returns all the teachers</response>
		/// <response code="400">If there is no teacher on the database</response>
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Teacher>))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Get([FromServices] ITeacherRepository repository, [FromQuery] int page = 0,
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
		///     Returns a single teacher by their id
		/// </summary>
		/// <response code="200">Returns the teacher</response>
		/// <response code="400">If there is not teacher with this ID</response>
		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Teacher))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetFromId([FromServices] ITeacherRepository repository, [FromRoute] int id)
		{
			var result = await repository.GetFromId(id);

			if (result == null)
				return BadRequest($"The Course {id} was not found.");

			return Ok(result);
		}

		/// <summary>
		///     Returns all the courses assigned to the teacher
		/// </summary>
		/// <response code="200">Returns all the courses assigned to the teacher</response>
		/// <response code="400">If there is no course associated with the teacher</response>
		[HttpGet("{id}/courses")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Course>))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetCoursesFromId([FromServices] ITeacherRepository repository,
			[FromRoute] int id)
		{
			var result = await repository.GetCoursesFromId(id);

			if (result == null)
				return BadRequest($"There are no courses assigned to this teacher.");

			return Ok(result);
		}

		/// <summary>
		///     Creates a new teacher
		/// </summary>
		/// <response code="200">Returns the created teacher</response>
		/// <response code="400">If there is an error</response>
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Teacher))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Create([FromServices] ITeacherRepository repository,
			[FromBody] NameData teacher)
		{
			try
			{
				var result = await repository.Create(teacher);
				return Ok(result);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		/// <summary>
		///     Updates a teacher
		/// </summary>
		/// <response code="200">Returns the updated teacher</response>
		/// <response code="400">If there is an error</response>
		[HttpPut("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Teacher))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Update([FromServices] ITeacherRepository repository, [FromRoute] int id,
			[FromBody] NameData teacher)
		{
			try
			{
				var result = await repository.Update(id, teacher.Name);
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
		///     Deletes a teacher
		/// </summary>
		/// <response code="200">If the teacher was deleted</response>
		/// <response code="400">If there is an error</response>
		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Delete([FromServices] ITeacherRepository repository, [FromRoute] int id)
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