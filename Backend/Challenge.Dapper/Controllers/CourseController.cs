using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Challenge.Dapper.Models;
using Challenge.Dapper.Repositories;
using Challenge_EF.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Challenge.Dapper.Controllers
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
		public async Task<IActionResult> Get([FromQuery] int page = 0, [FromQuery] int pageSize = 10)
		{
			List<Course> result;
			if (page == 0)
				result = (List<Course>) await CourseRepository.GetAll();
			else
				result = (List<Course>) await CourseRepository.GetPaged(page, pageSize);

			if (result.Count == 0)
				return BadRequest("No Courses were found.");

			return Ok(result);
		}

		/// <summary>
		///     Returns the number of courses on the database
		/// </summary>
		/// <response code="200">Returns the number of courses on the database</response>
		/// <response code="400">If there are no courses on the database</response>
		[HttpGet("total")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Course>))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetTotal()
		{
			int result = await CourseRepository.Count();

			if (result == 0)
				return BadRequest("No Courses were found.");

			return Ok(result);
		}

		/// <summary>
		///     Returns a single course by their id
		/// </summary>
		/// <response code="200">Returns the course</response>
		/// <response code="400">If there is no course with this id</response>
		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Course))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetFromId([FromRoute] int id)
		{
			var result = await CourseRepository.Get(id);

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
		public async Task<IActionResult> Create([FromBody] NameData course)
		{
			try
			{
				var result = await CourseRepository.Insert(course.Name);
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
		public async Task<IActionResult> Update([FromRoute] int id,
			[FromBody] NameData course)
		{
			var result = await CourseRepository.Get(id);

			if (result == null)
				return BadRequest($"The id {id} was not found.");

			result.Name = course.Name;

			try
			{
				await CourseRepository.Update(result);
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
		public async Task<IActionResult> Delete([FromRoute] int id)
		{
			try
			{
				await CourseRepository.Delete(id);
				return Ok();
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}
	}
}