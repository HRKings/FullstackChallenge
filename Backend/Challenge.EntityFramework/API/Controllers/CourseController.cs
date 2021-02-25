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
	public class CourseController : ControllerBase
	{
		/// <summary>
		///     Returns all the courses, can be paged using the page and pageSize query
		/// </summary>
		/// <response code="200">Returns all the courses</response>
		/// <response code="400">If there are no courses on the database</response>
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Course>))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Get([FromServices] ICourseRepository repository, [FromQuery] int page = 0,
			[FromQuery] int pageSize = 0)
		{
			if (page <= 0 || pageSize <= 0)
			{
				var result = await repository.GetAll();
				if (result?.Count == 0)
					return BadRequest("No courses were found");
				
				return Ok(result);
			}

			var pagedResult = await repository.GetPaged(page, pageSize);
			if (pagedResult.TotalItems == 0)
				return BadRequest("No courses were found");
				
			return Ok(pagedResult);
		}

		/// <summary>
		///     Returns a single course by their id
		/// </summary>
		/// <response code="200">Returns the course</response>
		/// <response code="400">If there is no course with this id</response>
		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Course))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetFromId([FromServices] ICourseRepository repository, [FromRoute] int id)
		{
			var result = await repository.GetFromId(id);

			if (result == null)
				return BadRequest($"The Course {id} was not found.");

			return Ok(result);
		}

		/// <summary>
		///     Creates a new course
		/// </summary>
		/// <response code="200">Returns the newly created course</response>
		/// <response code="400">If there is some error</response>
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Course))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Create([FromServices] ICourseRepository repository, [FromBody] NameData course)
		{
			try
			{
				var result = await repository.Create(course);
				return Ok(result);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		/// <summary>
		///     Updates a course
		/// </summary>
		/// <response code="200">Returns the updated course</response>
		/// <response code="400">If there is an error</response>
		[HttpPut("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Course))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Update([FromServices] ICourseRepository repository, [FromRoute] int id,
			[FromBody] NameData course)
		{
			try
			{
				var result = await repository.Update(id, course.Name);
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
		///     Deletes a course
		/// </summary>
		/// <response code="200">If the course was delete</response>
		/// <response code="400">If there is an error</response>
		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Delete([FromServices] ICourseRepository repository, [FromRoute] int id)
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