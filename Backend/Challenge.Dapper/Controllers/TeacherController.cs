using System;
using System.Collections.Generic;
using System.Linq;
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
		public async Task<IActionResult> Get([FromQuery] int page = 0, [FromQuery] int pageSize = 10)
		{
			List<Teacher> result;
			if (page == 0)
				result = (List<Teacher>) await TeacherRepository.GetAll();
			else
				result = (List<Teacher>) await TeacherRepository.GetPaged(page, pageSize);

			if (result.Count == 0)
				return BadRequest("No teachers were found.");

			return Ok(result);
		}

		/// <summary>
		///     Returns the number of teachers on the database
		/// </summary>
		/// <response code="200">Returns the number of teachers on the database</response>
		/// <response code="400">If there is no teacher on the database</response>
		[HttpGet("total")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetTotal()
		{
			int result = await TeacherRepository.Count();

			if (result == 0)
				return BadRequest("No teachers were found.");

			return Ok(result);
		}

		/// <summary>
		///     Returns a single teacher by their id
		/// </summary>
		/// <response code="200">Returns the teacher</response>
		/// <response code="400">If there is not teacher with this ID</response>
		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Teacher))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetFromId([FromRoute] int id)
		{
			var result = await TeacherRepository.Get(id);

			if (result == null)
				return BadRequest($"The teacher {id} was not found.");

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
		public async Task<IActionResult> GetCoursesFromId([FromRoute] int id)
		{
			var result = await TeacherRepository.Courses(id);

			if (result == null || !result.Any())
				return BadRequest($"The teacher {id} was not found.");

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
		public async Task<IActionResult> Create([FromBody] NameData teacher)
		{
			try
			{
				var result = await TeacherRepository.Insert(teacher.Name);

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
		public async Task<IActionResult> Update([FromRoute] int id,
			[FromBody] NameData teacher)
		{
			var result = await TeacherRepository.Get(id);

			if (result == null)
				return BadRequest($"The id {id} was not found.");

			result.Name = teacher.Name;

			var response = await TeacherRepository.Update(result);
			if (response != null)
				return Ok(result);
			return BadRequest("The teacher was not updated");
		}

		/// <summary>
		///     Deletes a teacher
		/// </summary>
		/// <response code="200">If the teacher was deleted</response>
		/// <response code="400">If there is an error</response>
		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Delete([FromRoute] int id)
		{
			var response = await TeacherRepository.Delete(id);

			if (response != null)
				return Ok();
			return BadRequest("The teacher was not deleted");
		}
	}
}